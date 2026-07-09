using Core.MedicalOperational.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Core.MedicalOperational.Application.Interfaces.Repositories;

namespace Core.MedicalOperational.Infrastructure.Persistence;

public class MedicalOperationalDbContext : DbContext, IUnitOfWork
{
    public MedicalOperationalDbContext(DbContextOptions<MedicalOperationalDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Doctor> Doctors => Set<Doctor>();

    public DbSet<Specialty> Specialties => Set<Specialty>();

    public DbSet<DoctorSpecialty> DoctorSpecialties => Set<DoctorSpecialty>();

    public DbSet<MedicalProcedure> MedicalProcedures => Set<MedicalProcedure>();

    public DbSet<MedicalRoom> MedicalRooms => Set<MedicalRoom>();

    public DbSet<MedicalEquipment> MedicalEquipments => Set<MedicalEquipment>();

    public DbSet<RoomEquipment> RoomEquipments => Set<RoomEquipment>();

    public DbSet<ProcedureRequiredEquipment> ProcedureRequiredEquipments => Set<ProcedureRequiredEquipment>();

    public DbSet<MedicalSupply> MedicalSupplies => Set<MedicalSupply>();

    public DbSet<SupplyStock> SupplyStocks => Set<SupplyStock>();

    public DbSet<ProcedureRequiredSupply> ProcedureRequiredSupplies => Set<ProcedureRequiredSupply>();

    public DbSet<MedicalAppointment> MedicalAppointments => Set<MedicalAppointment>();

    public DbSet<ExistingAssignment> ExistingAssignments => Set<ExistingAssignment>();

    public DbSet<OperationalCompatibilityResult> OperationalCompatibilityResults => Set<OperationalCompatibilityResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureApplicationUsers(modelBuilder);
        ConfigurePatients(modelBuilder);
        ConfigureDoctors(modelBuilder);
        ConfigureSpecialties(modelBuilder);
        ConfigureDoctorSpecialties(modelBuilder);
        ConfigureMedicalProcedures(modelBuilder);
        ConfigureMedicalRooms(modelBuilder);
        ConfigureMedicalEquipments(modelBuilder);
        ConfigureRoomEquipments(modelBuilder);
        ConfigureProcedureRequiredEquipments(modelBuilder);
        ConfigureMedicalSupplies(modelBuilder);
        ConfigureSupplyStocks(modelBuilder);
        ConfigureProcedureRequiredSupplies(modelBuilder);
        ConfigureMedicalAppointments(modelBuilder);
        ConfigureExistingAssignments(modelBuilder);
        ConfigureOperationalCompatibilityResults(modelBuilder);
    }
    private static void ConfigureApplicationUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("ApplicationUsers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.UserCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.PasswordHash)
                .IsRequired();

            entity.Property(x => x.PasswordSalt)
                .IsRequired();

            entity.Property(x => x.Role)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.DoctorId)
                .IsRequired(false);

            entity.Property(x => x.PatientId)
                .IsRequired(false);

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasIndex(x => x.UserCode)
                .IsUnique();

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.HasOne<Doctor>()
                .WithMany()
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Patient>()
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigurePatients(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patients");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.PatientCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.DocumentNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.BirthDate)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasIndex(x => x.PatientCode)
                .IsUnique();

            entity.HasIndex(x => x.DocumentNumber)
                .IsUnique();
        });
    }

    private static void ConfigureDoctors(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctors");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.DoctorCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.LicenseNumber)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasIndex(x => x.DoctorCode)
                .IsUnique();

            entity.HasIndex(x => x.LicenseNumber)
                .IsUnique();
        });
    }

    private static void ConfigureSpecialties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.ToTable("Specialties");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.SpecialtyCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasIndex(x => x.SpecialtyCode)
                .IsUnique();
        });
    }

    private static void ConfigureDoctorSpecialties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorSpecialty>(entity =>
        {
            entity.ToTable("DoctorSpecialties");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.DoctorId)
                .IsRequired();

            entity.Property(x => x.SpecialtyId)
                .IsRequired();

            entity.Property(x => x.IsMainSpecialty)
                .IsRequired();

            entity.HasIndex(x => new { x.DoctorId, x.SpecialtyId })
                .IsUnique();

            entity.HasOne<Doctor>()
                .WithMany()
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Specialty>()
                .WithMany()
                .HasForeignKey(x => x.SpecialtyId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureMedicalProcedures(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MedicalProcedure>(entity =>
        {
            entity.ToTable("MedicalProcedures");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.ProcedureCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.RequiredSpecialtyId)
                .IsRequired();

            entity.Property(x => x.EstimatedDurationMinutes)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasIndex(x => x.ProcedureCode)
                .IsUnique();

            entity.HasOne<Specialty>()
                .WithMany()
                .HasForeignKey(x => x.RequiredSpecialtyId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureMedicalRooms(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MedicalRoom>(entity =>
        {
            entity.ToTable("MedicalRooms");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.RoomCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.HasIndex(x => x.RoomCode)
                .IsUnique();
        });
    }

    private static void ConfigureMedicalEquipments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MedicalEquipment>(entity =>
        {
            entity.ToTable("MedicalEquipments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.EquipmentCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.HasIndex(x => x.EquipmentCode)
                .IsUnique();
        });
    }

    private static void ConfigureRoomEquipments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomEquipment>(entity =>
        {
            entity.ToTable("RoomEquipments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.RoomId)
                .IsRequired();

            entity.Property(x => x.EquipmentId)
                .IsRequired();

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Property(x => x.IsAvailable)
                .IsRequired();

            entity.HasOne<MedicalRoom>()
                .WithMany()
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<MedicalEquipment>()
                .WithMany()
                .HasForeignKey(x => x.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureProcedureRequiredEquipments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProcedureRequiredEquipment>(entity =>
        {
            entity.ToTable("ProcedureRequiredEquipments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.ProcedureId)
                .IsRequired();

            entity.Property(x => x.EquipmentId)
                .IsRequired();

            entity.Property(x => x.QuantityRequired)
                .IsRequired();

            entity.HasOne<MedicalProcedure>()
                .WithMany()
                .HasForeignKey(x => x.ProcedureId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<MedicalEquipment>()
                .WithMany()
                .HasForeignKey(x => x.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureMedicalSupplies(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MedicalSupply>(entity =>
        {
            entity.ToTable("MedicalSupplies");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.SupplyCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Unit)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasIndex(x => x.SupplyCode)
                .IsUnique();
        });
    }

    private static void ConfigureSupplyStocks(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SupplyStock>(entity =>
        {
            entity.ToTable("SupplyStocks");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.SupplyId)
                .IsRequired();

            entity.Property(x => x.AvailableQuantity)
                .IsRequired();

            entity.Property(x => x.MinimumQuantity)
                .IsRequired();

            entity.HasOne<MedicalSupply>()
                .WithMany()
                .HasForeignKey(x => x.SupplyId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureProcedureRequiredSupplies(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProcedureRequiredSupply>(entity =>
        {
            entity.ToTable("ProcedureRequiredSupplies");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.ProcedureId)
                .IsRequired();

            entity.Property(x => x.SupplyId)
                .IsRequired();

            entity.Property(x => x.QuantityRequired)
                .IsRequired();

            entity.HasOne<MedicalProcedure>()
                .WithMany()
                .HasForeignKey(x => x.ProcedureId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<MedicalSupply>()
                .WithMany()
                .HasForeignKey(x => x.SupplyId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureMedicalAppointments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MedicalAppointment>(entity =>
        {
            entity.ToTable("MedicalAppointments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.AppointmentCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.PatientId)
                .IsRequired();

            entity.Property(x => x.ProcedureId)
                .IsRequired();

            entity.Property(x => x.StartDate)
                .IsRequired();

            entity.Property(x => x.EndDate)
                .IsRequired();

            entity.Property(x => x.Notes)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(x => x.CancellationReason)
                .HasMaxLength(1000)
                .IsRequired(false);

            entity.Property(x => x.CancelledAt)
                .IsRequired(false);

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.HasIndex(x => x.AppointmentCode)
                .IsUnique();

            entity.HasOne<Patient>()
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<MedicalProcedure>()
                .WithMany()
                .HasForeignKey(x => x.ProcedureId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureExistingAssignments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExistingAssignment>(entity =>
        {
            entity.ToTable("ExistingAssignments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.DoctorId)
                .IsRequired();

            entity.Property(x => x.RoomId)
                .IsRequired();

            entity.Property(x => x.AppointmentId)
                .IsRequired();

            entity.Property(x => x.StartDate)
                .IsRequired();

            entity.Property(x => x.EndDate)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.HasOne<Doctor>()
                .WithMany()
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<MedicalRoom>()
                .WithMany()
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<MedicalAppointment>()
                .WithMany()
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureOperationalCompatibilityResults(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationalCompatibilityResult>(entity =>
        {
            entity.ToTable("OperationalCompatibilityResults");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.AnalysisDate)
                .IsRequired();

            entity.Property(x => x.AppointmentId)
                .IsRequired();

            entity.Property(x => x.PatientId)
                .IsRequired();

            entity.Property(x => x.ProcedureId)
                .IsRequired();

            entity.Property(x => x.DoctorId)
                .IsRequired();

            entity.Property(x => x.RoomId)
                .IsRequired();

            entity.Property(x => x.IsCompatible)
                .IsRequired();

            entity.Property(x => x.MissingEquipment)
                .HasMaxLength(1000);

            entity.Property(x => x.MissingSupplies)
                .HasMaxLength(1000);

            entity.Property(x => x.Conflicts)
                .HasMaxLength(1500);

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Severity)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Explanation)
                .HasMaxLength(1500)
                .IsRequired();

            entity.HasOne<MedicalAppointment>()
                .WithMany()
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Patient>()
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<MedicalProcedure>()
                .WithMany()
                .HasForeignKey(x => x.ProcedureId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Doctor>()
                .WithMany()
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<MedicalRoom>()
                .WithMany()
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
