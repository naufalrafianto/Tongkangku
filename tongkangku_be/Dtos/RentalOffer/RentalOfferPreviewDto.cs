namespace tongkangku_be.Dtos.RentalOffer
{
    public class RentalOfferPreviewDto
    {
        public decimal RatePerDay { get; set; }
        public int PlanDay { get; set; }
        public decimal DurationMultiplier { get; set; }
        public decimal BaseHirePrice { get; set; }
        public decimal HireAmount { get; set; }
        public decimal OperationalCost { get; set; }
        public decimal ContingencyCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal BunkerAmount { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
