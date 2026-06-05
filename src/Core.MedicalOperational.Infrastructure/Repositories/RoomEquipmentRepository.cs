using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class RoomEquipmentRepository : EfRepository<RoomEquipment>, IRoomEquipmentRepository
{
    public RoomEquipmentRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
