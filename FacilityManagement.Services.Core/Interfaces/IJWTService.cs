using FacilityManagement.Services.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IJWTService
    {
        string GetToken(User user, IConfiguration config, IList<string> userRoles);
    };
}
