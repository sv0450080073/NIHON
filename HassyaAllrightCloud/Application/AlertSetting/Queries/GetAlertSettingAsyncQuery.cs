using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AlertSetting.Queries
{
    public class GetAlertSettingAsyncQuery : IRequest<List<Domain.Dto.AlertSetting>>
    {
        public List<int> alertCds { get; set; } = new List<int>();
        public int tenantCdSeq { get; set; }
        public int syainCdSeq { get; set; }
        public int companyCdSeq { get; set; }

        public class Handler : IRequestHandler<GetAlertSettingAsyncQuery, List<Domain.Dto.AlertSetting>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<Domain.Dto.AlertSetting>> Handle(GetAlertSettingAsyncQuery request, CancellationToken cancellationToken = default)
            {
                List<Domain.Dto.AlertSetting> rows = new List<Domain.Dto.AlertSetting>();
                if(request.alertCds == null || !request.alertCds.Any())
                    return rows;

                var alerts1 = new int[] { 1001, 1002, 2001, 2002, 2006, 1003, 2003, 1004, 2004, 1005, 2005 };

                foreach (var item in request.alertCds)
                {
                    if(alerts1.Any(x => x == item))
                        AddNotNullData(rows, GetData(request, item));

                    if (item == 2007)
                    {
                        var alerts = GetDataForCode_2007(request.tenantCdSeq, request.syainCdSeq);
                        if (alerts != null && alerts.Any())
                            rows.AddRange(alerts);
                    }
                }

                var alertSetting2008 = GetDataForCode_2008_2009_2010_2011(request.tenantCdSeq, request.syainCdSeq, 2008);
                if (alertSetting2008 != null)
                    rows.Add(alertSetting2008);
                var alertSetting2011 = GetDataForCode_2008_2009_2010_2011(request.tenantCdSeq, request.syainCdSeq, 2011);
                if(alertSetting2011 != null)
                {
                    rows.Add(alertSetting2011);
                    return rows;
                }

                var alertSetting2010 = GetDataForCode_2008_2009_2010_2011(request.tenantCdSeq, request.syainCdSeq, 2010);
                if (alertSetting2010 != null)
                {
                    rows.Add(alertSetting2010);
                    return rows;
                }

                var alertSetting2009 = GetDataForCode_2008_2009_2010_2011(request.tenantCdSeq, request.syainCdSeq, 2009);
                if (alertSetting2009 != null)
                    rows.Add(alertSetting2009);

                return rows;
            }

            public List<Domain.Dto.AlertSetting> GetData(GetAlertSettingAsyncQuery request, int alertCd) {
                AlertSettingTimeLine alertSettingTimeLine = null;
                List<AlertSettingTimeLine> alertSettingTimeLines = new List<AlertSettingTimeLine>();

                _context.LoadStoredProc("PK_dGetAlertTimeline_R")
                                .AddParam("@TenantCdSeq", request.tenantCdSeq)
                                .AddParam("@SyainCdSeq", request.syainCdSeq)
                                .AddParam("@AlertCd", alertCd)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount32)
                                .Exec(r => { 
                                    if(r.Read())
                                    {
                                        alertSettingTimeLine = new AlertSettingTimeLine();
                                        alertSettingTimeLine.TenantCdSeq = string.IsNullOrWhiteSpace((string)r["TenantCdSeq"].ToString()) ? 0 : (int)r["TenantCdSeq"];
                                        alertSettingTimeLine.AlertCdSeq = string.IsNullOrWhiteSpace((string)r["AlertCdSeq"].ToString()) ? 0 : (int)r["AlertCdSeq"];
                                        alertSettingTimeLine.AlertKbn = string.IsNullOrWhiteSpace((string)r["AlertKbn"].ToString()) ? (short)0 : (short)r["AlertKbn"];
                                        alertSettingTimeLine.AlertCd = string.IsNullOrWhiteSpace((string)r["AlertCd"].ToString()) ? 0 : (int)r["AlertCd"];
                                        alertSettingTimeLine.AlertNm = (string)r["AlertNm"];
                                        alertSettingTimeLine.DefaultVal = string.IsNullOrWhiteSpace((string)r["DefaultVal"].ToString()) ? 0 : (int)r["DefaultVal"];
                                        alertSettingTimeLine.DefaultTimeline = string.IsNullOrWhiteSpace((string)r["DefaultTimeline"].ToString()) ? (byte)0 : (byte)r["DefaultTimeline"];
                                        alertSettingTimeLine.DefaultZengo = string.IsNullOrWhiteSpace((string)r["DefaultZengo"].ToString()) ? (byte)0 : (byte)r["DefaultZengo"];
                                        alertSettingTimeLine.DefaultDisplayKbn = string.IsNullOrWhiteSpace((string)r["DefaultDisplayKbn"].ToString()) ? (byte)0 : (byte)r["DefaultDisplayKbn"];
                                        alertSettingTimeLine.CurTenantCdSeq = string.IsNullOrWhiteSpace((string)r["CurTenantCdSeq"].ToString()) ? 0 : (int)r["CurTenantCdSeq"];
                                        alertSettingTimeLine.CurVal = string.IsNullOrWhiteSpace((string)r["CurVal"].ToString()) ? 0 : (int)r["CurVal"];
                                        alertSettingTimeLine.CurTimeline = string.IsNullOrWhiteSpace((string)r["CurTimeline"].ToString()) ?(byte) 0 : (byte)r["CurTimeline"];
                                        alertSettingTimeLine.CurZengo = string.IsNullOrWhiteSpace((string)r["CurZengo"].ToString()) ? (byte)0 : (byte)r["CurZengo"];
                                        alertSettingTimeLine.CurDisplayKbn = string.IsNullOrWhiteSpace((string)r["CurDisplayKbn"].ToString()) ? (byte)0 : (byte)r["CurDisplayKbn"];
                                        alertSettingTimeLine.SyainCdSeq = string.IsNullOrWhiteSpace((string)r["SyainCdSeq"].ToString()) ? 0 : (int)r["SyainCdSeq"];
                                        alertSettingTimeLine.UserDisplayKbn = string.IsNullOrWhiteSpace((string)r["UserDisplayKbn"].ToString()) ? (byte)0 : (byte)r["UserDisplayKbn"];
                                    }
                                    r.Close();
                                });

                if (alertSettingTimeLine == null || alertSettingTimeLine.CurDisplayKbn == 2)
                    return null;

                if((alertSettingTimeLine.CurDisplayKbn == 1 && alertSettingTimeLine.UserDisplayKbn == 1) ||
                    (alertSettingTimeLine.CurDisplayKbn == 0 && alertSettingTimeLine.DefaultDisplayKbn == 1
                    && alertSettingTimeLine.UserDisplayKbn == 1))
                {
                    string seiTaiYmd = ConvertTimeLineToTimeString(alertSettingTimeLine.DefaultVal, 
                        alertSettingTimeLine.DefaultTimeline, alertSettingTimeLine.DefaultZengo);
                    switch(alertCd)
                    {
                        case 1001:
                        case 1002:
                        case 2001:
                        case 2002:
                        case 2006:
                            return GetDataForCode_1001_1002_2001_2002_2006(request.tenantCdSeq, seiTaiYmd, alertCd);
                        case 1003:
                        case 2003:
                            return GetDataForCode_1003_2003(request.tenantCdSeq, seiTaiYmd);
                        case 1004:
                        case 1005:
                        case 2004:
                        case 2005:
                            return GetDataForCode_1004_1005_2004_2005(request.tenantCdSeq, request.companyCdSeq, seiTaiYmd, alertCd);
                    }
                }

                return null;
            }

            public List<Domain.Dto.AlertSetting> GetDataForCode_1001_1002_2001_2002_2006(int tenantCdSeq, string seiTaiYmd, int alertCd)
            {
                int count = 0;
                _context.LoadStoredProc("PK_dGetNumberAlertFromReservationData_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@SeiTaiYmd", seiTaiYmd)
                                .AddParam("@AlertNumber", alertCd)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount33)
                                .Exec(r => {
                                    if (r.Read())
                                    {
                                        count = (int)r["CountNumber"];
                                    }
                                    r.Close();
                                });
                if (count == 0)
                    return null;

                var alertSetting = new Domain.Dto.AlertSetting()
                {
                    AlertName = (alertCd == 1001) ? "未確定" : (alertCd == 1002) ? "未仮車" 
                    : (alertCd == 2001) ? "未配車" : (alertCd == 2002) ? "未配員" 
                    : (alertCd == 2006) ? "車両別日報　未登録" : string.Empty,
                    Date = DateTime.ParseExact(seiTaiYmd, "yyyyMMdd",
                    CultureInfo.InvariantCulture).ToString("MM月dd日", new CultureInfo("ja-JP")),
                    Link = (alertCd == 2001) ? "leaveapplicationmanagement" : "busschedule",
                    Number = count.ToString()
                };
                return new List<Domain.Dto.AlertSetting>() { alertSetting } ;
            }

            public List<Domain.Dto.AlertSetting> GetDataForCode_1003_2003(int tenantCdSeq, string seiTaiYmd)
            {
                string staYmd = null; string endYmd = null; string today = DateTime.Today.ToString("yyyyMMdd");
                List<AlertSettingForCode36> alertSettingForCode36s = new List<AlertSettingForCode36>();
                List<AlertSettingForCode37> alertSettingForCode37s = new List<AlertSettingForCode37>();
                List<AlertSettingForCode38> alertSettingForCode38s = new List<AlertSettingForCode38>();

                List<Domain.Dto.AlertSetting> alertSettings = new List<Domain.Dto.AlertSetting>();

                staYmd = (int.Parse(today) <= int.Parse(seiTaiYmd)) ? today : seiTaiYmd;
                endYmd = (int.Parse(today) <= int.Parse(seiTaiYmd)) ? seiTaiYmd : today;
                _context.LoadStoredProc("PK_dAcquisitionOfVehiclesAndSalesOffices_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@StaYmd", staYmd)
                                .AddParam("@EndYmd", endYmd)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount36)
                                .Exec(r => alertSettingForCode36s = r.ToList<AlertSettingForCode36>());
                var sharyoSeqList = string.Join(",", alertSettingForCode36s.Select(x => x.SyaRyoCdSeq));

                _context.LoadStoredProc("PK_dAcquisitionProcessOfReservedVehicleInformation_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@StaYmd", staYmd)
                                .AddParam("@EndYmd", endYmd)
                                .AddParam("@SharyoSeqList", sharyoSeqList)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount37)
                                .Exec(r => alertSettingForCode37s = r.ToList<AlertSettingForCode37>());

                _context.LoadStoredProc("PK_dAcquisitionProcessOfVehicleRepairInformation_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@StaYmd", staYmd)
                                .AddParam("@EndYmd", endYmd)
                                .AddParam("@SharyoSeqList", sharyoSeqList)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount38)
                                .Exec(r => alertSettingForCode38s = r.ToList<AlertSettingForCode38>());

                var StartDate = DateTime.ParseExact(staYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                var EndDate = DateTime.ParseExact(endYmd, "yyyyMMdd", CultureInfo.InvariantCulture);

                foreach (DateTime day in EachDay((DateTime)StartDate, (DateTime)EndDate)) {
                    var dateTimeYmd = day.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                    var allUnits = alertSettingForCode36s.Where(x => string.Compare(x.StaYmd, dateTimeYmd) <= 0 && string.Compare(x.EndYmd, dateTimeYmd) >= 0).Count();
                    var numberOfRideHailing = alertSettingForCode37s.Where(x => string.Compare(x.SyuKoYmd, dateTimeYmd) <= 0 && string.Compare(x.KikYmd, dateTimeYmd) >= 0).Count();
                    var numberOfRepairData = alertSettingForCode38s.Where(x => string.Compare(x.ShuriSYmd, dateTimeYmd) <= 0 && string.Compare(x.ShuriEYmd, dateTimeYmd) >= 0).Count();

                    if ((allUnits - numberOfRideHailing - numberOfRepairData) != 0)
                        return null;

                    var alertSetting = new Domain.Dto.AlertSetting()
                    {
                        AlertName = "空台数０未満",
                        Date = day.ToString("MM月dd日", new CultureInfo("ja-JP")),
                        Link = "busschedule",
                        Number = string.Empty
                    };
                    alertSettings.Add(alertSetting);
                }
                return alertSettings;
            }

            public List<Domain.Dto.AlertSetting> GetDataForCode_1004_1005_2004_2005(int tenantCdSeq, int companyCdSeq, string seiTaiYmd, int alertCd)
            {
                string staYmd = null; string endYmd = null; string today = DateTime.Today.ToString("yyyyMMdd");
                List<AlertSettingForCode39> alertSettingForCode39s = new List<AlertSettingForCode39>();
                List<AlertSettingForCode310> alertSettingForCode310s = new List<AlertSettingForCode310>();
                List<Domain.Dto.AlertSetting> alertSettings = new List<Domain.Dto.AlertSetting>();

                staYmd = (int.Parse(today) <= int.Parse(seiTaiYmd)) ? today : seiTaiYmd;
                endYmd = (int.Parse(today) <= int.Parse(seiTaiYmd)) ? seiTaiYmd : today;
                _context.LoadStoredProc("PK_dAcquisitionProcessOfEmployeeInformation_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@StaYmd", staYmd)
                                .AddParam("@EndYmd", endYmd)
                                .AddParam("@SyokumuKbnList", (alertCd == 1004 || alertCd == 2004) ? "1,2" : "3,4")
                                .AddParam("@CompanyCdSeq", companyCdSeq)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount39)
                                .Exec(r => alertSettingForCode39s = r.ToList<AlertSettingForCode39>());
                var syainSeqList = string.Join(",", alertSettingForCode39s.Select(x => x.SyainCdSeq));

                _context.LoadStoredProc("PK_dAcquisitionProcessOfWorkHolidayInformation_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@StaYmd", staYmd)
                                .AddParam("@EndYmd", endYmd)
                                .AddParam("@SyainSeqList", syainSeqList)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount310)
                                .Exec(r => alertSettingForCode310s = r.ToList<AlertSettingForCode310>());
                var StartDate = DateTime.ParseExact(staYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                var EndDate = DateTime.ParseExact(endYmd, "yyyyMMdd", CultureInfo.InvariantCulture);

                foreach (DateTime day in EachDay((DateTime)StartDate, (DateTime)EndDate)) {
                    var dateTimeYmd = day.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                    var allGuides = alertSettingForCode39s.Where(x => string.Compare(x.StaYmd, dateTimeYmd) <= 0 && string.Compare(x.EndYmd, dateTimeYmd) >= 0).Count();
                    var workHolidays = alertSettingForCode310s.Where(x => x.UnkYmd == dateTimeYmd).Count();
                    if ((allGuides - workHolidays) != 0)
                    {
                        return null;
                    }

                    var alertSetting = new Domain.Dto.AlertSetting()
                    {
                        AlertName = (alertCd == 1004 || alertCd == 2004) ? "空運転手０未満" : "空ガイド０未満",
                        Date = day.ToString("MM月dd日", new CultureInfo("ja-JP")),
                        Link = "busschedule",
                        Number = string.Empty
                    };
                    alertSettings.Add(alertSetting);
                }
                return alertSettings;
            }

            public List<Domain.Dto.AlertSetting> GetDataForCode_2007(int tenantCdSeq, int syainCdSeq)
            {
                List<Domain.Dto.AlertSetting> alertSettings = new List<Domain.Dto.AlertSetting>();
                AlertSettingTimeLine alertSettingTimeLine = null;
                List<AlertSettingTimeLine> alertSettingTimeLines = new List<AlertSettingTimeLine>();

                _context.LoadStoredProc("PK_dGetAlertTimeline_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@SyainCdSeq", syainCdSeq)
                                .AddParam("@AlertCd", 2007)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount32)
                                .Exec(r => {
                                    if (r.Read())
                                    {
                                        alertSettingTimeLine = new AlertSettingTimeLine();
                                        alertSettingTimeLine.TenantCdSeq = string.IsNullOrWhiteSpace((string)r["TenantCdSeq"].ToString()) ? 0 : (int)r["TenantCdSeq"];
                                        alertSettingTimeLine.AlertCdSeq = string.IsNullOrWhiteSpace((string)r["AlertCdSeq"].ToString()) ? 0 : (int)r["AlertCdSeq"];
                                        alertSettingTimeLine.AlertKbn = string.IsNullOrWhiteSpace((string)r["AlertKbn"].ToString()) ? (short)0 : (short)r["AlertKbn"];
                                        alertSettingTimeLine.AlertCd = string.IsNullOrWhiteSpace((string)r["AlertCd"].ToString()) ? 0 : (int)r["AlertCd"];
                                        alertSettingTimeLine.AlertNm = (string)r["AlertNm"];
                                        alertSettingTimeLine.DefaultVal = string.IsNullOrWhiteSpace((string)r["DefaultVal"].ToString()) ? 0 : (int)r["DefaultVal"];
                                        alertSettingTimeLine.DefaultTimeline = string.IsNullOrWhiteSpace((string)r["DefaultTimeline"].ToString()) ? (byte)0 : (byte)r["DefaultTimeline"];
                                        alertSettingTimeLine.DefaultZengo = string.IsNullOrWhiteSpace((string)r["DefaultZengo"].ToString()) ? (byte)0 : (byte)r["DefaultZengo"];
                                        alertSettingTimeLine.DefaultDisplayKbn = string.IsNullOrWhiteSpace((string)r["DefaultDisplayKbn"].ToString()) ? (byte)0 : (byte)r["DefaultDisplayKbn"];
                                        alertSettingTimeLine.CurTenantCdSeq = string.IsNullOrWhiteSpace((string)r["CurTenantCdSeq"].ToString()) ? 0 : (int)r["CurTenantCdSeq"];
                                        alertSettingTimeLine.CurVal = string.IsNullOrWhiteSpace((string)r["CurVal"].ToString()) ? 0 : (int)r["CurVal"];
                                        alertSettingTimeLine.CurTimeline = string.IsNullOrWhiteSpace((string)r["CurTimeline"].ToString()) ? (byte)0 : (byte)r["CurTimeline"];
                                        alertSettingTimeLine.CurZengo = string.IsNullOrWhiteSpace((string)r["CurZengo"].ToString()) ? (byte)0 : (byte)r["CurZengo"];
                                        alertSettingTimeLine.CurDisplayKbn = string.IsNullOrWhiteSpace((string)r["CurDisplayKbn"].ToString()) ? (byte)0 : (byte)r["CurDisplayKbn"];
                                        alertSettingTimeLine.SyainCdSeq = string.IsNullOrWhiteSpace((string)r["SyainCdSeq"].ToString()) ? 0 : (int)r["SyainCdSeq"];
                                        alertSettingTimeLine.UserDisplayKbn = string.IsNullOrWhiteSpace((string)r["UserDisplayKbn"].ToString()) ? (byte)0 : (byte)r["UserDisplayKbn"];
                                    }
                                    r.Close();
                                });

                if (alertSettingTimeLine == null || alertSettingTimeLine.CurDisplayKbn == 2)
                {
                    return null;
                }

                if ((alertSettingTimeLine.CurDisplayKbn == 1 && alertSettingTimeLine.UserDisplayKbn == 1) ||
                    (alertSettingTimeLine.CurDisplayKbn == 0 && alertSettingTimeLine.DefaultDisplayKbn == 1
                    && alertSettingTimeLine.UserDisplayKbn == 1))
                {
                    string seiTaiYmd = ConvertTimeLineToTimeString(alertSettingTimeLine.DefaultVal,
                        alertSettingTimeLine.DefaultTimeline, alertSettingTimeLine.DefaultZengo);
                    string staYmd = null; string endYmd = null; string today = DateTime.Today.ToString("yyyyMMdd");
                    staYmd = (int.Parse(today) <= int.Parse(seiTaiYmd)) ? today : seiTaiYmd;
                    endYmd = (int.Parse(today) <= int.Parse(seiTaiYmd)) ? seiTaiYmd : today;
                    List<VehicleAcquisitionProcess> vaps = new List<VehicleAcquisitionProcess>();
                    List<int> haiSsryCdSeqs = new List<int>();
                    DateTime? StartDate = null; DateTime? EndDate = null;

                    _context.LoadStoredProc("PK_dVehicleAcquisitionProcess_R")
                                    .AddParam("@TenantCdSeq", tenantCdSeq)
                                    .AddParam("@StaYmd", staYmd)
                                    .AddParam("@EndYmd", endYmd)
                                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount311)
                                    .Exec(r => { 
                                        while(r.Read())
                                        {
                                            VehicleAcquisitionProcess vap = new VehicleAcquisitionProcess();
                                            vap.HaiSsryCdSeq = Convert.ToInt32(r["HaiSsryCdSeq"]);
                                            vap.KssyaRseq = Convert.ToInt32(r["KssyaRseq"]);
                                            vap.TouYmd = (string)r["TouYmd"];
                                            vap.TouChTime = (string)r["TouChTime"];
                                            vap.HaiSymd = (string)r["HaiSymd"];
                                            vap.HaiStime = (string)r["HaiStime"];
                                            vaps.Add(vap);
                                        }
                                        r.Close();
                                    });   //list ordered
                    if (!vaps.Any() || vaps.Count == 1)
                        return null;

                    for (var i = 0; i < vaps.Count - 1; i++)
                    {
                        if (vaps.Count == 1)
                            break;

                        if (vaps[0].HaiSsryCdSeq != vaps[i + 1].HaiSsryCdSeq || string.IsNullOrWhiteSpace(vaps[0].TouYmd) || string.IsNullOrWhiteSpace(vaps[i+1].TouYmd)
                            || string.IsNullOrWhiteSpace(vaps[0].TouChTime) || string.IsNullOrWhiteSpace(vaps[i+1].TouChTime)
                            || string.IsNullOrWhiteSpace(vaps[0].HaiSymd) || string.IsNullOrWhiteSpace(vaps[i+1].HaiSymd)
                            || string.IsNullOrWhiteSpace(vaps[0].HaiStime) || string.IsNullOrWhiteSpace(vaps[i+1].HaiStime))
                        {
                            vaps.RemoveAt(0);
                            i = -1;
                            continue;
                        }
                        var iSta = long.Parse(vaps[0].HaiSymd + vaps[0].HaiStime);
                        var iEnd = long.Parse(vaps[0].TouYmd + vaps[0].TouChTime);
                        var i1Sta = long.Parse(vaps[i+1].HaiSymd + vaps[i+1].HaiStime);
                        var i1End = long.Parse(vaps[i+1].TouYmd + vaps[i+1].TouChTime);
                        //iSta < i1Sta < iEnd < i1End
                        if (i1Sta <= iEnd && iEnd <= i1End)
                        {
                            //time between i1Sta and iEnd
                            StartDate = DateTime.ParseExact(vaps[i+1].HaiSymd, "yyyyMMdd", CultureInfo.InvariantCulture);
                            EndDate = DateTime.ParseExact(vaps[0].TouYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                        }
                        //iSta < i1Sta < i1End < iEnd
                        if (i1Sta <= iEnd && i1End <= iEnd)
                        {
                            //time between i1Sta and i1End
                            StartDate = DateTime.ParseExact(vaps[i+1].HaiSymd, "yyyyMMdd", CultureInfo.InvariantCulture);
                            EndDate = DateTime.ParseExact(vaps[i+1].TouYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                        }

                        if (StartDate != null && EndDate != null)
                        {
                            foreach (DateTime day in EachDay((DateTime)StartDate, (DateTime)EndDate))
                            {
                                var existAlertSetting = alertSettings.Where(x => x.Date == day.ToString("MM月dd日", new CultureInfo("ja-JP"))).FirstOrDefault();
                                if (existAlertSetting == null)
                                {
                                    var alertSetting = new Domain.Dto.AlertSetting()
                                    {
                                        AlertName = "車両重複",
                                        Date = day.ToString("MM月dd日", new CultureInfo("ja-JP")),
                                        Link = "staffschart",
                                        Number = "1"
                                    };
                                    alertSettings.Add(alertSetting);
                                }
                                else
                                {
                                    existAlertSetting.Number = (int.Parse(existAlertSetting.Number) + 1).ToString();
                                }
                            }
                        }
                    }
                }
                
                return alertSettings;
            }

            public Domain.Dto.AlertSetting GetDataForCode_2008_2009_2010_2011(int tenantCdSeq, int syainCdSeq, int alertCd)
            {
                int count = 0;
                if(alertCd == 2008)
                {
                    _context.LoadStoredProc("PK_dAcquisitionOfVacationRequestInformation_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@SyainCdSeq", syainCdSeq)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount2008)
                                .Exec(r => {
                                    if (r.Read())
                                        count = (int)r["CountNumber"];
                                    r.Close();
                                });
                }

                if(alertCd == 2009 || alertCd == 2010 || alertCd == 2011)
                {
                    _context.LoadStoredProc("PK_dAcquisitionOfConsecutiveWorkingDays_R")
                                .AddParam("@TenantCdSeq", tenantCdSeq)
                                .AddParam("@SyainCdSeq", syainCdSeq)
                                .AddParam("@AlertNumber", alertCd)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount2009)
                                .Exec(r => {
                                    if (r.Read())
                                        count = (int)r["CountNumber"];
                                    r.Close();
                                });
                }

                if((count == 0 && alertCd == 2008) || 
                    (count != 0 && (alertCd == 2009 || alertCd == 2010 || alertCd == 2011)))
                    return null;

                var alertSetting = new Domain.Dto.AlertSetting()
                {
                    AlertName = (alertCd == 2008) ? "休暇申請" : (alertCd == 2009) ? "１２日連続勤務" 
                    : (alertCd == 2010) ? "１３日連続勤務" : (alertCd == 2011) ? "１４日以上連続勤務" : string.Empty,
                    Date = DateTime.Today.ToString("MM月dd日", new CultureInfo("ja-JP")),
                    Link = "staffschart",
                    Number = alertCd == 2008 ? count.ToString() : string.Empty
                };
                return alertSetting;
            }
            public string ConvertTimeLineToTimeString(int defaultVal, byte defaultTimeLine, byte defaultZengo)
            {
                return (defaultTimeLine == (byte)1 ? DateTime.Today.AddDays(defaultZengo == (byte)1 ?
                    (-1) * defaultVal : defaultVal) : DateTime.Today.AddMonths(defaultZengo == (byte)1 ?
                    (-1) * defaultVal : defaultVal)).ToString("yyyyMMdd");
            }

            public void AddNotNullData(List<Domain.Dto.AlertSetting> list, List<Domain.Dto.AlertSetting> value)
            {
                if(value == null || !value.Any())
                    return;
                list.AddRange(value);
            }

            public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
            {
                for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                    yield return day;
            }
        }
    }
}