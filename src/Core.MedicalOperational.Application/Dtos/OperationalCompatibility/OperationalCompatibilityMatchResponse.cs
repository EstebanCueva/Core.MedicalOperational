namespace Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

public class OperationalCompatibilityMatchResponse
{
    public int AppointmentId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string ProcedureCode { get; set; } = string.Empty;
    public string ProcedureName { get; set; } = string.Empty;
    public string DoctorCode { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string RoomCode { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public bool IsCompatible { get; set; }
    public List<string> MissingEquipment { get; set; } = new();
    public List<string> MissingSupplies { get; set; } = new();
    public List<string> Conflicts { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string NextAvailableDate { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public int ProcedureId { get; set; }
    public int DoctorId { get; set; }
    public int RoomId { get; set; }
}