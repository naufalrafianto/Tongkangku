namespace tongkangku_be.Dtos.LaytimeRecord
{
    public class UpdateLaytimeRecordDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int LaytimeHours { get; set; }
        public string? Notes { get; set; }
    }
}
