using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class MedicalEquipmentRepository : EfRepository<MedicalEquipment>, IMedicalEquipmentRepository
{
    public MedicalEquipmentRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
