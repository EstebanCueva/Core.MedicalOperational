using Core.MedicalOperational.Application.DTOs.MedicalRooms;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IMedicalRoomService
{
    Task<IReadOnlyCollection<MedicalRoomResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MedicalRoomResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MedicalRoomResponse> CreateAsync(CreateMedicalRoomRequest request, CancellationToken cancellationToken = default);
    Task<MedicalRoomResponse> UpdateAsync(int id, UpdateMedicalRoomRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
