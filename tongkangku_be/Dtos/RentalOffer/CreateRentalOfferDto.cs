namespace tongkangku_be.Dtos.RentalOffer
{
    public class CreateRentalOfferDto
    {
        public Guid RentalRequestId { get; set; }
        public decimal RatePerDay { get; set; }
        public decimal BunkerAmount { get; set; }
        public decimal OtherCharges { get; set; }
        public DateTime ValidUntil { get; set; }
        public string? Notes { get; set; }
    }
}
