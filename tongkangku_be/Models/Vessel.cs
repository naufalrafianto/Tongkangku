using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models;

using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{

    [Table("Vessel")]
    public class Vessel
    {
        [Key]
        public Guid id { get; set; }

        public string name { get; set; } = string.Empty;

        public Guid ownerId { get; set; }

        [ForeignKey(nameof(ownerId))]
        public User? owner { get; set; }

        public Guid categoryId { get; set; }

        [ForeignKey(nameof(categoryId))]
        public VesselCategory? category { get; set; }

        public Guid portId { get; set; }

        [ForeignKey(nameof(portId))]
        public Port? port { get; set; }

        public int capacityFeed { get; set; }

        public int dwtCapacity { get; set; }

        public int year { get; set; }

        public decimal ratePerDay { get; set; }

        public VesselStatus status { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<VesselDocs> VesselDocs { get; set; } = new List<VesselDocs>();
        public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();
        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
    }

}



