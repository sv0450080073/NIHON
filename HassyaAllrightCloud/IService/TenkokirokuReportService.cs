using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.AttendanceConfirm.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Reports.DataSource;
using DevExpress.DataAccess.ObjectBinding;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.IService
{
    public interface ITenkokirokuReportService : IReportService
    {
        Task<List<AttendanceConfirmReportData>> GetInfoMainReport(AttendanceConfirmReportData attendanceConfirmReportData);
        void ApplyFilter(ref AttendanceConfirmReportData reportData, List<CompanyChartData> companychartlst,
        List<DepartureOfficeData> vehicledispatchofficelst, List<ReservationClassComponentData> reservationlst, List<OutputOrderData> outputorderlst, Dictionary<string, string> filterValues);
        Dictionary<string, string> GetFieldValues(AttendanceConfirmReportData reportData);
        Task<List<CurrentAttendanceConfirm>> GetListAttendanceConfirmForSearch(AttendanceConfirmReportData searchParams);
        Task<List<AttendanceConfirmReportPDF>> GetPDFData(AttendanceConfirmReportData searchParams);
        Task<List<InfoHaiinEmployee>> GetInfoHaiinEmployee(string ukeNo, string unkRen, string teiDanNo, string bunkRen, string haiShaSyuKoYmd, string tenantCdSeq);
        Task<List<TehaiReport>> GetListTehaiByItemHaiSha(string ukeno, int unkRen, int teiDanNo, int bunkRen, int nittei, int tomKbn);
    }

    public class TenkokirokuReportService : ITenkokirokuReportService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediator;
        private IReportLayoutSettingService _reportLayoutSettingService;

        public TenkokirokuReportService(KobodbContext context, IMediator mediator, IReportLayoutSettingService reportLayoutSettingService)
        {
            _dbContext = context;
            _mediator = mediator;
            _reportLayoutSettingService = reportLayoutSettingService;
        }
        public async Task<List<AttendanceConfirmReportData>> GetInfoMainReport(AttendanceConfirmReportData conditionReport)
        {
            if (string.IsNullOrEmpty(conditionReport.OperationDate.ToString())
                || (conditionReport.CompanyChartData == null
                || conditionReport.CompanyChartData.Count == 0)
                || conditionReport.VehicleDispatchOffice1 == null
                || conditionReport.VehicleDispatchOffice1.EigyoCdSeq == -1
                || conditionReport.VehicleDispatchOffice2 == null
                || conditionReport.VehicleDispatchOffice2.EigyoCdSeq == -1
                || conditionReport.ReservationList == null
                || conditionReport.ReservationList.Count == 0
                ) return new List<AttendanceConfirmReportData>();

            int checkSelectAllCompany = 0,
                checkSelectAllBranch = 0,
                checkSelectAllBookingType = 0,
                checkHaiSKbn = 0;


            if (conditionReport.CompanyChartData.Count > 1 && conditionReport.CompanyChartData[0].CompanyCdSeq == 0)
            {
                checkSelectAllCompany = 1;
            }
            if (conditionReport.VehicleDispatchOffice1.EigyoCdSeq == 0
               || conditionReport.VehicleDispatchOffice2.EigyoCdSeq == 0)
            {
                checkSelectAllBranch = 1;
            }
            /*if (conditionReport.ReservationList.Select(c => c.YoyaKbnSeq).Contains(0))
             {
                 checkSelectAllBookingType = 1;
             }*/
            checkHaiSKbn = conditionReport.Undelivered == "未出力" ? 1 : 0;

            var data = new List<AttendanceConfirmReportData>();
            try
            {
                data = (from HAISHA in _dbContext.TkdHaisha
                        join YOYAKUSHO in _dbContext.TkdYyksho
                        on HAISHA.UkeNo equals YOYAKUSHO.UkeNo
                        into YOYAKUSHO_join
                        from YOYAKUSHO in YOYAKUSHO_join.DefaultIfEmpty()
                        join UNKOBI in _dbContext.TkdUnkobi
                        on new { HAISHA.UkeNo, H1 = HAISHA.UnkRen } equals
                        new { UNKOBI.UkeNo, H1 = UNKOBI.UnkRen }
                        into UNKOBI_join
                        from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                        join HENSYA in _dbContext.VpmHenSya
                        on HAISHA.HaiSsryCdSeq equals HENSYA.SyaRyoCdSeq
                        into HENSYA_join
                        from HENSYA in HENSYA_join.DefaultIfEmpty()
                        join EIGYOSHO in _dbContext.VpmEigyos
                        on HAISHA.SyuEigCdSeq equals EIGYOSHO.EigyoCdSeq
                        into EIGYOSHO_join
                        from EIGYOSHO in EIGYOSHO_join.DefaultIfEmpty()
                        join COMPANY in _dbContext.VpmCompny
                        on EIGYOSHO.CompanyCdSeq equals COMPANY.CompanyCdSeq
                        into COMPANY_join
                        from COMPANY in COMPANY_join.DefaultIfEmpty()
                        join SYARYO in _dbContext.VpmSyaRyo
                        on HAISHA.HaiSsryCdSeq equals SYARYO.SyaRyoCdSeq
                        into SYARYO_join
                        from SYARYO in SYARYO_join.DefaultIfEmpty()
                        join SYASYU in _dbContext.VpmSyaSyu
                        on SYARYO.SyaSyuCdSeq equals SYASYU.SyaSyuCdSeq
                        into SYASYU_join
                        from SYASYU in SYASYU_join.DefaultIfEmpty()
                        join YOYKBN in _dbContext.VpmYoyKbn
                        on new { YOYAKUSHO.YoyaKbnSeq, H1 = 1 } equals new { YOYKBN.YoyaKbnSeq, H1 = (int)YOYKBN.SiyoKbn }
                        into YOYKBN_join
                        from YOYKBN in YOYKBN_join.DefaultIfEmpty()
                        where HAISHA.SiyoKbn == 1
                        && HAISHA.HaiSsryCdSeq != 0
                        && COMPANY.TenantCdSeq == conditionReport.TenantCdSeq
                        && String.Compare(HAISHA.SyuKoYmd, conditionReport.OperationDate.ToString("yyyyMMdd")) <= 0
                        && String.Compare(HAISHA.KikYmd, conditionReport.OperationDate.ToString("yyyyMMdd")) >= 0
                        && (conditionReport.Undelivered == "未出力" ? HAISHA.HaiSsryCdSeq != 0 : HAISHA.HaiSsryCdSeq != null)
                        /*&& (checkSelectAllBookingType == 0 ? (conditionReport.ReservationList.Select(_ => _.YoyaKbnSeq).Contains(YOYAKUSHO.YoyaKbnSeq)) : YOYAKUSHO.YoyaKbnSeq != null)*/
                        && conditionReport.ReservationList.Select(_ => _.YoyaKbnSeq).Contains(YOYKBN.YoyaKbnSeq)
                        && (checkSelectAllBranch == 0 ? (EIGYOSHO.EigyoCd >= conditionReport.VehicleDispatchOffice1.EigyoCd
                        && EIGYOSHO.EigyoCd <= conditionReport.VehicleDispatchOffice2.EigyoCd) : HAISHA.SyuEigCdSeq != null)
                        && (checkSelectAllCompany == 0 ? conditionReport.CompanyChartData.Select(x => x.CompanyCdSeq).ToArray().Contains(COMPANY.CompanyCdSeq)
                                                   : COMPANY.CompanyCdSeq != null)
                        && (checkHaiSKbn == 1 ? HAISHA.HaiSkbn == 2 : HAISHA.HaiSkbn != null)
                        select new AttendanceConfirmReportData()
                        {
                            TenantCdSeq = COMPANY.TenantCdSeq
                        }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            return data;
        }
        public void ApplyFilter(ref AttendanceConfirmReportData reportData, List<CompanyChartData> companychartlst, List<DepartureOfficeData> vehicledispatchofficelst, List<ReservationClassComponentData> reservationlst, List<OutputOrderData> outputorderlst, Dictionary<string, string> filterValues)
        {
            foreach (var keyValue in filterValues)
            {
                if (keyValue.Key == nameof(reportData.OperationDate))
                {
                    DateTime operationDate;
                    if (DateTime.TryParseExact(keyValue.Value, "yyyyMMdd", null, DateTimeStyles.None, out operationDate))
                    {
                        reportData.OperationDate = operationDate;
                    }
                }
                if (keyValue.Key == nameof(reportData.CompanyChartData))
                {
                    reportData.CompanyChartData = new List<CompanyChartData>();
                    foreach (var companyItem in keyValue.Value.Split('-'))
                    {
                        int company;
                        if (int.TryParse(companyItem, out company))
                        {
                            reportData.CompanyChartData.Add(companychartlst.SingleOrDefault(c => c.CompanyCdSeq == company));
                        }
                    }
                }
                if (keyValue.Key == nameof(reportData.VehicleDispatchOffice1))
                {
                    int office;
                    if (int.TryParse(keyValue.Value, out office))
                    {
                        reportData.VehicleDispatchOffice1 = vehicledispatchofficelst.SingleOrDefault(c => c.EigyoCdSeq == office);
                    }
                }
                if (keyValue.Key == nameof(reportData.VehicleDispatchOffice2))
                {
                    int office;
                    if (int.TryParse(keyValue.Value, out office))
                    {
                        reportData.VehicleDispatchOffice2 = vehicledispatchofficelst.SingleOrDefault(c => c.EigyoCdSeq == office);
                    }
                }
                //if (keyValue.Key == nameof(reportData.ReservationList))
                //{
                //    var values = keyValue.Value.Split('-').Select(_ => int.Parse(_));
                //    reportData.ReservationList = reservationlst.Where(r => values.Contains(r.YoyaKbnSeq)).ToList();
                //}
                if (keyValue.Key == nameof(reportData.Undelivered))
                {
                    reportData.Undelivered = keyValue.Value;
                }
                if (keyValue.Key == nameof(reportData.OutputOrder))
                {
                    int outputOrder;
                    if (int.TryParse(keyValue.Value, out outputOrder))
                    {
                        reportData.OutputOrder = outputorderlst.SingleOrDefault(o => o.IdValue == outputOrder);
                    }
                }
                if (keyValue.Key == nameof(reportData.SizeOfPaper))
                {
                    reportData.SizeOfPaper = keyValue.Value;
                }
                if (keyValue.Key == nameof(reportData.TxtInstructions))
                {
                    reportData.TxtInstructions = keyValue.Value;
                }
                if (keyValue.Key == nameof(reportData.TxtKeyObjectives))
                {
                    reportData.TxtKeyObjectives = keyValue.Value;
                }
                if (keyValue.Key == nameof(reportData.KeyObjectivesList))
                {
                    reportData.KeyObjectivesList = keyValue.Value.Split('-').ToList();
                }
                if (keyValue.Key == nameof(reportData.InstructionsList))
                {
                    reportData.InstructionsList = keyValue.Value.Split('-').ToList();
                }
                if (keyValue.Key == nameof(reportData.TenantCdSeq))
                {
                    int tenant;
                    if (int.TryParse(keyValue.Value, out tenant))
                    {
                        reportData.TenantCdSeq = tenant;
                    }
                }
                if (keyValue.Key == nameof(reportData.SyainNm))
                {
                    reportData.SyainNm = keyValue.Value;
                }
                if (keyValue.Key == nameof(reportData.OutputSetting))
                {
                    int value;
                    if (int.TryParse(keyValue.Value, out value))
                    {
                        var result = (OutputInstruction)value;
                        reportData.OutputSetting = result;
                    }
                }
                if (keyValue.Key == nameof(reportData.BookingTypeFrom))
                {
                    int bookingfrom;
                    if (int.TryParse(keyValue.Value, out bookingfrom))
                    {
                        reportData.BookingTypeFrom = reservationlst.FirstOrDefault(d => d.YoyaKbnSeq == bookingfrom);
                    }
                }
                if (keyValue.Key == nameof(reportData.BookingTypeTo))
                {
                    int bookingto;
                    if (int.TryParse(keyValue.Value, out bookingto))
                    {
                        reportData.BookingTypeTo = reservationlst.FirstOrDefault(d => d.YoyaKbnSeq == bookingto);
                    }
                }

            }
        }
        public Dictionary<string, string> GetFieldValues(AttendanceConfirmReportData reportData)
        {
            var result = new Dictionary<string, string>
            {
                [nameof(reportData.OperationDate)] = reportData.OperationDate.ToString("yyyyMMdd"),
                [nameof(reportData.CompanyChartData)] = $"{string.Join('-', reportData.CompanyChartData.Select(c => c.CompanyCdSeq.ToString()))}",
                [nameof(reportData.VehicleDispatchOffice1)] = $"{reportData.VehicleDispatchOffice1.EigyoCdSeq}",
                [nameof(reportData.VehicleDispatchOffice2)] = $"{reportData.VehicleDispatchOffice2.EigyoCdSeq}",
                [nameof(reportData.ReservationList)] = string.Join('-', reportData.ReservationList.Select(_ => _.YoyaKbnSeq)),
                [nameof(reportData.Undelivered)] = $"{reportData.Undelivered}",
                [nameof(reportData.OutputOrder)] = $"{reportData.OutputOrder.IdValue}",
                [nameof(reportData.SizeOfPaper)] = $"{reportData.SizeOfPaper}",
                [nameof(reportData.TxtInstructions)] = $"{reportData.TxtInstructions}",
                [nameof(reportData.TxtKeyObjectives)] = $"{reportData.TxtKeyObjectives}",
                [nameof(reportData.KeyObjectivesList)] = $"{string.Join('-', reportData.KeyObjectivesList)}",
                [nameof(reportData.InstructionsList)] = $"{string.Join('-', reportData.InstructionsList)}",
                [nameof(reportData.TenantCdSeq)] = $"{reportData.TenantCdSeq}",
                [nameof(reportData.SyainNm)] = $"{reportData.SyainNm}",
                [nameof(reportData.OutputSetting)] = $"{ ((int)reportData.OutputSetting).ToString()}",
                [nameof(reportData.BookingTypeFrom)] = $"{reportData.BookingTypeFrom?.YoyaKbnSeq}",
                [nameof(reportData.BookingTypeTo)] = $"{reportData.BookingTypeTo?.YoyaKbnSeq}",
            };
            return result;
        }
        public async Task<List<CurrentAttendanceConfirm>> GetListAttendanceConfirmForSearch(AttendanceConfirmReportData searchParams)
        {
            return await _mediator.Send(new GetListAttendanceConfirm() { SearchParams = searchParams });
        }
        public async Task<List<InfoHaiinEmployee>> GetInfoHaiinEmployee(string ukeNo, string unkRen, string teiDanNo, string bunkRen, string haiShaSyuKoYmd, string tenantCdSeq)
        {
            return await _mediator.Send(new GetInfoHaiinEmployee()
            {
                Ukeno = ukeNo,
                Unkren = unkRen,
                TeiDanNo = teiDanNo,
                BunkRen = bunkRen,
                HaiShaSyuKoYmd = haiShaSyuKoYmd,
                TenantCdSeq = tenantCdSeq
            });
        }
        public async Task<List<AttendanceConfirmReportPDF>> GetPDFData(AttendanceConfirmReportData searchParams)
        {
            var data = new List<AttendanceConfirmReportPDF>();
            var listTotalRecord = new List<AttendanceConfirmReport>();
            var listCurrentAttendanceConfirm = new List<CurrentAttendanceConfirm>();
            var listInfoHaiinEmployeeByBus = new List<InfoHaiinEmployee>();
            var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMDHm);
            var page = 1;
            listCurrentAttendanceConfirm = await GetListAttendanceConfirmForSearch(searchParams);
            listCurrentAttendanceConfirm.ForEach(async e =>
            {
                await SetTehaiNm(e, searchParams);
                listInfoHaiinEmployeeByBus = await GetInfoHaiinEmployee(e.UkeNo ?? "", e.UnkRen ?? "", e.TeiDanNo ?? "", e.BunkRen ?? "", e.SyuKoYmd ?? "", new ClaimModel().TenantID.ToString());
                if (listInfoHaiinEmployeeByBus.Any())
                {
                    int stt = 0;
                    int countHaiin = listInfoHaiinEmployeeByBus.Count;
                    while (stt < countHaiin && countHaiin <= 5)
                    {
                        var objTmp = new AttendanceConfirmReport();
                        objTmp.BusCurrent = SetValueInfoBus(e);
                        objTmp.Haiin = SetValueHaiin(listInfoHaiinEmployeeByBus[stt]);
                        listTotalRecord.Add(objTmp);
                        stt++;
                    }
                }
                else
                {
                    var objTmp = new AttendanceConfirmReport();
                    objTmp.BusCurrent = SetValueInfoBus(e);
                    objTmp.Haiin = new InfoHaiinEmployee();
                    listTotalRecord.Add(objTmp);
                }
            });
            var listBranchDistinct = listTotalRecord.Select(x => x.BusCurrent.EigyoCdSeq).Distinct().ToList();
            foreach (var item in listBranchDistinct)
            {
                var dataByBranch = listTotalRecord.Where(x => x.BusCurrent.EigyoCdSeq == item).ToList();
                OnSetDataPerPage(data, dataByBranch, item, ref page);
            }
            data.ForEach(e =>
            {
                e.DateTimeFooter = searchParams.DateTimeFooter ?? "";
                e.DateTimeHeader = searchParams.OperationDate.ToString("yyyy/MM/dd (ddd)") ?? "";
                e.SyainNm = searchParams.SyainNm ?? "";
                e.InstructionsList = FormatListStringCondition(searchParams.InstructionsList);
                e.KeyObjectivesList = FormatListStringCondition(searchParams.KeyObjectivesList);
                e.TotalPage = page - 1;
            });
            return data;
        }
        private async Task SetTehaiNm(CurrentAttendanceConfirm dataCurrent, AttendanceConfirmReportData searchParams)
        {
            //Haishachi
            dataCurrent.VistLocation = "泊 中";
            dataCurrent.VistLocationCompact = "";
            int dayBusRuningA = dataCurrent.DayBusRunningReal;
            int daytotalB = dataCurrent.TotalDayBusRunReal;
            if (dayBusRuningA == 1)
            {
                dataCurrent.VistLocation = dataCurrent.HaiSNm;
                dataCurrent.VistLocationCompact = dataCurrent.HaiSKouKNm;
            }
            else
            {
                if (dataCurrent.HaiSYmd == dataCurrent.SyuKoYmd)
                {
                    var listTehai = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), Convert.ToInt32(dataCurrent.TeiDanNo), Convert.ToInt32(dataCurrent.BunkRen), dayBusRuningA - 1, 1);
                    if (listTehai.Any())
                    {
                        dataCurrent.VistLocation = listTehai.FirstOrDefault().TehNm;
                    }
                    else
                    {
                        var listTehaiAll = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), 0, 0, dayBusRuningA - 1, 1);
                        if(listTehaiAll.Any())
                        {
                            dataCurrent.VistLocation = listTehaiAll.FirstOrDefault().TehNm;
                        }    
                    }    
                    
                }
                else
                {
                    if (dayBusRuningA == 2)
                    {
                        var listTehai = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), Convert.ToInt32(dataCurrent.TeiDanNo), Convert.ToInt32(dataCurrent.BunkRen), dayBusRuningA - 1, 2);
                        if (listTehai.Any())
                        {
                            dataCurrent.VistLocation = listTehai.FirstOrDefault().TehNm;
                        }
                        else
                        {
                            var listTehaiAll = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), 0, 0, dayBusRuningA - 1, 2);
                            if (listTehaiAll.Any())
                            {
                                dataCurrent.VistLocation = listTehaiAll.FirstOrDefault().TehNm;
                            }
                        }
                    }
                    else
                    {
                        if (dayBusRuningA < daytotalB )
                        {
                            var listTehai = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), Convert.ToInt32(dataCurrent.TeiDanNo), Convert.ToInt32(dataCurrent.BunkRen), dayBusRuningA - 2, 1);
                            if (listTehai.Any())
                            {
                                dataCurrent.VistLocation = listTehai.FirstOrDefault().TehNm;
                            }
                            else
                            {
                                var listTehaiAll = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen),0,0, dayBusRuningA - 2, 1);
                                if (listTehaiAll.Any())
                                {
                                    dataCurrent.VistLocation = listTehaiAll.FirstOrDefault().TehNm;
                                }
                            }
                        }
                    }
                }
            }
            if (searchParams.OperationDate.ToString("yyyyMMdd").CompareTo(dataCurrent.TouYmd) >0)
            {
                dataCurrent.VistLocation = dataCurrent.HaiSNm = "泊 中";
                dataCurrent.VistLocationCompact = "";

            }    
                ///Touchachi
                string touNmTmp = "";
            string touSKouKNm = "";
            if (dataCurrent.HaiSYmd == dataCurrent.SyuKoYmd)
            {
                if (dayBusRuningA != daytotalB)
                {
                    var listTehai = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), Convert.ToInt32(dataCurrent.TeiDanNo), Convert.ToInt32(dataCurrent.BunkRen), dayBusRuningA, 1);
                    if (listTehai.Any())
                    {
                        touNmTmp = listTehai.FirstOrDefault().TehNm;
                    }
                    else
                    {
                        var listTehaiAll = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen),0, 0, dayBusRuningA, 1);
                        if (listTehaiAll.Any())
                        {
                            touNmTmp = listTehaiAll.FirstOrDefault().TehNm;
                        }
                    }
                }                   
            }
            else
            {
                if (dayBusRuningA == 1)
                {
                    var listTehai = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), Convert.ToInt32(dataCurrent.TeiDanNo), Convert.ToInt32(dataCurrent.BunkRen), dayBusRuningA, 2);
                    if (listTehai.Any())
                    {
                        touNmTmp = listTehai.FirstOrDefault().TehNm;
                    }
                    else
                    {
                        var listTehaiAll = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), 0, 0, dayBusRuningA, 2);
                        if (listTehaiAll.Any())
                        {
                            touNmTmp = listTehaiAll.FirstOrDefault().TehNm;
                        }
                    }
                }
                else
                {
                    if (dayBusRuningA < daytotalB -1 )
                    {
                        var listTehai = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), Convert.ToInt32(dataCurrent.TeiDanNo), Convert.ToInt32(dataCurrent.BunkRen), dayBusRuningA - 1, 1);
                        if (listTehai.Any())
                        {
                            touNmTmp = listTehai.FirstOrDefault().TehNm;
                        }
                        else
                        {
                            var listTehaiAll = await GetListTehaiByItemHaiSha(dataCurrent.UkeNo, Convert.ToInt32(dataCurrent.UnkRen), 0, 0, dayBusRuningA - 1, 1);
                            if (listTehaiAll.Any())
                            {
                                touNmTmp = listTehaiAll.FirstOrDefault().TehNm;
                            }
                        }

                    }
                }
            }
            if(searchParams.OperationDate.ToString("yyyyMMdd") == dataCurrent.TouYmd)
            {
                touNmTmp = dataCurrent.TouNm;
                touSKouKNm = dataCurrent.TouSKouKNm;
            }    
            dataCurrent.TouNm = touNmTmp;
            dataCurrent.TouSKouKNm = touSKouKNm;
        }
        private InfoHaiinEmployee SetValueHaiin(InfoHaiinEmployee data)
        {
            var objTmp = new InfoHaiinEmployee();
            objTmp.DR_SyainCd = data.DR_SyainCd;
            objTmp.DR_SyainNm = data.DR_SyainNm;
            objTmp.GUI_SyainCd = data.GUI_SyainCd;
            objTmp.GUI_SyainNm = data.GUI_SyainNm;
            return objTmp;
        }
        private CurrentAttendanceConfirm SetValueInfoBus(CurrentAttendanceConfirm item)
        {
            var result = new CurrentAttendanceConfirm();
            result.UkeNo = item.UkeNo;
            result.UnkRen = item.UnkRen;
            result.BunkRen = item.BunkRen;
            result.CompanyCdSeq = item.CompanyCdSeq;
            result.CompanyCd = item.CompanyCd;
            result.EigyoCdSeq = item.EigyoCdSeq;
            result.EigyoCd = item.EigyoCd;
            result.EigyoNm = item.EigyoNm;
            result.RyakuNm = item.RyakuNm;
            result.SyuKoYmd = item.SyuKoYmd;
            result.SyuKoTime = item.SyuKoTime;
            result.HaiSYmd = item.HaiSYmd;
            result.HaiSTime = item.HaiSTime;
            result.TouYmd = item.TouYmd;
            result.TouChTime = item.TouChTime;
            result.KikYmd = item.KikYmd;
            result.HAISHA_KikTime = item.HAISHA_KikTime;
            result.GoSya = item.GoSya;
            result.TeiDanNo = item.TeiDanNo;
            result.IkNm = item.IkNm;
            result.HaiSNm = item.HaiSNm;
            result.HaiSKouKCdSeq = item.HaiSKouKCdSeq;
            result.HaiSKouKNm = item.HaiSKouKNm;
            result.HaiSBinCdSeq = item.HaiSBinCdSeq;
            result.HaiSBinNm = item.HaiSBinNm;
            result.HaiSSetTime = item.HaiSSetTime;
            result.TouNm = item.TouNm;
            result.TouKouKCdSeq = item.TouKouKCdSeq;
            result.TouSKouKNm = item.TouSKouKNm;
            result.TouBinCdSeq = item.TouBinCdSeq;
            result.TouBinNm = item.TouBinNm;
            result.TouSetTime = item.TouSetTime;
            result.UNKOBI_SyuKoTime = item.UNKOBI_SyuKoTime;
            result.UNKOBI_HaiSYmd = item.UNKOBI_HaiSYmd;
            result.UNKOBI_HaiSTime = item.UNKOBI_HaiSTime;
            result.UNKOBI_TouYmd = item.UNKOBI_TouYmd;
            result.UNKOBI_TouChTime = item.UNKOBI_TouChTime;
            result.UNKOBI_KikTime = item.UNKOBI_KikTime;
            result.UNKOBI_DanTaNm = item.UNKOBI_DanTaNm;
            result.SyaSyuCd = item.SyaSyuCd;
            result.SyaSyuNm = item.SyaSyuNm;
            result.SyaRyoCdSeq = item.SyaRyoCdSeq;
            result.SyaRyoCd = item.SyaRyoCd;
            result.SyaRyoNm = item.SyaRyoNm;
            result.TenkoNo = item.TenkoNo;
            result.DayBusRunning = item.DayBusRunning;
            result.TotalDayBusRun = item.TotalDayBusRun;
            result.VistLocation = item.VistLocation;
            result.VistLocationCompact = item.VistLocationCompact;
            result.SyuKoTimeMain = item.SyuKoTimeMain;
            result.HaiSTimeMain = item.HaiSTimeMain;
            result.KikTimeMain = item.KikTimeMain;
            result.TotalBus = item.TotalBus;
            result.RowNum = item.RowNum; ;
            return result;
        }
        public string FormatListStringCondition(List<string> keyObjectivesList)
        {
            if (keyObjectivesList == null || keyObjectivesList.Count == 0) return "";
            return String.Join("-", keyObjectivesList.ToArray());
        }
        private void OnSetDataPerPage(
             List<AttendanceConfirmReportPDF> listData
           , List<AttendanceConfirmReport> listInfoBus
           , dynamic item
           , ref int page)
        {
            List<AttendanceConfirmReport> listTemp = new List<AttendanceConfirmReport>();
            listTemp = listInfoBus;
            var itemPerPage = 20;
            if (listTemp.Count > itemPerPage)
            {
                var count = Math.Ceiling(listTemp.Count * 1.0 / itemPerPage);
                for (int i = 0; i < count; i++)
                {
                    var onePage = new AttendanceConfirmReportPDF();
                    var listPerPage = listTemp.Skip(i * itemPerPage).Take(itemPerPage).ToList();
                    SetData(onePage, listPerPage, page, item, itemPerPage);
                    listData.Add(onePage);
                    page++;
                }
            }
            else
            {
                var onePage = new AttendanceConfirmReportPDF();
                SetData(onePage, listTemp, page, item, itemPerPage);
                listData.Add(onePage);
                page++;
            }

        }
        private void SetData(
            AttendanceConfirmReportPDF onePage,
            List<AttendanceConfirmReport> listPerPage,
            int page,
            dynamic item,
            int itemPerPage)
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new AttendanceConfirmReport());
            }
            var branchData = listPerPage.Where(x => x.BusCurrent.EigyoCdSeq == item).FirstOrDefault();
            List<string> listRowNum = new List<string>();
            foreach (var data in listPerPage)
            {
                var listDataSame = listPerPage.Where(x => x.BusCurrent.RowNum == data.BusCurrent.RowNum).ToList();
                if (listDataSame.Count >= 2 && !listRowNum.Contains(data.BusCurrent.RowNum))
                {
                    listRowNum.Add(data.BusCurrent.RowNum);
                    //todo
                    ResetValueInfoBus(listDataSame, data);
                }
            }
            onePage.EigyoCdEigyoNm = branchData.BusCurrent.EigyoNm;
            onePage.AttendanceConfirmReport = listPerPage;
            onePage.PageNumber = page;
        }
        private void ResetValueInfoBus(List<AttendanceConfirmReport> listData, AttendanceConfirmReport dataUnion)
        {
            foreach (var item in listData)
            {
                if (item != dataUnion)
                {
                    item.BusCurrent.UkeNo = "";
                    item.BusCurrent.UnkRen = "";
                    item.BusCurrent.BunkRen = "";
                    item.BusCurrent.CompanyCdSeq = "";
                    item.BusCurrent.CompanyCd = "";
                    item.BusCurrent.EigyoCdSeq = "";
                    item.BusCurrent.EigyoCd = "";
                    item.BusCurrent.EigyoNm = "";
                    item.BusCurrent.RyakuNm = "";
                    item.BusCurrent.SyuKoYmd = "";
                    item.BusCurrent.SyuKoTime = "";
                    item.BusCurrent.HaiSYmd = "";
                    item.BusCurrent.HaiSTime = "";
                    item.BusCurrent.TouYmd = "";
                    item.BusCurrent.TouChTime = "";
                    item.BusCurrent.KikYmd = "";
                    item.BusCurrent.HAISHA_KikTime = "";
                    item.BusCurrent.GoSya = "";
                    item.BusCurrent.TeiDanNo = "";
                    item.BusCurrent.IkNm = "";
                    item.BusCurrent.HaiSNm = "";
                    item.BusCurrent.HaiSKouKCdSeq = "";
                    item.BusCurrent.HaiSKouKNm = "";
                    item.BusCurrent.HaiSBinCdSeq = "";
                    item.BusCurrent.HaiSBinNm = "";
                    item.BusCurrent.HaiSSetTime = "";
                    item.BusCurrent.TouNm = "";
                    item.BusCurrent.TouKouKCdSeq = "";
                    item.BusCurrent.TouSKouKNm = "";
                    item.BusCurrent.TouBinCdSeq = "";
                    item.BusCurrent.TouBinNm = "";
                    item.BusCurrent.TouSetTime = "";
                    item.BusCurrent.UNKOBI_SyuKoTime = "";
                    item.BusCurrent.UNKOBI_HaiSYmd = "";
                    item.BusCurrent.UNKOBI_HaiSTime = "";
                    item.BusCurrent.UNKOBI_TouYmd = "";
                    item.BusCurrent.UNKOBI_TouChTime = "";
                    item.BusCurrent.UNKOBI_KikTime = "";
                    item.BusCurrent.UNKOBI_DanTaNm = "";
                    item.BusCurrent.SyaSyuCd = "";
                    item.BusCurrent.SyaSyuNm = "";
                    item.BusCurrent.SyaRyoCdSeq = "";
                    item.BusCurrent.SyaRyoCd = "";
                    item.BusCurrent.SyaRyoNm = "";
                    item.BusCurrent.TenkoNo = "";
                    item.BusCurrent.DayBusRunning = "";
                    item.BusCurrent.TotalDayBusRun = "";
                    item.BusCurrent.VistLocation = "";
                    item.BusCurrent.VistLocationCompact = "";
                    //item.BusCurrent.SyuKoTimeMain = "";
                    //item.BusCurrent.HaiSTimeMain = "";
                    //item.BusCurrent.KikTimeMain = "";
                    item.BusCurrent.TotalBus = "";
                    item.BusCurrent.RowNum = "";
                }
            }
        }
        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<AttendanceConfirmReportData>(queryParams);
            var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Attendanceconfirmreport, BaseNamespace.Attendanceconfirmreport, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq,
                searchParams.SizeOfPaper == "A3" ? (byte)PageSize.A3 : searchParams.SizeOfPaper == "A4" ? (byte)PageSize.A4 : searchParams.SizeOfPaper == "B4" ? (byte)PageSize.B4 : (byte)0);
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetPDFData(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<AttendanceConfirmReportPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(AttendanceConfirmReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        public async Task<List<TehaiReport>> GetListTehaiByItemHaiSha(string ukeno, int unkRen, int teiDanNo, int bunkRen, int nittei, int tomKbn)
        {
            return await _mediator.Send(new GetListTeHaiByItemHaiSha() { Ukeno = ukeno, UnkRen = unkRen, TeiDanNo = teiDanNo, BunkRen = bunkRen, Nittei = nittei, TomKbn = tomKbn });
        }
    }
}

