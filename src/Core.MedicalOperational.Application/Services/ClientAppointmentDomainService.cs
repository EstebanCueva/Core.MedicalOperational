using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class ClientAppointmentDomainService : IClientAppointmentDomainService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMedicalProcedureRepository _medicalProcedureRepository;
    private readonly IMedicalRoomRepository _medicalRoomRepository;
    private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
    private readonly IExistingAssignmentRepository _existingAssignmentRepository;

    public ClientAppointmentDomainService(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IMedicalProcedureRepository medicalProcedureRepository,
        IMedicalRoomRepository medicalRoomRepository,
        IMedicalAppointmentRepository medicalAppointmentRepository,
        IExistingAssignmentRepository existingAssignmentRepository)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _medicalProcedureRepository = medicalProcedureRepository;
        _medicalRoomRepository = medicalRoomRepository;
        _medicalAppointmentRepository = medicalAppointmentRepository;
        _existingAssignmentRepository = existingAssignmentRepository;
    }

    public async Task<ClientAppointmentContext> BuildContextAsync(
        int patientId,
        int doctorId,
        int procedureId,
        CancellationToken cancellationToken = default)
    {
        return new ClientAppointmentContext
        {
            Patient = await GetActivePatientAsync(patientId, cancellationToken),
            Doctor = await GetActiveDoctorAsync(doctorId, cancellationToken),
            Procedure = await GetActiveProcedureAsync(procedureId, cancellationToken),
            Rooms = await _medicalRoomRepository.GetAllAsync(cancellationToken),
            ExistingAssignments = await _existingAssignmentRepository.GetAllAsync(cancellationToken)
        };
    }

    public async Task<MedicalRoom> GetAvailableRoomAsync(int roomId, CancellationToken cancellationToken = default)
    {
        var room = await _medicalRoomRepository.GetByIdAsync(roomId, cancellationToken);

        if (room is null)
        {
            throw new DomainValidationException("Medical room not found.");
        }

        if (room.Status != RoomStatus.Available)
        {
            throw new BusinessRuleException("The selected room is not available.");
        }

        return room;
    }

    public async Task<MedicalAppointment> GetOwnedAppointmentAsync(
        int appointmentId,
        int patientId,
        CancellationToken cancellationToken = default)
    {
        var appointment = await _medicalAppointmentRepository.GetByIdAsync(appointmentId, cancellationToken);

        if (appointment is null)
        {
            throw new DomainValidationException("Medical appointment not found.");
        }

        if (appointment.PatientId != patientId)
        {
            throw new BusinessRuleException("The patient is not allowed to cancel this appointment.");
        }

        return appointment;
    }

    public TimeSpan ParseTime(string value, string fieldName)
    {
        if (TimeSpan.TryParse(value, out var parsedTime))
        {
            return parsedTime;
        }

        throw new DomainValidationException($"{fieldName} is invalid.");
    }

    public void ValidateAvailabilityRequest(AppointmentAvailabilityRequest request)
    {
        if (request.PatientId <= 0)
        {
            throw new DomainValidationException("Patient id is required.");
        }

        if (request.DoctorId <= 0)
        {
            throw new DomainValidationException("Doctor id is required.");
        }

        if (request.ProcedureId <= 0)
        {
            throw new DomainValidationException("Procedure id is required.");
        }

        if (string.IsNullOrWhiteSpace(request.PreferredStartTime) ||
            string.IsNullOrWhiteSpace(request.PreferredEndTime))
        {
            throw new DomainValidationException("Preferred time range is required.");
        }

        if (ParseTime(request.PreferredStartTime, "PreferredStartTime") >=
            ParseTime(request.PreferredEndTime, "PreferredEndTime"))
        {
            throw new DomainValidationException("Preferred time range is invalid.");
        }
    }

    public void ValidateScheduleRequest(ScheduleClientAppointmentRequest request)
    {
        if (request.PatientId <= 0)
        {
            throw new DomainValidationException("Patient id is required.");
        }

        if (request.DoctorId <= 0)
        {
            throw new DomainValidationException("Doctor id is required.");
        }

        if (request.ProcedureId <= 0)
        {
            throw new DomainValidationException("Procedure id is required.");
        }

        if (request.RoomId <= 0)
        {
            throw new DomainValidationException("Room id is required.");
        }

        if (request.StartDate >= request.EndDate)
        {
            throw new DomainValidationException("Appointment date range is invalid.");
        }
    }

    public void ValidateCancellationRequest(int appointmentId, CancelClientAppointmentRequest request)
    {
        if (appointmentId <= 0)
        {
            throw new DomainValidationException("Appointment id is required.");
        }

        if (request.PatientId <= 0)
        {
            throw new DomainValidationException("Patient id is required.");
        }

        if (string.IsNullOrWhiteSpace(request.CancellationReason))
        {
            throw new DomainValidationException("Cancellation reason is required.");
        }
    }

    public void EnsureSlotDurationMatchesProcedure(DateTime startDate, DateTime endDate, int estimatedDurationMinutes)
    {
        if (estimatedDurationMinutes > 0 &&
            (endDate - startDate).TotalMinutes != estimatedDurationMinutes)
        {
            throw new BusinessRuleException("The selected slot duration does not match the procedure duration.");
        }
    }

    public string GenerateAppointmentCode(DateTime startDate)
    {
        return $"APT-{startDate:yyyyMMdd}-{Guid.NewGuid():N}"[..24].ToUpperInvariant();
    }

    private async Task<Patient> GetActivePatientAsync(int patientId, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(patientId, cancellationToken);

        if (patient is null || !patient.IsActive)
        {
            throw new DomainValidationException("Patient not found or inactive.");
        }

        return patient;
    }

    private async Task<Doctor> GetActiveDoctorAsync(int doctorId, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);

        if (doctor is null || !doctor.IsActive)
        {
            throw new DomainValidationException("Doctor not found or inactive.");
        }

        return doctor;
    }

    private async Task<MedicalProcedure> GetActiveProcedureAsync(int procedureId, CancellationToken cancellationToken)
    {
        var procedure = await _medicalProcedureRepository.GetByIdAsync(procedureId, cancellationToken);

        if (procedure is null || !procedure.IsActive)
        {
            throw new DomainValidationException("Medical procedure not found or inactive.");
        }

        return procedure;
    }
}
