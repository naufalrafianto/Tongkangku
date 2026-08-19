namespace tongkangku_be.Dtos
{
    public class CreateRentalDto
    {
        public Guid VesselId { get; set; }
        public Guid ChartererId { get; set; }
        public DateTime StartDate{ get; set; }
        public int PlanDay{ get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
