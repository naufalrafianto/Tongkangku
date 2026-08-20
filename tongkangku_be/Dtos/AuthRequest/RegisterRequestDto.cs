namespace tongkangku_be.Dtos.AuthRequest
{
    public class RegisterRequestDto
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int role { get; set; }

    }

}