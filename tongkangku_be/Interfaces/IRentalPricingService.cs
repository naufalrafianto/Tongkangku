using tongkangku_be.Shared;

namespace tongkangku_be.Interfaces
{
    public interface IRentalPricingService
    {
        Task<PricingBreakdown> CalculateAsync(decimal ratePerDay, int planDay);
    }
}