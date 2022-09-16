using DevExpress.Charts.Native;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ILeaveDayService
    {
        Task<List<LoadLeaveDay>> Get(int StaffCdSeq);        
    }

    public class LeaveDayService : ILeaveDayService
    {
        private readonly KobodbContext _dbContext;
        public LeaveDayService(KobodbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<LoadLeaveDay>> Get(int StaffCdSeq)
        {
            return await (
                from tkdKikyuj in _dbContext.TkdKikyuj
                join VpmKinKyu in _dbContext.VpmKinKyu
                on tkdKikyuj.KinKyuCdSeq equals VpmKinKyu.KinKyuCdSeq into tv
                from tvTemp in tv.DefaultIfEmpty()
                join vpmsyain in _dbContext.VpmSyain
                on tkdKikyuj.SyainCdSeq equals vpmsyain.SyainCdSeq into tvv
                from tvvTemp in tvv.DefaultIfEmpty()
                where tkdKikyuj.SiyoKbn == 1
                    && tkdKikyuj.SyainCdSeq == StaffCdSeq
                select new LoadLeaveDay()
                {
                    Start = DateTime.ParseExact(tkdKikyuj.KinKyuSymd + tkdKikyuj.KinKyuStime, "yyyyMMddHHmm", null),
                    End = DateTime.ParseExact(tkdKikyuj.KinKyuEymd + tkdKikyuj.KinKyuEtime, "yyyyMMddHHmm", null),
                    Type = tkdKikyuj.KinKyuCdSeq,
                    TypeName = tvTemp.KinKyuNm,
                    EmployeeName = tvvTemp.SyainNm,
                    IsLeave = tvTemp.KinKyuKbn == 2 ? true : false,
                    Remark = tkdKikyuj.BikoNm

                }).ToListAsync();
        }

    }
}
