using System.Collections.Generic;
using System;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;
using System.Globalization;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_KikyujDataListService
    {
        Task<List<StaffDayOff>> Getdata(DateTime staffstardate, DateTime staffenddate, int tenantCdSeq,List<int> lstStaffid);
        void UpdateStaffdata(ItemStaff updatedStaff, int userlogin);
        void UpdateStaffLinedata(int KinKyuTblCdSeq, int SyainCdSeq, int userlogin);
        void DeleteDayoff(int KinKyuTblCdSeq, int userlogin);
        void UpdateDayoffData(ItemStaff updatedStaff, int userlogin);

    }
    public class TKD_KikyujDataService : ITKD_KikyujDataListService
    {
        private readonly KobodbContext _dbContext;
        private readonly BusScheduleHelper _busScheduleHelper;
        private readonly ITKD_KobanDataService _KobanDataService;

        public TKD_KikyujDataService(KobodbContext context, BusScheduleHelper BusScheduleHelper,ITKD_KobanDataService TKD_KobanDataService)
        {
            _dbContext = context;
            _busScheduleHelper = BusScheduleHelper;
            _KobanDataService = TKD_KobanDataService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kinKyuTblCdSeq"></param>
        /// <param name="userid"></param>
        public void DeleteDayoff(int kinKyuTblCdSeq, int userid)
        {
            var updateKikyuj = _dbContext.TkdKikyuj.Find(kinKyuTblCdSeq);
            updateKikyuj.SiyoKbn = 2;
            updateKikyuj.HenKai += 1;
            updateKikyuj.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            updateKikyuj.UpdTime = DateTime.Now.ToString("hhmmss");
            updateKikyuj.UpdSyainCd = userid;
            updateKikyuj.UpdPrgId = Common.UpdPrgId;
            _KobanDataService.DeleteKoban(kinKyuTblCdSeq);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="busstardate"></param>
        /// <param name="busenddate"></param>
        /// <param name="tenantCdSeq"></param>
        /// <returns></returns>
        public async Task<List<StaffDayOff>> Getdata(DateTime busstardate, DateTime busenddate, int tenantCdSeq, List<int> lstStaffid)
        {
            string DateStarAsString = busstardate.ToString("yyyyMMdd");
            string DateEndAsString = busenddate.ToString("yyyyMMdd");
            return await (from KIKYUJ in _dbContext.TkdKikyuj
                          join KYOSHE in _dbContext.VpmKyoShe on KIKYUJ.SyainCdSeq equals KYOSHE.SyainCdSeq into KYOSHE_join
                          from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                          join KINKYU in _dbContext.VpmKinKyu on KIKYUJ.KinKyuCdSeq equals KINKYU.KinKyuCdSeq into KINKYU_join
                          from KINKYU in KINKYU_join.DefaultIfEmpty()
                          join EIGYOS in _dbContext.VpmEigyos on KYOSHE.EigyoCdSeq equals EIGYOS.EigyoCdSeq into EIGYOS_join
                          from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                          join KAISHA in _dbContext.VpmCompny
                                on new
                                {
                                    EIGYOS.CompanyCdSeq,
                                    TenantCdSeq = new ClaimModel().TenantID,
                                    SiyoKbn = 1
                                }
                            equals new
                            {
                                KAISHA.CompanyCdSeq,
                                KAISHA.TenantCdSeq,
                                SiyoKbn = Convert.ToInt32(KAISHA.SiyoKbn)
                            }
                          where
                            lstStaffid.Contains(KIKYUJ.SyainCdSeq) &&
                           string.Compare(KIKYUJ.KinKyuEymd, DateStarAsString) >= 0 &&
                            string.Compare(KIKYUJ.KinKyuSymd, DateEndAsString) <= 0 &&
                            string.Compare(KYOSHE.EndYmd, DateStarAsString) >= 0 &&
                            string.Compare(KYOSHE.StaYmd, DateEndAsString) <= 0 &&
                            KIKYUJ.SiyoKbn == 1 &&
                            KINKYU.TenantCdSeq == new ClaimModel().TenantID
                          orderby
                            KIKYUJ.SyainCdSeq
                          select new StaffDayOff()
                          {
                              Kikyuj_KinKyuTblCdSeq = KIKYUJ.KinKyuTblCdSeq,
                              Kikyuj_SyainCdSeq = KIKYUJ.SyainCdSeq,
                              Kikyuj_KinKyuSYmd = KIKYUJ.KinKyuSymd,
                              Kikyuj_KinKyuSTime = KIKYUJ.KinKyuStime,
                              Kikyuj_KinKyuEYmd = KIKYUJ.KinKyuEymd,
                              Kikyuj_KinKyuETime = KIKYUJ.KinKyuEtime,
                              Kikyuj_KinKyuCdSeq = KIKYUJ.KinKyuCdSeq,
                              KinKyu_KinKyuCdSeq = KINKYU.KinKyuCdSeq,
                              KinKyu_KinKyuNm = KINKYU.KinKyuNm,
                              KinKyu_KinKyuKbn = KINKYU.KinKyuKbn,
                              KinKyu_ColKinKyu = KINKYU.ColKinKyu,
                              Eigyos_EigyoCdSeq = EIGYOS.EigyoCdSeq,
                              Compny_CompanyCdSeq = EIGYOS.CompanyCdSeq,
                              FuriYmd = _dbContext.TkdKoban.Where(t => t.KinKyuTblCdSeq == KIKYUJ.KinKyuTblCdSeq && t.SyainCdSeq == KIKYUJ.SyainCdSeq).FirstOrDefault().FuriYmd
                          }).ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedStaff"></param>
        /// <param name="userlogin"></param>
        public void UpdateStaffdata(ItemStaff updatedStaff, int userlogin)
        {
            if(updatedStaff.BookingId == "-1")
            {
                 TkdKikyuj updateKikyuj = new TkdKikyuj();
                updateKikyuj.HenKai = 0;
                updateKikyuj.SyainCdSeq = int.Parse(updatedStaff.BusLine);
                updateKikyuj.KinKyuSymd = updatedStaff.StartDate;
                updateKikyuj.KinKyuStime = updatedStaff.TimeStart.ToString("D4");
                updateKikyuj.KinKyuEymd = updatedStaff.EndDate;
                updateKikyuj.KinKyuEtime=updatedStaff.TimeEnd.ToString("D4");
                updateKikyuj.KinKyuCdSeq = updatedStaff.KinKyuCdSeq;
                updateKikyuj.BikoNm = updatedStaff.BikoNm;
                updateKikyuj.SiyoKbn = 1;
                updateKikyuj.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                updateKikyuj.UpdTime = DateTime.Now.ToString("hhmmss");
                updateKikyuj.UpdSyainCd = userlogin;
                updateKikyuj.UpdPrgId = Common.UpdPrgId;
                updateKikyuj.UpdPrgId = "KU1300";
                _dbContext.TkdKikyuj.Add(updateKikyuj);
                _dbContext.SaveChanges();
                _KobanDataService.UpdateDayoffData(updatedStaff.StartDate, updatedStaff.TimeStart.ToString("D4"), updatedStaff.EndDate, updatedStaff.TimeEnd.ToString("D4"), int.Parse(updatedStaff.BusLine),updateKikyuj.KinKyuTblCdSeq, updatedStaff.KinKyuKbn,updatedStaff.TransferDate, userlogin);
            }    
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KinKyuTblCdSeq"></param>
        /// <param name="SyainCdSeq"></param>
        /// <param name="userlogin"></param>
        public void UpdateStaffLinedata(int KinKyuTblCdSeq, int SyainCdSeq, int userlogin)
        {
            var std = _dbContext.TkdKikyuj.Find(KinKyuTblCdSeq);
            int SyainCdSeqold = std.SyainCdSeq;
            std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            std.UpdTime = DateTime.Now.ToString("HHmmss");
            std.UpdPrgId = Common.UpdPrgId;
            std.UpdSyainCd = userlogin;
            std.HenKai = (short)(std.HenKai + 1);
            std.SyainCdSeq = SyainCdSeq;
            _dbContext.Update(std);
            _dbContext.SaveChanges();
             _KobanDataService.UpdateDayoffLineData( SyainCdSeqold,SyainCdSeq,KinKyuTblCdSeq, userlogin);
        }
       public void UpdateDayoffData(ItemStaff updatedStaff, int userlogin)
        {
            var updateKikyuj = _dbContext.TkdKikyuj.Find(updatedStaff.KinKyuTblCdSeq);
            updateKikyuj.HenKai = (short)(updateKikyuj.HenKai+1);
            updateKikyuj.SyainCdSeq = int.Parse(updatedStaff.BusLine);
            updateKikyuj.KinKyuSymd = updatedStaff.StartDate;
            updateKikyuj.KinKyuStime = updatedStaff.TimeStart.ToString("D4");
            updateKikyuj.KinKyuEymd = updatedStaff.EndDate;
            updateKikyuj.KinKyuEtime = updatedStaff.TimeEnd.ToString("D4");
            updateKikyuj.KinKyuCdSeq = updatedStaff.KinKyuCdSeq;
            updateKikyuj.BikoNm = updatedStaff.BikoNm==null?"":updatedStaff.BikoNm;
            updateKikyuj.SiyoKbn = 1;
            updateKikyuj.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            updateKikyuj.UpdTime = DateTime.Now.ToString("hhmmss");
            updateKikyuj.UpdSyainCd = userlogin;
            updateKikyuj.UpdPrgId = Common.UpdPrgId;
            updateKikyuj.UpdPrgId = "KU1300";
            _dbContext.TkdKikyuj.Update(updateKikyuj);
            _dbContext.SaveChanges();
            _KobanDataService.DeleteKoban(updatedStaff.KinKyuTblCdSeq);
            _KobanDataService.UpdateDayoffData(updatedStaff.StartDate, updatedStaff.TimeStart.ToString("D4"), updatedStaff.EndDate, updatedStaff.TimeEnd.ToString("D4"), int.Parse(updatedStaff.BusLine),updateKikyuj.KinKyuTblCdSeq, updatedStaff.KinKyuKbn,updatedStaff.TransferDate, userlogin);
        }
    }
}
