namespace FacilityManagement.Services.DTOs
{
    public class ComplaintsReplyDTO
    {
        public string Reply { get; set; }
        public string UserId { get; set; }

        public ComplaintUserDTO User { get; set; }
    }
}
