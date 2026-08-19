using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    public class RentalRequest
    {
        public Guid id { get; set; }
        [Column("VESSEL_ID")]
        public Guid vesselId { get; set; }
        [ForeignKey(nameof(vesselId))]
        public Vessel? vessel { get; set; }
        [Column("CHARTERE_ID")]
        public Guid chartereId { get; set; }
        [ForeignKey(nameof(chartereId))]
        public User? chartere { get; set; }

        public DateTime startDate { get; set; }
        public int planDay { get; set; }
        public decimal totalEstimatedPrice { get; set; }
        public RentalRequestStatus status { get; set; }
        public string notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
