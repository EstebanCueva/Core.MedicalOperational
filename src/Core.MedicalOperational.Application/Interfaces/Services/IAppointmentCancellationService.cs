using Core.MedicalOperational.Application.DTOs.ClientAppointments;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IAppointmentCancellationService
{
    Task<CancelClientAppointmentResponse> CancelAsync(
        int appointmentId,
        CancelClientAppointmentRequest request,
        CancellationToken cancellationToken = default);
}
