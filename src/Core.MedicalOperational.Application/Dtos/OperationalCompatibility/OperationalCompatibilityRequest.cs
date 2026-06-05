namespace Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

public class OperationalCompatibilityRequest
{
    public DateTime AnalysisDate { get; set; } = DateTime.Now;
    public string? SpecialtyCode { get; set; }
    public string? ProcedureCode { get; set; }
    public bool IncludeOnlyCompatible { get; set; }
    public bool SaveResults { get; set; }
}