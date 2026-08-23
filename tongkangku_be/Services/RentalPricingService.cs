using Microsoft.EntityFrameworkCore;
using System.Net;
using tongkangku_be.Data;
using tongkangku_be.Interfaces;
using tongkangku_be.Models;
using tongkangku_be.Models.Enums;
using tongkangku_be.Shared;

namespace tongkangku_be.Services
{
    public class RentalPricingService(ApplicationDbContext context) : IRentalPricingService
    {
        private readonly ApplicationDbContext _context = context;

        public const decimal TaxRate = 0.012m;

        public async Task<PricingBreakdown> CalculateAsync(decimal ratePerDay, int planDay)
        {
            var setting = await GetActivePricingSettingAsync();

            if (setting.ContingencyRate < 0)
            {
                throw new AppException(
                    "Contingency rate cannot be negative.",
                    HttpStatusCode.InternalServerError,
                    "INVALID_CONTINGENCY_RATE"
                );
            }

            var operationalCosts = await GetActiveOperationalCostsAsync();

            var requiredCostTypes = new[]
            {
                CostType.Agency,
                CostType.Loading,
                CostType.Discharging,
                CostType.Other
            };

            var missingCostTypes = requiredCostTypes
                .Where(x => !operationalCosts.ContainsKey(x))
                .ToList();

            if (missingCostTypes.Count > 0)
            {
                throw new AppException(
                    $"Operational cost rate not configured for: {string.Join(", ", missingCostTypes)}",
                    HttpStatusCode.InternalServerError,
                    "COST_RATE_NOT_CONFIGURED"
                );
            }

            var durationMultiplier = GetDurationMultiplier(planDay, setting);

            if (durationMultiplier <= 0)
            {
                throw new AppException(
                    "Invalid duration multiplier.",
                    HttpStatusCode.InternalServerError,
                    "INVALID_DURATION_MULTIPLIER"
                );
            }

            var baseHirePrice = ratePerDay * planDay;
            var adjustedHirePrice = baseHirePrice * durationMultiplier;

            var operationalCost =
                operationalCosts[CostType.Agency] +
                operationalCosts[CostType.Loading] +
                operationalCosts[CostType.Discharging] +
                operationalCosts[CostType.Other];

            var contingencyCost = operationalCost * setting.ContingencyRate;

            var estimatedCost = adjustedHirePrice + operationalCost + contingencyCost;

            var taxAmount = estimatedCost * TaxRate;

            var grandTotal = estimatedCost + taxAmount;

            return new PricingBreakdown(
                durationMultiplier,
                baseHirePrice,
                adjustedHirePrice,
                operationalCost,
                contingencyCost,
                estimatedCost,
                taxAmount,
                grandTotal
            );
        }

        private async Task<RentalPricingSetting> GetActivePricingSettingAsync()
        {
            var setting = await _context.RentalPricingSettings
                .FirstOrDefaultAsync(x => x.IsActive);

            if (setting == null)
            {
                throw new AppException(
                    "Rental pricing setting is not configured.",
                    HttpStatusCode.InternalServerError,
                    "PRICING_NOT_CONFIGURED"
                );
            }

            return setting;
        }

        private async Task<Dictionary<CostType, decimal>> GetActiveOperationalCostsAsync()
        {
            return await _context.RentalOperationalCosts
                .Where(x => x.IsActive)
                .ToDictionaryAsync(x => x.CostType, x => x.Amount);
        }

        private static decimal GetDurationMultiplier(int planDay, RentalPricingSetting setting)
        {
            if (planDay < setting.ShortDurationMaxDays)
            {
                return setting.ShortDurationMultiplier;
            }

            if (planDay <= setting.MediumDurationMaxDays)
            {
                return setting.MediumDurationMultiplier;
            }

            return setting.LongDurationMultiplier;
        }
    }
}