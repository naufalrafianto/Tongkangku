using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;using tongkangku_be.Models;

namespace tongkangku_be.Models
{

    [Table("vessels")]
    public class Vessel
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User? Owner { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public VesselCategory? Category { get; set; }  
        public Guid PortId { get; set; }
        [ForeignKey(nameof(PortId))]
        public Port? Port { get; set; }

        public int CapacityFeed { get; set; }
        public int DwtCapacity { get; set; }
        public int Year { get; set; }
        public decimal RatePerDay { get; set; }
        public VesselStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        public ICollection<VesselDocs> VesselDocs { get; set; } = new List<VesselDocs>();
        public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();
    }

}



