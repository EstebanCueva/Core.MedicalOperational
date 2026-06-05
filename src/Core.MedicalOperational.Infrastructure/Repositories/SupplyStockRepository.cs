using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class SupplyStockRepository : EfRepository<SupplyStock>, ISupplyStockRepository
{
    public SupplyStockRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
