using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StoredProcedureEFCore;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetHaiShaDataTable : IRequest<List<HaiShaDataTable>>
    {
        public string Ukeno { get; set; } = "";
        public string Unkren { get; set; } = "";
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetHaiShaDataTable, List<HaiShaDataTable>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }
            public async Task<List<HaiShaDataTable>> Handle(GetHaiShaDataTable request, CancellationToken cancellationToken)
            {
                var result = new List<HaiShaQueryData>();
                try
                {
                    await _context.LoadStoredProc("PK_dGetHaiShaData")
                        .AddParam("@UkeNo", request.Ukeno)
                        .AddParam("@TenantCdSeq", request.TenantCdSeq)
                        .AddParam("@UnkRen", request.Unkren)
                        .ExecAsync(async e =>
                        {
                            result = await e.ToListAsync<HaiShaQueryData>();
                        });
                    if (result == null) return new List<HaiShaDataTable>();
                    return result.AsEnumerable().OrderBy(x => x.HAISHATeiDanNo).Select((r, i) => new HaiShaDataTable()
                    {
                        Row = i,
                        HAISHA_UkeNo = r.HAISHAUkeNo,
                        YYKSHO_UkeCD = r.YYKSHOUkeCD,
                        HAISHA_UnkRen = r.HAISHAUnkRen,
                        HAISHA_SyaSyuRen = r.HAISHASyaSyuRen,
                        HAISHA_TeiDanNo = r.HAISHATeiDanNo,
                        HAISHA_BunkRen = r.HAISHABunkRen,
                        HAISHA_GoSya = r.HAISHAGoSya,
                        HAISHA_SyuEigCdSeq = r.HAISHASyuEigCdSeq,
                        HAISHA_HaiSSryCdSeq = r.HAISHAHaiSSryCdSeq,
                        HAISHA_DanTaNm2 = r.HAISHADanTaNm2,
                        HAISHA_IkMapCdSeq = r.HAISHAIkMapCdSeq,
                        HAISHA_IkNm = r.HAISHAIkNm,
                        HAISHA_SyuKoYmd = r.HAISHASyuKoYmd,
                        HAISHA_SyuKoTime = r.HAISHASyuKoTime,
                        HAISHA_SyuPaTime = r.HAISHASyuPaTime,
                        HAISHA_HaiSYmd = r.HAISHAHaiSYmd,
                        HAISHA_HaiSTime = r.HAISHAHaiSTime,
                        HAISHA_HaiSCdSeq = r.HAISHAHaiSCdSeq,
                        HAISHA_HaiSNm = r.HAISHAHaiSNm,
                        HAISHA_HaiSJyus1 = r.HAISHAHaiSJyus1,
                        HAISHA_HaiSJyus2 = r.HAISHAHaiSJyus2,
                        HAISHA_HaiSKigou = r.HAISHAHaiSKigou,
                        HAISHA_HaiSKouKCdSeq = r.HAISHAHaiSKouKCdSeq,
                        HAISHA_HaiSKouKNm = r.HAISHAHaiSKouKNm,
                        HAISHA_HaiSBinCdSeq = r.HAISHAHaiSBinCdSeq,
                        HAISHA_HaisBinNm = r.HAISHAHaisBinNm,
                        HAISHA_HaiSSetTime = r.HAISHAHaiSSetTime,
                        HAISHA_KikYmd = r.HAISHAKikYmd,
                        HAISHA_KikTime = r.HAISHAKikTime,
                        HAISHA_TouYmd = r.HAISHATouYmd,
                        HAISHA_TouChTime = r.HAISHATouChTime,
                        HAISHA_TouCdSeq = r.HAISHATouCdSeq,
                        HAISHA_TouNm = r.HAISHATouNm,
                        HAISHA_TouJyusyo1 = r.HAISHATouJyusyo1,
                        HAISHA_TouJyusyo2 = r.HAISHATouJyusyo2,
                        HAISHA_TouKigou = r.HAISHATouKigou,
                        HAISHA_TouKouKCdSeq = r.HAISHATouKouKCdSeq,
                        HAISHA_TouSKouKNm = r.HAISHATouSKouKNm,
                        HAISHA_TouBinCdSeq = r.HAISHATouBinCdSeq,
                        HAISHA_TouBinNm = r.HAISHATouBinNm,
                        HAISHA_TouSetTime = r.HAISHATouSetTime,
                        HAISHA_JyoSyaJin = r.HAISHAJyoSyaJin,
                        HAISHA_PlusJin = r.HAISHAPlusJin,
                        HAISHA_DrvJin = r.HAISHADrvJin,
                        HAISHA_GuiSu = r.HAISHAGuiSu,
                        HAISHA_OthJinKbn1 = r.HAISHAOthJinKbn1,
                        HAISHA_OthJin1 = r.HAISHAOthJin1,
                        HAISHA_OthJinKbn2 = r.HAISHAOthJinKbn2,
                        HAISHA_OthJin2 = r.HAISHAOthJin2,
                        HAISHA_KSKbn = r.HAISHAKSKbn,
                        HAISHA_HaiSKbn = r.HAISHAHaiSKbn,
                        HAISHA_HaiIKbn = r.HAISHAHaiIKbn,
                        HAISHA_NippoKbn = r.HAISHANippoKbn,
                        HAISHA_YouTblSeq = r.HAISHAYouTblSeq,
                        HAISHA_YouKataKbn = r.HAISHAYouKataKbn,
                        HAISHA_SyaRyoUnc = r.HAISHASyaRyoUnc,
                        HAISHA_SyaRyoSyo = r.HAISHASyaRyoSyo,
                        HAISHA_SyaRyoUncSyaRyoSyo = r.HAISHASyaRyoUnc + r.HAISHASyaRyoSyo,
                        HAISHA_SyaRyoTes = r.HAISHASyaRyoTes,
                        HAISHA_YoushaUnc = r.HAISHAYoushaUnc,
                        HAISHA_YoushaSyo = r.HAISHAYoushaSyo,
                        HAISHA_YoushaTes = r.HAISHAYoushaTes,
                        HAISHA_PlatNo = r.HAISHAPlatNo,
                        HAISHA_UkeJyKbnCd = r.HAISHAUkeJyKbnCd,
                        UNKOBI_HaiSYmd = r.UNKOBIHaiSYmd,
                        UNKOBI_HaiSTime = r.UNKOBIHaiSTime,
                        UNKOBI_TouYmd = r.UNKOBITouYmd,
                        UNKOBI_TouChTime = r.UNKOBITouChTime,
                        UNKOBI_DanTaNm = r.UNKOBIDanTaNm,
                        UNKOBI_KSKbn = r.UNKOBIKSKbn,
                        UNKOBI_HaiSKbn = r.UNKOBIHaiSKbn,
                        UNKOBI_HaiIKbn = r.UNKOBIHaiIKbn,
                        UNKOBI_NippoKbn = r.UNKOBINippoKbn,
                        UNKOBI_YouKbn = r.UNKOBIYouKbn,
                        UNKOBI_UkeJyKbnCd = r.UNKOBIUkeJyKbnCd,
                        YYKSHO_TokuiSeq = r.YYKSHOTokuiSeq,
                        YYKSHO_SitenCdSeq = r.YYKSHOSitenCdSeq,
                        TOKISK_RyakuNm = r.TOKISKRyakuNm,
                        TOKIST_RyakuNm = r.TOKISTRyakuNm,
                        SYARYO_SyaRyoCd = r.SYARYOSyaRyoCd,
                        SYARYO_SyaRyoNm = r.SYARYOSyaRyoNm,
                        SYASYU_SyaSyuNm = r.SYASYUSyaSyuNm,
                        KAISHA_RyakuNm = r.KAISHARyakuNm,
                        EIGYOS_RyakuNm = r.EIGYOSRyakuNm,
                        YOUSHA_YouCdSeq = r.YOUSHAYouCdSeq,
                        YOUSHA_YouSitCdSeq = r.YOUSHAYouSitCdSeq,
                        YOUSHASAKI_RyakuNm = r.YOUSHASAKIRyakuNm,
                        YOUSHASAKISITEN_RyakuNm = r.YOUSHASAKISITENRyakuNm,
                        YOUSHAKATA_RyakuNm = r.YOUSHAKATARyakuNm,
                        CodeKb_OTHER1 = r.CodeKbOTHER1,
                        CodeKb_OTHER2 = r.CodeKbOTHER2,
                        SYASYU_KataKbn = r.SYASYUKataKbn,
                        KATAKB_RyakuNm = r.KATAKBRyakuNm == null ? "指定なし" : r.KATAKBRyakuNm,
                        YYKSHO_SeiTaiYmd = r.YYKSHOSeiTaiYmd,
                        YYKSHO_SeiEigCdSeq = r.YYKSHOSeiEigCdSeq,
                        HAISHA_BunKSyuJyn = r.HAISHABunKSyuJyn,
                        GYOUSYA_GyosyaCd = r.GYOUSYAGyosyaCd,
                        GYOUSYA_GyosyaNm = r.GYOUSYAGyosyaNm,
                        YOUSHASAKI_TokuiCd = r.YOUSHASAKITokuiCd,
                        YOUSHASAKISITEN_SitenCd = r.YOUSHASAKISITENSitenCd,
                    }).ToList();
                }
                catch (Exception ex)
                {
                    return new List<HaiShaDataTable>();
                }
            }
        }
    }
}
