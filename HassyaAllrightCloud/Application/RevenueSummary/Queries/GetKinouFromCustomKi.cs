using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.RevenueSummary.Queries
{
    public class GetKinouFromCustomKi : IRequest<CustomKiDto>
    {
        public CustomKiSearchModel SearchModel { get; set; }
        public class Handler : IRequestHandler<GetKinouFromCustomKi, CustomKiDto>
        {
            private KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }
            public async Task<CustomKiDto> Handle(GetKinouFromCustomKi request, CancellationToken cancellationToken)
            {
                return await _kobodbContext.VpdCustomKi.Where(i => i.SyainCdSeq == request.SearchModel.SyainCdSeq
                && i.KinouId == request.SearchModel.KinouId).Select(i => new CustomKiDto()
                {
                    Kinou01 = i.Kinou01,
                    Kinou02 = i.Kinou02
                }).FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}