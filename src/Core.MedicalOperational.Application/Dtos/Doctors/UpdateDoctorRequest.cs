namespace Core.MedicalOperational.Application.DTOs.Doctors;

public class UpdateDoctorRequest
{
    public string DoctorCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}