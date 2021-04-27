using System;
using System.Collections.Generic;
using System.Text;

namespace FacilityManagement.Services.DTOs
{
    public class ReplyDTO
    {
        public string Id { get; set; }
        public string Reply { get; set; }
        public string UserId { get; set; }
        public string CommentId { get; set; }
    }
}
