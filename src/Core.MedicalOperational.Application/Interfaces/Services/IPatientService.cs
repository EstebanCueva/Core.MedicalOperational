using Core.MedicalOperational.Application.DTOs.Patients;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IPatientService
{
    Task<IReadOnlyCollection<PatientResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PatientResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PatientResponse> CreateAsync(CreatePatientRequest request, CancellationToken cancellationToken = default);
    Task<PatientResponse> UpdateAsync(int id, UpdatePatientRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
