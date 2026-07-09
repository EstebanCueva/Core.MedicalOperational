# Core.MedicalOperational

API ASP.NET Core `net8.0` para operacion medica, agenda de citas y analisis de compatibilidad.

## Buenas practicas aplicadas

- `SRP`: la logica de agenda, cancelacion, disponibilidad y compatibilidad se separo en servicios pequenos.
- `OCP / Strategy`: las reglas de disponibilidad y compatibilidad viven en interfaces extensibles.
- `DIP`: los servicios dependen de abstracciones y no de EF Core directamente.
- `Repository + Unit of Work`: el acceso a datos se mantiene por repositorios y las escrituras se confirman con `IUnitOfWork`.
- `Facade / Application Service`: los controladores exponen servicios de aplicacion delgados.

## Arquitectura relevante

- Modulo cliente:
  - [ClientAppointmentService.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/ClientAppointmentService.cs)
  - [AppointmentAvailabilityService.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/AppointmentAvailabilityService.cs)
  - [AppointmentSchedulingService.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/AppointmentSchedulingService.cs)
  - [AppointmentCancellationService.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/AppointmentCancellationService.cs)
- Reglas cliente:
  - [DoctorSpecialtyAvailabilityRule.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/Rules/DoctorSpecialtyAvailabilityRule.cs)
  - [DoctorScheduleAvailabilityRule.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/Rules/DoctorScheduleAvailabilityRule.cs)
  - [RoomAvailabilityRule.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/Rules/RoomAvailabilityRule.cs)
- Compatibilidad operativa:
  - [OperationalCompatibilityService.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/OperationalCompatibilityService.cs)
  - [OperationalCompatibilityContextService.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/OperationalCompatibilityContextService.cs)
  - [OperationalCompatibilityPersistenceService.cs](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/OperationalCompatibilityPersistenceService.cs)
  - reglas en [Rules](C:/Users/SPELL/OneDrive/Desktop/Core.MedicalOperational/src/Core.MedicalOperational.Application/Services/Rules)

## Endpoints para frontend

### Login

`POST /api/auth/login`

```json
{
  "email": "client@demo.com",
  "password": "demo123"
}
```

### Disponibilidad

`POST /api/client-appointments/availability`

```json
{
  "patientId": 1,
  "doctorId": 1,
  "procedureId": 1,
  "requestedDate": "2026-05-20",
  "preferredStartTime": "08:00",
  "preferredEndTime": "12:00"
}
```

### Agendar cita

`POST /api/client-appointments/schedule`

```json
{
  "patientId": 1,
  "doctorId": 1,
  "procedureId": 1,
  "roomId": 1,
  "startDate": "2026-05-20T08:00:00",
  "endDate": "2026-05-20T08:30:00",
  "notes": "Paciente solicita confirmacion por correo."
}
```

### Cancelar cita propia

`PATCH /api/client-appointments/{id}/cancel`

```json
{
  "patientId": 1,
  "cancellationReason": "El paciente no podra asistir.",
  "requestedAt": "2026-05-19T16:30:00"
}
```

### Analisis de compatibilidad

`POST /api/operational-compatibility/analyze`

```json
{
  "analysisDate": "2026-05-20T00:00:00",
  "specialtyCode": null,
  "procedureCode": null,
  "includeOnlyCompatible": false,
  "saveResults": false
}
```

## Verificacion

- `dotnet build Core.MedicalOperational.sln`
- `dotnet test Core.MedicalOperational.sln`
