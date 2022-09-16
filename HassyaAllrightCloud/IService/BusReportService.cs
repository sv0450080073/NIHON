using DevExpress.Blazor.Internal;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IBusReportService
    {
        Task<List<BusReportData>> GetInfoMainReport(BusReportData busReportData);
        Dictionary<string, string> GetFieldValues(BusReportData reportData);
        void ApplyFilter(ref BusReportData reportData, Dictionary<string, string> filterValues);
    }
    public class BusReportService : IBusReportService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediator;

        public BusReportService(KobodbContext context, IMediator mediator)
        {
            _dbContext = context;
            _mediator = mediator;
        }

        public async Task<List<BusReportData>> GetInfoMainReport(BusReportData conditionReport)
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
               ) return new List<BusReportData>();

            int checkSelectAllCompany = 0,
                checkSelectAllBranch = 0,
                checkSelectAllBookingType = 0;
            //checkHaiSKbn = 0;


            if (conditionReport.CompanyChartData.Count > 1 && conditionReport.CompanyChartData[0].CompanyCdSeq == 0)
            {
                checkSelectAllCompany = 1;
            }
            if (conditionReport.VehicleDispatchOffice1.EigyoCdSeq == 0
               || conditionReport.VehicleDispatchOffice2.EigyoCdSeq == 0)
            {
                checkSelectAllBranch = 1;
            }
            if (conditionReport.ReservationList.Select(c => c.YoyaKbnSeq).Contains(0))
            {
                checkSelectAllBookingType = 1;
            }
            // checkHaiSKbn = conditionReport.Undelivered == "未出力" ? 1 : 0;

            var data = new List<BusReportData>();
            try
            {
                 data = await (from HAISHA in _dbContext.TkdHaisha
                               join YOYAKUSHO in _dbContext.TkdYyksho
                               on HAISHA.UkeNo equals YOYAKUSHO.UkeNo
                               into YOYAKUSHO_join
                               from YOYAKUSHO in YOYAKUSHO_join.DefaultIfEmpty()
                               join UNKOBI in _dbContext.TkdUnkobi
                               on new { HAISHA.UkeNo, H1 = HAISHA.UnkRen } equals
                               new { UNKOBI.UkeNo, H1 = UNKOBI.UnkRen }
                               into UNKOBI_join
                               from UNKOBI in UNKOBI_join.DefaultIfEmpty()
                               join SYARYO in _dbContext.VpmSyaRyo
                               on HAISHA.HaiSsryCdSeq equals SYARYO.SyaRyoCdSeq
                               into SYARYO_join
                               from SYARYO in SYARYO_join.DefaultIfEmpty()
                               join HENSYA in _dbContext.VpmHenSya
                               on SYARYO.SyaRyoCdSeq equals HENSYA.SyaRyoCdSeq
                               into HENSYA_join
                               from HENSYA in HENSYA_join.DefaultIfEmpty()
                               join EIGYOSHO in _dbContext.VpmEigyos
                               on HENSYA.EigyoCdSeq equals EIGYOSHO.EigyoCdSeq
                               into EIGYOSHO_join
                               from EIGYOSHO in EIGYOSHO_join.DefaultIfEmpty()
                               join COMPANY in _dbContext.VpmCompny
                               on new { H1 = EIGYOSHO.CompanyCdSeq, H2 = conditionReport.TenantCdSeq }
                               equals new { H1 = COMPANY.CompanyCdSeq, H2 = COMPANY.TenantCdSeq }
                               into COMPANY_join
                               from COMPANY in COMPANY_join.DefaultIfEmpty()
                               join YYKSYU in _dbContext.TkdYykSyu
                               on new { HAISHA.UkeNo, HAISHA.UnkRen, HAISHA.SyaSyuRen }
                               equals new { YYKSYU.UkeNo, YYKSYU.UnkRen, YYKSYU.SyaSyuRen }
                               into YYKSYU_join
                               from YYKSYU in YYKSYU_join.DefaultIfEmpty()
                               join SYASYU in _dbContext.VpmSyaSyu
                               on YYKSYU.SyaSyuCdSeq equals SYASYU.SyaSyuCdSeq
                               into SYASYU_join
                               from SYASYU in SYASYU_join.DefaultIfEmpty()
                               join SYASHYU1 in _dbContext.VpmSyaSyu
                               on SYARYO.SyaSyuCdSeq equals SYASHYU1.SyaSyuCdSeq
                               into SYASHYU1_join
                               from SYASHYU1 in SYASHYU1_join.DefaultIfEmpty()
                               join YOYKBN in _dbContext.VpmYoyKbn
                               on new { key1 = YOYAKUSHO.YoyaKbnSeq, key2 = (byte)1, key3 = conditionReport.TenantCdSeq} equals new {key1 = YOYKBN.YoyaKbnSeq, key2 = YOYKBN.SiyoKbn, key3 = YOYKBN.TenantCdSeq} 
                               where HAISHA.SiyoKbn == 1
                                &&  UNKOBI.SiyoKbn == 1
                                && HAISHA.Kskbn != 1
                                && YOYAKUSHO.YoyaSyu == 1
                                && YOYAKUSHO.SiyoKbn == 1
                                && YOYAKUSHO.TenantCdSeq == conditionReport.TenantCdSeq
                                && ((YYKSYU.SyaSyuCdSeq != 0 && SYASYU.TenantCdSeq == conditionReport.TenantCdSeq) || YYKSYU.SyaSyuCdSeq == 0)
                                && SYASHYU1.TenantCdSeq == conditionReport.TenantCdSeq
                                //&& HAISHA.HaiSsryCdSeq != 0
                                &&  YYKSYU.SiyoKbn == 1
                                && COMPANY.TenantCdSeq == conditionReport.TenantCdSeq
                                && String.Compare(HAISHA.SyuKoYmd, conditionReport.OperationDate.ToString("yyyyMMdd")) <= 0
                                && String.Compare(HAISHA.KikYmd, conditionReport.OperationDate.ToString("yyyyMMdd")) >= 0
                                && (conditionReport.Undelivered == "未出力" ? HAISHA.HaiSsryCdSeq != 0 : HAISHA.HaiSsryCdSeq != null)
                                && YOYKBN.YoyaKbnSeq >= (conditionReport.BookingFrom.YoyaKbnSeq != 0 ? conditionReport.BookingFrom.YoyaKbnSeq : 1)
                                && YOYKBN.YoyaKbnSeq <= (conditionReport.BookingTo.YoyaKbnSeq != 0 ? conditionReport.BookingTo.YoyaKbnSeq : 99)
                                //  && (checkSelectAllBookingType == 0 ? (conditionReport.ReservationList.Select(_ => _.YoyaKbnSeq).Contains(YOYAKUSHO.YoyaKbnSeq)) : YOYAKUSHO.YoyaKbnSeq != null)
                                //&& conditionReport.ReservationList.Select(_ => _.YoyaKbnSeq).Contains(YOYAKUSHO.YoyaKbnSeq)
                                && (checkSelectAllBranch == 0 ? (HAISHA.SyuEigCdSeq >= conditionReport.VehicleDispatchOffice1.EigyoCdSeq
                                && HAISHA.SyuEigCdSeq <= conditionReport.VehicleDispatchOffice2.EigyoCdSeq) : HAISHA.SyuEigCdSeq != null)
                                && (checkSelectAllCompany == 0 ? conditionReport.CompanyChartData.Select(x => x.CompanyCdSeq).ToArray().Contains(COMPANY.CompanyCdSeq)
                                                   : COMPANY.CompanyCdSeq != null)
                               //&& (checkHaiSKbn == 1 ? HAISHA.HaiSkbn == 2 : HAISHA.HaiSkbn != null)
                               select new BusReportData()
                               {
                                   TenantCdSeq = COMPANY.TenantCdSeq
                               }).ToListAsync();
             
            }
            catch (Exception)
            {
                return null;
            }
            return data;
        }

        public Dictionary<string, string> GetFieldValues(BusReportData reportData)
        {
            var result = new Dictionary<string, string>
            {
                [nameof(reportData.OperationDate)] = reportData.OperationDate.ToString("yyyyMMdd"),
                [nameof(reportData.CompanyChartData)] = string.Join('-', reportData.CompanyChartData.Select(c => c.CompanyCdSeq)),
                [nameof(reportData.VehicleDispatchOffice1)] = reportData.VehicleDispatchOffice1.EigyoCdSeq.ToString(),
                [nameof(reportData.VehicleDispatchOffice2)] = reportData.VehicleDispatchOffice2.EigyoCdSeq.ToString(),
                [nameof(reportData.ReservationList)] = string.Join('-', reportData.ReservationList.Select(_ => _.YoyaKbnSeq)),
                [nameof(reportData.Undelivered)] = reportData.Undelivered,
                [nameof(reportData.TemporaryCar)] = reportData.TemporaryCar,
                [nameof(reportData.OutputOrder)] = reportData.OutputOrder.IdValue.ToString(),
                [nameof(reportData.SizeOfPaper)] = reportData.SizeOfPaper,
                [nameof(reportData.OutputSetting)] = ((int)reportData.OutputSetting).ToString(),
                [nameof(reportData.BookingFrom)] = $"{reportData.BookingFrom.YoyaKbnSeq}",
                [nameof(reportData.BookingTo)] = $"{reportData.BookingTo.YoyaKbnSeq}",
            };
            return result;
        }

        public void ApplyFilter(ref BusReportData reportData, Dictionary<string, string> filterValues)
        {
            try
            {
                string outValueString = string.Empty;
                DateTime dt = new DateTime();
                var dataPropList = reportData
                    .GetType()
                    .GetProperties()
                    .Where(d => d.CanWrite && d.CanRead)
                    .ToList();
                foreach (var dataProp in dataPropList)
                {
                    if (filterValues.TryGetValue(dataProp.Name, out outValueString))
                    {
                        if (dataProp.PropertyType.IsGenericType || dataProp.PropertyType.IsClass && dataProp.PropertyType != typeof(string))
                        {
                            continue;
                        }
                        dynamic setValue = null;
                        if (dataProp.PropertyType == typeof(string))
                        {
                            setValue = outValueString;
                        }
                        else if (dataProp.PropertyType == typeof(DateTime))
                        {
                            if (DateTime.TryParseExact(outValueString, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                            {
                                setValue = dt;
                            }
                        }

                        dataProp.SetValue(reportData, setValue);
                    }
                }

                reportData.CompanyChartData.Clear();
                foreach (var companyItem in filterValues[nameof(reportData.CompanyChartData)].Split('-'))
                {
                    if (int.TryParse(companyItem, out int company))
                    {
                        reportData.CompanyChartData.Add(new CompanyChartData()
                        {
                            CompanyCdSeq = company
                        });
                    }
                }
                int outValue;
                if (filterValues.ContainsKey(nameof(reportData.VehicleDispatchOffice1)))
                {
                    if (int.TryParse(filterValues[nameof(reportData.VehicleDispatchOffice1)], out outValue))
                    {
                        reportData.VehicleDispatchOffice1 = new DepartureOfficeData() { EigyoCdSeq = outValue };
                    }
                }
                if (filterValues.ContainsKey(nameof(reportData.VehicleDispatchOffice2)))
                {
                    if (int.TryParse(filterValues[nameof(reportData.VehicleDispatchOffice2)], out outValue))
                    {
                        reportData.VehicleDispatchOffice2 = new DepartureOfficeData() { EigyoCdSeq = outValue };
                    }
                }
                if (filterValues.ContainsKey(nameof(reportData.OutputOrder)))
                {
                    if (int.TryParse(filterValues[nameof(reportData.OutputOrder)], out outValue))
                    {
                        reportData.OutputOrder = new OutputOrderData() { IdValue = outValue };
                    }
                }
                if (filterValues.ContainsKey(nameof(reportData.OutputSetting)))
                {
                    if (int.TryParse(filterValues[nameof(reportData.OutputSetting)], out outValue))
                    {
                        var result = (OutputInstruction)outValue;
                        reportData.OutputSetting = result;
                    }
                }
                if (filterValues.ContainsKey(nameof(reportData.ReservationList)))
                {
                    var yoyaKbnSeqList = filterValues[nameof(reportData.ReservationList)].Split('-').Select(_ => int.Parse(_));
                    reportData.ReservationList = yoyaKbnSeqList.Select(_ => new ReservationData() { YoyaKbnSeq = _ }).ToList();
                }
                if (filterValues.ContainsKey(nameof(reportData.BookingFrom)))
                {
                    if (int.TryParse(filterValues[nameof(reportData.BookingFrom)], out outValue))
                    {
                        reportData.BookingFrom = new ReservationData() { YoyaKbnSeq = outValue };
                        // reportData.BookingFrom = reportData.ReservationList.SingleOrDefault(d => d.YoyaKbnSeq == outValue);
                    }
                }
                if (filterValues.ContainsKey(nameof(reportData.BookingTo)))
                {
                    if (int.TryParse(filterValues[nameof(reportData.BookingTo)], out outValue))
                    {
                        reportData.BookingTo = new ReservationData() { YoyaKbnSeq = outValue };
                        //reportData.BookingTo = reportData.ReservationList.SingleOrDefault(d => d.YoyaKbnSeq == outValue);
                    }
                }
            }
            catch (Exception)
            {

            }


        }
    }
}
