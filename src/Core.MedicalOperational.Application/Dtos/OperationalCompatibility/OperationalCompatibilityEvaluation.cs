namespace Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

public class OperationalCompatibilityEvaluation
{
    public List<string> MissingEquipment { get; } = [];
    public List<string> MissingSupplies { get; } = [];
    public List<string> Conflicts { get; } = [];
    public bool IsCompatible => MissingEquipment.Count == 0 && MissingSupplies.Count == 0 && Conflicts.Count == 0;
}
