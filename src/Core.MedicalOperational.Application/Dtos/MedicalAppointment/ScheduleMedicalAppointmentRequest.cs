using System.ComponentModel.DataAnnotations;

namespace Core.MedicalOperational.Application.DTOs.MedicalAppointments;

public class ScheduleMedicalAppointmentRequest
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}