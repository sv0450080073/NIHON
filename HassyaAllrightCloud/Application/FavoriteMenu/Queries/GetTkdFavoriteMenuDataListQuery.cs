using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FavoriteMenu.Queries
{
    public class GetTkdFavoriteMenuDataListQuery : IRequest<List<TKD_FavoriteMenuData>>
    {
        public class Handler : IRequestHandler<GetTkdFavoriteMenuDataListQuery, List<TKD_FavoriteMenuData>>
        {
            private readonly KobodbContext _kobodbContext;

            public Handler(KobodbContext context)
            {
                _kobodbContext = context;
            }

            /// <summary>
            /// Query to get all of Favorite Menu
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<TKD_FavoriteMenuData>> Handle(GetTkdFavoriteMenuDataListQuery request, CancellationToken cancellationToken)
            {
                var query = (from record in _kobodbContext.TkdFavoriteMenu.Where(e => e.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq)
                             orderby record.DisplayOrder
                             select new TKD_FavoriteMenuData()
                             {
                                 FavoriteMenu_FavoriteMenuCdSeq = record.FavoriteMenuCdSeq,
                                 FavoriteMenu_MenuTitle = record.MenuTitle,
                                 FavoriteMenu_MenuUrl = record.MenuUrl,
                                 FavoriteMenu_DisplayOrder = record.DisplayOrder
                             });
                return query.ToList();
            }
        }
    }
}
