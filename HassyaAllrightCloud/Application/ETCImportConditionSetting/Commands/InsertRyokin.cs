using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETCImportConditionSetting.Commands
{
    public class InsertRyokin : IRequest<bool>
    {
        public VpmRyokin Model { get; set; }
        public class Handler : IRequestHandler<InsertRyokin, bool>
        {
            KobodbContext _context;
            public Handler(KobodbContext context) => _context = context;
            public async Task<bool> Handle(InsertRyokin request, CancellationToken cancellationToken)
            {
                var sql = $@"
                    INSERT INTO [dbo].[VPM_Ryokin]
                               ([RyokinTikuCd]
                               ,[RyokinCd]
                               ,[RoadCorporationKbn]
                               ,[RoadCorporationName]
                               ,[DouroName]
                               ,[RyokinNm]
                               ,[RyakuNm]
                               ,[SiyoStaYmd]
                               ,[SiyoEndYmd]
                               ,[UpdYmd]
                               ,[UpdTime]
                               ,[UpdSyainCd]
                               ,[UpdPrgID])
                         VALUES
                                ('{request.Model.RyokinTikuCd}'
                                ,'{request.Model.RyokinCd}'
                                ,'{request.Model.RoadCorporationKbn}'
                                ,'{request.Model.RoadCorporationName}'
                                ,'{request.Model.DouroName}'
                                ,'{request.Model.RyokinNm}'
                                ,'{request.Model.RyakuNm}'
                                ,'{request.Model.SiyoStaYmd}'
                                ,'{request.Model.SiyoEndYmd}'
                                ,'{request.Model.UpdYmd}'
                                ,'{request.Model.UpdTime}'
                                ,'{request.Model.UpdSyainCd}'
                                ,'{request.Model.UpdPrgId}')";
                var effectedRows = await _context.Database.ExecuteSqlRawAsync(sql);
                return effectedRows > 0;
            }
        }
    }
}
