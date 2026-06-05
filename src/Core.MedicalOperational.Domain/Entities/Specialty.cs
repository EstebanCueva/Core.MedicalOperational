namespace Core.MedicalOperational.Domain.Entities;

public class Specialty
{
    public int Id { get; set; }
    public string SpecialtyCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}