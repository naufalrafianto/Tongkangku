namespace tongkangku_be.Dtos.AuthRequest
{
    public class CurrentUserResponseDto
    {
        public Guid id {  get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}
