using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Infrastructure.Persistence;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class PatientRepository : EfRepository<Patient>, IPatientRepository
{
    public PatientRepository(MedicalOperationalDbContext context) : base(context)
    {
    }
}
