using HassyaAllrightCloud.Application.HaitaCheck.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IHaitaCheckService
    {
        Task<bool> GetHaitaCheck(List<HaitaModelCheck> HaitaModelsToCheck);
    }
    public class HaitaCheckService : IHaitaCheckService
    {
        private IMediator mediatR;
        public HaitaCheckService(IMediator mediator)
        {
            this.mediatR = mediator;
        }

        public async Task<bool> GetHaitaCheck(List<HaitaModelCheck> HaitaModelsToCheck)
        {
            return await mediatR.Send(new GetHaitaCheckQuery { HaitaModelsToCheck = HaitaModelsToCheck });
        }
    }
}
