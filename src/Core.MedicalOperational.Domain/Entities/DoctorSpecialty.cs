namespace Core.MedicalOperational.Domain.Entities;

public class DoctorSpecialty
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int SpecialtyId { get; set; }
    public bool IsMainSpecialty { get; set; }
}