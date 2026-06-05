namespace Core.MedicalOperational.Domain.Entities;

public class RoomEquipment
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int EquipmentId { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable { get; set; } = true;
}