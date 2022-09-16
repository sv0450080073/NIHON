using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ReceiptOutput.Queries
{
    public class GetReceiptOutputReport : IRequest<List<ReceiptOutputReport>>
    {
        public ReceiptOutputFormSeachModel SearchModel { get; set; }

        public class Handler : IRequestHandler<GetReceiptOutputReport, List<ReceiptOutputReport>>
        {
            private readonly KobodbContext _context;
            private readonly decimal Zeiritsu = (decimal)8;
            private readonly string ReducedTaxClass = "※";

            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<ReceiptOutputReport>> Handle(GetReceiptOutputReport request, CancellationToken cancellationToken)
            {
                try
                {
                    var curentUser = new ClaimModel();
                    var headerResultOutput = new List<HeaderResultOutput>();
                    var receiptOutputReport = new ReceiptOutputReport();
                    var detailedResultOutput = new List<DetailedResultOutput>();
                    int page = 1;

                    if (request?.SearchModel != null)
                    {
                        _context.LoadStoredProc("dbo.PK_ReceiptDetails_NoDetails")
                                     .AddParam("@IssueDate", request?.SearchModel?.IssueDate?.ToString(Formats.SlashyyyyMMdd) ?? "")
                                     .AddParam("@SyainCdSeq", curentUser.SyainCdSeq.ToString())
                                     .AddParam("@EigyoCdSeq", request?.SearchModel?.BillOffice?.EigyoCdSeq.ToString())
                                     .AddParam("@SeiOutSeqSeiRen", request?.SearchModel?.SeiOutSeqSeiRen.ToString())
                                     .AddParam("@UpdPrgID", Common.UpdPrgId)
                                     .AddParam("@TenantCdSeq", curentUser.TenantID)
                                     .AddParam("@RyoOutSeqOut", out IOutParam<int> ryoOutSeq)
                                     .AddParam("@RowCount", out IOutParam<int> rowCount)
                                     .Exec(r =>
                                     {
                                         headerResultOutput = r.ToList<HeaderResultOutput>();
                                         r.NextResult();
                                         receiptOutputReport.DetailedResultOutput = r.ToList<DetailedResultOutput>();
                                     });
                        List<ReceiptOutputReport> receiptOutputReporttList = new List<ReceiptOutputReport>();
                        int pageNoreceipt = headerResultOutput.Count() + 1;

                        headerResultOutput.ForEach(e =>
                        {
                            var mainInfo = headerResultOutput.Where(x => x.SeiOutSeq == e.SeiOutSeq && x.SeiRen == e.SeiRen).FirstOrDefault();
                            mainInfo.SeiHatYmd = !string.IsNullOrEmpty(mainInfo.SeiHatYmd?.Trim()) ? $"{DateTime.ParseExact(mainInfo.SeiHatYmd, Formats.yyyyMMdd, null).ToString("yyyy")}" +
                                                 $" 年 {DateTime.ParseExact(mainInfo.SeiHatYmd, Formats.yyyyMMdd, null).ToString("MM")}" +
                                                 $" 月 {DateTime.ParseExact(mainInfo.SeiHatYmd, Formats.yyyyMMdd, null).ToString("dd")}" : "";
                            mainInfo.ZipCd = "〒 " + mainInfo.ZipCd;
                            mainInfo.SeiEigZipCd = "〒 " + mainInfo.SeiEigZipCd;
                            mainInfo.PageReceipt = page++;
                            mainInfo.PageSize = headerResultOutput.Count() * 2;
                            mainInfo.NoPageReceipt = pageNoreceipt++;

                            var tableInfo = receiptOutputReport.DetailedResultOutput.Where(x => x.SeiOutSeq == mainInfo.SeiOutSeq && x.SeiRen == mainInfo.SeiRen).ToList();
                            foreach (var item in tableInfo)
                            {
                                item.HasYmd = !string.IsNullOrEmpty(item.HasYmd.Trim()) ? DateTime.ParseExact(item.HasYmd, Formats.yyyyMMdd, null).ToString(Formats.SlashyyyyMMdd) : "";
                                item.MeisaiUchiwake = item.SeiFutSyu == 1 ? (item.YoyaNm + System.Environment.NewLine + (!string.IsNullOrEmpty(item.HaiSYmd.Trim()) ? StringExtensions.AddSlash2YYYYMMDD(item.HaiSYmd) : "") + "～" +
                                                      (!string.IsNullOrEmpty(item.TouYmd?.Trim()) ? StringExtensions.AddSlash2YYYYMMDD(item.TouYmd) : "") + (item.Zeiritsu == Zeiritsu ? "　　　　　　　　　　　　　　　　"
                                                      + ReducedTaxClass : string.Empty)) : item.FutTumNm;
                                item.SyaSyuNmDisplay = item.SyaSyuNm + System.Environment.NewLine + item.Suryo;
                            }
                            var newReceiptOutputReport = new ReceiptOutputReport()
                            {
                                HeaderResultOutput = mainInfo,
                                DetailedResultOutput = tableInfo,
                                RyoOutSeq = int.Parse(ryoOutSeq.ToString())
                            };
                            receiptOutputReporttList.Add(newReceiptOutputReport);
                        });
                        return receiptOutputReporttList;
                    }
                    return new List<ReceiptOutputReport>();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
