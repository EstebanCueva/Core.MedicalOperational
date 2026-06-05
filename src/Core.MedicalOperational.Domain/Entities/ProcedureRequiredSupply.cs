namespace Core.MedicalOperational.Domain.Entities;

public class ProcedureRequiredSupply
{
    public int Id { get; set; }
    public int ProcedureId { get; set; }
    public int SupplyId { get; set; }
    public int QuantityRequired { get; set; }
}