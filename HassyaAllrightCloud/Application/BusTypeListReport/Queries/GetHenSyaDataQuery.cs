using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusTypeListReport.Queries
{
    public class GetHenSyaDataQuery : IRequest<List<HenSyaDataSource>>
    {
        public BusTypeListData BusTypeListDataParam;
        public class Handler : IRequestHandler<GetHenSyaDataQuery, List<HenSyaDataSource>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetHenSyaDataQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ILogger<GetHenSyaDataQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            }
            public async Task<List<HenSyaDataSource>> Handle(GetHenSyaDataQuery request, CancellationToken cancellationToken)
            {
                List<HenSyaDataSource> result = new List<HenSyaDataSource>();
                try
                {
                    var param = request.BusTypeListDataParam;
                    result = (from HEN in _context.VpmHenSya
                              join SyaRyo in _context.VpmSyaRyo
                              on HEN.SyaRyoCdSeq equals SyaRyo.SyaRyoCdSeq
                              into SyaRyo_join
                              from SyaRyo in SyaRyo_join.DefaultIfEmpty()
                              join SyaSyu in _context.VpmSyaSyu
                              on new { E1 = SyaRyo.SyaSyuCdSeq, E2 = param.TenantCdSeq , E3=1 } 
                              equals new { E1 = SyaSyu.SyaSyuCdSeq, E2 = SyaSyu.TenantCdSeq , E3= (int)SyaSyu.SiyoKbn }
                              into SyaSyu_join
                              from SyaSyu in SyaSyu_join.DefaultIfEmpty()
                              join EI in _context.VpmEigyos
                              on HEN.EigyoCdSeq equals EI.EigyoCdSeq
                              into EI_join
                              from EI in EI_join.DefaultIfEmpty()
                              join COM in _context.VpmCompny
                              on  new {E1=  EI.CompanyCdSeq} equals new { E1= COM.CompanyCdSeq} 
                              into COM_join
                              from COM in COM_join.DefaultIfEmpty()
                              where HEN.StaYmd.CompareTo(param.StartDate.AddDays(param.numberDay).ToString("yyyyMMdd")) <= 0
                                   && HEN.EndYmd.CompareTo(param.StartDate.ToString("yyyyMMdd")) >= 0
                                   && SyaSyu.TenantCdSeq == param.TenantCdSeq
                                   && EI.EigyoCd <= (param.BranchStart.EigyoCd==0 ? EI.EigyoCd : param.BranchStart.EigyoCd)
                                   && EI.EigyoCd >= (param.BranchEnd.EigyoCd == 0 ? EI.EigyoCd : param.BranchEnd.EigyoCd)
                                   && SyaSyu.SyaSyuCd <= (param.VehicleFrom.SyaSyuCd == 0 ? SyaSyu.SyaSyuCd : param.VehicleFrom.SyaSyuCd)
                                   && SyaSyu.SyaSyuCd >= (param.VehicleTo.SyaSyuCd == 0 ? SyaSyu.SyaSyuCd : param.VehicleTo.SyaSyuCd)
                                   && COM.TenantCdSeq == param.TenantCdSeq
                              select new HenSyaDataSource()
                              {
                                  SyaRyoCdSeq = HEN.SyaRyoCdSeq,
                                  StaYmd= HEN.StaYmd,
                                  EndYmd= HEN.EndYmd,
                                  EigyoCdSeq= EI.EigyoCdSeq,
                                  CompanyCdSeq= COM.CompanyCdSeq,
                                  SyaSyuCdSeq = SyaSyu.SyaSyuCdSeq
                              }).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
