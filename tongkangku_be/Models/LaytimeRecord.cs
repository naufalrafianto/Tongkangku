using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("laytime_record")]
    public class LaytimeRecord
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int LayTimeHours { get; set; }
        public int ActualDurationHours { get; set; }
        public int OverTimeHours { get; set; }
        public int SavedHours { get; set; }
        public decimal DemurrageRate { get; set; }
        public decimal DemurrageAmount { get; set; }
        public decimal DespatchRate { get; set; }
        public decimal DespatchAmount { get; set; } 
        public decimal TotalPrice { get; set; } 
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;


    }
}
