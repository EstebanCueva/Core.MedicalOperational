using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class ProcedureRequiredEquipmentRepository : EfRepository<ProcedureRequiredEquipment>, IProcedureRequiredEquipmentRepository
{
    public ProcedureRequiredEquipmentRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
