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
    public class UpdateStaffSchedule : IRequest<bool>
    {
        public StaffScheduleData model { get; set; }
        public class Handler : IRequestHandler<UpdateStaffSchedule, bool>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<bool> Handle(UpdateStaffSchedule request, CancellationToken cancellationToken)
            {
                var codeKbn = _dbContext.VpmCodeKb.Where(x => x.CodeSyu == FormatString.yoteiType).ToList();
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

                var bookedSchedule = _dbContext.TkdSchYotKsya.Where(r => r.YoteiSeq == request.model.scheduleId).ToList();
                var newSchedule = _dbContext.TkdSchYotei.Where(y => y.YoteiSeq == (int)request.model.scheduleId).FirstOrDefault();

                newSchedule.YoteiType = codeKbn.Where(x => x.CodeKbn == request.model.DisplayType.ToString()).FirstOrDefault().CodeKbnSeq;
                newSchedule.KinKyuCdSeq = request.model.VacationType;
                newSchedule.SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                newSchedule.KinKyuTblCdSeq = 0;
                newSchedule.Title = request.model.Text;
                newSchedule.YoteiSymd = request.model.StartDate.ToString().Substring(0, 10).Replace("/", string.Empty);
                newSchedule.YoteiStime = request.model.StartDate.ToString().Substring(11).Replace(":", string.Empty).Length == 5 ? request.model.StartDate.ToString().Substring(11).Replace(":", string.Empty).Insert(0, "0") : request.model.StartDate.ToString().Substring(11).Replace(":", string.Empty);
                newSchedule.YoteiEymd = request.model.EndDate.ToString().Substring(0, 10).Replace("/", string.Empty);
                newSchedule.YoteiEtime = request.model.EndDate.ToString().Substring(11).Replace(":", string.Empty).Length == 5 ? request.model.EndDate.ToString().Substring(11).Replace(":", string.Empty).Insert(0, "0") : request.model.EndDate.ToString().Substring(11).Replace(":", string.Empty);
                newSchedule.TukiLabKbn = scheduleLable;
                newSchedule.AllDayKbn = request.model.AllDay == true ? (byte)1 : (byte)0;
                newSchedule.KuriRule = request.model.RecurrenceRule;
                newSchedule.KuriReg = request.model.RecurrenceException;
                newSchedule.KuriEndYmd = endOfRecurrence;
                newSchedule.GaiKkKbn = (byte)request.model.isPublic;
                newSchedule.YoteiBiko = request.model.Description;
                newSchedule.YoteiShoKbn = (byte)1;
                newSchedule.ShoSyainCdSeq = (byte)0;
                newSchedule.ShoUpdYmd = "";
                newSchedule.ShoUpdTime = "";
                newSchedule.ShoRejBiko = "";
                newSchedule.SiyoKbn = (byte)1;
                newSchedule.UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty);
                newSchedule.UpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8).Replace(":", string.Empty);
                newSchedule.UpdSyainCd = Constants.SyainCdSeq;
                newSchedule.UpdPrgId = Constants.CompanyCdSeq.ToString();
                newSchedule.CalendarSeq = request.model.CalendarSeq;
                var StartDate = DateTime.ParseExact(newSchedule.YoteiSymd + newSchedule.YoteiStime, "yyyyMMddHHmmss", null);
                var EndDate = DateTime.ParseExact(newSchedule.YoteiEymd + newSchedule.YoteiEtime, "yyyyMMddHHmmss", null);
                if (request.model.StartDate == StartDate && request.model.EndDate == EndDate && bookedSchedule.Count > 0)
                {
                    UpdateStaffOfSchedule(request.model, bookedSchedule);
                }
                else if (bookedSchedule.Count > 0)
                {
                    ResetBookingSchedule(request.model);
                }
                if (request.model.Staffs != null && bookedSchedule.Count == 0)
                {
                    SaveStaffOfSchedule(request.model, newSchedule.YoteiSeq);
                }
                _dbContext.TkdSchYotei.Update(newSchedule);
                _dbContext.SaveChanges();
                return true;
            }
            private bool UpdateStaffOfSchedule(StaffScheduleData model, List<TkdSchYotKsya> bookedSchedule)
            {
                var deleteSchedules = bookedSchedule.Where(r => !model.Staffs.Any(y => y == r.SyainCdSeq));
                var addingStaffCdSeqs = model.Staffs.Where(st => !bookedSchedule.Any(b => b.SyainCdSeq == st));
                var schedules = _dbContext.TkdSchYotKsya.Where(y => y.YoteiSeq == model.scheduleId).ToList();
                foreach (var item in deleteSchedules)
                {
                    _dbContext.TkdSchYotKsya.Remove(item);
                }
                foreach (var item in addingStaffCdSeqs)
                {
                    TkdSchYotKsya tkdSchYotKsya = new TkdSchYotKsya()
                    {
                        YoteiSeq = model.scheduleId,
                        SyainCdSeq = item,
                        UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty),
                        UpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8).Replace(":", string.Empty),
                        UpdSyainCd = Constants.SyainCdSeq,
                        UpdPrgId = Constants.CompanyCdSeq.ToString()
                    };
                    _dbContext.TkdSchYotKsya.Add(tkdSchYotKsya);
                }
                return true;
            }
            private bool ResetBookingSchedule(StaffScheduleData model)
            {
                foreach (var item in model.Staffs)
                {
                    var resetSchedule = _dbContext.TkdSchYotKsyaFb.Where(r => r.SyainCdSeq == item && r.YoteiSeq == model.scheduleId).FirstOrDefault();
                    resetSchedule.YoteiSymd = model.StartDate.ToString().Substring(0, 10).Replace("/", string.Empty);
                    resetSchedule.YoteiStime = model.StartDate.ToString().Substring(11).Replace(":", string.Empty);
                    resetSchedule.AcceptKbn = 0;
                    resetSchedule.UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty);
                    resetSchedule.UpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8).Replace(":", string.Empty);

                    _dbContext.TkdSchYotKsyaFb.Update(resetSchedule);
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
                    if (!_dbContext.TkdSchYotKsya.Where(x => x.YoteiSeq == tkdSchYotKsya.YoteiSeq && x.SyainCdSeq == tkdSchYotKsya.SyainCdSeq).Any())
                    {
                        _dbContext.TkdSchYotKsya.Add(tkdSchYotKsya);
                    }
                    _dbContext.SaveChanges();
                }
                return true;
            }
        
        }
    }
}
