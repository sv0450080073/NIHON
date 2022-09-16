using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.Staff.Queries;
using HassyaAllrightCloud.Application.SubContractorStatusReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.SubContractorStatus;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    interface ISubContractorStatusReportService : IReportService
    {
        Task<SubContractorStatusSearchPaged> SearchSubContractorStatusSearchPaged(SubContractorStatusData data, int tenantId, int page, int itemPerPage);
        Task<List<SubContractorStatusExportCsvData>> GetSubContractorStatusCsvData(SubContractorStatusReportSearchParams prs);
        Task<List<SubContractorStatusReportData>> GetSubContractorStatusReportData(SubContractorStatusReportSearchParams prs);
        Task<List<SubContractorStatusReportPagedData>> GetSubContractorStatusReportPagedData(SubContractorStatusReportSearchParams prs);
    }

    public class SubContractorStatusReportService : ISubContractorStatusReportService
    {
        private readonly IMediator _mediatR;

        public SubContractorStatusReportService(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            XtraReport report = new XtraReport();
            ObjectDataSource dataSource = new ObjectDataSource();
            var prviewParam = EncryptHelper.DecryptFromUrl<SubContractorStatusReportSearchParams>(queryParams);

            switch (prviewParam.SearchCondition.PaperSize)
            {
                case PaperSize.A3:
                    report = new Reports.ReportTemplate.SubContractor.SubContractorA3();
                    break;
                case PaperSize.A4:
                    report = new Reports.ReportTemplate.SubContractor.SubContractorA4();
                    break;
                case PaperSize.B4:
                    report = new Reports.ReportTemplate.SubContractor.SubContractorB4();
                    break;
                default:
                    break;
            }

            var data = await GetSubContractorStatusReportPagedData(prviewParam);

            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<SubContractorStatusReportPagedData>),
                Value = data
            };
            dataSource.Name = "objectDataSource2";
            dataSource.DataSource = typeof(SubContractorStatusReportDataSource);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        public async Task<List<SubContractorStatusReportData>> GetSubContractorStatusReportData(SubContractorStatusReportSearchParams prs)
        {
            return await _mediatR.Send(new GetSubContractorStatusReportQuery(prs.SearchCondition, prs.TenantId));
        }

        public Task<List<SubContractorStatusExportCsvData>> GetSubContractorStatusCsvData(SubContractorStatusReportSearchParams prs)
        {
            return _mediatR.Send(new GetSubContractorStatusCsvDataQuery(prs.SearchCondition, prs.TenantId));
        }

        public async Task<List<SubContractorStatusReportPagedData>> GetSubContractorStatusReportPagedData(SubContractorStatusReportSearchParams prs)
        {
            var currentStaffLogin = await _mediatR.Send(new GetStaffByStaffIdQuery(prs.UserLoginId, prs.TenantId));
            var reportData = await _mediatR.Send(new GetSubContractorStatusReportQuery(prs.SearchCondition, prs.TenantId));

            if (!reportData?.Any() ?? true)
                return new List<SubContractorStatusReportPagedData>();
            return PaggingReportData(prs, reportData, (currentStaffLogin).FirstOrDefault(), 13);
        }

        public Task<SubContractorStatusSearchPaged> SearchSubContractorStatusSearchPaged(SubContractorStatusData data, int tenantId, int page, int itemPerPage)
        {
            return _mediatR.Send(new SearchSubContractorStatusQuery(data, tenantId, page, itemPerPage));
        }


        #region Helper
        private List<SubContractorStatusReportPagedData> PaggingReportData(SubContractorStatusReportSearchParams prs, List<SubContractorStatusReportData> reportDatas, LoadStaff currentStaff, int itemPerPage = 13)
        {
            var grp = GroupReportData(prs.SearchCondition, reportDatas, currentStaff);
            var groupDisplay = MoneyCalculation(prs.SearchCondition, reportDatas, grp);

            var paged = new List<SubContractorStatusReportPagedData>();

            foreach (var item in groupDisplay)
            {
                var result = HandleRepeatItem(item.ReportDatas);

                for (int pageIndex = 1; ; pageIndex++)
                {
                    var pagedItem = PagedList<SubContractorStatusReportData>.ToPagedList(result.AsQueryable(), pageIndex, itemPerPage);

                    int currentPageCount = pagedItem.Count;
                    if (currentPageCount == itemPerPage)
                    {
                        var newPage = new SubContractorStatusReportPagedData();
                        newPage.SimpleCloneProperties(item);
                        newPage.ReportDatas = pagedItem;

                        paged.Add(newPage);
                    }
                    else
                    {
                        if (currentPageCount == 0)
                        {
                            break;
                        }
                        else
                        {
                            var newPage = new SubContractorStatusReportPagedData();
                            newPage.SimpleCloneProperties(item);
                            newPage.ReportDatas = pagedItem;

                            paged.Add(newPage);
                            break;
                        }
                    }
                }
            }

            return paged;
        }

        private List<SubContractorStatusReportData> HandleRepeatItem(List<SubContractorStatusReportData> reportDatas)
        {
            for(int start = 0; start < reportDatas.Count - 1;)
            {
                if(reportDatas[start].IsRowSumResult)
                {
                    start++;
                    continue;
                }

                for(int repeat = start + 1; repeat < reportDatas.Count; repeat++)
                {
                    var startItem = reportDatas[repeat];
                    var repeatItem = reportDatas[repeat];

                    if (startItem.UkeNo == repeatItem.UkeNo && 
                        startItem.UnkRen == repeatItem.UnkRen && 
                        !repeatItem.IsRowSumResult)
                        reportDatas[repeat].IsRepeatItem = true;
                    else
                    {
                        start = repeat;
                        break;
                    }
                }
            }

            return reportDatas;
        }

        private List<SubContractorStatusReportPagedData> GroupReportData(SubContractorStatusData searchCondition, List<SubContractorStatusReportData> reportDatas, LoadStaff currentStaff)
        {
            List<SubContractorStatusReportPagedData> groupDisplay = new List<SubContractorStatusReportPagedData>();

            if(searchCondition.PageBreak.Option == Commons.Constants.PageBreak.Yes && !(searchCondition.Group.Option == GroupDivision.All))
            {
                return reportDatas.GroupBy(_ => _.YouTokuiSeq)
                        .Select(_ => new SubContractorStatusReportPagedData
                        {
                            IsNormalPagging = false,
                            DateType = searchCondition.DateTypeText,
                            OutputInfo = searchCondition.OwnCompanyType.DisplayName,
                            DateInfo = $"{searchCondition.StartDate.ToString("yyyy/MM/dd")} ～ {searchCondition.EndDate.ToString("yyyy/MM/dd")}",
                            CustomerInfo = $"{_.FirstOrDefault().SkTokuiCd:D4} {_.FirstOrDefault().YouSkRyakuNm}",
                            UkeCdInfo = $"{searchCondition.UkeCdFrom} ～ {searchCondition.UkeCdTo}",
                            BranchInfo = $"{searchCondition.BranchStart?.BranchText} ～ {searchCondition.BranchEnd?.BranchText}",
                            StaffInfo = GetStaffInfo(searchCondition.StaffStart, searchCondition.StaffEnd),
                            PrintedStaffCD = int.Parse(currentStaff.SyainCd).ToString("D10"),
                            PrintedStaffName = currentStaff.SyainNm,
                            ReportDatas = _.ToList(),
                        }).ToList();
            }
            else
            {
                return new List<SubContractorStatusReportPagedData>
                {
                    new SubContractorStatusReportPagedData
                    {
                        IsNormalPagging = true,
                        DateType = searchCondition.DateTypeText,
                        OutputInfo = searchCondition.OwnCompanyType.DisplayName,
                        DateInfo = $"{searchCondition.StartDate.ToString("yyyy/MM/dd")} ～ {searchCondition.EndDate.ToString("yyyy/MM/dd")}",
                        CustomerInfo = GetCustomerInfo(searchCondition.CustomerStart, searchCondition.CustomerEnd),
                        UkeCdInfo = $"{searchCondition.UkeCdFrom} ～ {searchCondition.UkeCdTo}",
                        BranchInfo = $"{searchCondition.BranchStart?.BranchText} ～ {searchCondition.BranchEnd?.BranchText}",
                        StaffInfo = GetStaffInfo(searchCondition.StaffStart, searchCondition.StaffEnd),
                        PrintedStaffCD = int.Parse(currentStaff.SyainCd).ToString("D10"),
                        PrintedStaffName = currentStaff.SyainNm,
                        ReportDatas = HandeleReportGroupWithoutPagging(reportDatas, searchCondition.Group.Option),
                    }
                };
            }

            string GetStaffInfo(LoadStaff from, LoadStaff to)
            {
                string fromText = $"";
                string toText = $"";

                if (from != null)
                    fromText = $"{SplitStaffInfo(from.SyainCd, from.SyainNm)}";
                if (to != null)
                    toText = $"{SplitStaffInfo(to.SyainCd, to.SyainNm)}";

                return $"{fromText}～{toText}";
            }
            string SplitStaffInfo(string staffCd, string staffName)
            {
                if (!string.IsNullOrEmpty(staffName) && staffName.Length > 7)
                    staffName = staffName.Substring(0, 7);

                return $"{long.Parse(staffCd):D10} : {staffName}";
            }
            string GetCustomerInfo(LoadCustomerList from, LoadCustomerList to)
            {
                string fromText = $"";
                string toText = $"";

                if(from != null)
                    fromText = $"{from?.TokuiCd:D4}{from.SitenCd:D4} {from.RyakuNm} {from.SitenRyakuNm}";
                if(to != null)
                    toText = $"{to.TokuiCd:D4}{to.SitenCd:D4} {to.RyakuNm} {to.SitenRyakuNm}";

                return $"{fromText}～{toText}";
            }

            List<SubContractorStatusReportData> HandeleReportGroupWithoutPagging(List<SubContractorStatusReportData> reportDatas, GroupDivision group)
            {
                return group switch
                {
                    GroupDivision.All => reportDatas,
                    GroupDivision.ParentCompanies => reportDatas.GroupBy(_=>_.YouTokuiSeq).SelectMany(_=>_.ToList()).ToList(),
                    GroupDivision.SubsidiaryCompanies => reportDatas.GroupBy(_ => new { _.YouTokuiSeq, _.YouSitenCdSeq }).SelectMany(_ => _.ToList()).ToList(),
                    _ => throw new InvalidOperationException(),
                };
            }
        }

        private List<SubContractorStatusReportPagedData> MoneyCalculation(SubContractorStatusData searchCondition, List<SubContractorStatusReportData> reportDatas, List<SubContractorStatusReportPagedData> groupedData)
        {
            for(int i = 0; i < groupedData.Count; i++)
            {
                groupedData[i].ReportDatas = Calculation(groupedData[i].ReportDatas, searchCondition.Group.Option);
            }

            var totalMoneyRow = FillSumRow(reportDatas, "≪合　　　計≫");
            groupedData?.LastOrDefault()?.ReportDatas.Insert((groupedData.LastOrDefault()?.ReportDatas?.Count  ?? 0), totalMoneyRow);

            return groupedData;
        }

        private List<SubContractorStatusReportData> Calculation(List<SubContractorStatusReportData> src, GroupDivision group)
        {
            switch (group)
            {
                case GroupDivision.All: // sum in the last page
                    
                        break;
                case GroupDivision.ParentCompanies: // sum of tokui
                    return 
                    src.GroupBy(_ => _.YouTokuiSeq).SelectMany(_ => {
                        var currentList = _.ToList();
                        var sumRow = FillSumRow(currentList, "<<業　者　計>>");
                        currentList.Insert(currentList.Count, sumRow);

                        return currentList;
                    }).ToList();
                    
                    //break;
                case GroupDivision.SubsidiaryCompanies: // sum siten & tokui
                    return
                    src.GroupBy(t => t.YouTokuiSeq).SelectMany(tokui =>
                    {
                        var currentTokuiList = tokui.ToList();
                        currentTokuiList = currentTokuiList.GroupBy(s => s.YouSitenCdSeq).SelectMany(siten =>
                        {
                            var currentSitenList = siten.ToList();
                            var tokuiSitenSum = FillSumRow(currentSitenList, "<<支　店　計>>");
                            currentSitenList.Insert(currentSitenList.Count, tokuiSitenSum);

                            return currentSitenList;
                        }).ToList();

                        var sumTokuiRow = FillSumRow(currentTokuiList, "<<業　者　計>>");
                        currentTokuiList.Insert(currentTokuiList.Count, sumTokuiRow);
                        return currentTokuiList;
                    }).ToList();

                    ////sum siten
                    //var gr2 = src.GroupBy(_ => new { _.YouTokuiSeq, _.YouSitenCdSeq }).Select(_ => new { _.Key.YouTokuiSeq,  _.Key.YouSitenCdSeq, TokuiSitenList = _.ToList() }).ToList();
                    //foreach (var item in gr2)
                    //{
                    //    var tokuiSitenSum = FillSumRow(item.TokuiSitenList, "<<支　店　計>>");
                    //    var lastItem = item.TokuiSitenList[item.TokuiSitenList.Count - 1];
                    //    int index = src.FindLastIndex(_ => _.UkeNo == lastItem.UkeNo && _.UnkRen == lastItem.UnkRen && _.YouTokuiSeq == lastItem.YouTokuiSeq && _.YouSitenCdSeq == lastItem.YouSitenCdSeq);
                    //    src.Insert(index + 1, tokuiSitenSum);
                    //}

                    ////sum tokui
                    //var sumTokuiRow = FillSumRow(src, "<<業　者　計>>");
                    //src.Insert(src.Count, sumTokuiRow);
                    //break;
                default:
                    break;
            }
            return src;
        }

        private SubContractorStatusReportData FillSumRow(List<SubContractorStatusReportData> src, string textForTotal = "")
        {
            var srcLoanSum = src.Where(_ => !_.IsRowSumResult);
            var distinct = srcLoanSum.DistinctBy(_ => new { _.UkeNo, _.UnkRen });
            var distinctFutai = srcLoanSum.DistinctBy(_ => new { _.UkeNo, _.UnkRen, _.YouTokuiSeq, _.YouSitenCdSeq });
            var last = srcLoanSum.LastOrDefault();
            var sumRow = new SubContractorStatusReportData() 
            { 
                IsRowSumResult = true,
                TotalBusLoan = srcLoanSum.Count(),
                RowTotalDisplayText = textForTotal,
                UkeNo = last.UkeNo,
                UnkRen = last.UnkRen,
                YouTokuiSeq = last.YouTokuiSeq,
                YouSitenCdSeq = last.YouSitenCdSeq,
            };
  
            foreach (var item in distinct)  // 
            {
                sumRow._totalNumber += item._totalNumber;

                sumRow._sumSyaRyoUnc += item._sumSyaRyoUnc;
                sumRow._sumZeiRui += item._sumZeiRui;
                sumRow._sumTesuRyoG += item._sumTesuRyoG;
                sumRow._sumTicket += item._sumTicket;

                sumRow._sumGuideFee += item._sumGuideFee;
                sumRow._sumGuideTax += item._sumGuideTax;
                sumRow._sumUnitGuiderFee += item._sumUnitGuiderFee;
                sumRow._totalGuider += item._totalGuider;

                sumRow._incidentalFee += item._incidentalFee;
                sumRow._incidentalTax += item._incidentalTax;
                sumRow._incidentalCharge += item._incidentalCharge;
                sumRow._totalIncidental += item._totalIncidental;
            }

            foreach (var item in srcLoanSum) 
            {
                sumRow._youshaUnc += item._youshaUnc;
                sumRow._youshaSyo += item._youshaSyo;
                sumRow._youshaTes += item._youshaTes;
                sumRow._sumTicketLoan += item._sumTicketLoan;
            }

            foreach (var item in distinctFutai)
            {
                sumRow._youFutTumGuiKin += item._youFutTumGuiKin;
                sumRow._youFutTumGuiTax += item._youFutTumGuiTax;
                sumRow._youFutTumGuiTes += item._youFutTumGuiTes;
                sumRow._totalYouFutTumGui += item._totalYouFutTumGui;

                sumRow._youFutTumKin += item._youFutTumKin;
                sumRow._youFutTumTax += item._youFutTumTax;
                sumRow._youFutTumTes += item._youFutTumTes;
                sumRow._totalYouFutTum += item._totalYouFutTum;
            }

            return sumRow;
        }
        #endregion
    }
}
