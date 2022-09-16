using HassyaAllrightCloud.Application.CommonComponents.Queries;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService.CommonComponents
{
    public interface IReservationClassComponentService
    {
        Task<List<ReservationClassComponentData>> GetListReservationClass();
    }
    public class ReservationClassComponentService : IReservationClassComponentService
    {
        private readonly IMediator _mediator;
        public ReservationClassComponentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<ReservationClassComponentData>> GetListReservationClass()
        {
            return await _mediator.Send(new GetListReservationClassQuery());
        }
    }
}
