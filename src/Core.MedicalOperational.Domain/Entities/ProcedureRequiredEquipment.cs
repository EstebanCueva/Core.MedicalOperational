namespace Core.MedicalOperational.Domain.Entities;

public class ProcedureRequiredEquipment
{
    public int Id { get; set; } 
    public int ProcedureId { get; set; }
    public int EquipmentId { get; set; }
    public int QuantityRequired { get; set; }
}