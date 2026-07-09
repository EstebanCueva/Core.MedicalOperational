using Core.MedicalOperational.Application.DTOs.MedicalAppointments;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class MedicalAppointmentService : IMedicalAppointmentService
{
    private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
    private readonly IExistingAssignmentRepository _existingAssignmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MedicalAppointmentService(
        IMedicalAppointmentRepository medicalAppointmentRepository,
        IExistingAssignmentRepository existingAssignmentRepository,
        IUnitOfWork unitOfWork)
    {
        _medicalAppointmentRepository = medicalAppointmentRepository;
        _existingAssignmentRepository = existingAssignmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyCollection<MedicalAppointmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
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

    public async Task<MedicalAppointmentResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
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
            Notes = request.Notes,
            CancellationReason = request.CancellationReason,
            CancelledAt = request.CancelledAt,
            Status = request.Status
        };

        var created = await _medicalAppointmentRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToResponse(created);
    }

    public async Task<MedicalAppointmentResponse> UpdateAsync(int id, UpdateMedicalAppointmentRequest request, CancellationToken cancellationToken = default)
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
        appointment.Notes = request.Notes;
        appointment.CancellationReason = request.CancellationReason;
        appointment.CancelledAt = request.CancelledAt;
        appointment.Status = request.Status;

        await _medicalAppointmentRepository.UpdateAsync(appointment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToResponse(appointment);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _medicalAppointmentRepository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
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
            Notes = appointment.Notes,
            CancellationReason = appointment.CancellationReason,
            CancelledAt = appointment.CancelledAt,
            Status = appointment.Status
        };
    }
}
