namespace Core.MedicalOperational.Application.DTOs.MedicalProcedures;

public class UpdateMedicalProcedureRequest
{
    public string ProcedureCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int RequiredSpecialtyId { get; set; }
    public int EstimatedDurationMinutes { get; set; }
    public bool IsActive { get; set; }
}