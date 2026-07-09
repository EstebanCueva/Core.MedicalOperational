using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class AppointmentSchedulingService : IAppointmentSchedulingService
{
    private readonly IAppointmentAvailabilityService _availabilityService;
    private readonly IClientAppointmentDomainService _domainService;
    private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
    private readonly IExistingAssignmentRepository _existingAssignmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentSchedulingService(
        IAppointmentAvailabilityService availabilityService,
        IClientAppointmentDomainService domainService,
        IMedicalAppointmentRepository medicalAppointmentRepository,
        IExistingAssignmentRepository existingAssignmentRepository,
        IUnitOfWork unitOfWork)
    {
        _availabilityService = availabilityService;
        _domainService = domainService;
        _medicalAppointmentRepository = medicalAppointmentRepository;
        _existingAssignmentRepository = existingAssignmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ScheduleClientAppointmentResponse> ScheduleAsync(
        ScheduleClientAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!await _availabilityService.IsSlotAvailableAsync(request, cancellationToken))
        {
            throw new BusinessRuleException("The selected slot is no longer available.");
        }

        var appointment = new MedicalAppointment
        {
            AppointmentCode = _domainService.GenerateAppointmentCode(request.StartDate),
            PatientId = request.PatientId,
            ProcedureId = request.ProcedureId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Notes = request.Notes.Trim(),
            Status = AppointmentStatus.Scheduled
        };

        var createdAppointment = await _medicalAppointmentRepository.AddAsync(appointment, cancellationToken);

        await _existingAssignmentRepository.AddAsync(new ExistingAssignment
        {
            DoctorId = request.DoctorId,
            RoomId = request.RoomId,
            AppointmentId = createdAppointment.Id,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = AssignmentStatus.Active
        }, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ScheduleClientAppointmentResponse
        {
            AppointmentId = createdAppointment.Id,
            AppointmentCode = createdAppointment.AppointmentCode,
            PatientId = createdAppointment.PatientId,
            DoctorId = request.DoctorId,
            RoomId = request.RoomId,
            ProcedureId = createdAppointment.ProcedureId,
            StartDate = createdAppointment.StartDate,
            EndDate = createdAppointment.EndDate,
            Status = createdAppointment.Status.ToString(),
            Message = "Appointment scheduled successfully."
        };
    }
}
