namespace tongkangku_be.Dtos
{
    public class LoginResponseDto
    {
        public string status { get; set; } = "success";
        public string Token { get; set; } = string.Empty;
        public UserSummary User { get; set; } = new UserSummary();
    }
    public class UserSummary
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
