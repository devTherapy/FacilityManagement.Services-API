using System;
using System.Collections.Generic;
using System.Text;

namespace FacilityManagement.Services.DTOs
{
    public class GraphApiResponseDTO
    {
        public ExternalLoginSsoDTO externalLoginUser { get; set; }
        public string Error { get; set; }
    }
}
