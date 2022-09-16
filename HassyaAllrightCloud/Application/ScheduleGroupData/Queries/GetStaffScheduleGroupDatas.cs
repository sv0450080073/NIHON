using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ScheduleGroupData.Queries
{
    public class GetStaffScheduleGroupDatas : IRequest<IEnumerable<StaffScheduleGroupData>>
    {
        public int groupId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public bool isCustomGr { get; set; }

        public class Handler : IRequestHandler<GetStaffScheduleGroupDatas, IEnumerable<StaffScheduleGroupData>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<IEnumerable<StaffScheduleGroupData>> Handle(GetStaffScheduleGroupDatas request, CancellationToken cancellationToken)
            {
                List<StaffScheduleGroupData> result = new List<StaffScheduleGroupData>();
                List<PlanData> planDatas = new List<PlanData>();
                List<JourneyData> journeyDatas = new List<JourneyData>();
                List<LeaveData> leaveDatas = new List<LeaveData>();
                List<StaffsData> staffDatas = new List<StaffsData>();
                if (!request.isCustomGr)
                {
                    planDatas = GetPlanDataByGroup(request.groupId, request.fromDate, request.toDate);
                    journeyDatas = GetJourneysDataByGroup(request.groupId, request.fromDate, request.toDate);
                    leaveDatas = GetLeaveDayByGroup(request.groupId, new ClaimModel().TenantID, request.fromDate, request.toDate);
                    staffDatas = GetStaffByGroup(request.groupId);
                }
                else
                {
                    planDatas = GetPlanDataByGroupCustom(request.groupId, request.fromDate, request.toDate);
                    journeyDatas = GetJourneysDataByGroupCustom(request.groupId, request.fromDate, request.toDate);
                    leaveDatas = GetLeaveDayByGroupCustom(request.groupId, new ClaimModel().TenantID, request.fromDate, request.toDate);
                    staffDatas = GetStaffByGroupCustom(request.groupId);
                }
                foreach (var leave in leaveDatas)
                {
                    result.Add(new StaffScheduleGroupData()
                    {
                        startDate = DateTime.ParseExact(leave.KinKyuSYmd + leave.KinKyuSTime, "yyyyMMddHHmm", null),
                        endDate = DateTime.ParseExact(leave.KinKyuEYmd + leave.KinKyuETime, "yyyyMMddHHmm", null),
                        Description = leave.Title,
                        EmployeeId = leave.SyainCd,
                        EmployeeName = leave.SyainNm,
                        ScheduleType = "leave",
                        LeaveType = leave.KinKyuKbn.ToString(),
                        color = leave.KinKyuKbn != null ? Int32.Parse(leave.KinKyuKbn.ToString()) == 2 ? "#FFCDD2" : "#E1E9DD" : "#5bc3f0",
                        PlanComment = leave.BikoNm,
                        LeaveName = leave.KinKyuNm
                    });
                }

                foreach (var plan in planDatas)
                {
                    var schedule = _dbContext.TkdSchYotei.Where(x => x.YoteiSeq == plan.YoteiSeq).FirstOrDefault();
                    var currentCodeKbn = _dbContext.VpmCodeKb.Where(x => x.CodeKbnSeq == schedule.YoteiType).FirstOrDefault()?.CodeKbn;
                    result.Add(new StaffScheduleGroupData()
                    {
                        EmployeeId = plan.SyainCd,
                        startDate = DateTime.ParseExact(plan.YoteiSYmd + plan.YoteiSTime, "yyyyMMddHHmmss", null),
                        endDate = DateTime.ParseExact(plan.YoteiEYmd + plan.YoteiETime, "yyyyMMddHHmmss", null),
                        EmployeeName = plan.SyainNm,
                        Description = plan.Title,
                        ScheduleType = "plan",
                        Creator = plan.SyainNm,
                        Participant = plan.EventParticipant,
                        PlanComment = plan.YoteiBiko,
                        IsPublic = plan.GaiKkKbn.ToString(),
                        color = plan.GaiKkKbn == (byte)1 ? currentCodeKbn == "1" ? "#FDDFDD" : currentCodeKbn == "2" ? "#F6DFF0" : "#DBF0EE" : "#E0E0E0",
                        scheduleId = plan.YoteiSeq.ToString(),
                        recurrenceRule = plan.KuriRule,
                        label = plan.TukiLabKbn,
                        plantype = currentCodeKbn,
                        isAllDay = plan.AllDayKbn == 1 ? true : false
                    });
                }
                
                foreach (var jouney in journeyDatas)
                {
                    result.Add(new StaffScheduleGroupData()
                    {
                        EmployeeId = staffDatas.Where(x => x.SyainNm == jouney.SyainNm).FirstOrDefault()?.SyainCd,
                        startDate = DateTime.ParseExact(jouney.SyuKoYmd + jouney.SyuKoTime, "yyyyMMddHHmm", null),
                        endDate = DateTime.ParseExact(jouney.KikYmd + jouney.KikTime, "yyyyMMddHHmm", null),
                        EmployeeName = jouney.SyainNm,
                        Description = jouney.Title,
                        ScheduleType = "journey",
                        color = "#FEEDDB",
                        Destination = jouney.IkNm
                    });
                }
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].id = (i + 1).ToString();
                }
                return result;
            }

            private List<LeaveData> GetLeaveDayByGroup(int groupId, int tenantId, string fromDate, string toDate)
            {
                List<LeaveData> rows = null;
                _dbContext.LoadStoredProc("PK_dLeave_R")
                    .AddParam("TenantCdSeq", tenantId)
                    .AddParam("GroupId", groupId)
                    .AddParam("FromDate", fromDate)
                    .AddParam("ToDate", toDate)
                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                    .Exec(r => rows = r.ToList<LeaveData>());
                return rows;
            }
            private List<StaffsData> GetStaffByGroup(int groupId)
            {
                string DateAsString = DateTime.Today.ToString("yyyyMMdd");
                return (from syain in _dbContext.VpmSyain
                        from kyoshe in _dbContext.VpmKyoShe
                        where syain.SyainCdSeq == kyoshe.SyainCdSeq && DateAsString.CompareTo(kyoshe.StaYmd) >= 0 && DateAsString.CompareTo(kyoshe.EndYmd) <= 0
                        && kyoshe.EigyoCdSeq == groupId
                        orderby syain.SyainCd, syain.SyainCdSeq
                        select new StaffsData(syain)).ToList();
            }
            private List<PlanData> GetPlanDataByGroup(int groupId, string fromDate, string toDate)
            {
                List<PlanData> rows = null;
                _dbContext.LoadStoredProc("PK_dPlan_R")
                    .AddParam("GroupId", groupId)
                    .AddParam("FromDate", fromDate)
                    .AddParam("ToDate", toDate)
                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                    .Exec(r => rows = r.ToList<PlanData>());
                return rows;

            }
            private List<JourneyData> GetJourneysDataByGroup(int groupId, string fromDate, string toDate)
            {
                List<JourneyData> rows = null;
                _dbContext.LoadStoredProc("PK_dJourneys_R")
                    .AddParam("GroupId", groupId)
                    .AddParam("FromDate", fromDate)
                    .AddParam("ToDate", toDate)
                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                    .Exec(r => rows = r.ToList<JourneyData>());
                return rows;
            }

            private List<LeaveData> GetLeaveDayByGroupCustom(int groupId, int tenantId, string fromDate, string toDate)
            {
                List<LeaveData> rows = null;
                _dbContext.LoadStoredProc("PK_dLeaveCustomGr_R")
                    .AddParam("TenantCdSeq", tenantId)
                    .AddParam("GroupId", groupId)
                    .AddParam("FromDate", fromDate)
                    .AddParam("ToDate", toDate)
                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                    .Exec(r => rows = r.ToList<LeaveData>());
                return rows;
            }

            private List<JourneyData> GetJourneysDataByGroupCustom(int groupId, string fromDate, string toDate)
            {
                List<JourneyData> rows = null;
                _dbContext.LoadStoredProc("PK_dJourneysCustomGr_R")
                    .AddParam("GroupId", groupId)
                    .AddParam("FromDate", fromDate)
                    .AddParam("ToDate", toDate)
                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                    .Exec(r => rows = r.ToList<JourneyData>());
                return rows;
            }

            private List<PlanData> GetPlanDataByGroupCustom(int groupId, string fromDate, string toDate)
            {
                List<PlanData> rows = null;
                _dbContext.LoadStoredProc("PK_dPlanCustomGr_R")
                    .AddParam("GroupId", groupId)
                    .AddParam("FromDate", fromDate)
                    .AddParam("ToDate", toDate)
                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                    .Exec(r => rows = r.ToList<PlanData>());
                return rows;

            }

            private List<StaffsData> GetStaffByGroupCustom(int groupId)
            {
                var staffs = (from customgr in _dbContext.TkdSchCusGrpMem
                              join syain in _dbContext.VpmSyain
                              on customgr.SyainCdSeq equals syain.SyainCdSeq into cs
                              from csTemp in cs.DefaultIfEmpty()
                              where customgr.CusGrpSeq == groupId
                              select new StaffsData()
                              {
                                  SyainCd = csTemp.SyainCd,
                                  SyainNm = csTemp.SyainNm,
                                  SyainCdSeq = csTemp.SyainCdSeq
                              }).ToList();

                return staffs;
            }
        }
    }
}
