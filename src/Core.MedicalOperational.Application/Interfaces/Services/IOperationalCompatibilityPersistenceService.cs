using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IOperationalCompatibilityPersistenceService
{
    Task SaveAsync(
        DateTime analysisDate,
        MedicalAppointment appointment,
        Patient patient,
        MedicalProcedure procedure,
        Doctor doctor,
        MedicalRoom room,
        OperationalCompatibilityMatchResponse response,
        CancellationToken cancellationToken = default);

    Task FlushAsync(CancellationToken cancellationToken = default);
}
