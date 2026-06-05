using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user, DateTime expiresAt);
}