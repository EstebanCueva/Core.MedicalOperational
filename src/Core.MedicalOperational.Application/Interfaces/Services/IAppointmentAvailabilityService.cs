using Core.MedicalOperational.Application.DTOs.ClientAppointments;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IAppointmentAvailabilityService
{
    Task<AppointmentAvailabilityResponse> GetAvailabilityAsync(
        AppointmentAvailabilityRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> IsSlotAvailableAsync(
        ScheduleClientAppointmentRequest request,
        CancellationToken cancellationToken = default);
}
