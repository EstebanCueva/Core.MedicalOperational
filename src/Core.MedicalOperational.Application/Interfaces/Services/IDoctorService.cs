using Core.MedicalOperational.Application.DTOs.Doctors;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IDoctorService
{
    Task<IReadOnlyCollection<DoctorResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DoctorResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DoctorResponse> CreateAsync(CreateDoctorRequest request, CancellationToken cancellationToken = default);
    Task<DoctorResponse> UpdateAsync(int id, UpdateDoctorRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
