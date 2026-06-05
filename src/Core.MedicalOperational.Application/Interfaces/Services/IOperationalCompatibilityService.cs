using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IOperationalCompatibilityService
{
    Task<OperationalCompatibilityResponse> AnalyzeAsync(OperationalCompatibilityRequest request, CancellationToken cancellationToken = default);
}
