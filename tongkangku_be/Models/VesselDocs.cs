using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models;

namespace tongkangku_be.Models
{
    public class VesselDocs
    {
        public Guid id { get; set; }

       [Column("VESSEL_ID")]
        public Guid vesselid { get; set; }
        [ForeignKey(nameof(vesselid))]
        public Vessel? vessel { get; set; }
        public string documentType { get; set; }
        public string docsName { get; set; }
        public string docsNum { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime expiryDate { get; set; }
        public string fileUrl { get; set; }
       public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    }
}

