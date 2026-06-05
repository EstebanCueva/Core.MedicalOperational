using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.DTOs.MedicalRooms;

public class CreateMedicalRoomRequest
{
    public string RoomCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public RoomStatus Status { get; set; } = RoomStatus.Available;
}