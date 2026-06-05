using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Domain.Entities;

public class OperationalCompatibilityResult
{
    public int Id { get; set; }

    public DateTime AnalysisDate { get; set; }

    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int ProcedureId { get; set; }

    public int DoctorId { get; set; }

    public int RoomId { get; set; }

    public bool IsCompatible { get; set; }

    public string MissingEquipment { get; set; } = string.Empty;

    public string MissingSupplies { get; set; } = string.Empty;

    public string Conflicts { get; set; } = string.Empty;

    public CompatibilityStatus Status { get; set; }
    public CompatibilitySeverity Severity { get; set; }
    public string Explanation { get; set; } = string.Empty;
}