using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Domain.Dto;
using System.Linq;
using System.Globalization;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_ShuriDataListService
    {

        Task<List<TKD_ShuriData>> Getdata(DateTime busstardate, DateTime busenddate);
        void UpdateStatusBusRepair(int ShuriTblSeq, int userid);
        void UpdateBusRepair(ItemBus item, int userid);

    }
    public class TKD_ShuriDataService : ITKD_ShuriDataListService
    {
        private readonly KobodbContext _context;

        public TKD_ShuriDataService(KobodbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// update bus repair
        /// </summary>
        /// <param name="itembusrp"></param>
        /// <param name="userid"></param>
        public void UpdateBusRepair(ItemBus itembusrp, int userid)
        {
            var updateShuri = _context.TkdShuri.Find(itembusrp.Shuri_ShuriTblSeq);
            updateShuri.SyaRyoCdSeq = int.Parse(itembusrp.BusLine);
            updateShuri.BikoNm = itembusrp.BikoNm;
            updateShuri.ShuriCdSeq = itembusrp.ShuriCdSeq;
            updateShuri.ShuriSymd = itembusrp.StartDate;
            updateShuri.ShuriStime = itembusrp.TimeStart.ToString("D4");
            updateShuri.ShuriEymd = itembusrp.EndDate;
            updateShuri.ShuriEtime = itembusrp.TimeEnd.ToString("D4");
            updateShuri.HenKai += 1;
            updateShuri.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            updateShuri.UpdTime = DateTime.Now.ToString("hhmmss");
            updateShuri.UpdSyainCd = userid;
            updateShuri.UpdPrgId = Common.UpdPrgId;
            var ShuYmdlst = _context.TkdShuYmd.Where(t => t.ShuriTblSeq == itembusrp.Shuri_ShuriTblSeq).ToList();
            foreach (var item in ShuYmdlst)
            {
                var deleteShuYmd = _context.TkdShuYmd.Find(item.ShuriTblSeq, item.ShuriYmd);
                _context.TkdShuYmd.Remove(deleteShuYmd);
            }
            _context.SaveChanges();
            DateTime startdatene;
            DateTime.TryParseExact(itembusrp.StartDate,
                           "yyyyMMdd",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out startdatene);

            DateTime enddatene;
            DateTime.TryParseExact(itembusrp.EndDate,
                           "yyyyMMdd",
                           CultureInfo.CurrentCulture,
                           DateTimeStyles.None,
                           out enddatene);
            double totaldate = (enddatene - startdatene).TotalDays;
            if (totaldate == 0)
            {
                TkdShuYmd newitem = new TkdShuYmd();
                newitem.ShuriTblSeq = itembusrp.Shuri_ShuriTblSeq;
                newitem.ShuriYmd = itembusrp.StartDate;
                newitem.HenKai = 0;
                newitem.ShuriStime = itembusrp.TimeStart.ToString("D4");
                newitem.ShuriEtime = itembusrp.TimeEnd.ToString("D4");
                newitem.SiyoKbn = 1;
                newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                newitem.UpdTime = DateTime.Now.ToString("hhmm");
                newitem.UpdSyainCd = userid;
                newitem.UpdPrgId = Common.UpdPrgId;
                _context.TkdShuYmd.Add(newitem);
                _context.SaveChanges();
            }
            else
            {
                if (totaldate == 1)
                {
                    TkdShuYmd newitemfirst = new TkdShuYmd();
                    newitemfirst.ShuriTblSeq = itembusrp.Shuri_ShuriTblSeq;
                    newitemfirst.ShuriYmd = itembusrp.StartDate;
                    newitemfirst.HenKai = 0;
                    newitemfirst.ShuriStime = itembusrp.TimeStart.ToString("D4");
                    newitemfirst.ShuriEtime = "2359";
                    newitemfirst.SiyoKbn = 1;
                    newitemfirst.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitemfirst.UpdTime = DateTime.Now.ToString("hhmm");
                    newitemfirst.UpdSyainCd = userid;
                    newitemfirst.UpdPrgId = Common.UpdPrgId;
                    _context.TkdShuYmd.Add(newitemfirst);

                    TkdShuYmd newitemlast = new TkdShuYmd();
                    newitemlast.ShuriTblSeq = itembusrp.Shuri_ShuriTblSeq;
                    newitemlast.ShuriYmd = itembusrp.EndDate;
                    newitemlast.HenKai = 0;
                    newitemlast.ShuriStime = "0000";
                    newitemlast.ShuriEtime = itembusrp.TimeEnd.ToString("D4");
                    newitemlast.SiyoKbn = 1;
                    newitemlast.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitemlast.UpdTime = DateTime.Now.ToString("hhmm");
                    newitemlast.UpdSyainCd = userid;
                    newitemlast.UpdPrgId = Common.UpdPrgId;
                    _context.TkdShuYmd.Add(newitemlast);
                }
                else
                {
                    TkdShuYmd newitemfirst = new TkdShuYmd();
                    newitemfirst.ShuriTblSeq = itembusrp.Shuri_ShuriTblSeq;
                    newitemfirst.ShuriYmd = itembusrp.StartDate;
                    newitemfirst.HenKai = 0;
                    newitemfirst.ShuriStime = itembusrp.TimeStart.ToString("D4");
                    newitemfirst.ShuriEtime = "2359";
                    newitemfirst.SiyoKbn = 1;
                    newitemfirst.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitemfirst.UpdTime = DateTime.Now.ToString("hhmm");
                    newitemfirst.UpdSyainCd = userid;
                    newitemfirst.UpdPrgId = Common.UpdPrgId;
                    _context.TkdShuYmd.Add(newitemfirst);
                    for (int i = 1; i < totaldate; i++)
                    {
                        TkdShuYmd newitem = new TkdShuYmd();
                        newitem.ShuriTblSeq = itembusrp.Shuri_ShuriTblSeq;
                        newitem.ShuriYmd = startdatene.AddDays(i).ToString("yyyyMMdd");
                        newitem.HenKai = 0;
                        newitem.ShuriStime = "0000";
                        newitem.ShuriEtime = "2359";
                        newitem.SiyoKbn = 1;
                        newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        newitem.UpdTime = DateTime.Now.ToString("hhmm");
                        newitem.UpdSyainCd = userid;
                        newitem.UpdPrgId = Common.UpdPrgId;
                        _context.TkdShuYmd.Add(newitem);
                    }
                    TkdShuYmd newitemlast = new TkdShuYmd();
                    newitemlast.ShuriTblSeq = itembusrp.Shuri_ShuriTblSeq;
                    newitemlast.ShuriYmd = itembusrp.EndDate;
                    newitemlast.HenKai = 0;
                    newitemlast.ShuriStime = "0000";
                    newitemlast.ShuriEtime = itembusrp.TimeEnd.ToString("D4");
                    newitemlast.SiyoKbn = 1;
                    newitemlast.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitemlast.UpdTime = DateTime.Now.ToString("hhmm");
                    newitemlast.UpdSyainCd = userid;
                    newitemlast.UpdPrgId = Common.UpdPrgId;
                    _context.TkdShuYmd.Add(newitemlast);
                }
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// update status busrepair
        /// </summary>
        /// <param name="ShuriTblSeq"></param>
        /// <param name="userid"></param>
        public void UpdateStatusBusRepair(int ShuriTblSeq, int userid)
        {
            var updateShuri = _context.TkdShuri.Find(ShuriTblSeq);
            updateShuri.SiyoKbn = 2;
            updateShuri.HenKai += 1;
            updateShuri.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            updateShuri.UpdTime = DateTime.Now.ToString("hhmmss");
            updateShuri.UpdSyainCd = userid;
            updateShuri.UpdPrgId = Common.UpdPrgId;
            var ShuYmdlst = _context.TkdShuYmd.Where(t => t.ShuriTblSeq == ShuriTblSeq).ToList();
            foreach (var item in ShuYmdlst)
            {
                var deleteShuYmd = _context.TkdShuYmd.Find(item.ShuriTblSeq, item.ShuriYmd);
                _context.TkdShuYmd.Remove(deleteShuYmd);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// get data bus repair
        /// </summary>
        /// <param name="busstardate"></param>
        /// <param name="busenddate"></param>
        /// <returns></returns>
        public async Task<List<TKD_ShuriData>> Getdata(DateTime busstardate, DateTime busenddate)
        {
            int tenantCdSeq = new ClaimModel().TenantID;
            string DateStarAsString = busstardate.ToString("yyyyMMdd");
            string DateEndAsString = busenddate.ToString("yyyyMMdd");
            return await (
                from TKDShuri in _context.TkdShuri
                join TKDShuYmd in _context.TkdShuYmd on TKDShuri.ShuriTblSeq equals TKDShuYmd.ShuriTblSeq into TKD_ShuYmd_join
                from TKDShuYmd in TKD_ShuYmd_join.DefaultIfEmpty()
                    /*Update by M*/
                    join VPM_SyaRyo in _context.VpmSyaRyo 
                    on TKDShuri.SyaRyoCdSeq  equals VPM_SyaRyo.SyaRyoCdSeq into VPM_SyaRyo_join
                from VPM_SyaRyo in VPM_SyaRyo_join.DefaultIfEmpty()
                join VPM_SyaSyu in _context.VpmSyaSyu 
                on VPM_SyaRyo.SyaSyuCdSeq  equals VPM_SyaSyu.SyaSyuCdSeq into VPM_SyaSyu_join
                 from VPM_SyaSyu in VPM_SyaSyu_join.DefaultIfEmpty()
                /*End*/
                where TKDShuri.SiyoKbn == 1 && TKDShuYmd.SiyoKbn == 1 &&
                VPM_SyaSyu.TenantCdSeq==tenantCdSeq &&
                string.Compare(TKDShuri.ShuriEymd, DateStarAsString) >= 0 &&
                 string.Compare(TKDShuri.ShuriSymd, DateEndAsString) <= 0
                select new TKD_ShuriData
                {
                    Shuri_ShuriCdSeq = TKDShuri.ShuriCdSeq,
                    Shuri_SyaRyoCdSeq = TKDShuri.SyaRyoCdSeq,
                    Shuri_ShuriTblSeq = TKDShuri.ShuriTblSeq,
                    Shuri_ShuriSYmd = TKDShuri.ShuriSymd,
                    Shuri_ShuriSTime = TKDShuri.ShuriStime,
                    Shuri_ShuriEYmd = TKDShuri.ShuriEymd,
                    Shuri_ShuriETime = TKDShuri.ShuriEtime,
                    Shuri_BikoNm = TKDShuri.BikoNm,
                    SyaRyo_NinKaKbn = VPM_SyaRyo.NinkaKbn,
                    Syasyu_KataKbn = VPM_SyaSyu.KataKbn
                }).Distinct().ToListAsync();

        }
    }
}
