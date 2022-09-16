using HassyaAllrightCloud.Application.ETCForm.Queries;
using HassyaAllrightCloud.Application.TransportActualResult.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IETCFormService
    {
        Task<List<RyokinDataItem>> GetRyoKin();
    }
    public class ETCFormService : IETCFormService
    {
        private readonly IMediator _mediator;
        public ETCFormService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<RyokinDataItem>> GetRyoKin()
        {
            return await _mediator.Send(new GetRyoKin());
        }
    }
}
