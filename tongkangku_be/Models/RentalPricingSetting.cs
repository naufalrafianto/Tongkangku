using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("rental_pricing_settings")]
    public class RentalPricingSetting
    {
        public Guid Id { get; set; }

        public decimal ContingencyRate { get; set; }
        public decimal TargetMargin { get; set; }

        public int ShortDurationMaxDays { get; set; }
        public decimal ShortDurationMultiplier { get; set; }

        public int MediumDurationMaxDays { get; set; }
        public decimal MediumDurationMultiplier { get; set; }

        public decimal LongDurationMultiplier { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
