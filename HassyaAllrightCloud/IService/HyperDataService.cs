using HassyaAllrightCloud.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Helpers;
using System.Drawing;
using MediatR;
using HassyaAllrightCloud.Application.HyperData.Queries;
using HassyaAllrightCloud.Application.HyperData.Commands;
using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.ObjectBinding;
using HassyaAllrightCloud.Reports.DataSource;

namespace HassyaAllrightCloud.IService
{
    public interface IHyperDataService
    {
        Task<List<SuperMenuReservationData>> GetSuperMenuReservationData(HyperFormData hyperData, int CompanyId, int TenantId, int? OffSet = null, int Fetch = 0);
        Task<SuperMenuReservationTotalGridData> GetSuperMenuReservationTotalData(HyperFormData hyperData, int CompanyId, int TenantId);
        Task<List<HyperGraphData>> GetHyperGraphData(HyperFormData hyperData, int CompanyId, int TenantId, bool IsGetCount = false);
        Task<List<SalePerTime>> GetSaleByDay(List<HyperGraphData> GraphData, string TypeDate, DateTime StartDate, DateTime EndDate);
        Task<List<SalePerTime>> GetSaleByMonth(List<HyperGraphData> GraphData, string TypeDate, DateTime StartDate, DateTime EndDate);
        Task<List<SalePerStaff>> GetSaleByStaff(List<HyperGraphData> GraphData, string TypeDate);
        Task<List<SalePerCustomer>> GetSaleByCustomer(List<HyperGraphData> GraphData, string TypeDate);
        Task<List<SalePerGroupClassification>> GetSaleByGroupClassification(List<HyperGraphData> GraphData, string TypeDate);
        void JoinSaleByStaff(ref List<SalePerStaff> listThisYear, ref List<SalePerStaff> listLastYear);
        void JoinSaleByCustomer(ref List<SalePerCustomer> listThisYear, ref List<SalePerCustomer> listLastYear);
        void JoinSaleByGroupClassification(ref List<SalePerGroupClassification> listThisYear, ref List<SalePerGroupClassification> listLastYear);
        Task<List<ReservationDataToCheck>> GetDataReservationToCheck(List<string> ReceiptNumber);
        Task<bool> CancelRevervation(List<string> ReceiptNumber, List<ReservationDataToCheck> listCheck);
        Task<bool> ConfirmRevervation(List<string> ReceiptNumber);
        // Get data for super menu vehicle
        Task<List<SuperMenuVehicleData>> GetSuperMenuVehicleData(HyperFormData hyperData, int CompanyId, int TenantId, int? OffSet = null, int Fetch = 0);
        Task<SuperMenuVehicleTotalGridData> GetSuperMenuVehicleTotalData(HyperFormData hyperData, int CompanyId, int TenantId);
        string CheckValidationForGraph(GraphTypeDisplay type, HyperFormData hyperData, int dateType);
        Color GetColorByIndex(int index);
        HyperFormData GetDataLastYear(List<HyperFormData> CurrentHyperForm, int GraphIndex, byte isThisYear, string currentYears, bool _isMonth);
        Task<List<SuperMenuVehicleReportPDF>> GetSuperMenuVehicleReportData(HyperFormData hyperData, int CompanyId, int TenantId, int Fetch = 0, int? OffSet = null);
        Task<List<SuperMenuVehicleCsv>> GetSuperMenuVehicleCsvData(HyperFormData hyperData, int CompanyId, int TenantId, int Fetch = 0, int? OffSet = null);
        Task<List<SuperMenuReservationReportPDF>> GetSuperMenuReservationReportData(HyperFormData hyperData, int CompanyId, int TenantId);
        Task<List<SuperMenuReservationCsv>> GetSuperMenuReservationCsvData(HyperFormData hyperData, int CompanyId, int TenantId);
        Task<bool> HaitaCheck(List<HaiTaParam> haiTaParams);
    }

    public class HyperDataService : IHyperDataService, IReportService
    {
        private readonly KobodbContext _dbContext;
        // New structor
        private IMediator mediatR;

        public HyperDataService(KobodbContext context, IMediator mediatR)
        {
            _dbContext = context;
            this.mediatR = mediatR;
        }
        public async Task<List<SuperMenuReservationData>> GetSuperMenuReservationData(HyperFormData hyperData, int CompanyId, int TenantId, int? OffSet = null, int Fetch = 0)
        {
            return await mediatR.Send(new GetSuperMenuReservationDataQuery { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId, fetch = Fetch, offSet = OffSet });
        }
        public async Task<SuperMenuReservationTotalGridData> GetSuperMenuReservationTotalData(HyperFormData hyperData, int CompanyId, int TenantId)
        {
            return await mediatR.Send(new GetSuperMenuReservationTotalGridData { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId });
        }
        public async Task<List<HyperGraphData>> GetHyperGraphData(HyperFormData hyperData, int CompanyId, int TenantId, bool IsGetCount = false)
        {
            return await mediatR.Send(new GetHyperGraphDataQuery { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId, isGetCount = IsGetCount });
        }
        public async Task<List<SalePerTime>> GetSaleByDay(List<HyperGraphData> GraphData, string TypeDate, DateTime StartDate, DateTime EndDate)
        {
            return await mediatR.Send(new GetSaleByDayQuery { graphData = GraphData, typeDate = TypeDate, startDate = StartDate, endDate = EndDate });
        }
        public async Task<List<SalePerTime>> GetSaleByMonth(List<HyperGraphData> GraphData, string TypeDate, DateTime StartDate, DateTime EndDate)
        {
            return await mediatR.Send(new GetSaleByMonthQuery { graphData = GraphData, typeDate = TypeDate, startDate = StartDate, endDate = EndDate });
        }
        public async Task<List<SalePerStaff>> GetSaleByStaff(List<HyperGraphData> GraphData, string TypeDate)
        {
            return await mediatR.Send(new GetSaleByStaffQuery { graphData = GraphData, typeDate = TypeDate });
        }
        public async Task<List<SalePerCustomer>> GetSaleByCustomer(List<HyperGraphData> GraphData, string TypeDate)
        {
            return await mediatR.Send(new GetSaleByCustomerQuery { graphData = GraphData, typeDate = TypeDate });
        }
        public async Task<List<SalePerGroupClassification>> GetSaleByGroupClassification(List<HyperGraphData> GraphData, string TypeDate)
        {
            return await mediatR.Send(new GetSaleByGroupClassificationQuery { graphData = GraphData, typeDate = TypeDate });
        }
        public async Task<List<ReservationDataToCheck>> GetDataReservationToCheck(List<string> ReceiptNumber)
        {
            return await mediatR.Send(new GetDataReservationToCheckQuery { receiptNumber = ReceiptNumber });
        }
        public async Task<bool> CancelRevervation(List<string> ReceiptNumber, List<ReservationDataToCheck> listCheck)
        {
            return await mediatR.Send(new CancelRevervationCommand { receiptNumber = ReceiptNumber, lstCheck = listCheck });
        }
        public async Task<bool> ConfirmRevervation(List<string> ReceiptNumber)
        {
            return await mediatR.Send(new ConfirmRevervationCommand { receiptNumber = ReceiptNumber });
        }
        public async Task<List<SuperMenuVehicleData>> GetSuperMenuVehicleData(HyperFormData hyperData, int CompanyId, int TenantId, int? OffSet = null, int Fetch = 0)
        {
            return await mediatR.Send(new GetDataSuperMenuVehicleGridQuery { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId, fetch = Fetch, offSet = OffSet });
        }
        public async Task<SuperMenuVehicleTotalGridData> GetSuperMenuVehicleTotalData(HyperFormData hyperData, int CompanyId, int TenantId)
        {
            return await mediatR.Send(new GetDataForSuperMenuVehicleTotalGridQuery { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId });
        }
        public string CheckValidationForGraph(GraphTypeDisplay type, HyperFormData hyperData, int dateType)
        {
            switch (type)
            {
                case GraphTypeDisplay.GraphSaleStaffDayBar:
                case GraphTypeDisplay.GraphCustomerDayBar:
                case GraphTypeDisplay.GraphSaleDayLine:
                case GraphTypeDisplay.GraphSaleQuanDayBarLine:
                    if ((dateType == (int)DateType.Dispatch && (hyperData.HaishaBiFrom == null || hyperData.HaishaBiTo == null || ((TimeSpan)(hyperData.HaishaBiTo - hyperData.HaishaBiFrom)).Days > 30)) ||
                        (dateType == (int)DateType.Arrival && (hyperData.TochakuBiFrom == null || hyperData.TochakuBiTo == null || ((TimeSpan)(hyperData.TochakuBiTo - hyperData.TochakuBiFrom)).Days > 30)) ||
                        (dateType == (int)DateType.Reservation && (hyperData.YoyakuBiFrom == null || hyperData.YoyakuBiTo == null || ((TimeSpan)(hyperData.YoyakuBiTo - hyperData.YoyakuBiFrom)).Days > 30)))
                    {
                        return "DateSpecifyInvalidForGraph";
                    }
                    break;
                case GraphTypeDisplay.GraphSaleStaffMonthBar:
                case GraphTypeDisplay.GraphCustomerMonthBar:
                case GraphTypeDisplay.GraphSaleMonthLine:
                case GraphTypeDisplay.GraphOrganizationBar:
                case GraphTypeDisplay.GraphOrganizationPie:
                case GraphTypeDisplay.GraphSaleQuanMonthBarLine:
                    if ((dateType == (int)DateType.Dispatch && (hyperData.HaishaBiFrom == null || hyperData.HaishaBiTo == null ||
                            (((DateTime)hyperData.HaishaBiTo).Year - ((DateTime)hyperData.HaishaBiFrom).Year) * 12 + ((DateTime)hyperData.HaishaBiTo).Month - ((DateTime)hyperData.HaishaBiFrom).Month >= 12)) ||
                        (dateType == (int)DateType.Arrival && (hyperData.TochakuBiFrom == null || hyperData.TochakuBiTo == null ||
                            (((DateTime)hyperData.TochakuBiTo).Year - ((DateTime)hyperData.TochakuBiFrom).Year) * 12 + ((DateTime)hyperData.TochakuBiTo).Month - ((DateTime)hyperData.TochakuBiFrom).Month >= 12)) ||
                        (dateType == (int)DateType.Reservation && (hyperData.YoyakuBiFrom == null || hyperData.YoyakuBiTo == null ||
                            (((DateTime)hyperData.YoyakuBiTo).Year - ((DateTime)hyperData.YoyakuBiFrom).Year) * 12 + ((DateTime)hyperData.YoyakuBiTo).Month - ((DateTime)hyperData.YoyakuBiFrom).Month >= 12)))
                    {
                        return "MonthSpecifyInvalidForGraph";
                    }
                    break;
                default:
                    break;
            }
            return "";
        }
        public Color GetColorByIndex(int index)
        {
            List<Color> lstColor = new List<Color>();
            lstColor.Add(Color.FromArgb(26, 179, 148));
            lstColor.Add(Color.FromArgb(40, 167, 69));
            lstColor.Add(Color.FromArgb(220, 53, 69));
            lstColor.Add(Color.FromArgb(255, 193, 7));
            lstColor.Add(Color.FromArgb(23, 162, 184));
            lstColor.Add(Color.FromArgb(108, 117, 125));
            lstColor.Add(Color.FromArgb(130, 110, 69));
            lstColor.Add(Color.FromArgb(238, 123, 38));
            lstColor.Add(Color.FromArgb(139, 178, 96));
            lstColor.Add(Color.FromArgb(66, 140, 155));
            lstColor.Add(Color.FromArgb(67, 148, 137));
            lstColor.Add(Color.FromArgb(33, 173, 109));
            lstColor.Add(Color.FromArgb(100, 129, 69));
            lstColor.Add(Color.FromArgb(160, 91, 69));
            lstColor.Add(Color.FromArgb(243, 146, 28));
            lstColor.Add(Color.FromArgb(178, 183, 66));
            lstColor.Add(Color.FromArgb(232, 100, 48));
            lstColor.Add(Color.FromArgb(175, 82, 69));
            lstColor.Add(Color.FromArgb(246, 158, 23));
            lstColor.Add(Color.FromArgb(51, 147, 164));
            int countColor = lstColor.Count;
            return lstColor[index % countColor];
        }

        public void JoinSaleByStaff(ref List<SalePerStaff> listThisYear, ref List<SalePerStaff> listLastYear)
        {
            join2YearStaff(ref listThisYear, ref listLastYear, 1);
            join2YearStaff(ref listLastYear, ref listThisYear, -1);
        }
        private void join2YearStaff(ref List<SalePerStaff> listThisYear, ref List<SalePerStaff> listLastYear, int addYear)
        {
            Dictionary<string, SalePerStaff> saleLastYear = listLastYear.ToDictionary(x => x.StaffCd, y => y);
            Dictionary<string, SalePerStaff> saleThisYear = listThisYear.ToDictionary(x => x.StaffCd, y => y);
            foreach (var item in listLastYear)
            {
                SalePerStaff sf = new SalePerStaff();
                if (!saleThisYear.ContainsKey(item.StaffCd))
                {
                    sf = new SalePerStaff() { StaffCd = item.StaffCd, StaffSeq = item.StaffSeq, StaffName = item.StaffName, Sale = 0, Time = item.Time.AddYears(addYear) };
                    listThisYear.Add(sf);
                    saleThisYear.Add(item.StaffCd, sf);
                }
            }
            listThisYear = listThisYear.OrderBy(x => x.StaffCd).ToList();
        }
        public void JoinSaleByCustomer(ref List<SalePerCustomer> listThisYear, ref List<SalePerCustomer> listLastYear)
        {
            join2YearSaleByCustomer(ref listThisYear, ref listLastYear, 1);
            join2YearSaleByCustomer(ref listLastYear, ref listThisYear, -1);
        }
        private void join2YearSaleByCustomer(ref List<SalePerCustomer> listThisYear, ref List<SalePerCustomer> listLastYear, int addYear)
        {
            Dictionary<int, SalePerCustomer> saleLastYear = listLastYear.ToDictionary(x => x.BranchSeq, y => y);
            Dictionary<int, SalePerCustomer> saleThisYear = listThisYear.ToDictionary(x => x.BranchSeq, y => y);
            // Compare data customer 2 year.
            foreach (var item in listLastYear)
            {
                if (!saleThisYear.ContainsKey(item.BranchSeq))
                {
                    SalePerCustomer sf = new SalePerCustomer() { CustomerCd = item.CustomerCd, CustomerSeq = item.CustomerSeq, CustomerName = item.CustomerName, BranchCd = item.BranchCd, BranchSeq = item.BranchSeq, BranchName = item.BranchName, Sale = 0, Time = (item.Time.AddYears(addYear)) };
                    listThisYear.Add(sf);
                    saleThisYear.Add(item.BranchSeq, sf);
                }
            }
            listThisYear = listThisYear.OrderBy(x => x.Key).ToList();
        }
        public void JoinSaleByGroupClassification(ref List<SalePerGroupClassification> listThisYear, ref List<SalePerGroupClassification> listLastYear)
        {
            join2YearSaleByGroupClassification(ref listThisYear, ref listLastYear, 1);
            join2YearSaleByGroupClassification(ref listLastYear, ref listThisYear, -1);
        }
        private void join2YearSaleByGroupClassification(ref List<SalePerGroupClassification> listThisYear, ref List<SalePerGroupClassification> listLastYear, int addYear)
        {
            Dictionary<string, SalePerGroupClassification> saleLastYear = listLastYear.ToDictionary(x => x.GroupClassificationCd, y => y);
            Dictionary<string, SalePerGroupClassification> saleThisYear = listThisYear.ToDictionary(x => x.GroupClassificationCd, y => y);
            // Compare data GroupClassification 2 year.
            foreach (var item in listLastYear)
            {
                if (!saleThisYear.ContainsKey(item.GroupClassificationCd))
                {
                    SalePerGroupClassification sf = new SalePerGroupClassification() { GroupClassificationCd = item.GroupClassificationCd, GroupClassificationName = item.GroupClassificationName, Sale = 0, Time = item.Time.AddYears(addYear) };
                    listThisYear.Add(sf);
                    saleThisYear.Add(item.GroupClassificationCd, sf);
                }
            }
            listThisYear = listThisYear.OrderBy(x => x.GroupClassificationCd).ToList();
        }

        public HyperFormData GetDataLastYear(List<HyperFormData> CurrentHyperForm, int GraphIndex, byte isThisYear, string currentYear, bool _isMonth)
        {
            HyperFormData result = new HyperFormData(CurrentHyperForm[GraphIndex]);
            if (isThisYear == 1)
            {
                result.HaishaBiFrom = result.HaishaBiFrom != null ? ChangeLastYear(result.HaishaBiFrom, currentYear, true, _isMonth) : result.HaishaBiFrom;
                result.HaishaBiTo = result.HaishaBiTo != null ? ChangeLastYear(result.HaishaBiTo, currentYear, false, _isMonth) : result.HaishaBiTo;
                result.TochakuBiFrom = result.TochakuBiFrom != null ? ChangeLastYear(result.TochakuBiFrom, currentYear, true, _isMonth) : result.TochakuBiFrom;
                result.TochakuBiTo = result.TochakuBiTo != null ? ChangeLastYear(result.TochakuBiTo, currentYear, false, _isMonth) : result.TochakuBiTo;
                result.YoyakuBiFrom = result.YoyakuBiFrom != null ? ChangeLastYear(result.YoyakuBiFrom, currentYear, true, _isMonth) : result.YoyakuBiFrom;
                result.YoyakuBiTo = result.YoyakuBiTo != null ? ChangeLastYear(result.YoyakuBiTo, currentYear, false, _isMonth) : result.YoyakuBiTo;
            }
            return result;
        }
        private DateTime? ChangeLastYear(DateTime? date, string currentYear, bool isFrom, bool _isMonth)
        {
            DateTime? result = date;
            if (isFrom)
            {
                if (DateTime.IsLeapYear(int.Parse(currentYear)))
                {
                    if (date.Value.Month == 2 && date.Value.Day == 29)
                    {
                        return DateDisplayValue(string.Concat((int.Parse(currentYear) - 1).ToString(), date.Value.AddDays(1).ToString("MMdd")));
                    }
                }
            }
            if (!isFrom)
            {
                if (DateTime.IsLeapYear(int.Parse(currentYear)))
                {
                    if (date.Value.Month == 2 && date.Value.Day == 29)
                    {
                        return DateDisplayValue(string.Concat((int.Parse(currentYear) - 1).ToString(), date.Value.AddDays(-1).ToString("MMdd")));
                    }
                }
                if (DateTime.IsLeapYear(int.Parse(currentYear) - 1))
                {
                    if (date.Value.Month == 2 && date.Value.Day == 28 && _isMonth)
                    {
                        return DateDisplayValue(string.Concat((int.Parse(currentYear) - 1).ToString(), "0229"));
                    }
                }
            }
            return DateDisplayValue(string.Concat((int.Parse(currentYear) - 1).ToString(), date.Value.ToString("yyyyMMdd").Substring(4, 4)));
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
        public async Task<List<SuperMenuVehicleReportPDF>> GetSuperMenuVehicleReportData(HyperFormData hyperData, int CompanyId, int TenantId, int Fetch = 0, int? OffSet = null)
        {
            return await mediatR.Send(new GetSuperMenuVehicleDataReport { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId, fetch = Fetch, offSet = OffSet });
        }
        public async Task<List<SuperMenuVehicleCsv>> GetSuperMenuVehicleCsvData(HyperFormData hyperData, int CompanyId, int TenantId, int Fetch = 0, int? OffSet = null)
        {
            return await mediatR.Send(new GetSuperMenuVehicleDataCsvQuery { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId, fetch = Fetch, offSet = OffSet });
        }
        public async Task<List<SuperMenuReservationReportPDF>> GetSuperMenuReservationReportData(HyperFormData hyperData, int CompanyId, int TenantId)
        {
            return await mediatR.Send(new GetSuperMenuReservationDataReportQuery { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId });
        }
        public async Task<List<SuperMenuReservationCsv>> GetSuperMenuReservationCsvData(HyperFormData hyperData, int CompanyId, int TenantId)
        {
            return await mediatR.Send(new GetSuperMenuReservationDataCsvQuery { hyperData = hyperData, companyId = CompanyId, tenantId = TenantId });
        }
        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<HyperFormData>(queryParams);

            if (searchParams.Type == 1)
            {
                XtraReport report = new Reports.SuperReservationReportA4();
                if (searchParams.PageSize.IdValue == 2)
                {
                    report = new Reports.SuperReservationReportA3();
                }
                else
                {
                    if (searchParams.PageSize.IdValue == 3)
                        report = new Reports.SuperReservationReportB4();
                }
                ObjectDataSource dataSource = new ObjectDataSource();
                var data = await GetSuperMenuReservationReportData(searchParams, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                Parameter param = new Parameter()
                {
                    Name = "data",
                    Type = typeof(List<SuperMenuReservationReportPDF>),
                    Value = data
                };
                dataSource.Name = "objectDataSource1";
                dataSource.DataSource = typeof(SuperReservationReportDS);
                dataSource.Constructor = new ObjectConstructorInfo(param);
                dataSource.DataMember = "_data";
                report.DataSource = dataSource;
                return report;
            }
            else
            {
                XtraReport report = new Reports.SuperVehicleReportA4();
                if (searchParams.PageSize.IdValue == 2)
                {
                    report = new Reports.SuperVehicleReportA3();
                }
                else
                {
                    if (searchParams.PageSize.IdValue == 3)
                        report = new Reports.SuperVehicleReportB4();
                }
                ObjectDataSource dataSource = new ObjectDataSource();
                var data = await GetSuperMenuVehicleReportData(searchParams, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new ClaimModel().TenantID);
                Parameter param = new Parameter()
                {
                    Name = "data",
                    Type = typeof(List<SuperMenuVehicleReportPDF>),
                    Value = data
                };
                dataSource.Name = "objectDataSource1";
                dataSource.DataSource = typeof(SuperVehicleReportDS);
                dataSource.Constructor = new ObjectConstructorInfo(param);
                dataSource.DataMember = "_data";
                report.DataSource = dataSource;
                return report;
            }
        }
        public async Task<bool> HaitaCheck(List<HaiTaParam> haiTaParams)
        {
            return await mediatR.Send(new HaitaCheck { HaiTaParams = haiTaParams });
        }
    }
}
