using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Services;

public class OperationalCompatibilityPersistenceService : IOperationalCompatibilityPersistenceService
{
    private readonly IOperationalCompatibilityResultRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public OperationalCompatibilityPersistenceService(
        IOperationalCompatibilityResultRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task SaveAsync(
        DateTime analysisDate,
        MedicalAppointment appointment,
        Patient patient,
        MedicalProcedure procedure,
        Doctor doctor,
        MedicalRoom room,
        OperationalCompatibilityMatchResponse response,
        CancellationToken cancellationToken = default)
    {
        await _repository.AddAsync(new OperationalCompatibilityResult
        {
            AnalysisDate = analysisDate,
            AppointmentId = appointment.Id,
            PatientId = patient.Id,
            ProcedureId = procedure.Id,
            DoctorId = doctor.Id,
            RoomId = room.Id,
            IsCompatible = response.IsCompatible,
            MissingEquipment = string.Join(", ", response.MissingEquipment),
            MissingSupplies = string.Join(", ", response.MissingSupplies),
            Conflicts = string.Join(", ", response.Conflicts),
            Status = response.IsCompatible ? CompatibilityStatus.Compatible : CompatibilityStatus.NotCompatible,
            Severity = response.IsCompatible ? CompatibilitySeverity.Information : CompatibilitySeverity.Critical,
            Explanation = response.Explanation
        }, cancellationToken);
    }

    public Task FlushAsync(CancellationToken cancellationToken = default)
    {
        return _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
