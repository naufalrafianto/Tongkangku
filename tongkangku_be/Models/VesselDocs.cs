using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models;

namespace tongkangku_be.Models
{
    [Table("VesselDocs")]
    public class VesselDocs
    {
        [Key]
        public Guid id { get; set; }

        public Guid vesselId { get; set; }

        [ForeignKey(nameof(vesselId))]
        public Vessel? vessel { get; set; }

        public string? documentType { get; set; }

        public string? docsName { get; set; }

        public string? docsNum { get; set; }

        public DateTime? issueDate { get; set; }

        public DateTime? expiryDate { get; set; }

        public string? fileUrl { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
    }
}

