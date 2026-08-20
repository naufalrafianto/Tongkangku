namespace tongkangku_be.Dtos.RentalRequest
{
    public class CreateRentalCargoDto
    {
        public Guid CargoTypeId { get; set; }

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = "MT";
    }
}
