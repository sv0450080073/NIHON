using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetOutDataTableAsyncQuery : IRequest<List<OutDataTableOutput>>
    {
        public string ukeNo { get; set; }
        public class Handler : IRequestHandler<GetOutDataTableAsyncQuery, List<OutDataTableOutput>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<OutDataTableOutput>> Handle(GetOutDataTableAsyncQuery request, CancellationToken cancellationToken = default)
            {
                    List<OutDataTableOutput> rows = null;
                    var claimModel = new ClaimModel();
                    _context.LoadStoredProc("PK_dOutDataTable_R")
                                    .AddParam("@EigyoCd", claimModel.EigyoCdSeq)
                                    .AddParam("@CompanyCd", claimModel.CompanyID)
                                    .AddParam("@UkeNo", request.ukeNo)
                                    .AddParam("@TenantCdSeq", claimModel.TenantID)
                                    .AddParam("@ROWCOUNT", out IOutParam<int> rowCount)
                                    .Exec(r => rows = r.ToList<OutDataTableOutput>());
                    return rows;               
            }
        }
    }
}
