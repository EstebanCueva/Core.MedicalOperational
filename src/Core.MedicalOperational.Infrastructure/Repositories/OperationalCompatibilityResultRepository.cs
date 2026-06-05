using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class OperationalCompatibilityResultRepository : EfRepository<OperationalCompatibilityResult>, IOperationalCompatibilityResultRepository
{
    public OperationalCompatibilityResultRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
