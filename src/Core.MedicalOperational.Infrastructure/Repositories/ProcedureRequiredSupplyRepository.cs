using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class ProcedureRequiredSupplyRepository : EfRepository<ProcedureRequiredSupply>, IProcedureRequiredSupplyRepository
{
    public ProcedureRequiredSupplyRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
