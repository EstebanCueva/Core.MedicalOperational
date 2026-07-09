using Core.MedicalOperational.Application.DTOs.ClientAppointments;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IClientAppointmentService
{
    Task<AppointmentAvailabilityResponse> GetAvailabilityAsync(
        AppointmentAvailabilityRequest request,
        CancellationToken cancellationToken = default);

    Task<ScheduleClientAppointmentResponse> ScheduleAsync(
        ScheduleClientAppointmentRequest request,
        CancellationToken cancellationToken = default);

    Task<CancelClientAppointmentResponse> CancelAsync(
        int appointmentId,
        CancelClientAppointmentRequest request,
        CancellationToken cancellationToken = default);
}
