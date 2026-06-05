namespace Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

public class OperationalCompatibilityResponse
{
    public DateTime AnalysisDate { get; set; }
    public int TotalAppointmentsAnalyzed { get; set; }
    public int TotalDoctorsAnalyzed { get; set; }
    public int TotalRoomsAnalyzed { get; set; }
    public int TotalCompatibleOptions { get; set; }
    public int TotalRejectedOptions { get; set; }
    public List<OperationalCompatibilityMatchResponse> Results { get; set; } = new();
    public List<string> GeneralObservations { get; set; } = new();
}