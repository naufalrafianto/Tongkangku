namespace tongkangku_be.Dtos.RentalRequest
{
    public class EstimateRentalDto
    {
        public Guid VesselId { get; set; }
        public DateTime StartDate { get; set; }
        public int PlanDay { get; set; }
    }
}
