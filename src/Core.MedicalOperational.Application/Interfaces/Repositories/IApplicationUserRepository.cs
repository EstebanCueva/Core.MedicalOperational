using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.Interfaces.Repositories;

public interface IApplicationUserRepository : IBaseRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByEmailAsync(string email,CancellationToken cancellationToken = default);
}