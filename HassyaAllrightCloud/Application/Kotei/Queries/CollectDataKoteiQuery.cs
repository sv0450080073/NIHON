using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.Kotei.Queries
{
    public class CollectDataKoteiQuery:IRequest<List<TkdKotei>>
    {
        public TKD_KoteikData TKD_KoteiFormData { get; set; }

        public class Handler : IRequestHandler<CollectDataKoteiQuery, List<TkdKotei>>
        {
            public async Task<List<TkdKotei>> Handle(CollectDataKoteiQuery request, CancellationToken cancellationToken)
            {
                List<TkdKotei> listKotei = new List<TkdKotei>();
                var journeydata = request.TKD_KoteiFormData.JourneyLst;
                foreach (var journeyitem in journeydata)
                {
                    TkdKotei Kotei = new TkdKotei();
                    Kotei.Koutei = journeyitem.Koutei;
                    Kotei.KouRen = journeyitem.KouRen;
                    Kotei.SiyoKbn = 1;
                    Kotei.HenKai = 0;
                    Kotei.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    Kotei.UpdTime = DateTime.Now.ToString("HHmmss");
                    Kotei.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    Kotei.UpdPrgId = "KJ1100";
                    listKotei.Add(Kotei);
                }

                return await Task.FromResult(listKotei);
            }
        }
    }
}
