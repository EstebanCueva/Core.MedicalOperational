using System.Security.Cryptography;
using System.Text;
using Core.MedicalOperational.Application.DTOs.Auth;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IApplicationUserRepository applicationUserRepository,
        IJwtTokenService jwtTokenService)
    {
        _applicationUserRepository = applicationUserRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _applicationUserRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null)
        {
            throw new DomainValidationException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new BusinessRuleException("User is inactive.");
        }

        var isPasswordValid = VerifyPassword(
            request.Password,
            user.PasswordHash,
            user.PasswordSalt);

        if (!isPasswordValid)
        {
            throw new DomainValidationException("Invalid email or password.");
        }

        var expiresAt = DateTime.UtcNow.AddHours(2);
        var token = _jwtTokenService.GenerateToken(user, expiresAt);

        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            UserCode = user.UserCode,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            DoctorId = user.DoctorId,
            ExpiresAt = expiresAt
        };
    }

    private static bool VerifyPassword(
        string password,
        byte[] storedHash,
        byte[] storedSalt)
    {
        using var hmac = new HMACSHA256(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        if (computedHash.Length != storedHash.Length)
        {
            return false;
        }

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != storedHash[i])
            {
                return false;
            }
        }

        return true;
    }
}