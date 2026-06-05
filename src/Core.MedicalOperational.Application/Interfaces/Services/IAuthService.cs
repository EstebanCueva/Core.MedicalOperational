using Core.MedicalOperational.Application.DTOs.Auth;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request,CancellationToken cancellationToken = default);
}