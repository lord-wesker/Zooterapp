using System.Threading.Tasks;
using Zooterapp.Common.Models;

namespace Zooterapp.Common.Services
{
    public interface IApiService
    {
        Task<Response<PetOwnerResponse>> GetPetOwnerByEmailAsync(
            string urlBase, 
            string servicePrefix, 
            string controller, 
            string tokenType, 
            string accessToken, 
            string email);

        Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase, 
            string servicePrefix, 
            string controller, 
            TokenRequest 
            request);

        Task<bool> CheckConnectionAsync(string url);
    }
}