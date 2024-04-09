using FBOLinx.DB.Models;
using System.Threading.Tasks;

namespace FBOLinx.Web.Auth
{
    public interface IAPIKeyManager
    {
        Task<IntegrationPartners> GetIntegrationPartner();
    }
}
