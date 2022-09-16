using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Application.AdvancePaymentDetails.Query
{
    public class GetAdvancePaymentDetailsReportPDFQuery : IRequest<List<AdvancePaymentDetailsReportPDF>>
    {
        public AdvancePaymentDetailsSearchParam SearchParams { get; set; }
        public class Handler : IRequestHandler<GetAdvancePaymentDetailsReportPDFQuery, List<AdvancePaymentDetailsReportPDF>>
        {
            private readonly IAdvancePaymentDetailsService _service;
            public Handler(IAdvancePaymentDetailsService service)
            {
                _service = service;
            }
            public async Task<List<AdvancePaymentDetailsReportPDF>> Handle(GetAdvancePaymentDetailsReportPDFQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.SearchParams;

                byte itemPerPage = 24;
                var PDFdata = new List<AdvancePaymentDetailsReportPDF>();
                List<AdvancePaymentDetailsModel> listHeader = await _service.GetListAdvancePaymentDetailsHeader(searchParam);
                List<AdvancePaymentDetailsChildModel> listChild = await _service.GetListAdvancePaymentDetailsChild(searchParam);
                List<AdvancePaymentDetailsChildModel> listChildTemp = new List<AdvancePaymentDetailsChildModel>();
                var page = 1;
                if (listHeader.Any())
                {
                    foreach (AdvancePaymentDetailsModel record in listHeader)
                    {
                        listChildTemp = listChild.Where(e => e.UkeNo == record.UkeNo).ToList();

                        if (listChildTemp.Count(_ => _.FutGuiKbn == 0) > itemPerPage || listChildTemp.Count(_ => _.FutGuiKbn != 0) > itemPerPage)
                        {
                            var count = Math.Ceiling(listChildTemp.Count * 1.0 / itemPerPage);
                            for (int i = 0; i < count; i++)
                            {
                                var listPageLeft = listChildTemp.Where(_ => _.FutGuiKbn == 0).Skip(i * itemPerPage).Take(itemPerPage).ToList();
                                var listPageRight = listChildTemp.Where(_ => _.FutGuiKbn != 0).Skip(i * itemPerPage).Take(itemPerPage).ToList();
                                if (i == count - 1)
                                {
                                    AddBlankChildData(listPageLeft, itemPerPage);
                                    AddBlankChildData(listPageRight, itemPerPage);
                                }

                                var listChildModel = AddListChild(itemPerPage, listPageLeft, listPageRight);
                                var etcGaku = listPageLeft.Sum(_ => _.Kingaku);
                                var tatekeaGaku = listPageRight.Sum(_ => _.Kingaku);

                                SetDataPerPage(PDFdata, record, listChildModel, etcGaku, tatekeaGaku, ref page);
                            }
                        }
                        else
                        {
                            var listPageLeft = listChildTemp.Where(_ => _.FutGuiKbn == 0).ToList();
                            var listPageRight = listChildTemp.Where(_ => _.FutGuiKbn != 0).ToList();
                            AddBlankChildData(listPageLeft, itemPerPage);
                            AddBlankChildData(listPageRight, itemPerPage);

                            var listChildModel = AddListChild(itemPerPage, listPageLeft, listPageRight);
                            var etcGaku = listPageLeft.Sum(_ => _.Kingaku);
                            var tatekeaGaku = listPageRight.Sum(_ => _.Kingaku);

                            SetDataPerPage(PDFdata, record, listChildModel, etcGaku, tatekeaGaku, ref page);
                        }
                    }
                }
                else
                {
                    AdvancePaymentDetailsReportPDF PDFdataTemp = new AdvancePaymentDetailsReportPDF();
                    var listItemTemp = new List<AdvancePaymentDetailsChildModel>();
                    AddBlankChildData(listItemTemp, itemPerPage);
                    var listChildModel = AddListChild(itemPerPage, listItemTemp, listItemTemp);
                    PDFdataTemp.ChildModel = listChildModel;
                    PDFdata.Add(PDFdataTemp);
                }
                PDFdata.ForEach(e =>
                {
                    e.CurrentDate = DateTime.Now.ToString(CommonConstants.FormatYMD);
                    e.TotalPage = page - 1;
                });
                return PDFdata;
            }

            private List<ChildModel> AddListChild(int itemPerPage, List<AdvancePaymentDetailsChildModel> listPageLeft, List<AdvancePaymentDetailsChildModel> listPageRight)
            {
                List<ChildModel> listChildModel = new List<ChildModel>();
                for (int j = 0; j < itemPerPage; j++)
                {
                    ChildModel data = new ChildModel();
                    data.PaymentDetailsChildListLeft = listPageLeft[j];
                    data.PaymentDetailsChildListRight = listPageRight[j];
                    listChildModel.Add(data);
                }
                return listChildModel;
            }

            public void SetDataPerPage(List<AdvancePaymentDetailsReportPDF> PDFdata, AdvancePaymentDetailsModel record, List<ChildModel> listChild, int etcGaku, int tatekeaGaku, ref int page)
            {
                AdvancePaymentDetailsReportPDF PDFdataTemp = new AdvancePaymentDetailsReportPDF();
                PDFdataTemp.PageNumber = page++;
                PDFdataTemp.Data = record;
                PDFdataTemp.Data.EtcGaku = etcGaku;
                PDFdataTemp.Data.TatekaeGaku = tatekeaGaku;
                PDFdataTemp.ChildModel = listChild;
                PDFdata.Add(PDFdataTemp);
            }

            public void AddBlankChildData(List<AdvancePaymentDetailsChildModel> listChildTemp, int itemPerPage)
            {
                while (listChildTemp.Count < itemPerPage)
                {
                    listChildTemp.Add(new AdvancePaymentDetailsChildModel());
                }
            }
        }
    }
}
