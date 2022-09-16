using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETCImportConditionSetting.Queries
{
    public class IsETCImportExist : IRequest<bool>
    {
        public ETCImportSearchModel Model { get; set; }
        public class Handler : IRequestHandler<IsETCImportExist, bool>
        {
            KobodbContext _context;
            public Handler(KobodbContext context) => _context = context;
            public async Task<bool> Handle(IsETCImportExist request, CancellationToken cancellationToken)
            {
                try
                {
                    await _context.LoadStoredProc("[dbo].[PK_dETCImportPreCheck_R]")
                              .AddParam("@TenantCdSeq", request?.Model?.TenantCdSeq)
                              .AddParam("@CardNo", request?.Model?.CardNo ?? "")
                              .AddParam("@UnkYmd", request?.Model?.UnkYmd ?? "")
                              .AddParam("@UnkTime", request?.Model?.UnkTime ?? "")
                              .AddParam("@SyaRyoCd", request?.Model?.SyaRyoCd.ToString() ?? "")
                              .AddParam("@ROWCOUNT", out IOutParam<int> retParam)
                          .ExecNonQueryAsync(cancellationToken);
                    return retParam.Value != 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
