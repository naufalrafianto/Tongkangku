namespace tongkangku_be.Dtos.RentalRequest
{
    public class RentalEstimateResponseDto
    {
        public Guid VesselId { get; set; }
        public string VesselName { get; set; } = string.Empty;
        public bool IsVesselAvailable { get; set; }

        public decimal RatePerDay { get; set; }
        public int PlanDay { get; set; }
        public decimal DurationMultiplier { get; set; }

        public decimal BaseHirePrice { get; set; }
        public decimal AdjustedHirePrice { get; set; }
        public decimal OperationalCost { get; set; }
        public decimal ContingencyCost { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal TargetMargin { get; set; }
        public decimal TotalEstimatedPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }

        public decimal GrandTotal { get; set; }

    }
}
