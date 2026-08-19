using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("vessel_docs")]
    public class VesselDocs
    {
        [Key]
        public Guid Id { get; set; }

        public Guid VesselId { get; set; }

        [ForeignKey(nameof(VesselId))]
        public Vessel? Vessel { get; set; }

        public string? DocumentType { get; set; }

        public string? DocsName { get; set; }

        public string? DocsNum { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string? FileUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

