using SharedLibraries.UI.Models;
using SharedLibraries.UI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using HassyaAllrightCloud.Application.SyainCulture.Queries;
using MediatR;
using HassyaAllrightCloud.Application.KoboMenu.Queries;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.IService
{
    public class MenuService : IKoboMenuService
    {
        private readonly IMediator _mediator;
        public MenuService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> GetCurrentCultureAsync(int syainCdSeq)
        {
            return await _mediator.Send(new GetSyainCultureQuery(new ClaimModel().SyainCdSeq));
        }

        public async Task<List<KoboMenuItemModel>> GetMenuItemsAsync(int serviceCdSeq, int syainCdSeq, int tenantCdSeq, string langCode)
        {
            return await _mediator.Send(new GetMenuItems(syainCdSeq, tenantCdSeq, serviceCdSeq, langCode));
        }


    }
}
