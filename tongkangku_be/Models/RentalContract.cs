using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using tongkangku_be.Models.Enums;

namespace tongkangku_be.Models
{
    [Table("rental_contract")]
    public class RentalContract
    {
        [Key]

        public Guid Id { get; set; }
        public string ContractNum { get; set; } = string.Empty;
        public Guid RentalRequestId { get; set; }
        public Guid ChartererId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LaytimeHours { get; set; }
        public decimal DemurrageRate { get; set; }
        public decimal DespatchRate { get; set; }
        public decimal TotalPrice { get; set; }
        public RentalContractStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(RentalRequestId))]
        public RentalRequest? RentalRequest { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public User? Owner {get; set; }

        public ICollection<ContractCargo> ContractCargos { get; set; }
        = new List<ContractCargo>();
    }

}