using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Extensions;

namespace HassyaAllrightCloud.Application.SimpleQuotationReport.Queries
{
    public class GetSimpleQuotationReportQuery : IRequest<List<SimpleQuotationDataReport>>
    {
        private readonly List<BookingKeyData> _bookingKeyList;
        private readonly int _tenantId;
        private readonly bool _isWithJourney;
        private readonly bool _isDisplayMinMaxPrice;

        public GetSimpleQuotationReportQuery(List<BookingKeyData> bookingKeyList, int tenantId, bool isWithJourney, bool isDisplayMinMaxPrice)
        {
            _bookingKeyList = bookingKeyList ?? throw new ArgumentNullException(nameof(List<BookingKeyData>), "Cannot get report simple quotation with null key list");
            _bookingKeyList = _bookingKeyList.Where(b => !string.IsNullOrWhiteSpace(b.UkeNo)).ToList();
            _tenantId = tenantId;
            _isWithJourney = isWithJourney;
            _isDisplayMinMaxPrice = isDisplayMinMaxPrice;
        }

        public class Handler : IRequestHandler<GetSimpleQuotationReportQuery, List<SimpleQuotationDataReport>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<SimpleQuotationDataReport>> Handle(GetSimpleQuotationReportQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var resultList = new List<SimpleQuotationDataReport>();
                    var connection = _context.Database.GetDbConnection();
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Pro_GetSimpleQuotation_R";
                    command.Parameters.AddWithValue("@tenantId", request._tenantId);
                    command.Parameters.AddWithValue("@isWithJourney", request._isWithJourney);
                    command.Parameters.AddWithValue("@bookingKeys", request._bookingKeyList.Select(_ => new { _.UkeNo, _.UnkRen }).ToDataTable());

                    await command.Connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var headerList = new List<SimpleQuotationDataReport.Header>();
                        while (await reader.ReadAsync())
                        {
                            var header = new SimpleQuotationDataReport.Header();
                            header.UkeNo = reader["UkeNo"].ToString();
                            header.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                            header.UkeCd = Convert.ToInt64(reader["UkeCd"]);
                            header.DantaNm = reader["UnDanTaNm"].ToString();
                            header.CompanyNm = reader["CompanyNm"].ToString();
                            header.EigyoNm = reader["EiEigyoNm"].ToString();
                            header.Jyus1 = reader["EiJyus1"].ToString();
                            header.Jyus2 = reader["EiJyus2"].ToString();
                            header.Eigyos_TelNo = reader["EiTelNo"].ToString();
                            header.Eigyos_FaxNo = reader["EiFaxNo"].ToString();
                            header.SyainNm = reader["StaffSyainNm"].ToString();
                            header.HaiSYmd = reader["UnHaiSYmd"].ToString();
                            header.TouYmd = reader["UnTouYmd"].ToString();
                            header.JyoSyaJin = Convert.ToInt16(reader["UnJyoSyaJin"]);
                            header.PlusJin = Convert.ToInt16(reader["UnPlusJin"]);
                            header.TokuiNm = reader["TokuTokuiNm"].ToString();
                            header.TokuRyakuNm = reader["TokuRyakuNm"].ToString();
                            header.KanJNm = reader["UnKanJNm"].ToString();
                            header.TokiSt_TelNo = reader["ShitenTelNo"].ToString();
                            header.TokiSt_FaxNo = reader["ShitenFaxNo"].ToString();
                            header.MitBiko = reader["MitBiko"].ToString();
                            headerList.Add(header);
                        }

                        await reader.NextResultAsync();
                        var footerList = new List<SimpleQuotationDataReport.Footer>();
                        while (await reader.ReadAsync())
                        {
                            var footer = new SimpleQuotationDataReport.Footer();
                            footer.UkeNo = reader["UkeNo"].ToString();
                            footer.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                            footer.SoukouKiro = Convert.ToInt64(reader["SoukouKiro"]);
                            footer.SoukouTime = Convert.ToInt64(reader["SoukouTime"]);
                            footer.MaxPrice = Convert.ToInt64(reader["MaxPrice"]);
                            footer.MinPrice = Convert.ToInt64(reader["MinPrice"]);
                            footer.IsDisplayMinMaxPrice = request._isDisplayMinMaxPrice;
                            footerList.Add(footer);
                        }

                        await reader.NextResultAsync();
                        var bodyDataList = new List<SimpleQuotationDataReport.Body>();
                        while (await reader.ReadAsync())
                        {
                            var bodyFare = new SimpleQuotationDataReport.Body();
                            bodyFare.RowType = Convert.ToByte(reader["RowType"]);
                            bodyFare.UkeNo = reader["UkeNo"].ToString();
                            bodyFare.ZeiKbn = Convert.ToByte(reader["ZeiKbn"]);
                            bodyFare.ItemName = reader["ItemName"].ToString();
                            bodyFare.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                            bodyFare.Suryo = Convert.ToInt16(reader["Quantity"]);
                            bodyFare.TaxTypeName = reader["TaxName"].ToString();
                            bodyFare.Tanka = Convert.ToInt32(reader["Price"]);
                            bodyFare.Zeiritsu = Convert.ToDecimal(reader["Zeiritsu"]);
                            bodyFare.SyohiHasu = Convert.ToByte(reader["SyohiHasu"]);
                            bodyDataList.Add(bodyFare);
                        }

                        await reader.NextResultAsync();
                        while (await reader.ReadAsync())
                        {
                            var bodyncidental = new SimpleQuotationDataReport.Body();
                            bodyncidental.RowType = 3;
                            bodyncidental.UkeNo = reader["UkeNo"].ToString();
                            bodyncidental.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                            bodyncidental.ZeiKbn = Convert.ToByte(reader["ZeiKbn"]);
                            bodyncidental.FutTumKbn = Convert.ToByte(reader["FutTumKbn"]);
                            bodyncidental.ItemName = reader["FutTumNm"].ToString();
                            bodyncidental.BikoNm = reader["BikoNm"].ToString();
                            bodyncidental.Suryo = Convert.ToInt16(reader["Suryo"]);
                            bodyncidental.TaxTypeName = reader["TaxName"].ToString();
                            bodyncidental.Tanka = Convert.ToInt32(reader["TanKa"]);
                            bodyncidental.Zeiritsu = Convert.ToDecimal(reader["Zeiritsu"]);
                            bodyncidental.SyohiHasu = Convert.ToByte(reader["SyohiHasu"]);
                            bodyncidental.FutGuiKbn = Convert.ToByte(reader["FutGuiKbn"]);
                            bodyDataList.Add(bodyncidental);
                        }

                        await reader.NextResultAsync();
                        var footerCarCountList = new List<SimpleQuotationDataReport.FooterCarCount>();
                        var koteiList = new List<SimpleQuotationDataReport.BodyJourney>();
                        var tehaiList = new List<SimpleQuotationDataReport.BodyJourney>();
                        if (request._isWithJourney == false)
                        {
                            while (await reader.ReadAsync())
                            {
                                var footer = new SimpleQuotationDataReport.FooterCarCount();
                                footer.UkeNo = reader["UkeNo"].ToString();
                                footer.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                                footer.Value = Convert.ToInt64(reader["SumSyaSyuDai"]);
                                footer.CodeKbnNm = reader["CodeKbnNm"].ToString();
                                footer.CodeKbn = reader["CodeKbn"].ToString();
                                footerCarCountList.Add(footer);
                            }
                        }
                        else
                        {
                            while (await reader.ReadAsync())
                            {
                                var journey = new SimpleQuotationDataReport.BodyJourney();
                                journey.UkeNo = reader["UkeNo"].ToString();
                                journey.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                                journey.HaiSYmd = reader["HaiSYmd"].ToString();
                                journey.Nittei = Convert.ToInt16(reader["Nittei"]);
                                journey.Koutei = reader["Koutei"].ToString();
                                koteiList.Add(journey);
                            }

                            await reader.NextResultAsync();
                            while (await reader.ReadAsync())
                            {
                                var journey = new SimpleQuotationDataReport.BodyJourney();
                                journey.UkeNo = reader["UkeNo"].ToString();
                                journey.UnkRen = Convert.ToInt32(reader["UnkRen"]);
                                journey.HaiSYmd = reader["HaiSYmd"].ToString();
                                journey.Nittei = Convert.ToInt16(reader["Nittei"]);
                                journey.TehNm = reader["TehNm"].ToString();
                                tehaiList.Add(journey);
                            }
                        }

                        foreach (var headerItem in headerList)
                        {
                            var pagedDataRp = new SimpleQuotationDataReport();

                            pagedDataRp.HeaderData = headerItem;
                            pagedDataRp.BodyDataList = bodyDataList.Where(b => b.UkeNo == headerItem.UkeNo && b.UnkRen == headerItem.UnkRen).ToList();
                            pagedDataRp.KoteiDataList = koteiList.Where(b => b.UkeNo == headerItem.UkeNo && b.UnkRen == headerItem.UnkRen).ToList();
                            pagedDataRp.TehaiDataList = tehaiList.Where(b => b.UkeNo == headerItem.UkeNo && b.UnkRen == headerItem.UnkRen).ToList();
                            pagedDataRp.FooterData = footerList.Where(b => b.UkeNo == headerItem.UkeNo && b.UnkRen == headerItem.UnkRen).FirstOrDefault();
                            pagedDataRp.FooterCarCountList = footerCarCountList.Where(b => b.UkeNo == headerItem.UkeNo && b.UnkRen == headerItem.UnkRen).ToList();

                            pagedDataRp.UpdateAllSummaryFields();

                            resultList.Add(pagedDataRp);
                        }
                    }

                    await command.Connection.CloseAsync();
                    return resultList;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
