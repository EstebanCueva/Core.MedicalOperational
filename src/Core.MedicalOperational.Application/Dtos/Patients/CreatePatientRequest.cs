namespace Core.MedicalOperational.Application.DTOs.Patients;

public class CreatePatientRequest
{
    public string PatientCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}