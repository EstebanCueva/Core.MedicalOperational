using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Enums;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class AppointmentCancellationService : IAppointmentCancellationService
{
    private readonly IClientAppointmentDomainService _domainService;
    private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
    private readonly IExistingAssignmentRepository _existingAssignmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentCancellationService(
        IClientAppointmentDomainService domainService,
        IMedicalAppointmentRepository medicalAppointmentRepository,
        IExistingAssignmentRepository existingAssignmentRepository,
        IUnitOfWork unitOfWork)
    {
        _domainService = domainService;
        _medicalAppointmentRepository = medicalAppointmentRepository;
        _existingAssignmentRepository = existingAssignmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CancelClientAppointmentResponse> CancelAsync(
        int appointmentId,
        CancelClientAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        _domainService.ValidateCancellationRequest(appointmentId, request);

        var appointment = await _domainService.GetOwnedAppointmentAsync(appointmentId, request.PatientId, cancellationToken);

        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            throw new BusinessRuleException("The appointment is already cancelled.");
        }

        var previousStatus = appointment.Status;
        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancellationReason = request.CancellationReason.Trim();
        appointment.CancelledAt = request.RequestedAt;

        await _medicalAppointmentRepository.UpdateAsync(appointment, cancellationToken);

        var assignments = await _existingAssignmentRepository.GetAllAsync(cancellationToken);

        foreach (var assignment in assignments)
        {
            if (assignment.AppointmentId != appointmentId || assignment.Status != AssignmentStatus.Active)
            {
                continue;
            }

            assignment.Status = AssignmentStatus.Cancelled;
            await _existingAssignmentRepository.UpdateAsync(assignment, cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CancelClientAppointmentResponse
        {
            AppointmentId = appointment.Id,
            PreviousStatus = previousStatus.ToString(),
            CurrentStatus = appointment.Status.ToString(),
            CancelledAt = request.RequestedAt,
            Message = "Appointment cancelled and schedule released."
        };
    }
}
