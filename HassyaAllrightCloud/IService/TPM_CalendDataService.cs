using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_CalendDataListService
    {
        TkdCalend Getdatabydate(int company, byte calensys, string date);
        Task<List<TkdCalend>> Getdatabydays(int company, byte calensys, string startdate, string enddate,int TenantCdSeq);

        bool UpdateCommentDate(string date, string comment, int company, byte calensys, int userlogin,string UpdPrgId);

    }
    public class TPM_CalendDataService : ITPM_CalendDataListService
    {
        private readonly KobodbContext _dbContext;

        public TPM_CalendDataService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<TkdCalend>> Getdatabydays(int company, byte calensys, string startdate, string enddate,int TenantCdSeq)
        {
            return await _dbContext.TkdCalend.Where(t => t.CompanyCdSeq == company && t.CalenSyu == calensys && t.CalenYmd.CompareTo(startdate) >= 0 && t.CalenYmd.CompareTo(enddate) <= 0).ToListAsync();
        }
        public TkdCalend Getdatabydate(int company, byte calensys, string date)
        {
            try
            {
                string yymm = date.Substring(0, 6);
                string dd = date.Substring(6, 2);
                return _dbContext.TkdCalend.Where(t => t.CompanyCdSeq == company && t.CalenSyu == calensys && t.CalenYm == yymm && t.CalenRen == dd).FirstOrDefault();
            }
            catch
            {
                return new TkdCalend();
            }
            
        }
        /// <summary>
        /// update comment in header
        /// </summary>
        /// <param name="date"></param>
        /// <param name="comment"></param>
        /// <param name="company"></param>
        /// <param name="calensys"></param>
        /// <param name="userlogin"></param>
        /// <returns></returns>
        public bool UpdateCommentDate(string date, string comment, int company, byte calensys, int userlogin,string UpdPrgId)
        {
            DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            string yymm = date.Substring(0, 6);
            string dd = date.Substring(6, 2);
            string commentold = "";
            try
            {
                var update = _dbContext.TkdCalend.Find(company, calensys, yymm, dd);
                if (update == null)
                {
                    TkdCalend newCalen = new TkdCalend();
                    newCalen.CompanyCdSeq = company;
                    newCalen.CalenSyu = 1;
                    newCalen.CalenYm = yymm;
                    newCalen.CalenRen = dd;
                    newCalen.CalenYmd = date;
                    if (dt.DayOfWeek == DayOfWeek.Saturday)
                    {
                        newCalen.CalenKbn = 2;
                    }
                    else if (dt.DayOfWeek == DayOfWeek.Sunday)
                    {
                        newCalen.CalenKbn = 3;
                    }
                    else
                    {
                        newCalen.CalenKbn = 1;
                    }
                    newCalen.CalenRank = 0;
                    newCalen.CalenCom = comment;
                    newCalen.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newCalen.UpdTime = DateTime.Now.ToString("HHmmss");
                    newCalen.UpdPrgId = UpdPrgId;
                    newCalen.UpdSyainCd = userlogin;
                    _dbContext.TkdCalend.Add(newCalen);
                    _dbContext.SaveChanges();
                }
                else
                {
                    commentold = update.CalenCom;
                    update.CalenCom = comment;
                    update.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    update.UpdTime = DateTime.Now.ToString("HHmmss");
                    update.UpdPrgId = UpdPrgId;
                    update.UpdSyainCd = userlogin;
                    _dbContext.Update(update);
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch
            {
                var update = _dbContext.TkdCalend.Find(company, calensys, yymm, dd);
                update.CalenCom = commentold;
                update.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                update.UpdTime = DateTime.Now.ToString("HHmmss");
                update.UpdPrgId = UpdPrgId;
                update.UpdSyainCd = userlogin;
                _dbContext.Update(update);
                _dbContext.SaveChanges();
                return false;
            }
        }
    }
}
