using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid id { get; set; }

        public string name { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;

        public UserRole role { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

        [InverseProperty(nameof(Vessel.owner))]
        public ICollection<Vessel> Vessels { get; set; } = new List<Vessel>();

        [InverseProperty(nameof(RentalRequest.charterer))]
        public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();

        [InverseProperty(nameof(RentalContract.owner))]
        public ICollection<RentalContract> OwnerContracts { get; set; } = new List<RentalContract>();

        [InverseProperty(nameof(RentalContract.charterer))]
        public ICollection<RentalContract> ChartererContracts { get; set; } = new List<RentalContract>();
    }
}
