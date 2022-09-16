using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.BusTypeListReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.IService
{
    public interface IBusTypeListReportService : IReportService
    {
        Task<List<TPM_CodeKbCodeSyuData>> GetCodeKbn(int tenantId, string codeSyu);
        Task<List<BusTypeItemDataReport>> GetBusTypeListReport(BusTypeListData paramSearch);
        Task<List<BusTypeDetailDataReport>> GetBusTypeDetailData(BusTypeListData paramSearch);
        Task<List<NumberVehicleOfBusUnAsign>> GetNumberOfBusUnAsignDataReport(BusTypeListData paramSearch, BusMode busMode);
        Task<List<BusTypeDetailDataReport>> FillterDataByBusType(List<BusTypeDetailDataReport> dataSource, BusTypeListData paramSearch, BusTypeItemDataReport busTypeItem);
        Task<List<BusTypeDetailDataReport>> FillterDataByModeGroup(List<BusTypeDetailDataReport> dataSource, BusTypeListData paramSearch, BusTypeItemDataReport itemBusTypeWithBranchOrCom, GroupMode groupMode);
        Task<List<NumberVehicleOfBusUnAsign>> FillterBusUnAsignDataByModeGroup(List<NumberVehicleOfBusUnAsign> busUnAsignDataSource, BusTypeListData paramSearch, BusTypeItemDataReport itemBusTypeWithBranchOrCom, GroupMode groupMode, int kataKbn);
        Task<List<NumberVehicleOfBusUnAsign>> FillterBusHiringData(List<NumberVehicleOfBusUnAsign> dataSource, BusTypeListData paramSearch);
        Task<List<NumberVehicleOfBusUnAsign>> FillterBusHiringDataByModeGroup(List<NumberVehicleOfBusUnAsign> busHiringDataSource, BusTypeListData paramSearch, BusTypeItemDataReport itemBusTypeWithBranchOrCom, GroupMode groupMode);
        Task<List<BusTypeDetailDataReport>> SumDataByKataKbn(List<BusTypeDetailDataReport> data, int kataKbn, GroupMode groupOption, BusTypeItemDataReport itemBusTypeWithBranchOrCom, ShowMode showMode);
        Task<List<NumberVehicleOfBusUnAsign>> FillterBusUnAsignDataByKataKbn(List<NumberVehicleOfBusUnAsign> dataSource, BusTypeListData paramSearch, int kataKbn, ShowMode showMode);
        Task<List<BusTypeDetailDataReport>> FillterDataByEmployee(List<BusTypeDetailDataReport> dataSource, BusTypeListData paramSearch, StaffPos staffPosition, BusTypeItemDataReport itemBusTypeWithBranchOrCom, GroupMode groupOption);
        Task<List<BusTypeItemDataReport>> GroupDataByBusType(List<BusTypeItemDataReport> dataSource, GroupMode groupOption, List<BusTypeItemDataReport> dataCompanyOrBranchList);
        Task<List<BusTypeItemDataReport>> GroupDataByGroupOption(List<BusTypeItemDataReport> dataSource, GroupMode groupOption);
        Task<List<BusTypeListReportPDF>> GetPDFData(BusTypeListData searchParams);
        Task<List<BusTypeListReportGroupPDF>> GetPDFDataReportGroup(BusTypeListData searchParams);
        Task<List<BusRepairDataSource>> GetBusRepairReportData(BusTypeListData searchParams);
        Task<List<HenSyaDataSource>> GetHenSyaReportData(BusTypeListData searchParams);
        Task<List<CalendarReport>> GetDayOff(int countryCdSeq, int tenantCdSeq, List<BusTypeItemDataReport> companyList);
        Dictionary<string, string> GetFieldValues(BusTypeListData reportData);
        Task<List<BusTypeItemDataReport>> FillterNumberOfBusTypeByGroupOption(List<BusTypeItemDataReport> dataSource, GroupMode groupOption);
        void ApplyFilter(ref BusTypeListData reportData, List<ReservationClassComponentData> bookingTypeList, List<CompanyData> companychartlst, List<LoadSaleBranch> branchList, List<LoadStaffList> staffList, List<DestinationData> destinationList, List<TPM_CodeKbCodeSyuData> busTypeList, List<BusTypesData> vehicleTypeList, List<DepositOutputClass> depositOutputClasses, Dictionary<string, string> filterValues);
    }
    public class BusTypeListReportService : IBusTypeListReportService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly ITPM_CodeSyService _codeSyuService;
        private readonly ILogger<BusTypeListReportService> _logger;

        public BusTypeListReportService(KobodbContext context, IMediator mediator, ITPM_CodeSyService codeSyuService, ILogger<BusTypeListReportService> logger)
        {
            _dbContext = context;
            _mediator = mediator;
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            _logger = logger;

        }

        #region Load Data Show Screen By SearchParam

        public async Task<List<BusTypeItemDataReport>> GetBusTypeListReport(BusTypeListData paramSearch)
        {
            return await _mediator.Send(new GetBusTypeListDataQuery { BusTypeListDataParam = paramSearch });
        }
        public async Task<List<TPM_CodeKbCodeSyuData>> GetCodeKbn(int tenantId, string codeSyu)
        {

            return await _mediator.Send(new GetCodeKbnCodeSyuDataQuery(codeSyu = codeSyu, tenantId = tenantId));
        }
        public async Task<List<BusTypeDetailDataReport>> GetBusTypeDetailData(BusTypeListData paramSearch)
        {
            return await _mediator.Send(new GetNumberOfVehicleDataQuery { BusTypeListDataParam = paramSearch });
        }
        public async Task<List<NumberVehicleOfBusUnAsign>> GetNumberOfBusUnAsignDataReport(BusTypeListData paramSearch, BusMode busMode)
        {
            return await _mediator.Send(new GetNumberOfBusUnAsignDataQuery { BusTypeListDataParam = paramSearch, BusMode = busMode });
        }
        public async Task<List<BusRepairDataSource>> GetBusRepairReportData(BusTypeListData searchParams)
        {
            return await _mediator.Send(new GetBusRepairDataQuery { BusTypeListDataParam = searchParams });
        }
        public async Task<List<HenSyaDataSource>> GetHenSyaReportData(BusTypeListData searchParams)
        {
            return await _mediator.Send(new GetHenSyaDataQuery { BusTypeListDataParam = searchParams });
        }
        public async Task<List<CalendarReport>> GetDayOff(int countryCdSeq, int tenantCdSeq, List<BusTypeItemDataReport> companyList)
        {
            return await _mediator.Send(new GetDayOffByTenantCdSeq { CountryCdSeq = countryCdSeq, TenantCdSeq = tenantCdSeq, CompanyList = companyList });
        }
        public async Task<List<BusTypeDetailDataReport>> FillterDataByBusType(List<BusTypeDetailDataReport> dataSource, BusTypeListData paramSearch, BusTypeItemDataReport busTypeItem)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                int kataKbn = 0;
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    int i = 1;
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyaSyuCdSeq = busTypeItem.SyaSyuCdSeq,
                        SyuKoYmd = strDate,
                        STT = i
                    };
                    var dateFillter = (from data in dataSource
                                       where data.SyuKoYmd.CompareTo(strDate) <= 0
                                      && data.KikYmd.CompareTo(strDate) >= 0
                                     && data.SyaSyuCdSeq == busTypeItem.SyaSyuCdSeq
                                       select (data)).ToList();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        kataKbn = dateFillter.FirstOrDefault().KataKbn;
                        var dataGroup = (from data in dateFillter
                                         group data by new { data.KataKbn, data.SyaSyuCdSeq, data.SyaSyuNm } into kq
                                         select new BusTypeDetailDataReport()
                                         {
                                             SyaSyuCdSeq = kq.Key.SyaSyuCdSeq,
                                             SyaSyuNm = kq.Key.SyaSyuNm,
                                             SyuKoYmd = strDate,
                                             Number = kq.Count(),
                                             KataKbn = kq.Key.KataKbn,
                                             STT = i
                                         }).ToList().OrderBy(x => x.SyuKoYmd);

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                    i++;
                }
                result.ForEach(x => x.KataKbn = kataKbn);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<NumberVehicleOfBusUnAsign>> FillterBusUnAsignDataByKataKbn(List<NumberVehicleOfBusUnAsign> dataSource, BusTypeListData paramSearch, int kataKbn, ShowMode showMode)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<NumberVehicleOfBusUnAsign>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new NumberVehicleOfBusUnAsign()
                    {
                        NumberOfVehicle = 0,
                        KataKbn = kataKbn,
                        SyuKoYmd = strDate
                    };
                    var dateFillter = new List<NumberVehicleOfBusUnAsign>();
                    if (showMode == ShowMode.Screen)
                    {
                        dateFillter = (from data in dataSource
                                       where data.SyuKoYmd.CompareTo(strDate) <= 0
                                      && data.KikYmd.CompareTo(strDate) >= 0
                                     && data.KataKbn == kataKbn
                                       select (data)).ToList();
                        if (dateFillter != null && dateFillter.Any())
                        {
                            var dataGroup = (from data in dateFillter
                                             group data by new { data.KataKbn } into kq
                                             select new NumberVehicleOfBusUnAsign()
                                             {
                                                 KataKbn = kq.Key.KataKbn,
                                                 SyuKoYmd = strDate,
                                                 NumberOfVehicle = kq.Count()
                                             }).ToList().OrderBy(x => x.SyuKoYmd);

                            if (dataGroup != null && dataGroup.Any())
                            {
                                result.Add(dataGroup.FirstOrDefault());
                            }
                            else
                            {
                                result.Add(dataNull);
                            }
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else if (showMode == ShowMode.Report)
                    {
                        dateFillter = (from data in dataSource
                                       where data.SyuKoYmd.CompareTo(strDate) <= 0
                                      && data.KikYmd.CompareTo(strDate) >= 0
                                       select (data)).ToList();
                        if (dateFillter != null && dateFillter.Any())
                        {
                            var dataGroup = (from data in dateFillter
                                                 //group data by new { data.SyuKoYmd } into kq
                                             select new NumberVehicleOfBusUnAsign()
                                             {
                                                 SyuKoYmd = strDate,
                                                 NumberOfVehicle = dateFillter.Count()
                                             }).ToList().OrderBy(x => x.SyuKoYmd);

                            if (dataGroup != null && dataGroup.Any())
                            {
                                result.Add(dataGroup.FirstOrDefault());
                            }
                            else
                            {
                                result.Add(dataNull);
                            }
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<NumberVehicleOfBusUnAsign>> FillterBusHiringData(List<NumberVehicleOfBusUnAsign> dataSource, BusTypeListData paramSearch)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<NumberVehicleOfBusUnAsign>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new NumberVehicleOfBusUnAsign()
                    {
                        NumberOfVehicle = 0,
                        SyuKoYmd = strDate
                    };
                    var dateFillter = (from data in dataSource
                                       where data.SyuKoYmd.CompareTo(strDate) <= 0
                                      && data.KikYmd.CompareTo(strDate) >= 0
                                       select (data)).ToList();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        var dataGroup = (from data in dateFillter
                                         select new NumberVehicleOfBusUnAsign()
                                         {
                                             SyuKoYmd = strDate,
                                             NumberOfVehicle = dateFillter.Count()
                                         }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<BusTypeDetailDataReport>> SumDataByKataKbn(List<BusTypeDetailDataReport> data, int kataKbn, GroupMode groupOption, BusTypeItemDataReport itemBusTypeWithBranchOrCom, ShowMode showMode)
        {
            try
            {
                var result = new List<BusTypeDetailDataReport>();
                var dataTemp = new List<BusTypeDetailDataReport>();
                if (showMode == ShowMode.Screen)
                {
                    if (groupOption == GroupMode.All)
                    {
                        result = (from av in data
                                  where av.KataKbn == kataKbn
                                  group av by new { av.KataKbn, av.SyuKoYmd } into kq
                                  select new BusTypeDetailDataReport()
                                  {
                                      KataKbn = kq.Key.KataKbn,
                                      Number = kq.Sum(x => x.Number),
                                      SyuKoYmd = kq.Key.SyuKoYmd
                                  }).ToList().OrderBy(x => x.SyuKoYmd).ToList();

                    }
                    else if (groupOption == GroupMode.Branch)
                    {
                        dataTemp = (from av in data
                                    where av.KataKbn == kataKbn
                                    && av.EigyoCdSeq == itemBusTypeWithBranchOrCom.EigyoCdSeq
                                    select (av)).ToList();
                        result = (from av in dataTemp
                                  group av by new { av.KataKbn, av.SyuKoYmd } into kq
                                  select new BusTypeDetailDataReport()
                                  {
                                      KataKbn = kq.Key.KataKbn,
                                      Number = kq.Sum(x => x.Number),
                                      SyuKoYmd = kq.Key.SyuKoYmd,
                                      EigyoCdSeq = itemBusTypeWithBranchOrCom.EigyoCdSeq
                                  }).ToList().OrderBy(x => x.SyuKoYmd).ToList();
                    }
                    else if (groupOption == GroupMode.Company)
                    {
                        dataTemp = (from av in data
                                    where av.KataKbn == kataKbn && av.CompanyCdSeq == itemBusTypeWithBranchOrCom.CompanyCdSeq
                                    select (av)).ToList();
                        result = (from av in dataTemp
                                  group av by new { av.KataKbn, av.SyuKoYmd } into kq
                                  select new BusTypeDetailDataReport()
                                  {
                                      KataKbn = kq.Key.KataKbn,
                                      Number = kq.Sum(x => x.Number),
                                      SyuKoYmd = kq.Key.SyuKoYmd,
                                      CompanyCdSeq = itemBusTypeWithBranchOrCom.CompanyCdSeq

                                  }).ToList().OrderBy(x => x.SyuKoYmd).ToList();
                    }
                }
                else if (showMode == ShowMode.Report)
                {
                    result = (from av in data
                              group av by new { av.SyuKoYmd } into kq
                              select new BusTypeDetailDataReport()
                              {
                                  Number = kq.Sum(x => x.Number),
                                  SyuKoYmd = kq.Key.SyuKoYmd
                              }).ToList().OrderBy(x => x.SyuKoYmd).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<BusTypeItemDataReport>> GroupDataByGroupOption(List<BusTypeItemDataReport> dataSource, GroupMode groupOption)
        {
            try
            {
                var result = new List<BusTypeItemDataReport>();
                if (groupOption == GroupMode.All)
                {
                    result = (from av in dataSource
                              group av by new { av.SyaSyuCdSeq, av.SyaSyuNm } into kq
                              select new BusTypeItemDataReport()
                              {
                                  SyaSyuCdSeq = kq.Key.SyaSyuCdSeq,
                                  SyaSyuNm = kq.Key.SyaSyuNm,
                                  //CountNumberOfVehicle = kq.Count()
                              }).ToList();
                }
                else if (groupOption == GroupMode.Branch)
                {
                    result = (from av in dataSource
                              group av by new { av.EigyoCdSeq, av.Ei_RyakuNm, av.CompanyCdSeq } into kq
                              select new BusTypeItemDataReport()
                              {
                                  EigyoCdSeq = kq.Key.EigyoCdSeq,
                                  Ei_RyakuNm = kq.Key.Ei_RyakuNm,
                                  CompanyCdSeq = kq.Key.CompanyCdSeq,
                                  CountNumberOfVehicle = kq.Count()
                              }).ToList().OrderBy(x => x.EigyoCdSeq).ToList();
                }
                else if (groupOption == GroupMode.Company)
                {
                    result = (from av in dataSource
                              group av by new { av.CompanyCdSeq, av.COM_RyakuNm } into kq
                              select new BusTypeItemDataReport()
                              {
                                  CompanyCdSeq = kq.Key.CompanyCdSeq,
                                  COM_RyakuNm = kq.Key.COM_RyakuNm,
                                  CountNumberOfVehicle = kq.Count()
                              }).ToList().OrderBy(x => x.CompanyCdSeq).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<BusTypeDetailDataReport>> FillterDataByEmployee(List<BusTypeDetailDataReport> dataSource, BusTypeListData paramSearch, StaffPos staffPosition, BusTypeItemDataReport itemBusTypeWithBranchOrCom, GroupMode groupOption)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        DrvJin = 0,
                        GuiSu = 0,
                        SyuKoYmd = strDate,
                        IsDriver = staffPosition == StaffPos.Driver ? 1 : 2,
                        CompanyCdSeq = itemBusTypeWithBranchOrCom?.CompanyCdSeq ?? 0,
                        EigyoCdSeq = itemBusTypeWithBranchOrCom?.EigyoCdSeq ?? 0
                    };
                    var dateFillter = new List<BusTypeDetailDataReport>();
                    if (groupOption == GroupMode.All)
                    {
                        dateFillter = (from data in dataSource
                                       where data.SyuKoYmd.CompareTo(strDate) <= 0
                                      && data.KikYmd.CompareTo(strDate) >= 0
                                       select (data)).ToList();
                    }
                    else if (groupOption == GroupMode.Branch)
                    {
                        dateFillter = (from data in dataSource
                                       where data.SyuKoYmd.CompareTo(strDate) <= 0
                                      && data.KikYmd.CompareTo(strDate) >= 0
                                      && data.EigyoCdSeq == itemBusTypeWithBranchOrCom.EigyoCdSeq
                                       select (data)).ToList();
                    }
                    else if (groupOption == GroupMode.Company)
                    {
                        dateFillter = (from data in dataSource
                                       where data.SyuKoYmd.CompareTo(strDate) <= 0
                                      && data.KikYmd.CompareTo(strDate) >= 0
                                      && data.CompanyCdSeq == itemBusTypeWithBranchOrCom.CompanyCdSeq
                                       select (data)).ToList();
                    }
                    if (dateFillter != null && dateFillter.Any())
                    {
                        var dataGroup = new BusTypeDetailDataReport();
                        if (staffPosition == StaffPos.Driver)
                        {
                            dataGroup = (from data in dateFillter
                                         select new BusTypeDetailDataReport()
                                         {
                                             SyuKoYmd = strDate,
                                             Number = dateFillter.Sum(x => x.DrvJin),
                                             IsDriver = staffPosition == StaffPos.Driver ? 1 : 2,
                                             CompanyCdSeq = itemBusTypeWithBranchOrCom?.CompanyCdSeq ?? 0,
                                             EigyoCdSeq = itemBusTypeWithBranchOrCom?.EigyoCdSeq ?? 0
                                         }).FirstOrDefault();
                        }
                        else if (staffPosition == StaffPos.GuiSu)
                        {
                            dataGroup = (from data in dateFillter
                                         select new BusTypeDetailDataReport()
                                         {
                                             SyuKoYmd = strDate,
                                             Number = dateFillter.Sum(x => x.GuiSu),
                                             IsDriver = staffPosition == StaffPos.Driver ? 1 : 2,
                                             CompanyCdSeq = itemBusTypeWithBranchOrCom?.CompanyCdSeq ?? 0,
                                             EigyoCdSeq = itemBusTypeWithBranchOrCom?.EigyoCdSeq ?? 0
                                         }).FirstOrDefault();
                        }
                        if (dataGroup != null)
                        {
                            result.Add(dataGroup);
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<BusTypeItemDataReport>> GroupDataByBusType(List<BusTypeItemDataReport> dataSource, GroupMode groupOption, List<BusTypeItemDataReport> dataCompanyOrBranchList)
        {
            try
            {
                var result = new List<BusTypeItemDataReport>();
                foreach (var item in dataCompanyOrBranchList)
                {
                    var dataTmp = new List<BusTypeItemDataReport>();
                    if (groupOption == GroupMode.Company)
                    {

                        dataTmp = dataSource.Where(x => x.CompanyCdSeq == item.CompanyCdSeq).ToList();
                        dataTmp = (from av in dataTmp
                                   group av by new { av.SyaSyuCdSeq, av.SyaSyuNm } into kq
                                   select new BusTypeItemDataReport()
                                   {
                                       SyaSyuCdSeq = kq.Key.SyaSyuCdSeq,
                                       SyaSyuNm = kq.Key.SyaSyuNm,
                                       CompanyCdSeq = item.CompanyCdSeq,
                                       CountNumberOfVehicle = kq.Count()
                                   }).ToList();
                    }
                    else if (groupOption == GroupMode.Branch)
                    {
                        dataTmp = dataSource.Where(x => x.EigyoCdSeq == item.EigyoCdSeq).ToList();
                        dataTmp = (from av in dataTmp
                                   group av by new { av.SyaSyuCdSeq, av.SyaSyuNm } into kq
                                   select new BusTypeItemDataReport()
                                   {
                                       SyaSyuCdSeq = kq.Key.SyaSyuCdSeq,
                                       SyaSyuNm = kq.Key.SyaSyuNm,
                                       EigyoCdSeq = item.EigyoCdSeq,
                                       CountNumberOfVehicle = kq.Count()
                                   }).ToList();
                    }
                    result.AddRange(dataTmp);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<BusTypeDetailDataReport>> FillterDataByModeGroup(List<BusTypeDetailDataReport> dataSource, BusTypeListData paramSearch, BusTypeItemDataReport itemBusTypeWithBranchOrCom, GroupMode groupOption)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                int kataKbn = 0;
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyaSyuCdSeq = itemBusTypeWithBranchOrCom.SyaSyuCdSeq,
                        CompanyCdSeq = itemBusTypeWithBranchOrCom.CompanyCdSeq,
                        EigyoCdSeq = itemBusTypeWithBranchOrCom.EigyoCdSeq,
                        SyuKoYmd = strDate
                    };
                    var dateFillter = new List<BusTypeDetailDataReport>();
                    dateFillter = (from data in dataSource
                                   where data.SyuKoYmd.CompareTo(strDate) <= 0
                                  && data.KikYmd.CompareTo(strDate) >= 0
                                 && data.SyaSyuCdSeq == itemBusTypeWithBranchOrCom.SyaSyuCdSeq
                                 && (groupOption == GroupMode.Branch ? data.EigyoCdSeq == itemBusTypeWithBranchOrCom.EigyoCdSeq :
                                 data.CompanyCdSeq == itemBusTypeWithBranchOrCom.CompanyCdSeq)
                                   select (data)).ToList();
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        kataKbn = dateFillter.FirstOrDefault().KataKbn;
                        dataGroup = (from data in dateFillter
                                     group data by new { data.KataKbn, data.SyaSyuCdSeq, data.SyaSyuNm } into kq
                                     select new BusTypeDetailDataReport()
                                     {
                                         SyaSyuCdSeq = kq.Key.SyaSyuCdSeq,
                                         SyaSyuNm = kq.Key.SyaSyuNm,
                                         SyuKoYmd = strDate,
                                         EigyoCdSeq = itemBusTypeWithBranchOrCom.EigyoCdSeq,
                                         CompanyCdSeq = itemBusTypeWithBranchOrCom.CompanyCdSeq,
                                         KataKbn = kq.Key.KataKbn,
                                         Number = kq.Count()
                                     }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                result.ForEach(x => x.KataKbn = kataKbn);
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<NumberVehicleOfBusUnAsign>> FillterBusUnAsignDataByModeGroup(List<NumberVehicleOfBusUnAsign> busUnAsignDataSource, BusTypeListData paramSearch, BusTypeItemDataReport itemBusTypeWithBranchOrCom, GroupMode groupOption, int kataKbn)
        {
            try
            {
                var result = new List<NumberVehicleOfBusUnAsign>();
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new NumberVehicleOfBusUnAsign()
                    {
                        KataKbn = kataKbn,
                        CompanyCdSeq = itemBusTypeWithBranchOrCom.CompanyCdSeq,
                        UkeEigCdSeq = itemBusTypeWithBranchOrCom.EigyoCdSeq,
                        SyuKoYmd = strDate,
                        NumberOfVehicle = 0
                    };
                    var dateFillter = new List<NumberVehicleOfBusUnAsign>();
                    dateFillter = (from data in busUnAsignDataSource
                                   where data.SyuKoYmd.CompareTo(strDate) <= 0
                                  && data.KikYmd.CompareTo(strDate) >= 0
                                 && data.KataKbn == kataKbn
                                 && (groupOption == GroupMode.Branch ? data.UkeEigCdSeq == itemBusTypeWithBranchOrCom.EigyoCdSeq :
                                 data.CompanyCdSeq == itemBusTypeWithBranchOrCom.CompanyCdSeq)
                                   select (data)).ToList();
                    var dataGroup = new List<NumberVehicleOfBusUnAsign>();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        dataGroup = (from data in dateFillter
                                     group data by new { data.KataKbn } into kq
                                     select new NumberVehicleOfBusUnAsign()
                                     {
                                         KataKbn = kataKbn,
                                         SyuKoYmd = strDate,
                                         UkeEigCdSeq = itemBusTypeWithBranchOrCom.EigyoCdSeq,
                                         CompanyCdSeq = itemBusTypeWithBranchOrCom.CompanyCdSeq,
                                         NumberOfVehicle = kq.Count()
                                     }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        public async Task<List<NumberVehicleOfBusUnAsign>> FillterBusHiringDataByModeGroup(List<NumberVehicleOfBusUnAsign> busHiringDataSource, BusTypeListData paramSearch, BusTypeItemDataReport itemBusTypeWithBranchOrCom, GroupMode groupOption)
        {
            try
            {
                var result = new List<NumberVehicleOfBusUnAsign>();
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new NumberVehicleOfBusUnAsign()
                    {
                        CompanyCdSeq = itemBusTypeWithBranchOrCom.CompanyCdSeq,
                        UkeEigCdSeq = itemBusTypeWithBranchOrCom.EigyoCdSeq,
                        SyuKoYmd = strDate,
                        NumberOfVehicle = 0
                    };
                    var dateFillter = new List<NumberVehicleOfBusUnAsign>();
                    dateFillter = (from data in busHiringDataSource
                                   where data.SyuKoYmd.CompareTo(strDate) <= 0
                                  && data.KikYmd.CompareTo(strDate) >= 0
                                 && (groupOption == GroupMode.Branch ? data.UkeEigCdSeq == itemBusTypeWithBranchOrCom.EigyoCdSeq :
                                 data.CompanyCdSeq == itemBusTypeWithBranchOrCom.CompanyCdSeq)
                                   select (data)).ToList();
                    var dataGroup = new List<NumberVehicleOfBusUnAsign>();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        dataGroup = (from data in dateFillter
                                     select new NumberVehicleOfBusUnAsign()
                                     {
                                         SyuKoYmd = strDate,
                                         UkeEigCdSeq = itemBusTypeWithBranchOrCom.EigyoCdSeq,
                                         CompanyCdSeq = itemBusTypeWithBranchOrCom.CompanyCdSeq,
                                         NumberOfVehicle = dateFillter.Count()
                                     }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }

        public async Task<List<BusTypeItemDataReport>> FillterNumberOfBusTypeByGroupOption(List<BusTypeItemDataReport> dataSource, GroupMode groupOption)
        {
            var result = new List<BusTypeItemDataReport>();
            try
            {
                if (groupOption == GroupMode.All)
                {
                    result = (from av in dataSource
                              group av by new { av.SyaSyuCdSeq, av.SyaSyuNm } into kq
                              select new BusTypeItemDataReport()
                              {
                                  SyaSyuCdSeq = kq.Key.SyaSyuCdSeq,
                                  SyaSyuNm = kq.Key.SyaSyuNm,
                                  CountNumberOfVehicle = kq.Count()
                              }).ToList();
                }
                else if (groupOption == GroupMode.Branch)
                {
                    result = (from av in dataSource
                              group av by new { av.EigyoCdSeq, av.Ei_RyakuNm, av.CompanyCdSeq, av.SyaSyuCdSeq, av.SyaSyuNm } into kq
                              select new BusTypeItemDataReport()
                              {
                                  EigyoCdSeq = kq.Key.EigyoCdSeq,
                                  Ei_RyakuNm = kq.Key.Ei_RyakuNm,
                                  CompanyCdSeq = kq.Key.CompanyCdSeq,
                                  CountNumberOfVehicle = kq.Count(),
                                  SyaSyuCdSeq = kq.Key.SyaSyuCdSeq,
                                  SyaSyuNm = kq.Key.SyaSyuNm,
                              }).ToList().OrderBy(x => x.EigyoCdSeq).ToList();
                }
                else if (groupOption == GroupMode.Company)
                {
                    result = (from av in dataSource
                              group av by new { av.CompanyCdSeq, av.COM_RyakuNm, av.SyaSyuCdSeq, av.SyaSyuNm } into kq
                              select new BusTypeItemDataReport()
                              {
                                  CompanyCdSeq = kq.Key.CompanyCdSeq,
                                  COM_RyakuNm = kq.Key.COM_RyakuNm,
                                  CountNumberOfVehicle = kq.Count(),
                                  SyaSyuCdSeq = kq.Key.SyaSyuCdSeq,
                                  SyaSyuNm = kq.Key.SyaSyuNm
                              }).ToList().OrderBy(x => x.CompanyCdSeq).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return result;
            }
        }

        #endregion

        #region Load Data Report Group 
        public async Task<List<BusTypeListReportGroupPDF>> GetPDFDataReportGroup(BusTypeListData searchParams)
        {
            try
            {
                var data = new List<BusTypeListReportGroupPDF>();
                var page = 1;
                var reportDataBodyList = new List<BusTypeReportGroupBody>();
                var busTypesDataList = new List<BusTypeItemDataReport>();
                var busTypeList = new List<BusTypeItemDataReport>();
                var busTypeDetailDataList = new List<BusTypeDetailDataReport>();
                var itemPageInfo = new OrtherInfo();
                //1.Get busType List Group 
                busTypesDataList = await GetBusTypeListReport(searchParams);
                busTypeList = await GroupDataByGroupOption(busTypesDataList, GroupMode.All);
                //2.get bustype detail 
                busTypeDetailDataList = await GetBusTypeDetailData(searchParams);
                //3. Get bus unsign + hirring
                var busUnAsignDataSource = await GetNumberOfBusUnAsignDataReport(searchParams, BusMode.BusUnAsign);
                var sumBusUnAsignDataList = await FillterBusUnAsignDataByKataKbn(busUnAsignDataSource, searchParams, 0, ShowMode.Report);
                //3.Get BusHiring
                var busHiringDataSource = await GetNumberOfBusUnAsignDataReport(searchParams, BusMode.BusHiring);
                var sumBusHiringDataList = await FillterBusHiringData(busHiringDataSource, searchParams);
                //Get Source data tkd_Shuri 
                var busRepairDataSource = await GetBusRepairReportData(searchParams);
                var henSyaDataSource = await GetHenSyaReportData(searchParams);
                var reportHeader = SetDataHeader(searchParams);
                //Get yyksyu data => Hiring 
                var tkdYykSyuDataList = _dbContext.TkdYykSyu.Where(x => busHiringDataSource.Select(y => y.UkeNo).ToArray().Contains(x.UkeNo)
                && busHiringDataSource.Select(y => y.UnkRen).ToArray().Contains(x.UnkRen)
                && x.SiyoKbn == 1).ToList();
                //set orther info
                itemPageInfo = SetItemPageInfo(searchParams);
                foreach (var item in busTypeList)
                {
                    var itemReportBody = new BusTypeReportGroupBody();
                    itemReportBody.HeatderTable = reportHeader;
                    var busKSKbn02Row = FillterBusKSKbn02(busTypeDetailDataList, item, searchParams);
                    var busHaiSKbnRow = FillterBusHaiSKbn02(busTypeDetailDataList, item, searchParams);
                    var busRepairRow = FillterBusRepair(busRepairDataSource, item, searchParams);
                    var busTotalRow = FillterTotalBus(henSyaDataSource, item, searchParams);
                    var busHiringRow = FillterBusUnAsign(busHiringDataSource, item, searchParams, BusMode.BusHiring, tkdYykSyuDataList);
                    var busUnAsignRow = FillterBusUnAsign(busUnAsignDataSource, item, searchParams, BusMode.BusUnAsign, tkdYykSyuDataList);
                    var busFee = FillterBusFee(busKSKbn02Row, busHaiSKbnRow, busRepairRow, busTotalRow, searchParams);
                    /* var busHigaeri = FillterBusRunMode(busTypeDetailDataList, item, searchParams, BusRunModeReport.HiGaeRi);
                     var busTomaride = FillterBusRunMode(busTypeDetailDataList, item, searchParams, BusRunModeReport.TomariDe);
                     var busHakuchu = FillterBusRunMode(busTypeDetailDataList, item, searchParams, BusRunModeReport.HaKuChu);
                     var busHakuki = FillterBusRunMode(busTypeDetailDataList, item, searchParams, BusRunModeReport.HaKuKi);
                     var busYakode = FillterBusRunMode(busTypeDetailDataList, item, searchParams, BusRunModeReport.YaKoDe);
                     var busYakoki = FillterBusRunMode(busTypeDetailDataList, item, searchParams, BusRunModeReport.YaKoKi);
                     var busYakochu = FillterBusRunMode(busTypeDetailDataList, item, searchParams, BusRunModeReport.YaKoChu);*/
                    var dataBusRunModeAll = FillterBusRunModeReport(busTypeDetailDataList, item, searchParams);
                    ///Parse List string to list object
                    itemReportBody.BusKSKbn02 = SetDataBodyRow(busKSKbn02Row, item);
                    itemReportBody.BusHaiSKbn02 = SetDataBodyRow(busHaiSKbnRow, item);
                    itemReportBody.BusRepair = SetDataBodyRow(busRepairRow, item);
                    itemReportBody.BusFee = SetDataBodyRow(busFee, item);
                    itemReportBody.BusHiring = SetDataBodyRow(busHiringRow, item);
                    itemReportBody.BusUnAsign = SetDataBodyRow(busUnAsignRow, item);
                    itemReportBody.BusTotal = SetDataBodyRow(busTotalRow, item);
                    //bus run mode
                    itemReportBody.HiGaeRi = SetDataBodyRow(dataBusRunModeAll.BusHigaeri.OrderBy(x => x.SyuKoYmd).ToList(), item);
                    itemReportBody.TomariDe = SetDataBodyRow(dataBusRunModeAll.BusTomaride.OrderBy(x => x.SyuKoYmd).ToList(), item);
                    itemReportBody.HaKuChu = SetDataBodyRow(dataBusRunModeAll.BusHakuchu.OrderBy(x => x.SyuKoYmd).ToList(), item);
                    itemReportBody.HaKuKi = SetDataBodyRow(dataBusRunModeAll.BusHakuki.OrderBy(x => x.SyuKoYmd).ToList(), item);
                    itemReportBody.YaKoDe = SetDataBodyRow(dataBusRunModeAll.BusYakode.OrderBy(x => x.SyuKoYmd).ToList(), item);
                    itemReportBody.YaKoKi = SetDataBodyRow(dataBusRunModeAll.BusYakoki.OrderBy(x => x.SyuKoYmd).ToList(), item);
                    itemReportBody.YaKoChu = SetDataBodyRow(dataBusRunModeAll.BusYakochu.OrderBy(x => x.SyuKoYmd).ToList(), item);
                    itemReportBody.SyaSyuName = item.SyaSyuNm;
                    //body add 
                    reportDataBodyList.Add(itemReportBody);
                }
                //Add Bus Type Dont Now
                var busTypeUnKnowitem = InsertBusUnKnow(busUnAsignDataSource, busHiringDataSource, searchParams);
                reportDataBodyList.Add(busTypeUnKnowitem);
                OnSetDataPerReportGroupPage(data, reportDataBodyList, itemPageInfo, ref page);
                data.ForEach(e =>
                {
                    e.TotalPage = page - 1;
                });
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        private void OnSetDataPerReportGroupPage(
            List<BusTypeListReportGroupPDF> listData
          , List<BusTypeReportGroupBody> listBody
          , dynamic item
          , ref int page)
        {
            try
            {
                List<BusTypeReportGroupBody> listTemp = new List<BusTypeReportGroupBody>();
                listTemp = listBody;
                var itemPerPage = 3;
                if (listTemp.Count > itemPerPage)
                {
                    var count = Math.Ceiling(listTemp.Count * 1.0 / itemPerPage);
                    for (int i = 0; i < count; i++)
                    {
                        var onePage = new BusTypeListReportGroupPDF();
                        var listPerPage = listTemp.Skip(i * itemPerPage).Take(itemPerPage).ToList();
                        SetDataReportGroup(onePage, listPerPage, page, item, itemPerPage);
                        listData.Add(onePage);
                        page++;
                    }
                }
                else
                {
                    var onePage = new BusTypeListReportGroupPDF();
                    SetDataReportGroup(onePage, listTemp, page, item, itemPerPage);
                    listData.Add(onePage);
                    page++;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

        }
        private void SetDataReportGroup(
       BusTypeListReportGroupPDF onePage,
       List<BusTypeReportGroupBody> listPerPage,
       int page,
       dynamic item,
       int itemPerPage)
        {
            try
            {
                while (listPerPage.Count < itemPerPage)
                {
                    listPerPage.Add(new BusTypeReportGroupBody());
                }
                onePage.OrtherInfo = item;
                onePage.ReportGroupBodyList = listPerPage;
                onePage.PageNumber = page;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

        }

        //Bus KSKbn
        private List<BusTypeDetailDataReport> FillterBusKSKbn02(List<BusTypeDetailDataReport> dataSource, BusTypeItemDataReport item, BusTypeListData paramSearch)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyuKoYmd = strDate,
                        SyaSyuCdSeq = item.SyaSyuCdSeq
                    };
                    var dateFillter = new List<BusTypeDetailDataReport>();
                    dateFillter = (from data in dataSource
                                   where data.SyuKoYmd.CompareTo(strDate) <= 0
                                  && data.KikYmd.CompareTo(strDate) >= 0
                                  && data.KSKbn == 2 && data.HaiSKbn != 2 && data.HaiSSryCdSeq != 0
                                  && data.SyaSyuCdSeq == item.SyaSyuCdSeq
                                   select (data)).ToList();
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        dataGroup = (from data in dateFillter
                                     select new BusTypeDetailDataReport()
                                     {
                                         SyaSyuCdSeq = item.SyaSyuCdSeq,
                                         SyuKoYmd = strDate,
                                         Number = dateFillter.Count()
                                     }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        //Bus HaiSKbn
        private List<BusTypeDetailDataReport> FillterBusHaiSKbn02(List<BusTypeDetailDataReport> dataSource, BusTypeItemDataReport item, BusTypeListData paramSearch)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyuKoYmd = strDate,
                        SyaSyuCdSeq = item.SyaSyuCdSeq
                    };
                    var dateFillter = new List<BusTypeDetailDataReport>();
                    dateFillter = (from data in dataSource
                                   where data.SyuKoYmd.CompareTo(strDate) <= 0
                                  && data.KikYmd.CompareTo(strDate) >= 0
                                  && data.HaiSKbn == 2 && data.HaiSSryCdSeq != 0
                                  && data.SyaSyuCdSeq == item.SyaSyuCdSeq
                                   select (data)).ToList();
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        dataGroup = (from data in dateFillter
                                     select new BusTypeDetailDataReport()
                                     {
                                         SyaSyuCdSeq = item.SyaSyuCdSeq,
                                         SyuKoYmd = strDate,
                                         Number = dateFillter.Count()
                                     }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        //Bus Repair 
        private List<BusTypeDetailDataReport> FillterBusRepair(List<BusRepairDataSource> dataSource, BusTypeItemDataReport item, BusTypeListData paramSearch)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyuKoYmd = strDate,
                        SyaSyuCdSeq = item.SyaSyuCdSeq
                    };
                    var dateFillter = new List<BusRepairDataSource>();
                    dateFillter = (from data in dataSource
                                   where data.ShuriSYmd.CompareTo(strDate) <= 0
                                  && data.ShuriEYmd.CompareTo(strDate) >= 0
                                  && data.SyaSyuCdSeq == item.SyaSyuCdSeq
                                   select (data)).ToList();
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        var dataGroupSyaRyo = (from data in dateFillter
                                               group data by new { data.SyaRyoCdSeq } into kq
                                               select new BusTypeDetailDataReport()
                                               {
                                                   SyaSyuCdSeq = item.SyaSyuCdSeq,
                                                   SyuKoYmd = strDate,
                                                   Number = kq.Count()
                                               }).ToList();
                        dataGroup = (from data in dataGroupSyaRyo
                                     select new BusTypeDetailDataReport()
                                     {
                                         SyaSyuCdSeq = item.SyaSyuCdSeq,
                                         SyuKoYmd = strDate,
                                         Number = dataGroupSyaRyo.Count()
                                     }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        // Bus Fee
        private List<BusTypeDetailDataReport> FillterTotalBus(List<HenSyaDataSource> dataSource, BusTypeItemDataReport item, BusTypeListData paramSearch)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyuKoYmd = strDate,
                        SyaSyuCdSeq = item.SyaSyuCdSeq
                    };
                    var dateFillter = new List<HenSyaDataSource>();
                    dateFillter = (from data in dataSource
                                   where data.StaYmd.CompareTo(strDate) <= 0
                                  && data.EndYmd.CompareTo(strDate) >= 0
                                   && data.SyaSyuCdSeq == item.SyaSyuCdSeq
                                   select (data)).ToList();
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        dataGroup = (from data in dateFillter
                                     select new BusTypeDetailDataReport()
                                     {
                                         SyaSyuCdSeq = item.SyaSyuCdSeq,
                                         SyuKoYmd = strDate,
                                         Number = dateFillter.Count()
                                     }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        //Bus Hiring  or Bus UnAsign
        private List<BusTypeDetailDataReport> FillterBusUnAsign(List<NumberVehicleOfBusUnAsign> dataSource, BusTypeItemDataReport item, BusTypeListData paramSearch, BusMode busMode, List<TkdYykSyu> tkdYykSyuDataList)
        {
            try
            {

                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyuKoYmd = strDate,
                        SyaSyuCdSeq = item.SyaSyuCdSeq
                    };
                    var dateFillter = new List<NumberVehicleOfBusUnAsign>();
                    dateFillter = SelectDataBusUnAsignOrHiringByCondition(dataSource, busMode, item, strDate, tkdYykSyuDataList);
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    if (dateFillter != null && dateFillter.Any())
                    {
                        dataGroup = (from data in dateFillter
                                     select new BusTypeDetailDataReport()
                                     {
                                         SyaSyuCdSeq = item.SyaSyuCdSeq,
                                         SyuKoYmd = strDate,
                                         Number = dateFillter.Count()
                                     }).ToList();

                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        //Bus Total
        private List<BusTypeDetailDataReport> FillterBusFee(List<BusTypeDetailDataReport> busKSKbn02DataList,
            List<BusTypeDetailDataReport> busHaiSKbn02DataList, List<BusTypeDetailDataReport> busRepairDataList,
            List<BusTypeDetailDataReport> busTotalList, BusTypeListData paramSearch)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var strDate = date.ToString("yyyyMMdd");
                    int totalNumberBus = busTotalList.Where(x => x.SyuKoYmd == strDate).FirstOrDefault().Number;
                    int numberBusKSKbn = busKSKbn02DataList.Where(x => x.SyuKoYmd == strDate).FirstOrDefault().Number;
                    int numberBusHaiSKbn = busHaiSKbn02DataList.Where(x => x.SyuKoYmd == strDate).FirstOrDefault().Number;
                    int numberBusRepair = busRepairDataList.Where(x => x.SyuKoYmd == strDate).FirstOrDefault().Number;
                    int numberBusFee = totalNumberBus - (numberBusKSKbn + numberBusHaiSKbn + numberBusRepair);
                    var dataByDay = new BusTypeDetailDataReport()
                    {
                        Number = numberBusFee,
                        SyuKoYmd = strDate
                    };
                    result.Add(dataByDay);
                }
                //result.ForEach(x => x.KataKbn = kataKbn);
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        //BusRunMode
        private List<BusTypeDetailDataReport> FillterBusRunMode(List<BusTypeDetailDataReport> dataSource, BusTypeItemDataReport item, BusTypeListData paramSearch, BusRunModeReport busRunModeReport)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                int numberBus = 0;
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    numberBus = 0;
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,

                        SyuKoYmd = strDate,
                        SyaSyuCdSeq = item.SyaSyuCdSeq
                    };
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    var dataBySyaSyuCdSeq = dataSource.Where(x => x.SyaSyuCdSeq == item.SyaSyuCdSeq).ToList();
                    var dateFillter = CheckDataByCondition(dataBySyaSyuCdSeq, busRunModeReport, strDate);
                    if (dateFillter != null && dateFillter.Any())
                    {
                        dataGroup = (from data in dateFillter
                                     select new BusTypeDetailDataReport()
                                     {
                                         SyaSyuCdSeq = item.SyaSyuCdSeq,
                                         SyuKoYmd = strDate,
                                         Number = dateFillter.Count()
                                     }).ToList();
                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        private BusRunMode FillterBusRunModeReport(List<BusTypeDetailDataReport> dataSource, BusTypeItemDataReport item, BusTypeListData paramSearch)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                int numberBus = 0;
                var result = new List<BusTypeDetailDataReport>();
                var resultNew = new BusRunMode();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    int numberHigaeri = 0;
                    int numberTomaride = 0;
                    int numberHakuchu = 0;
                    int numberHakuki = 0;
                    int numberYakode = 0;
                    int numberYakoki = 0;
                    int numberYakochu = 0;
                    numberBus = 0;
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyuKoYmd = strDate,
                        SyaSyuCdSeq = item.SyaSyuCdSeq
                    };
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    var dataBySyaSyuCdSeq = dataSource.Where(x => x.SyaSyuCdSeq == item.SyaSyuCdSeq).ToList();
                    // checked
                    if (dataBySyaSyuCdSeq != null && dataBySyaSyuCdSeq.Any() && !string.IsNullOrEmpty(strDate) && !string.IsNullOrWhiteSpace(strDate))
                    {
                        var dataByDayCurrent = (from data in dataBySyaSyuCdSeq
                                                where data.SyuKoYmd.CompareTo(strDate) <= 0
                                               && data.KikYmd.CompareTo(strDate) >= 0
                                                select (data)).ToList();
                        if (dataByDayCurrent != null && dataByDayCurrent.Any())
                        {
                            foreach (var dataItem in dataByDayCurrent)
                            {
                                //HiGaeRi
                                if (dataItem.UN_SyukoYmd.CompareTo(dataItem.UN_KikYmd) == 0)
                                {
                                    numberHigaeri += 1;
                                }
                                else
                                {
                                    if (dataItem.UN_UnkoJKbn == 3 || dataItem.UN_UnkoJKbn == 4)
                                    {
                                        /*if(dataItem.UN_SyukoYmd.CompareTo(dataItem.UN_KikYmd) != 0)
                                        {*/
                                        if (dataItem.UN_SyukoYmd.CompareTo(strDate) == 0)
                                        {
                                            numberYakode += 1;
                                        }
                                        else if (dataItem.UN_KikYmd.CompareTo(strDate) == 0)
                                        {
                                            numberYakoki += 1;
                                        }
                                        else if (dataItem.UN_SyukoYmd.CompareTo(strDate) != 0 && dataItem.UN_KikYmd.CompareTo(strDate) != 0)
                                        {
                                            numberYakochu += 1;
                                        }
                                        else
                                        {
                                            //99（なし）
                                        }
                                        /*  }    */
                                    }
                                    else
                                    {
                                        if(dataItem.UN_SyukoYmd.CompareTo(strDate) == 0)
                                        {
                                            numberTomaride += 1;
                                        }
                                        else if(dataItem.UN_SyukoYmd.CompareTo(strDate) != 0
                                            && dataItem.UN_KikYmd.CompareTo(strDate) != 0)
                                        {
                                            numberHakuchu += 1;
                                        }
                                        else if(dataItem.UN_HaiSYmd.CompareTo(dataItem.UN_TouYmd) != 0
                                            && dataItem.UN_KikYmd.CompareTo(strDate) == 0)
                                        {
                                            numberHakuki += 1;
                                        }
                                        else
                                        {
                                            //99（なし）
                                        }
                                    }
                                }
                            }
                            resultNew.BusHigaeri.Add(new BusTypeDetailDataReport()
                            {
                                Number = numberHigaeri,
                                SyuKoYmd = strDate,
                                SyaSyuCdSeq = item.SyaSyuCdSeq
                            });
                            resultNew.BusTomaride.Add(new BusTypeDetailDataReport()
                            {
                                Number = numberTomaride,
                                SyuKoYmd = strDate,
                                SyaSyuCdSeq = item.SyaSyuCdSeq
                            });
                            resultNew.BusHakuchu.Add(new BusTypeDetailDataReport()
                            {
                                Number = numberHakuchu,
                                SyuKoYmd = strDate,
                                SyaSyuCdSeq = item.SyaSyuCdSeq

                            });
                            resultNew.BusHakuki.Add(new BusTypeDetailDataReport()
                            {
                                Number = numberHakuki,
                                SyuKoYmd = strDate,
                                SyaSyuCdSeq = item.SyaSyuCdSeq
                            });
                            resultNew.BusYakode.Add(new BusTypeDetailDataReport()
                            {
                                Number = numberYakode,
                                SyuKoYmd = strDate,
                                SyaSyuCdSeq = item.SyaSyuCdSeq
                            });
                            resultNew.BusYakoki.Add(new BusTypeDetailDataReport()
                            {
                                Number = numberYakoki,
                                SyuKoYmd = strDate,
                                SyaSyuCdSeq = item.SyaSyuCdSeq
                            });
                            resultNew.BusYakochu.Add(new BusTypeDetailDataReport()
                            {
                                Number = numberYakochu,
                                SyuKoYmd = strDate,
                                SyaSyuCdSeq = item.SyaSyuCdSeq
                            });
                        }
                        else
                        {
                            resultNew.BusHigaeri.Add(dataNull);
                            resultNew.BusTomaride.Add(dataNull);
                            resultNew.BusHakuchu.Add(dataNull);
                            resultNew.BusHakuki.Add(dataNull);
                            resultNew.BusYakode.Add(dataNull);
                            resultNew.BusYakoki.Add(dataNull);
                            resultNew.BusYakochu.Add(dataNull);
                        }
                    }
                    else
                    {
                    }
                }
                return resultNew;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        private List<BusTypeDetailDataReport> CheckDataByCondition(List<BusTypeDetailDataReport> dataSource, BusRunModeReport busRunMode, string strDate)
        {
            try
            {
                var result = new List<BusTypeDetailDataReport>();
                if (dataSource != null && dataSource.Any() && !string.IsNullOrEmpty(strDate) && !string.IsNullOrWhiteSpace(strDate))
                {
                    if (busRunMode == BusRunModeReport.HiGaeRi)
                    {
                        result = (from data in dataSource
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && data.UN_SyukoYmd.CompareTo(data.UN_KikYmd) == 0
                                  select (data)).ToList();
                    }
                    else if (busRunMode == BusRunModeReport.TomariDe)
                    {
                        result = (from data in dataSource
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && data.UN_UnkoJKbn != 3
                                 && data.UN_UnkoJKbn != 4
                                 && data.UN_SyukoYmd.CompareTo(data.UN_KikYmd) != 0
                                 && data.UN_SyukoYmd.CompareTo(strDate) == 0
                                  select (data)).ToList();
                    }
                    else if (busRunMode == BusRunModeReport.HaKuChu)
                    {
                        result = (from data in dataSource
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && data.UN_UnkoJKbn != 3
                                 && data.UN_UnkoJKbn != 4
                                 && data.UN_SyukoYmd.CompareTo(data.UN_KikYmd) != 0
                                 && data.UN_SyukoYmd.CompareTo(strDate) != 0
                                  && data.UN_KikYmd.CompareTo(strDate) != 0
                                  select (data)).ToList();
                    }
                    else if (busRunMode == BusRunModeReport.HaKuKi)
                    {
                        result = (from data in dataSource
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && data.UN_UnkoJKbn != 3
                                 && data.UN_UnkoJKbn != 4
                                 && data.UN_HaiSYmd.CompareTo(data.UN_TouYmd) != 0
                                 && data.UN_SyukoYmd.CompareTo(strDate) == 0
                                  select (data)).ToList();
                    }
                    else if (busRunMode == BusRunModeReport.YaKoDe)
                    {
                        result = (from data in dataSource
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && (data.UN_UnkoJKbn == 3 || data.UN_UnkoJKbn == 4)
                                 && data.UN_SyukoYmd.CompareTo(data.UN_KikYmd) != 0
                                 && data.UN_SyukoYmd.CompareTo(strDate) == 0
                                  select (data)).ToList();
                    }
                    else if (busRunMode == BusRunModeReport.YaKoKi)
                    {
                        result = (from data in dataSource
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && (data.UN_UnkoJKbn == 3 || data.UN_UnkoJKbn == 4)
                                 && data.UN_SyukoYmd.CompareTo(data.UN_KikYmd) != 0
                                 && data.UN_KikYmd.CompareTo(strDate) == 0
                                  select (data)).ToList();
                    }
                    else if (busRunMode == BusRunModeReport.YaKoChu)
                    {
                        result = (from data in dataSource
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && (data.UN_UnkoJKbn == 3 || data.UN_UnkoJKbn == 4)
                                 && data.UN_SyukoYmd.CompareTo(data.UN_KikYmd) != 0
                                 && data.UN_SyukoYmd.CompareTo(strDate) != 0
                                   && data.UN_KikYmd.CompareTo(strDate) != 0
                                  select (data)).ToList();
                    }
                }
                else
                {
                    return null;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }
        //Bus Type Not UnKnow
        private BusTypeReportGroupBody InsertBusUnKnow(List<NumberVehicleOfBusUnAsign> busUnAsignList, List<NumberVehicleOfBusUnAsign> busHiringList, BusTypeListData paramSearch)
        {
            var busUnKnow = new BusTypeReportGroupBody();
            try
            {
                var busHiringUnKnowData = FillterBusUnKnow(busHiringList, BusMode.BusHiring, paramSearch);
                var busUnAsignUnKnowData = FillterBusUnKnow(busUnAsignList, BusMode.BusUnAsign, paramSearch);
                busUnKnow.BusUnAsign = SetDataBodyRow(busUnAsignUnKnowData, new BusTypeItemDataReport());
                busUnKnow.BusHiring = SetDataBodyRow(busUnAsignUnKnowData, new BusTypeItemDataReport());
                return busUnKnow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return busUnKnow;
            }
        }
        private List<BusTypeDetailDataReport> FillterBusUnKnow(List<NumberVehicleOfBusUnAsign> dataSource, BusMode busMode, BusTypeListData paramSearch)
        {
            try
            {
                DateTime dateStart = paramSearch.StartDate;
                DateTime dateEnd = dateStart.AddDays(paramSearch.numberDay);
                int numberBus = 0;
                var result = new List<BusTypeDetailDataReport>();
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    numberBus = 0;
                    var strDate = date.ToString("yyyyMMdd");
                    var dataNull = new BusTypeDetailDataReport()
                    {
                        Number = 0,
                        SyuKoYmd = strDate,
                        SyaSyuCdSeq = 0
                    };
                    var dataGroup = new List<BusTypeDetailDataReport>();
                    var dateFillter = SelectDataBusUnKnowByCondition(dataSource, busMode, strDate);
                    if (dateFillter != null && dateFillter.Any())
                    {
                        dataGroup = (from data in dateFillter
                                     select new BusTypeDetailDataReport()
                                     {
                                         SyaSyuCdSeq = 0,
                                         SyuKoYmd = strDate,
                                         Number = dateFillter.Count()
                                     }).ToList();
                        if (dataGroup != null && dataGroup.Any())
                        {
                            result.Add(dataGroup.FirstOrDefault());
                        }
                        else
                        {
                            result.Add(dataNull);
                        }
                    }
                    else
                    {
                        result.Add(dataNull);
                    }
                }
                return result.OrderBy(x => x.SyuKoYmd).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }


        }
        private List<NumberVehicleOfBusUnAsign> SelectDataBusUnKnowByCondition(List<NumberVehicleOfBusUnAsign> dataSource, BusMode busMode, string strDate)
        {
            try
            {
                var result = new List<NumberVehicleOfBusUnAsign>();
                if (busMode == BusMode.BusUnAsign)
                {
                    result = (from data in dataSource
                              where data.SyuKoYmd.CompareTo(strDate) <= 0
                             && data.KikYmd.CompareTo(strDate) >= 0
                             && data.YYSYU_SyaSyuCdSeq == 0
                              select (data)).ToList();

                }
                else if (busMode == BusMode.BusHiring)
                {
                    result = (from data in dataSource
                              join YYSYU in _dbContext.TkdYykSyu
                              on new { H1 = data.UkeNo, H2 = data.UnkRen, H3 = data.SyaSyuRen, H4 = 1 }
                              equals new { H1 = YYSYU.UkeNo, H2 = (int)YYSYU.UnkRen, H3 = (int)YYSYU.SyaSyuRen, H4 = (int)YYSYU.SiyoKbn }
                              where data.SyuKoYmd.CompareTo(strDate) <= 0
                             && data.KikYmd.CompareTo(strDate) >= 0
                             && YYSYU.SyaSyuCdSeq == 0
                              select (data)).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        private List<NumberVehicleOfBusUnAsign> SelectDataBusUnAsignOrHiringByCondition(List<NumberVehicleOfBusUnAsign> dataSource, BusMode busMode, BusTypeItemDataReport item, string strDate, List<TkdYykSyu> tkdYykSyuDataList)
        {
            try
            {
                if (dataSource != null && dataSource.Any() && !string.IsNullOrEmpty(strDate) && !string.IsNullOrWhiteSpace(strDate))
                {
                    var result = new List<NumberVehicleOfBusUnAsign>();
                    if (busMode == BusMode.BusUnAsign)
                    {
                        result = (from data in dataSource
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && data.YYSYU_SyaSyuCdSeq == item.SyaSyuCdSeq
                                  select (data)).ToList();

                    }
                    else if (busMode == BusMode.BusHiring)
                    {
                        result = (from data in dataSource
                                  join YYSYU in tkdYykSyuDataList
                                  on new { H1 = data.UkeNo, H2 = data.UnkRen, H3 = data.SyaSyuRen, H4 = 1 }
                                  equals new { H1 = YYSYU.UkeNo, H2 = (int)YYSYU.UnkRen, H3 = (int)YYSYU.SyaSyuRen, H4 = (int)YYSYU.SiyoKbn }
                                  where data.SyuKoYmd.CompareTo(strDate) <= 0
                                 && data.KikYmd.CompareTo(strDate) >= 0
                                 && YYSYU.SyaSyuCdSeq == item.SyaSyuCdSeq
                                  select (data)).ToList();
                    }
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        #endregion

        #region  Load Data Report Normal 

        public async Task<List<BusTypeListReportPDF>> GetPDFData(BusTypeListData searchParams)
        {
            var data = new List<BusTypeListReportPDF>();
            try
            {
                var page = 1;
                var busTypesDataList = new List<BusTypeItemDataReport>();
                var busTypeList = new List<BusTypeItemDataReport>();
                var busUnAsignDataSource = new List<NumberVehicleOfBusUnAsign>();
                var busHiringDataSource = new List<NumberVehicleOfBusUnAsign>();
                var busTypeDataRow = new List<BusTypeDetailDataReport>();
                var busTypeDataRowList = new List<BusTypeDetailDataReport>();
                var busTypeDetailDataList = new List<BusTypeDetailDataReport>();
                var sumBusUnAsignDataList = new List<NumberVehicleOfBusUnAsign>();
                var sumBusHiringDataList = new List<NumberVehicleOfBusUnAsign>();
                var sumBusTypeNormal = new List<BusTypeDetailDataReport>();
                var sumEmployeeDriver = new List<BusTypeDetailDataReport>();
                var sumEmployeeGui = new List<BusTypeDetailDataReport>();
                var reportFooter = new BusTypeListReportFooter();
                var reportBodyList = new List<BusTypeReportBody>();
                var reportHeader = new BusTypeReportHeatder();
                var itemPageInfo = new OrtherInfo();
                //0. set data orther 
                itemPageInfo = SetItemPageInfo(searchParams);
                //1. get data Bustype normal 
                busTypesDataList = await GetBusTypeListReport(searchParams);
                busTypeList = await GroupDataByGroupOption(busTypesDataList, GroupMode.All);
                busTypeDetailDataList = await GetBusTypeDetailData(searchParams);
                //2.Get Sum all BusUnsign
                busUnAsignDataSource = await GetNumberOfBusUnAsignDataReport(searchParams, BusMode.BusUnAsign);
                //return 1 list -> sum all by day
                sumBusUnAsignDataList = await FillterBusUnAsignDataByKataKbn(busUnAsignDataSource, searchParams, 0, ShowMode.Report);
                //3.Get BusHiring
                busHiringDataSource = await GetNumberOfBusUnAsignDataReport(searchParams, BusMode.BusHiring);
                sumBusHiringDataList = await FillterBusHiringData(busHiringDataSource, searchParams);
                //4.Total driver and Gui 
                sumEmployeeDriver = await FillterDataByEmployee(busTypeDetailDataList, searchParams, StaffPos.Driver, null, GroupMode.All);
                sumEmployeeGui = await FillterDataByEmployee(busTypeDetailDataList, searchParams, StaffPos.GuiSu, null, GroupMode.All);
                //5. Get numberof per BusType 
                var numberOfBusTypeByGroup = await FillterNumberOfBusTypeByGroupOption(busTypesDataList, GroupMode.All);
                //set data
                reportHeader = SetDataHeader(searchParams);
                foreach (var item in busTypeList)
                {
                    busTypeDataRow = await FillterDataByBusType(busTypeDetailDataList, searchParams, item);
                    var numberBusTypeBySyaSyuSeqItem = numberOfBusTypeByGroup.Where(x => x.SyaSyuCdSeq == item.SyaSyuCdSeq).FirstOrDefault();
                    var dataBodyRow = SetDataBodyRow(busTypeDataRow, numberBusTypeBySyaSyuSeqItem);
                    dataBodyRow.HeatderTable = reportHeader;
                    reportBodyList.Add(dataBodyRow);
                    busTypeDataRowList.AddRange(busTypeDataRow);
                }
                //4.SumBusTypeNormal 
                sumBusTypeNormal = await SumDataByKataKbn(busTypeDataRowList, 0, GroupMode.Branch, null, ShowMode.Report);
                //set data per page
                OnSetDataPerPage(data, reportBodyList, itemPageInfo, ref page);
                //Set data Footer
                reportFooter.SumBusTypeHiringList = SetDataFooterRow(sumBusHiringDataList, null, SumMode.SumBusHiring, null, null);
                reportFooter.SumBusTypeUnAsignList = SetDataFooterRow(sumBusUnAsignDataList, null, SumMode.SumBusUnAsign, null, null);
                reportFooter.SumBusTypeNormalList = SetDataFooterRow(null, sumBusTypeNormal, SumMode.SumBusNormal, sumBusUnAsignDataList, sumBusHiringDataList);
                reportFooter.SumDriverList = SetDataFooterRow(null, sumEmployeeDriver, SumMode.SumBusDriver, null, null);
                reportFooter.SumGuiSuList = SetDataFooterRow(null, sumEmployeeGui, SumMode.SumBusGuiSu, null, null);
                SetSumItemAllPage(data, ref page, reportFooter, itemPageInfo);
                data.ForEach(e =>
                {
                    e.TotalPage = page - 1;
                });
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return data;
            }
        }
        private OrtherInfo SetItemPageInfo(BusTypeListData searchParams)
        {
            var item = new OrtherInfo();
            try
            {
                item.CurrentDate = DateTime.Now.ToString("yyyyMMddHHmm");
                item.DateSearch = searchParams.StartDate.ToString("yyyyMMdd");
                item.SyainCd = searchParams.SyainCd;
                item.SyainNm = searchParams.SyainNm;
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return item;
            }
        }

        private void OnSetDataPerPage(
              List<BusTypeListReportPDF> listData
            , List<BusTypeReportBody> listBody
            , dynamic item
            , ref int page)
        {
            try
            {
                List<BusTypeReportBody> listTemp = new List<BusTypeReportBody>();
                listTemp = listBody;
                var itemPerPage = 40;
                if (listTemp.Count > itemPerPage)
                {
                    var count = Math.Ceiling(listTemp.Count * 1.0 / itemPerPage);
                    for (int i = 0; i < count; i++)
                    {
                        var onePage = new BusTypeListReportPDF();
                        var listPerPage = listTemp.Skip(i * itemPerPage).Take(itemPerPage).ToList();
                        SetData(onePage, listPerPage, page, item, itemPerPage);
                        listData.Add(onePage);
                        page++;
                    }
                }
                else
                {
                    var onePage = new BusTypeListReportPDF();
                    SetData(onePage, listTemp, page, item, itemPerPage);
                    listData.Add(onePage);
                    page++;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

        }

        private void SetData(
         BusTypeListReportPDF onePage,
         List<BusTypeReportBody> listPerPage,
         int page,
         dynamic item,
         int itemPerPage)
        {
            try
            {
                while (listPerPage.Count < itemPerPage)
                {
                    listPerPage.Add(new BusTypeReportBody());
                }
                onePage.OrtherInfo = item;
                onePage.ReportBodyList = listPerPage;
                onePage.PageNumber = page;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

        }
        private BusTypeReportBody SetDataBodyRow(List<BusTypeDetailDataReport> dataSource, BusTypeItemDataReport item)
        {
            var result = new BusTypeReportBody();
            try
            {
                if (dataSource != null && dataSource.Any())
                {
                    if (item != null)
                    {
                        result.BusTypeNm = item.SyaSyuNm ?? "";
                        result.NumberOfBusType = item.CountNumberOfVehicle;
                    }
                    result.CalValue01 = dataSource[0].Number;
                    result.CalValue02 = dataSource[1].Number;
                    result.CalValue03 = dataSource[2].Number;
                    result.CalValue04 = dataSource[3].Number;
                    result.CalValue05 = dataSource[4].Number;
                    result.CalValue06 = dataSource[5].Number;
                    result.CalValue07 = dataSource[6].Number;
                    result.CalValue08 = dataSource[7].Number;
                    result.CalValue09 = dataSource[8].Number;
                    result.CalValue10 = dataSource[9].Number;
                    result.CalValue11 = dataSource[10].Number;
                    result.CalValue12 = dataSource[11].Number;
                    result.CalValue13 = dataSource[12].Number;
                    result.CalValue14 = dataSource[13].Number;
                    result.CalValue15 = dataSource[14].Number;
                    result.CalValue16 = dataSource[15].Number;
                    result.CalValue17 = dataSource[16].Number;
                    result.CalValue18 = dataSource[17].Number;
                    result.CalValue19 = dataSource[18].Number;
                    result.CalValue20 = dataSource[19].Number;
                    result.CalValue21 = dataSource[20].Number;
                    result.CalValue22 = dataSource[21].Number;
                    result.CalValue23 = dataSource[22].Number;
                    result.CalValue24 = dataSource[23].Number;
                    result.CalValue25 = dataSource[24].Number;
                    result.CalValue26 = dataSource[25].Number;
                    result.CalValue27 = dataSource[26].Number;
                    result.CalValue28 = dataSource[27].Number;
                    result.CalValue29 = dataSource[28].Number;
                    result.CalValue30 = dataSource[29].Number;
                    result.CalValue31 = dataSource[30].Number;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return result;
            }
        }
        private List<string> SetDateListStr(BusTypeListData searchParams)
        {
            List<string> strDateList = new List<string>();
            try
            {
                CultureInfo ci = new CultureInfo("ja-JP");
                DateTime dateStart = searchParams.StartDate;
                DateTime dateEnd = dateStart.AddDays(searchParams.numberDay);
                for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                {
                    var dateTmp = date.ToString("dd日", ci) + " - " + date.ToString("(ddd)", ci);
                    strDateList.Add(dateTmp);
                }
                return strDateList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return strDateList;
            }
        }
        private BusTypeReportHeatder SetDataHeader(BusTypeListData searchParams)
        {
            var result = new BusTypeReportHeatder();
            try
            {
                var dateDataList = SetDateListStr(searchParams);
                if (dateDataList.Any())
                {
                    result.CalText01 = dateDataList[0];
                    result.CalText02 = dateDataList[1];
                    result.CalText03 = dateDataList[2];
                    result.CalText04 = dateDataList[3];
                    result.CalText05 = dateDataList[4];
                    result.CalText06 = dateDataList[5];
                    result.CalText07 = dateDataList[6];
                    result.CalText08 = dateDataList[7];
                    result.CalText09 = dateDataList[8];
                    result.CalText10 = dateDataList[9];
                    result.CalText11 = dateDataList[10];
                    result.CalText12 = dateDataList[11];
                    result.CalText13 = dateDataList[12];
                    result.CalText14 = dateDataList[13];
                    result.CalText15 = dateDataList[14];
                    result.CalText16 = dateDataList[15];
                    result.CalText17 = dateDataList[16];
                    result.CalText18 = dateDataList[17];
                    result.CalText19 = dateDataList[18];
                    result.CalText20 = dateDataList[19];
                    result.CalText21 = dateDataList[20];
                    result.CalText22 = dateDataList[21];
                    result.CalText23 = dateDataList[22];
                    result.CalText24 = dateDataList[23];
                    result.CalText25 = dateDataList[24];
                    result.CalText26 = dateDataList[25];
                    result.CalText27 = dateDataList[26];
                    result.CalText28 = dateDataList[27];
                    result.CalText29 = dateDataList[28];
                    result.CalText30 = dateDataList[29];
                    result.CalText31 = dateDataList[30];
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return result;
            }

        }
        private void SetSumItemAllPage(List<BusTypeListReportPDF> listData, ref int page, BusTypeListReportFooter dataFooter
            , OrtherInfo ortherInfo)
        {
            try
            {
                if (listData != null && listData.Any())
                {
                    var dataPageLast = listData.Last();
                    var dataRowLastBodyList = dataPageLast.ReportBodyList.Skip(35).Take(5).ToList();
                    bool isDataRowEmpty = true;
                    foreach (var item in dataRowLastBodyList)
                    {
                        if (!string.IsNullOrEmpty(item.BusTypeNm) && !string.IsNullOrWhiteSpace(item.BusTypeNm))
                        {
                            isDataRowEmpty = false;
                        }
                    }
                    if (isDataRowEmpty)
                    {
                        listData.Last().ReportBodyList[35] = dataFooter.SumBusTypeHiringList;
                        listData.Last().ReportBodyList[36] = dataFooter.SumBusTypeUnAsignList;
                        listData.Last().ReportBodyList[37] = dataFooter.SumBusTypeNormalList;
                        listData.Last().ReportBodyList[38] = dataFooter.SumDriverList;
                        listData.Last().ReportBodyList[39] = dataFooter.SumGuiSuList;
                    }
                    else
                    {
                        var onePage = new BusTypeListReportPDF();
                        var reportBodyListPerPage = new List<BusTypeReportBody>();
                        while (reportBodyListPerPage.Count < 35)
                        {
                            reportBodyListPerPage.Add(new BusTypeReportBody());
                        }
                        reportBodyListPerPage[35] = dataFooter.SumBusTypeHiringList;
                        reportBodyListPerPage[36] = dataFooter.SumBusTypeUnAsignList;
                        reportBodyListPerPage[37] = dataFooter.SumBusTypeNormalList;
                        reportBodyListPerPage[38] = dataFooter.SumDriverList;
                        reportBodyListPerPage[39] = dataFooter.SumGuiSuList;
                        onePage.OrtherInfo = ortherInfo;
                        onePage.ReportBodyList = reportBodyListPerPage;
                        onePage.PageNumber = page + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

        }

        private BusTypeReportBody SetDataFooterRow(List<NumberVehicleOfBusUnAsign> dataBusUnAsignList,
            List<BusTypeDetailDataReport> dataBusNormal, SumMode sumMode, List<NumberVehicleOfBusUnAsign> dataBusUnAsignSum, List<NumberVehicleOfBusUnAsign> dataBusHiringSum)
        {
            var result = new BusTypeReportBody();
            try
            {
                switch (sumMode)
                {
                    case SumMode.SumBusNormal:
                        result.BusTypeNm = " 合計 ";
                        break;
                    case SumMode.SumBusHiring:
                        result.BusTypeNm = " 傭車 ";
                        break;
                    case SumMode.SumBusUnAsign:
                        result.BusTypeNm = " 未仮車 ";
                        break;
                    case SumMode.SumBusDriver:
                        result.BusTypeNm = " 運転手数 ";
                        break;
                    case SumMode.SumBusGuiSu:
                        result.BusTypeNm = " ガイド数 ";
                        break;
                }
                if (dataBusUnAsignList != null && dataBusUnAsignList.Any()) //BusUnAsign
                {
                    result.CalValue01 = dataBusUnAsignList[0].NumberOfVehicle;
                    result.CalValue02 = dataBusUnAsignList[1].NumberOfVehicle;
                    result.CalValue03 = dataBusUnAsignList[2].NumberOfVehicle;
                    result.CalValue04 = dataBusUnAsignList[3].NumberOfVehicle;
                    result.CalValue05 = dataBusUnAsignList[4].NumberOfVehicle;
                    result.CalValue06 = dataBusUnAsignList[5].NumberOfVehicle;
                    result.CalValue07 = dataBusUnAsignList[6].NumberOfVehicle;
                    result.CalValue08 = dataBusUnAsignList[7].NumberOfVehicle;
                    result.CalValue09 = dataBusUnAsignList[8].NumberOfVehicle;
                    result.CalValue10 = dataBusUnAsignList[9].NumberOfVehicle;
                    result.CalValue11 = dataBusUnAsignList[10].NumberOfVehicle;
                    result.CalValue12 = dataBusUnAsignList[11].NumberOfVehicle;
                    result.CalValue13 = dataBusUnAsignList[12].NumberOfVehicle;
                    result.CalValue14 = dataBusUnAsignList[13].NumberOfVehicle;
                    result.CalValue15 = dataBusUnAsignList[14].NumberOfVehicle;
                    result.CalValue16 = dataBusUnAsignList[15].NumberOfVehicle;
                    result.CalValue17 = dataBusUnAsignList[16].NumberOfVehicle;
                    result.CalValue18 = dataBusUnAsignList[17].NumberOfVehicle;
                    result.CalValue19 = dataBusUnAsignList[18].NumberOfVehicle;
                    result.CalValue20 = dataBusUnAsignList[19].NumberOfVehicle;
                    result.CalValue21 = dataBusUnAsignList[20].NumberOfVehicle;
                    result.CalValue22 = dataBusUnAsignList[21].NumberOfVehicle;
                    result.CalValue23 = dataBusUnAsignList[22].NumberOfVehicle;
                    result.CalValue24 = dataBusUnAsignList[23].NumberOfVehicle;
                    result.CalValue25 = dataBusUnAsignList[24].NumberOfVehicle;
                    result.CalValue26 = dataBusUnAsignList[25].NumberOfVehicle;
                    result.CalValue27 = dataBusUnAsignList[26].NumberOfVehicle;
                    result.CalValue28 = dataBusUnAsignList[27].NumberOfVehicle;
                    result.CalValue29 = dataBusUnAsignList[28].NumberOfVehicle;
                    result.CalValue30 = dataBusUnAsignList[29].NumberOfVehicle;
                    result.CalValue31 = dataBusUnAsignList[30].NumberOfVehicle;
                }
                else if (dataBusNormal != null && dataBusNormal.Any())
                {
                    if (dataBusUnAsignSum != null && dataBusUnAsignSum.Any() && dataBusHiringSum != null && dataBusHiringSum.Any())
                    {
                        result.CalValue01 = dataBusNormal[0].Number + dataBusUnAsignSum[0].NumberOfVehicle + dataBusHiringSum[0].NumberOfVehicle;
                        result.CalValue02 = dataBusNormal[1].Number + dataBusUnAsignSum[1].NumberOfVehicle + dataBusHiringSum[1].NumberOfVehicle;
                        result.CalValue03 = dataBusNormal[2].Number + dataBusUnAsignSum[2].NumberOfVehicle + dataBusHiringSum[2].NumberOfVehicle;
                        result.CalValue04 = dataBusNormal[3].Number + dataBusUnAsignSum[3].NumberOfVehicle + dataBusHiringSum[3].NumberOfVehicle;
                        result.CalValue05 = dataBusNormal[4].Number + dataBusUnAsignSum[4].NumberOfVehicle + dataBusHiringSum[4].NumberOfVehicle;
                        result.CalValue06 = dataBusNormal[5].Number + dataBusUnAsignSum[5].NumberOfVehicle + dataBusHiringSum[5].NumberOfVehicle;
                        result.CalValue07 = dataBusNormal[6].Number + dataBusUnAsignSum[6].NumberOfVehicle + dataBusHiringSum[6].NumberOfVehicle;
                        result.CalValue08 = dataBusNormal[7].Number + dataBusUnAsignSum[7].NumberOfVehicle + dataBusHiringSum[7].NumberOfVehicle;
                        result.CalValue09 = dataBusNormal[8].Number + dataBusUnAsignSum[8].NumberOfVehicle + dataBusHiringSum[8].NumberOfVehicle;
                        result.CalValue10 = dataBusNormal[9].Number + dataBusUnAsignSum[9].NumberOfVehicle + dataBusHiringSum[9].NumberOfVehicle;
                        result.CalValue11 = dataBusNormal[10].Number + dataBusUnAsignSum[10].NumberOfVehicle + dataBusHiringSum[10].NumberOfVehicle;
                        result.CalValue12 = dataBusNormal[11].Number + dataBusUnAsignSum[11].NumberOfVehicle + dataBusHiringSum[11].NumberOfVehicle;
                        result.CalValue13 = dataBusNormal[12].Number + dataBusUnAsignSum[12].NumberOfVehicle + dataBusHiringSum[12].NumberOfVehicle;
                        result.CalValue14 = dataBusNormal[13].Number + dataBusUnAsignSum[13].NumberOfVehicle + dataBusHiringSum[13].NumberOfVehicle;
                        result.CalValue15 = dataBusNormal[14].Number + dataBusUnAsignSum[14].NumberOfVehicle + dataBusHiringSum[14].NumberOfVehicle;
                        result.CalValue16 = dataBusNormal[15].Number + dataBusUnAsignSum[15].NumberOfVehicle + dataBusHiringSum[15].NumberOfVehicle;
                        result.CalValue17 = dataBusNormal[16].Number + dataBusUnAsignSum[16].NumberOfVehicle + dataBusHiringSum[16].NumberOfVehicle;
                        result.CalValue18 = dataBusNormal[17].Number + dataBusUnAsignSum[17].NumberOfVehicle + dataBusHiringSum[17].NumberOfVehicle;
                        result.CalValue19 = dataBusNormal[18].Number + dataBusUnAsignSum[18].NumberOfVehicle + dataBusHiringSum[18].NumberOfVehicle;
                        result.CalValue20 = dataBusNormal[19].Number + dataBusUnAsignSum[19].NumberOfVehicle + dataBusHiringSum[19].NumberOfVehicle;
                        result.CalValue21 = dataBusNormal[20].Number + dataBusUnAsignSum[20].NumberOfVehicle + dataBusHiringSum[20].NumberOfVehicle;
                        result.CalValue22 = dataBusNormal[21].Number + dataBusUnAsignSum[21].NumberOfVehicle + dataBusHiringSum[21].NumberOfVehicle;
                        result.CalValue23 = dataBusNormal[22].Number + dataBusUnAsignSum[22].NumberOfVehicle + dataBusHiringSum[22].NumberOfVehicle;
                        result.CalValue24 = dataBusNormal[23].Number + dataBusUnAsignSum[23].NumberOfVehicle + dataBusHiringSum[23].NumberOfVehicle;
                        result.CalValue25 = dataBusNormal[24].Number + dataBusUnAsignSum[24].NumberOfVehicle + dataBusHiringSum[24].NumberOfVehicle;
                        result.CalValue26 = dataBusNormal[25].Number + dataBusUnAsignSum[25].NumberOfVehicle + dataBusHiringSum[25].NumberOfVehicle;
                        result.CalValue27 = dataBusNormal[26].Number + dataBusUnAsignSum[26].NumberOfVehicle + dataBusHiringSum[26].NumberOfVehicle;
                        result.CalValue28 = dataBusNormal[27].Number + dataBusUnAsignSum[27].NumberOfVehicle + dataBusHiringSum[27].NumberOfVehicle;
                        result.CalValue29 = dataBusNormal[28].Number + dataBusUnAsignSum[28].NumberOfVehicle + dataBusHiringSum[28].NumberOfVehicle;
                        result.CalValue30 = dataBusNormal[29].Number + dataBusUnAsignSum[29].NumberOfVehicle + dataBusHiringSum[29].NumberOfVehicle;
                        result.CalValue31 = dataBusNormal[30].Number + dataBusUnAsignSum[30].NumberOfVehicle + dataBusHiringSum[30].NumberOfVehicle;
                    }
                    else
                    {
                        result.CalValue01 = dataBusNormal[0].Number;
                        result.CalValue02 = dataBusNormal[1].Number;
                        result.CalValue03 = dataBusNormal[2].Number;
                        result.CalValue04 = dataBusNormal[3].Number;
                        result.CalValue05 = dataBusNormal[4].Number;
                        result.CalValue06 = dataBusNormal[5].Number;
                        result.CalValue07 = dataBusNormal[6].Number;
                        result.CalValue08 = dataBusNormal[7].Number;
                        result.CalValue09 = dataBusNormal[8].Number;
                        result.CalValue10 = dataBusNormal[9].Number;
                        result.CalValue11 = dataBusNormal[10].Number;
                        result.CalValue12 = dataBusNormal[11].Number;
                        result.CalValue13 = dataBusNormal[12].Number;
                        result.CalValue14 = dataBusNormal[13].Number;
                        result.CalValue15 = dataBusNormal[14].Number;
                        result.CalValue16 = dataBusNormal[15].Number;
                        result.CalValue17 = dataBusNormal[16].Number;
                        result.CalValue18 = dataBusNormal[17].Number;
                        result.CalValue19 = dataBusNormal[18].Number;
                        result.CalValue20 = dataBusNormal[19].Number;
                        result.CalValue21 = dataBusNormal[20].Number;
                        result.CalValue22 = dataBusNormal[21].Number;
                        result.CalValue23 = dataBusNormal[22].Number;
                        result.CalValue24 = dataBusNormal[23].Number;
                        result.CalValue25 = dataBusNormal[24].Number;
                        result.CalValue26 = dataBusNormal[25].Number;
                        result.CalValue27 = dataBusNormal[26].Number;
                        result.CalValue28 = dataBusNormal[27].Number;
                        result.CalValue29 = dataBusNormal[28].Number;
                        result.CalValue30 = dataBusNormal[29].Number;
                        result.CalValue31 = dataBusNormal[30].Number;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return result;
            }
        }
        #endregion
        #region  Report PDF
        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            try
            {
                var searchParams = EncryptHelper.DecryptFromUrl<BusTypeListData>(queryParams);
                XtraReport report = new XtraReport();
                if (searchParams.DepositOutputTemplate.Id == 1)
                {
                    switch (searchParams.PaperSize)
                    {
                        case PaperSize.A3:
                            report = new Reports.ReportTemplate.BusTypeList.BusTypeListA3();
                            break;
                        case PaperSize.A4:
                            report = new Reports.ReportTemplate.BusTypeList.BusTypeList();
                            break;
                        case PaperSize.B4:
                            report = new Reports.ReportTemplate.BusTypeList.BusTypeListB4();
                            break;
                        default:
                            break;
                    }
                }
                else if (searchParams.DepositOutputTemplate.Id == 2)
                {
                    switch (searchParams.PaperSize)
                    {
                        case PaperSize.A3:
                            report = new Reports.ReportTemplate.BusTypeList.BusTypeListGroupA3();
                            break;
                        case PaperSize.A4:
                            report = new Reports.ReportTemplate.BusTypeList.BusTypeListGroupA4();
                            break;
                        case PaperSize.B4:
                            report = new Reports.ReportTemplate.BusTypeList.BusTypeListGroupB4();
                            break;
                        default:
                            break;
                    }
                }

                ObjectDataSource dataSource = new ObjectDataSource();
                Parameter param = new Parameter();
                var dataReport = new List<BusTypeListReportPDF>();
                var dataReportGroup = new List<BusTypeListReportGroupPDF>();
                if (searchParams.DepositOutputTemplate.Id == 1)
                {
                    dataReport = await GetPDFData(searchParams);

                    param.Name = "data";
                    param.Type = typeof(List<BusTypeListReportPDF>);
                    param.Value = dataReport;
                    dataSource.Name = "objectDataSource1";
                    dataSource.DataSource = typeof(BusTypeListReportDS);
                    dataSource.Constructor = new ObjectConstructorInfo(param);
                    dataSource.DataMember = "_data";

                }
                else if (searchParams.DepositOutputTemplate.Id == 2)
                {
                    dataReportGroup = await GetPDFDataReportGroup(searchParams);
                    param.Name = "data";
                    param.Type = typeof(List<BusTypeListReportGroupPDF>);
                    param.Value = dataReportGroup;
                    dataSource.Name = "objectDataSource1";
                    dataSource.DataSource = typeof(BusTypeListReportGroupDS);
                    dataSource.Constructor = new ObjectConstructorInfo(param);
                    dataSource.DataMember = "_data";
                }
                report.DataSource = dataSource;
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }



        #endregion


        #region Save Fiiter
        //todo check null
        public Dictionary<string, string> GetFieldValues(BusTypeListData reportData)
        {
            try
            {
                var result = new Dictionary<string, string>
                {
                    [nameof(reportData.StartDate)] = reportData.StartDate.ToString("yyyyMMdd"),
                    [nameof(reportData.BookingTypeFrom)] = reportData.BookingTypeFrom?.YoyaKbnSeq.ToString() ?? string.Empty,
                    [nameof(reportData.BookingTypeTo)] = reportData.BookingTypeTo?.YoyaKbnSeq.ToString() ?? string.Empty,
                    [nameof(reportData.Company)] = reportData.Company?.CompanyCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.BranchStart)] = reportData.BranchStart?.EigyoCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.BranchEnd)] = reportData.BranchEnd?.EigyoCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.SalesStaffStart)] = reportData.SalesStaffStart?.SyainCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.SalesStaffEnd)] = reportData.SalesStaffEnd?.SyainCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.PersonInputStart)] = reportData.PersonInputStart?.SyainCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.PersonInputEnd)] = reportData.PersonInputEnd?.SyainCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.DestinationStart)] = reportData.DestinationStart?.BasyoMapCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.DestinationEnd)] = reportData.DestinationEnd?.BasyoMapCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.BusType)] = reportData.BusType.CodeKbn ?? string.Empty,
                    [nameof(reportData.VehicleFrom)] = reportData.VehicleFrom?.SyaSyuCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.VehicleTo)] = reportData.VehicleTo?.SyaSyuCdSeq.ToString() ?? string.Empty,
                    [nameof(reportData.GridSize)] = $"{((int)reportData.GridSize).ToString()}",
                    [nameof(reportData.GroupMode)] = $"{((int)reportData.GroupMode).ToString()}",
                    [nameof(reportData.OutputType)] = $"{((int)reportData.OutputType).ToString()}",
                    [nameof(reportData.PaperSize)] = $"{((int)reportData.PaperSize).ToString()}",
                    [nameof(reportData.DepositOutputTemplate)] = $"{((int)reportData.DepositOutputTemplate.Id).ToString()}",
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }

        public void ApplyFilter(ref BusTypeListData reportData,
            List<ReservationClassComponentData> bookingTypeList,
            List<CompanyData> companychartlst,
            List<LoadSaleBranch> branchList,
            List<LoadStaffList> staffList,
            List<DestinationData> destinationList,
            List<TPM_CodeKbCodeSyuData> busTypeList,
            List<BusTypesData> vehicleTypeList,
           List<DepositOutputClass> depositOutputClasses,
            Dictionary<string, string> filterValues)
        {

            try
            {
                foreach (var keyValue in filterValues)
                {
                    if (keyValue.Key == nameof(reportData.StartDate))
                    {
                        DateTime operationDate;
                        if (DateTime.TryParseExact(keyValue.Value, "yyyyMMdd", null, DateTimeStyles.None, out operationDate))
                        {
                            reportData.StartDate = operationDate;
                        }
                    }
                    if (keyValue.Key == nameof(reportData.ReservationList))
                    {
                        var values = keyValue.Value.Split('-').Select(_ => int.Parse(_));
                        reportData.ReservationList = bookingTypeList.Where(r => values.Contains(r.YoyaKbnSeq)).ToList();
                    }
                    if (keyValue.Key == nameof(reportData.BookingTypeFrom))
                    {
                        int bookingfrom;
                        if (int.TryParse(keyValue.Value, out bookingfrom))
                        {
                            reportData.BookingTypeFrom = bookingTypeList.FirstOrDefault(d => d.YoyaKbnSeq == bookingfrom);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.BookingTypeTo))
                    {
                        int bookingto;
                        if (int.TryParse(keyValue.Value, out bookingto))
                        {
                            reportData.BookingTypeTo = bookingTypeList.FirstOrDefault(d => d.YoyaKbnSeq == bookingto);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.Company))
                    {
                        int comSeq;
                        if (int.TryParse(keyValue.Value, out comSeq))
                        {
                            reportData.Company = companychartlst.SingleOrDefault(d => d.CompanyCdSeq == comSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.BranchStart))
                    {
                        int branchSeq;
                        if (int.TryParse(keyValue.Value, out branchSeq))
                        {
                            reportData.BranchStart = branchList.FirstOrDefault(d => d.EigyoCdSeq == branchSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.BranchEnd))
                    {
                        int branchSeq;
                        if (int.TryParse(keyValue.Value, out branchSeq))
                        {
                            reportData.BranchEnd = branchList.FirstOrDefault(d => d.EigyoCdSeq == branchSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.SalesStaffStart))
                    {
                        int staffSeq;
                        if (int.TryParse(keyValue.Value, out staffSeq))
                        {
                            reportData.SalesStaffStart = staffList.FirstOrDefault(d => d.SyainCdSeq == staffSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.SalesStaffEnd))
                    {
                        int staffSeq;
                        if (int.TryParse(keyValue.Value, out staffSeq))
                        {
                            reportData.SalesStaffEnd = staffList.FirstOrDefault(d => d.SyainCdSeq == staffSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.PersonInputStart))
                    {
                        int staffSeq;
                        if (int.TryParse(keyValue.Value, out staffSeq))
                        {
                            reportData.PersonInputStart = staffList.SingleOrDefault(d => d.SyainCdSeq == staffSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.PersonInputEnd))
                    {
                        int staffSeq;
                        if (int.TryParse(keyValue.Value, out staffSeq))
                        {
                            reportData.PersonInputEnd = staffList.FirstOrDefault(d => d.SyainCdSeq == staffSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.DestinationStart))
                    {
                        int mapSeq;
                        if (int.TryParse(keyValue.Value, out mapSeq))
                        {
                            reportData.DestinationStart = destinationList.FirstOrDefault(d => d.BasyoMapCdSeq == mapSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.DestinationEnd))
                    {
                        int mapSeq;
                        if (int.TryParse(keyValue.Value, out mapSeq))
                        {
                            reportData.DestinationEnd = destinationList.FirstOrDefault(d => d.BasyoMapCdSeq == mapSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.BusType))
                    {
                        int codeKbn;
                        if (int.TryParse(keyValue.Value, out codeKbn))
                        {
                            if (codeKbn == 0)
                            {
                                reportData.BusType = busTypeList.FirstOrDefault();
                            }
                            else
                            {
                                reportData.BusType = busTypeList.SingleOrDefault(d => d.CodeKbn == codeKbn.ToString());
                            }
                        }
                    }
                    if (keyValue.Key == nameof(reportData.VehicleFrom))
                    {
                        int vehicleSeq;
                        if (int.TryParse(keyValue.Value, out vehicleSeq))
                        {
                            reportData.VehicleFrom = vehicleTypeList.FirstOrDefault(d => d.SyaSyuCdSeq == vehicleSeq);
                        }
                    }
                    if (keyValue.Key == nameof(reportData.VehicleTo))
                    {
                        int vehicleSeq;
                        if (int.TryParse(keyValue.Value, out vehicleSeq))
                        {
                            reportData.VehicleTo = vehicleTypeList.FirstOrDefault(d => d.SyaSyuCdSeq == vehicleSeq);
                        }
                    }

                    //GridSize
                    if (keyValue.Key == nameof(reportData.GridSize))
                    {
                        int value;
                        if (int.TryParse(keyValue.Value, out value))
                        {
                            var result = (ViewMode)value;
                            reportData.GridSize = result;
                        }
                    }
                    //Group            
                    if (keyValue.Key == nameof(reportData.GroupMode))
                    {
                        int value;
                        if (int.TryParse(keyValue.Value, out value))
                        {
                            var result = (GroupMode)value;
                            reportData.GroupMode = result;
                        }
                    }
                    //PREVIEW
                    if (keyValue.Key == nameof(reportData.OutputType))
                    {
                        int value;
                        if (int.TryParse(keyValue.Value, out value))
                        {
                            var result = (OutputReportType)value;
                            reportData.OutputType = result;
                        }
                    }
                    //Page Size
                    if (keyValue.Key == nameof(reportData.PaperSize))
                    {
                        int value;
                        if (int.TryParse(keyValue.Value, out value))
                        {
                            var result = (PaperSize)value;
                            reportData.PaperSize = result;
                        }
                    }
                    if (keyValue.Key == nameof(reportData.DepositOutputTemplate))
                    {
                        int value;
                        if (int.TryParse(keyValue.Value, out value))
                        {
                            reportData.DepositOutputTemplate = depositOutputClasses.FirstOrDefault(x => x.Id == value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

        }
        #endregion
    }
}
