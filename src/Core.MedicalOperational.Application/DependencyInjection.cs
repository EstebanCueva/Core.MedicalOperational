using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.MedicalOperational.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOperationalCompatibilityService, OperationalCompatibilityService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IMedicalProcedureService, MedicalProcedureService>();
        services.AddScoped<IMedicalRoomService, MedicalRoomService>();
        services.AddScoped<IMedicalAppointmentService, MedicalAppointmentService>();

        return services;
    }
}