using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Domain.Dto.BusAllocationData;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetBusAllocationDataGridQuery : IRequest<List<BusAllocationDataGrid>>
    {
        public BusAllocationSearch BusAllocationSearch { get; set; }
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetBusAllocationDataGridQuery, List<BusAllocationDataGrid>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }
            public async Task<List<BusAllocationDataGrid>> Handle(GetBusAllocationDataGridQuery request, CancellationToken cancellationToken)
            {
                var haiShaParam = request.BusAllocationSearch;
                var result = new List<BusAllocationDataGrid>();
                List<byte> YoyaKbnlst = new List<byte>();
                foreach(var item in request.BusAllocationSearch.BookingTypes)
                {
                    YoyaKbnlst.Add((byte)item.YoyaKbn);
                }    
                int checkBookingParam = 0;
                bool checkParse = int.TryParse(haiShaParam.bookingParam, out checkBookingParam);
                var ukeno = request.TenantCdSeq.ToString("D5") + checkBookingParam.ToString("D10");
                var haiSYmdUnkobi = new List<string>();
                haiSYmdUnkobi = _context.TkdUnkobi.Where(x => x.HaiSymd == haiShaParam.pickupDate.ToString("yyyyMMdd")
                && (checkBookingParam > 0 ? x.UkeNo == ukeno : x.UkeNo !=null )).Select(x=>x.HaiSymd).ToList();
                int tenantIdOTHJINKBN = await _codeSyuService.CheckTenantByKanriKbnAsync(request.TenantCdSeq, "OTHJINKBN");
                try
                {
                    result = (from HAISHA in _context.TkdHaisha
                              join YYKSHO in _context.TkdYyksho
                              on new { H1 = HAISHA.UkeNo, H2 = 1, H3 = 1, H4 = request.TenantCdSeq }
                              equals new { H1 = YYKSHO.UkeNo, H2 = (int)YYKSHO.YoyaSyu, H3 = (int)YYKSHO.SiyoKbn, H4 = YYKSHO.TenantCdSeq }
                              join UNKOBI in _context.TkdUnkobi
                              on new { H1 = HAISHA.UkeNo, H2 = HAISHA.UnkRen, H3 = 1 }
                              equals new { H1 = UNKOBI.UkeNo, H2 = UNKOBI.UnkRen, H3 = (int)UNKOBI.SiyoKbn }
                              into UNKOBI_join
                              from UNKOBI in UNKOBI_join.DefaultIfEmpty()                              
                              join EIGYOS in _context.VpmEigyos
                              on HAISHA.SyuEigCdSeq equals EIGYOS.EigyoCdSeq
                              into EIGYOS_join
                              from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                              join KAISHA in _context.VpmCompny
                              on new { H1 = EIGYOS.CompanyCdSeq, H2 = request.TenantCdSeq }
                              equals new { H1 = KAISHA.CompanyCdSeq, H2 = KAISHA.TenantCdSeq }
                              into KAISHA_join
                              from KAISHA in KAISHA_join.DefaultIfEmpty()
                              join SYARYO in _context.VpmSyaRyo
                              on HAISHA.HaiSsryCdSeq equals SYARYO.SyaRyoCdSeq
                              into SYARYO_join
                              from SYARYO in SYARYO_join.DefaultIfEmpty()
                              join SYASYU in _context.VpmSyaSyu
                              on new { H1 = SYARYO.SyaSyuCdSeq, H2 = request.TenantCdSeq }
                              equals new { H1 = SYASYU.SyaSyuCdSeq, H2 = SYASYU.TenantCdSeq }
                              into SYASYU_join
                              from SYASYU in SYASYU_join.DefaultIfEmpty()
                              join OTHERJIN1 in _context.VpmCodeKb
                              on new { H1 = HAISHA.OthJinKbn1.ToString(), H2 = "OTHJINKBN", H3 = tenantIdOTHJINKBN }
                              equals new { H1 = OTHERJIN1.CodeKbn, H2 = OTHERJIN1.CodeSyu, H3 = OTHERJIN1.TenantCdSeq }
                              into OTHERJIN1_join
                              from OTHERJIN1 in OTHERJIN1_join.DefaultIfEmpty()
                              join OTHERJIN2 in _context.VpmCodeKb
                              on new { H1 = HAISHA.OthJinKbn2.ToString(), H2 = "OTHJINKBN", H3 = tenantIdOTHJINKBN }
                              equals new { H1 = OTHERJIN2.CodeKbn, H2 = OTHERJIN2.CodeSyu, H3 = OTHERJIN2.TenantCdSeq }
                              into OTHERJIN2_join
                              from OTHERJIN2 in OTHERJIN2_join.DefaultIfEmpty()
                              join TOKISK in _context.VpmTokisk
                              on new { H1 = YYKSHO.TokuiSeq, H2 = request.TenantCdSeq }
                              equals new { H1 = TOKISK.TokuiSeq, H2 = TOKISK.TenantCdSeq }
                              into TOKISK_join
                              from TOKISK in TOKISK_join.DefaultIfEmpty()
                              join TOKIST in _context.VpmTokiSt
                              on new { H1 = YYKSHO.TokuiSeq, H2 = YYKSHO.SitenCdSeq }
                              equals new { H1 = TOKIST.TokuiSeq, H2 = TOKIST.SitenCdSeq }
                              into TOKIST_join
                              from TOKIST in TOKIST_join.DefaultIfEmpty()
                              join HENSYA in _context.VpmHenSya
                              on new { H1 = HAISHA.HaiSsryCdSeq }
                              equals new { H1 = HENSYA.SyaRyoCdSeq }
                              into HENSYA_join
                              from HENSYA in HENSYA_join.DefaultIfEmpty()
                              join YOYKBN in _context.VpmYoyKbn
                              on new { H1 = YYKSHO.YoyaKbnSeq,  H2 = 1, H3 = YYKSHO.TenantCdSeq}
                              equals new { H1 = YOYKBN.YoyaKbnSeq, H2 = (int)YOYKBN.SiyoKbn, H3 = YOYKBN.TenantCdSeq }
                              into YOYKBN_join
                              from YOYKBN in YOYKBN_join.DefaultIfEmpty()
                              where (haiShaParam.BranchChart.EigyoCdSeq==0? HAISHA.SyuEigCdSeq!=null || HAISHA.SyuEigCdSeq == null :
                              HAISHA.SyuEigCdSeq == haiShaParam.BranchChart.EigyoCdSeq )
                              && HAISHA.SiyoKbn == 1
                              && (checkBookingParam >0 ? YYKSHO.UkeCd == checkBookingParam : YYKSHO.UkeCd !=null)
                              && (YoyaKbnlst.Count==0 ? YOYKBN.YoyaKbn != null:
                              YoyaKbnlst.Contains(YOYKBN.YoyaKbn))
                              && (haiShaParam.UnprovisionedVehicle1 == "無" ? HAISHA.HaiSsryCdSeq != 0 && HAISHA.YouTblSeq==0 
                              : HAISHA.HaiSsryCdSeq != null )
                              && (haiShaParam.DateSpecified == "配車日" ? UNKOBI.HaiSymd == haiShaParam.pickupDate.ToString("yyyyMMdd")
                              : UNKOBI.TouYmd == haiShaParam.pickupDate.ToString("yyyyMMdd"))
                              && (haiShaParam.CompanyChart.CompanyCdSeq ==0? KAISHA.CompanyCdSeq !=null || KAISHA.CompanyCdSeq == null :
                              KAISHA.CompanyCdSeq == haiShaParam.CompanyChart.CompanyCdSeq )
                              && (TOKISK.SiyoStaYmd !=null ? String.Compare(TOKISK.SiyoStaYmd, UNKOBI.HaiSymd) <= 0 : TOKISK.SiyoStaYmd == null || TOKISK.SiyoStaYmd != null)
                              && (TOKISK.SiyoEndYmd !=null ? String.Compare(TOKISK.SiyoEndYmd, UNKOBI.HaiSymd) >= 0 : TOKISK.SiyoEndYmd == null || TOKISK.SiyoEndYmd != null)
                              && (TOKIST.SiyoStaYmd != null ? String.Compare(TOKIST.SiyoStaYmd, UNKOBI.HaiSymd) <= 0 : TOKIST.SiyoStaYmd == null || TOKIST.SiyoStaYmd != null)
                              && (TOKIST.SiyoEndYmd != null ? String.Compare(TOKIST.SiyoEndYmd, UNKOBI.HaiSymd) >= 0 : TOKIST.SiyoEndYmd == null || TOKIST.SiyoEndYmd != null)
                              && (HENSYA.StaYmd != null ? String.Compare(HENSYA.StaYmd, UNKOBI.HaiSymd) <= 0 : HENSYA.StaYmd ==null || HENSYA.StaYmd !=null)
                              && (HENSYA.EndYmd != null ? String.Compare(HENSYA.EndYmd, UNKOBI.HaiSymd) >= 0 : HENSYA.EndYmd == null || HENSYA.EndYmd != null)
                              && (String.IsNullOrWhiteSpace(haiShaParam.HaiSKbn) ? HAISHA.HaiSkbn !=null : HAISHA.HaiSkbn !=2)
                              select new BusAllocationDataGrid()
                              {
                                  YYKSHO_UkeNo = YYKSHO.UkeNo,
                                  YYKSHO_UkeCd = YYKSHO.UkeCd,
                                  TOKISK_TokuiNm = TOKISK.TokuiNm,
                                  TOKIST_SitenNm = TOKIST.SitenNm,
                                  HAISHA_UnkRen = HAISHA.UnkRen,
                                  HAISHA_SyaSyuRen = HAISHA.SyaSyuRen,
                                  HAISHA_TeiDanNo = HAISHA.TeiDanNo,
                                  HAISHA_BunkRen = HAISHA.BunkRen,
                                  HAISHA_GoSya = HAISHA.GoSya,
                                  HAISHA_SyuEigCdSeq = HAISHA.SyuEigCdSeq,
                                  EIGYOS_RyakuNm = EIGYOS.RyakuNm,
                                  HAISHA_KikEigSeq = HAISHA.KikEigSeq,
                                  HAISHA_HaiSSryCdSeq = HAISHA.HaiSsryCdSeq,
                                  SYARYO_SyaRyoNm = SYARYO.SyaRyoNm,
                                  SYASYU_SyaSyuNm = SYASYU.SyaSyuNm,
                                  HAISHA_DanTaNm2 = HAISHA.DanTaNm2,
                                  HAISHA_SyuKoYmd = HAISHA.SyuKoYmd,
                                  HAISHA_SyuKoTime = HAISHA.SyuKoTime,
                                  HAISHA_HaiSYmd = HAISHA.HaiSymd,
                                  HAISHA_HaiSTime = HAISHA.HaiStime,
                                  HAISHA_SyuPaTime = HAISHA.SyuPaTime,
                                  HAISHA_IkMapCdSeq = HAISHA.IkMapCdSeq,
                                  HAISHA_IkNm = HAISHA.IkNm,
                                  HAISHA_HaiSCdSeq = HAISHA.HaiScdSeq,
                                  HAISHA_HaiSNm = HAISHA.HaiSnm,
                                  HAISHA_HaiSJyus1 = HAISHA.HaiSjyus1,
                                  HAISHA_HaiSJyus2 = HAISHA.HaiSjyus2,
                                  HAISHA_HaiSKigou = HAISHA.HaiSkigou,
                                  HAISHA_HaiSKouKCdSeq = HAISHA.HaiSkouKcdSeq,
                                  HAISHA_HaiSKouKNm = HAISHA.HaiSkouKnm,
                                  HAISHA_HaiSBinCdSeq = HAISHA.HaiSbinCdSeq,
                                  HAISHA_HaiSBinNm = HAISHA.HaiSbinNm,
                                  HAISHA_HaiSSetTime = HAISHA.HaiSsetTime,
                                  HAISHA_TouYmd = HAISHA.TouYmd,
                                  HAISHA_TouChTime = HAISHA.TouChTime,
                                  HAISHA_KikYmd = HAISHA.KikYmd,
                                  HAISHA_KikTime = HAISHA.KikTime,
                                  HAISHA_TouCdSeq = HAISHA.TouCdSeq,
                                  HAISHA_TouNm = HAISHA.TouNm,
                                  HAISHA_TouJyusyo1 = HAISHA.TouJyusyo1,
                                  HAISHA_TouJyusyo2 = HAISHA.TouJyusyo2,
                                  HAISHA_TouKigou = HAISHA.TouKigou,
                                  HAISHA_TouKouKCdSeq = HAISHA.TouKouKcdSeq,
                                  HAISHA_TouSKouKNm = HAISHA.TouSkouKnm,
                                  HAISHA_TouBinCdSeq = HAISHA.TouBinCdSeq,
                                  HAISHA_TouBinNm = HAISHA.TouBinNm,
                                  HAISHA_TouSetTime = HAISHA.TouSetTime,
                                  HAISHA_JyoSyaJin = HAISHA.JyoSyaJin,
                                  HAISHA_PlusJin = HAISHA.PlusJin,
                                  HAISHA_DrvJin = HAISHA.DrvJin,
                                  HAISHA_GuiSu = HAISHA.GuiSu,
                                  HAISHA_OthJinKbn1 = HAISHA.OthJinKbn1,
                                  OTHERJIN1_RyakuNm = OTHERJIN1.RyakuNm,
                                  HAISHA_OthJin1 = HAISHA.OthJin1,
                                  HAISHA_OthJinKbn2 = HAISHA.OthJinKbn2,
                                  OTHERJIN2_RyakuNm = OTHERJIN2.RyakuNm,
                                  HAISHA_OthJin2 = HAISHA.OthJin2,
                                  HAISHA_KSKbn = HAISHA.Kskbn,
                                  HAISHA_HaiSKbn = HAISHA.HaiSkbn,
                                  HAISHA_HaiIKbn = HAISHA.HaiIkbn,
                                  HAISHA_NippoKbn = HAISHA.NippoKbn,
                                  HAISHA_YouTblSeq = HAISHA.YouTblSeq,
                                  HAISHA_SyaRyoUncSyaRyoSyo = HAISHA.SyaRyoUnc + HAISHA.SyaRyoSyo,
                                  HAISHA_SyaRyoUnc = HAISHA.SyaRyoUnc,
                                  HAISHA_SyaRyoSyo = HAISHA.SyaRyoSyo,
                                  HAISHA_SyaRyoTes = HAISHA.SyaRyoTes,
                                  HAISHA_PlatNo = HAISHA.PlatNo,
                                  HAISHA_CustomItems1 = HAISHA.CustomItems1,
                                  HAISHA_CustomItems2 = HAISHA.CustomItems2,
                                  HAISHA_CustomItems3 = HAISHA.CustomItems3,
                                  HAISHA_CustomItems4 = HAISHA.CustomItems4,
                                  HAISHA_CustomItems5 = HAISHA.CustomItems5,
                                  HAISHA_CustomItems6 = HAISHA.CustomItems6,
                                  HAISHA_CustomItems7 = HAISHA.CustomItems7,
                                  HAISHA_CustomItems8 = HAISHA.CustomItems8,
                                  HAISHA_CustomItems9 = HAISHA.CustomItems9,
                                  HAISHA_CustomItems10 = HAISHA.CustomItems10,
                                  HAISHA_CustomItems11 = HAISHA.CustomItems11,
                                  HAISHA_CustomItems12 = HAISHA.CustomItems12,
                                  HAISHA_CustomItems13 = HAISHA.CustomItems13,
                                  HAISHA_CustomItems14 = HAISHA.CustomItems14,
                                  HAISHA_CustomItems15 = HAISHA.CustomItems15,
                                  HAISHA_CustomItems16 = HAISHA.CustomItems16,
                                  HAISHA_CustomItems17 = HAISHA.CustomItems17,
                                  HAISHA_CustomItems18 = HAISHA.CustomItems18,
                                  HAISHA_CustomItems19 = HAISHA.CustomItems19,
                                  HAISHA_CustomItems20 = HAISHA.CustomItems20,
                                  UNKOBI_UnkoJKbn = UNKOBI.UnkoJkbn,
                                  UNKOBI_DanTaNm = UNKOBI.DanTaNm,
                                  UNKOBI_HaiSYmd = UNKOBI.HaiSymd,
                                  UNKOBI_HaiSTime = UNKOBI.HaiStime,
                                  UNKOBI_SyukoYmd = UNKOBI.SyukoYmd,
                                  UNKOBI_SyuKoTime = UNKOBI.SyuKoTime,
                                  UNKOBI_TouYmd = UNKOBI.TouYmd,
                                  UNKOBI_TouChTime = UNKOBI.TouChTime,
                                  UNKOBI_KikYmd = UNKOBI.KikYmd,
                                  UNKOBI_KikTime = UNKOBI.KikTime,
                                  HENSYA_TenkoNo = HENSYA.TenkoNo,
                                  SYARYO_SyainCdSeq = SYARYO.SyainCdSeq,
                                  YYKSHO_YoyaKbnSeq=YYKSHO.YoyaKbnSeq
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
