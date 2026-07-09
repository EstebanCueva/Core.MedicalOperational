using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IOperationalCompatibilityRule
{
    Task ApplyAsync(
        OperationalCompatibilityCandidate candidate,
        OperationalCompatibilityEvaluation evaluation,
        CancellationToken cancellationToken = default);
}
