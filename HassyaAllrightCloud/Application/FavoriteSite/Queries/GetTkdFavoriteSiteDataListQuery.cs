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
    public class GetTkdFavoriteSiteDataListQuery : IRequest<List<TKD_FavoriteSiteData>>
    {
        private readonly KobodbContext _kobodbContext;

        public class Handler : IRequestHandler<GetTkdFavoriteSiteDataListQuery, List<TKD_FavoriteSiteData>>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext context)
            {
                _kobodbContext = context;
            }

            /// <summary>
            /// Query to get all of Favorite Site
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<TKD_FavoriteSiteData>> Handle(GetTkdFavoriteSiteDataListQuery request, CancellationToken cancellationToken)
            {
                var query = (from record in _kobodbContext.TkdFavoriteSite.Where(e => e.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq)
                             orderby record.DisplayOrder
                             select new TKD_FavoriteSiteData()
                             {
                                 FavoriteSite_FavoriteSiteCdSeq = record.FavoriteSiteCdSeq,
                                 FavoriteSite_SiteTitle = record.SiteTitle,
                                 FavoriteSite_SiteUrl = record.SiteUrl,
                                 FavoriteSite_DisplayOrder = record.DisplayOrder
                             });
                return query.ToList();
            }
        }

    }
}
