using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StaffSchedule.Commands
{
    public class SaveStaffSchedule : IRequest<bool>
    {
        public StaffScheduleData model { get; set; }
        public class Handler : IRequestHandler<SaveStaffSchedule, bool>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<bool> Handle(SaveStaffSchedule request, CancellationToken cancellationToken)
            {
                var endOfRecurrence = string.Empty;
                if (request.model.RecurrenceRule != null && request.model.RecurrenceRule != string.Empty)
                {
                    var Recurrences = RecurrenceHelper.GetRecurrenceDateTimeCollection(request.model.RecurrenceRule, request.model.StartDate).ToList();
                    endOfRecurrence = Recurrences.Count != 0 ? Recurrences[Recurrences.Count - 1].ToString().Substring(0, 10).Replace("/", string.Empty) : request.model.EndDate.ToString().Substring(0, 10).Replace("/", string.Empty);
                }

                var scheduleLable = string.Empty;
                if (request.model.ScheduleLabel.Count != 0)
                {
                    if (request.model.ScheduleLabel.Count == 1)
                    {
                        scheduleLable = request.model.ScheduleLabel[0].ToString();
                    }
                    else
                    {
                        scheduleLable = request.model.ScheduleLabel[0].ToString() + "," + request.model.ScheduleLabel[1].ToString();
                    }
                }
                var codeKbn = _dbContext.VpmCodeKb.Where(x => x.CodeSyu == FormatString.yoteiType).ToList();
                TkdSchYotei tkdSchYotei = new TkdSchYotei()
                {
                    YoteiType = codeKbn.Where(x => x.CodeKbn == request.model.DisplayType.ToString()).FirstOrDefault().CodeKbnSeq,
                    KinKyuCdSeq = request.model.VacationType,
                    SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                    KinKyuTblCdSeq = 0,
                    Title = request.model.Text,
                    YoteiSymd = request.model.StartDate.ToString().Substring(0, 10).Replace("/", string.Empty),
                    YoteiStime = request.model.StartDate.ToString().Substring(11).Replace(":", string.Empty).Length == 5 ? request.model.StartDate.ToString().Substring(11).Replace(":", string.Empty).Insert(0, "0") : request.model.StartDate.ToString().Substring(11).Replace(":", string.Empty),
                    YoteiEymd = request.model.EndDate.ToString().Substring(0, 10).Replace("/", string.Empty),
                    YoteiEtime = request.model.EndDate.ToString().Substring(11).Replace(":", string.Empty).Length == 5 ? request.model.EndDate.ToString().Substring(11).Replace(":", string.Empty).Insert(0, "0") : request.model.EndDate.ToString().Substring(11).Replace(":", string.Empty),
                    TukiLabKbn = scheduleLable,
                    AllDayKbn = request.model.AllDay == true ? (byte)1 : (byte)0,
                    KuriRule = request.model.RecurrenceRule == null ? "" : request.model.RecurrenceRule,
                    KuriReg = request.model.RecurrenceException == null ? "" : request.model.RecurrenceException,
                    KuriEndYmd = endOfRecurrence,
                    GaiKkKbn = (byte)request.model.isPublic,
                    YoteiBiko = request.model.Description == null ? "" : request.model.Description,
                    YoteiShoKbn = (byte)1,
                    ShoSyainCdSeq = (byte)0,
                    ShoUpdYmd = "",
                    ShoUpdTime = "",
                    ShoRejBiko = "",
                    SiyoKbn = (byte)1,
                    UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty),
                    UpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8).Replace(":", string.Empty),
                    UpdSyainCd = Constants.SyainCdSeq,
                    UpdPrgId = "CONVERT",
                    CalendarSeq = request.model.CalendarSeq,
                    KuriKbn = request.model.RecurrenceRule == null ? (byte)0 : (byte)1
                };
                _dbContext.TkdSchYotei.Add(tkdSchYotei);
                _dbContext.SaveChanges();
                if (request.model.Staffs != null)
                {
                    SaveStaffOfSchedule(request.model, tkdSchYotei.YoteiSeq);
                }
                return true;
            }
            private bool SaveStaffOfSchedule(StaffScheduleData model, int yoteiSeq)
            {
                foreach (var item in model.Staffs)
                {
                    TkdSchYotKsya tkdSchYotKsya = new TkdSchYotKsya()
                    {
                        YoteiSeq = yoteiSeq,
                        SyainCdSeq = item,
                        UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty),
                        UpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8).Replace(":", string.Empty),
                        UpdSyainCd = Constants.SyainCdSeq,
                        UpdPrgId = Constants.CompanyCdSeq.ToString()
                    };
                    _dbContext.TkdSchYotKsya.Add(tkdSchYotKsya);
                }
                _dbContext.SaveChanges();
                return true;
            }
        }
    }
}
