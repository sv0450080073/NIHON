using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.YoushaNotice.Queries
{
    public class CollectDataYoushaNoticeQuery : IRequest<TkdYoushaNotice>
    {
        public CarCooperationData carCooperationData { get; set; }
        public class Handler : IRequestHandler<CollectDataYoushaNoticeQuery, TkdYoushaNotice>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<TkdYoushaNotice> Handle(CollectDataYoushaNoticeQuery request, CancellationToken cancellationToken)
            {
                TkdYoushaNotice youshaNotice = new TkdYoushaNotice();
                youshaNotice.MotoTenantCdSeq = new ClaimModel().TenantID;
                youshaNotice.MotoUkeNo = request.carCooperationData.UkeNo;
                youshaNotice.MotoUnkRen = request.carCooperationData.UnkRen;
                youshaNotice.MotoYouTblSeq = request.carCooperationData.YouTblSeq;
                youshaNotice.HaiSYmd = request.carCooperationData.MinDate;
                youshaNotice.HaiSTime = request.carCooperationData.MinTime;
                youshaNotice.TouYmd = request.carCooperationData.MaxDate;
                youshaNotice.TouChTime = request.carCooperationData.MaxTime;
                youshaNotice.BigtypeNum=(short)request.carCooperationData.LargeCount;
                youshaNotice.MediumtypeNum=(short)request.carCooperationData.MediumCount;
                youshaNotice.SmalltypeNum=(short)request.carCooperationData.SmallCount;
                youshaNotice.DanTaNm = request.carCooperationData.DanTaNm;
                youshaNotice.UkeTenantCdSeq = request.carCooperationData.UkeTenantCdSeq;
                youshaNotice.MotoTokuiSeq = request.carCooperationData.MotoTokuiSeq;
                youshaNotice.MotoSitenCdSeq = request.carCooperationData.MotoSitenCdSeq;
                youshaNotice.UnReadKbn = 1;
                youshaNotice.RegiterKbn = 1;
                youshaNotice.SiyoKbn = 1;
                youshaNotice.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                youshaNotice.UpdTime = DateTime.Now.ToString("HHmmss");
                youshaNotice.UpdSyainCd = new ClaimModel().SyainCdSeq;
                youshaNotice.UpdPrgId = "KU2100";
                return await Task.FromResult(youshaNotice);
            }
        }
    }
}
