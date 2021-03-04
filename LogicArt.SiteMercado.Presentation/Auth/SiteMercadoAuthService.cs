using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace LogicArt.SiteMercado.Presentation.Auth
{
    public class SiteMercadoAuthService
    {
        public async Task<SiteMercadoAuthResponse> AuthenticateAsync(string username, string password)
        {
            const string endpoint = "https://dev.sitemercado.com.br/api/login";
            using var httpClient = new HttpClient();
            httpClient.SetBasicAuthentication(username, password);
            var response = await httpClient.PostAsync(endpoint, null);
            return await response.Content.ReadFromJsonAsync<SiteMercadoAuthResponse>();
        }
    }
}
