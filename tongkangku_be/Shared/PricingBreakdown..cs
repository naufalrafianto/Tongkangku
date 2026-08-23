namespace tongkangku_be.Shared
{
    public record PricingBreakdown(
        decimal DurationMultiplier,
        decimal BaseHirePrice,
        decimal AdjustedHirePrice,
        decimal OperationalCost,
        decimal ContingencyCost,
        decimal EstimatedCost,
        decimal TaxAmount,
        decimal GrandTotal
    );
}