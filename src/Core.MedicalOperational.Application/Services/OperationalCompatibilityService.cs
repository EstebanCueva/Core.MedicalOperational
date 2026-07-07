using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Services;

public class OperationalCompatibilityService : IOperationalCompatibilityService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly IDoctorSpecialtyRepository _doctorSpecialtyRepository;
    private readonly IMedicalProcedureRepository _medicalProcedureRepository;
    private readonly IMedicalRoomRepository _medicalRoomRepository;
    private readonly IMedicalEquipmentRepository _medicalEquipmentRepository;
    private readonly IRoomEquipmentRepository _roomEquipmentRepository;
    private readonly IProcedureRequiredEquipmentRepository _procedureRequiredEquipmentRepository;
    private readonly IMedicalSupplyRepository _medicalSupplyRepository;
    private readonly ISupplyStockRepository _supplyStockRepository;
    private readonly IProcedureRequiredSupplyRepository _procedureRequiredSupplyRepository;
    private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
    private readonly IExistingAssignmentRepository _existingAssignmentRepository;
    private readonly IOperationalCompatibilityResultRepository _operationalCompatibilityResultRepository;

    public OperationalCompatibilityService(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        ISpecialtyRepository specialtyRepository,
        IDoctorSpecialtyRepository doctorSpecialtyRepository,
        IMedicalProcedureRepository medicalProcedureRepository,
        IMedicalRoomRepository medicalRoomRepository,
        IMedicalEquipmentRepository medicalEquipmentRepository,
        IRoomEquipmentRepository roomEquipmentRepository,
        IProcedureRequiredEquipmentRepository procedureRequiredEquipmentRepository,
        IMedicalSupplyRepository medicalSupplyRepository,
        ISupplyStockRepository supplyStockRepository,
        IProcedureRequiredSupplyRepository procedureRequiredSupplyRepository,
        IMedicalAppointmentRepository medicalAppointmentRepository,
        IExistingAssignmentRepository existingAssignmentRepository,
        IOperationalCompatibilityResultRepository operationalCompatibilityResultRepository)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _specialtyRepository = specialtyRepository;
        _doctorSpecialtyRepository = doctorSpecialtyRepository;
        _medicalProcedureRepository = medicalProcedureRepository;
        _medicalRoomRepository = medicalRoomRepository;
        _medicalEquipmentRepository = medicalEquipmentRepository;
        _roomEquipmentRepository = roomEquipmentRepository;
        _procedureRequiredEquipmentRepository = procedureRequiredEquipmentRepository;
        _medicalSupplyRepository = medicalSupplyRepository;
        _supplyStockRepository = supplyStockRepository;
        _procedureRequiredSupplyRepository = procedureRequiredSupplyRepository;
        _medicalAppointmentRepository = medicalAppointmentRepository;
        _existingAssignmentRepository = existingAssignmentRepository;
        _operationalCompatibilityResultRepository = operationalCompatibilityResultRepository;
    }

    public async Task<OperationalCompatibilityResponse> AnalyzeAsync(
        OperationalCompatibilityRequest request,
        CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);
        var doctors = await _doctorRepository.GetAllAsync(cancellationToken);
        var specialties = await _specialtyRepository.GetAllAsync(cancellationToken);
        var doctorSpecialties = await _doctorSpecialtyRepository.GetAllAsync(cancellationToken);
        var procedures = await _medicalProcedureRepository.GetAllAsync(cancellationToken);
        var rooms = await _medicalRoomRepository.GetAllAsync(cancellationToken);
        var equipments = await _medicalEquipmentRepository.GetAllAsync(cancellationToken);
        var roomEquipments = await _roomEquipmentRepository.GetAllAsync(cancellationToken);
        var requiredEquipments = await _procedureRequiredEquipmentRepository.GetAllAsync(cancellationToken);
        var supplies = await _medicalSupplyRepository.GetAllAsync(cancellationToken);
        var stocks = await _supplyStockRepository.GetAllAsync(cancellationToken);
        var requiredSupplies = await _procedureRequiredSupplyRepository.GetAllAsync(cancellationToken);
        var appointments = await _medicalAppointmentRepository.GetAllAsync(cancellationToken);
        var existingAssignments = await _existingAssignmentRepository.GetAllAsync(cancellationToken);

        var response = new OperationalCompatibilityResponse
        {
            AnalysisDate = request.AnalysisDate,
            TotalDoctorsAnalyzed = CountActiveDoctors(doctors),
            TotalRoomsAnalyzed = rooms.Count
        };

        var totalAppointmentsAnalyzed = 0;
        var totalCompatibleOptions = 0;
        var totalRejectedOptions = 0;

        // FOREACH #1: recorre citas médicas pendientes o programadas
        foreach (var appointment in appointments)
        {
            if (appointment.Status == AppointmentStatus.Cancelled ||
                appointment.Status == AppointmentStatus.Completed)
            {
                continue;
            }

            if (appointment.StartDate.Date != request.AnalysisDate.Date)
            {
                continue;
            }

            Patient? selectedPatient = null;

            // FOREACH #2: busca paciente relacionado con la cita
            foreach (var patient in patients)
            {
                if (patient.Id == appointment.PatientId && patient.IsActive)
                {
                    selectedPatient = patient;
                    break;
                }
            }

            if (selectedPatient is null)
            {
                continue;
            }

            MedicalProcedure? selectedProcedure = null;

            // FOREACH #3: busca procedimiento relacionado con la cita
            foreach (var procedure in procedures)
            {
                if (procedure.Id == appointment.ProcedureId && procedure.IsActive)
                {
                    selectedProcedure = procedure;
                    break;
                }
            }

            if (selectedProcedure is null)
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(request.ProcedureCode) &&
                selectedProcedure.ProcedureCode != request.ProcedureCode)
            {
                continue;
            }

            if (!MatchesSpecialtyFilter(selectedProcedure, specialties, request.SpecialtyCode))
            {
                continue;
            }

            totalAppointmentsAnalyzed++;

            // FOREACH #4: recorre médicos candidatos
            foreach (var doctor in doctors)
            {
                if (!doctor.IsActive)
                {
                    continue;
                }

                var doctorHasRequiredSpecialty = false;

                // FOREACH #5: recorre especialidades del médico
                foreach (var doctorSpecialty in doctorSpecialties)
                {
                    if (doctorSpecialty.DoctorId == doctor.Id &&
                        doctorSpecialty.SpecialtyId == selectedProcedure.RequiredSpecialtyId)
                    {
                        doctorHasRequiredSpecialty = true;
                        break;
                    }
                }

                // FOREACH #6: recorre salas médicas candidatas
                foreach (var room in rooms)
                {
                    var missingEquipment = new List<string>();
                    var missingSupplies = new List<string>();
                    var conflicts = new List<string>();

                    if (!doctorHasRequiredSpecialty)
                    {
                        AddUnique(
                            conflicts,
                            "Doctor does not have the required specialty for the procedure.");
                    }

                    if (room.Status != RoomStatus.Available)
                    {
                        AddUnique(
                            conflicts,
                            $"Room is not available. Current status: {room.Status}.");
                    }

                    // FOREACH #7: recorre equipos requeridos por el procedimiento
                    foreach (var requiredEquipment in requiredEquipments)
                    {
                        if (requiredEquipment.ProcedureId != selectedProcedure.Id)
                        {
                            continue;
                        }

                        var hasRequiredEquipmentInRoom = false;

                        // FOREACH #8: recorre equipos disponibles en la sala
                        foreach (var roomEquipment in roomEquipments)
                        {
                            if (roomEquipment.RoomId == room.Id &&
                                roomEquipment.EquipmentId == requiredEquipment.EquipmentId &&
                                roomEquipment.IsAvailable &&
                                roomEquipment.Quantity >= requiredEquipment.QuantityRequired)
                            {
                                var equipmentIsOperational = false;

                                foreach (var equipment in equipments)
                                {
                                    if (equipment.Id == roomEquipment.EquipmentId &&
                                        equipment.Status == EquipmentStatus.Available)
                                    {
                                        equipmentIsOperational = true;
                                        break;
                                    }
                                }

                                if (equipmentIsOperational)
                                {
                                    hasRequiredEquipmentInRoom = true;
                                    break;
                                }
                            }
                        }

                        if (!hasRequiredEquipmentInRoom)
                        {
                            AddUnique(
                                missingEquipment,
                                GetEquipmentName(requiredEquipment.EquipmentId, equipments));
                        }
                    }

                    // FOREACH #9: recorre insumos requeridos por el procedimiento
                    foreach (var requiredSupply in requiredSupplies)
                    {
                        if (requiredSupply.ProcedureId != selectedProcedure.Id)
                        {
                            continue;
                        }

                        var hasEnoughStock = false;

                        // FOREACH #10: recorre stock disponible de insumos
                        foreach (var stock in stocks)
                        {
                            if (stock.SupplyId == requiredSupply.SupplyId &&
                                stock.AvailableQuantity >= requiredSupply.QuantityRequired)
                            {
                                hasEnoughStock = true;
                                break;
                            }
                        }

                        if (!hasEnoughStock)
                        {
                            AddUnique(
                                missingSupplies,
                                GetSupplyName(requiredSupply.SupplyId, supplies));
                        }
                    }

                    // FOREACH #11: recorre asignaciones existentes para detectar conflictos
                    foreach (var existingAssignment in existingAssignments)
                    {
                        if (existingAssignment.Status != AssignmentStatus.Active)
                        {
                            continue;
                        }

                        // Mejora importante:
                        if (existingAssignment.AppointmentId == appointment.Id)
                        {
                            continue;
                        }

                        var hasTimeConflict = HasDateOverlap(
                            appointment.StartDate,
                            appointment.EndDate,
                            existingAssignment.StartDate,
                            existingAssignment.EndDate);

                        if (!hasTimeConflict)
                        {
                            continue;
                        }

                        if (existingAssignment.DoctorId == doctor.Id)
                        {
                            AddUnique(
                                conflicts,
                                "Doctor has another assignment during the requested time.");
                        }

                        if (existingAssignment.RoomId == room.Id)
                        {
                            AddUnique(
                                conflicts,
                                "Room has another assignment during the requested time.");
                        }
                    }

                    var isCompatible =
                        missingEquipment.Count == 0 &&
                        missingSupplies.Count == 0 &&
                        conflicts.Count == 0;

                    // Primero contamos compatibles/rechazados.
                    // Así, aunque IncludeOnlyCompatible sea true, los rechazados sí se contabilizan.
                    if (isCompatible)
                    {
                        totalCompatibleOptions++;
                    }
                    else
                    {
                        totalRejectedOptions++;
                    }


                    //Ver CUADNO UNA CITA PUEDE ESTAR DISPONBLE Y QUE ME DE LA FECHA de la cita o algun parametro para mostrar de cuando puede estar disponible,

                    DateTime? nextAvailableDate = null;

                    if (!isCompatible)
                    {
                        nextAvailableDate = CalculateNextAvailableDate(
                            appointment,
                            doctor,
                            room,
                            existingAssignments,
                            missingEquipment,
                            missingSupplies,
                            conflicts);
                    }


                    var resultResponse = new OperationalCompatibilityMatchResponse
                    {
                        AppointmentId = appointment.Id,
                        PatientId = selectedPatient.Id,
                        ProcedureId = selectedProcedure.Id,
                        DoctorId = doctor.Id,
                        RoomId = room.Id,
                        PatientName = selectedPatient.FullName,
                        ProcedureCode = selectedProcedure.ProcedureCode,
                        ProcedureName = selectedProcedure.Name,
                        DoctorCode = doctor.DoctorCode,
                        DoctorName = doctor.FullName,
                        RoomCode = room.RoomCode,
                        RoomName = room.Name,
                        IsCompatible = isCompatible,
                        MissingEquipment = missingEquipment,
                        MissingSupplies = missingSupplies,
                        Conflicts = conflicts,
                        Status = isCompatible
                            ? CompatibilityStatus.Compatible.ToString()
                            : CompatibilityStatus.NotCompatible.ToString(),
                        Explanation = isCompatible
                            ? "The appointment can be assigned to this doctor and room."
                            : "The appointment cannot be assigned because one or more operational conditions are not met.",
                        NextAvailableDate = nextAvailableDate.HasValue
                            ? nextAvailableDate.Value.ToString("yyyy-MM-ddTHH:mm:ss")
                            : null
                    };



                    response.Results.Add(resultResponse);

                    if (request.SaveResults)
                    {
                        var resultEntity = CreateResultEntity(
                            request.AnalysisDate,
                            appointment,
                            selectedPatient,
                            selectedProcedure,
                            doctor,
                            room,
                            resultResponse);

                        await _operationalCompatibilityResultRepository.AddAsync(
                            resultEntity,
                            cancellationToken);
                    }
                }
            }
        }

        response.TotalAppointmentsAnalyzed = totalAppointmentsAnalyzed;
        response.TotalCompatibleOptions = totalCompatibleOptions;
        response.TotalRejectedOptions = totalRejectedOptions;

        response.GeneralObservations.Add("Operational compatibility analysis completed.");
        response.GeneralObservations.Add($"Only appointments from {request.AnalysisDate:yyyy-MM-dd} were analyzed.");
        response.GeneralObservations.Add("The service validates patients, doctors, specialties, rooms, equipment, supplies, stock and schedule conflicts.");
        response.GeneralObservations.Add("The main business logic is implemented with visible nested foreach loops.");

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

    private static bool HasDateOverlap(
        DateTime firstStart,
        DateTime firstEnd,
        DateTime secondStart,
        DateTime secondEnd)
    {
        return firstStart < secondEnd && secondStart < firstEnd;
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
    private static DateTime? CalculateNextAvailableDate(
    MedicalAppointment appointment,
    Doctor doctor,
    MedicalRoom room,
    IReadOnlyCollection<ExistingAssignment> existingAssignments,
    IReadOnlyCollection<string> missingEquipment,
    IReadOnlyCollection<string> missingSupplies,
    IReadOnlyCollection<string> conflicts)
    {
        if (missingEquipment.Count > 0 || missingSupplies.Count > 0)
        {
            return null;
        }

        var hasScheduleConflict = false;
        var latestConflictEndDate = appointment.EndDate;

        foreach (var conflict in conflicts)
        {
            if (conflict == "Doctor has another assignment during the requested time." ||
                conflict == "Room has another assignment during the requested time.")
            {
                hasScheduleConflict = true;
                break;
            }
        }

        if (!hasScheduleConflict)
        {
            return null;
        }

        foreach (var existingAssignment in existingAssignments)
        {
            if (existingAssignment.Status != AssignmentStatus.Active)
            {
                continue;
            }

            if (existingAssignment.AppointmentId == appointment.Id)
            {
                continue;
            }

            var isSameDoctor = existingAssignment.DoctorId == doctor.Id;
            var isSameRoom = existingAssignment.RoomId == room.Id;

            if (!isSameDoctor && !isSameRoom)
            {
                continue;
            }

            var hasTimeConflict = HasDateOverlap(
                appointment.StartDate,
                appointment.EndDate,
                existingAssignment.StartDate,
                existingAssignment.EndDate);

            if (!hasTimeConflict)
            {
                continue;
            }

            if (existingAssignment.EndDate > latestConflictEndDate)
            {
                latestConflictEndDate = existingAssignment.EndDate;
            }
        }

        return latestConflictEndDate.AddMinutes(15);
    }
    private static string GetEquipmentName(
        int equipmentId,
        IReadOnlyCollection<MedicalEquipment> equipments)
    {
        foreach (var equipment in equipments)
        {
            if (equipment.Id == equipmentId)
            {
                return equipment.Name;
            }
        }

        return $"Equipment Id {equipmentId}";
    }

    private static string GetSupplyName(
        int supplyId,
        IReadOnlyCollection<MedicalSupply> supplies)
    {
        foreach (var supply in supplies)
        {
            if (supply.Id == supplyId)
            {
                return supply.Name;
            }
        }

        return $"Supply Id {supplyId}";
    }

    private static void AddUnique(List<string> values, string value)
    {
        foreach (var item in values)
        {
            if (item == value)
            {
                return;
            }
        }

        values.Add(value);
    }

    private static OperationalCompatibilityResult CreateResultEntity(
        DateTime analysisDate,
        MedicalAppointment appointment,
        Patient patient,
        MedicalProcedure procedure,
        Doctor doctor,
        MedicalRoom room,
        OperationalCompatibilityMatchResponse response)
    {
        return new OperationalCompatibilityResult
        {
            AnalysisDate = analysisDate,
            AppointmentId = appointment.Id,
            PatientId = patient.Id,
            ProcedureId = procedure.Id,
            DoctorId = doctor.Id,
            RoomId = room.Id,
            IsCompatible = response.IsCompatible,
            MissingEquipment = string.Join(", ", response.MissingEquipment),
            MissingSupplies = string.Join(", ", response.MissingSupplies),
            Conflicts = string.Join(", ", response.Conflicts),
            Status = response.IsCompatible
                ? CompatibilityStatus.Compatible
                : CompatibilityStatus.NotCompatible,
            Severity = response.IsCompatible
                ? CompatibilitySeverity.Information
                : CompatibilitySeverity.Critical,
            Explanation = response.Explanation
        };
    }
}