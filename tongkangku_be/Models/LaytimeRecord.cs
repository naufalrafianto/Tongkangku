using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    [Table("laytime_record")]
    public class LaytimeRecord
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ContractId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int LaytimeHours { get; set; }
        public int ActualDurationHours { get; set; }

        public int OvertimeHours { get; set; }
        public int SavedHours { get; set; }

        public decimal DemurrageRate { get; set; }
        public decimal DemurrageAmount { get; set; }

        public decimal DespatchRate { get; set; }
        public decimal DespatchAmount { get; set; }

        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ContractId))]
        public RentalContract? Contract { get; set; }
    }
}