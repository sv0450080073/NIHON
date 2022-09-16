using HassyaAllrightCloud.Application.CustomItem.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ICustomItemService
    {
        Task<List<CustomFieldConfigs>> GetCustomFieldConfigs(int tenant);
    }
    public class CustomItemService : ICustomItemService
    {
        private IMediator mediatR;
        public CustomItemService(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }
        public async Task<List<CustomFieldConfigs>> GetCustomFieldConfigs(int tenant)
        {
            return await mediatR.Send(new GetCustomItemQuery(tenant));
        }
    }
}
