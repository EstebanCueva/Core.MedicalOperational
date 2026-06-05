using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class MedicalAppointmentRepository : EfRepository<MedicalAppointment>, IMedicalAppointmentRepository
{
    public MedicalAppointmentRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
