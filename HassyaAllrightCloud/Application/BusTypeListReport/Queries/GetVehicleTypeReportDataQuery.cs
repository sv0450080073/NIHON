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
    public class GetVehicleTypeReportDataQuery : IRequest<List<VehicleTypeReport>>
    {
        private readonly string _codeSyu;
        private readonly int _tenantId;
        public GetVehicleTypeReportDataQuery(string codeSyu, int tenantId)
        {
            _codeSyu = codeSyu;
            _tenantId = tenantId;
        }
        public class Handler : IRequestHandler<GetVehicleTypeReportDataQuery, List<VehicleTypeReport>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetVehicleTypeReportDataQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ILogger<GetVehicleTypeReportDataQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            }

            public async Task<List<VehicleTypeReport>> Handle(GetVehicleTypeReportDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<VehicleTypeReport>();
                try
                {
                    int tenantIdByCodeSyu = await _codeSyuService.CheckTenantByKanriKbnAsync(request._tenantId, request._codeSyu);
                    var CntSyaSyu = (from TPM_HenSya in _context.VpmHenSya
                                     join SyaRyo in _context.VpmSyaRyo
                                     on TPM_HenSya.SyaRyoCdSeq equals SyaRyo.SyaRyoCdSeq
                                     into SyaRyo_join
                                     from SyaRyo in SyaRyo_join.DefaultIfEmpty()
                                     join SyaSyu in _context.VpmSyaSyu
                                     on new { E1 = SyaRyo.SyaSyuCdSeq, E2 = 1 }
                                     equals new { E1 = SyaSyu.SyaSyuCdSeq, E2 = (int)SyaSyu.SiyoKbn }
                                     into SyaSyu_join
                                     from SyaSyu in SyaSyu_join.DefaultIfEmpty()
                                     join Eigyos in _context.VpmEigyos
                                     on new { E1 = TPM_HenSya.EigyoCdSeq, E2 = 1 }
                                     equals new { E1 = Eigyos.EigyoCdSeq, E2 = (int)Eigyos.SiyoKbn }
                                     into Eigyos_join
                                     from Eigyos in Eigyos_join.DefaultIfEmpty()
                                     join Compny in _context.VpmCompny
                                     on new { E1 = Eigyos.CompanyCdSeq, E2 = 1 }
                                     equals new { E1 = Compny.CompanyCdSeq, E2 = (int)Compny.SiyoKbn }
                                     into Compny_join
                                     from Compny in Compny_join.DefaultIfEmpty()
                                     where TPM_HenSya.StaYmd.CompareTo("") <= 0
                                     && TPM_HenSya.EndYmd.CompareTo("") >= 0
                                     && Compny.CompanyCdSeq == 1
                                     select new TbTMP()
                                     {
                                         SyaSyu_SyaSyuCdSeq = SyaSyu.SyaSyuCdSeq
                                         //Count_SyaSyuCdSeq = Count
                                     }).ToList();
                    var uniqueSyaSyu = (from ac in CntSyaSyu
                                        group ac by ac.Count_SyaSyuCdSeq into av
                                        select new TbTMP()
                                        {
                                            SyaSyu_SyaSyuCdSeq = av.Key,
                                            Count_SyaSyuCdSeq = av.Count()
                                            
                                        }).ToList();

                    var  data = (from SyaSyu1 in _context.VpmSyaSyu
                              join Tb_tpm in uniqueSyaSyu
                              on SyaSyu1.SyaSyuCdSeq equals Tb_tpm.SyaSyu_SyaSyuCdSeq
                              into Tb_tpm_join
                              from Tb_tpm in Tb_tpm_join.DefaultIfEmpty()
                              join CodeKb01 in _context.VpmCodeKb
                              on new { E1 = SyaSyu1.KataKbn.ToString(), E3 = "KATAKBN", E4 = 1, E5 = tenantIdByCodeSyu }
                              equals new { E1 = CodeKb01.CodeKbn, E3 = CodeKb01.CodeSyu, E4 = (int)CodeKb01.SiyoKbn, E5 = CodeKb01.TenantCdSeq }
                              into CodeKb01_join
                              from CodeKb01 in CodeKb01_join.DefaultIfEmpty()
                              join SyaRyo1 in _context.VpmSyaRyo
                              on SyaSyu1.SyaSyuCdSeq equals SyaRyo1.SyaSyuCdSeq
                              into SyaRyo1_join
                              from SyaRyo1 in SyaRyo1_join.DefaultIfEmpty()
                              join HenSya1 in _context.VpmHenSya
                              on SyaRyo1.SyaRyoCdSeq equals HenSya1.SyaRyoCdSeq
                              into HenSya1_join
                              from HenSya1 in HenSya1_join.DefaultIfEmpty()
                              join Eigo1 in _context.VpmEigyos
                              on new { E1 = HenSya1.EigyoCdSeq, E2 = 1 }
                              equals new { E1 = Eigo1.EigyoCdSeq, E2 = (int)Eigo1.SiyoKbn }
                              into Eigo1_join
                              from Eigo1 in Eigo1_join.DefaultIfEmpty()
                              join Compny1 in _context.VpmCompny
                               on new { E1 = Eigo1.CompanyCdSeq, E2 = 1 }
                              equals new { E1 = Compny1.CompanyCdSeq, E2 = (int)Compny1.SiyoKbn }
                              where SyaSyu1.SiyoKbn == 1
                               && HenSya1.StaYmd.CompareTo("") <= 0
                               && HenSya1.EndYmd.CompareTo("") >= 0
                              // && SyaSyu1.SyaSyuCd == //todo in 
                              && SyaSyu1.TenantCdSeq == 1
                              && SyaSyu1.KataKbn == 1
                              && Compny1.CompanyCdSeq == 1
                              select new VehicleTypeReport()
                              {
                                  SyaSyu1_SyaSyuCdSeq = SyaSyu1.SyaSyuCdSeq,
                                  SyaSyu1_SyaSyuNm = SyaSyu1.SyaSyuNm,
                                  CntSyaSyu_CNT= Tb_tpm.Count_SyaSyuCdSeq
                              }).ToList();
                    result = (from kq in data
                             group kq by new { kq.SyaSyu1_SyaSyuCdSeq, kq.SyaSyu1_SyaSyuNm, kq.CntSyaSyu_CNT } into kv
                             select new  VehicleTypeReport()
                             {
                                 SyaSyu1_SyaSyuCdSeq = kv.Key.SyaSyu1_SyaSyuCdSeq,
                                 SyaSyu1_SyaSyuNm = kv.Key.SyaSyu1_SyaSyuNm,
                                 CntSyaSyu_CNT = kv.Key.CntSyaSyu_CNT
                             }).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
                return result;
            }
        }
    }
}
