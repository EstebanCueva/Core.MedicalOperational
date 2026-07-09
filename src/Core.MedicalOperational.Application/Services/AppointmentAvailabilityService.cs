using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Services;

namespace Core.MedicalOperational.Application.Services;

public class AppointmentAvailabilityService : IAppointmentAvailabilityService
{
    private readonly IClientAppointmentDomainService _domainService;
    private readonly IEnumerable<IAppointmentAvailabilityRule> _rules;

    public AppointmentAvailabilityService(
        IClientAppointmentDomainService domainService,
        IEnumerable<IAppointmentAvailabilityRule> rules)
    {
        _domainService = domainService;
        _rules = rules;
    }

    public async Task<AppointmentAvailabilityResponse> GetAvailabilityAsync(
        AppointmentAvailabilityRequest request,
        CancellationToken cancellationToken = default)
    {
        _domainService.ValidateAvailabilityRequest(request);

        var context = await _domainService.BuildContextAsync(
            request.PatientId,
            request.DoctorId,
            request.ProcedureId,
            cancellationToken);

        var requestedDay = request.RequestedDate.Date;
        var windowStart = requestedDay.Add(_domainService.ParseTime(request.PreferredStartTime, "PreferredStartTime"));
        var windowEnd = requestedDay.Add(_domainService.ParseTime(request.PreferredEndTime, "PreferredEndTime"));
        var slotDuration = TimeSpan.FromMinutes(context.Procedure.EstimatedDurationMinutes);
        var availableSlots = new List<AvailableSlotResponse>();

        for (var currentStart = windowStart; currentStart.Add(slotDuration) <= windowEnd; currentStart = currentStart.Add(slotDuration))
        {
            var currentEnd = currentStart.Add(slotDuration);

            foreach (var room in context.Rooms)
            {
                var candidate = new AppointmentSlotCandidate
                {
                    PatientId = context.Patient.Id,
                    Doctor = context.Doctor,
                    Procedure = context.Procedure,
                    Room = room,
                    StartDate = currentStart,
                    EndDate = currentEnd,
                    ExistingAssignments = context.ExistingAssignments
                };

                if (!await IsCandidateAvailableAsync(candidate, cancellationToken))
                {
                    continue;
                }

                availableSlots.Add(new AvailableSlotResponse
                {
                    StartDate = currentStart,
                    EndDate = currentEnd,
                    RoomId = room.Id,
                    RoomName = room.Name,
                    IsAvailable = true
                });
            }
        }

        return new AppointmentAvailabilityResponse
        {
            DoctorId = context.Doctor.Id,
            DoctorName = context.Doctor.FullName,
            ProcedureId = context.Procedure.Id,
            ProcedureName = context.Procedure.Name,
            RequestedDate = requestedDay,
            AvailableSlots = availableSlots,
            Message = availableSlots.Count > 0
                ? "Available slots found for the selected doctor."
                : $"No available slots were found for patient {context.Patient.FullName} on the selected date."
        };
    }

    public async Task<bool> IsSlotAvailableAsync(
        ScheduleClientAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        _domainService.ValidateScheduleRequest(request);

        var context = await _domainService.BuildContextAsync(
            request.PatientId,
            request.DoctorId,
            request.ProcedureId,
            cancellationToken);

        _domainService.EnsureSlotDurationMatchesProcedure(
            request.StartDate,
            request.EndDate,
            context.Procedure.EstimatedDurationMinutes);

        var room = await _domainService.GetAvailableRoomAsync(request.RoomId, cancellationToken);

        return await IsCandidateAvailableAsync(new AppointmentSlotCandidate
        {
            PatientId = context.Patient.Id,
            Doctor = context.Doctor,
            Procedure = context.Procedure,
            Room = room,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ExistingAssignments = context.ExistingAssignments
        }, cancellationToken);
    }

    private async Task<bool> IsCandidateAvailableAsync(
        AppointmentSlotCandidate candidate,
        CancellationToken cancellationToken)
    {
        foreach (var rule in _rules)
        {
            if (!await rule.IsSatisfiedAsync(candidate, cancellationToken))
            {
                return false;
            }
        }

        return true;
    }
}
