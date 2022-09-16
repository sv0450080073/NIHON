using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetCodeBunruiData : IRequest<List<CodeKbnBunruiDataPopup>>
    {
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetCodeBunruiData, List<CodeKbnBunruiDataPopup>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService; 
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }
            public async Task<List<CodeKbnBunruiDataPopup>> Handle(GetCodeBunruiData request, CancellationToken cancellationToken)
            {
                var result = new List<CodeKbnBunruiDataPopup>();
                try
                {
                    string codeSyuBUNRUICD = "BUNRUICD";
                    int tenantBUNRUICD = await _codeSyuService.CheckTenantByKanriKbnAsync(request.TenantCdSeq, codeSyuBUNRUICD);
                    result = (from VPM_Haichi in _context.VpmHaichi
                              join VPM_CodeKb in _context.VpmCodeKb
                              on new {C1 = VPM_Haichi.BunruiCdSeq, C2 = 1, C3= tenantBUNRUICD }
                              equals new { C1 = VPM_CodeKb.CodeKbnSeq, C2 =(int)VPM_CodeKb.SiyoKbn, C3 = VPM_CodeKb.TenantCdSeq }
                              into VPM_CodeKb_join
                              from VPM_CodeKb in VPM_CodeKb_join.DefaultIfEmpty()
                              where VPM_Haichi.SiyoKbn ==1 && VPM_CodeKb.CodeSyu == codeSyuBUNRUICD
                              select new CodeKbnBunruiDataPopup()
                              {
                                  HAICHI_BunruiCdSeq= VPM_Haichi.BunruiCdSeq,
                                  HAICHI_HaiSCdSeq = VPM_Haichi.HaiScdSeq,
                                  HAICHI_HaiSCd= VPM_Haichi.HaiScd,
                                  HAICHI_RyakuNm= VPM_Haichi.RyakuNm,
                                  HAICHI_Jyus1= VPM_Haichi.Jyus1,
                                  HAICHI_Jyus2 = VPM_Haichi.Jyus2,
                                  HAICHI_HaiSKigou= VPM_Haichi.HaiSkigou,
                                  BUNRUI_CodeKbnNm= VPM_CodeKb.CodeKbnNm
                              }).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
