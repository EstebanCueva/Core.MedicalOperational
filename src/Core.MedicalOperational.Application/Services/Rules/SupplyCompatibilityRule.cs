using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Services;

namespace Core.MedicalOperational.Application.Services.Rules;

public class SupplyCompatibilityRule : IOperationalCompatibilityRule
{
    public Task ApplyAsync(
        OperationalCompatibilityCandidate candidate,
        OperationalCompatibilityEvaluation evaluation,
        CancellationToken cancellationToken = default)
    {
        foreach (var requiredSupply in candidate.Context.RequiredSupplies)
        {
            if (requiredSupply.ProcedureId != candidate.Procedure.Id)
            {
                continue;
            }

            var hasEnoughStock = false;

            foreach (var stock in candidate.Context.Stocks)
            {
                if (stock.SupplyId == requiredSupply.SupplyId &&
                    stock.AvailableQuantity >= requiredSupply.QuantityRequired)
                {
                    hasEnoughStock = true;
                    break;
                }
            }

            if (!hasEnoughStock)
            {
                AddUnique(evaluation.MissingSupplies, GetSupplyName(requiredSupply.SupplyId, candidate.Context));
            }
        }

        return Task.CompletedTask;
    }

    private static string GetSupplyName(int supplyId, OperationalCompatibilityAnalysisContext context)
    {
        foreach (var supply in context.Supplies)
        {
            if (supply.Id == supplyId)
            {
                return supply.Name;
            }
        }

        return $"Supply Id {supplyId}";
    }

    private static void AddUnique(List<string> values, string value)
    {
        if (!values.Contains(value))
        {
            values.Add(value);
        }
    }
}
