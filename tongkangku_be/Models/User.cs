using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [InverseProperty(nameof(Vessel.Owner))]
        public ICollection<Vessel> Vessels { get; set; } = new List<Vessel>();

        [InverseProperty(nameof(RentalRequest.Charterer))]
        public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();

        [InverseProperty(nameof(RentalContract.Owner))]
        public ICollection<RentalContract> OwnerContracts { get; set; } = new List<RentalContract>();

    }
}
