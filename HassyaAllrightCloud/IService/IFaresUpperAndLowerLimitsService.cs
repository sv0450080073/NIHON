using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.FaresUpperAndLowerLimits.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IFaresUpperAndLowerLimitsService : IReportService
    {
        Task<CommonListCombobox> GetCommonCombobox();
        Task<List<Cause>> GetCauseCombobox();
        Task<bool> SaveOrUpdateCause(TkdMaxMinFareFeeCause item, bool isSave);
        Task<bool> CheckCauseIsExist(TkdMaxMinFareFeeCauseParam item);
        Task<TkdMaxMinFareFeeCause> GetFirstOrDefaultTKDMaxMinFareFeeCause(TkdMaxMinFareFeeCauseParam param);
        Task<List<FaresUpperAndLowerLimitObject>> GetFaresUpperAndLowerLimitsList(FaresUpperAndLowerLimitsFormSearch searchModel);
        Task<FaresUpperAndLowerLimitReport> GetDataReport(FaresUpperAndLowerLimitsFormSearch searchParam);
        Task<List<FaresUpperAndLowerLimitCsv>> GetDataCsv(FaresUpperAndLowerLimitsFormSearch searchParam);
    }
    public class FaresUpperAndLowerLimitsService : IFaresUpperAndLowerLimitsService
    {
        private readonly IMediator _mediator;
        private readonly KobodbContext _context;

        public FaresUpperAndLowerLimitsService(IMediator mediator, KobodbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<CommonListCombobox> GetCommonCombobox()
        {
            return await _mediator.Send(new GetCommonCombobox());
        }

        public async Task<List<FaresUpperAndLowerLimitObject>> GetFaresUpperAndLowerLimitsList(FaresUpperAndLowerLimitsFormSearch searchModel)
        {
            return await _mediator.Send(new GetFaresUpperAndLowerLimitsList { SearchParam = searchModel });
        }

        public async Task<List<Cause>> GetCauseCombobox()
        {
            return await _mediator.Send(new GetCauseCombobox());
        }

        public async Task<bool> SaveOrUpdateCause(TkdMaxMinFareFeeCause causeItem, bool isSave)
        {
            return await _mediator.Send(new SaveOrUpdateCause { CauseItem = causeItem, IsSaveCause = isSave });
        }

        public async Task<bool> CheckCauseIsExist(TkdMaxMinFareFeeCauseParam causeItem)
        {
            return await _mediator.Send(new CheckCauseIsExist { Param = causeItem });
        }

        public async Task<TkdMaxMinFareFeeCause> GetFirstOrDefaultTKDMaxMinFareFeeCause(TkdMaxMinFareFeeCauseParam causeItem)
        {
            return await _mediator.Send(new GetFirstOrDefaultTKDMaxMinFareFeeCause { Param = causeItem });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<FaresUpperAndLowerLimitsFormSearch>(queryParams);
            XtraReport report = new Reports.ReportTemplate.FaresUpperAndLowerLimits.FaresUpperAndLowerLimitsA4();
            if (searchParams.FormSetting == FormSetting.A3.ToString())
                report = new Reports.ReportTemplate.FaresUpperAndLowerLimits.FaresUpperAndLowerLimitsA3();
            else if (searchParams.FormSetting == FormSetting.B4.ToString())
                report = new Reports.ReportTemplate.FaresUpperAndLowerLimits.FaresUpperAndLowerLimitsB4();
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetDataReport(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(FaresUpperAndLowerLimitReport),
                Value = data
            };
            dataSource.Name = "objectDataSource5";
            dataSource.DataSource = typeof(FaresUpperAndLowerLimitReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        public async Task<FaresUpperAndLowerLimitReport> GetDataReport(FaresUpperAndLowerLimitsFormSearch searchParam)
        {
            var result = (await GetFaresUpperAndLowerLimitsList(searchParam)).Select(x => new FaresUpperAndLowerLimitGrid
            {
                GridReservationNumber = x.UkeNo.Substring(5, 10),
                GridOperationYmd = $"{DateTime.ParseExact(x.HaiSYmd, Formats.yyyyMMdd, null).ToString(Commons.DateTimeFormat.yyyyMMddSlash) } ～ {DateTime.ParseExact(x.TouYmd, Formats.yyyyMMdd, null).ToString(Commons.DateTimeFormat.yyyyMMddSlash)}",
                GridVehicleName = x.SyaryoNm,
                GridPlan = "計画",
                GridActualResult = "実績",
                GridPlanMinAmount = x?.MitsumoriSumMinAmount.ToString("#,##0"),
                GridPlanMaxAmount = x?.MitsumoriSumMaxAmount.ToString("#,##0"),
                GridActualMinAmount = x?.JissekiSumMinAmount.ToString("#,##0"),
                GridActualMaxAmount = x?.JissekiSumMaxAmount.ToString("#,##0"),
                GridBillingAmount = x?.SeikyuGaku > x?.JissekiSumMaxAmount ? ($"▲{x?.SeikyuGaku.ToString("#,##0")}") : (x?.SeikyuGaku < x?.JissekiSumMinAmount ? ($"▼{x?.SeikyuGaku.ToString("#,##0")}") : x?.SeikyuGaku.ToString("#,##0")),
                GridCause = x.CauseNm?.Trim() ?? "",
                GridPlanRunningKilomet = x?.RunningKmSum.ToString("#,##0.#0"),
                GridPlanTotalTime = $"{Convert.ToInt32(x.RestraintTimeSum.Trim()).ToString("D4").Substring(0, 2)}:{Convert.ToInt32(x.RestraintTimeSum.Trim()).ToString("D4").Substring(2, 2)}",
                GridActualRunningKilomet = (x?.EndMeter - x?.StMeter)?.ToString("#,##0.#0"),
                GridActualTotalTime = $"{CaculateTime(x.KoskuTime, x.InspectionTime).Item1}:{CaculateTime(x.KoskuTime, x.InspectionTime).Item2}",
                GridChangedDriver = x.ChangeDriverFeeFlag == 1 ? "〇" : "",
                GridSpecialVehicle = x.SpecialFlg == 1 ? "〇" : "",
                GridDisabledPersonDiscount = x?.DisabledPersonDiscount == 1 ? "〇" : "",
                GridSchoolDiscount = x?.SchoolDiscount == 1 ? "〇" : "",
                CssClass = (x?.SeikyuGaku > x?.JissekiSumMaxAmount ? "fa fa-caret-up" : (x?.SeikyuGaku < x?.JissekiSumMinAmount ? "fa fa-caret-down" : "")),
                UnkRenGrid = x?.UnkRen ?? 0,
                SyaSyuRenGrid = x?.SyaSyuRen ?? 0,
                TeiDanNoGrid = x?.TeiDanNo ?? 0,
                BunkRenGrid = x?.BunkRen ?? 0
            }).ToList();
            return new FaresUpperAndLowerLimitReport
            {
                FaresUpperAndLowerLimitGrid = result,
                SaleOfficeName = searchParam?.SaleOffice?.RyakuNm,
                OutputStartEndDate = $"{searchParam?.OutputStartDate?.ToString(Commons.DateTimeFormat.yyyyMMddSlash)} ～ {searchParam?.OutputEndDate?.ToString(Commons.DateTimeFormat.yyyyMMddSlash)}",
                CurrentDate = DateTime.Now.ToString(Formats._ddMMyyyy)
            };
        }

        public async Task<List<FaresUpperAndLowerLimitCsv>> GetDataCsv(FaresUpperAndLowerLimitsFormSearch searchParam)
        {
            var data = (await GetFaresUpperAndLowerLimitsList(searchParam)).Select(x => new FaresUpperAndLowerLimitCsv
            {
                GridReservationNumber = x.UkeNo.Substring(5, 10),
                DateDispatch = x.HaiSYmd,
                DateArrival = x.TouYmd,
                GridVehicleName = x.SyaryoNm,
                GridPlanMinAmount = x.MitsumoriSumMinAmount.ToString(),
                GridPlanMaxAmount = x.MitsumoriSumMaxAmount.ToString(),
                GridActualMinAmount = x.JissekiSumMinAmount.ToString(),
                GridActualMaxAmount = x.JissekiSumMaxAmount.ToString(),
                GridBillingAmount = x.SeikyuGaku.ToString(),
                GridCause = x.CodeKbnNm,
                GridPlanRunningKilomet = x.RunningKmSum.ToString(),
                GridPlanTotalTime = x.RestraintTimeSum.ToString(),
                GridActualRunningKilomet = (x.EndMeter - x.StMeter).ToString(),
                GridActualTotalTime = $"{CaculateTime(x.KoskuTime, x.InspectionTime).Item1}{CaculateTime(x.KoskuTime, x.InspectionTime).Item2}",
                GridChangedDriver = x.ChangeDriverFeeFlag == 0 ? "なし" : "ある",
                GridSpecialVehicle = x.SpecialFlg == 0 ? "なし" : "ある",
                GridDisabledPersonDiscount = x.DisabledPersonDiscount == 0 ? "なし" : "ある",
                GridSchoolDiscount = x.SchoolDiscount == 0 ? "なし" : "ある",
            }).ToList();
            return data;
        }

        private Tuple<string, string> CaculateTime(string time1, string time2)
        {
            var hours = time1.Substring(0, 2) + time2.Substring(0, 2);
            var minutes = time1.Substring(1, 2) + time2.Substring(1, 2);
            var sumHours = (Convert.ToInt32(hours) + (Convert.ToInt32(minutes) / 60)).ToString();
            var sumMinutes = (Convert.ToInt32(minutes) % 60).ToString();
            if (sumHours.Length == 1)
                sumHours = sumHours.PadLeft(2, '0');
            if (sumMinutes.Length == 1)
                sumMinutes = sumHours.PadLeft(2, '0');
            return new Tuple<string, string>(sumHours, sumMinutes);
        }
    }
}
