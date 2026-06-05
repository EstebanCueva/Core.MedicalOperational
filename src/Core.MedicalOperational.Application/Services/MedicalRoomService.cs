using Core.MedicalOperational.Application.DTOs.MedicalRooms;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class MedicalRoomService : IMedicalRoomService
{
    private readonly IMedicalRoomRepository _medicalRoomRepository;

    public MedicalRoomService(IMedicalRoomRepository medicalRoomRepository)
    {
        _medicalRoomRepository = medicalRoomRepository;
    }

    public async Task<IReadOnlyCollection<MedicalRoomResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await _medicalRoomRepository.GetAllAsync(cancellationToken);
        var response = new List<MedicalRoomResponse>();

        foreach (var room in rooms)
        {
            response.Add(MapToResponse(room));
        }

        return response;
    }

    public async Task<MedicalRoomResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _medicalRoomRepository.GetByIdAsync(id, cancellationToken);
        return room is null ? null : MapToResponse(room);
    }

    public async Task<MedicalRoomResponse> CreateAsync(
        CreateMedicalRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = new MedicalRoom
        {
            RoomCode = request.RoomCode,
            Name = request.Name,
            Status = request.Status
        };

        var created = await _medicalRoomRepository.AddAsync(entity, cancellationToken);
        return MapToResponse(created);
    }

    public async Task<MedicalRoomResponse> UpdateAsync(int id, UpdateMedicalRoomRequest request, CancellationToken cancellationToken = default)
    {
        var room = await _medicalRoomRepository.GetByIdAsync(id, cancellationToken);

        if (room is null)
        {
            throw new DomainValidationException("Medical room not found.");
        }

        room.RoomCode = request.RoomCode;
        room.Name = request.Name;
        room.Status = request.Status;

        await _medicalRoomRepository.UpdateAsync(room, cancellationToken);
        return MapToResponse(room);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _medicalRoomRepository.DeleteAsync(id, cancellationToken);
    }

    private static MedicalRoomResponse MapToResponse(MedicalRoom room)
    {
        return new MedicalRoomResponse
        {
            Id = room.Id,
            RoomCode = room.RoomCode,
            Name = room.Name,
            Status = room.Status
        };
    }
}
