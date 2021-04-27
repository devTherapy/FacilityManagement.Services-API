using System.Collections.Generic;

namespace FacilityManagement.Services.DTOs.ManualMappers
{
    public static class ResponseMessageMapper
    {
        public static List<InviteResponseDTO> MapToResponseMsg(Dictionary<string, string> data)
        {
            var response = new List<InviteResponseDTO>();
            foreach (var item in data)
            {
                response.Add(new InviteResponseDTO { Email = item.Key, message = item.Value });
            }

            return response;
        }
    }
}