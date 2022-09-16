using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.VehicleStatisticsSurvey.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IVehicleStatisticsSurveyService : IReportService
    {
        Task<List<VehicleStatisticsSurveyPDF>> GetPDFData(VehicleStatisticsSurveySearchParam searchModel);
    }
    public class VehicleStatisticsSurveyService : IVehicleStatisticsSurveyService
    {
        private readonly IMediator _mediator;
        public VehicleStatisticsSurveyService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<VehicleStatisticsSurveyPDF>> GetPDFData(VehicleStatisticsSurveySearchParam searchModel)
        {
            List<VehicleStatisticsSurveyPDF> final = new List<VehicleStatisticsSurveyPDF>();
            var list = await _mediator.Send(new GetListVehicleStatisticsSurvey() { searchParam = searchModel });
            for(int i = 0; i < 3; i++)
            {
                var listTemp = list.Where(_ => _.UnsouKbn == (i + 1)).ToList();
                if(listTemp.Count > 0)
                {
                    VehicleStatisticsSurveyPDF finalItem = new VehicleStatisticsSurveyPDF();
                    finalItem.UnsouKbnNm = listTemp[0].UnsouKbnNm;
                    finalItem.ProcessingDate = searchModel.ProcessingDate.ToString(CommonConstants.FormatYM);
                    if(listTemp[0].UnsouKbn == (byte)ShippingType.Specific)
                    {
                        finalItem.Shipping = 4;
                    }
                    else
                    {
                        finalItem.Shipping = 3;
                    }
                    foreach (var item in listTemp)
                    {
                        finalItem.SumYosuJin += item.YusoJin;
                        finalItem.SumNobeSumCnt += item.NobeSumCnt;
                        finalItem.SumNobeJitCnt += item.NobeJitCnt;
                        finalItem.SumJitSumKm += (int)item.JitSumKm;
                        finalItem.SumJitJisaKm += (int)item.JitJisaKm;
                        finalItem.SumJitKisoKm += (int)item.JitKisoKm;
                        finalItem.SumUnkoCnt += item.UnkoCnt;
                        finalItem.SumNobeRinCnt += item.NobeRinCnt;
                        if(item.NenryoKbn == 5)
                        {
                            if(item.LastMonthYusoJin == 0)
                            {
                                finalItem.SumLastMonthYusoJin = "000";
                            }
                            else
                            {
                                finalItem.SumLastMonthYusoJin = (finalItem.SumYosuJin * 1.0 / item.LastMonthYusoJin * 100).ToString("N2").Replace(".", "");
                            }
                            finalItem.EndOfMonthCnt = item.EndOfMonthCnt;
                        }
                    }
                    if (finalItem.SumNobeJitCnt == 0)
                    {
                        finalItem.SumNobe = "000";
                        finalItem.SumJit = "000";
                        finalItem.SumUnko = "000";
                    }
                    else
                    {
                        finalItem.SumNobe = (finalItem.SumYosuJin * 1.0 / finalItem.SumNobeJitCnt).ToString("N2").Replace(".", "");
                        finalItem.SumJit = (finalItem.SumJitSumKm * 1.0 / finalItem.SumNobeJitCnt).ToString("N2").Replace(".", "");
                        finalItem.SumUnko = (finalItem.SumUnkoCnt * 1.0 / finalItem.SumNobeJitCnt).ToString("N2").Replace(".", "");
                    }
                    final.Add(finalItem);
                }
            }
            return final;
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<VehicleStatisticsSurveySearchParam>(queryParams);
            XtraReport report = new HassyaAllrightCloud.Reports.ReportTemplate.VehicleStatisticsSurvey.VehicleStatisticsSurvey();

            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetPDFData(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<VehicleStatisticsSurveyPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(VehicleStatisticsSurveyDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;

            report.BeforePrint += (sender, e) =>
            {
                var temp = (sender as Reports.ReportTemplate.VehicleStatisticsSurvey.VehicleStatisticsSurvey);
                var labels = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("lblVisible")).ToList();
                foreach (var label in labels)
                {
                    label.PrintOnPage += (labelsender, labele) =>
                    {
                        var index = labele.PageIndex;
                        if (data[index].Shipping == 4)
                        {
                            (labelsender as XRLabel).Visible = false;
                        }
                    };
                }
            };

            return report;
        }

        public enum ShippingType
        {
            General = 1,
            Specific = 2,
            Special = 3
        }
    }
}
