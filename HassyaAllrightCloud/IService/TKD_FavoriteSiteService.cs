using HassyaAllrightCloud.Application.FavoriteSite;
using HassyaAllrightCloud.Application.FavoriteSite.Commands;
using HassyaAllrightCloud.Application.FavoriteSite.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_FavoriteSiteService
    {
        public Task<List<TKD_FavoriteSiteData>> GetFavoriteSiteList();
        public Task<TKD_FavoriteSiteData> GetFavoriteSiteById(int id);
        public Task<string> CreateFavoriteSite(TKD_FavoriteSiteData favoriteSiteData);
        public Task<string> UpdateFavoriteSite(TKD_FavoriteSiteData favoriteSiteData);
        public Task<string> DeleteFavoriteSite(int id);
        Task<bool> SaveFavoriteSiteOrder(ListOrderDto[] orderedList);
    }

    public class TKD_FavoriteSiteService : ITKD_FavoriteSiteService
    {
        public IMediator _mediator;

        public TKD_FavoriteSiteService(IMediator mediatR)
        {
            _mediator = mediatR;
        }

        public async Task<List<TKD_FavoriteSiteData>> GetFavoriteSiteList()
        {
            return await _mediator.Send(new GetTkdFavoriteSiteDataListQuery());
        }

        public async Task<TKD_FavoriteSiteData> GetFavoriteSiteById(int id)
        {
            return await _mediator.Send(new GetTkdFavoriteSiteDataByIdQuery() { ID = id });
        }

        public async Task<string> CreateFavoriteSite(TKD_FavoriteSiteData favoriteSiteData)
        {
            return await _mediator.Send(new CreateTkdFavoriteSiteCommand { FavoriteSiteData = favoriteSiteData });
        }

        public async Task<string> UpdateFavoriteSite(TKD_FavoriteSiteData favoriteSiteData)
        {
            return await _mediator.Send(new UpdateTkdFavoriteSiteCommand { FavoriteSiteData = favoriteSiteData });
        }

        public async Task<string> DeleteFavoriteSite(int id)
        {
            return await _mediator.Send(new DeleteTkdFavoriteSiteCommand { ID = id });
        }

        public async Task<bool> SaveFavoriteSiteOrder(ListOrderDto[] orderedList)
        {
            return await _mediator.Send(new SaveFavoriteSiteOrderCommand { OrderedList = orderedList });
        }
    }
}
