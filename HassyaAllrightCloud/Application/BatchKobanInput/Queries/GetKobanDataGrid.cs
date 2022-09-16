using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Threading;
using StoredProcedureEFCore;

namespace HassyaAllrightCloud.Application.BatchKobanInput.Queries
{
    public class GetKobanDataGrid : IRequest<List<KobanDataGridModel>>
    {
        public BatchKobanInputFilterModel Model { get; set; }
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetKobanDataGrid, List<KobanDataGridModel>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<KobanDataGridModel>> Handle(GetKobanDataGrid request, CancellationToken cancellationToken = default)
            {
                var currentCompany = _dbcontext.VpmCompny.Where(x => x.CompanyCdSeq == new ClaimModel().CompanyID).FirstOrDefault();
                List<KobanDataGridModel> dataGridModels = new List<KobanDataGridModel>();
                _dbcontext.LoadStoredProc("PK_dKobanData_R")
                         .AddParam("@TargetYmd", request.Model.KinmuYmd.ToString("yyyyMMdd"))
                         .AddParam("@CompanyCd", request.Model.Company != null ? request.Model.Company.CompanyCd : currentCompany != null ? currentCompany.CompanyCd : 1)
                         .AddParam("@EigyoCdStr", request.Model.EigyoStart != null ? request.Model.EigyoStart.EigyoCd : 0)
                         .AddParam("@EigyoCdEnd", request.Model.EigyoEnd != null ? request.Model.EigyoEnd.EigyoCd : 0)
                         .AddParam("@SyainCdStr", request.Model.SyainStart != null ? request.Model.SyainStart.SyainCd : string.Empty)
                         .AddParam("@SyainCdEnd", request.Model.SyainEnd != null ? request.Model.SyainEnd.SyainCd : string.Empty)
                         .AddParam("@SyokumuCdStr", request.Model.SyokumuStart != null ? request.Model.SyokumuStart.SyokumuCd : 0)
                         .AddParam("@SyokumuCdEnd", request.Model.SyokumuEnd != null ? request.Model.SyokumuEnd.SyokumuCd : 0)
                         .AddParam("@SyuJun", request.Model.SyuJun.StringValue)
                         .AddParam("@TenantCdSeq", request.TenantCdSeq)
                         .AddParam("@ROWCOUNT", out IOutParam<int> rowCount)

                         .Exec(r => dataGridModels = r.ToList<KobanDataGridModel>());

                return dataGridModels;
            }
        }
    }
}
