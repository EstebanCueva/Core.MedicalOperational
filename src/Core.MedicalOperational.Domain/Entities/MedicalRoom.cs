using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Domain.Entities;

public class MedicalRoom
{
    public int Id { get; set; }
    public string RoomCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public RoomStatus Status { get; set; } = RoomStatus.Available;
}