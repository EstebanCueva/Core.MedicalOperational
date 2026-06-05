using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class SpecialtyRepository : EfRepository<Specialty>, ISpecialtyRepository
{
    public SpecialtyRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
