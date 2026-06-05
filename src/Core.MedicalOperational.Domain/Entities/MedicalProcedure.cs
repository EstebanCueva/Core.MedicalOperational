namespace Core.MedicalOperational.Domain.Entities;

public class MedicalProcedure
{
    public int Id { get; set; }
    public string ProcedureCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int RequiredSpecialtyId { get; set; }
    public int EstimatedDurationMinutes { get; set; }
    public bool IsActive { get; set; } = true;
}