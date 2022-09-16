using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Pages.Components.TransportDailyReport;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportDailyReport.Queries
{
    public class GetTransportDailyReportPDFQuery : IRequest<List<TransportDailyReportPDF>>
    {
        public TransportDailyReportSearchParams SearchParam { get; set; }
        public class Handler : IRequestHandler<GetTransportDailyReportPDFQuery, List<TransportDailyReportPDF>>
        {
            private readonly KobodbContext _context;
            private readonly IStringLocalizer<ListData> _lang;
            public Handler(KobodbContext context, IStringLocalizer<ListData> lang)
            {
                _context = context;
                _lang = lang;
            }

            public async Task<List<TransportDailyReportPDF>> Handle(GetTransportDailyReportPDFQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.SearchParam;
                List<TransportDailyReportData> listData = new List<TransportDailyReportData>();

                var connection = _context.Database.GetDbConnection();
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dTransportDailyReports_R";

                command.Parameters.AddWithValue("@OutStei", searchParam.OutputCategory);
                command.Parameters.AddWithValue("@UnkYmd", searchParam.selectedDate == null ? string.Empty : searchParam.selectedDate.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@CompanyCd", searchParam.selectedCompany?.CompanyCd ?? 0);
                command.Parameters.AddWithValue("@StaEigyoCd", searchParam.selectedEigyoFrom?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@EndEigyoCd", searchParam.selectedEigyoTo?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@SyuKbn", searchParam.aggregation?.Value ?? 0);
                command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                listData = MapTableToObjectHelper.ConvertDataTable<TransportDailyReportData>(dt);

                List<TotalTransportDailyReportData> listTotalData = new List<TotalTransportDailyReportData>();

                command = new SqlCommand();
                command.Connection = (SqlConnection)connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PK_dTotalTransportDailyReport_R";

                command.Parameters.AddWithValue("@OutStei", searchParam.OutputCategory);
                command.Parameters.AddWithValue("@UnkYmd", searchParam.selectedDate == null ? string.Empty : searchParam.selectedDate.ToString(CommonConstants.FormatYMD));
                command.Parameters.AddWithValue("@CompanyCd", searchParam.selectedCompany?.CompanyCd ?? 0);
                command.Parameters.AddWithValue("@StaEigyoCd", searchParam.selectedEigyoFrom?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@EndEigyoCd", searchParam.selectedEigyoTo?.EigyoCd ?? 0);
                command.Parameters.AddWithValue("@SyuKbn", searchParam.aggregation?.Value ?? 0);
                command.Parameters.AddWithValue("@TenantCdSeq", searchParam.TenantCdSeq);

                adapter = new SqlDataAdapter(command);

                dt = new DataTable();
                adapter.Fill(dt);

                listTotalData = MapTableToObjectHelper.ConvertDataTable<TotalTransportDailyReportData>(dt);

                await command.Connection.CloseAsync();
                adapter.Dispose();

                var result = GetPDFData(listData, listTotalData, searchParam);

                return result;
            }

            private List<TransportDailyReportPDF> GetPDFData(List<TransportDailyReportData> listData, List<TotalTransportDailyReportData> listTotalData,
                TransportDailyReportSearchParams searchParam)
            {
                byte itemPerPage = 15;
                List<TransportDailyReportPDF> result = new List<TransportDailyReportPDF>();
                var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMDHm);
                var unkoDate = searchParam.selectedDate.ToString(CommonConstants.FormatYMDWithSlashFull);
                var displayLabel = searchParam.OutputCategory == 1 ? _lang["outgoing_childlist_2"] : _lang["returnamount_childlist_2"];

                var listEigyoPDF = (from t in listData.Where(_ => !string.IsNullOrEmpty(_.EigyoCd) && int.Parse(_.EigyoCd) != 0)
                                    group t by t.EigyoCd into temp
                                    select new EigyoSearchData()
                                    {
                                        EigyoCd = temp.Select(_ => int.Parse(_.EigyoCd)).FirstOrDefault(),
                                        RyakuNm = temp.Select(_ => _.EigyoNm).FirstOrDefault()
                                    }).ToList();

                int page = 1;
                foreach (var eigyo in listEigyoPDF)
                {
                    int no = 1;
                    var list = listData.Where(_ => !string.IsNullOrEmpty(_.EigyoCd) && int.Parse(_.EigyoCd) == eigyo.EigyoCd).ToList();
                    var totalData = listTotalData.FirstOrDefault(_ => _.EigyoCd == eigyo.EigyoCd);
                    TotalData total = new TotalData();
                    MapTotal(total, totalData);
                    if (list.Count > itemPerPage)
                    {
                        var count = Math.Ceiling(list.Count * 1.0 / itemPerPage);
                        for (int i = 0; i < count; i++)
                        {
                            bool formatTotal = false;
                            if (i == count - 1)
                            {
                                formatTotal = true;
                            }
                            var listPerPage = list.Skip(i * itemPerPage).Take(itemPerPage).ToList();
                            CalculateTotalData(listPerPage, total);
                            var item = SetItemPerPage(listPerPage, total, currentDate, unkoDate, displayLabel, eigyo.RyakuNm, formatTotal, ref page, ref no);
                            item.isDisplayTotal = formatTotal;
                            result.Add(item);
                        }
                    }
                    else
                    {
                        while (list.Count < itemPerPage)
                        {
                            list.Add(new TransportDailyReportData());
                        }
                        var item = SetItemPerPage(list, total, currentDate, unkoDate, displayLabel, eigyo.RyakuNm, true, ref page, ref no);
                        item.isDisplayTotal = true;
                        result.Add(item);
                    }
                }

                foreach (var item in result)
                {
                    item.TotalPage = page - 1;
                }

                return result;
            }

            private TransportDailyReportPDF SetItemPerPage(List<TransportDailyReportData> listData, TotalData totalData,
            string currentDate, string unkoDate, string displayLabel, string eigyoNm, bool formatTotal, ref int page, ref int no)
            {
                var itemPerPage = new TransportDailyReportPDF();
                itemPerPage.Data = listData;
                int count = no;
                itemPerPage.Data.ForEach(item =>
                {
                    FormatDataPerPage(item, ref count);
                });
                no = count;
                itemPerPage.TotalData = totalData;
                FormatTotalDataPerPage(itemPerPage.TotalData, formatTotal);
                itemPerPage.Page = page++;
                itemPerPage.SyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd;
                itemPerPage.SyainNm = new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name;
                itemPerPage.CurrentDate = currentDate;
                itemPerPage.UnkoDate = unkoDate;
                itemPerPage.DisplayLabel = displayLabel;
                itemPerPage.EigyoNm = eigyoNm;
                return itemPerPage;
            }

            private void CalculateTotalData(List<TransportDailyReportData> listData, TotalData totalData)
            {
                totalData.SumJyoSyaJin = listData.Sum(_ => int.Parse(_.JyoSyaJin)).ToString();
                totalData.SumSyaRyoUnc = listData.Sum(_ => _.SyaRyoUnc).ToString();
                totalData.SumSyaRyoTes = listData.Sum(_ => _.SyaRyoTes).ToString();
                totalData.SumJisaIPKm = listData.Sum(_ => decimal.Parse(_.Total_JisaIPKm)).ToString();
                totalData.SumJisaKSKm = listData.Sum(_ => decimal.Parse(_.Total_JisaKSKm)).ToString();
                totalData.SumKisoIPKm = listData.Sum(_ => decimal.Parse(_.Total_KisoIPKm)).ToString();
                totalData.SumKisoKOKm = listData.Sum(_ => decimal.Parse(_.Total_KisoKSKm)).ToString();
                totalData.SumOthKm = listData.Sum(_ => decimal.Parse(_.Total_OthKm)).ToString();
                totalData.SumTotalKm = listData.Sum(_ => decimal.Parse(_.Total_TotalKm)).ToString();
                totalData.SumNenryo1 = listData.Sum(_ => decimal.Parse(_.Nenryo1)).ToString();
                totalData.SumNenryo2 = listData.Sum(_ => decimal.Parse(_.Nenryo2)).ToString();
                totalData.SumNenryo3 = listData.Sum(_ => decimal.Parse(_.Nenryo3)).ToString();
            }

            private void FormatDataPerPage(TransportDailyReportData item, ref int count)
            {
                item.No = (count++).AddCommas();
                item.Total_JisaIPKm = item.Total_JisaIPKm.AddCommas();
                item.Total_JisaKSKm = item.Total_JisaKSKm.AddCommas();
                item.Total_KisoIPKm = item.Total_KisoIPKm.AddCommas();
                item.Total_KisoKSKm = item.Total_KisoKSKm.AddCommas();
                item.Total_OthKm = item.Total_OthKm.AddCommas();
                item.Total_TotalKm = item.Total_TotalKm.AddCommas();
                item.Nenryo1 = item.Nenryo1.AddCommas();
                item.Nenryo2 = item.Nenryo2.AddCommas();
                item.Nenryo3 = item.Nenryo3.AddCommas();
            }


            private void FormatTotalDataPerPage(TotalData item, bool formatTotal)
            {
                if (formatTotal)
                {
                    item.CurMonthJyoSyaJin = item.CurMonthJyoSyaJin.AddCommas();
                    item.CurMonthSyaRyoUnc = item.CurMonthSyaRyoUnc.AddCommas();
                    item.CurMonthSyaRyoTes = item.CurMonthSyaRyoTes.AddCommas();
                    item.CurMonthJisaIPKm = item.CurMonthJisaIPKm.AddCommas();
                    item.CurMonthJisaKSKm = item.CurMonthJisaKSKm.AddCommas();
                    item.CurMonthKisoIPKm = item.CurMonthKisoIPKm.AddCommas();
                    item.CurMonthKisoKOKm = item.CurMonthKisoKOKm.AddCommas();
                    item.CurMonthOthKm = item.CurMonthOthKm.AddCommas();
                    item.CurMonthTotalKm = item.CurMonthTotalKm.AddCommas();
                    item.CurMonthNenryo1 = item.CurMonthNenryo1.AddCommas();
                    item.CurMonthNenryo2 = item.CurMonthNenryo2.AddCommas();
                    item.CurMonthNenryo3 = item.CurMonthNenryo3.AddCommas();
                }
                item.SumJyoSyaJin = item.SumJyoSyaJin.AddCommas();
                item.SumSyaRyoUnc = item.SumSyaRyoUnc.AddCommas();
                item.SumSyaRyoTes = item.SumSyaRyoTes.AddCommas();
                item.SumJisaIPKm = item.SumJisaIPKm.AddCommas();
                item.SumJisaKSKm = item.SumJisaKSKm.AddCommas();
                item.SumKisoIPKm = item.SumKisoIPKm.AddCommas();
                item.SumKisoKOKm = item.SumKisoKOKm.AddCommas();
                item.SumOthKm = item.SumOthKm.AddCommas();
                item.SumTotalKm = item.SumTotalKm.AddCommas();
                item.SumNenryo1 = item.SumNenryo1.AddCommas();
                item.SumNenryo2 = item.SumNenryo2.AddCommas();
                item.SumNenryo3 = item.SumNenryo3.AddCommas();
            }

            private void MapTotal(TotalData total, TotalTransportDailyReportData totalData)
            {
                total.SumJyoSyaJin = totalData.SumJyoSyaJin;
                total.SumSyaRyoUnc = totalData.SumSyaRyoUnc;
                total.SumSyaRyoTes = totalData.SumSyaRyoTes;
                total.SumJisaIPKm = totalData.SumJisaIPKm;
                total.SumJisaKSKm = totalData.SumJisaKSKm;
                total.SumKisoIPKm = totalData.SumKisoIPKm;
                total.SumKisoKOKm = totalData.SumKisoKOKm;
                total.SumOthKm = totalData.SumOthKm;
                total.SumTotalKm = totalData.SumTotalKm;
                total.SumNenryo1 = totalData.SumNenryo1;
                total.SumNenryo2 = totalData.SumNenryo2;
                total.SumNenryo3 = totalData.SumNenryo3;
                total.CurMonthSyaRyoUnc = totalData.CurMonthSyaRyoUnc;
                total.CurMonthSyaRyoTes = totalData.CurMonthSyaRyoTes;
                total.CurMonthJyoSyaJin = totalData.CurMonthJyoSyaJin;
                total.CurMonthPlusJin = totalData.CurMonthPlusJin;
                total.CurMonthJisaIPKm = totalData.CurMonthJisaIPKm;
                total.CurMonthJisaKSKm = totalData.CurMonthJisaKSKm;
                total.CurMonthKisoIPKm = totalData.CurMonthKisoIPKm;
                total.CurMonthKisoKOKm = totalData.CurMonthKisoKOKm;
                total.CurMonthOthKm = totalData.CurMonthOthKm;
                total.CurMonthTotalKm = totalData.CurMonthTotalKm;
                total.CurMonthNenryo1 = totalData.CurMonthNenryo1;
                total.CurMonthNenryo2 = totalData.CurMonthNenryo2;
                total.CurMonthNenryo3 = totalData.CurMonthNenryo3;
            }
        }
    }
}
