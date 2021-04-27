using System.Collections.Generic;

namespace FacilityManagement.Services.DTOs
{
    public class EmailCheck
    {
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();

        public ICollection<string> Emails { get; set; } = new List<string>();
    }
}