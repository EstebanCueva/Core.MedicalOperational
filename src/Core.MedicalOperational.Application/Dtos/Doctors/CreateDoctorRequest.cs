namespace Core.MedicalOperational.Application.DTOs.Doctors;

public class CreateDoctorRequest
{
    public string DoctorCode { get; set; } = string.Empty;  
    public string FullName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
}