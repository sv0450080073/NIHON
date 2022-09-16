using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Domain.Dto;
using System.Linq;
using HassyaAllrightCloud.Commons.Helpers;
using DevExpress.CodeParser;
using DevExpress.XtraRichEdit.Commands;
using System.Globalization;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Commons.Constants;
using BlazorContextMenu;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_KobanDataService
    {
        void UpdateDayoffData(string startDate, string startTime, string endDate, string endTime, int SyainCdSeq,int KinKyuTblCdSeq, int KinKyuKbn,string furiYdm, int userlogin);
        void UpdateDayoffLineData(int SyainCdSeqold,int SyainCdSeq,int KinKyuTblCdSeq,  int userlogin);
        void UpdateLineData(int SyainCdSeqold,int SyainCdSeq,string ukeNo, short unkRen, short teiDanNo, short bunkRen,  int userlogin, ItemStaff updatedStaff);
        void DeleteKobanbyUkeno(string ukeNo,short unkRen, short teiDanNo,short bunkRen,int syainCdSeq);
        Task DeleteKobanbyHaisha(string ukeNo,short unkRen, short teiDanNo,short bunkRen);
        void UpdateTimeKoban(string startDate, string startTime, string endDate, string endTime, int SyainCdSeq, int CompanyCdSeq,ItemStaff itemStaff, int userlogin, string formNm);
        void UpdateLineKoban();
        void DeleteKoban(int kinKyuTblCdSeq);
        void UpdateKouBnRenbyDate(string fromdate, string todate);

    }
    public class TKD_KobanDataService : ITKD_KobanDataService
    {
        private readonly KobodbContext _dbContext;
        private readonly BusScheduleHelper _helper;
         private readonly IVPM_SyuTaikinCalculationTimeService _calculationTime;
        public TKD_KobanDataService(KobodbContext context, BusScheduleHelper helper ,IVPM_SyuTaikinCalculationTimeService vpm_SyuTaikinCalculationTimeService)
        {
            _dbContext = context;
            _helper = helper;
            _calculationTime = vpm_SyuTaikinCalculationTimeService;

        }
        public void UpdateKouBnRenbyDate(string fromdate, string todate)
        {
            DateTime startdatene;
            DateTime.TryParseExact(fromdate,
                           "yyyyMMdd",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out startdatene);

            DateTime enddatene;
            DateTime.TryParseExact(todate,
                           "yyyyMMdd",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out enddatene);
            for(var day = startdatene.Date; day.Date <= enddatene.Date; day = day.AddDays(1))
            {
                var checkKoban = _dbContext.TkdKoban.OrderBy(t => t.SyukinYmd).ThenBy(t => t.SyukinTime).Where(t=>t.UnkYmd==day.ToString("yyyyMMdd"))
           .ToList() 
           .GroupBy(job => job.SyainCdSeq)
            .Select(g => new { g, count = g.Count() })
                  .SelectMany(t => t.g.Select(b => b)
                                      .Zip(Enumerable.Range(1, t.count), (j, i) => new { Item = j, rn = i })).ToList();
                foreach (var item in checkKoban)
                {
                    var removeKoban = _dbContext.TkdKoban.Find(item.Item.UnkYmd, item.Item.SyainCdSeq, item.Item.KouBnRen);
                    _dbContext.TkdKoban.Remove(removeKoban);
                }
                _dbContext.SaveChanges();
                foreach (var item in checkKoban)
                {
                    TkdKoban inserttkdKoban = new TkdKoban();
                    inserttkdKoban = item.Item;
                    inserttkdKoban.KouBnRen = (short)item.rn;
                    _dbContext.TkdKoban.Add(inserttkdKoban);
                    _dbContext.SaveChanges();
                }
            }  
        }
        public async void UpdateTimeKoban(string startDate, string startTime, string endDate, string endTime, int SyainCdSeq, int CompanyCdSeq, ItemStaff itemStaff, int userlogin, string formNm)
        {
            var CalculationTimedata = _calculationTime.GetSyuTaikinCalculationTime();
            int syukinCalculationTimeMinute = 0;
            int taikinCalculationTimeMinutes = 0;
            DateTime startdatene;
            DateTime.TryParseExact(startDate,
                           "yyyyMMdd",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out startdatene);

            DateTime enddatene;
            DateTime.TryParseExact(endDate,
                           "yyyyMMdd",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out enddatene);
            if (endTime == "0000")
            {
                enddatene = enddatene.AddMinutes(-1);
            }
            double totaldate = (enddatene - startdatene).TotalDays;
            if (totaldate < 1)
            {
                var checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                int countDayoff = 1;
                if (checkcount.Count > 0)
                {
                    countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                }
                TkdKoban newitem = new TkdKoban();
                newitem.UnkYmd = startDate;
                newitem.SyainCdSeq = SyainCdSeq;
                newitem.KouBnRen = (short)countDayoff;
                newitem.HenKai = 0;
                newitem.SyugyoKbn = 1;
                newitem.KinKyuTblCdSeq = 0;
                newitem.UkeNo = itemStaff.BookingId;
                newitem.UnkRen = itemStaff.HaUnkRen;
                newitem.SyaSyuRen = (short)itemStaff.SyaSyuRen;
                newitem.TeiDanNo = itemStaff.TeiDanNo;
                newitem.BunkRen = itemStaff.BunkRen;
                newitem.RotCdSeq = 0;
                newitem.RenEigCd = 0;
                newitem.SigySyu = 0;
                newitem.KitYmd = "";
                newitem.SigyKbn = 0;
                newitem.SiyoKbn = 1;
                newitem.SigyCd = "";
                newitem.KouZokPtnKbn = setKouZokPtnKbn(newitem.UnkYmd, itemStaff);
                if (CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null && CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null)
                {
                    syukinCalculationTimeMinute = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().SyukinCalculationTimeMinutes;
                    taikinCalculationTimeMinutes = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().TaikinCalculationTimeMinutes;
                }
                else
                {
                    syukinCalculationTimeMinute = 0;
                    taikinCalculationTimeMinutes = 0;
                }

                DateTime SyukinDate;
                DateTime.TryParseExact(startDate + startTime,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out SyukinDate);
                DateTime TaikinDate;
                DateTime.TryParseExact(endDate + endTime,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out TaikinDate);

                newitem.SyukinYmd = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("yyyyMMdd");
                newitem.TaikinYmd = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("yyyyMMdd");
                newitem.SyukinTime = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("HHmm");
                newitem.TaiknTime = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("HHmm");
                newitem.FuriYmd = "";
                newitem.RouTime = "";
                newitem.KouStime = "";
                newitem.TaikTime = "";
                newitem.KyuKtime = "";
                newitem.JitdTime = "";
                newitem.ZangTime = "";
                newitem.UsinyTime = "";
                newitem.Syukinbasy = "";
                newitem.SsinTime = "";
                newitem.BikoNm = "";
                newitem.TaiknBasy = "";
                newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                newitem.UpdTime = DateTime.Now.ToString("hhmmss");
                newitem.UpdSyainCd = userlogin;
                newitem.UpdPrgId = formNm;
                _dbContext.TkdKoban.Add(newitem);
                _dbContext.SaveChanges();
            }
            else
            {
                if (totaldate < 2)
                {
                    var checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    int countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    TkdKoban newitem = new TkdKoban();
                    newitem.UnkYmd = startDate;
                    newitem.SyainCdSeq = SyainCdSeq;
                    newitem.KouBnRen = (short)countDayoff;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = 0;
                    newitem.UkeNo = itemStaff.BookingId;
                    newitem.UnkRen = itemStaff.HaUnkRen;
                    newitem.SyaSyuRen = (short)itemStaff.SyaSyuRen;
                    newitem.TeiDanNo = itemStaff.TeiDanNo;
                    newitem.BunkRen = itemStaff.BunkRen;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    newitem.KouZokPtnKbn = setKouZokPtnKbn(newitem.UnkYmd, itemStaff);
                    if (CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null && CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null)
                    {
                        syukinCalculationTimeMinute = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().SyukinCalculationTimeMinutes;
                        taikinCalculationTimeMinutes = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().TaikinCalculationTimeMinutes;
                    }
                    else
                    {
                        syukinCalculationTimeMinute = 0;
                        taikinCalculationTimeMinutes = 0;
                    }
                    DateTime SyukinDate;
                    DateTime.TryParseExact(startDate + startTime,
                                               "yyyyMMddHHmm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out SyukinDate);
                    DateTime TaikinDate;
                    DateTime.TryParseExact(startDate + "2359",
                                               "yyyyMMddHHmm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out TaikinDate);

                    newitem.SyukinYmd = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("yyyyMMdd");
                    newitem.TaikinYmd = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("yyyyMMdd");
                    newitem.SyukinTime = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("HHmm");
                    newitem.TaiknTime = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("HHmm");
                    newitem.FuriYmd = "";
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmm");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = formNm;
                    _dbContext.TkdKoban.Add(newitem);
                    _dbContext.SaveChanges();

                    checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(endDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(endDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    newitem = new TkdKoban();
                    newitem.UnkYmd = enddatene.ToString("yyyyMMdd");
                    newitem.SyainCdSeq = SyainCdSeq;
                    newitem.KouBnRen = (short)countDayoff;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = 0;
                    newitem.UkeNo = itemStaff.BookingId;
                    newitem.UnkRen = itemStaff.HaUnkRen;
                    newitem.SyaSyuRen = (short)itemStaff.SyaSyuRen;
                    newitem.TeiDanNo = itemStaff.TeiDanNo;
                    newitem.BunkRen = itemStaff.BunkRen;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    if (CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null && CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null)
                    {
                        syukinCalculationTimeMinute = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().SyukinCalculationTimeMinutes;
                        taikinCalculationTimeMinutes = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().TaikinCalculationTimeMinutes;
                    }
                    else
                    {
                        syukinCalculationTimeMinute = 0;
                        taikinCalculationTimeMinutes = 0;
                    }
                    DateTime.TryParseExact(enddatene.ToString("yyyyMMdd") + "0000",
                                               "yyyyMMddHHmm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out SyukinDate);
                    DateTime.TryParseExact(endDate + endTime,
                                               "yyyyMMddHHmm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out TaikinDate);
                    newitem.SyukinYmd = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("yyyyMMdd");
                    newitem.TaikinYmd = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("yyyyMMdd");
                    newitem.SyukinTime = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("HHmm");
                    newitem.TaiknTime = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("HHmm");
                    newitem.FuriYmd = "";
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmm");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = formNm;
                    _dbContext.TkdKoban.Add(newitem);
                    _dbContext.SaveChanges();
                }
                else
                {
                    var checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    int countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    TkdKoban newitem = new TkdKoban();
                    newitem.UnkYmd = startDate;
                    newitem.SyainCdSeq = SyainCdSeq;
                    newitem.KouBnRen = (short)countDayoff;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = 0;
                    newitem.UkeNo = itemStaff.BookingId;
                    newitem.UnkRen = itemStaff.HaUnkRen;
                    newitem.SyaSyuRen = (short)itemStaff.SyaSyuRen;
                    newitem.TeiDanNo = itemStaff.TeiDanNo;
                    newitem.BunkRen = itemStaff.BunkRen;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    newitem.KouZokPtnKbn = setKouZokPtnKbn(newitem.UnkYmd, itemStaff);
                    if (CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null && CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null)
                    {
                        syukinCalculationTimeMinute = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().SyukinCalculationTimeMinutes;
                        taikinCalculationTimeMinutes = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().TaikinCalculationTimeMinutes;
                    }
                    else
                    {
                        syukinCalculationTimeMinute = 0;
                        taikinCalculationTimeMinutes = 0;
                    }
                    DateTime SyukinDate;
                    DateTime.TryParseExact(startDate + startTime,
                                               "yyyyMMddHHmm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out SyukinDate);
                    DateTime TaikinDate;
                    DateTime.TryParseExact(startDate + "2359",
                                               "yyyyMMddHHmm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out TaikinDate);

                    newitem.SyukinYmd = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("yyyyMMdd");
                    newitem.TaikinYmd = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("yyyyMMdd");
                    newitem.SyukinTime = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("HHmm");
                    newitem.TaiknTime = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("HHmm");
                    newitem.FuriYmd = "";
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmm");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = formNm;
                    _dbContext.TkdKoban.Add(newitem);
                    _dbContext.SaveChanges();

                    for (int i = 1; i < totaldate; i++)
                    {
                        checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startdatene.AddDays(i).ToString("yyyyMMdd")) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                        countDayoff = 1;
                        if (checkcount.Count > 0)
                        {
                            countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startdatene.AddDays(i).ToString("yyyyMMdd")) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                        }
                        newitem = new TkdKoban();
                        newitem.UnkYmd = startdatene.AddDays(i).ToString("yyyyMMdd");
                        newitem.SyainCdSeq = SyainCdSeq;
                        newitem.KouBnRen = (short)countDayoff;
                        newitem.HenKai = 0;
                        newitem.SyugyoKbn = 1;
                        newitem.KinKyuTblCdSeq = 0;
                        newitem.UkeNo = itemStaff.BookingId;
                        newitem.UnkRen = itemStaff.HaUnkRen;
                        newitem.SyaSyuRen = (short)itemStaff.SyaSyuRen;
                        newitem.TeiDanNo = itemStaff.TeiDanNo;
                        newitem.BunkRen = itemStaff.BunkRen;
                        newitem.RotCdSeq = 0;
                        newitem.RenEigCd = 0;
                        newitem.SigySyu = 0;
                        newitem.KitYmd = "";
                        newitem.SigyKbn = 0;
                        newitem.SiyoKbn = 1;
                        newitem.SigyCd = "";
                        newitem.KouZokPtnKbn = setKouZokPtnKbn(newitem.UnkYmd, itemStaff); ;
                        if (CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null && CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null)
                        {
                            syukinCalculationTimeMinute = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().SyukinCalculationTimeMinutes;
                            taikinCalculationTimeMinutes = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().TaikinCalculationTimeMinutes;
                        }
                        else
                        {
                            syukinCalculationTimeMinute = 0;
                            taikinCalculationTimeMinutes = 0;
                        }
                        DateTime.TryParseExact(startdatene.AddDays(i).ToString("yyyyMMdd") + "0000",
                                                   "yyyyMMddHHmm",
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None,
                                                   out SyukinDate);
                        DateTime.TryParseExact(startdatene.AddDays(i).ToString("yyyyMMdd") + "2359",
                                                    "yyyyMMddHHmm",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out TaikinDate);

                        newitem.SyukinYmd = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("yyyyMMdd");
                        newitem.TaikinYmd = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("yyyyMMdd");
                        newitem.SyukinTime = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("HHmm");
                        newitem.TaiknTime = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("HHmm");
                        newitem.FuriYmd = "";
                        newitem.RouTime = "";
                        newitem.KouStime = "";
                        newitem.TaikTime = "";
                        newitem.KyuKtime = "";
                        newitem.JitdTime = "";
                        newitem.ZangTime = "";
                        newitem.UsinyTime = "";
                        newitem.Syukinbasy = "";
                        newitem.SsinTime = "";
                        newitem.BikoNm = "";
                        newitem.TaiknBasy = "";
                        newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        newitem.UpdTime = DateTime.Now.ToString("hhmm");
                        newitem.UpdSyainCd = userlogin;
                        newitem.UpdPrgId = formNm;
                        _dbContext.TkdKoban.Add(newitem);
                        _dbContext.SaveChanges();
                    }
                    checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(endDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(endDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    newitem = new TkdKoban();
                    newitem.UnkYmd = enddatene.ToString("yyyyMMdd");
                    newitem.SyainCdSeq = SyainCdSeq;
                    newitem.KouBnRen = (short)countDayoff;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = 0;
                    newitem.UkeNo = itemStaff.BookingId;
                    newitem.UnkRen = itemStaff.HaUnkRen;
                    newitem.SyaSyuRen = (short)itemStaff.SyaSyuRen;
                    newitem.TeiDanNo = itemStaff.TeiDanNo;
                    newitem.BunkRen = itemStaff.BunkRen;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    newitem.KouZokPtnKbn = setKouZokPtnKbn(newitem.UnkYmd, itemStaff);
                    if (CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null && CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault() != null)
                    {
                        syukinCalculationTimeMinute = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().SyukinCalculationTimeMinutes;
                        taikinCalculationTimeMinutes = CalculationTimedata.Where(t => t.CompanyCdSeq == CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault().TaikinCalculationTimeMinutes;
                    }
                    else
                    {
                        syukinCalculationTimeMinute = 0;
                        taikinCalculationTimeMinutes = 0;
                    }

                    DateTime.TryParseExact(enddatene.ToString("yyyyMMdd") + "0000",
                                               "yyyyMMddHHmm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out SyukinDate);
                    DateTime.TryParseExact(endDate + endTime,
                                               "yyyyMMddHHmm",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out TaikinDate);
                    newitem.SyukinYmd = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("yyyyMMdd");
                    newitem.TaikinYmd = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("yyyyMMdd");
                    newitem.SyukinTime = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("HHmm");
                    newitem.TaiknTime = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("HHmm");
                    newitem.FuriYmd = "";
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmm");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = formNm;
                    _dbContext.TkdKoban.Add(newitem);
                    _dbContext.SaveChanges();
                }
            }
            UpdateKouBnRenbyDate(startDate, endDate);

        }
        private byte setKouZokPtnKbn(string unkYmd,ItemStaff itemStaff)
        {
            byte kouZokPtnKbn=99;
             if (itemStaff.unSyuKoYmd.CompareTo(itemStaff.unKikYmd)==0)
            {
                kouZokPtnKbn = 1;
                return kouZokPtnKbn;
            }    
            if((itemStaff.UnkoJKbn!=3&&itemStaff.UnkoJKbn!=4)&&(itemStaff.unSyuKoYmd.CompareTo(itemStaff.unKikYmd)!=0)&&(itemStaff.unSyuKoYmd.CompareTo(unkYmd)==0))
            {
                 kouZokPtnKbn = 2;
                return kouZokPtnKbn;
            }    
            if((itemStaff.unSyuKoYmd.CompareTo(itemStaff.unKikYmd)!=0)&&(itemStaff.unSyuKoYmd.CompareTo(unkYmd)!=0)&&(itemStaff.unKikYmd.CompareTo(unkYmd)!=0))
            {
                kouZokPtnKbn = 3;
                 return kouZokPtnKbn;
            }    
            if((itemStaff.UnkoJKbn!=3&&itemStaff.UnkoJKbn!=4)&&(itemStaff.unHaiSYmd.CompareTo(itemStaff.unTouYmd)!=0)&&(itemStaff.unKikYmd.CompareTo(unkYmd)==0) )
            {
                kouZokPtnKbn = 4;
                return kouZokPtnKbn;
            }    
            if((itemStaff.UnkoJKbn==3||itemStaff.UnkoJKbn==4)&&(itemStaff.unSyuKoYmd.CompareTo(itemStaff.unKikYmd)!=0)&&(itemStaff.unSyuKoYmd.CompareTo(unkYmd)==0))
            {
                kouZokPtnKbn = 5;
                return kouZokPtnKbn;
            }    
            if((itemStaff.UnkoJKbn==3||itemStaff.UnkoJKbn==4)&&(itemStaff.unSyuKoYmd.CompareTo(itemStaff.unKikYmd)!=0)&&(itemStaff.unKikYmd.CompareTo(unkYmd)==0))
            {
                kouZokPtnKbn = 6;
                return kouZokPtnKbn;
            }    
                return kouZokPtnKbn;
        }

        public void UpdateLineKoban()
        {

        }
        public void DeleteKoban(int kinKyuTblCdSeq)
        {
            var ShuYmdlst = _dbContext.TkdKoban.Where(t => t.KinKyuTblCdSeq == kinKyuTblCdSeq).ToList();
            foreach (var item in ShuYmdlst)
            {
                var deleteKoban = _dbContext.TkdKoban.Find(item.UnkYmd, item.SyainCdSeq, item.KouBnRen);
                _dbContext.TkdKoban.Remove(deleteKoban);
                _dbContext.SaveChanges();
                UpdateKouBnRenbyDate(item.UnkYmd, item.UnkYmd);
            }
        }
        public void DeleteKobanbyUkeno(string ukeNo,short unkRen, short teiDanNo,short bunkRen,int syainCdSeq)
        {
            var Haishalst = _dbContext.TkdKoban.Where(t => t.UkeNo == ukeNo && t.UnkRen==unkRen && t.BunkRen==bunkRen &&t.TeiDanNo==teiDanNo && t.SyainCdSeq==syainCdSeq).ToList();
            foreach (var item in Haishalst)
            {
                var deleteKoban = _dbContext.TkdKoban.Find(item.UnkYmd, item.SyainCdSeq, item.KouBnRen);
                _dbContext.TkdKoban.Remove(deleteKoban);
                _dbContext.SaveChanges();
                UpdateKouBnRenbyDate(item.UnkYmd, item.UnkYmd);
            }
        }
        public async Task DeleteKobanbyHaisha(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            var Haishalst = _dbContext.TkdKoban.Where(t => t.UkeNo == ukeNo && t.UnkRen == unkRen && t.BunkRen == bunkRen && t.TeiDanNo == teiDanNo ).ToList();
            foreach (var item in Haishalst)
            {
                var deleteKoban = _dbContext.TkdKoban.Find(item.UnkYmd, item.SyainCdSeq, item.KouBnRen);
                _dbContext.TkdKoban.Remove(deleteKoban);
                await _dbContext.SaveChangesAsync();
                UpdateKouBnRenbyDate(item.UnkYmd, item.UnkYmd);
            }
        }
        public void UpdateDayoffLineData(int SyainCdSeqold,int SyainCdSeq,int KinKyuTblCdSeq,  int userlogin)
        {
            var checkkoban=_dbContext.TkdKoban.Where(t => t.SyainCdSeq == SyainCdSeqold && t.KinKyuTblCdSeq == KinKyuTblCdSeq).ToList();
            foreach (var item in checkkoban)
            {
                var removeKoban = _dbContext.TkdKoban.Find(item.UnkYmd, item.SyainCdSeq, item.KouBnRen);
                _dbContext.TkdKoban.Remove(removeKoban);
            }
            _dbContext.SaveChanges();
                foreach (var item in checkkoban)
            {
                TkdKoban inserttkdKoban = new TkdKoban();
                inserttkdKoban = item;
                inserttkdKoban.SyugyoKbn = 1;
                inserttkdKoban.SyainCdSeq = SyainCdSeq;
                inserttkdKoban.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                inserttkdKoban.UpdTime = DateTime.Now.ToString("HHmmss");
                inserttkdKoban.UpdPrgId = Common.UpdPrgId;
                inserttkdKoban.UpdSyainCd = userlogin;
                inserttkdKoban.HenKai = (short)(inserttkdKoban.HenKai + 1);
                _dbContext.TkdKoban.Add(inserttkdKoban);
                _dbContext.SaveChanges();
                UpdateKouBnRenbyDate(item.UnkYmd, item.UnkYmd);
            }  
        }

        public void UpdateLineData(int SyainCdSeqold,int SyainCdSeq,string ukeNo, short unkRen, short teiDanNo, short bunkRen,  int userlogin, ItemStaff updatedStaff)
        {
            var checkkoban = _dbContext.TkdKoban.Where(t => t.SyainCdSeq == SyainCdSeqold && t.UkeNo == ukeNo && t.UnkRen==unkRen&& t.TeiDanNo==teiDanNo&&t.BunkRen==bunkRen).ToList();
            if(checkkoban.Count()!=0)
            {
                foreach (var item in checkkoban)
                {
                    var removeKoban = _dbContext.TkdKoban.Find(item.UnkYmd, item.SyainCdSeq, item.KouBnRen);
                    _dbContext.TkdKoban.Remove(removeKoban);
                }
                _dbContext.SaveChanges();
                foreach (var item in checkkoban)
                {
                    var checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(item.UnkYmd) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    int countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(item.UnkYmd) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    TkdKoban inserttkdKoban = new TkdKoban();
                    inserttkdKoban = item;
                    inserttkdKoban.KouBnRen = (short)countDayoff;
                    inserttkdKoban.SyugyoKbn = 1;
                    inserttkdKoban.SyainCdSeq = SyainCdSeq;
                    inserttkdKoban.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    inserttkdKoban.UpdTime = DateTime.Now.ToString("HHmmss");
                    inserttkdKoban.UpdPrgId = Common.UpdPrgId;
                    inserttkdKoban.UpdSyainCd = userlogin;
                    inserttkdKoban.HenKai = (short)(inserttkdKoban.HenKai + 1);
                    _dbContext.TkdKoban.Add(inserttkdKoban);
                    _dbContext.SaveChanges();
                    UpdateKouBnRenbyDate(item.UnkYmd, item.UnkYmd);
                }
            }  
            else
            {
                int CompanyCdSeq = (from KYOSHE in _dbContext.VpmKyoShe
                                    join SYAIN in _dbContext.VpmSyain on KYOSHE.SyainCdSeq equals SYAIN.SyainCdSeq into SYAIN_join
                                    from SYAIN in SYAIN_join.DefaultIfEmpty()
                                    join EIGYOS in _dbContext.VpmEigyos on KYOSHE.EigyoCdSeq equals EIGYOS.EigyoCdSeq into EIGYOS_join
                                    from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                                    join KAISHA in _dbContext.VpmCompny
                                          on new { EIGYOS.CompanyCdSeq,  TenantCdSeq = new ClaimModel().TenantID }
                                      equals new { KAISHA.CompanyCdSeq, KAISHA.TenantCdSeq }
                                    where
                                     string.Compare(KYOSHE.StaYmd, updatedStaff.StartDate) <= 0 &&
                                     string.Compare(KYOSHE.EndYmd, updatedStaff.StartDate) >= 0 &&
                                     KYOSHE.SyainCdSeq == int.Parse(updatedStaff.BusLine)
                                    select new
                                    {
                                        KAISHA.CompanyCdSeq
                                    }).First().CompanyCdSeq;
                UpdateTimeKoban(updatedStaff.StartDate, updatedStaff.TimeStart.ToString("D4"), updatedStaff.EndDate, updatedStaff.TimeEnd.ToString("D4"), int.Parse(updatedStaff.BusLine),CompanyCdSeq, updatedStaff, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,"KU1300");
            }    
            
        }


        public void UpdateDayoffData(string startDate, string startTime, string endDate, string endTime, int SyainCdSeq,int KinKyuTblCdSeq, int KinKyuKbn,string furiYdm, int userlogin)
        {
            DateTime startdatene;
            DateTime.TryParseExact(startDate,
                           "yyyyMMdd",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out startdatene);

            DateTime enddatene;
            DateTime.TryParseExact(endDate, 
                           "yyyyMMdd",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out enddatene);
            double totaldate = (enddatene - startdatene).TotalDays;
            if (totaldate == 0)
            {
                var checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                int countDayoff = 1;
                if(checkcount.Count>0)
                {
                    countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max()+1;
                }    
                TkdKoban newitem = new TkdKoban();
                newitem.UnkYmd = startDate;
                newitem.SyainCdSeq = SyainCdSeq;
                newitem.KouBnRen = (short)countDayoff;
                newitem.HenKai = 0;
                newitem.SyugyoKbn = 1;
                newitem.KinKyuTblCdSeq = KinKyuTblCdSeq;
                newitem.UkeNo = "0";
                newitem.UnkRen = 0;
                newitem.SyaSyuRen = 0;
                newitem.TeiDanNo = 0;
                newitem.BunkRen = 0;
                newitem.RotCdSeq = 0;
                newitem.RenEigCd = 0;
                newitem.SigySyu = 0;
                newitem.KitYmd = "";
                newitem.SigyKbn = 0;
                newitem.SiyoKbn = 1;
                newitem.SigyCd = "";
                newitem.KouZokPtnKbn = 8;
                if(KinKyuKbn==1)
                {
                    newitem.KouZokPtnKbn = 9;
                }
                newitem.SyukinYmd = startDate;
                newitem.TaikinYmd = endDate;
                newitem.SyukinTime = startTime;
                newitem.TaiknTime = endTime;
                newitem.FuriYmd = furiYdm==null?"":furiYdm ;
                newitem.RouTime = "";
                newitem.KouStime = "";
                newitem.TaikTime = "";
                newitem.KyuKtime = "";
                newitem.JitdTime = "";
                newitem.ZangTime = "";
                newitem.UsinyTime = "";
                newitem.Syukinbasy = "";
                newitem.SsinTime = "";
                newitem.BikoNm = "";
                newitem.TaiknBasy = "";
                newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                newitem.UpdTime = DateTime.Now.ToString("hhmm");
                newitem.UpdSyainCd = userlogin;
                newitem.UpdPrgId = "KU1300";
                _dbContext.TkdKoban.Add(newitem);
                _dbContext.SaveChanges();
            }
            else
            {
                if (totaldate == 1)
                {
                    var checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    int countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    TkdKoban newitem = new TkdKoban();
                    newitem.UnkYmd = startDate;
                    newitem.SyainCdSeq = SyainCdSeq;
                    newitem.KouBnRen = (short)countDayoff;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = KinKyuTblCdSeq;
                    newitem.UkeNo = "0";
                    newitem.UnkRen = 0;
                    newitem.SyaSyuRen = 0;
                    newitem.TeiDanNo = 0;
                    newitem.BunkRen = 0;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    newitem.KouZokPtnKbn = 8;
                    if (KinKyuKbn == 1)
                    {
                        newitem.KouZokPtnKbn = 9;
                    }
                    newitem.SyukinYmd = startDate;
                    newitem.TaikinYmd = startDate;
                    newitem.SyukinTime = startTime;
                    newitem.TaiknTime = "2359";
                    newitem.FuriYmd = furiYdm==null?"":furiYdm;
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmm");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = "KU1300";
                    _dbContext.TkdKoban.Add(newitem);
                    _dbContext.SaveChanges();

                    checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(endDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(endDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    newitem = new TkdKoban();
                    newitem.UnkYmd = endDate;
                    newitem.SyainCdSeq = SyainCdSeq;
                    newitem.KouBnRen = (short)countDayoff;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = KinKyuTblCdSeq;
                    newitem.UkeNo = "0";
                    newitem.UnkRen = 0;
                    newitem.SyaSyuRen = 0;
                    newitem.TeiDanNo = 0;
                    newitem.BunkRen = 0;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    newitem.KouZokPtnKbn = 8;
                    if (KinKyuKbn == 1)
                    {
                        newitem.KouZokPtnKbn = 9;
                    }
                    newitem.SyukinYmd = endDate;
                    newitem.TaikinYmd = endDate;
                    newitem.SyukinTime = "0000";
                    newitem.TaiknTime = endTime;
                    newitem.FuriYmd = furiYdm==null?"":furiYdm;
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmm");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = "KU1300";
                    _dbContext.TkdKoban.Add(newitem);
                    _dbContext.SaveChanges();
                }
                else
                {
                    var checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    int countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    TkdKoban newitem = new TkdKoban();
                    newitem.UnkYmd = startDate;
                    newitem.SyainCdSeq = SyainCdSeq;
                    newitem.KouBnRen = (short)countDayoff;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = KinKyuTblCdSeq;
                    newitem.UkeNo = "0";
                    newitem.UnkRen = 0;
                    newitem.SyaSyuRen = 0;
                    newitem.TeiDanNo = 0;
                    newitem.BunkRen = 0;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    newitem.KouZokPtnKbn = 8;
                    if (KinKyuKbn == 1)
                    {
                        newitem.KouZokPtnKbn = 9;
                    }
                    newitem.SyukinYmd = startDate;
                    newitem.TaikinYmd = startDate;
                    newitem.SyukinTime = startTime;
                    newitem.TaiknTime = "2359";
                    newitem.FuriYmd = furiYdm==null?"":furiYdm;
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmm");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = "KU1300";
                    _dbContext.TkdKoban.Add(newitem);
                    _dbContext.SaveChanges();

                    for (int i = 1; i < totaldate; i++)
                    {
                        checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startdatene.AddDays(i).ToString("yyyyMMdd")) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                        countDayoff = 1;
                        if (checkcount.Count > 0)
                        {
                            countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(startdatene.AddDays(i).ToString("yyyyMMdd")) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                        }
                        newitem = new TkdKoban();
                        newitem.UnkYmd = startdatene.AddDays(i).ToString("yyyyMMdd");
                        newitem.SyainCdSeq = SyainCdSeq;
                        newitem.KouBnRen = (short)countDayoff;
                        newitem.HenKai = 0;
                        newitem.SyugyoKbn = 1;
                        newitem.KinKyuTblCdSeq = KinKyuTblCdSeq;
                        newitem.UkeNo = "0";
                        newitem.UnkRen = 0;
                        newitem.SyaSyuRen = 0;
                        newitem.TeiDanNo = 0;
                        newitem.BunkRen = 0;
                        newitem.RotCdSeq = 0;
                        newitem.RenEigCd = 0;
                        newitem.SigySyu = 0;
                        newitem.KitYmd = "";
                        newitem.SigyKbn = 0;
                        newitem.SiyoKbn = 1;
                        newitem.SigyCd = "";
                        newitem.KouZokPtnKbn = 8;
                        if (KinKyuKbn == 1)
                        {
                            newitem.KouZokPtnKbn = 9;
                        }
                        newitem.SyukinYmd = newitem.UnkYmd;
                        newitem.TaikinYmd = newitem.UnkYmd;
                        newitem.SyukinTime = "0000";
                        newitem.TaiknTime = "2359";
                        newitem.FuriYmd = furiYdm==null?"":furiYdm;
                        newitem.RouTime = "";
                        newitem.KouStime = "";
                        newitem.TaikTime = "";
                        newitem.KyuKtime = "";
                        newitem.JitdTime = "";
                        newitem.ZangTime = "";
                        newitem.UsinyTime = "";
                        newitem.Syukinbasy = "";
                        newitem.SsinTime = "";
                        newitem.BikoNm = "";
                        newitem.TaiknBasy = "";
                        newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        newitem.UpdTime = DateTime.Now.ToString("hhmm");
                        newitem.UpdSyainCd = userlogin;
                        newitem.UpdPrgId = "KU1300";
                        _dbContext.TkdKoban.Add(newitem);
                        _dbContext.SaveChanges();
                    }
                        checkcount = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(endDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).ToList();
                    countDayoff = 1;
                    if (checkcount.Count > 0)
                    {
                        countDayoff = _dbContext.TkdKoban.Where(t => t.UnkYmd.CompareTo(endDate) == 0 && t.SyainCdSeq == SyainCdSeq).Select(t => t.KouBnRen).Max() + 1;
                    }
                    newitem = new TkdKoban();
                    newitem.UnkYmd = endDate;
                    newitem.SyainCdSeq = SyainCdSeq;
                    newitem.KouBnRen = (short)countDayoff;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = KinKyuTblCdSeq;
                    newitem.UkeNo = "0";
                    newitem.UnkRen = 0;
                    newitem.SyaSyuRen = 0;
                    newitem.TeiDanNo = 0;
                    newitem.BunkRen = 0;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    newitem.KouZokPtnKbn = 8;
                    if (KinKyuKbn == 1)
                    {
                        newitem.KouZokPtnKbn = 9;
                    }
                    newitem.SyukinYmd = endDate;
                    newitem.TaikinYmd = endDate;
                    newitem.SyukinTime = "0000";
                    newitem.TaiknTime = endTime;
                    newitem.FuriYmd = furiYdm==null?"":furiYdm;
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmm");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = "KU1300";
                    _dbContext.TkdKoban.Add(newitem);
                    _dbContext.SaveChanges();                }

            }
            UpdateKouBnRenbyDate(startDate, endDate);
        }
    }
}
