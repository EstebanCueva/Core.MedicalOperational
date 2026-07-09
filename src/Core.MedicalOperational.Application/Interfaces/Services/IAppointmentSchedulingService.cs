using Core.MedicalOperational.Application.DTOs.ClientAppointments;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IAppointmentSchedulingService
{
    Task<ScheduleClientAppointmentResponse> ScheduleAsync(
        ScheduleClientAppointmentRequest request,
        CancellationToken cancellationToken = default);
}
