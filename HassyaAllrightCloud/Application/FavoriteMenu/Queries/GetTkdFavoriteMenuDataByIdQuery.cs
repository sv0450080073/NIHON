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

namespace HassyaAllrightCloud.Application.FavoriteMenu.Queries
{
    public class GetTkdFavoriteMenuDataByIdQuery : IRequest<TKD_FavoriteMenuData>
    {
        public int ID { get; set; }

        public class Handler : IRequestHandler<GetTkdFavoriteMenuDataByIdQuery, TKD_FavoriteMenuData>
        {
            private readonly KobodbContext _kobodbContext;

            public Handler(KobodbContext context)
            {
                _kobodbContext = context;
            }

            /// <summary>
            /// Query to get one of Favorite Menu by ID
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<TKD_FavoriteMenuData> Handle(GetTkdFavoriteMenuDataByIdQuery request, CancellationToken cancellationToken)
            {
                return (from record in _kobodbContext.TkdFavoriteMenu
                             where record.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq & record.FavoriteMenuCdSeq == request.ID
                             select new TKD_FavoriteMenuData()
                             {
                                 FavoriteMenu_FavoriteMenuCdSeq = record.FavoriteMenuCdSeq,
                                 FavoriteMenu_MenuTitle = record.MenuTitle,
                                 FavoriteMenu_MenuUrl = record.MenuUrl,
                                 FavoriteMenu_DisplayOrder = record.DisplayOrder
                             }).FirstOrDefault();
            }
        }
    }
}
