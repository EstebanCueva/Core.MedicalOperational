using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class MedicalSupplyRepository : EfRepository<MedicalSupply>, IMedicalSupplyRepository
{
    public MedicalSupplyRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
