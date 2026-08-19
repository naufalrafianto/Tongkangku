using System.ComponentModel.DataAnnotations.Schema;

namespace tongkangku_be.Models
{
    [Table("USER")]
    public class User
    {
        
        public Guid id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string pw { get; set; }
        public UserEnum role { get; set; }
        
    }
}
