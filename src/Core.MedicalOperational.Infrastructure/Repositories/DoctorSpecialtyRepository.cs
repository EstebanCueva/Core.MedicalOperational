using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class DoctorSpecialtyRepository : EfRepository<DoctorSpecialty>, IDoctorSpecialtyRepository
{
    public DoctorSpecialtyRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
