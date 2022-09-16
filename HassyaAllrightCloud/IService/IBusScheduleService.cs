using HassyaAllrightCloud.Application.BusLine.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IBusScheduleService
    {
        Task<List<ResponseHaiTaCheck>> GetUpYmdTimes(List<ParamHaiTaCheck> dataParam);
    }
    public class BusScheduleService : IBusScheduleService
    {
        private readonly KobodbContext _context;
        private readonly IMediator _mediator;
        public BusScheduleService(KobodbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<List<ResponseHaiTaCheck>> GetUpYmdTimes(List<ParamHaiTaCheck> dataParam)
        {
            return await _mediator.Send(new GetUpYmdTimes { DataParam = dataParam });
        }
    }
}
