using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Services.Rules;

public class EquipmentCompatibilityRule : IOperationalCompatibilityRule
{
    public Task ApplyAsync(
        OperationalCompatibilityCandidate candidate,
        OperationalCompatibilityEvaluation evaluation,
        CancellationToken cancellationToken = default)
    {
        foreach (var requiredEquipment in candidate.Context.RequiredEquipments)
        {
            if (requiredEquipment.ProcedureId != candidate.Procedure.Id)
            {
                continue;
            }

            var hasRequiredEquipmentInRoom = false;

            foreach (var roomEquipment in candidate.Context.RoomEquipments)
            {
                if (roomEquipment.RoomId == candidate.Room.Id &&
                    roomEquipment.EquipmentId == requiredEquipment.EquipmentId &&
                    roomEquipment.IsAvailable &&
                    roomEquipment.Quantity >= requiredEquipment.QuantityRequired)
                {
                    foreach (var equipment in candidate.Context.Equipments)
                    {
                        if (equipment.Id == roomEquipment.EquipmentId &&
                            equipment.Status == EquipmentStatus.Available)
                        {
                            hasRequiredEquipmentInRoom = true;
                            break;
                        }
                    }
                }

                if (hasRequiredEquipmentInRoom)
                {
                    break;
                }
            }

            if (!hasRequiredEquipmentInRoom)
            {
                AddUnique(evaluation.MissingEquipment, GetEquipmentName(requiredEquipment.EquipmentId, candidate.Context));
            }
        }

        return Task.CompletedTask;
    }

    private static string GetEquipmentName(int equipmentId, OperationalCompatibilityAnalysisContext context)
    {
        foreach (var equipment in context.Equipments)
        {
            if (equipment.Id == equipmentId)
            {
                return equipment.Name;
            }
        }

        return $"Equipment Id {equipmentId}";
    }

    private static void AddUnique(List<string> values, string value)
    {
        if (!values.Contains(value))
        {
            values.Add(value);
        }
    }
}
