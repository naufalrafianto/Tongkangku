namespace tongkangku_be.Models
{
    public class LaytimeRecord
    {
        public Guid id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int LayTimeHours { get; set; }
        public int actualDurationHours { get; set; }
        public int overTimeHours { get; set; }
        public int savedHours { get; set; }
        public decimal demurrageRate { get; set; }
        public decimal demurrageAmount { get; set; }
        public decimal despatchRate { get; set; }
        public decimal despatchAmount { get; set; } 
        public string notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;


    }
}
