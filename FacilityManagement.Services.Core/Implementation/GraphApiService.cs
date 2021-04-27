using FacilityManagement.Common.Utilities.Helpers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class GraphApiService : IGraphApiService
    {
        public async Task<GraphApiResponseDTO> GetAuthorizedUserDetails(string token)
        {
            GraphApiResponseDTO graphApiResponse = new GraphApiResponseDTO();
            var client = new GraphApiHelper(token);
            string url = "https://graph.microsoft.com/v1.0/me";

            using HttpResponseMessage response = await client.ApiClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                ExternalLoginSsoDTO user = await JsonSerializer.DeserializeAsync<ExternalLoginSsoDTO>(await response.Content.ReadAsStreamAsync());

                graphApiResponse.externalLoginUser = user;
                return graphApiResponse;
            }
            else
            {
                graphApiResponse.Error = response.ReasonPhrase;
                return graphApiResponse;
            }
        }
    }
}
