namespace tongkangku_be.Models
{
    public class Port
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
