using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("CargoType")]
    public class CargoType
    {
        [Key]
        public Guid id { get; set; }

        public string name { get; set; } = string.Empty;

        public string? description { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ContractCargo> ContractCargos { get; set; } = new List<ContractCargo>();
    }
}