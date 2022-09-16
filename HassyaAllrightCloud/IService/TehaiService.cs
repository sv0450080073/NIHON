using HassyaAllrightCloud.Application.Tehai.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITehaiService
    {
        Task<string> GetHaitaCheck(string ukeNo, short unkRen, short teiDanNo, short bunkRen);
    }

    public class TehaiService : ITehaiService
    {
        private readonly IMediator _mediator;
        public TehaiService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> GetHaitaCheck(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            return await _mediator.Send(new GetHaitaCheckQuery() { UkeNo = ukeNo, UnkRen = unkRen, TeiDanNo = teiDanNo, BunkRen = bunkRen });
        }
    }
}
