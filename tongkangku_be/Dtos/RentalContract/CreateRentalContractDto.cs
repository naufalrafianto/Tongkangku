namespace tongkangku_be.Dtos.RentalContract
{
    public class CreateRentalContractDto
    {
        public Guid OfferId { get; set; }

        public decimal? DemurrageRate { get; set; }
        public decimal? DespatchRate { get; set; }
    }
}
