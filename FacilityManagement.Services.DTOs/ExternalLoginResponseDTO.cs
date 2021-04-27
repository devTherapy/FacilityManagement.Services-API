namespace FacilityManagement.Services.DTOs
{
    public class ExternalLoginResponseDTO
    {
        public string Id { get; set; }
        public bool IsProfileCompleted { get; set; }
        public string Token { get; set; }
    }
}
