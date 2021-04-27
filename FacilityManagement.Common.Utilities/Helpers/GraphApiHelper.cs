using System.Net.Http;
using System.Net.Http.Headers;

namespace FacilityManagement.Common.Utilities.Helpers
{
    public class GraphApiHelper
    {
        public HttpClient ApiClient { get; set; }

        public GraphApiHelper(string token)
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
