using System.Collections.Generic;

namespace FacilityManagement.Services.DTOs
{
    public class ComplaintCommentsDTO
    {
        public string Comment { get; set; }
        public ICollection<ComplaintsReplyDTO> Replies { get; set; }
        public ComplaintUserDTO User { get; set; }
    }
}
