using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.AttendanceReport.Queries;
using HassyaAllrightCloud.Application.SaleCompany.Queries;
using HassyaAllrightCloud.Application.TransportActualResult.Queries;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Pages;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IAttendanceReportService
    {
        Task<IEnumerable<CodeKbDataItem>> GetCodeKb(int tenantId);
        Task<List<AttendanceReportPage>> GetReportData(AttendanceReportModel model, CancellationToken token);
        Task<List<CompanyData>> GetCompanyListItems(int tenantID);
        Task<List<KasSetDto>> GetKasSetDto(int companyCdSeq, CancellationToken cancellationToken);
    }

    public class AttendanceReportService : IAttendanceReportService, IReportService
    {
        private IMediator _mediator;
        private IReportLoadingService _reportLoadingService;
        private IReportLayoutSettingService _reportLayoutSettingService;

        public AttendanceReportService(
            IMediator mediator,
            IReportLoadingService reportLoadingService, IReportLayoutSettingService reportLayoutSettingService)
        {
            _mediator = mediator;
            _reportLoadingService = reportLoadingService;
            _reportLayoutSettingService = reportLayoutSettingService;
        }
        public async Task<IEnumerable<CodeKbDataItem>> GetCodeKb(int tenantId)
        {
            return await _mediator.Send(new GetCodeKb() { TenantId = tenantId, CodeSyu = "UNSOUKBN" });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<AttendanceReportModel>(queryParams);
            var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Attendancereport, BaseNamespace.Attendancereport,
    new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)searchParams.PageSize);
            _reportLoadingService.Start(searchParams.ReportId);
            var data = await GetReportData(searchParams, default);
            _reportLoadingService.Stop(searchParams.ReportId);
            report.DataSource = InitObjectDataSource(data.ToList(), typeof(AttendanceReportDataSource), "objectDataSource1");

            return report;
        }

        private ObjectDataSource InitObjectDataSource<T>(T data, Type dataSourceType, string dataSourceName)
        {
            Parameter param = new Parameter()
            {
                Name = "dataSource",
                Type = typeof(T),
                Value = data
            };
            ObjectDataSource dataSource = new ObjectDataSource();
            dataSource.Name = dataSourceName;
            dataSource.DataSource = dataSourceType;
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "DataSource";
            return dataSource;
        }

        public async Task<List<AttendanceReportPage>> GetReportData(AttendanceReportModel model, CancellationToken token)
        {
            var result = new List<AttendanceReportPage>();
            var task1 = _mediator.Send(new GetKasSet() { CompanyCdSeq = model.CompanyCdSeq });
            var task2 = _mediator.Send(new GetListVehicle() { Model = model }, token);
            var task3 = _mediator.Send(new GetListVehicleAllocation() { Model = model }, token);
            var task4 = _mediator.Send(new GetListVehicleRepair() { Model = model }, token);
            var task5 = _mediator.Send(new GetCommonData() { Model = model });
            var task6 = _mediator.Send(new GetListStaff() { Model = model });
            var task7 = _mediator.Send(new GetHolidayData() { Model = model });
            var processingDate = DateTime.ParseExact(model.ProcessingDate, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);

            var kasetList = task1.Result;
            KasSetDto kaset = new KasSetDto();
            if (kasetList.Any()) kaset = kasetList.FirstOrDefault();
            var sql1 = task2.Result;
            var sql2 = task3.Result;
            var sql3 = task4.Result;
            var sql4 = task5.Result;
            var sql5 = task6.Result;
            var sql6 = task7.Result;


            var distinctEigyoCd = sql1.Select(one => one.EigyosEigyoCd).Union(sql4.Select(four => four.EigyosEigyoCd)).Distinct();
            var eigyoNm = string.Empty;
            foreach (var cd in distinctEigyoCd)
            {
                var tempSql1 = sql1.Where(e => e.EigyosEigyoCd == cd);
                if (tempSql1.Any()) eigyoNm = tempSql1.First().EigyosEigyoNm;
                var tempSql3 = sql3.Where(e => e.EigyosEigyoCd == cd);
                var tempSql4 = sql4.Where(e => e.EigyosEigyoCd == cd);
                if (tempSql4.Any() && string.IsNullOrEmpty(eigyoNm)) eigyoNm = tempSql4.First().EigyosEigyoNm;
                var tempSql5 = sql5.Join(tempSql4.ToList(), e => e.SyainCdSeq, f => f.SyainSyainCdSeq, (four, five) => four);
                var tempSql6 = sql6.Join(tempSql4, six => six.SyainCdSeq, four => four.SyainSyainCdSeq, (six, four) => six);

                var col1516 = tempSql1.Where(e => !sql2.Any(va => va.HaiSSryCdSeq == e.SyaRyoCdSeq) &&
                                                !tempSql3.Any(r => r.SyaRyoCdSeq == e.SyaRyoCdSeq)).Select(e => new VehicleReportItem() { SyaRyoSyaRyoNm = e.SyaRyoSyaRyoNm, SyaSyuSyaSyuNm = e.SyaSyuSyaSyuNm });

                var col1718 = tempSql1.Join(tempSql3.ToList(),
                               one => one.SyaRyoCdSeq,
                               three => three.SyaRyoCdSeq,
                               (one, three) => new VehicleInfo() { SyaRyoSyaRyoNm = one.SyaRyoSyaRyoNm, SyuRiCdRyakuNm = three.SyuRiCdRyakuNm });
                
                
                var temp5 = tempSql4.Where(e => !tempSql5.Any(s => s.SyainCdSeq == e.SyainSyainCdSeq) &&
                                            !tempSql6.Any(h => h.KinKyuCdKinKyuKbn == 2 && h.SyainCdSeq == e.SyainSyainCdSeq));
                var temp6 = tempSql6.Where(e => e.KinKyuCdKinKyuKbn == 1);

                var col1921 = temp5.Where(e => e.SyokumSyokumuKbn == 1 || e.SyokumSyokumuKbn == 2).SelectMany(
                                            x => temp6.Where(e => e.SyainCdSeq == x.SyainSyainCdSeq).DefaultIfEmpty(),
                                            (x, y) => new { SyainSyainNm = x.SyainSyainNm, KinKyuCdRyakuNm = y?.KinKyuCdRyakuNm })
                                .Select(e => new SyainSyainDataItem() { SyainSyainNm = e.SyainSyainNm, KinKyuCdRyakuNm = e.KinKyuCdRyakuNm });

                var col2022 = temp5.Where(e => e.SyokumSyokumuKbn == 3 || e.SyokumSyokumuKbn == 4).SelectMany(
                                            x => temp6.Where(e => e.SyainCdSeq == x.SyainSyainCdSeq).DefaultIfEmpty(),
                                            (x, y) => new { SyainSyainNm = x.SyainSyainNm, KinKyuCdRyakuNm = y?.KinKyuCdRyakuNm })
                                .Select(e => new SyainSyainDataItem() { SyainSyainNm = e.SyainSyainNm, KinKyuCdRyakuNm = e.KinKyuCdRyakuNm });

                var temp = tempSql4.Join(tempSql6.Where(i => i.KinKyuCdKinKyuKbn == 2), four => four.SyainSyainCdSeq, six => six.SyainCdSeq, (four, six) => new
                {
                    SyainSyainNm = four.SyainSyainNm,
                    KinKyuCdSeq = six.KinKyuCdSeq
                });
                var col23 = temp.Where(e => e.KinKyuCdSeq == 1).Select(e => e.SyainSyainNm).ToList();
                var col24 = temp.Where(e => e.KinKyuCdSeq == 2).Select(e => e.SyainSyainNm).ToList();
                var col25 = temp.Where(e => e.KinKyuCdSeq == 3).Select(e => e.SyainSyainNm).ToList();
                var col26 = temp.Where(e => e.KinKyuCdSeq == 4).Select(e => e.SyainSyainNm).ToList();
                var col27 = temp.Where(e => e.KinKyuCdSeq == 5).Select(e => e.SyainSyainNm).ToList();
                var col28 = temp.Where(e => e.KinKyuCdSeq == 6).Select(e => e.SyainSyainNm).ToList();
                var col29 = temp.Where(e => e.KinKyuCdSeq == 7).Select(e => e.SyainSyainNm).ToList();
                var col30 = temp.Where(e => e.KinKyuCdSeq == 8).Select(e => e.SyainSyainNm).ToList();
                var col31 = temp.Where(e => e.KinKyuCdSeq == 9).Select(e => e.SyainSyainNm).ToList();
                var col32 = temp.Where(e => e.KinKyuCdSeq == 10).Select(e => e.SyainSyainNm).ToList();

                var temp1 = sql2.Where(e => !tempSql3.Any(vr => vr.SyaRyoCdSeq == e.HaiSSryCdSeq));
                var col33 = temp1.Count(e => e.SyuKoYmd == e.KikYmd);
                var col34 = temp1.Count(e => e.SyuKoYmd != e.KikYmd && e.KikYmd != model.ProcessingDate);
                var col35 = temp1.Count(e => e.SyuKoYmd != e.KikYmd && e.KikYmd == model.ProcessingDate);
                var col36 = col33 + col34 + col35;

                var col37 = col1718.Count();

                var col38 = tempSql5.Count(e => !tempSql6.Any(h => h.SyainCdSeq == e.SyainCdSeq) && tempSql4.Any(c => c.SyainSyainCdSeq == e.SyainCdSeq && (c.SyokumSyokumuKbn == 1 || c.SyokumSyokumuKbn == 2)));
                var col39 = tempSql5.Count(e => !tempSql6.Any(h => h.SyainCdSeq == e.SyainCdSeq) && tempSql4.Any(c => c.SyainSyainCdSeq == e.SyainCdSeq && (c.SyokumSyokumuKbn == 3 || c.SyokumSyokumuKbn == 4)));
                var col40 = col38 + col39;

                var col41 = tempSql6.Count(e => tempSql4.Any(c => c.SyainSyainCdSeq == e.SyainCdSeq && (c.SyokumSyokumuKbn == 1 || c.SyokumSyokumuKbn == 2)));
                var col42 = tempSql6.Count(e => tempSql4.Any(c => c.SyainSyainCdSeq == e.SyainCdSeq && (c.SyokumSyokumuKbn == 3 || c.SyokumSyokumuKbn == 4)));
                var col43 = col41 + col42;

                var col44 = col1516.Count();

                var col45 = col1921.Count();
                var col46 = col2022.Count();
                var col47 = col45 + col46;

                var col48 = sql2.Where(e => e.SyuKoYmd == model.ProcessingDate).Select(e => int.Parse(e.SyuKoTime)).OrderBy(e => e).FirstOrDefault();
                var col49 = sql2.Where(e => e.KikYmd == model.ProcessingDate).Select(e => int.Parse(e.KikTime)).OrderByDescending(e => e).FirstOrDefault();

                var holidayInfos = new List<HolidayInfo>();
                var maxCount = new List<int>() { col23.Count, col24.Count, col25.Count, col26.Count, col27.Count, col28.Count, col29.Count, col30.Count, col31.Count, col32.Count}.Max();
                for (var i = 0; i < maxCount; i++)
                {
                    holidayInfos.Add(new HolidayInfo()
                    {
                        Holiday1 = col23.Count < i + 1 ? string.Empty : col23[i],
                        Holiday2 = col24.Count < i + 1 ? string.Empty : col24[i],
                        Holiday3 = col25.Count < i + 1 ? string.Empty : col25[i],
                        Holiday4 = col26.Count < i + 1 ? string.Empty : col26[i],
                        Holiday5 = col27.Count < i + 1 ? string.Empty : col27[i],
                        Holiday6 = col28.Count < i + 1 ? string.Empty : col28[i],
                        Holiday7 = col29.Count < i + 1 ? string.Empty : col29[i],
                        Holiday8 = col30.Count < i + 1 ? string.Empty : col30[i],
                        Holiday9 = col31.Count < i + 1 ? string.Empty : col31[i],
                        Holiday10 = col32.Count < i + 1 ? string.Empty : col32[i]
                    });
                }

                var totalData = new AttendanceReportDataItem()
                {
                    VehicleItems = col1516.ToList(),
                    VehicleInfoList = col1718.ToList(),
                    // Syain
                    Drivers = col1921.ToList(),
                    Guiders = col2022.ToList(),
                    // Holiday
                    HolidayInfos = holidayInfos
                };

                // 40 is total rows per page of VehicleItems, VehicleInfoList, Drivers, Guiders
                // 19 is total rows per page of HolidayInfos
                var maxPage = new List<int>() {
                    (totalData.VehicleItems.Count % 40 > 0) ? (totalData.VehicleItems.Count / 40 + 1) : totalData.VehicleItems.Count,
                    (totalData.VehicleInfoList.Count % 40 > 0) ? (totalData.VehicleInfoList.Count / 40 + 1) : totalData.VehicleInfoList.Count,
                    (totalData.Drivers.Count % 40 > 0) ? (totalData.Drivers.Count / 40 + 1) : totalData.Drivers.Count,
                    (totalData.Guiders.Count % 40 > 0) ? (totalData.Guiders.Count / 40 + 1) : totalData.Guiders.Count,
                    (totalData.HolidayInfos.Count % 19 > 0) ? (totalData.HolidayInfos.Count / 19 + 1) : totalData.HolidayInfos.Count,
                }.Max();

                var listResult = new List<AttendanceReportDataItem>();
                for(var i = 0; i < maxPage; i++)
                {
                    listResult.Add(new AttendanceReportDataItem()
                    {
                        VehicleItems = totalData.VehicleItems.Skip(i * 40).Take(40).Select((e, index) => { e.Index = index; return e; }).ToList(),
                        VehicleInfoList = totalData.VehicleInfoList.Skip(i * 40).Take(40).Select((e, index) => { e.Index = index; return e; }).ToList(),
                        // Syain
                        Drivers = totalData.Drivers.Skip(i * 40).Take(40).Select((e, index) => { e.Index = index; return e; }).ToList(),
                        Guiders = totalData.Guiders.Skip(i * 40).Take(40).Select((e, index) => { e.Index = index; return e; }).ToList(),
                        // Holiday
                        HolidayInfos = totalData.HolidayInfos.Skip(i * 19).Take(19).Select((e, index) => { e.Index = index; return e; }).ToList()
                    });
                }
                
                foreach(var item in listResult)
                {
                    var commonData = new AttendanceReportCommonData()
                    {
                        DailyVehiclesCount = col33 == 0 ? string.Empty : col33.AddCommas(),
                        TodayVehiclesCount = col34 == 0 ? string.Empty : col34.AddCommas(),
                        OvernightVehiclesCount = col35 == 0 ? string.Empty : col35.AddCommas(),
                        WorkingVehiclesTotal = col36 == 0 ? string.Empty : col36.AddCommas(),
                        SuspendedVehiclesCount = col37 == 0 ? string.Empty : col37.AddCommas(),
                        WorkingDriversCount = col38 == 0 ? string.Empty : col38.AddCommas(),
                        WorkingGuidesCount = col39 == 0 ? string.Empty : col39.AddCommas(),
                        WorkingStaffsTotal = col40 == 0 ? string.Empty : col40.AddCommas(),
                        AbsenceDriversCount = col41 == 0 ? string.Empty : col41.AddCommas(),
                        AbsenceGuidesCount = col42 == 0 ? string.Empty : col42.AddCommas(),
                        AbsenceStaffsCount = col43 == 0 ? string.Empty : col43.AddCommas(),
                        WaitingVehiclesCount = col44 == 0 ? string.Empty : col44.AddCommas(),
                        WaitingDriversCount = col45 == 0 ? string.Empty : col45.AddCommas(),
                        WaitingGuidesCount = col46 == 0 ? string.Empty : col46.AddCommas(),
                        WaitingStaffsCount = col47 == 0 ? string.Empty : col47.AddCommas(),
                        FirstStartTime = col48 == 0 ? string.Empty : $"{col48:0000}".Insert(2, ":"),
                        LastReturnTime = col49 == 0 ? string.Empty : $"{col49:0000}".Insert(2, ":"),
                        CurrentDateTime = DateTime.Now.ToString(DateTimeFormat.yyyyMMddSlashHHmmColon),
                        CurrentUser = $"{new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd} {new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name}",
                        EigyoName = eigyoNm,
                        HolidayTypeNm1 = kaset.JisKinKyuNm01,
                        HolidayTypeNm2 = kaset.JisKinKyuNm02,
                        HolidayTypeNm3 = kaset.JisKinKyuNm03,
                        HolidayTypeNm4 = kaset.JisKinKyuNm04,
                        HolidayTypeNm5 = kaset.JisKinKyuNm05,
                        HolidayTypeNm6 = kaset.JisKinKyuNm06,
                        HolidayTypeNm7 = kaset.JisKinKyuNm07,
                        HolidayTypeNm8 = kaset.JisKinKyuNm08,
                        HolidayTypeNm9 = kaset.JisKinKyuNm09,
                        HolidayTypeNm10 = kaset.JisKinKyuNm10,
                        ProcessingDate = processingDate.ToString(DateTimeFormat.yyyyMMdd_ddd_JP)
                    };
                    var temp4 = new AttendanceReportPage()
                    {
                        CommonData = commonData,
                        Data = item
                    };
                    result.Add(temp4);
                }
            }

            var pageCount = 1;
            foreach (var item in result)
            {
                item.CommonData.CurrentPage = $"{pageCount}/{result.Count}";
                pageCount++;
            }
            return result;
        }

        public async Task<List<CompanyData>> GetCompanyListItems(int tenantID)
        {
            return await _mediator.Send(new GetCompanyByTenantIdQuery(tenantID));
        }

        public async Task<List<KasSetDto>> GetKasSetDto(int companyCdSeq, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetKasSet() { CompanyCdSeq = companyCdSeq }, cancellationToken);
        }
    }
}
