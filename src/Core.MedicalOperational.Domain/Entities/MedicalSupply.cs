namespace Core.MedicalOperational.Domain.Entities;

public class MedicalSupply
{
    public int Id { get; set; }
    public string SupplyCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}