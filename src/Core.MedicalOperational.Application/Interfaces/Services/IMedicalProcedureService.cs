using Core.MedicalOperational.Application.DTOs.MedicalProcedures;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IMedicalProcedureService
{
    Task<IReadOnlyCollection<MedicalProcedureResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MedicalProcedureResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MedicalProcedureResponse> CreateAsync(CreateMedicalProcedureRequest request, CancellationToken cancellationToken = default);
    Task<MedicalProcedureResponse> UpdateAsync(int id, UpdateMedicalProcedureRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
