namespace FacilityManagement.Services.DTOs
{
    public class LoginResponseDTO
    {
        public string UserId { get; set; }
        public bool IsProfileCompleted { get; set; }
        public string Token { get; set; }
    }
}
