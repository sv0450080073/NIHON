using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LeaveApplicationManagement.Queries
{
    public class GetScheduleDataGrid : IRequest<(IEnumerable<ScheduleManageGridData>, int)>
    {
        public ScheduleManageForm searchModel { get; set; }
        public class Handler : IRequestHandler<GetScheduleDataGrid, (IEnumerable<ScheduleManageGridData>, int)>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<(IEnumerable<ScheduleManageGridData>, int)> Handle(GetScheduleDataGrid request, CancellationToken cancellationToken)
            {
                var result = new List<ScheduleManageGridData>();
                var scheduleData = new List<ScheduleManageData>();
                var yoteiShoKbn = request.searchModel.ApprovalStatus != null ? ((request.searchModel.ApprovalStatus.status.Equals(StaffScheduleConstants.Pending) ? "1" : request.searchModel.ApprovalStatus.status.Equals(StaffScheduleConstants.Accept) ? "2" : "3" )) : string.Empty;
                _dbContext.LoadStoredProc("PK_dSchedule_R")
                          .AddParam("TenantCdSeq", new ClaimModel().TenantID)
                          .AddParam("EigyoCdSeq", request.searchModel.Branch != null ? request.searchModel.Branch.Seg : 0)
                          .AddParam("YoteiSYmd", request.searchModel.StartDate != null ? request.searchModel.StartDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("YoteiEYmd", request.searchModel.EndDate != null ? request.searchModel.EndDate.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty)
                          .AddParam("SyainCdSeq", request.searchModel.Staff != null ? request.searchModel.Staff.Seg : 0)
                          .AddParam("YoteiShoKbn", yoteiShoKbn)
                          .AddParam("CusGrpSeq", request.searchModel.CustomGroup != null ? request.searchModel.CustomGroup.CusGrpSeq : 0)
                          .AddParam("Take", request.searchModel.PageSize)
                          .AddParam("Skip", request.searchModel.PageNum * request.searchModel.PageSize)

                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                                    .Exec(r => scheduleData = r.ToList<ScheduleManageData>());
                int count = rowCount.Value;
                int i = request.searchModel.PageNum * request.searchModel.PageSize;
                var culture = new System.Globalization.CultureInfo("ja-JP");
                foreach (var item in scheduleData)
                {
                    var sDayOfWeek = culture.DateTimeFormat.GetDayName(DateTime.ParseExact(item.YoteiSYmd, "yyyyMMdd", null).DayOfWeek).Substring(0,1);
                    var eDayOfWeek = culture.DateTimeFormat.GetDayName(DateTime.ParseExact(item.YoteiEYmd, "yyyyMMdd", null).DayOfWeek).Substring(0,1);                    
                    result.Add(new ScheduleManageGridData()
                    {
                        No = i + 1,
                        EigyoNm = item.EigyoNm,
                        KinkyuNm = item.KinkyuNm,
                        ShoSyainCd = item.ShoSyainCd,
                        ShoSyainNm = item.ShoSyainNm,
                        SyainCd = item.SyainCd,
                        SyainNm = item.SyainNm,
                        Title = item.Title,
                        TukiLabKbn = item.TukiLabKbn.Equals("1") ? StaffScheduleConstants.Absolute : item.TukiLabKbn.Equals("2") ? StaffScheduleConstants.Hope : item.TukiLabKbn.Equals("1,2") ? StaffScheduleConstants.Absolute+","+StaffScheduleConstants.Hope : item.TukiLabKbn.Equals("2,1") ? StaffScheduleConstants.Hope + "," + StaffScheduleConstants.Absolute : string.Empty,
                        YoteiShoKbn = item.YoteiShoKbn == 1 ? StaffScheduleConstants.PendingSt : item.YoteiShoKbn == 2 ? StaffScheduleConstants.AcceptSt : StaffScheduleConstants.RefusetSt,
                        YoteiTypeNm = item.YoteiTypeNm,
                        StartDate = item.AllDayKbn == 1 ? (item.YoteiSYmd.Insert(4, "/").Insert(7, "/") + " (" + sDayOfWeek + ") " + StaffScheduleConstants.Day) : (item.YoteiSYmd.Insert(4, "/").Insert(7, "/") + " (" + sDayOfWeek + ") " + " " + item.YoteiSTime.Insert(2, ":").Insert(5, ":").Substring(0, 5)),
                        EndDate = item.AllDayKbn == 1 ? (item.YoteiEYmd.Insert(4, "/").Insert(7, "/") + " (" + eDayOfWeek + ") " + StaffScheduleConstants.Day) : (item.YoteiEYmd.Insert(4, "/").Insert(7, "/") + " (" + eDayOfWeek + ") " + " " + item.YoteiETime.Insert(2, ":").Insert(5, ":").Substring(0, 5)),
                        UpdateTime = item.ShoUpdYmd.TrimEnd().Length < 8 ? string.Empty : item.ShoUpdYmd.Insert(4, "/").Insert(7, "/") +" "+ item.ShoUpdTime.Substring(0,4).Insert(2, ":"),
                        ScheduleId = item.YoteiSeq
                    });
                    i++;
                }

                return (result, count);
            }
        }
    }
}
