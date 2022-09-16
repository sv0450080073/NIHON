using HassyaAllrightCloud.Application.BookingIncidental.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IFutaiService
    {
        Task<MaxUpdYmdTime> FutTumMishumMaxUpdYmdTimeAsync(string ukeNo, IncidentalViewMode mode);
    }

    public class FutaiService : IFutaiService
    {
        private readonly IMediator _mediator;
        public FutaiService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<MaxUpdYmdTime> FutTumMishumMaxUpdYmdTimeAsync(string ukeNo, IncidentalViewMode mode)
        {
            return await _mediator.Send(new FutTumMishumMaxUpdYmdTime(ukeNo, mode));
        }
    }
}
