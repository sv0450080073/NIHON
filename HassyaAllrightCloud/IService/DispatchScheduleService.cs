using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IDispatchScheduleService
    {
        Task<List<LoadDispatchSchedule>> Get(int StaffCdSeq);
    }

    public class DispatchScheduleService : IDispatchScheduleService
    {
        private readonly KobodbContext _dbContext;
        public DispatchScheduleService(KobodbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<LoadDispatchSchedule>> Get(int StaffCdSeq)
        {
            return await (
                from haiin in _dbContext.TkdHaiin
                join haisha in _dbContext.TkdHaisha
                    on new { haiin.UkeNo, haiin.UnkRen, haiin.TeiDanNo, haiin.BunkRen }
                    equals new { haisha.UkeNo, haisha.UnkRen, haisha.TeiDanNo, haisha.BunkRen }
                where haiin.SiyoKbn == 1
                    && haisha.SiyoKbn == 1
                    && haiin.SyainCdSeq == StaffCdSeq
                select new LoadDispatchSchedule()
                {
                    StaffCdSeq = haiin.SyainCdSeq,
                    StockOut = DateTime.ParseExact(haisha.SyuKoYmd + haisha.SyuKoTime, "yyyyMMddHHmm", null),
                    Arrival = DateTime.ParseExact(haisha.TouYmd + haisha.TouChTime, "yyyyMMddHHmm", null),
                    Destination = haisha.IkNm
                }).ToListAsync();
        }
    }
}
