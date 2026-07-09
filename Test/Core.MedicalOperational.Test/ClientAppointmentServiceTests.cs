using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Application.Services;
using Core.MedicalOperational.Application.Services.Rules;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Test;

public class ClientAppointmentServiceTests
{
    [Fact]
    public async Task ScheduleAsync_CreatesScheduledAppointment_WhenSlotIsAvailable()
    {
        var service = CreateService();

        var response = await service.ScheduleAsync(new ScheduleClientAppointmentRequest
        {
            PatientId = 1,
            DoctorId = 1,
            ProcedureId = 1,
            RoomId = 1,
            StartDate = new DateTime(2026, 5, 20, 8, 0, 0),
            EndDate = new DateTime(2026, 5, 20, 8, 30, 0),
            Notes = "Paciente solicita confirmacion por correo."
        });

        Assert.Equal("Scheduled", response.Status);
        Assert.StartsWith("APT-", response.AppointmentCode);
        Assert.Equal(1, response.DoctorId);
        Assert.Equal(1, response.RoomId);
    }

    [Fact]
    public async Task ScheduleAsync_ThrowsBusinessRuleException_WhenDoctorHasConflict()
    {
        var service = CreateService(existingAssignments:
        [
            new ExistingAssignment
            {
                Id = 1,
                AppointmentId = 50,
                DoctorId = 1,
                RoomId = 2,
                StartDate = new DateTime(2026, 5, 20, 8, 0, 0),
                EndDate = new DateTime(2026, 5, 20, 8, 30, 0),
                Status = AssignmentStatus.Active
            }
        ]);

        await Assert.ThrowsAsync<BusinessRuleException>(() => service.ScheduleAsync(new ScheduleClientAppointmentRequest
        {
            PatientId = 1,
            DoctorId = 1,
            ProcedureId = 1,
            RoomId = 1,
            StartDate = new DateTime(2026, 5, 20, 8, 0, 0),
            EndDate = new DateTime(2026, 5, 20, 8, 30, 0)
        }));
    }

    [Fact]
    public async Task CancelAsync_ChangesStatusToCancelled_WhenPatientOwnsAppointment()
    {
        var appointmentRepository = new InMemoryMedicalAppointmentRepository(
        [
            new MedicalAppointment
            {
                Id = 101,
                AppointmentCode = "APT-20260520-101",
                PatientId = 1,
                ProcedureId = 1,
                StartDate = new DateTime(2026, 5, 20, 8, 0, 0),
                EndDate = new DateTime(2026, 5, 20, 8, 30, 0),
                Status = AppointmentStatus.Scheduled
            }
        ]);

        var assignmentRepository = new InMemoryExistingAssignmentRepository(
        [
            new ExistingAssignment
            {
                Id = 10,
                AppointmentId = 101,
                DoctorId = 1,
                RoomId = 1,
                StartDate = new DateTime(2026, 5, 20, 8, 0, 0),
                EndDate = new DateTime(2026, 5, 20, 8, 30, 0),
                Status = AssignmentStatus.Active
            }
        ]);

        var service = CreateService(appointmentRepository: appointmentRepository, existingAssignmentRepository: assignmentRepository);

        var response = await service.CancelAsync(101, new CancelClientAppointmentRequest
        {
            PatientId = 1,
            CancellationReason = "El paciente no podra asistir.",
            RequestedAt = new DateTime(2026, 5, 19, 16, 30, 0)
        });

        Assert.Equal("Scheduled", response.PreviousStatus);
        Assert.Equal("Cancelled", response.CurrentStatus);
        Assert.Equal(AppointmentStatus.Cancelled, (await appointmentRepository.GetByIdAsync(101))!.Status);
        Assert.Equal(AssignmentStatus.Cancelled, (await assignmentRepository.GetByIdAsync(10))!.Status);
    }

    [Fact]
    public async Task CancelAsync_ThrowsBusinessRuleException_WhenPatientDoesNotOwnAppointment()
    {
        var appointmentRepository = new InMemoryMedicalAppointmentRepository(
        [
            new MedicalAppointment
            {
                Id = 101,
                AppointmentCode = "APT-20260520-101",
                PatientId = 1,
                ProcedureId = 1,
                StartDate = new DateTime(2026, 5, 20, 8, 0, 0),
                EndDate = new DateTime(2026, 5, 20, 8, 30, 0),
                Status = AppointmentStatus.Scheduled
            }
        ]);

        var service = CreateService(appointmentRepository: appointmentRepository);

        await Assert.ThrowsAsync<BusinessRuleException>(() => service.CancelAsync(101, new CancelClientAppointmentRequest
        {
            PatientId = 2,
            CancellationReason = "Intento invalido.",
            RequestedAt = new DateTime(2026, 5, 19, 16, 30, 0)
        }));
    }

    private static IClientAppointmentService CreateService(
        IReadOnlyCollection<ExistingAssignment>? existingAssignments = null,
        InMemoryMedicalAppointmentRepository? appointmentRepository = null,
        InMemoryExistingAssignmentRepository? existingAssignmentRepository = null)
    {
        var patientRepository = new InMemoryPatientRepository(
        [
            new Patient
            {
                Id = 1,
                PatientCode = "PAT-001",
                FullName = "Paciente Uno",
                DocumentNumber = "123",
                BirthDate = new DateTime(1990, 1, 1),
                IsActive = true
            },
            new Patient
            {
                Id = 2,
                PatientCode = "PAT-002",
                FullName = "Paciente Dos",
                DocumentNumber = "456",
                BirthDate = new DateTime(1992, 1, 1),
                IsActive = true
            }
        ]);

        var doctorRepository = new InMemoryDoctorRepository(
        [
            new Doctor
            {
                Id = 1,
                DoctorCode = "DOC-001",
                FullName = "Dr. Andres Hidalgo",
                LicenseNumber = "CMP-001",
                IsActive = true
            }
        ]);

        var doctorSpecialtyRepository = new InMemoryDoctorSpecialtyRepository(
        [
            new DoctorSpecialty
            {
                Id = 1,
                DoctorId = 1,
                SpecialtyId = 10,
                IsMainSpecialty = true
            }
        ]);

        var procedureRepository = new InMemoryMedicalProcedureRepository(
        [
            new MedicalProcedure
            {
                Id = 1,
                ProcedureCode = "PROC-001",
                Name = "Radiografia de torax",
                RequiredSpecialtyId = 10,
                EstimatedDurationMinutes = 30,
                IsActive = true
            }
        ]);

        var roomRepository = new InMemoryMedicalRoomRepository(
        [
            new MedicalRoom
            {
                Id = 1,
                RoomCode = "ROOM-001",
                Name = "Sala de Rayos X 1",
                Status = RoomStatus.Available
            },
            new MedicalRoom
            {
                Id = 2,
                RoomCode = "ROOM-002",
                Name = "Sala de Rayos X 2",
                Status = RoomStatus.Available
            }
        ]);

        var medicalAppointmentRepository = appointmentRepository ?? new InMemoryMedicalAppointmentRepository([]);
        var assignmentRepository = existingAssignmentRepository ?? new InMemoryExistingAssignmentRepository(existingAssignments ?? []);

        var domainService = new ClientAppointmentDomainService(
            patientRepository,
            doctorRepository,
            procedureRepository,
            roomRepository,
            medicalAppointmentRepository,
            assignmentRepository);
        var unitOfWork = new InMemoryUnitOfWork();

        var rules = new IAppointmentAvailabilityRule[]
        {
            new DoctorSpecialtyAvailabilityRule(doctorSpecialtyRepository),
            new DoctorScheduleAvailabilityRule(),
            new RoomAvailabilityRule()
        };

        var availabilityService = new AppointmentAvailabilityService(domainService, rules);
        var schedulingService = new AppointmentSchedulingService(
            availabilityService,
            domainService,
            medicalAppointmentRepository,
            assignmentRepository,
            unitOfWork);
        var cancellationService = new AppointmentCancellationService(
            domainService,
            medicalAppointmentRepository,
            assignmentRepository,
            unitOfWork);

        return new ClientAppointmentService(
            availabilityService,
            schedulingService,
            cancellationService);
    }

    private class InMemoryBaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly List<T> _items;
        private int _nextId;

        public InMemoryBaseRepository(IReadOnlyCollection<T> seed)
        {
            _items = [.. seed];
            _nextId = _items.Count == 0 ? 1 : _items.Max(GetId) + 1;
        }

        public Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<T>>(_items.ToList());
        }

        public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(item => GetId(item) == id));
        }

        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (GetId(entity) == 0)
            {
                SetId(entity, _nextId++);
            }

            _items.Add(entity);
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var index = _items.FindIndex(item => GetId(item) == GetId(entity));

            if (index >= 0)
            {
                _items[index] = entity;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var index = _items.FindIndex(item => GetId(item) == id);

            if (index >= 0)
            {
                _items.RemoveAt(index);
            }

            return Task.CompletedTask;
        }
    }

    private sealed class InMemoryPatientRepository(IReadOnlyCollection<Patient> seed)
        : InMemoryBaseRepository<Patient>(seed), IPatientRepository;

    private sealed class InMemoryDoctorRepository(IReadOnlyCollection<Doctor> seed)
        : InMemoryBaseRepository<Doctor>(seed), IDoctorRepository;

    private sealed class InMemoryDoctorSpecialtyRepository(IReadOnlyCollection<DoctorSpecialty> seed)
        : InMemoryBaseRepository<DoctorSpecialty>(seed), IDoctorSpecialtyRepository;

    private sealed class InMemoryMedicalProcedureRepository(IReadOnlyCollection<MedicalProcedure> seed)
        : InMemoryBaseRepository<MedicalProcedure>(seed), IMedicalProcedureRepository;

    private sealed class InMemoryMedicalRoomRepository(IReadOnlyCollection<MedicalRoom> seed)
        : InMemoryBaseRepository<MedicalRoom>(seed), IMedicalRoomRepository;

    private sealed class InMemoryMedicalAppointmentRepository(IReadOnlyCollection<MedicalAppointment> seed)
        : InMemoryBaseRepository<MedicalAppointment>(seed), IMedicalAppointmentRepository;

    private sealed class InMemoryExistingAssignmentRepository(IReadOnlyCollection<ExistingAssignment> seed)
        : InMemoryBaseRepository<ExistingAssignment>(seed), IExistingAssignmentRepository;

    private sealed class InMemoryUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }

    private static int GetId<T>(T entity) where T : class
    {
        return entity switch
        {
            Patient item => item.Id,
            Doctor item => item.Id,
            DoctorSpecialty item => item.Id,
            MedicalProcedure item => item.Id,
            MedicalRoom item => item.Id,
            MedicalAppointment item => item.Id,
            ExistingAssignment item => item.Id,
            _ => throw new NotSupportedException(typeof(T).Name)
        };
    }

    private static void SetId<T>(T entity, int id) where T : class
    {
        switch (entity)
        {
            case Patient item:
                item.Id = id;
                break;
            case Doctor item:
                item.Id = id;
                break;
            case DoctorSpecialty item:
                item.Id = id;
                break;
            case MedicalProcedure item:
                item.Id = id;
                break;
            case MedicalRoom item:
                item.Id = id;
                break;
            case MedicalAppointment item:
                item.Id = id;
                break;
            case ExistingAssignment item:
                item.Id = id;
                break;
            default:
                throw new NotSupportedException(typeof(T).Name);
        }
    }
}
