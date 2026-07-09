using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Services;

public class OperationalCompatibilityService : IOperationalCompatibilityService
{
    private readonly IOperationalCompatibilityContextService _contextService;
    private readonly IEnumerable<IOperationalCompatibilityRule> _rules;
    private readonly IOperationalCompatibilityPersistenceService _persistenceService;
    private readonly IOperationalCompatibilityNextAvailableDateService _nextAvailableDateService;

    public OperationalCompatibilityService(
        IOperationalCompatibilityContextService contextService,
        IEnumerable<IOperationalCompatibilityRule> rules,
        IOperationalCompatibilityPersistenceService persistenceService,
        IOperationalCompatibilityNextAvailableDateService nextAvailableDateService)
    {
        _contextService = contextService;
        _rules = rules;
        _persistenceService = persistenceService;
        _nextAvailableDateService = nextAvailableDateService;
    }

    public async Task<OperationalCompatibilityResponse> AnalyzeAsync(
        OperationalCompatibilityRequest request,
        CancellationToken cancellationToken = default)
    {
        var context = await _contextService.BuildAsync(cancellationToken);

        var response = new OperationalCompatibilityResponse
        {
            AnalysisDate = request.AnalysisDate,
            TotalDoctorsAnalyzed = CountActiveDoctors(context.Doctors),
            TotalRoomsAnalyzed = context.Rooms.Count
        };

        var totalAppointmentsAnalyzed = 0;
        var totalCompatibleOptions = 0;
        var totalRejectedOptions = 0;

        foreach (var appointment in context.Appointments)
        {
            if (appointment.Status is AppointmentStatus.Cancelled or AppointmentStatus.Completed)
            {
                continue;
            }

            if (appointment.StartDate.Date != request.AnalysisDate.Date)
            {
                continue;
            }

            var patient = GetActivePatient(context.Patients, appointment.PatientId);
            var procedure = GetActiveProcedure(context.Procedures, appointment.ProcedureId);

            if (patient is null || procedure is null)
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(request.ProcedureCode) &&
                procedure.ProcedureCode != request.ProcedureCode)
            {
                continue;
            }

            if (!MatchesSpecialtyFilter(procedure, context.Specialties, request.SpecialtyCode))
            {
                continue;
            }

            totalAppointmentsAnalyzed++;

            foreach (var doctor in context.Doctors)
            {
                if (!doctor.IsActive)
                {
                    continue;
                }

                foreach (var room in context.Rooms)
                {
                    var candidate = new OperationalCompatibilityCandidate
                    {
                        Context = context,
                        Appointment = appointment,
                        Patient = patient,
                        Procedure = procedure,
                        Doctor = doctor,
                        Room = room
                    };

                    var evaluation = new OperationalCompatibilityEvaluation();

                    foreach (var rule in _rules)
                    {
                        await rule.ApplyAsync(candidate, evaluation, cancellationToken);
                    }

                    if (evaluation.IsCompatible)
                    {
                        totalCompatibleOptions++;
                    }
                    else
                    {
                        totalRejectedOptions++;
                    }

                    var matchResponse = new OperationalCompatibilityMatchResponse
                    {
                        AppointmentId = appointment.Id,
                        PatientName = patient.FullName,
                        ProcedureCode = procedure.ProcedureCode,
                        ProcedureName = procedure.Name,
                        DoctorCode = doctor.DoctorCode,
                        DoctorName = doctor.FullName,
                        RoomCode = room.RoomCode,
                        RoomName = room.Name,
                        IsCompatible = evaluation.IsCompatible,
                        MissingEquipment = evaluation.MissingEquipment,
                        MissingSupplies = evaluation.MissingSupplies,
                        Conflicts = evaluation.Conflicts,
                        Status = evaluation.IsCompatible
                            ? CompatibilityStatus.Compatible.ToString()
                            : CompatibilityStatus.NotCompatible.ToString(),
                        Explanation = evaluation.IsCompatible
                            ? "The appointment can be assigned to this doctor and room."
                            : "The appointment cannot be assigned because one or more operational conditions are not met.",
                        NextAvailableDate = _nextAvailableDateService.Calculate(
                                appointment,
                                doctor,
                                room,
                                context.ExistingAssignments,
                                evaluation.MissingEquipment,
                                evaluation.MissingSupplies,
                                evaluation.Conflicts)
                            ?.ToString("yyyy-MM-ddTHH:mm:ss")
                    };

                    if (!request.IncludeOnlyCompatible || matchResponse.IsCompatible)
                    {
                        response.Results.Add(matchResponse);
                    }

                    if (request.SaveResults)
                    {
                        await _persistenceService.SaveAsync(
                            request.AnalysisDate,
                            appointment,
                            patient,
                            procedure,
                            doctor,
                            room,
                            matchResponse,
                            cancellationToken);
                    }
                }
            }
        }

        if (request.SaveResults)
        {
            await _persistenceService.FlushAsync(cancellationToken);
        }

        response.TotalAppointmentsAnalyzed = totalAppointmentsAnalyzed;
        response.TotalCompatibleOptions = totalCompatibleOptions;
        response.TotalRejectedOptions = totalRejectedOptions;
        response.GeneralObservations.Add("Operational compatibility analysis completed.");
        response.GeneralObservations.Add($"Only appointments from {request.AnalysisDate:yyyy-MM-dd} were analyzed.");
        response.GeneralObservations.Add("The service validates patients, doctors, specialties, rooms, equipment, supplies, stock and schedule conflicts.");
        response.GeneralObservations.Add("Compatibility rules are evaluated through independent strategy services.");

        if (request.IncludeOnlyCompatible)
        {
            response.GeneralObservations.Add("Only compatible options were included in the results, but rejected options were still counted.");
        }

        if (!string.IsNullOrWhiteSpace(request.ProcedureCode))
        {
            response.GeneralObservations.Add($"Procedure filter applied: {request.ProcedureCode}.");
        }

        if (!string.IsNullOrWhiteSpace(request.SpecialtyCode))
        {
            response.GeneralObservations.Add($"Specialty filter applied: {request.SpecialtyCode}.");
        }

        return response;
    }

    private static Patient? GetActivePatient(IReadOnlyCollection<Patient> patients, int patientId)
    {
        foreach (var patient in patients)
        {
            if (patient.Id == patientId && patient.IsActive)
            {
                return patient;
            }
        }

        return null;
    }

    private static MedicalProcedure? GetActiveProcedure(IReadOnlyCollection<MedicalProcedure> procedures, int procedureId)
    {
        foreach (var procedure in procedures)
        {
            if (procedure.Id == procedureId && procedure.IsActive)
            {
                return procedure;
            }
        }

        return null;
    }

    private static bool MatchesSpecialtyFilter(
        MedicalProcedure procedure,
        IReadOnlyCollection<Specialty> specialties,
        string? specialtyCode)
    {
        if (string.IsNullOrWhiteSpace(specialtyCode))
        {
            return true;
        }

        foreach (var specialty in specialties)
        {
            if (specialty.Id == procedure.RequiredSpecialtyId &&
                specialty.SpecialtyCode == specialtyCode)
            {
                return true;
            }
        }

        return false;
    }

    private static int CountActiveDoctors(IReadOnlyCollection<Doctor> doctors)
    {
        var total = 0;

        foreach (var doctor in doctors)
        {
            if (doctor.IsActive)
            {
                total++;
            }
        }

        return total;
    }
}
