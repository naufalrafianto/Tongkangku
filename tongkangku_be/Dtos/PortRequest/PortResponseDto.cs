namespace tongkangku_be.Dtos.PortRequest
{
    public class PortResponseDto
    {
        public Guid Id { get; set; }
        public  string Name { get; set; } = string.Empty;
         public string? City { get; set; }
        public string? Province { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
