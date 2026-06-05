namespace Core.MedicalOperational.Domain.Entities;

public class SupplyStock
{
    public int Id { get; set; }
    public int SupplyId { get; set; }
    public int AvailableQuantity { get; set; }
    public int MinimumQuantity { get; set; }
}