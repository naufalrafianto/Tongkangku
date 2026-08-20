namespace tongkangku_be.Dtos
{
    public class RegisterResponseDto
    {
    
        public string name {  get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string role {  get; set; } = string.Empty;
        public Guid id { get; set; }


    }
}
