using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Domain.Entities;

public class MedicalEquipment
{
    public int Id { get; set; }
    public string EquipmentCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Available;
}