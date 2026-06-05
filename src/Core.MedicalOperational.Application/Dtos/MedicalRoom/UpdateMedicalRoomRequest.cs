using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.DTOs.MedicalRooms;

public class UpdateMedicalRoomRequest
{
    public string RoomCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public RoomStatus Status { get; set; }
}