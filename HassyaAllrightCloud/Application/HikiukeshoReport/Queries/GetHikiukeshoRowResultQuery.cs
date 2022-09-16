using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HikiukeshoReport
{
    public class GetHikiukeshoRowResultQuery : IRequest<int>
    {
        public TransportationContractFormData TransportationContract { get; set; }
        public class Handler : IRequestHandler<GetHikiukeshoRowResultQuery, int>
        {
            private readonly KobodbContext context;
            private readonly HikiukeshoHelper hikiukeshoHelper;

            public Handler(KobodbContext _context, HikiukeshoHelper _hikiukeshoHelper)
            {
                this.context = _context;
                this.hikiukeshoHelper = _hikiukeshoHelper;
            }

            public async Task<int> Handle(GetHikiukeshoRowResultQuery request, CancellationToken cancellationToken)
            {
                if (request.TransportationContract == null) return 0;
                var storedBuilder = hikiukeshoHelper.CreateStoredBuilder("dbo.PK_HkUkYyk_All_R", context, request.TransportationContract);

                storedBuilder.AddParam("@ROWCOUNT", out IOutParam<int> retParam)
                .ExecNonQuery();

                var rowcount = retParam.Value;
                return rowcount;
            }
        }
    }
}
