namespace tongkangku_be.Dtos.AuthRequest
{
    public class RegisterRequestDto
    {
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public int role { get; set; }

    }

}