using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.TransportActualResult.Queries;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Pages;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITransportActualResultService : IReportService
    {
        Task<IEnumerable<CodeKbDataItem>> GetCodeKb(int tenantId);
        Task<List<TransportActualResultReportData>> GetReportData(ReportSearchModel model, CancellationToken token);
    }

    public class TransportActualResultService : ITransportActualResultService
    {
        private IMediator _mediator;
        private IStringLocalizer<TransportActualResult> _lang;
        private IReportLoadingService _reportLoadingService;
        public TransportActualResultService(
            IMediator mediator,
            IStringLocalizer<TransportActualResult> lang,
            IReportLoadingService reportLoadingService)
        {
            _mediator = mediator;
            _lang = lang;
            _reportLoadingService = reportLoadingService;
        }
        public async Task<IEnumerable<CodeKbDataItem>> GetCodeKb(int tenantId)
        {
            return await _mediator.Send(new GetCodeKb() { TenantId = tenantId,  CodeSyu = "UNSOUKBN" });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<ReportSearchModel>(queryParams);
            XtraReport report = new Reports.ReportTemplate.TransportActualResult.TransportActualResult();
            _reportLoadingService.Start(searchParams.ReportId);
            var data = await GetReportData(searchParams, default);
            _reportLoadingService.Stop(searchParams.ReportId);
            report.DataSource = InitObjectDataSource(data.ToList(), typeof(TransportActualResultDataSource), "objectDataSource1");

            return report;
        }

        private ObjectDataSource InitObjectDataSource<T>(T data, Type dataSourceType, string dataSourceName)
        {
            Parameter param = new Parameter()
            {
                Name = "dataSource",
                Type = typeof(T),
                Value = data
            };
            ObjectDataSource dataSource = new ObjectDataSource();
            dataSource.Name = dataSourceName;
            dataSource.DataSource = dataSourceType;
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "DataSource";
            return dataSource;
        }

        public async Task<List<TransportActualResultReportData>> GetReportData(ReportSearchModel model, CancellationToken token)
        {
            var jigyoCarSumCnt = await _mediator.Send(new GetHenSya()
            {
                Model = new HenSyaSearchModel()
                {
                    EigyoFrom = model.EigyoFrom,
                    EigyoTo = model.EigyoTo,
                    ProcessingYear = $"{model.ProcessingYear}0331"
                }
            });


            var spModels = await _mediator.Send(new GetTransportActualResult() { Model = model }, token);
            return spModels.Select(m => new TransportActualResultReportData()
            {
                BusinessYmd = (model.ProcessingYear + 1).ToString(),
                ProcessingYear = model.ProcessingYear.ToString(),
                UnsouKbnNm = m.UnsouKbn == 1 ? _lang["UnsouKbn_Private"] :
                                m.UnsouKbn == 2 ? _lang["UnsouKbn_Specific"] :
                                m.UnsouKbn == 3 ? _lang["UnsouKbn_Special"] :
                                _lang["UnsouKbn_Other"],
                JigyoCarSumCnt = jigyoCarSumCnt.AddCommas(),
                NobeSumCnt = m.TotalNobeSumCnt.AddCommas(),
                JitJisaKm = $"{m.TotalJitJisaKm:#,##0}",
                JitSumKm = $"{m.TotalJitSumKm:#,##0}",
                NobeJitCnt = m.TotalNobeJitCnt.AddCommas(),
                UnkoCnt = m.TotalUnkoCnt.AddCommas(),
                UnkoOthAllCnt = m.TotalUnkoOthAllCnt.AddCommas(),
                UnsoSyu = $"{m.TotalUnsoSyu:#,##0}",
                YusoJin = m.TotalYusoJin.AddCommas()
            }).ToList();
        }
    }
}
