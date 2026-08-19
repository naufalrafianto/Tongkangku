using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    public class LaytimeRecord
   [Table("LaytimeRecord")]
    public class LaytimeRecord
    {
        [Key]
        public Guid id { get; set; }

        public Guid contractId { get; set; }

        [ForeignKey(nameof(contractId))]
        public RentalContract? contract { get; set; }

        public OperationType operationType { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public int laytimeHours { get; set; }

        public int actualDurationHours { get; set; }

        public int overtimeHours { get; set; }

        public int savedHours { get; set; }

        public decimal demurrageRate { get; set; }

        public decimal demurrageAmount { get; set; }

        public decimal despatchRate { get; set; }

        public decimal despatchAmount { get; set; }

        public string? notes { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
    }
}
