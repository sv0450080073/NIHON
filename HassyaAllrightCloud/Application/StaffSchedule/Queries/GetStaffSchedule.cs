using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StaffSchedule.Queries
{
    public class GetStaffSchedule : IRequest<IEnumerable<StaffScheduleData>>
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public class Handler : IRequestHandler<GetStaffSchedule, IEnumerable<StaffScheduleData>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<IEnumerable<StaffScheduleData>> Handle(GetStaffSchedule request, CancellationToken cancellationToken)
            {
                var codeKbn = _dbContext.VpmCodeKb.Where(x => x.CodeSyu == FormatString.yoteiType).ToList();
                List<StaffScheduleData> Result = new List<StaffScheduleData>();
                if (request.UserId == 0)
                {
                    // 配車データ取得
                    List<LoadDispatchSchedule> DispatchSchedulesInfo = await new DispatchScheduleService(_dbContext).Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                    foreach (LoadDispatchSchedule DispatchScheduleInfo in DispatchSchedulesInfo)
                    {
                        Result.Add(new StaffScheduleData()
                        {
                            ScheduleYoteiType = "0",
                            ScheduleType = 0,
                            VacationType = 0,
                            ScheduleLabel = new List<int>(),
                            Staffs = new List<int>(),
                            Text = DispatchScheduleInfo.Destination,
                            AllDay = false,
                            StartDate = DispatchScheduleInfo.StockOut,
                            EndDate = DispatchScheduleInfo.Arrival,
                            Description = "",
                            RecurrenceRule = "",
                            RecurrenceException = null,
                            IsEditable = false,
                            status = ScheduleYoteiType.Trainning,
                            color = "#FEEDDB",
                            Destination = DispatchScheduleInfo.Destination
                        }); ;
                    }

                    // 勤務休日データ取得
                    List<LoadLeaveDay> LeaveDaysInfo = await new LeaveDayService(_dbContext).Get(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                    foreach (LoadLeaveDay LeaveDayInfo in LeaveDaysInfo)
                    {
                        Result.Add(new StaffScheduleData()
                        {
                            ScheduleYoteiType = "1",
                            ScheduleType = LeaveDayInfo.IsLeave == true ? 1 : 2,
                            VacationType = LeaveDayInfo.Type,
                            ScheduleLabel = new List<int>(),
                            Staffs = new List<int>(),
                            Text = LeaveDayInfo.TypeName,
                            AllDay = false,
                            StartDate = LeaveDayInfo.Start,
                            EndDate = LeaveDayInfo.End,
                            Description = "",
                            RecurrenceRule = "",
                            RecurrenceException = null,
                            IsEditable = false,
                            color = LeaveDayInfo.IsLeave == true ? "#FFCDD2" : "#E1E9DD",
                            status = LeaveDayInfo.IsLeave == true ? StaffScheduleConstants.IsLeaving : StaffScheduleConstants.IsWorking,
                            EmployeeName = LeaveDayInfo.EmployeeName,
                            Remark = LeaveDayInfo.Remark
                        });
                    }

                    // 編集不可スケジュールデータ取得
                    List<LoadStaffSchedule> StaffSchedulesUnEditable = await GetStaffScheduleUnEditable(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                    int CurrentSeq = -1;
                    foreach (LoadStaffSchedule StaffScheduleUnEditable in StaffSchedulesUnEditable)
                    {
                        var codeKb = codeKbn.Where(x => x.CodeKbnSeq == StaffScheduleUnEditable.ScheduleType).FirstOrDefault() != null ? Int32.Parse(codeKbn.Where(x => x.CodeKbnSeq == StaffScheduleUnEditable.ScheduleType).FirstOrDefault().CodeKbn) : 0;
                        if (StaffScheduleUnEditable.ScheduleSeq != CurrentSeq)
                        {
                            Result.Add(new StaffScheduleData()
                            {
                                ScheduleYoteiType = "2",
                                ScheduleType = codeKb,
                                VacationType = StaffScheduleUnEditable.VacationType,
                                ScheduleLabel = string.IsNullOrEmpty(StaffScheduleUnEditable.ScheduleLabel) ?
                                    new List<int>() : StaffScheduleUnEditable.ScheduleLabel.Split(',').Select(int.Parse).ToList(),
                                Staffs = new List<int> { StaffScheduleUnEditable.CreateStaff },
                                Text = StaffScheduleUnEditable.Text,
                                AllDay = StaffScheduleUnEditable.AllDay,
                                StartDate = StaffScheduleUnEditable.StartDate,
                                EndDate = StaffScheduleUnEditable.EndDate,
                                Description = StaffScheduleUnEditable.Description,
                                RecurrenceRule = StaffScheduleUnEditable.RecurrenceRule,
                                RecurrenceException = StaffScheduleUnEditable.RecurrenceException,
                                IsEditable = false,
                                scheduleId = StaffScheduleUnEditable.scheduleId,
                                isPublic = StaffScheduleUnEditable.isPublic,
                                status =  codeKb == 1 ? ScheduleYoteiType.ScheduleLeave : codeKb == 2 ? ScheduleYoteiType.ScheduleMeeting : ScheduleYoteiType.ScheduleJourney,
                                color = codeKb == 1 ? "#FDDFDD" : codeKb == 2 ? "#F6DFF0" : "#DBF0EE"
                            });
                            CurrentSeq = StaffScheduleUnEditable.ScheduleSeq;
                        }
                        if (StaffScheduleUnEditable.Staff != 0)
                        {
                            Result[Result.Count() - 1].Staffs.Add(StaffScheduleUnEditable.Staff);
                        }
                    }

                    // 編集可スケジュールデータ取得

                    List<LoadStaffSchedule> StaffSchedulesEditable = await GetStaffScheduleEditable(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                    CurrentSeq = -1;
                    foreach (LoadStaffSchedule StaffScheduleEditable in StaffSchedulesEditable)
                    {
                        var codeKb = codeKbn.Where(x => x.CodeKbnSeq == StaffScheduleEditable.ScheduleType).FirstOrDefault() != null ? Int32.Parse(codeKbn.Where(x => x.CodeKbnSeq == StaffScheduleEditable.ScheduleType).FirstOrDefault().CodeKbn) : 0;
                        if (StaffScheduleEditable.ScheduleSeq != CurrentSeq)
                        {
                            Result.Add(new StaffScheduleData()
                            {
                                ScheduleYoteiType = "2",
                                ScheduleType = codeKb,
                                VacationType = StaffScheduleEditable.VacationType,
                                ScheduleLabel = string.IsNullOrEmpty(StaffScheduleEditable.ScheduleLabel) ?
                                    new List<int>() : StaffScheduleEditable.ScheduleLabel.Split(',').Select(int.Parse).ToList(),
                                Staffs = new List<int> {},
                                Text = StaffScheduleEditable.Text,
                                AllDay = StaffScheduleEditable.AllDay,
                                StartDate = StaffScheduleEditable.StartDate,
                                EndDate = StaffScheduleEditable.EndDate,
                                Description = StaffScheduleEditable.Description,
                                RecurrenceRule = StaffScheduleEditable.RecurrenceRule,
                                RecurrenceException = StaffScheduleEditable.RecurrenceException,
                                IsEditable = true,
                                scheduleId = StaffScheduleEditable.scheduleId,
                                isPublic = StaffScheduleEditable.isPublic,
                                status = codeKb == 1 ? ScheduleYoteiType.ScheduleLeave : codeKb == 2 ? ScheduleYoteiType.ScheduleMeeting : ScheduleYoteiType.ScheduleJourney,
                                color = codeKb == 1 ? "#FDDFDD" : codeKb == 2 ? "#F6DFF0" : "#DBF0EE",
                                YoteiShoKbn = StaffScheduleEditable.YoteiShoKbn,
                                RemarkApprover = StaffScheduleEditable.RemarkApprover,
                                Authorizer = StaffScheduleEditable.Authorizer,
                                DateAppover = DateDisplayValue(StaffScheduleEditable.DateAppover),
                                TimeAppover = StaffScheduleEditable.TimeAppover,
                                IsSendNoti = true,
                                StaffIdToSend = 0
                            });
                            CurrentSeq = StaffScheduleEditable.ScheduleSeq;
                        }
                        if (StaffScheduleEditable.Staff != 0)
                        {
                            Result[Result.Count() - 1].Staffs.Add(StaffScheduleEditable.Staff);
                        }
                    }
                }
                return Result;
            }
            private async Task<List<LoadStaffSchedule>> GetStaffScheduleUnEditable(int UserId)
            {
                return await (
                        from yoteiKSya1 in _dbContext.TkdSchYotKsya
                        join yotei in _dbContext.TkdSchYotei
                            on yoteiKSya1.YoteiSeq equals yotei.YoteiSeq
                        join yoteiKSya2 in _dbContext.TkdSchYotKsya
                            on yotei.YoteiSeq equals yoteiKSya2.YoteiSeq into yotei_join
                        from yoteiKsyaAll in yotei_join.DefaultIfEmpty()
                        where yotei.SiyoKbn == 1
                            && yotei.KinKyuTblCdSeq == 0
                            && yoteiKSya1.SyainCdSeq == UserId

                        select new LoadStaffSchedule()
                        {
                            ScheduleSeq = yotei.YoteiSeq,
                            ScheduleType = yotei.YoteiType,
                            VacationType = yotei.KinKyuCdSeq,
                            ScheduleLabel = yotei.TukiLabKbn,
                            Staff = yoteiKsyaAll.SyainCdSeq,
                            CreateStaff = yotei.SyainCdSeq,
                            Text = yotei.Title,
                            AllDay = yotei.AllDayKbn == 1,
                            StartDate = DateTime.ParseExact(yotei.YoteiSymd + yotei.YoteiStime, "yyyyMMddHHmmss", null),
                            EndDate = DateTime.ParseExact(yotei.YoteiEymd + yotei.YoteiEtime, "yyyyMMddHHmmss", null),
                            Description = yotei.YoteiBiko,
                            RecurrenceRule = yotei.KuriRule,
                            RecurrenceException = yotei.KuriReg,
                            isPublic = Int32.Parse(yotei.GaiKkKbn.ToString()),
                            scheduleId = yotei.YoteiSeq
                        }).ToListAsync();
            }
            private async Task<List<LoadStaffSchedule>> GetStaffScheduleEditable(int UserId)
            {
                return await (
                        from yotei in _dbContext.TkdSchYotei
                        join yoteiKSya in _dbContext.TkdSchYotKsya
                            on yotei.YoteiSeq equals yoteiKSya.YoteiSeq into yotei_join
                        from yoteiKsyaAll in yotei_join.DefaultIfEmpty()
                        join vpmsyain in _dbContext.VpmSyain
                           on yotei.ShoSyainCdSeq equals vpmsyain.SyainCdSeq into syain_join
                        from vpmsyain01 in syain_join.DefaultIfEmpty()
                        where yotei.SiyoKbn == 1
                            && yotei.SyainCdSeq == UserId

                        select new LoadStaffSchedule()
                        {
                            ScheduleSeq = yotei.YoteiSeq,
                            ScheduleType = yotei.YoteiType,
                            VacationType = yotei.KinKyuCdSeq,
                            ScheduleLabel = yotei.TukiLabKbn,
                            Staff = yoteiKsyaAll.SyainCdSeq,
                            Text = yotei.Title,
                            AllDay = yotei.AllDayKbn == 1,
                            StartDate = DateTime.ParseExact(yotei.YoteiSymd + yotei.YoteiStime, "yyyyMMddHHmmss", null),
                            EndDate = DateTime.ParseExact(yotei.YoteiEymd + yotei.YoteiEtime, "yyyyMMddHHmmss", null),
                            Description = yotei.YoteiBiko,
                            RecurrenceRule = yotei.KuriRule,
                            RecurrenceException = yotei.KuriReg,
                            isPublic = Int32.Parse(yotei.GaiKkKbn.ToString()),
                            scheduleId = yotei.YoteiSeq,
                            YoteiShoKbn = yotei.YoteiShoKbn,
                            RemarkApprover = yotei.ShoRejBiko,
                            Authorizer = vpmsyain01.SyainNm,
                            TimeAppover = yotei.ShoUpdTime,
                            DateAppover = yotei.ShoUpdYmd,

                        }).ToListAsync();

            }

            public DateTime? DateDisplayValue(string Ymd)
            {
                DateTime DateValue;
                string DateFormat = "yyyyMMdd";
                if (!DateTime.TryParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateValue))
                {
                    return null;
                }
                else
                {
                    return DateTime.ParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture);
                }
            }
        }
    }
}
