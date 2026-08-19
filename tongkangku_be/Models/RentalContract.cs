using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("RentalContract")]
    public class RentalContract
    {
        [Key]
        public Guid id { get; set; }

        public string contractNum { get; set; } = string.Empty;

        public Guid vesselId { get; set; }

        [ForeignKey(nameof(vesselId))]
        public Vessel? vessel { get; set; }

        public Guid ownerId { get; set; }

        [ForeignKey(nameof(ownerId))]
        public User? owner { get; set; }

        public Guid chartererId { get; set; }

        [ForeignKey(nameof(chartererId))]
        public User? charterer { get; set; }

        public Guid rentalRequestId { get; set; }

        [ForeignKey(nameof(rentalRequestId))]
        public RentalRequest? rentalRequest { get; set; }

        public DateTime startDate { get; set; }

        public DateTime plannedEndDate { get; set; }

        public DateTime actualEndDate { get; set; }

        public int planDay { get; set; }

        public int laytimeHour { get; set; }

        public decimal demurrageRate { get; set; }

        public decimal despatchRate { get; set; }

        public decimal totalPrice { get; set; }

        public ContractStatus status { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<LaytimeRecord> LaytimeRecords { get; set; } = new List<LaytimeRecord>();
    }
}