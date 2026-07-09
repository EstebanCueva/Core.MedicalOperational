using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Services;

namespace Core.MedicalOperational.Application.Services;

public class ClientAppointmentService : IClientAppointmentService
{
    private readonly IAppointmentAvailabilityService _appointmentAvailabilityService;
    private readonly IAppointmentSchedulingService _appointmentSchedulingService;
    private readonly IAppointmentCancellationService _appointmentCancellationService;

    public ClientAppointmentService(
        IAppointmentAvailabilityService appointmentAvailabilityService,
        IAppointmentSchedulingService appointmentSchedulingService,
        IAppointmentCancellationService appointmentCancellationService)
    {
        _appointmentAvailabilityService = appointmentAvailabilityService;
        _appointmentSchedulingService = appointmentSchedulingService;
        _appointmentCancellationService = appointmentCancellationService;
    }

    public Task<AppointmentAvailabilityResponse> GetAvailabilityAsync(
        AppointmentAvailabilityRequest request,
        CancellationToken cancellationToken = default)
    {
        return _appointmentAvailabilityService.GetAvailabilityAsync(request, cancellationToken);
    }

    public Task<ScheduleClientAppointmentResponse> ScheduleAsync(
        ScheduleClientAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        return _appointmentSchedulingService.ScheduleAsync(request, cancellationToken);
    }

    public Task<CancelClientAppointmentResponse> CancelAsync(
        int appointmentId,
        CancelClientAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        return _appointmentCancellationService.CancelAsync(appointmentId, request, cancellationToken);
    }
}
