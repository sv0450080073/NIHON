using SharedLibraries.UI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedLibraries.UI.Services
{
    public interface IKoboMenuService
    {
        Task<List<KoboMenuItemModel>> GetMenuItemsAsync(int serviceCdSeq, int syainCdSeq, int tenantCdSeq, string langCode);
        Task<string> GetCurrentCultureAsync(int syainCdSeq);
    }
}
