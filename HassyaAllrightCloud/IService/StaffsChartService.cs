using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Domain.Dto;
using System.Linq;
using HassyaAllrightCloud.Commons.Helpers;
using DevExpress.CodeParser;

namespace HassyaAllrightCloud.IService
{
    public interface IStaffsChartService
    {
        Task<List<StaffsLines>> GetStaffdatabooking(DateTime busstardate, DateTime busenddate, int YoyaKbnSeq, int TenantCdSeq);
        Task<List<StaffsLines>> GetStaffdatabookingbyid(string ukeno, short unkren, short teidanno, short bunkren,  int TenantCdSeq);
        Task<List<StaffsLines>> GetStaffdatanonassign(DateTime busstardate, DateTime busenddate, int TenantCdSeq);
        Task<List<StaffsLines>> GetStaffdatagraybooking(int yoyaKbnSeq,int syuEigCdSeqFrom, int syuEigCdSeqTo, ConfigStaffsChart searchData, int TenantCdSeq);
        Task<List<StaffsLines>> GetStaffdatahaiiin(string ukeno, short unkren, short teidanno, int YoyaKbnSeq, int TenantCdSeq);
        Task<List<StaffsLines>> GetStaffdatahaiiinBusallocation(string ukeno, short unkren, short teidanno, short bunkRen, int TenantCdSeq);
        Task<List<StaffsLines>> GetStaffdatahaiiinbooking(string ukeno, short unkren, short teidanno, short bunkRen, int TenantCdSeq);
    }
    public class StaffsChartService : IStaffsChartService
    {
        private readonly KobodbContext _dbContext;
        private readonly BusScheduleHelper _helper;
        private readonly ITPM_CodeSyService _codeSyuService;

        public StaffsChartService(KobodbContext context, BusScheduleHelper helper, ITPM_CodeSyService codeSyuService)
        {
            _dbContext = context;
            _helper = helper;
            _codeSyuService = codeSyuService;

        }
        public async Task<List<StaffsLines>> GetStaffdatagraybooking(int yoyaKbnSeq,int syuEigCdSeqFrom, int syuEigCdSeqTo, ConfigStaffsChart searchData, int TenantCdSeq)
        {
            string codeSyuUKEJYKBNCD = "UKEJYKBNCD";
            int tenantUKEJYKBNCD = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuUKEJYKBNCD);
            string codeSyuSIJJOKB1 = "SIJJOKBN1";
            int tenantSIJJOKB1 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB1);
            string codeSyuSIJJOKB2 = "SIJJOKBN2";
            int tenantSIJJOKB2 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB2);
            string codeSyuSIJJOKB3 = "SIJJOKBN3";
            int tenantSIJJOKB3 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB3);
            string codeSyuSIJJOKB4 = "SIJJOKBN4";
            int tenantSIJJOKB4 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB4);
            string codeSyuSIJJOKB5 = "SIJJOKBN5";
            int tenantSIJJOKB5 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB5);
            return await (from HAISHA in _dbContext.TkdHaisha
                          join YYKASHO in _dbContext.TkdYyksho
                                on new { HAISHA.UkeNo, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYKASHO.UkeNo, YYKASHO.TenantCdSeq }
                          join YYKSYU in _dbContext.TkdYykSyu
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen, SiyoKbn = 1 }
                            equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen, SiyoKbn =  Convert.ToInt32(YYKSYU.SiyoKbn) } into YYKSYU_join
                          from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                          join YYSYASYU in _dbContext.VpmSyaSyu
                                on new { YYKSYU.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYSYASYU.SyaSyuCdSeq, YYSYASYU.TenantCdSeq } into YYSYASYU_join
                          from YYSYASYU in YYSYASYU_join.DefaultIfEmpty()
                          join HAIIN in _dbContext.TkdHaiin
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.TeiDanNo, HAISHA.BunkRen, SiyoKbn = 1 }
                            equals new { HAIIN.UkeNo, HAIIN.UnkRen, HAIIN.TeiDanNo, HAIIN.BunkRen, SiyoKbn = Convert.ToInt32(HAIIN.SiyoKbn) } into HAIIN_join
                          from HAIIN in HAIIN_join.DefaultIfEmpty()
                          join KYOSHE in _dbContext.VpmKyoShe on HAIIN.SyainCdSeq equals KYOSHE.SyainCdSeq into KYOSHE_join
                          from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                          join SYOKUM in _dbContext.VpmSyokum on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq into SYOKUM_join
                          from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                          join UNKOBI in _dbContext.TkdUnkobi
                                on new { HAISHA.UkeNo, HAISHA.UnkRen }
                            equals new { UNKOBI.UkeNo, UNKOBI.UnkRen } into UNKOBI_join
                          from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                          join EIGYOSHOS in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.SyuEigCdSeq } equals new { EigyoCdSeq = EIGYOSHOS.EigyoCdSeq } into EIGYOSHOS_join
                          from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                                on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                          join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                          from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                          join KAISHA1 in _dbContext.VpmCompny
                                on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq }
                          join TOKISK in _dbContext.VpmTokisk
                                on new { YYKASHO.TokuiSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { TOKISK.TokuiSeq, TOKISK.TenantCdSeq } into TOKISK_join
                          from TOKISK in TOKISK_join.DefaultIfEmpty()
                          join TOKIST in _dbContext.VpmTokiSt
                                on new { YYKASHO.TokuiSeq, YYKASHO.SitenCdSeq }
                            equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                          from TOKIST in TOKIST_join.DefaultIfEmpty()
                          join SYARYO in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = HAISHA.HaiSsryCdSeq } equals new { SyaRyoCdSeq = SYARYO.SyaRyoCdSeq } into SYARYO_join
                          from SYARYO in SYARYO_join.DefaultIfEmpty()
                          join SYASYU in _dbContext.VpmSyaSyu
                                on new { SYARYO.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { SYASYU.SyaSyuCdSeq, SYASYU.TenantCdSeq } into SYASYU_join
                          from SYASYU in SYASYU_join.DefaultIfEmpty()
                          join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                          from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                          where
                            YYKASHO.YoyaSyu == 1 &&
                            (yoyaKbnSeq!=0?YYKASHO.YoyaKbnSeq == yoyaKbnSeq:YYKASHO.YoyaKbnSeq >= 0) &&
                            HAISHA.YouTblSeq == 0 &&
                            HAISHA.SiyoKbn == 1 &&

                            //string.Compare(TOKIST.SiyoEndYmd, searchData.DepartureDateStart) <= 0 &&
                            //string.Compare(TOKIST.SiyoStaYmd, searchData.DepartureDateEnd) >= 0 &&
                            ((HAISHA.SyuKoYmd + HAISHA.SyuKoTime).CompareTo(searchData.DepartureDateStart+searchData.DepartureTimeStart)>=0 && (HAISHA.SyuKoYmd + HAISHA.SyuKoTime).CompareTo( searchData.DepartureDateEnd+searchData.DepartureTimeEnd) <=0 ||
                            (HAISHA.KikYmd + HAISHA.KikTime).CompareTo(searchData.ArrivalDateStart+searchData.ArrivalTimeStart) >= 0 && (HAISHA.KikYmd + HAISHA.KikTime).CompareTo(searchData.ArrivalDateEnd+searchData.ArrivalTimeEnd) <= 0 ||
                            (HAISHA.HaiSymd + HAISHA.HaiStime).CompareTo(searchData.DeliveryDateStart+searchData.DeliveryTimeStart) >= 0  && (HAISHA.HaiSymd + HAISHA.HaiStime).CompareTo(searchData.DeliveryDateEnd+searchData.DeliveryTimeEnd) <=0  ||
                            (HAISHA.TouYmd + HAISHA.TouChTime).CompareTo(searchData.ReturnDateStart+searchData.ReturnTimeStart) >=0  && (HAISHA.TouYmd + HAISHA.TouChTime).CompareTo(searchData.ReturnDateEnd+searchData.ReturnTimeEnd) <=0 ) &&
                            (syuEigCdSeqFrom!=0?HAISHA.SyuEigCdSeq >= syuEigCdSeqFrom: HAISHA.SyuEigCdSeq>=0) &&
                            (syuEigCdSeqTo!=0?HAISHA.SyuEigCdSeq <= syuEigCdSeqTo:HAISHA.SyuEigCdSeq>=0) &&
                            HAISHA.Kskbn != 1 &&
                            HAISHA.NippoKbn != 2 &&
                            YYKASHO.SeiTaiYmd.CompareTo(_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd!=null?_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd:"19100101") > 0
                          select new StaffsLines()
                          {
                              Haisha_UkeNo = HAISHA.UkeNo,
                              Haisha_UnkRen = HAISHA.UnkRen,
                              Haisha_SyaSyuRen = HAISHA.SyaSyuRen,
                              Haisha_TeiDanNo = HAISHA.TeiDanNo,
                              Haisha_BunkRen = HAISHA.BunkRen,
                              Haisha_GoSya = HAISHA.GoSya,
                              Haisha_KSKbn = HAISHA.Kskbn,
                              Haisha_HaiSKbn = HAISHA.HaiSkbn,
                              Haisha_HaiIKbn = HAISHA.HaiIkbn,
                              Haisha_KhinKbn = HAISHA.KhinKbn,
                              Haisha_NippoKbn = HAISHA.NippoKbn,
                              Haisha_DanTaNm2 = HAISHA.DanTaNm2,
                              Haisha_IkNm = HAISHA.IkNm,
                              Haisha_HaiSSryCdSeq = HAISHA.HaiSsryCdSeq,
                              Haisha_SyuEigCdSeq = HAISHA.SyuEigCdSeq,
                              Haisha_HaiSYmd = HAISHA.HaiSymd,
                              Haisha_HaiSTime = HAISHA.HaiStime,
                              Haisha_SyuKoYmd = HAISHA.SyuKoYmd,
                              Haisha_SyuKoTime = HAISHA.SyuKoTime,
                              Haisha_SyuPaTime = HAISHA.SyuPaTime,
                              Haisha_BunKSyuJyn=HAISHA.BunKsyuJyn,
                              Haisha_HaiSNm = HAISHA.HaiSnm,
                              Haisha_TouYmd = HAISHA.TouYmd,
                              Haisha_TouChTime = HAISHA.TouChTime,
                              Haisha_KikYmd = HAISHA.KikYmd,
                              Haisha_KikTime = HAISHA.KikTime,
                              Haisha_TouNm = HAISHA.TouNm,
                              Haisha_JyoSyaJin = HAISHA.JyoSyaJin,
                              Haisha_PlusJin = HAISHA.PlusJin,
                              Haisha_DrvJin = HAISHA.DrvJin,
                              Haisha_GuiSu = HAISHA.GuiSu,
                              Haisha_HaiSKouKNm = HAISHA.HaiSkouKnm,
                              Haisha_HaiSBinNm = HAISHA.HaiSbinNm,
                              Haisha_HaiSSetTime = HAISHA.HaiSsetTime,
                              Haisha_HaiSKigou = HAISHA.HaiSkigou,
                              Haisha_TouSKouKNm = HAISHA.TouSkouKnm,
                              Haisha_TouSBinNm = HAISHA.TouBinNm,
                              Haisha_TouSetTime = HAISHA.TouSetTime,
                              Haisha_TouKigou = HAISHA.TouKigou,
                              Haisha_OthJin1 = HAISHA.OthJin1,
                              Haisha_OthJin2 = HAISHA.OthJin2,
                              Haisha_PlatNo = HAISHA.PlatNo,
                              Haisha_HaiCom = HAISHA.HaiCom,
                              Haisha_SyaRyoUnc = HAISHA.SyaRyoUnc,
                              Haisha_HaiSJyus1 = HAISHA.HaiSjyus1,
                              Haisha_HaiSjyus2 = HAISHA.HaiSjyus2,
                              Haisha_TouJyusyo1 = HAISHA.TouJyusyo1,
                              Haisha_TouJyusyo2 = HAISHA.TouJyusyo2,
                              Haisha_KikEigSeq = HAISHA.KikEigSeq,
                              Haisha_YouTblSeq = HAISHA.YouTblSeq,
                              Yykasho_UkeCd = YYKASHO.UkeCd,
                              Yykasho_NippoKbn = YYKASHO.NippoKbn,
                              Yykasho_KaktYmd = YYKASHO.KaktYmd,
                              Yykasho_TokuiSeq = YYKASHO.TokuiSeq,
                              Yykasho_UkeEigCdSeq = YYKASHO.UkeEigCdSeq,
                              Tokisk_RyakuNm = TOKISK.RyakuNm,
                              Yykasho_SitenCdSeq = YYKASHO.SitenCdSeq,
                              Tokist_RyakuNm = TOKIST.RyakuNm,
                              Yykasho_InTanCdSeq = YYKASHO.InTanCdSeq,
                              Syain_SyainNm = SYAIN.SyainNm,
                              Yykasho_EigTanCdSeq = YYKASHO.EigTanCdSeq,
                              Eigyotan_SyainNm = EIGYOTAN.SyainNm,
                              Unkobi_DanTaNm = UNKOBI.DanTaNm,
                              Unkobi_HaiSYmd = UNKOBI.HaiSymd,
                              Unkobi_HaiSTime = UNKOBI.HaiStime,
                              Unkobi_TouYmd = UNKOBI.TouYmd,
                              Unkobi_TouChTime = UNKOBI.TouChTime,
                              Unkobi_ZenHaFlg = UNKOBI.ZenHaFlg,
                              Unkobi_KhakFlg = UNKOBI.KhakFlg,
                              Unkobi_SyukoYmd = UNKOBI.SyukoYmd,
                              Unkobi_KikYmd = UNKOBI.KikYmd,
                              Unkobi_UnkoJKbn = UNKOBI.UnkoJkbn,
                              Syaryo_SyaRyoNm = SYARYO.SyaRyoNm,
                              Syaryo_TeiCnt = SYARYO.TeiCnt,
                              Syasyu_SyaSyuNm = SYASYU.SyaSyuNm,
                              Syasyu_SyaSyuKigo = SYASYU.SyaSyuKigo,
                              YySyasyu_SyaSyuNm = YYSYASYU.SyaSyuNm,
                              Haiin_UkeNo = HAIIN.UkeNo,
                              Haiin_UnkRen = HAIIN.UnkRen,
                              Haiin_TeiDanNo = HAIIN.TeiDanNo,
                              Haiin_BunkRen = HAIIN.BunkRen,
                              Haiin_HaiInRen = HAIIN.HaiInRen,
                              Haiin_SyainCdSeq = HAIIN.SyainCdSeq,
                              Haiin_SyukinTime = HAIIN.SyukinTime,
                              Haiin_Syukinbasy = HAIIN.Syukinbasy,
                              Haiin_TaiknTime = HAIIN.TaiknTime,
                              Haiin_TaiknBasy = HAIIN.TaiknBasy,
                              Kyoshe_SyokumuCdSeq = KYOSHE.SyokumuCdSeq,
                              Syokum_SyokumuKbn = SYOKUM.SyokumuKbn,
                              Syokum_SyokumuNm = SYOKUM.SyokumuNm,
                              Eigyoshos_RyakuNm = EIGYOSHOS.RyakuNm,
                              Eigyoshos1_RyakuNm = EIGYOSHOS1.RyakuNm,
                              Haisha_BikoNm = HAISHA.BikoNm,
                              Yyksho_BikoNm = YYKASHO.BikoNm,
                              Unkobi_BikoNm = UNKOBI.BikoNm,
                              Yyksyu_KataKbn = (from TKD_YykSyu in _dbContext.TkdYykSyu
                                                where
                                        TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                                        TKD_YykSyu.SiyoKbn == 1
                                                select
                                                    TKD_YykSyu.KataKbn).First(),
                              Yyksyu_SyaSyuDai =
                              (from TKD_YykSyu in _dbContext.TkdYykSyu
                               where
                       TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                       TKD_YykSyu.SiyoKbn == 1
                               select new
                               {
                                   TKD_YykSyu.SyaSyuDai
                               }).Sum(p => p.SyaSyuDai),
                              UKEJOKEN_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                    where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.UkeJyKbnCd) && UKEJOKEN.CodeSyu == codeSyuUKEJYKBNCD && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantUKEJYKBNCD
                                                    select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN1_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn1) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB1 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB1
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN2_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn2) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB2 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB2
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN3_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn3) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB3 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB3
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN4_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn4) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB4 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB4
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN5_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn5) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB5 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB5
                                                     select UKEJOKEN.CodeKbnNm).First(),

                          }).ToListAsync();
        }
        public async Task<List<StaffsLines>> GetStaffdatabooking(DateTime busstardate, DateTime busenddate, int YoyaKbnSeq, int TenantCdSeq)
        {
            string codeSyuUKEJYKBNCD = "UKEJYKBNCD";
            int tenantUKEJYKBNCD = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuUKEJYKBNCD);
            string codeSyuSIJJOKB1 = "SIJJOKBN1";
            int tenantSIJJOKB1 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB1);
            string codeSyuSIJJOKB2 = "SIJJOKBN2";
            int tenantSIJJOKB2 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB2);
            string codeSyuSIJJOKB3 = "SIJJOKBN3";
            int tenantSIJJOKB3 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB3);
            string codeSyuSIJJOKB4 = "SIJJOKBN4";
            int tenantSIJJOKB4 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB4);
            string codeSyuSIJJOKB5 = "SIJJOKBN5";
            int tenantSIJJOKB5 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB5);
            string DateStarAsString = busstardate.ToString("yyyyMMdd");
            string DateEndAsString = busenddate.ToString("yyyyMMdd");
            return await (from HAISHA in _dbContext.TkdHaisha
                          join YYKASHO in _dbContext.TkdYyksho
                                on new { HAISHA.UkeNo, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYKASHO.UkeNo, YYKASHO.TenantCdSeq }
                          join YYKSYU in _dbContext.TkdYykSyu
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen, SiyoKbn = 1 }
                            equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen, SiyoKbn = Convert.ToInt32(YYKSYU.SiyoKbn) } into YYKSYU_join
                          from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                          join YYSYASYU in _dbContext.VpmSyaSyu
                                on new { YYKSYU.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYSYASYU.SyaSyuCdSeq, YYSYASYU.TenantCdSeq } into YYSYASYU_join
                          from YYSYASYU in YYSYASYU_join.DefaultIfEmpty()
                          join HAIIN in _dbContext.TkdHaiin
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.TeiDanNo, HAISHA.BunkRen }
                            equals new { HAIIN.UkeNo, HAIIN.UnkRen, HAIIN.TeiDanNo, HAIIN.BunkRen }
                          join KYOSHE in _dbContext.VpmKyoShe on HAIIN.SyainCdSeq equals KYOSHE.SyainCdSeq into KYOSHE_join
                          from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                          join SYOKUM in _dbContext.VpmSyokum on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq into SYOKUM_join
                          from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                          join UNKOBI in _dbContext.TkdUnkobi
                                on new { HAISHA.UkeNo, HAISHA.UnkRen }
                            equals new { UNKOBI.UkeNo, UNKOBI.UnkRen } into UNKOBI_join
                          from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                          join EIGYOSHOS in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.SyuEigCdSeq } equals new { EigyoCdSeq = EIGYOSHOS.EigyoCdSeq } into EIGYOSHOS_join
                          from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                                on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                          join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                          from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                          join KAISHA1 in _dbContext.VpmCompny
                                on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq }
                          join TOKISK in _dbContext.VpmTokisk
                                on new { YYKASHO.TokuiSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { TOKISK.TokuiSeq, TOKISK.TenantCdSeq } into TOKISK_join
                          from TOKISK in TOKISK_join.DefaultIfEmpty()
                          join TOKIST in _dbContext.VpmTokiSt
                                on new { YYKASHO.TokuiSeq, YYKASHO.SitenCdSeq }
                            equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                          from TOKIST in TOKIST_join.DefaultIfEmpty()
                          join SYARYO in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = HAISHA.HaiSsryCdSeq } equals new { SyaRyoCdSeq = SYARYO.SyaRyoCdSeq } into SYARYO_join
                          from SYARYO in SYARYO_join.DefaultIfEmpty()
                          join SYASYU in _dbContext.VpmSyaSyu
                                on new { SYARYO.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { SYASYU.SyaSyuCdSeq, SYASYU.TenantCdSeq } into SYASYU_join
                          from SYASYU in SYASYU_join.DefaultIfEmpty()
                          join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                          from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                          where
                            YYKASHO.YoyaSyu == 1 &&
                            YYKASHO.YoyaKbnSeq == 1 &&
                          string.Compare(HAISHA.KikYmd, DateStarAsString) >= 0 &&
                           string.Compare(HAISHA.SyuKoYmd, DateEndAsString) <= 0
                            &&
                           String.Compare(TOKISK.SiyoStaYmd, DateStarAsString) <= 0 &&
                           String.Compare(TOKISK.SiyoEndYmd, DateStarAsString) >= 0 &&
                            HAISHA.Kskbn != 1 &&
                            HAISHA.YouTblSeq == 0 &&
                            HAISHA.SiyoKbn == 1 &&
                            HAIIN.SiyoKbn == 1 &&
                            YYKASHO.SiyoKbn == 1 
                          orderby
                            HAISHA.UkeNo,
                            HAIIN.HaiInRen
                          select new StaffsLines()
                          {
                              Haisha_UkeNo = HAISHA.UkeNo,
                              Haisha_UnkRen = HAISHA.UnkRen,
                              Haisha_SyaSyuRen = HAISHA.SyaSyuRen,
                              Haisha_TeiDanNo = HAISHA.TeiDanNo,
                              Haisha_BunkRen = HAISHA.BunkRen,
                              Haisha_GoSya = HAISHA.GoSya,
                              Haisha_KSKbn = HAISHA.Kskbn,
                              Haisha_HaiSKbn = HAISHA.HaiSkbn,
                              Haisha_HaiIKbn = HAISHA.HaiIkbn,
                              Haisha_KhinKbn = HAISHA.KhinKbn,
                              Haisha_NippoKbn = HAISHA.NippoKbn,
                              Haisha_DanTaNm2 = HAISHA.DanTaNm2,
                              Haisha_IkNm = HAISHA.IkNm,
                              Haisha_HaiSSryCdSeq = HAISHA.HaiSsryCdSeq,
                              Haisha_SyuEigCdSeq = HAISHA.SyuEigCdSeq,
                              Haisha_HaiSYmd = HAISHA.HaiSymd,
                              Haisha_HaiSTime = HAISHA.HaiStime,
                              Haisha_SyuKoYmd = HAISHA.SyuKoYmd,
                              Haisha_SyuKoTime = HAISHA.SyuKoTime,
                              Haisha_SyuPaTime = HAISHA.SyuPaTime,
                              Haisha_HaiSNm = HAISHA.HaiSnm,
                              Haisha_TouYmd = HAISHA.TouYmd,
                              Haisha_TouChTime = HAISHA.TouChTime,
                              Haisha_KikYmd = HAISHA.KikYmd,
                              Haisha_KikTime = HAISHA.KikTime,
                              Haisha_TouNm = HAISHA.TouNm,
                              Haisha_BunKSyuJyn=HAISHA.BunKsyuJyn,
                              Haisha_JyoSyaJin = HAISHA.JyoSyaJin,
                              Haisha_PlusJin = HAISHA.PlusJin,
                              Haisha_DrvJin = HAISHA.DrvJin,
                              Haisha_GuiSu = HAISHA.GuiSu,
                              Haisha_HaiSKouKNm = HAISHA.HaiSkouKnm,
                              Haisha_HaiSBinNm = HAISHA.HaiSbinNm,
                              Haisha_HaiSSetTime = HAISHA.HaiSsetTime,
                              Haisha_HaiSKigou = HAISHA.HaiSkigou,
                              Haisha_TouSKouKNm = HAISHA.TouSkouKnm,
                              Haisha_TouSBinNm = HAISHA.TouBinNm,
                              Haisha_TouSetTime = HAISHA.TouSetTime,
                              Haisha_TouKigou = HAISHA.TouKigou,
                              Haisha_OthJin1 = HAISHA.OthJin1,
                              Haisha_OthJin2 = HAISHA.OthJin2,
                              Haisha_PlatNo = HAISHA.PlatNo,
                              Haisha_HaiCom = HAISHA.HaiCom,
                              Haisha_SyaRyoUnc = HAISHA.SyaRyoUnc,
                              Haisha_HaiSJyus1 = HAISHA.HaiSjyus1,
                              Haisha_HaiSjyus2 = HAISHA.HaiSjyus2,
                              Haisha_TouJyusyo1 = HAISHA.TouJyusyo1,
                              Haisha_TouJyusyo2 = HAISHA.TouJyusyo2,
                              Haisha_KikEigSeq = HAISHA.KikEigSeq,
                              Haisha_YouTblSeq=HAISHA.YouTblSeq,
                              Yykasho_UkeCd = YYKASHO.UkeCd,
                              Yykasho_NippoKbn = YYKASHO.NippoKbn,
                              Yykasho_KaktYmd = YYKASHO.KaktYmd,
                              Yykasho_TokuiSeq = YYKASHO.TokuiSeq,
                              Yykasho_UkeEigCdSeq = YYKASHO.UkeEigCdSeq,
                              Tokisk_RyakuNm = TOKISK.RyakuNm,
                              Yykasho_SitenCdSeq = YYKASHO.SitenCdSeq,
                              Tokist_RyakuNm = TOKIST.RyakuNm,
                              Yykasho_InTanCdSeq = YYKASHO.InTanCdSeq,
                              Syain_SyainNm = SYAIN.SyainNm,
                              Yykasho_EigTanCdSeq = YYKASHO.EigTanCdSeq,
                              Eigyotan_SyainNm = EIGYOTAN.SyainNm,
                              Unkobi_DanTaNm = UNKOBI.DanTaNm,
                              Unkobi_HaiSYmd = UNKOBI.HaiSymd,
                              Unkobi_HaiSTime = UNKOBI.HaiStime,
                              Unkobi_TouYmd = UNKOBI.TouYmd,
                              Unkobi_TouChTime = UNKOBI.TouChTime,
                              Unkobi_ZenHaFlg = UNKOBI.ZenHaFlg,
                              Unkobi_KhakFlg = UNKOBI.KhakFlg,
                              Unkobi_SyukoYmd = UNKOBI.SyukoYmd,
                              Unkobi_KikYmd = UNKOBI.KikYmd,
                              Unkobi_UnkoJKbn = UNKOBI.UnkoJkbn,
                              Syaryo_SyaRyoNm = SYARYO.SyaRyoNm,
                              Syaryo_TeiCnt = SYARYO.TeiCnt,
                              Syasyu_SyaSyuNm = SYASYU.SyaSyuNm,
                              Syasyu_SyaSyuKigo = SYASYU.SyaSyuKigo,
                              YySyasyu_SyaSyuNm = YYSYASYU.SyaSyuNm,
                              Haiin_UkeNo = HAIIN.UkeNo,
                              Haiin_UnkRen = HAIIN.UnkRen,
                              Haiin_TeiDanNo = HAIIN.TeiDanNo,
                              Haiin_BunkRen = HAIIN.BunkRen,
                              Haiin_HaiInRen = HAIIN.HaiInRen,
                              Haiin_SyainCdSeq = HAIIN.SyainCdSeq,
                              Haiin_SyukinTime = HAIIN.SyukinTime,
                              Haiin_Syukinbasy = HAIIN.Syukinbasy,
                              Haiin_TaiknTime = HAIIN.TaiknTime,
                              Haiin_TaiknBasy = HAIIN.TaiknBasy,
                              Kyoshe_SyokumuCdSeq = KYOSHE.SyokumuCdSeq,
                              Syokum_SyokumuKbn = SYOKUM.SyokumuKbn,
                              Syokum_SyokumuNm = SYOKUM.SyokumuNm,
                              Eigyoshos_RyakuNm = EIGYOSHOS.RyakuNm,
                              Eigyoshos1_RyakuNm = EIGYOSHOS1.RyakuNm,
                              Haisha_BikoNm = HAISHA.BikoNm,
                              Yyksho_BikoNm = YYKASHO.BikoNm,
                              Unkobi_BikoNm = UNKOBI.BikoNm,
                              Yyksyu_KataKbn = (from TKD_YykSyu in _dbContext.TkdYykSyu
                                                where
                                        TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                                        TKD_YykSyu.SiyoKbn == 1
                                                select
                                                    TKD_YykSyu.KataKbn).First(),
                              Yyksyu_SyaSyuDai =
                              (from TKD_YykSyu in _dbContext.TkdYykSyu
                               where
                       TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                       TKD_YykSyu.SiyoKbn == 1
                               select new
                               {
                                   TKD_YykSyu.SyaSyuDai
                               }).Sum(p => p.SyaSyuDai),
                              UKEJOKEN_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                    where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.UkeJyKbnCd) && UKEJOKEN.CodeSyu == codeSyuUKEJYKBNCD && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantUKEJYKBNCD
                                                    select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN1_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn1) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB1 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB1
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN2_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn2) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB2 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB2
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN3_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn3) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB3 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB3
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN4_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn4) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB4 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB4
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN5_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn5) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB5 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB5
                                                     select UKEJOKEN.CodeKbnNm).First(),

                          }).ToListAsync();
        }

        public async Task<List<StaffsLines>> GetStaffdatabookingbyid(string ukeno, short unkren, short teidanno, short bunkren,  int TenantCdSeq)
        {
            string codeSyuUKEJYKBNCD = "UKEJYKBNCD";
            int tenantUKEJYKBNCD = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuUKEJYKBNCD);
            string codeSyuSIJJOKB1 = "SIJJOKBN1";
            int tenantSIJJOKB1 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB1);
            string codeSyuSIJJOKB2 = "SIJJOKBN2";
            int tenantSIJJOKB2 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB2);
            string codeSyuSIJJOKB3 = "SIJJOKBN3";
            int tenantSIJJOKB3 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB3);
            string codeSyuSIJJOKB4 = "SIJJOKBN4";
            int tenantSIJJOKB4 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB4);
            string codeSyuSIJJOKB5 = "SIJJOKBN5";
            int tenantSIJJOKB5 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB5);
            return await (from HAISHA in _dbContext.TkdHaisha
                          join YYKASHO in _dbContext.TkdYyksho
                                on new { HAISHA.UkeNo, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYKASHO.UkeNo, YYKASHO.TenantCdSeq }
                          join YYKSYU in _dbContext.TkdYykSyu
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen, SiyoKbn = 1 }
                            equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen, SiyoKbn = Convert.ToInt32(YYKSYU.SiyoKbn) } into YYKSYU_join
                          from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                          join YYSYASYU in _dbContext.VpmSyaSyu
                                on new { YYKSYU.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYSYASYU.SyaSyuCdSeq, YYSYASYU.TenantCdSeq } into YYSYASYU_join
                          from YYSYASYU in YYSYASYU_join.DefaultIfEmpty()
                          join HAIIN in _dbContext.TkdHaiin
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.TeiDanNo, HAISHA.BunkRen }
                            equals new { HAIIN.UkeNo, HAIIN.UnkRen, HAIIN.TeiDanNo, HAIIN.BunkRen }
                          join KYOSHE in _dbContext.VpmKyoShe on HAIIN.SyainCdSeq equals KYOSHE.SyainCdSeq into KYOSHE_join
                          from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                          join SYOKUM in _dbContext.VpmSyokum on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq into SYOKUM_join
                          from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                          join UNKOBI in _dbContext.TkdUnkobi
                                on new { HAISHA.UkeNo, HAISHA.UnkRen }
                            equals new { UNKOBI.UkeNo, UNKOBI.UnkRen } into UNKOBI_join
                          from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                          join EIGYOSHOS in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.SyuEigCdSeq } equals new { EigyoCdSeq = EIGYOSHOS.EigyoCdSeq } into EIGYOSHOS_join
                          from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                                on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                          join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                          from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                          join KAISHA1 in _dbContext.VpmCompny
                                on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq }
                          join TOKISK in _dbContext.VpmTokisk
                                on new { YYKASHO.TokuiSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { TOKISK.TokuiSeq, TOKISK.TenantCdSeq } into TOKISK_join
                          from TOKISK in TOKISK_join.DefaultIfEmpty()
                          join TOKIST in _dbContext.VpmTokiSt
                                on new { YYKASHO.TokuiSeq, YYKASHO.SitenCdSeq }
                            equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                          from TOKIST in TOKIST_join.DefaultIfEmpty()
                          join SYARYO in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = HAISHA.HaiSsryCdSeq } equals new { SyaRyoCdSeq = SYARYO.SyaRyoCdSeq } into SYARYO_join
                          from SYARYO in SYARYO_join.DefaultIfEmpty()
                          join SYASYU in _dbContext.VpmSyaSyu
                                on new { SYARYO.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { SYASYU.SyaSyuCdSeq, SYASYU.TenantCdSeq } into SYASYU_join
                          from SYASYU in SYASYU_join.DefaultIfEmpty()
                          join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                          from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                          where
                            YYKASHO.YoyaSyu == 1 &&
                            HAISHA.UkeNo==ukeno &&
                            HAISHA.UnkRen==unkren &&
                            HAISHA.BunkRen==bunkren &&
                            HAISHA.TeiDanNo==teidanno&&
                            HAISHA.Kskbn != 1 &&
                            HAISHA.YouTblSeq == 0 &&
                            HAISHA.SiyoKbn == 1 &&
                            HAIIN.SiyoKbn == 1 &&
                            YYKASHO.SiyoKbn == 1 &&
                            HAISHA.NippoKbn != 2 &&
                            YYKASHO.SeiTaiYmd.CompareTo(_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd!=null?_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd:"19100101") > 0
                          orderby
                            HAISHA.UkeNo,
                            HAIIN.HaiInRen
                          select new StaffsLines()
                          {
                              Haisha_UkeNo = HAISHA.UkeNo,
                              Haisha_UnkRen = HAISHA.UnkRen,
                              Haisha_SyaSyuRen = HAISHA.SyaSyuRen,
                              Haisha_TeiDanNo = HAISHA.TeiDanNo,
                              Haisha_BunkRen = HAISHA.BunkRen,
                              Haisha_GoSya = HAISHA.GoSya,
                              Haisha_KSKbn = HAISHA.Kskbn,
                              Haisha_HaiSKbn = HAISHA.HaiSkbn,
                              Haisha_HaiIKbn = HAISHA.HaiIkbn,
                              Haisha_KhinKbn = HAISHA.KhinKbn,
                              Haisha_NippoKbn = HAISHA.NippoKbn,
                              Haisha_DanTaNm2 = HAISHA.DanTaNm2,
                              Haisha_IkNm = HAISHA.IkNm,
                              Haisha_HaiSSryCdSeq = HAISHA.HaiSsryCdSeq,
                              Haisha_SyuEigCdSeq = HAISHA.SyuEigCdSeq,
                              Haisha_HaiSYmd = HAISHA.HaiSymd,
                              Haisha_HaiSTime = HAISHA.HaiStime,
                              Haisha_SyuKoYmd = HAISHA.SyuKoYmd,
                              Haisha_SyuKoTime = HAISHA.SyuKoTime,
                              Haisha_SyuPaTime = HAISHA.SyuPaTime,
                              Haisha_HaiSNm = HAISHA.HaiSnm,
                              Haisha_TouYmd = HAISHA.TouYmd,
                              Haisha_TouChTime = HAISHA.TouChTime,
                              Haisha_KikYmd = HAISHA.KikYmd,
                              Haisha_KikTime = HAISHA.KikTime,
                              Haisha_TouNm = HAISHA.TouNm,
                              Haisha_BunKSyuJyn=HAISHA.BunKsyuJyn,
                              Haisha_JyoSyaJin = HAISHA.JyoSyaJin,
                              Haisha_PlusJin = HAISHA.PlusJin,
                              Haisha_DrvJin = HAISHA.DrvJin,
                              Haisha_GuiSu = HAISHA.GuiSu,
                              Haisha_HaiSKouKNm = HAISHA.HaiSkouKnm,
                              Haisha_HaiSBinNm = HAISHA.HaiSbinNm,
                              Haisha_HaiSSetTime = HAISHA.HaiSsetTime,
                              Haisha_HaiSKigou = HAISHA.HaiSkigou,
                              Haisha_TouSKouKNm = HAISHA.TouSkouKnm,
                              Haisha_TouSBinNm = HAISHA.TouBinNm,
                              Haisha_TouSetTime = HAISHA.TouSetTime,
                              Haisha_TouKigou = HAISHA.TouKigou,
                              Haisha_OthJin1 = HAISHA.OthJin1,
                              Haisha_OthJin2 = HAISHA.OthJin2,
                              Haisha_PlatNo = HAISHA.PlatNo,
                              Haisha_HaiCom = HAISHA.HaiCom,
                              Haisha_SyaRyoUnc = HAISHA.SyaRyoUnc,
                              Haisha_HaiSJyus1 = HAISHA.HaiSjyus1,
                              Haisha_HaiSjyus2 = HAISHA.HaiSjyus2,
                              Haisha_TouJyusyo1 = HAISHA.TouJyusyo1,
                              Haisha_TouJyusyo2 = HAISHA.TouJyusyo2,
                              Haisha_KikEigSeq = HAISHA.KikEigSeq,
                              Haisha_YouTblSeq = HAISHA.YouTblSeq,
                              Yykasho_UkeCd = YYKASHO.UkeCd,
                              Yykasho_NippoKbn = YYKASHO.NippoKbn,
                              Yykasho_KaktYmd = YYKASHO.KaktYmd,
                              Yykasho_TokuiSeq = YYKASHO.TokuiSeq,
                              Yykasho_UkeEigCdSeq = YYKASHO.UkeEigCdSeq,
                              Tokisk_RyakuNm = TOKISK.RyakuNm,
                              Yykasho_SitenCdSeq = YYKASHO.SitenCdSeq,
                              Tokist_RyakuNm = TOKIST.RyakuNm,
                              Yykasho_InTanCdSeq = YYKASHO.InTanCdSeq,
                              Syain_SyainNm = SYAIN.SyainNm,
                              Yykasho_EigTanCdSeq = YYKASHO.EigTanCdSeq,
                              Eigyotan_SyainNm = EIGYOTAN.SyainNm,
                              Unkobi_DanTaNm = UNKOBI.DanTaNm,
                              Unkobi_HaiSYmd = UNKOBI.HaiSymd,
                              Unkobi_HaiSTime = UNKOBI.HaiStime,
                              Unkobi_TouYmd = UNKOBI.TouYmd,
                              Unkobi_TouChTime = UNKOBI.TouChTime,
                              Unkobi_ZenHaFlg = UNKOBI.ZenHaFlg,
                              Unkobi_KhakFlg = UNKOBI.KhakFlg,
                              Unkobi_SyukoYmd = UNKOBI.SyukoYmd,
                              Unkobi_KikYmd = UNKOBI.KikYmd,
                              Unkobi_UnkoJKbn = UNKOBI.UnkoJkbn,
                              Syaryo_SyaRyoNm = SYARYO.SyaRyoNm,
                              Syaryo_TeiCnt = SYARYO.TeiCnt,
                              Syasyu_SyaSyuNm = SYASYU.SyaSyuNm,
                              Syasyu_SyaSyuKigo = SYASYU.SyaSyuKigo,
                              YySyasyu_SyaSyuNm = YYSYASYU.SyaSyuNm,
                              Haiin_UkeNo = HAIIN.UkeNo,
                              Haiin_UnkRen = HAIIN.UnkRen,
                              Haiin_TeiDanNo = HAIIN.TeiDanNo,
                              Haiin_BunkRen = HAIIN.BunkRen,
                              Haiin_HaiInRen = HAIIN.HaiInRen,
                              Haiin_SyainCdSeq = HAIIN.SyainCdSeq,
                              Haiin_SyukinTime = HAIIN.SyukinTime,
                              Haiin_Syukinbasy = HAIIN.Syukinbasy,
                              Haiin_TaiknTime = HAIIN.TaiknTime,
                              Haiin_TaiknBasy = HAIIN.TaiknBasy,
                              Kyoshe_SyokumuCdSeq = KYOSHE.SyokumuCdSeq,
                              Syokum_SyokumuKbn = SYOKUM.SyokumuKbn,
                              Syokum_SyokumuNm = SYOKUM.SyokumuNm,
                              Eigyoshos_RyakuNm = EIGYOSHOS.RyakuNm,
                              Eigyoshos1_RyakuNm = EIGYOSHOS1.RyakuNm,
                              Haisha_BikoNm = HAISHA.BikoNm,
                              Yyksho_BikoNm = YYKASHO.BikoNm,
                              Unkobi_BikoNm = UNKOBI.BikoNm,
                              Yyksyu_KataKbn = (from TKD_YykSyu in _dbContext.TkdYykSyu
                                                where
                                        TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                                        TKD_YykSyu.SiyoKbn == 1
                                                select
                                                    TKD_YykSyu.KataKbn).First(),
                              Yyksyu_SyaSyuDai =
                              (from TKD_YykSyu in _dbContext.TkdYykSyu
                               where
                       TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                       TKD_YykSyu.SiyoKbn == 1
                               select new
                               {
                                   TKD_YykSyu.SyaSyuDai
                               }).Sum(p => p.SyaSyuDai),
                              UKEJOKEN_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                    where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.UkeJyKbnCd) && UKEJOKEN.CodeSyu == codeSyuUKEJYKBNCD && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantUKEJYKBNCD
                                                    select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN1_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn1) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB1 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB1
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN2_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn2) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB2 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB2
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN3_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn3) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB3 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB3
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN4_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn4) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB4 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB4
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN5_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn5) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB5 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB5
                                                     select UKEJOKEN.CodeKbnNm).First(),

                          }).ToListAsync();
        }
        public async Task<List<StaffsLines>> GetStaffdatanonassign(DateTime busstardate, DateTime busenddate, int TenantCdSeq)
        {
            string codeSyuUKEJYKBNCD = "UKEJYKBNCD";
            int tenantUKEJYKBNCD = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuUKEJYKBNCD);
            string codeSyuSIJJOKB1 = "SIJJOKBN1";
            int tenantSIJJOKB1 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB1);
            string codeSyuSIJJOKB2 = "SIJJOKBN2";
            int tenantSIJJOKB2 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB2);
            string codeSyuSIJJOKB3 = "SIJJOKBN3";
            int tenantSIJJOKB3 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB3);
            string codeSyuSIJJOKB4 = "SIJJOKBN4";
            int tenantSIJJOKB4 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB4);
            string codeSyuSIJJOKB5 = "SIJJOKBN5";
            int tenantSIJJOKB5 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB5);
            string DateStarAsString = busstardate.ToString("yyyyMMdd");
            string DateEndAsString = busenddate.ToString("yyyyMMdd");
            return await (from HAISHA in _dbContext.TkdHaisha
                          join YYKASHO in _dbContext.TkdYyksho
                                on new { HAISHA.UkeNo, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYKASHO.UkeNo, YYKASHO.TenantCdSeq }
                          join YYKSYU in _dbContext.TkdYykSyu
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen, SiyoKbn = 1 }
                            equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen, SiyoKbn = Convert.ToInt32(YYKSYU.SiyoKbn) } into YYKSYU_join
                          from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                          join YYSYASYU in _dbContext.VpmSyaSyu
                                on new { YYKSYU.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYSYASYU.SyaSyuCdSeq, YYSYASYU.TenantCdSeq } into YYSYASYU_join
                          from YYSYASYU in YYSYASYU_join.DefaultIfEmpty()
                          join HAIIN in _dbContext.TkdHaiin
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.TeiDanNo, HAISHA.BunkRen }
                            equals new { HAIIN.UkeNo, HAIIN.UnkRen, HAIIN.TeiDanNo, HAIIN.BunkRen }
                          join KYOSHE in _dbContext.VpmKyoShe on HAIIN.SyainCdSeq equals KYOSHE.SyainCdSeq into KYOSHE_join
                          from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                          join SYOKUM in _dbContext.VpmSyokum on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq into SYOKUM_join
                          from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                          join UNKOBI in _dbContext.TkdUnkobi
                                on new { HAISHA.UkeNo, HAISHA.UnkRen }
                            equals new { UNKOBI.UkeNo, UNKOBI.UnkRen } into UNKOBI_join
                          from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                          join EIGYOSHOS in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.SyuEigCdSeq } equals new { EigyoCdSeq = EIGYOSHOS.EigyoCdSeq } into EIGYOSHOS_join
                          from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                                on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                          join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                          from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                          join KAISHA1 in _dbContext.VpmCompny
                                on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq }
                          join TOKISK in _dbContext.VpmTokisk
                                on new { YYKASHO.TokuiSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { TOKISK.TokuiSeq, TOKISK.TenantCdSeq } into TOKISK_join
                          from TOKISK in TOKISK_join.DefaultIfEmpty()
                          join TOKIST in _dbContext.VpmTokiSt
                                on new { YYKASHO.TokuiSeq, YYKASHO.SitenCdSeq }
                            equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                          from TOKIST in TOKIST_join.DefaultIfEmpty()
                          join SYARYO in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = HAISHA.HaiSsryCdSeq } equals new { SyaRyoCdSeq = SYARYO.SyaRyoCdSeq } into SYARYO_join
                          from SYARYO in SYARYO_join.DefaultIfEmpty()
                          join SYASYU in _dbContext.VpmSyaSyu
                                on new { SYARYO.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { SYASYU.SyaSyuCdSeq, SYASYU.TenantCdSeq } into SYASYU_join
                          from SYASYU in SYASYU_join.DefaultIfEmpty()
                          join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                          from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                          where
                            YYKASHO.YoyaSyu == 1 &&
                          string.Compare(HAISHA.KikYmd, DateStarAsString) >= 0 &&
                           string.Compare(HAISHA.SyuKoYmd, DateEndAsString) <= 0
                            &&
                           String.Compare(TOKISK.SiyoStaYmd, DateStarAsString) <= 0 &&
                           String.Compare(TOKISK.SiyoEndYmd, DateStarAsString) >= 0 &&
                            HAISHA.Kskbn != 1 &&
                            HAISHA.YouTblSeq == 0 &&
                            HAISHA.SiyoKbn == 1 &&
                            HAIIN.SiyoKbn == 2 &&
                            YYKASHO.SiyoKbn == 1 &&
                            HAISHA.NippoKbn != 2 &&
                            YYKASHO.SeiTaiYmd.CompareTo(_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd!=null?_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd:"19100101") > 0
                          orderby
                            HAISHA.UkeNo,
                            HAIIN.HaiInRen
                          select new StaffsLines()
                          {
                              Haisha_UkeNo = HAISHA.UkeNo,
                              Haisha_UnkRen = HAISHA.UnkRen,
                              Haisha_SyaSyuRen = HAISHA.SyaSyuRen,
                              Haisha_TeiDanNo = HAISHA.TeiDanNo,
                              Haisha_BunkRen = HAISHA.BunkRen,
                              Haisha_GoSya = HAISHA.GoSya,
                              Haisha_KSKbn = HAISHA.Kskbn,
                              Haisha_HaiSKbn = HAISHA.HaiSkbn,
                              Haisha_HaiIKbn = HAISHA.HaiIkbn,
                              Haisha_KhinKbn = HAISHA.KhinKbn,
                              Haisha_NippoKbn = HAISHA.NippoKbn,
                              Haisha_DanTaNm2 = HAISHA.DanTaNm2,
                              Haisha_IkNm = HAISHA.IkNm,
                              Haisha_HaiSSryCdSeq = HAISHA.HaiSsryCdSeq,
                              Haisha_SyuEigCdSeq = HAISHA.SyuEigCdSeq,
                              Haisha_HaiSYmd = HAISHA.HaiSymd,
                              Haisha_HaiSTime = HAISHA.HaiStime,
                              Haisha_SyuKoYmd = HAISHA.SyuKoYmd,
                              Haisha_SyuKoTime = HAISHA.SyuKoTime,
                              Haisha_SyuPaTime = HAISHA.SyuPaTime,
                              Haisha_HaiSNm = HAISHA.HaiSnm,
                              Haisha_TouYmd = HAISHA.TouYmd,
                              Haisha_TouChTime = HAISHA.TouChTime,
                              Haisha_KikYmd = HAISHA.KikYmd,
                              Haisha_KikTime = HAISHA.KikTime,
                              Haisha_TouNm = HAISHA.TouNm,
                              Haisha_JyoSyaJin = HAISHA.JyoSyaJin,
                              Haisha_PlusJin = HAISHA.PlusJin,
                              Haisha_DrvJin = HAISHA.DrvJin,
                              Haisha_GuiSu = HAISHA.GuiSu,
                              Haisha_HaiSKouKNm = HAISHA.HaiSkouKnm,
                              Haisha_HaiSBinNm = HAISHA.HaiSbinNm,
                              Haisha_HaiSSetTime = HAISHA.HaiSsetTime,
                              Haisha_HaiSKigou = HAISHA.HaiSkigou,
                              Haisha_TouSKouKNm = HAISHA.TouSkouKnm,
                              Haisha_TouSBinNm = HAISHA.TouBinNm,
                              Haisha_TouSetTime = HAISHA.TouSetTime,
                              Haisha_TouKigou = HAISHA.TouKigou,
                              Haisha_OthJin1 = HAISHA.OthJin1,
                              Haisha_OthJin2 = HAISHA.OthJin2,
                              Haisha_PlatNo = HAISHA.PlatNo,
                              Haisha_HaiCom = HAISHA.HaiCom,
                              Haisha_SyaRyoUnc = HAISHA.SyaRyoUnc,
                              Haisha_HaiSJyus1 = HAISHA.HaiSjyus1,
                              Haisha_HaiSjyus2 = HAISHA.HaiSjyus2,
                              Haisha_TouJyusyo1 = HAISHA.TouJyusyo1,
                              Haisha_TouJyusyo2 = HAISHA.TouJyusyo2,
                              Haisha_KikEigSeq = HAISHA.KikEigSeq,
                              Haisha_YouTblSeq = HAISHA.YouTblSeq,
                              Yykasho_UkeCd = YYKASHO.UkeCd,
                              Yykasho_NippoKbn = YYKASHO.NippoKbn,
                              Yykasho_KaktYmd = YYKASHO.KaktYmd,
                              Yykasho_TokuiSeq = YYKASHO.TokuiSeq,
                              Yykasho_UkeEigCdSeq = YYKASHO.UkeEigCdSeq,
                              Tokisk_RyakuNm = TOKISK.RyakuNm,
                              Yykasho_SitenCdSeq = YYKASHO.SitenCdSeq,
                              Tokist_RyakuNm = TOKIST.RyakuNm,
                              Yykasho_InTanCdSeq = YYKASHO.InTanCdSeq,
                              Syain_SyainNm = SYAIN.SyainNm,
                              Yykasho_EigTanCdSeq = YYKASHO.EigTanCdSeq,
                              Eigyotan_SyainNm = EIGYOTAN.SyainNm,
                              Unkobi_DanTaNm = UNKOBI.DanTaNm,
                              Unkobi_HaiSYmd = UNKOBI.HaiSymd,
                              Unkobi_HaiSTime = UNKOBI.HaiStime,
                              Unkobi_TouYmd = UNKOBI.TouYmd,
                              Unkobi_TouChTime = UNKOBI.TouChTime,
                              Unkobi_ZenHaFlg = UNKOBI.ZenHaFlg,
                              Unkobi_KhakFlg = UNKOBI.KhakFlg,
                              Unkobi_SyukoYmd = UNKOBI.SyukoYmd,
                              Unkobi_KikYmd = UNKOBI.KikYmd,
                              Unkobi_UnkoJKbn = UNKOBI.UnkoJkbn,
                              Syaryo_SyaRyoNm = SYARYO.SyaRyoNm,
                              Syaryo_TeiCnt = SYARYO.TeiCnt,
                              Syasyu_SyaSyuNm = SYASYU.SyaSyuNm,
                              Syasyu_SyaSyuKigo = SYASYU.SyaSyuKigo,
                              YySyasyu_SyaSyuNm = YYSYASYU.SyaSyuNm,
                              Haiin_UkeNo = HAIIN.UkeNo,
                              Haiin_UnkRen = HAIIN.UnkRen,
                              Haiin_TeiDanNo = HAIIN.TeiDanNo,
                              Haiin_BunkRen = HAIIN.BunkRen,
                              Haiin_HaiInRen = HAIIN.HaiInRen,
                              Haiin_SyainCdSeq = HAIIN.SyainCdSeq,
                              Haiin_SyukinTime = HAIIN.SyukinTime,
                              Haiin_Syukinbasy = HAIIN.Syukinbasy,
                              Haiin_TaiknTime = HAIIN.TaiknTime,
                              Haiin_TaiknBasy = HAIIN.TaiknBasy,
                              Kyoshe_SyokumuCdSeq = KYOSHE.SyokumuCdSeq,
                              Syokum_SyokumuKbn = SYOKUM.SyokumuKbn,
                              Syokum_SyokumuNm = SYOKUM.SyokumuNm,
                              Eigyoshos_RyakuNm = EIGYOSHOS.RyakuNm,
                              Eigyoshos1_RyakuNm = EIGYOSHOS1.RyakuNm,
                              Haisha_BikoNm = HAISHA.BikoNm,
                              Yyksho_BikoNm = YYKASHO.BikoNm,
                              Unkobi_BikoNm = UNKOBI.BikoNm,
                              Yyksyu_KataKbn = (from TKD_YykSyu in _dbContext.TkdYykSyu
                                                where
                                        TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                                        TKD_YykSyu.SiyoKbn == 1
                                                select
                                                    TKD_YykSyu.KataKbn).First(),
                              Yyksyu_SyaSyuDai =
                              (from TKD_YykSyu in _dbContext.TkdYykSyu
                               where
                       TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                       TKD_YykSyu.SiyoKbn == 1
                               select new
                               {
                                   TKD_YykSyu.SyaSyuDai
                               }).Sum(p => p.SyaSyuDai),
                              UKEJOKEN_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                    where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.UkeJyKbnCd) && UKEJOKEN.CodeSyu == codeSyuUKEJYKBNCD && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantUKEJYKBNCD
                                                    select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN1_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn1) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB1 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB1
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN2_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn2) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB2 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB2
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN3_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn3) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB3 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB3
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN4_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn4) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB4 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB4
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN5_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn5) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB5 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB5
                                                     select UKEJOKEN.CodeKbnNm).First(),

                          }).ToListAsync();
        }
       
        public async Task<List<StaffsLines>> GetStaffdatahaiiinBusallocation(string ukeno,short unkren,short teidanno, short bunkren ,int TenantCdSeq)
        {
            string codeSyuUKEJYKBNCD = "UKEJYKBNCD";
            var UKEJYKBNCD = _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuUKEJYKBNCD);
            string codeSyuSIJJOKB1 = "SIJJOKBN1";
            var SIJJOKB1 = _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB1);
            string codeSyuSIJJOKB2 = "SIJJOKBN1";
            var SIJJOKB2 = _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB2);
            string codeSyuSIJJOKB3 = "SIJJOKBN1";
            var SIJJOKB3 = _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB3);
            string codeSyuSIJJOKB4 = "SIJJOKBN1";
            var SIJJOKB4 = _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB4);
            string codeSyuSIJJOKB5 = "SIJJOKBN1";
            var SIJJOKB5 = _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB5);

            await Task.WhenAll(UKEJYKBNCD, SIJJOKB1, SIJJOKB2, SIJJOKB3, SIJJOKB4, SIJJOKB5);

            int tenantUKEJYKBNCD = await UKEJYKBNCD;
            int tenantSIJJOKB1 = await SIJJOKB1;
            int tenantSIJJOKB2 = await SIJJOKB2;
            int tenantSIJJOKB3 = await SIJJOKB3;
            int tenantSIJJOKB4 = await SIJJOKB4;
            int tenantSIJJOKB5 = await SIJJOKB5;

            return await 
                (from HAISHA in _dbContext.TkdHaisha
                          join YYKASHO in _dbContext.TkdYyksho
                                on new { HAISHA.UkeNo, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYKASHO.UkeNo, YYKASHO.TenantCdSeq }
                          join YYKSYU in _dbContext.TkdYykSyu
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen, SiyoKbn = 1 }
                            equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen, SiyoKbn = Convert.ToInt32(YYKSYU.SiyoKbn) } into YYKSYU_join
                          from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                          join YYSYASYU in _dbContext.VpmSyaSyu
                                on new { YYKSYU.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYSYASYU.SyaSyuCdSeq, YYSYASYU.TenantCdSeq } into YYSYASYU_join
                          from YYSYASYU in YYSYASYU_join.DefaultIfEmpty()
                          join HAIIN in _dbContext.TkdHaiin
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.TeiDanNo, HAISHA.BunkRen }
                            equals new { HAIIN.UkeNo, HAIIN.UnkRen, HAIIN.TeiDanNo, HAIIN.BunkRen }
                          join KYOSHE in _dbContext.VpmKyoShe on HAIIN.SyainCdSeq equals KYOSHE.SyainCdSeq into KYOSHE_join
                          from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                          join SYOKUM in _dbContext.VpmSyokum on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq into SYOKUM_join
                          from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                          join UNKOBI in _dbContext.TkdUnkobi
                                on new { HAISHA.UkeNo, HAISHA.UnkRen }
                            equals new { UNKOBI.UkeNo, UNKOBI.UnkRen } into UNKOBI_join
                          from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                          join EIGYOSHOS in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.SyuEigCdSeq } equals new { EigyoCdSeq = EIGYOSHOS.EigyoCdSeq } into EIGYOSHOS_join
                          from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                          /*join KAISHA in _dbContext.VpmCompny
                                on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }*/
                          join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                          from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                          /*join KAISHA1 in _dbContext.VpmCompny
                                on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq }*/
                          join TOKISK in _dbContext.VpmTokisk
                                on new { YYKASHO.TokuiSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { TOKISK.TokuiSeq, TOKISK.TenantCdSeq } into TOKISK_join
                          from TOKISK in TOKISK_join.DefaultIfEmpty()
                          join TOKIST in _dbContext.VpmTokiSt
                                on new { YYKASHO.TokuiSeq, YYKASHO.SitenCdSeq }
                            equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                          from TOKIST in TOKIST_join.DefaultIfEmpty()
                          join SYARYO in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = HAISHA.HaiSsryCdSeq } equals new { SyaRyoCdSeq = SYARYO.SyaRyoCdSeq } into SYARYO_join
                          from SYARYO in SYARYO_join.DefaultIfEmpty()
                          join SYASYU in _dbContext.VpmSyaSyu
                                on new { SYARYO.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { SYASYU.SyaSyuCdSeq, SYASYU.TenantCdSeq } into SYASYU_join
                          from SYASYU in SYASYU_join.DefaultIfEmpty()
                          join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                          from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                          where
                            YYKASHO.YoyaSyu == 1 &&
                            //HAISHA.Kskbn != 1 &&
                            HAISHA.YouTblSeq == 0 &&
                            HAISHA.SiyoKbn == 1 &&
                            HAIIN.SiyoKbn == 1 &&
                            HAIIN.UkeNo == ukeno &&
                            HAIIN.TeiDanNo==teidanno &&
                            HAIIN.UnkRen==unkren&&
                            YYKASHO.SiyoKbn == 1 &&
                            //HAISHA.NippoKbn != 2 &&
                            HAIIN.BunkRen == bunkren &&
                            YYKASHO.SeiTaiYmd.CompareTo(_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd!=null?_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd:"19100101") > 0
                          orderby
                            HAISHA.UkeNo,
                            HAIIN.HaiInRen
                          select new StaffsLines()
                          {
                              Haisha_UkeNo = HAISHA.UkeNo,
                              Haisha_UnkRen = HAISHA.UnkRen,
                              Haisha_SyaSyuRen = HAISHA.SyaSyuRen,
                              Haisha_TeiDanNo = HAISHA.TeiDanNo,
                              Haisha_BunkRen = HAISHA.BunkRen,
                              Haisha_GoSya = HAISHA.GoSya,
                              Haisha_KSKbn = HAISHA.Kskbn,
                              Haisha_HaiSKbn = HAISHA.HaiSkbn,
                              Haisha_HaiIKbn = HAISHA.HaiIkbn,
                              Haisha_KhinKbn = HAISHA.KhinKbn,
                              Haisha_NippoKbn = HAISHA.NippoKbn,
                              Haisha_DanTaNm2 = HAISHA.DanTaNm2,
                              Haisha_IkNm = HAISHA.IkNm,
                              Haisha_HaiSSryCdSeq = HAISHA.HaiSsryCdSeq,
                              Haisha_SyuEigCdSeq = HAISHA.SyuEigCdSeq,
                              Haisha_HaiSYmd = HAISHA.HaiSymd,
                              Haisha_HaiSTime = HAISHA.HaiStime,
                              Haisha_SyuKoYmd = HAISHA.SyuKoYmd,
                              Haisha_SyuKoTime = HAISHA.SyuKoTime,
                              Haisha_SyuPaTime = HAISHA.SyuPaTime,
                              Haisha_HaiSNm = HAISHA.HaiSnm,
                              Haisha_TouYmd = HAISHA.TouYmd,
                              Haisha_TouChTime = HAISHA.TouChTime,
                              Haisha_KikYmd = HAISHA.KikYmd,
                              Haisha_KikTime = HAISHA.KikTime,
                              Haisha_TouNm = HAISHA.TouNm,
                              Haisha_JyoSyaJin = HAISHA.JyoSyaJin,
                              Haisha_PlusJin = HAISHA.PlusJin,
                              Haisha_DrvJin = HAISHA.DrvJin,
                              Haisha_GuiSu = HAISHA.GuiSu,
                              Haisha_HaiSKouKNm = HAISHA.HaiSkouKnm,
                              Haisha_HaiSBinNm = HAISHA.HaiSbinNm,
                              Haisha_HaiSSetTime = HAISHA.HaiSsetTime,
                              Haisha_HaiSKigou = HAISHA.HaiSkigou,
                              Haisha_TouSKouKNm = HAISHA.TouSkouKnm,
                              Haisha_BunKSyuJyn=HAISHA.BunKsyuJyn,
                              Haisha_TouSBinNm = HAISHA.TouBinNm,
                              Haisha_TouSetTime = HAISHA.TouSetTime,
                              Haisha_TouKigou = HAISHA.TouKigou,
                              Haisha_OthJin1 = HAISHA.OthJin1,
                              Haisha_OthJin2 = HAISHA.OthJin2,
                              Haisha_PlatNo = HAISHA.PlatNo,
                              Haisha_HaiCom = HAISHA.HaiCom,
                              Haisha_SyaRyoUnc = HAISHA.SyaRyoUnc,
                              Haisha_HaiSJyus1 = HAISHA.HaiSjyus1,
                              Haisha_HaiSjyus2 = HAISHA.HaiSjyus2,
                              Haisha_TouJyusyo1 = HAISHA.TouJyusyo1,
                              Haisha_TouJyusyo2 = HAISHA.TouJyusyo2,
                              Haisha_KikEigSeq = HAISHA.KikEigSeq,
                              Haisha_YouTblSeq = HAISHA.YouTblSeq,
                              Yykasho_UkeCd = YYKASHO.UkeCd,
                              Yykasho_NippoKbn = YYKASHO.NippoKbn,
                              Yykasho_KaktYmd = YYKASHO.KaktYmd,
                              Yykasho_TokuiSeq = YYKASHO.TokuiSeq,
                              Yykasho_UkeEigCdSeq = YYKASHO.UkeEigCdSeq,
                              Tokisk_RyakuNm = TOKISK.RyakuNm,
                              Yykasho_SitenCdSeq = YYKASHO.SitenCdSeq,
                              Tokist_RyakuNm = TOKIST.RyakuNm,
                              Yykasho_InTanCdSeq = YYKASHO.InTanCdSeq,
                              Syain_SyainNm = SYAIN.SyainNm,
                              Yykasho_EigTanCdSeq = YYKASHO.EigTanCdSeq,
                              Eigyotan_SyainNm = EIGYOTAN.SyainNm,
                              Unkobi_DanTaNm = UNKOBI.DanTaNm,
                              Unkobi_HaiSYmd = UNKOBI.HaiSymd,
                              Unkobi_HaiSTime = UNKOBI.HaiStime,
                              Unkobi_TouYmd = UNKOBI.TouYmd,
                              Unkobi_TouChTime = UNKOBI.TouChTime,
                              Unkobi_ZenHaFlg = UNKOBI.ZenHaFlg,
                              Unkobi_KhakFlg = UNKOBI.KhakFlg,
                              Unkobi_SyukoYmd = UNKOBI.SyukoYmd,
                              Unkobi_KikYmd = UNKOBI.KikYmd,
                              Unkobi_UnkoJKbn = UNKOBI.UnkoJkbn,
                              Syaryo_SyaRyoNm = SYARYO.SyaRyoNm,
                              Syaryo_TeiCnt = SYARYO.TeiCnt,
                              Syasyu_SyaSyuNm = SYASYU.SyaSyuNm,
                              Syasyu_SyaSyuKigo = SYASYU.SyaSyuKigo,
                              YySyasyu_SyaSyuNm = YYSYASYU.SyaSyuNm,
                              Haiin_UkeNo = HAIIN.UkeNo,
                              Haiin_UnkRen = HAIIN.UnkRen,
                              Haiin_TeiDanNo = HAIIN.TeiDanNo,
                              Haiin_BunkRen = HAIIN.BunkRen,
                              Haiin_HaiInRen = HAIIN.HaiInRen,
                              Haiin_SyainCdSeq = HAIIN.SyainCdSeq,
                              Haiin_SyukinTime = HAIIN.SyukinTime,
                              Haiin_Syukinbasy = HAIIN.Syukinbasy,
                              Haiin_TaiknTime = HAIIN.TaiknTime,
                              Haiin_TaiknBasy = HAIIN.TaiknBasy,
                              Kyoshe_SyokumuCdSeq = KYOSHE.SyokumuCdSeq,
                              Syokum_SyokumuKbn = SYOKUM.SyokumuKbn,
                              Syokum_SyokumuNm = SYOKUM.SyokumuNm,
                              Eigyoshos_RyakuNm = EIGYOSHOS.RyakuNm,
                              Eigyoshos1_RyakuNm = EIGYOSHOS1.RyakuNm,
                              Haisha_BikoNm = HAISHA.BikoNm,
                              Yyksho_BikoNm = YYKASHO.BikoNm,
                              Unkobi_BikoNm = UNKOBI.BikoNm,
                              Yyksyu_KataKbn = (from TKD_YykSyu in _dbContext.TkdYykSyu
                                                where
                                        TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                                        TKD_YykSyu.SiyoKbn == 1
                                                select
                                                    TKD_YykSyu.KataKbn).First(),
                              Yyksyu_SyaSyuDai =
                              (from TKD_YykSyu in _dbContext.TkdYykSyu
                               where
                       TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                       TKD_YykSyu.SiyoKbn == 1
                               select new
                               {
                                   TKD_YykSyu.SyaSyuDai
                               }).Sum(p => p.SyaSyuDai),
                              UKEJOKEN_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                    where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.UkeJyKbnCd) && UKEJOKEN.CodeSyu == codeSyuUKEJYKBNCD && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantUKEJYKBNCD
                                                    select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN1_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn1) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB1 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB1
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN2_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn2) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB2 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB2
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN3_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn3) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB3 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB3
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN4_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn4) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB4 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB4
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN5_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn5) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB5 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB5
                                                     select UKEJOKEN.CodeKbnNm).First(),

                          }).ToListAsync();
        }
        public async Task<List<StaffsLines>> GetStaffdatahaiiin(string ukeno, short unkren, short teidanno, int YoyaKbnSeq, int TenantCdSeq)
        {
            string codeSyuUKEJYKBNCD = "UKEJYKBNCD";
            int tenantUKEJYKBNCD = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuUKEJYKBNCD);
            string codeSyuSIJJOKB1 = "SIJJOKBN1";
            int tenantSIJJOKB1 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB1);
            string codeSyuSIJJOKB2 = "SIJJOKBN2";
            int tenantSIJJOKB2 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB2);
            string codeSyuSIJJOKB3 = "SIJJOKBN3";
            int tenantSIJJOKB3 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB3);
            string codeSyuSIJJOKB4 = "SIJJOKBN4";
            int tenantSIJJOKB4 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB4);
            string codeSyuSIJJOKB5 = "SIJJOKBN5";
            int tenantSIJJOKB5 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB5);
            return (from HAISHA in _dbContext.TkdHaisha
                    join YYKASHO in _dbContext.TkdYyksho
                          on new { HAISHA.UkeNo, TenantCdSeq =  new  ClaimModel().TenantID }
                      equals new { YYKASHO.UkeNo, YYKASHO.TenantCdSeq }
                    join YYKSYU in _dbContext.TkdYykSyu
                          on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen, SiyoKbn = 1 }
                      equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen, SiyoKbn = Convert.ToInt32(YYKSYU.SiyoKbn) } into YYKSYU_join
                    from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                    join YYSYASYU in _dbContext.VpmSyaSyu
                          on new { YYKSYU.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                      equals new { YYSYASYU.SyaSyuCdSeq, YYSYASYU.TenantCdSeq } into YYSYASYU_join
                    from YYSYASYU in YYSYASYU_join.DefaultIfEmpty()
                    join HAIIN in _dbContext.TkdHaiin
                          on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.TeiDanNo, HAISHA.BunkRen }
                      equals new { HAIIN.UkeNo, HAIIN.UnkRen, HAIIN.TeiDanNo, HAIIN.BunkRen }
                    join KYOSHE in _dbContext.VpmKyoShe on HAIIN.SyainCdSeq equals KYOSHE.SyainCdSeq into KYOSHE_join
                    from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                    join SYOKUM in _dbContext.VpmSyokum on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq into SYOKUM_join
                    from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                    join UNKOBI in _dbContext.TkdUnkobi
                          on new { HAISHA.UkeNo, HAISHA.UnkRen }
                      equals new { UNKOBI.UkeNo, UNKOBI.UnkRen } into UNKOBI_join
                    from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                    join EIGYOSHOS in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.SyuEigCdSeq } equals new { EigyoCdSeq = EIGYOSHOS.EigyoCdSeq } into EIGYOSHOS_join
                    from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                    join KAISHA in _dbContext.VpmCompny
                          on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                      equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                    join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                    from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                    join KAISHA1 in _dbContext.VpmCompny
                          on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                      equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq }
                    join TOKISK in _dbContext.VpmTokisk
                          on new { YYKASHO.TokuiSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                      equals new { TOKISK.TokuiSeq, TOKISK.TenantCdSeq } into TOKISK_join
                    from TOKISK in TOKISK_join.DefaultIfEmpty()
                    join TOKIST in _dbContext.VpmTokiSt
                          on new { YYKASHO.TokuiSeq, YYKASHO.SitenCdSeq }
                      equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                    from TOKIST in TOKIST_join.DefaultIfEmpty()
                    join SYARYO in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = HAISHA.HaiSsryCdSeq } equals new { SyaRyoCdSeq = SYARYO.SyaRyoCdSeq } into SYARYO_join
                    from SYARYO in SYARYO_join.DefaultIfEmpty()
                    join SYASYU in _dbContext.VpmSyaSyu
                          on new { SYARYO.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                      equals new { SYASYU.SyaSyuCdSeq, SYASYU.TenantCdSeq } into SYASYU_join
                    from SYASYU in SYASYU_join.DefaultIfEmpty()
                    join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                    from SYAIN in SYAIN_join.DefaultIfEmpty()
                    join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                    from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                    where
                      YYKASHO.YoyaSyu == 1 &&
                      YYKASHO.YoyaKbnSeq == 1 &&
                      HAISHA.Kskbn != 1 &&
                      HAISHA.YouTblSeq == 0 &&
                      HAISHA.SiyoKbn == 1 &&
                      HAIIN.SiyoKbn == 1 &&
                      HAIIN.UkeNo == ukeno &&
                      HAIIN.TeiDanNo == teidanno &&
                      HAIIN.UnkRen == unkren &&
                      YYKASHO.SiyoKbn == 1 &&
                      HAISHA.NippoKbn != 2 &&
                      YYKASHO.SeiTaiYmd.CompareTo(_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd != null ? _dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd : "19100101") > 0
                    orderby
                      HAISHA.UkeNo,
                      HAIIN.HaiInRen
                    select new StaffsLines()
                    {
                        Haisha_UkeNo = HAISHA.UkeNo,
                        Haisha_UnkRen = HAISHA.UnkRen,
                        Haisha_SyaSyuRen = HAISHA.SyaSyuRen,
                        Haisha_TeiDanNo = HAISHA.TeiDanNo,
                        Haisha_BunkRen = HAISHA.BunkRen,
                        Haisha_GoSya = HAISHA.GoSya,
                        Haisha_KSKbn = HAISHA.Kskbn,
                        Haisha_HaiSKbn = HAISHA.HaiSkbn,
                        Haisha_HaiIKbn = HAISHA.HaiIkbn,
                        Haisha_KhinKbn = HAISHA.KhinKbn,
                        Haisha_NippoKbn = HAISHA.NippoKbn,
                        Haisha_DanTaNm2 = HAISHA.DanTaNm2,
                        Haisha_IkNm = HAISHA.IkNm,
                        Haisha_HaiSSryCdSeq = HAISHA.HaiSsryCdSeq,
                        Haisha_SyuEigCdSeq = HAISHA.SyuEigCdSeq,
                        Haisha_HaiSYmd = HAISHA.HaiSymd,
                        Haisha_HaiSTime = HAISHA.HaiStime,
                        Haisha_SyuKoYmd = HAISHA.SyuKoYmd,
                        Haisha_SyuKoTime = HAISHA.SyuKoTime,
                        Haisha_SyuPaTime = HAISHA.SyuPaTime,
                        Haisha_HaiSNm = HAISHA.HaiSnm,
                        Haisha_TouYmd = HAISHA.TouYmd,
                        Haisha_TouChTime = HAISHA.TouChTime,
                        Haisha_KikYmd = HAISHA.KikYmd,
                        Haisha_KikTime = HAISHA.KikTime,
                        Haisha_TouNm = HAISHA.TouNm,
                        Haisha_JyoSyaJin = HAISHA.JyoSyaJin,
                        Haisha_PlusJin = HAISHA.PlusJin,
                        Haisha_DrvJin = HAISHA.DrvJin,
                        Haisha_GuiSu = HAISHA.GuiSu,
                        Haisha_HaiSKouKNm = HAISHA.HaiSkouKnm,
                        Haisha_HaiSBinNm = HAISHA.HaiSbinNm,
                        Haisha_HaiSSetTime = HAISHA.HaiSsetTime,
                        Haisha_HaiSKigou = HAISHA.HaiSkigou,
                        Haisha_TouSKouKNm = HAISHA.TouSkouKnm,
                        Haisha_BunKSyuJyn = HAISHA.BunKsyuJyn,
                        Haisha_TouSBinNm = HAISHA.TouBinNm,
                        Haisha_TouSetTime = HAISHA.TouSetTime,
                        Haisha_TouKigou = HAISHA.TouKigou,
                        Haisha_OthJin1 = HAISHA.OthJin1,
                        Haisha_OthJin2 = HAISHA.OthJin2,
                        Haisha_PlatNo = HAISHA.PlatNo,
                        Haisha_HaiCom = HAISHA.HaiCom,
                        Haisha_SyaRyoUnc = HAISHA.SyaRyoUnc,
                        Haisha_HaiSJyus1 = HAISHA.HaiSjyus1,
                        Haisha_HaiSjyus2 = HAISHA.HaiSjyus2,
                        Haisha_TouJyusyo1 = HAISHA.TouJyusyo1,
                        Haisha_TouJyusyo2 = HAISHA.TouJyusyo2,
                        Haisha_KikEigSeq = HAISHA.KikEigSeq,
                        Haisha_YouTblSeq = HAISHA.YouTblSeq,
                        Yykasho_UkeCd = YYKASHO.UkeCd,
                        Yykasho_NippoKbn = YYKASHO.NippoKbn,
                        Yykasho_KaktYmd = YYKASHO.KaktYmd,
                        Yykasho_TokuiSeq = YYKASHO.TokuiSeq,
                        Yykasho_UkeEigCdSeq = YYKASHO.UkeEigCdSeq,
                        Tokisk_RyakuNm = TOKISK.RyakuNm,
                        Yykasho_SitenCdSeq = YYKASHO.SitenCdSeq,
                        Tokist_RyakuNm = TOKIST.RyakuNm,
                        Yykasho_InTanCdSeq = YYKASHO.InTanCdSeq,
                        Syain_SyainNm = SYAIN.SyainNm,
                        Yykasho_EigTanCdSeq = YYKASHO.EigTanCdSeq,
                        Eigyotan_SyainNm = EIGYOTAN.SyainNm,
                        Unkobi_DanTaNm = UNKOBI.DanTaNm,
                        Unkobi_HaiSYmd = UNKOBI.HaiSymd,
                        Unkobi_HaiSTime = UNKOBI.HaiStime,
                        Unkobi_TouYmd = UNKOBI.TouYmd,
                        Unkobi_TouChTime = UNKOBI.TouChTime,
                        Unkobi_ZenHaFlg = UNKOBI.ZenHaFlg,
                        Unkobi_KhakFlg = UNKOBI.KhakFlg,
                        Unkobi_SyukoYmd = UNKOBI.SyukoYmd,
                        Unkobi_KikYmd = UNKOBI.KikYmd,
                        Unkobi_UnkoJKbn = UNKOBI.UnkoJkbn,
                        Syaryo_SyaRyoNm = SYARYO.SyaRyoNm,
                        Syaryo_TeiCnt = SYARYO.TeiCnt,
                        Syasyu_SyaSyuNm = SYASYU.SyaSyuNm,
                        Syasyu_SyaSyuKigo = SYASYU.SyaSyuKigo,
                        YySyasyu_SyaSyuNm = YYSYASYU.SyaSyuNm,
                        Haiin_UkeNo = HAIIN.UkeNo,
                        Haiin_UnkRen = HAIIN.UnkRen,
                        Haiin_TeiDanNo = HAIIN.TeiDanNo,
                        Haiin_BunkRen = HAIIN.BunkRen,
                        Haiin_HaiInRen = HAIIN.HaiInRen,
                        Haiin_SyainCdSeq = HAIIN.SyainCdSeq,
                        Haiin_SyukinTime = HAIIN.SyukinTime,
                        Haiin_Syukinbasy = HAIIN.Syukinbasy,
                        Haiin_TaiknTime = HAIIN.TaiknTime,
                        Haiin_TaiknBasy = HAIIN.TaiknBasy,
                        Kyoshe_SyokumuCdSeq = KYOSHE.SyokumuCdSeq,
                        Syokum_SyokumuKbn = SYOKUM.SyokumuKbn,
                        Syokum_SyokumuNm = SYOKUM.SyokumuNm,
                        Eigyoshos_RyakuNm = EIGYOSHOS.RyakuNm,
                        Eigyoshos1_RyakuNm = EIGYOSHOS1.RyakuNm,
                        Haisha_BikoNm = HAISHA.BikoNm,
                        Yyksho_BikoNm = YYKASHO.BikoNm,
                        Unkobi_BikoNm = UNKOBI.BikoNm,
                        Yyksyu_KataKbn = (from TKD_YykSyu in _dbContext.TkdYykSyu
                                          where
                                  TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                                  TKD_YykSyu.SiyoKbn == 1
                                          select
                                              TKD_YykSyu.KataKbn).First(),
                        Yyksyu_SyaSyuDai =
                        (from TKD_YykSyu in _dbContext.TkdYykSyu
                         where
                 TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                 TKD_YykSyu.SiyoKbn == 1
                         select new
                         {
                             TKD_YykSyu.SyaSyuDai
                         }).Sum(p => p.SyaSyuDai),
                        UKEJOKEN_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                              where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.UkeJyKbnCd) && UKEJOKEN.CodeSyu == codeSyuUKEJYKBNCD && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantUKEJYKBNCD
                                              select UKEJOKEN.CodeKbnNm).First(),
                        SIJJOKBN1_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                               where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn1) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB1 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB1
                                               select UKEJOKEN.CodeKbnNm).First(),
                        SIJJOKBN2_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                               where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn2) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB2 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB2
                                               select UKEJOKEN.CodeKbnNm).First(),
                        SIJJOKBN3_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                               where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn3) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB3 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB3
                                               select UKEJOKEN.CodeKbnNm).First(),
                        SIJJOKBN4_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                               where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn4) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB4 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB4
                                               select UKEJOKEN.CodeKbnNm).First(),
                        SIJJOKBN5_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                               where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn5) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB5 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB5
                                               select UKEJOKEN.CodeKbnNm).First(),

                    }).ToList();
        }
        public async Task<List<StaffsLines>> GetStaffdatahaiiinbooking(string ukeno,short unkren,short teidanno, short bunkRen, int TenantCdSeq)
        {
            string codeSyuUKEJYKBNCD = "UKEJYKBNCD";
            int tenantUKEJYKBNCD = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuUKEJYKBNCD);
            string codeSyuSIJJOKB1 = "SIJJOKBN1";
            int tenantSIJJOKB1 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB1);
            string codeSyuSIJJOKB2 = "SIJJOKBN2";
            int tenantSIJJOKB2 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB2);
            string codeSyuSIJJOKB3 = "SIJJOKBN3";
            int tenantSIJJOKB3 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB3);
            string codeSyuSIJJOKB4 = "SIJJOKBN4";
            int tenantSIJJOKB4 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB4);
            string codeSyuSIJJOKB5 = "SIJJOKBN5";
            int tenantSIJJOKB5 = await _codeSyuService.CheckTenantByKanriKbnAsync(TenantCdSeq, codeSyuSIJJOKB5);
            return await (from HAISHA in _dbContext.TkdHaisha
                          join YYKASHO in _dbContext.TkdYyksho
                                on new { HAISHA.UkeNo, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYKASHO.UkeNo, YYKASHO.TenantCdSeq }
                          join YYKSYU in _dbContext.TkdYykSyu
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen, SiyoKbn = 1 }
                            equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen, SiyoKbn = Convert.ToInt32(YYKSYU.SiyoKbn) } into YYKSYU_join
                          from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                          join YYSYASYU in _dbContext.VpmSyaSyu
                                on new { YYKSYU.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { YYSYASYU.SyaSyuCdSeq, YYSYASYU.TenantCdSeq } into YYSYASYU_join
                          from YYSYASYU in YYSYASYU_join.DefaultIfEmpty()
                          join HAIIN in _dbContext.TkdHaiin
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.TeiDanNo, HAISHA.BunkRen }
                            equals new { HAIIN.UkeNo, HAIIN.UnkRen, HAIIN.TeiDanNo, HAIIN.BunkRen }
                          join KYOSHE in _dbContext.VpmKyoShe on HAIIN.SyainCdSeq equals KYOSHE.SyainCdSeq into KYOSHE_join
                          from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                          join SYOKUM in _dbContext.VpmSyokum on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq into SYOKUM_join
                          from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                          join UNKOBI in _dbContext.TkdUnkobi
                                on new { HAISHA.UkeNo, HAISHA.UnkRen }
                            equals new { UNKOBI.UkeNo, UNKOBI.UnkRen } into UNKOBI_join
                          from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                          join EIGYOSHOS in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.SyuEigCdSeq } equals new { EigyoCdSeq = EIGYOSHOS.EigyoCdSeq } into EIGYOSHOS_join
                          from EIGYOSHOS in EIGYOSHOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                                on new { EIGYOSHOS.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                          join EIGYOSHOS1 in _dbContext.VpmEigyos on new { EigyoCdSeq = HAISHA.KikEigSeq } equals new { EigyoCdSeq = EIGYOSHOS1.EigyoCdSeq } into EIGYOSHOS1_join
                          from EIGYOSHOS1 in EIGYOSHOS1_join.DefaultIfEmpty()
                          join KAISHA1 in _dbContext.VpmCompny
                                on new { EIGYOSHOS1.CompanyCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { KAISHA1.CompanyCdSeq, KAISHA1.TenantCdSeq }
                          join TOKISK in _dbContext.VpmTokisk
                                on new { YYKASHO.TokuiSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { TOKISK.TokuiSeq, TOKISK.TenantCdSeq } into TOKISK_join
                          from TOKISK in TOKISK_join.DefaultIfEmpty()
                          join TOKIST in _dbContext.VpmTokiSt
                                on new { YYKASHO.TokuiSeq, YYKASHO.SitenCdSeq }
                            equals new { TOKIST.TokuiSeq, TOKIST.SitenCdSeq } into TOKIST_join
                          from TOKIST in TOKIST_join.DefaultIfEmpty()
                          join SYARYO in _dbContext.VpmSyaRyo on new { SyaRyoCdSeq = HAISHA.HaiSsryCdSeq } equals new { SyaRyoCdSeq = SYARYO.SyaRyoCdSeq } into SYARYO_join
                          from SYARYO in SYARYO_join.DefaultIfEmpty()
                          join SYASYU in _dbContext.VpmSyaSyu
                                on new { SYARYO.SyaSyuCdSeq, TenantCdSeq =  new  ClaimModel().TenantID }
                            equals new { SYASYU.SyaSyuCdSeq, SYASYU.TenantCdSeq } into SYASYU_join
                          from SYASYU in SYASYU_join.DefaultIfEmpty()
                          join SYAIN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.InTanCdSeq } equals new { SyainCdSeq = SYAIN.SyainCdSeq } into SYAIN_join
                          from SYAIN in SYAIN_join.DefaultIfEmpty()
                          join EIGYOTAN in _dbContext.VpmSyain on new { SyainCdSeq = YYKASHO.EigTanCdSeq } equals new { SyainCdSeq = EIGYOTAN.SyainCdSeq } into EIGYOTAN_join
                          from EIGYOTAN in EIGYOTAN_join.DefaultIfEmpty()
                          where
                            YYKASHO.YoyaSyu == 1 &&
                            YYKASHO.YoyaKbnSeq == 1 &&
                            HAISHA.Kskbn != 1 &&
                            HAISHA.YouTblSeq == 0 &&
                            HAISHA.SiyoKbn == 1 &&
                            HAIIN.SiyoKbn == 1 &&
                            HAIIN.UkeNo == ukeno &&
                            HAIIN.TeiDanNo==teidanno &&
                            HAIIN.UnkRen==unkren&&
                            HAIIN.BunkRen==bunkRen&&
                            YYKASHO.SiyoKbn == 1 &&
                            HAISHA.NippoKbn != 2 &&
                            YYKASHO.SeiTaiYmd.CompareTo(_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd!=null?_dbContext.TkdLockTable.Where(t => t.TenantCdSeq == TenantCdSeq && t.EigyoCdSeq == YYKASHO.SeiEigCdSeq).First().LockYmd:"19100101") > 0
                          orderby
                            HAISHA.UkeNo,
                            HAIIN.HaiInRen
                          select new StaffsLines()
                          {
                              Haisha_UkeNo = HAISHA.UkeNo,
                              Haisha_UnkRen = HAISHA.UnkRen,
                              Haisha_SyaSyuRen = HAISHA.SyaSyuRen,
                              Haisha_TeiDanNo = HAISHA.TeiDanNo,
                              Haisha_BunkRen = HAISHA.BunkRen,
                              Haisha_GoSya = HAISHA.GoSya,
                              Haisha_KSKbn = HAISHA.Kskbn,
                              Haisha_HaiSKbn = HAISHA.HaiSkbn,
                              Haisha_HaiIKbn = HAISHA.HaiIkbn,
                              Haisha_KhinKbn = HAISHA.KhinKbn,
                              Haisha_NippoKbn = HAISHA.NippoKbn,
                              Haisha_DanTaNm2 = HAISHA.DanTaNm2,
                              Haisha_IkNm = HAISHA.IkNm,
                              Haisha_HaiSSryCdSeq = HAISHA.HaiSsryCdSeq,
                              Haisha_SyuEigCdSeq = HAISHA.SyuEigCdSeq,
                              Haisha_HaiSYmd = HAISHA.HaiSymd,
                              Haisha_HaiSTime = HAISHA.HaiStime,
                              Haisha_SyuKoYmd = HAISHA.SyuKoYmd,
                              Haisha_SyuKoTime = HAISHA.SyuKoTime,
                              Haisha_SyuPaTime = HAISHA.SyuPaTime,
                              Haisha_HaiSNm = HAISHA.HaiSnm,
                              Haisha_TouYmd = HAISHA.TouYmd,
                              Haisha_TouChTime = HAISHA.TouChTime,
                              Haisha_KikYmd = HAISHA.KikYmd,
                              Haisha_KikTime = HAISHA.KikTime,
                              Haisha_TouNm = HAISHA.TouNm,
                              Haisha_JyoSyaJin = HAISHA.JyoSyaJin,
                              Haisha_PlusJin = HAISHA.PlusJin,
                              Haisha_DrvJin = HAISHA.DrvJin,
                              Haisha_GuiSu = HAISHA.GuiSu,
                              Haisha_HaiSKouKNm = HAISHA.HaiSkouKnm,
                              Haisha_HaiSBinNm = HAISHA.HaiSbinNm,
                              Haisha_HaiSSetTime = HAISHA.HaiSsetTime,
                              Haisha_HaiSKigou = HAISHA.HaiSkigou,
                              Haisha_TouSKouKNm = HAISHA.TouSkouKnm,
                              Haisha_BunKSyuJyn=HAISHA.BunKsyuJyn,
                              Haisha_TouSBinNm = HAISHA.TouBinNm,
                              Haisha_TouSetTime = HAISHA.TouSetTime,
                              Haisha_TouKigou = HAISHA.TouKigou,
                              Haisha_OthJin1 = HAISHA.OthJin1,
                              Haisha_OthJin2 = HAISHA.OthJin2,
                              Haisha_PlatNo = HAISHA.PlatNo,
                              Haisha_HaiCom = HAISHA.HaiCom,
                              Haisha_SyaRyoUnc = HAISHA.SyaRyoUnc,
                              Haisha_HaiSJyus1 = HAISHA.HaiSjyus1,
                              Haisha_HaiSjyus2 = HAISHA.HaiSjyus2,
                              Haisha_TouJyusyo1 = HAISHA.TouJyusyo1,
                              Haisha_TouJyusyo2 = HAISHA.TouJyusyo2,
                              Haisha_KikEigSeq = HAISHA.KikEigSeq,
                              Haisha_YouTblSeq = HAISHA.YouTblSeq,
                              Yykasho_UkeCd = YYKASHO.UkeCd,
                              Yykasho_NippoKbn = YYKASHO.NippoKbn,
                              Yykasho_KaktYmd = YYKASHO.KaktYmd,
                              Yykasho_TokuiSeq = YYKASHO.TokuiSeq,
                              Yykasho_UkeEigCdSeq = YYKASHO.UkeEigCdSeq,
                              Tokisk_RyakuNm = TOKISK.RyakuNm,
                              Yykasho_SitenCdSeq = YYKASHO.SitenCdSeq,
                              Tokist_RyakuNm = TOKIST.RyakuNm,
                              Yykasho_InTanCdSeq = YYKASHO.InTanCdSeq,
                              Syain_SyainNm = SYAIN.SyainNm,
                              Yykasho_EigTanCdSeq = YYKASHO.EigTanCdSeq,
                              Eigyotan_SyainNm = EIGYOTAN.SyainNm,
                              Unkobi_DanTaNm = UNKOBI.DanTaNm,
                              Unkobi_HaiSYmd = UNKOBI.HaiSymd,
                              Unkobi_HaiSTime = UNKOBI.HaiStime,
                              Unkobi_TouYmd = UNKOBI.TouYmd,
                              Unkobi_TouChTime = UNKOBI.TouChTime,
                              Unkobi_ZenHaFlg = UNKOBI.ZenHaFlg,
                              Unkobi_KhakFlg = UNKOBI.KhakFlg,
                              Unkobi_SyukoYmd = UNKOBI.SyukoYmd,
                              Unkobi_KikYmd = UNKOBI.KikYmd,
                              Unkobi_UnkoJKbn = UNKOBI.UnkoJkbn,
                              Syaryo_SyaRyoNm = SYARYO.SyaRyoNm,
                              Syaryo_TeiCnt = SYARYO.TeiCnt,
                              Syasyu_SyaSyuNm = SYASYU.SyaSyuNm,
                              Syasyu_SyaSyuKigo = SYASYU.SyaSyuKigo,
                              YySyasyu_SyaSyuNm = YYSYASYU.SyaSyuNm,
                              Haiin_UkeNo = HAIIN.UkeNo,
                              Haiin_UnkRen = HAIIN.UnkRen,
                              Haiin_TeiDanNo = HAIIN.TeiDanNo,
                              Haiin_BunkRen = HAIIN.BunkRen,
                              Haiin_HaiInRen = HAIIN.HaiInRen,
                              Haiin_SyainCdSeq = HAIIN.SyainCdSeq,
                              Haiin_SyukinTime = HAIIN.SyukinTime,
                              Haiin_Syukinbasy = HAIIN.Syukinbasy,
                              Haiin_TaiknTime = HAIIN.TaiknTime,
                              Haiin_TaiknBasy = HAIIN.TaiknBasy,
                              Kyoshe_SyokumuCdSeq = KYOSHE.SyokumuCdSeq,
                              Syokum_SyokumuKbn = SYOKUM.SyokumuKbn,
                              Syokum_SyokumuNm = SYOKUM.SyokumuNm,
                              Eigyoshos_RyakuNm = EIGYOSHOS.RyakuNm,
                              Eigyoshos1_RyakuNm = EIGYOSHOS1.RyakuNm,
                              Haisha_BikoNm = HAISHA.BikoNm,
                              Yyksho_BikoNm = YYKASHO.BikoNm,
                              Unkobi_BikoNm = UNKOBI.BikoNm,
                              Yyksyu_KataKbn = (from TKD_YykSyu in _dbContext.TkdYykSyu
                                                where
                                        TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                                        TKD_YykSyu.SiyoKbn == 1
                                                select
                                                    TKD_YykSyu.KataKbn).First(),
                              Yyksyu_SyaSyuDai =
                              (from TKD_YykSyu in _dbContext.TkdYykSyu
                               where
                       TKD_YykSyu.UkeNo == HAISHA.UkeNo &&
                       TKD_YykSyu.SiyoKbn == 1
                               select new
                               {
                                   TKD_YykSyu.SyaSyuDai
                               }).Sum(p => p.SyaSyuDai),
                              UKEJOKEN_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                    where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.UkeJyKbnCd) && UKEJOKEN.CodeSyu == codeSyuUKEJYKBNCD && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantUKEJYKBNCD
                                                    select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN1_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn1) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB1 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB1
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN2_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn2) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB2 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB2
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN3_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn3) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB3 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB3
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN4_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn4) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB4 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB4
                                                     select UKEJOKEN.CodeKbnNm).First(),
                              SIJJOKBN5_CodeKbnNm = (from UKEJOKEN in _dbContext.VpmCodeKb
                                                     where UKEJOKEN.CodeKbn == Convert.ToString(HAISHA.SijJoKbn5) && UKEJOKEN.CodeSyu == codeSyuSIJJOKB5 && UKEJOKEN.SiyoKbn == 1 && UKEJOKEN.TenantCdSeq == tenantSIJJOKB5
                                                     select UKEJOKEN.CodeKbnNm).First(),

                          }).ToListAsync();
        }
    }
}
