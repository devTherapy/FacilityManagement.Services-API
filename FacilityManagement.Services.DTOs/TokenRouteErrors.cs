using FacilityManagement.Services.Models;
using System.Collections.Generic;

namespace FacilityManagement.Services.DTOs
{
    public class TokenRouteErrors
    {
        public Dictionary<User, string> UserAndPassword { get; set; } = new Dictionary<User, string>();

        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}