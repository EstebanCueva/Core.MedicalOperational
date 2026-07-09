using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IOperationalCompatibilityContextService
{
    Task<OperationalCompatibilityAnalysisContext> BuildAsync(CancellationToken cancellationToken = default);
}
