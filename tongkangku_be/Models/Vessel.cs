using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models;

namespace tongkangku_be.Models
{

    public class Vessel
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        [Column("VESSEL_OWNER_ID")]
        public Guid OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User? Owner { get; set; }
        [Column("VESSEL_CATEGORY_ID")]
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public VesselCategory Category { get; set; }
        [Column("VESSEL_PORT_ID")]
        public Guid PortId { get; set; }
        [ForeignKey(nameof(PortId))]
        public Port? Port { get; set; }

        public int capacityFeed { get; set; }
        public int dwtCapacity { get; set; }
        public int year { get; set; }
        public decimal ratePerDay { get; set; }
        public VesselStatus status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public ICollection<VesselDocs> VesselDocs { get; set; } = new List<VesselDocs>();
    }

}



