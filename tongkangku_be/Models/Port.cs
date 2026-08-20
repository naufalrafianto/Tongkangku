using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("ports")]
    public class Port
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? City { get; set; }

        public string? Province { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Vessel> Vessels { get; set; }
            = new List<Vessel>();

        public ICollection<RentalRequest> LoadingRentalRequests { get; set; }
            = new List<RentalRequest>();
 
        public ICollection<RentalRequest> DischargingRentalRequests { get; set; }
            = new List<RentalRequest>();

    }
}
