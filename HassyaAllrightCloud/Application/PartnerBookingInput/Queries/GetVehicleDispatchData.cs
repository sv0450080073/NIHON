using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetVehicleDispatchData : IRequest<List<VehicleDispatchPopup>>
    {
        public int TenantCdSeq { get; set; } = 0;
        public string Date { get; set; } = "";
        public class Handler : IRequestHandler<GetVehicleDispatchData, List<VehicleDispatchPopup>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<VehicleDispatchPopup>> Handle(GetVehicleDispatchData request, CancellationToken cancellationToken)
            {
                var result = new List<VehicleDispatchPopup>();
                try
                {
                    result = (from KOUTU in _context.VpmKoutu
                              join BUNRUI in _context.VpmCodeKb
                              on new { K1 = KOUTU.BunruiCdSeq, K2 = 1, K3 = request.TenantCdSeq }
                              equals new { K1 = BUNRUI.CodeKbnSeq, K2 = (int)BUNRUI.SiyoKbn, K3 = BUNRUI.TenantCdSeq }
                              into BUNRUI_join
                              from BUNRUI in BUNRUI_join.DefaultIfEmpty()
                              join BIN in _context.VpmBin
                              on new { K1 = KOUTU.BunruiCdSeq }
                              equals new { K1 = BIN.KoukCdSeq }
                              where KOUTU.SiyoKbn == 1
                              && String.Compare(BIN.SiyoStaYmd, "") <= 0
                              && String.Compare(BIN.SiyoStaYmd, "") >= 0
                              select new VehicleDispatchPopup()
                              {
                                  KOUTU_BunruiCdSeq= KOUTU.BunruiCdSeq,
                                  KOUTU_KoukCdSeq= KOUTU.KoukCdSeq,
                                  KOUTU_KoukCd= KOUTU.KoukCd,
                                  KOUTU_KoukNm= KOUTU.KoukNm,
                                  BUNRUI_CodeKbnNm= BUNRUI.CodeKbnNm,
                                  BIN_BinCdSeq= BIN.BinCdSeq,
                                  BIN_BinCd= BIN.BinCd,
                                  BIN_BinNm= BIN.BinNm,
                                  BIN_SyuPaTime= BIN.SyuPaTime,
                                  BIN_TouChTime= BIN.TouChTime
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