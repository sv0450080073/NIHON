using HassyaAllrightCloud.Application.FavoriteMenu.Commands;
using HassyaAllrightCloud.Application.FavoriteMenu.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_FavoriteMenuService
    {
        public Task<List<TKD_FavoriteMenuData>> GetFavoriteMenuList();
        public Task<TKD_FavoriteMenuData> GetFavoriteMenuById(int id);
        public Task<string> CreateFavoriteMenu(TKD_FavoriteMenuData favoriteMenuData);
        public Task<string> UpdateFavoriteMenu(TKD_FavoriteMenuData favoriteMenuData);
        public Task<string> DeleteFavoriteMenu(int id);
        Task<bool> SaveFavoriteMenuOrder(ListOrderDto[] orderedList);
    }

    public class TKD_FavoriteMenuService : ITKD_FavoriteMenuService
    {
        private IMediator _mediator;
        public TKD_FavoriteMenuService(IMediator mediatR)
        {
            _mediator = mediatR;
        }

        public async Task<List<TKD_FavoriteMenuData>> GetFavoriteMenuList()
        {
            return await _mediator.Send(new GetTkdFavoriteMenuDataListQuery());
        }

        public async Task<TKD_FavoriteMenuData> GetFavoriteMenuById(int id)
        {
            return await _mediator.Send(new GetTkdFavoriteMenuDataByIdQuery() { ID = id });
        }

        public async Task<string> CreateFavoriteMenu(TKD_FavoriteMenuData favoriteMenuData)
        {
            return await _mediator.Send(new CreateTkdFavoriteMenuCommand { FavoriteMenuData = favoriteMenuData });
        }

        public async Task<string> UpdateFavoriteMenu(TKD_FavoriteMenuData favoriteMenuData)
        {
            return await _mediator.Send(new UpdateTkdFavoriteMenuCommand { FavoriteMenuData = favoriteMenuData });
        }

        public async Task<string> DeleteFavoriteMenu(int id)
        {
            return await _mediator.Send(new DeleteTkdFavoriteMenuCommand { ID = id });
        }

        public async Task<bool> SaveFavoriteMenuOrder(ListOrderDto[] orderedList)
        {
            return await _mediator.Send(new SaveFavoriteMenuOrderCommand { OrderedList = orderedList });
        }
    }
}
