using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.TransportationSummary.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IMonthlyTransportationAnnualService : IReportService
    {
        Task<List<JitHouReports>> GetJitHouAnnualDataReport(SearchParam jitHouParam);
    }
    public class MonthlyTransportationAnnualService : IMonthlyTransportationAnnualService
    {
        private readonly IMediator _mediator;
        private readonly KobodbContext _context;

        public MonthlyTransportationAnnualService(IMediator mediator, KobodbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<List<JitHouReports>> GetJitHouAnnualDataReport(SearchParam searchParam)
        {
            return await _mediator.Send(new GetJitHousAnnual { SearchParam = searchParam });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<SearchParam>(queryParams);

            XtraReport reportJitHou = new Reports.JitHouAnnualReport();
            ObjectDataSource dataSourceJitHou = new ObjectDataSource();
            var dataJitHou = await GetJitHouAnnualDataReport(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<JitHouReports>),
                Value = dataJitHou
            };
            dataSourceJitHou.Name = "objectDataSource1";
            dataSourceJitHou.DataSource = typeof(JitHouReportReportDS);
            dataSourceJitHou.Constructor = new ObjectConstructorInfo(param);
            dataSourceJitHou.DataMember = "_data";
            reportJitHou.DataSource = dataSourceJitHou;
            return reportJitHou;
        }
    }
}
