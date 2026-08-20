using tongkangku_be.Models.Enums;

namespace tongkangku_be.Dtos.RentalRequest
{
    public class RentalResponseDto
    {
        public Guid Id { get; set; }
        public Guid VesselId { get; set; }
        public string? VesselName { get; set; } = string.Empty;
        public Guid ChartererId{ get; set; }
        public string? ChartererName { get; set; } = string.Empty;
        public DateTime StartDate{ get; set; } 
        public int PlanDay{ get; set; }
        public decimal TotalEstimatedPrice{ get; set; }
        public RentalRequestStatus Status { get; set; }
        public string? RejectionReason { get; set; } = string.Empty;
        public string? Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class RentalStatusResponseDto
    {
        public Guid Id { get; set; }
        public RentalRequestStatus Status { get; set; }
    }
}