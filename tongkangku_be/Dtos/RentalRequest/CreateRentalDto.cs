using tongkangku_be.Models.Enums;

namespace tongkangku_be.Dtos.RentalRequest
{
    public class CreateRentalDto
    {
        public Guid VesselId { get; set; }
        public CharterType CharterType { get; set; }
        public Guid LoadingPortId { get; set; }
        public Guid DischargingPortId { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public int PlanDay{ get; set; }
        public string Notes { get; set; } = string.Empty;

        public List<CreateRentalCargoDto> Cargos { get; set; }
        = new List<CreateRentalCargoDto>();
    }
}
