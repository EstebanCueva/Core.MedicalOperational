using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class MedicalProcedureRepository : EfRepository<MedicalProcedure>, IMedicalProcedureRepository
{
    public MedicalProcedureRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
