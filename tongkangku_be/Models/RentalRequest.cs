using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    [Table("RentalRequest")]
    public class RentalRequest
    {
        [Key]
        public Guid id { get; set; }

        public Guid vesselId { get; set; }

        [ForeignKey(nameof(vesselId))]
        public Vessel? vessel { get; set; }

        public Guid chartererId { get; set; }

        [ForeignKey(nameof(chartererId))]
        public User? charterer { get; set; }

        public DateTime startDate { get; set; }

        public int planDay { get; set; }

        public decimal totalEstimatedPrice { get; set; }

        public RentalRequestStatus status { get; set; }

        public string? notes { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

        public RentalContract? RentalContract { get; set; }
    }
}
