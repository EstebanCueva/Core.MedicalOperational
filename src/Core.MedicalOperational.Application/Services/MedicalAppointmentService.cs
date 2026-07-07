using Core.MedicalOperational.Application.Dtos.MedicalAppointment;
using Core.MedicalOperational.Application.DTOs.MedicalAppointments;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class MedicalAppointmentService : IMedicalAppointmentService
{
    private const int SlotStepMinutes = 15;
    private const int MaxSearchDays = 30;

    private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
    private readonly IExistingAssignmentRepository _existingAssignmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMedicalRoomRepository _medicalRoomRepository;

    public MedicalAppointmentService(
        IMedicalAppointmentRepository medicalAppointmentRepository,
        IExistingAssignmentRepository existingAssignmentRepository,
        IDoctorRepository doctorRepository,
        IMedicalRoomRepository medicalRoomRepository)
    {
        _medicalAppointmentRepository = medicalAppointmentRepository;
        _existingAssignmentRepository = existingAssignmentRepository;
        _doctorRepository = doctorRepository;
        _medicalRoomRepository = medicalRoomRepository;
    }

    public async Task<IReadOnlyCollection<MedicalAppointmentResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var appointments = await _medicalAppointmentRepository.GetAllAsync(cancellationToken);
        var response = new List<MedicalAppointmentResponse>();

        foreach (var appointment in appointments)
        {
            response.Add(MapToResponse(appointment));
        }

        return response;
    }

    public async Task<IReadOnlyCollection<MedicalAppointmentResponse>> GetByDoctorIdAsync(
        int doctorId,
        CancellationToken cancellationToken = default)
    {
        if (doctorId <= 0)
        {
            throw new DomainValidationException("Doctor id is required.");
        }

        var assignments = await _existingAssignmentRepository.GetAllAsync(cancellationToken);
        var appointments = await _medicalAppointmentRepository.GetAllAsync(cancellationToken);

        var response = new List<MedicalAppointmentResponse>();
        var addedAppointmentIds = new List<int>();

        foreach (var assignment in assignments)
        {
            if (assignment.DoctorId != doctorId)
            {
                continue;
            }

            if (assignment.Status != AssignmentStatus.Active)
            {
                continue;
            }

            foreach (var appointment in appointments)
            {
                if (appointment.Id != assignment.AppointmentId)
                {
                    continue;
                }

                var alreadyAdded = false;

                foreach (var addedId in addedAppointmentIds)
                {
                    if (addedId == appointment.Id)
                    {
                        alreadyAdded = true;
                        break;
                    }
                }

                if (alreadyAdded)
                {
                    continue;
                }

                response.Add(MapToResponse(appointment));
                addedAppointmentIds.Add(appointment.Id);
                break;
            }
        }

        return response;
    }

    public async Task<MedicalAppointmentResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var appointment = await _medicalAppointmentRepository.GetByIdAsync(id, cancellationToken);
        return appointment is null ? null : MapToResponse(appointment);
    }

    public async Task<MedicalAppointmentResponse> CreateAsync(
        CreateMedicalAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = new MedicalAppointment
        {
            AppointmentCode = request.AppointmentCode,
            PatientId = request.PatientId,
            ProcedureId = request.ProcedureId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status
        };

        var created = await _medicalAppointmentRepository.AddAsync(entity, cancellationToken);
        return MapToResponse(created);
    }

    public async Task<MedicalAppointmentResponse> UpdateAsync(
        int id,
        UpdateMedicalAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var appointment = await _medicalAppointmentRepository.GetByIdAsync(id, cancellationToken);

        if (appointment is null)
        {
            throw new DomainValidationException("Medical appointment not found.");
        }

        appointment.AppointmentCode = request.AppointmentCode;
        appointment.PatientId = request.PatientId;
        appointment.ProcedureId = request.ProcedureId;
        appointment.StartDate = request.StartDate;
        appointment.EndDate = request.EndDate;
        appointment.Status = request.Status;

        await _medicalAppointmentRepository.UpdateAsync(appointment, cancellationToken);
        return MapToResponse(appointment);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await _medicalAppointmentRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<ScheduleMedicalAppointmentResponse> ScheduleAsync(
        int id,
        ScheduleMedicalAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new DomainValidationException("Medical appointment id is required.");
        }

        if (request.DoctorId <= 0)
        {
            throw new DomainValidationException("Doctor id is required.");
        }

        if (request.RoomId <= 0)
        {
            throw new DomainValidationException("Room id is required.");
        }

        if (request.StartDate >= request.EndDate)
        {
            throw new DomainValidationException("The start date must be lower than the end date.");
        }

        var appointment = await _medicalAppointmentRepository.GetByIdAsync(id, cancellationToken);

        if (appointment is null)
        {
            throw new DomainValidationException("Medical appointment not found.");
        }

        if (appointment.Status == AppointmentStatus.Completed)
        {
            throw new DomainValidationException("Completed appointments cannot be scheduled again.");
        }

        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId, cancellationToken);

        if (doctor is null || !doctor.IsActive)
        {
            throw new DomainValidationException("Doctor not found or inactive.");
        }

        var room = await _medicalRoomRepository.GetByIdAsync(request.RoomId, cancellationToken);

        if (room is null)
        {
            throw new DomainValidationException("Medical room not found.");
        }

        if (room.Status != RoomStatus.Available)
        {
            throw new DomainValidationException($"Medical room is not available. Current status: {room.Status}.");
        }

        var assignments = await _existingAssignmentRepository.GetAllAsync(cancellationToken);
        var conflicts = new List<string>();

        foreach (var assignment in assignments)
        {
            if (assignment.Status != AssignmentStatus.Active)
            {
                continue;
            }

            if (assignment.AppointmentId == appointment.Id)
            {
                conflicts.Add("This appointment already has an active schedule.");
                continue;
            }

            var hasTimeConflict = HasDateOverlap(
                request.StartDate,
                request.EndDate,
                assignment.StartDate,
                assignment.EndDate);

            if (!hasTimeConflict)
            {
                continue;
            }

            if (assignment.DoctorId == request.DoctorId)
            {
                AddUnique(conflicts, "Doctor has another assignment during the requested time.");
            }

            if (assignment.RoomId == request.RoomId)
            {
                AddUnique(conflicts, "Room has another assignment during the requested time.");
            }
        }

        if (conflicts.Count > 0)
        {
            throw new DomainValidationException(string.Join(" ", conflicts));
        }

        appointment.StartDate = request.StartDate;
        appointment.EndDate = request.EndDate;
        appointment.Status = AppointmentStatus.Scheduled;

        await _medicalAppointmentRepository.UpdateAsync(appointment, cancellationToken);

        var newAssignment = new ExistingAssignment
        {
            DoctorId = request.DoctorId,
            RoomId = request.RoomId,
            AppointmentId = appointment.Id,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = AssignmentStatus.Active
        };

        var createdAssignment = await _existingAssignmentRepository.AddAsync(newAssignment, cancellationToken);

        return new ScheduleMedicalAppointmentResponse
        {
            AppointmentId = appointment.Id,
            AssignmentId = createdAssignment.Id,
            DoctorId = createdAssignment.DoctorId,
            RoomId = createdAssignment.RoomId,
            StartDate = createdAssignment.StartDate,
            EndDate = createdAssignment.EndDate,
            AppointmentStatus = appointment.Status,
            AssignmentStatus = createdAssignment.Status,
            Message = "Medical appointment scheduled successfully."
        };
    }

    public async Task<CancelMedicalAppointmentResponse> CancelAsync(
        int id,
        CancelMedicalAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new DomainValidationException("Medical appointment id is required.");
        }

        var appointment = await _medicalAppointmentRepository.GetByIdAsync(id, cancellationToken);

        if (appointment is null)
        {
            throw new DomainValidationException("Medical appointment not found.");
        }

        if (appointment.Status == AppointmentStatus.Completed)
        {
            throw new DomainValidationException("Completed appointments cannot be cancelled.");
        }

        var assignments = await _existingAssignmentRepository.GetAllAsync(cancellationToken);
        var cancelledAssignments = 0;
        int? suggestedDoctorId = null;
        int? suggestedRoomId = null;

        foreach (var assignment in assignments)
        {
            if (assignment.AppointmentId != appointment.Id)
            {
                continue;
            }

            if (assignment.Status != AssignmentStatus.Active)
            {
                continue;
            }

            suggestedDoctorId ??= assignment.DoctorId;
            suggestedRoomId ??= assignment.RoomId;

            assignment.Status = AssignmentStatus.Cancelled;
            await _existingAssignmentRepository.UpdateAsync(assignment, cancellationToken);

            cancelledAssignments++;
        }

        appointment.Status = AppointmentStatus.Cancelled;
        await _medicalAppointmentRepository.UpdateAsync(appointment, cancellationToken);

        var duration = appointment.EndDate - appointment.StartDate;

        if (duration <= TimeSpan.Zero)
        {
            duration = TimeSpan.FromMinutes(30);
        }

        DateTime? recommendedStartDate = null;
        DateTime? recommendedEndDate = null;
        var rescheduleExplanation = "No active doctor and room assignment was found, so a reschedule date could not be calculated.";

        if (suggestedDoctorId.HasValue && suggestedRoomId.HasValue)
        {
            var searchFrom = request.SearchFrom ?? GetDefaultSearchFrom(appointment.StartDate);

            recommendedStartDate = CalculateNextAvailableSlot(
                suggestedDoctorId.Value,
                suggestedRoomId.Value,
                searchFrom,
                duration,
                assignments,
                appointment.Id);

            if (recommendedStartDate.HasValue)
            {
                recommendedEndDate = recommendedStartDate.Value.Add(duration);
                rescheduleExplanation = "Suggested date calculated with the same doctor, same room and the original appointment duration.";
            }
            else
            {
                rescheduleExplanation = $"No available slot was found in the next {MaxSearchDays} days for the same doctor and room.";
            }
        }

        return new CancelMedicalAppointmentResponse
        {
            AppointmentId = appointment.Id,
            AppointmentStatus = appointment.Status,
            CancelledAssignments = cancelledAssignments,
            SuggestedDoctorId = suggestedDoctorId,
            SuggestedRoomId = suggestedRoomId,
            RecommendedRescheduleStartDate = recommendedStartDate,
            RecommendedRescheduleEndDate = recommendedEndDate,
            Message = "Medical appointment cancelled successfully.",
            RescheduleExplanation = rescheduleExplanation
        };
    }

    private static MedicalAppointmentResponse MapToResponse(MedicalAppointment appointment)
    {
        return new MedicalAppointmentResponse
        {
            Id = appointment.Id,
            AppointmentCode = appointment.AppointmentCode,
            PatientId = appointment.PatientId,
            ProcedureId = appointment.ProcedureId,
            StartDate = appointment.StartDate,
            EndDate = appointment.EndDate,
            Status = appointment.Status
        };
    }

    private static bool HasDateOverlap(
        DateTime firstStart,
        DateTime firstEnd,
        DateTime secondStart,
        DateTime secondEnd)
    {
        return firstStart < secondEnd && secondStart < firstEnd;
    }

    private static DateTime GetDefaultSearchFrom(DateTime appointmentStartDate)
    {
        var currentDate = DateTime.Now;
        var searchFrom = currentDate > appointmentStartDate ? currentDate : appointmentStartDate;
        return RoundToNextSlot(searchFrom);
    }

    private static DateTime RoundToNextSlot(DateTime value)
    {
        var minutesToAdd = SlotStepMinutes - value.Minute % SlotStepMinutes;

        if (minutesToAdd == SlotStepMinutes && value.Second == 0 && value.Millisecond == 0)
        {
            return value;
        }

        return value
            .AddMinutes(minutesToAdd)
            .AddSeconds(-value.Second)
            .AddMilliseconds(-value.Millisecond);
    }

    private static DateTime? CalculateNextAvailableSlot(
        int doctorId,
        int roomId,
        DateTime searchFrom,
        TimeSpan duration,
        IReadOnlyCollection<ExistingAssignment> assignments,
        int ignoredAppointmentId)
    {
        var candidateStart = RoundToNextSlot(searchFrom);
        var searchLimit = candidateStart.AddDays(MaxSearchDays);

        while (candidateStart <= searchLimit)
        {
            var candidateEnd = candidateStart.Add(duration);
            var hasConflict = false;
            DateTime? latestConflictEnd = null;

            foreach (var assignment in assignments)
            {
                if (assignment.Status != AssignmentStatus.Active)
                {
                    continue;
                }

                if (assignment.AppointmentId == ignoredAppointmentId)
                {
                    continue;
                }

                var isSameDoctor = assignment.DoctorId == doctorId;
                var isSameRoom = assignment.RoomId == roomId;

                if (!isSameDoctor && !isSameRoom)
                {
                    continue;
                }

                var hasTimeConflict = HasDateOverlap(
                    candidateStart,
                    candidateEnd,
                    assignment.StartDate,
                    assignment.EndDate);

                if (!hasTimeConflict)
                {
                    continue;
                }

                hasConflict = true;

                if (!latestConflictEnd.HasValue || assignment.EndDate > latestConflictEnd.Value)
                {
                    latestConflictEnd = assignment.EndDate;
                }
            }

            if (!hasConflict)
            {
                return candidateStart;
            }

            candidateStart = latestConflictEnd.HasValue
                ? RoundToNextSlot(latestConflictEnd.Value.AddMinutes(SlotStepMinutes))
                : candidateStart.AddMinutes(SlotStepMinutes);
        }

        return null;
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
}