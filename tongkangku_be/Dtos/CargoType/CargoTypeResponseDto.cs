namespace tongkangku_be.Dtos.CargoType
{
    public class CargoTypeResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
