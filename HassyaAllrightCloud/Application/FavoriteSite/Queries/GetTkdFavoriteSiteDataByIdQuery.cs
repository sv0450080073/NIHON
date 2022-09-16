using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FavoriteSite.Queries
{
    public class GetTkdFavoriteSiteDataByIdQuery : IRequest<TKD_FavoriteSiteData>
    {
        public int ID { get; set; }

        public class Handler : IRequestHandler<GetTkdFavoriteSiteDataByIdQuery, TKD_FavoriteSiteData>
        {
            private readonly KobodbContext _kobodbContext;

            public Handler(KobodbContext context)
            {
                _kobodbContext = context;
            }

            public async Task<TKD_FavoriteSiteData> Handle(GetTkdFavoriteSiteDataByIdQuery request, CancellationToken cancellationToken)
            {
                var query = (from record in _kobodbContext.TkdFavoriteSite.Where(e => e.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq & e.FavoriteSiteCdSeq == request.ID)
                             select new TKD_FavoriteSiteData()
                             {
                                 FavoriteSite_FavoriteSiteCdSeq = record.FavoriteSiteCdSeq,
                                 FavoriteSite_SiteTitle = record.SiteTitle,
                                 FavoriteSite_SiteUrl = record.SiteUrl,
                                 FavoriteSite_DisplayOrder = record.DisplayOrder
                             });
                return query.FirstOrDefault();
            }
        }
    }
}
