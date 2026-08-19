using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("Port")]
    public class Port
    {
        [Key]
        public Guid id { get; set; }

        public string name { get; set; } = string.Empty;

        public string? city { get; set; }

        public string? province { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
