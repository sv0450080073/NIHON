using HassyaAllrightCloud.Application.CommonComponents.Queries;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService.CommonComponents
{
    public interface ICustomerComponentService
    {
        Task<List<CustomerComponentGyosyaData>> GetListGyosya();
        Task<List<CustomerComponentTokiskData>> GetListTokisk(string strDate = "", string endDate = "");
        Task<List<CustomerComponentTokiStData>> GetListTokiSt(string strDate = "", string endDate = "");
    }

    public class CustomerComponentService : ICustomerComponentService
    {
        private readonly IMediator _mediator;
        public CustomerComponentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<CustomerComponentGyosyaData>> GetListGyosya()
        {
            return await _mediator.Send(new GetListGyosyaQuery());
        }

        public async Task<List<CustomerComponentTokiskData>> GetListTokisk(string strDate = "", string endDate = "")
        {
            return await _mediator.Send(new GetListTokiskQuery { strDate = strDate , endDate  = endDate});
        }

        public async Task<List<CustomerComponentTokiStData>> GetListTokiSt(string strDate = "", string endDate = "")
        {
            return await _mediator.Send(new GetListTokiStQuery { strDate = strDate, endDate = endDate });
        }
    }
}
