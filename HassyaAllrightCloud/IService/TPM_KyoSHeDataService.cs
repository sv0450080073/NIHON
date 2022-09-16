using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_KyoSHeDataListService
    {
        Task<List<TPM_KyoSHeData>> Getdata(DateTime Busstartdate, int[] EigyoCdSeqslt, int tenantCdSeq);
        Task<List<HaishadrgdData>> Getdatadriandgui(DateTime Busstartdate, int[] EigyoCdSeqslt, int tenantCdSeq);
        Task<List<TKD_KikyujData>> Getdataoff(DateTime Busstartdate, int[] SyainCdSeq);
        Task<List<TKD_KobanData>> GetdataKoban(DateTime Busstartdate, int[] SyainCdSeq);

    }
    public class TPM_KyoSHeDataService : ITPM_KyoSHeDataListService
    {
        private readonly KobodbContext _context;

        public TPM_KyoSHeDataService(KobodbContext context)
        {
            _context = context;
        }
        public async Task<List<HaishadrgdData>> Getdatadriandgui(DateTime Busstartdate, int[] EigyoCdSeqslt, int tenantCdSeq)
        {
            string date = Busstartdate.ToString("yyyyMMdd");
            return await (from HAISHA in _context.TkdHaisha
                          join YYKSHO in _context.TkdYyksho on HAISHA.UkeNo equals YYKSHO.UkeNo into YYKSHO_join
                          from YYKSHO in YYKSHO_join.DefaultIfEmpty()
                          join YYKSYU in _context.TkdYykSyu
                                on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen }
                            equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen } into YYKSYU_join
                          from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                          join Eigyos in _context.VpmEigyos
                                on HAISHA.SyuEigCdSeq equals Eigyos.EigyoCdSeq into Eigyos_join
                          from Eigyos in Eigyos_join.DefaultIfEmpty()
                          where
                            HAISHA.SiyoKbn == 1 &&
                            HAISHA.YouTblSeq == 0 &&
                             string.Compare(HAISHA.SyuKoYmd, date) <= 0 &&
                            string.Compare(HAISHA.KikYmd, date) >= 0 &&
                            (EigyoCdSeqslt.Contains(HAISHA.SyuEigCdSeq)||HAISHA.SyuEigCdSeq == 0) &&
                            YYKSHO.TenantCdSeq == tenantCdSeq &&
                            YYKSHO.YoyaSyu == 1 &&
                            YYKSHO.SiyoKbn == 1 &&
                            YYKSYU.SiyoKbn ==1
                          group new { HAISHA, YYKSYU } by new
                          {
                              HAISHA.SyuEigCdSeq,
                              HAISHA.Kskbn,
                              Eigyos.CompanyCdSeq,
                          } into g
                          select new HaishadrgdData()
                          {
                              date=Busstartdate,
                              allDrivers = g.Sum(p => p.HAISHA.DrvJin),
                              allGuides = g.Sum(p => p.HAISHA.GuiSu),
                              largeDriver = g.Sum(p => (
                              p.YYKSYU.KataKbn == 1 ? (int)p.HAISHA.DrvJin : 0)),
                              MediumDriver = g.Sum(p => (
                              p.YYKSYU.KataKbn == 2 ? (int)p.HAISHA.DrvJin : 0)),
                              SmallDriver = g.Sum(p => (
                              p.YYKSYU.KataKbn == 3 ? (int)p.HAISHA.DrvJin : 0)),
                              EigyoCdSeq= g.Key.SyuEigCdSeq,
                              KSKbn=g.Key.Kskbn,
                              CompanyCdSeq=g.Key.CompanyCdSeq
                          }).ToListAsync();

        }
        /// <summary>
        /// get data date off staff
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="SyainCdSeqslt"></param>
        /// <returns></returns>
        public async Task<List<TKD_KikyujData>> Getdataoff(DateTime startdate, int[] SyainCdSeqslt)
        {
            string date = startdate.ToString("yyyyMMdd");
            return await (from Kikyuj in _context.TkdKikyuj
                          where SyainCdSeqslt.Contains(Kikyuj.SyainCdSeq) &&
                          string.Compare(Kikyuj.KinKyuSymd, date) <= 0 &&
                          string.Compare(Kikyuj.KinKyuEymd, date) >= 0 &&
                          Kikyuj.SiyoKbn == 1
                          orderby Kikyuj.SyainCdSeq, Kikyuj.KinKyuTblCdSeq
                          select new TKD_KikyujData
                          {
                              KinKyuTblCdSeq = Kikyuj.KinKyuTblCdSeq,
                              SyainCdSeq = Kikyuj.SyainCdSeq,
                              KinKyuSYmd = Kikyuj.KinKyuSymd,
                              KinKyuSTime = Kikyuj.KinKyuStime,
                              KinKyuEYmd = Kikyuj.KinKyuEymd,
                              KinKyuETime = Kikyuj.KinKyuEtime,
                              KinKyuCdSeq = Kikyuj.KinKyuCdSeq,
                              BikoNm = Kikyuj.BikoNm,
                          }).ToListAsync();


        }

        /// <summary>
        ///  get data bus staff
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="EigyoCdSeqslt"></param>
        /// <returns></returns>
        public async Task<List<TPM_KyoSHeData>> Getdata(DateTime startdate, int[] EigyoCdSeqslt, int tenantCdSeq)
        {
            string date = startdate.ToString("yyyyMMdd");
            return await (from KyoSHe in _context.VpmKyoShe
                          join Syokum in _context.VpmSyokum
                                on new { KyoSHe.SyokumuCdSeq, TenantCdSeq = tenantCdSeq }
                                equals new { Syokum.SyokumuCdSeq, Syokum.TenantCdSeq } into Syokum_join
                          from Syokum in Syokum_join.DefaultIfEmpty()
                          join Eigyos in _context.VpmEigyos 
                                on KyoSHe.EigyoCdSeq equals Eigyos.EigyoCdSeq into Eigyos_join
                          from Eigyos in Eigyos_join.DefaultIfEmpty()
                          where (new int[] { 1, 2, 3, 4 }).Contains(Syokum.SyokumuKbn) &&
                                EigyoCdSeqslt.Contains(KyoSHe.EigyoCdSeq) &&
                                string.Compare(KyoSHe.StaYmd, date) <= 0 &&
                                string.Compare(KyoSHe.EndYmd, date) >= 0
                          orderby KyoSHe.SyainCdSeq, KyoSHe.SyokumuCdSeq
                          select new TPM_KyoSHeData
                          {
                              SyainCdSeq = KyoSHe.SyainCdSeq,
                              SyokumuCdSeq = KyoSHe.SyokumuCdSeq,
                              EigyoCdSeq = KyoSHe.EigyoCdSeq,
                              TenkoNo = KyoSHe.TenkoNo,
                              StaYmd = KyoSHe.StaYmd,
                              EndYmd = KyoSHe.EndYmd,
                              SyokumuKbn=Syokum.SyokumuKbn,
                              CompanyCdSeq = Eigyos.CompanyCdSeq,
                              BigTypeDrivingFlg = KyoSHe.BigTypeDrivingFlg,
                              MediumTypeDrivingFlg = KyoSHe.MediumTypeDrivingFlg,
                              SmallTypeDrivingFlg = KyoSHe.SmallTypeDrivingFlg
                          }).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="SyainCdSeqslt"></param>
        /// <returns></returns>
        public async Task<List<TKD_KobanData>> GetdataKoban(DateTime startdate, int[] SyainCdSeqslt)
        {
            string date = startdate.ToString("yyyyMMdd");
            return await (from Koban in _context.TkdKoban
                          join Kikyuj in _context.TkdKikyuj
                                on new { Koban.KinKyuTblCdSeq, Koban.SyainCdSeq, SiyoKbn = 1 }
                                equals new { Kikyuj.KinKyuTblCdSeq, Kikyuj.SyainCdSeq, SiyoKbn = Convert.ToInt32(Kikyuj.SiyoKbn) } into Kikyuj_join
                          from Kikyuj in Kikyuj_join.DefaultIfEmpty()
                          join KinKyu in _context.VpmKinKyu
                                on Kikyuj.KinKyuCdSeq equals KinKyu.KinKyuCdSeq into KinKyu_join
                          from KinKyu in KinKyu_join.DefaultIfEmpty()
                          join KyoShe in _context.VpmKyoShe
                            on new { Koban.SyainCdSeq } equals new { KyoShe.SyainCdSeq } into KyoShe_join
                          from KyoShe in KyoShe_join.DefaultIfEmpty()
                          where string.Compare(KyoShe.StaYmd, Koban.UnkYmd) <= 0 &&
                                string.Compare(KyoShe.EndYmd, Koban.UnkYmd) >= 0 &&
                                string.Compare(Koban.UnkYmd, date) <= 0 &&
                                string.Compare(Koban.UnkYmd, date) >= 0 &&
                                Koban.KinKyuTblCdSeq!=0 &&
                                KinKyu.TenantCdSeq == new ClaimModel().TenantID &&
                                SyainCdSeqslt.Contains(Koban.SyainCdSeq)
                          select new TKD_KobanData
                          {
                              UnkYmd = Koban.UnkYmd,
                              SyainCdSeq = Koban.SyainCdSeq,
                              KouBnRen = Koban.KouBnRen,
                              KinKyuTblCdSeq = Kikyuj.KinKyuTblCdSeq,
                              KinKyuKbn = KinKyu.KinKyuKbn,
                              KyusyutsuKbn = KinKyu.KyusyutsuKbn,
                              BigTypeDrivingFlg = KyoShe.BigTypeDrivingFlg,
                              MediumTypeDrivingFlg = KyoShe.MediumTypeDrivingFlg,
                              SmallTypeDrivingFlg = KyoShe.SmallTypeDrivingFlg
                          }).ToListAsync();


        }
    }
}
