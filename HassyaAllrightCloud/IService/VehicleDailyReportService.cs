using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.VehicleDailyReport.Commands;
using HassyaAllrightCloud.Application.VehicleDailyReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IVehicleDailyReportService : IReportService
    {
        Task<string> GetCompanyName(int tenantCdSeq, int companyCdSeq);
        Task<List<BusSaleBranchSearch>> GetListBusSaleBranch(int tenantCdSeq);
        Task<List<BusCodeSearch>> GetListBusCode();
        Task<List<ReservationSearch>> GetListReservation();
        Task<List<VehicleDailyReportModel>> GetListVehicleDailyReport(VehicleDailyReportSearchParam searchParams);
        Task<List<CurrentBus>> GetListBusForSearch(VehicleDailyReportSearchParam searchParams);
        Task<List<string>> GetListUnkYmdForSearch(VehicleDailyReportSearchParam searchParams);
        Task<List<string>> GetListUnkYmdForModify(string ukeNo);
        Task<List<VehicleDailyReportData>> GetListVehicleDailyReportForUpdate(VehicleDailyReportModel searchParams);
        Task<bool> SaveVehicleDailyReport(VehicleDailyReportData dto);
        Task<bool> DeleteVehicleDailyReport(VehicleDailyReportData dto);
        Task<List<VehicleDailyReportPDF>> GetPDFData(VehicleDailyReportSearchParam searchParams);
        Task<List<VehicleDailyReportHaisha>> GetHaisha(VehicleDailyReportModel searchParams);
        Task<HaitaCheckVehicleDailyReport> GetHaitaCheck(string ukeNo, short unkRen, short teiDanNo, short bunkRen, string unkYmd);
    }

    public class VehicleDailyReportService : IVehicleDailyReportService
    {
        private readonly IMediator _mediator;
        private readonly IReportLayoutSettingService _reportLayoutSettingService;
        public VehicleDailyReportService(IMediator mediator, IReportLayoutSettingService reportLayoutSettingService)
        {
            _mediator = mediator;
            _reportLayoutSettingService = reportLayoutSettingService;
        }

        public async Task<string> GetCompanyName(int tenantCdSeq, int companyCdSeq)
        {
            return await _mediator.Send(new GetCompanyNameForSearchQuery() { CompanyCdSeq = companyCdSeq, TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<BusSaleBranchSearch>> GetListBusSaleBranch(int tenantCdSeq)
        {
            return await _mediator.Send(new GetListBusSaleBranchForSearchQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<BusCodeSearch>> GetListBusCode()
        {
            return await _mediator.Send(new GetListBusCodeForSearchQuery());
        }

        public async Task<List<ReservationSearch>> GetListReservation()
        {
            return await _mediator.Send(new GetListReservationForSearchQuery());
        }

        public async Task<List<VehicleDailyReportModel>> GetListVehicleDailyReport(VehicleDailyReportSearchParam searchParams)
        {
            return await _mediator.Send(new GetListVehicleDailyReportQuery() { searchParams = searchParams });
        }

        public async Task<List<CurrentBus>> GetListBusForSearch(VehicleDailyReportSearchParam searchParams)
        {
            return await _mediator.Send(new GetListSyaRyoForSearchQuery() { searchParams = searchParams });
        }

        public async Task<List<string>> GetListUnkYmdForSearch(VehicleDailyReportSearchParam searchParams)
        {
            return await _mediator.Send(new GetListUnkYmdForSearchQuery() { searchParams = searchParams });
        }

        public async Task<List<string>> GetListUnkYmdForModify(string ukeNo)
        {
            return await _mediator.Send(new GetListUnkYmdForModifyQuery() { UkeNo = ukeNo });
        }

        public async Task<List<VehicleDailyReportData>> GetListVehicleDailyReportForUpdate(VehicleDailyReportModel searchParams)
        {
            return await _mediator.Send(new GetVehicleDailyReportForEditQuery() { searchParams = searchParams });
        }
        
        public async Task<bool> SaveVehicleDailyReport(VehicleDailyReportData dto)
        {
            return await _mediator.Send(new VehicleDailyReportSaveCommand() { dto = dto });
        }

        public async Task<bool> DeleteVehicleDailyReport(VehicleDailyReportData dto)
        {
            return await _mediator.Send(new VehicleDailyReportDeleteCommand() { dto = dto });
        }

        public async Task<List<VehicleDailyReportPDF>> GetPDFData(VehicleDailyReportSearchParam searchParams)
        {
            var data = new List<VehicleDailyReportPDF>();
            var searchParam = (VehicleDailyReportSearchParam)searchParams.Clone();
            searchParam.SyaRyoCdSeq = 0;
            searchParam.selectedUnkYmd = string.Empty;
            var list = await GetListVehicleDailyReport(searchParam); 

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].UnkYmd == null)
                {
                    list[i].UnkYmd = list[i].SyukoYmd;
                }
                
                var start = DateTime.ParseExact(list[i].SyukoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
                var end = DateTime.ParseExact(list[i].KikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
                var rangeDate = Enumerable.Range(0, 1 + end.Subtract(start).Days).Select(offset => start.AddDays(offset).ToString(CommonConstants.FormatYMD)).ToList();
                var tempCount = i;
                foreach (var date in rangeDate)
                {
                    var count = 0;
                    count = list.Count(_ => _.UnkYmd == date);

                    if (count == 0)
                    {
                        tempCount = tempCount + 1;
                        list.Insert(tempCount, createDefaultModel(date, list[i]));
                    }
                }
                i = tempCount;
            }

            var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMDHm);
            var page = 1;
            string startYmd = searchParams.ScheduleYmdStart == null ? string.Empty : searchParams.ScheduleYmdStart.Value.ToString(CommonConstants.Format2YMD);
            string endYmd = searchParams.ScheduleYmdEnd == null ? string.Empty : searchParams.ScheduleYmdEnd.Value.ToString(CommonConstants.Format2YMD);

            list = list.OrderBy(_ => _.UnkYmd).ToList();
            var listCurrentBus = await GetListBusForSearch(searchParams);
            listCurrentBus.ForEach(e =>
            {
                OnSetDataPerPage(data, list, e, 0, currentDate, ref page);
            });

            data.ForEach(e => {
                e.TotalPage = page - 1;
                e.StrYmd = startYmd;
                e.EndYmd = endYmd;
            });

            return data;
        }

        private VehicleDailyReportModel createDefaultModel(string date, VehicleDailyReportModel baseModel)
        {
            var model = new VehicleDailyReportModel()
            {
                UnkYmd = date,
                SyaryoNm = baseModel.SyaryoNm,
                DanTaNm = baseModel.DanTaNm,
                IkNm = baseModel.IkNm,
                TokuiRyakuNm = baseModel.TokuiRyakuNm,
                SitenRyakuNm = baseModel.SitenRyakuNm,
                HaiSYmd = baseModel.HaiSYmd,
                TouYmd = baseModel.TouYmd,
                SyukoYmd = baseModel.SyukoYmd,
                KikYmd = baseModel.KikYmd,
                Haisha_SyukoTime = baseModel.Haisha_SyukoTime,
                Haisha_KikTime = baseModel.Haisha_KikTime,
                Shabni_SyukoTime = baseModel.Shabni_SyukoTime,
                Shabni_KikTime = baseModel.Shabni_KikTime,
                JyoSyaJin = 0,
                PlusJin = 0,
                JisaIPKm = 0,
                JisaKSKm = 0,
                KisoIPKm = 0,
                KisoKOKm = 0,
                OthKm = 0,
                StMeter = 0,
                EndMeter = 0,
                NenryoRyakuNm1 = baseModel.NenryoRyakuNm1,
                NenryoRyakuNm2 = baseModel.NenryoRyakuNm2,
                NenryoRyakuNm3 = baseModel.NenryoRyakuNm3,
                Nenryo1 = 0,
                Nenryo2 = 0,
                Nenryo3 = 0,
                YoyaKbnNm = baseModel.YoyaKbnNm,
                UkeNo = baseModel.UkeNo,
                UnkRen = baseModel.UnkRen,
                TeiDanNo = baseModel.TeiDanNo,
                BunkRen = baseModel.BunkRen,
                SyainNm1 = baseModel.SyainNm1,
                SyainNm2 = baseModel.SyainNm2,
                SyainNm3 = baseModel.SyainNm3,
                SyainNm4 = baseModel.SyainNm4,
                SyainNm5 = baseModel.SyainNm5,
                DanTaNm2 = baseModel.DanTaNm2,
                SyaRyoCd = baseModel.SyaRyoCd,
            };
            return model;
        }

        private List<VehicleDailyReportChildModel> InitChildList(List<VehicleDailyReportModel> list, List<VehicleDailyReportModel> listPerPage)
        {
            var listChild = new List<VehicleDailyReportChildModel>();

            var temp = new VehicleDailyReportChildModel();
            temp.Text = "頁計";
            listPerPage.ForEach(e =>
            {
                temp.NumberOfTrips += e.UnkKai;
                temp.BoardingPersonnel += e.JyoSyaJin;
                temp.PlusPersonnel += e.PlusJin;
                temp.ActualKmGeneral += e.JisaIPKm;
                temp.ActualKmHighSpeed += e.JisaKSKm;
                temp.ForwardingKmGeneral += e.KisoIPKm;
                temp.ForwardingKmHighSpeed += e.KisoKOKm;
                temp.OtherKm += e.OthKm;
                temp.TotalMile = temp.TotalMile + (e.EndMeter - e.StMeter);
                temp.Fuel1 += e.Nenryo1;
                temp.Fuel2 += e.Nenryo2;
                temp.Fuel3 += e.Nenryo3;
            });

            listChild.Add(temp);

            var tempTotal = new VehicleDailyReportChildModel();
            tempTotal.Text = "累計";
            list.ForEach(e =>
            {
                tempTotal.NumberOfTrips += e.UnkKai;
                tempTotal.BoardingPersonnel += e.JyoSyaJin;
                tempTotal.PlusPersonnel += e.PlusJin;
                tempTotal.ActualKmGeneral += e.JisaIPKm;
                tempTotal.ActualKmHighSpeed += e.JisaKSKm;
                tempTotal.ForwardingKmGeneral += e.KisoIPKm;
                tempTotal.ForwardingKmHighSpeed += e.KisoKOKm;
                tempTotal.OtherKm += e.OthKm;
                tempTotal.TotalMile = temp.TotalMile + (e.EndMeter - e.StMeter);
                tempTotal.Fuel1 += e.Nenryo1;
                tempTotal.Fuel2 += e.Nenryo2;
                tempTotal.Fuel3 += e.Nenryo3;
            });

            listChild.Add(tempTotal);
            return listChild;
        }

        private void OnSetDataPerPage(List<VehicleDailyReportPDF> listData, List<VehicleDailyReportModel> list, dynamic item, byte type, string currentDate, ref int page)
        {
            var itemPerPage = 22;
            List<VehicleDailyReportModel> listTemp = new List<VehicleDailyReportModel>();
            if (type == 0)
            {
                listTemp = list.Where(_ => _.SyaRyoCdSeq == item.SyaRyoCdSeq).ToList();
            }
            else
            {
                listTemp = list.Where(_ => _.UnkYmd == item).ToList();
            }

            if (listTemp.Count > itemPerPage)
            {
                var count = Math.Ceiling(listTemp.Count * 1.0 / itemPerPage);
                for (int i = 0; i < count; i++)
                {
                    var onePage = new VehicleDailyReportPDF();
                    var listPerPage = listTemp.Skip(i * itemPerPage).Take(itemPerPage).ToList();
                    SetData(onePage, listTemp, listPerPage, currentDate, page, item, type, itemPerPage);
                    listData.Add(onePage);
                    page++;
                }
            }
            else
            {
                var onePage = new VehicleDailyReportPDF();
                SetData(onePage, listTemp, listTemp, currentDate, page, item, type, itemPerPage);
                listData.Add(onePage);
                page++;
            }
        }

        private void SetData(VehicleDailyReportPDF onePage, List<VehicleDailyReportModel> list,
            List<VehicleDailyReportModel> listPerPage, string currentDate, int page, dynamic item, int type, int itemPerPage)
        {
            while (listPerPage.Count < itemPerPage) //add theem dong trong
            {
                listPerPage.Add(new VehicleDailyReportModel());
            }
            onePage.ListData = listPerPage;
            onePage.ListData.ForEach(e =>
            {
                e.dayOfWeekTou = GetDayOfWeek(e.TouYmd);
                e.dayOfWeekHai = GetDayOfWeek(e.HaiSYmd);
            });
            onePage.ListTotal = InitChildList(list, listPerPage);
            onePage.CurrentDate = currentDate;
            onePage.PageNumber = page;
            if (type == 0)
                onePage.Bus = item;
        }

        private string GetDayOfWeek(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                var datetime = DateTime.ParseExact(date, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
                return datetime.ToString("(ddd)");
            }
            return string.Empty;
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<VehicleDailyReportSearchParam>(queryParams);
            //XtraReport report = new Reports.VehicleDailyReport(); 
            var paperSize = searchParams.PaperSize != null ? searchParams.PaperSize.Value : (byte)PaperSize.A4;
            XtraReport report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.VehicleDaily, BaseNamespace.VehicleDailyReportTemplate, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, paperSize);
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetPDFData(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<VehicleDailyReportPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(VehicleDailyReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        public async Task<List<VehicleDailyReportHaisha>> GetHaisha(VehicleDailyReportModel searchParams)
        {
            return await _mediator.Send(new GetHaishaForCalculateAmountQuery() { searchParams = searchParams });
        }

        public async Task<HaitaCheckVehicleDailyReport> GetHaitaCheck(string ukeNo, short unkRen, short teiDanNo, short bunkRen, string unkYmd)
        {
            return await _mediator.Send(new GetHaitaCheckQuery() { UkeNo = ukeNo, UnkRen = unkRen, TeiDanNo = teiDanNo, BunkRen = bunkRen, UnkYmd = unkYmd });
        }
    }
}
