using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Domain.Entities;

public class ApplicationUser
{
    public int Id { get; set; }

    public string UserCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

    public UserRole Role { get; set; }

    public int? DoctorId { get; set; }

    public int? PatientId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
