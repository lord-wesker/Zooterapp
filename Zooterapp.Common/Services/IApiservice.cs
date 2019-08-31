using System.Threading.Tasks;
using Zooterapp.Common.Models;

namespace Zooterapp.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetOwnerByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, string email);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
    }
}