using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class MedicalRoomRepository : EfRepository<MedicalRoom>, IMedicalRoomRepository
{
    public MedicalRoomRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
