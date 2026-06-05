using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class ApplicationUserRepository : EfRepository<ApplicationUser>, IApplicationUserRepository
{
    public ApplicationUserRepository(MedicalOperationalDbContext context) : base(context)
    {
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email,CancellationToken cancellationToken = default)
    {
        return await Context.ApplicationUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email,cancellationToken);
    }
}