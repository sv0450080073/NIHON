using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.StaffSchedule.Queries
{
    public class SaveCalendarSet : IRequest<bool>
    {
        public CalendarSetModel CalendarSetModel { get; set; }

        public bool IsDelete { get; set; }

        public class Handler : IRequestHandler<SaveCalendarSet, bool>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<bool> Handle(SaveCalendarSet request, CancellationToken cancellationToken)
            {
                if (request.IsDelete)
                {
                    var existCalendar = _dbcontext.TkdSchCalendar.Where(x => x.CalendarSeq == request.CalendarSetModel.CalendarSeq).FirstOrDefault();
                    if(existCalendar != null)
                    {
                        existCalendar.SiyoKbn = 0;
                        _dbcontext.TkdSchCalendar.Update(existCalendar);
                    }
                }
                else
                {
                    var existCalendar = _dbcontext.TkdSchCalendar.Where(x => x.CalendarSeq == request.CalendarSetModel.CalendarSeq).FirstOrDefault();

                    if (existCalendar == null)
                    {
                        TkdSchCalendar tkdSchCalendar = new TkdSchCalendar()
                        {
                            CalendarName = request.CalendarSetModel.CalendarName,
                            CompanyCdSeq = request.CalendarSetModel.CompanyCdSeq,
                            UpdPrgId = Common.UpdPrgId,
                            UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                            UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty),
                            UpdTime = DateTime.Now.ToString().Substring(11).Replace(":", string.Empty),
                            SiyoKbn = 1
                        };
                        _dbcontext.TkdSchCalendar.Add(tkdSchCalendar);
                    }
                    else
                    {
                        existCalendar.CalendarName = request.CalendarSetModel.CalendarName;
                        _dbcontext.TkdSchCalendar.Update(existCalendar);
                    }
                }
                
                _dbcontext.SaveChanges();
                return true;
            }
        }
    }
}
