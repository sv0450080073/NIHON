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

namespace HassyaAllrightCloud.Application.FavoriteMenu.Commands
{
    public class UpdateTkdFavoriteMenuCommand : IRequest<string>
    {

        public TKD_FavoriteMenuData FavoriteMenuData { get; set; }

        public class Handler : IRequestHandler<UpdateTkdFavoriteMenuCommand, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(UpdateTkdFavoriteMenuCommand request, CancellationToken cancellationToken)
            {
                string result = null;
                TkdFavoriteMenu tkdFavoriteMenu = await _context.TkdFavoriteMenu.FindAsync(request.FavoriteMenuData.FavoriteMenu_FavoriteMenuCdSeq);

                if (tkdFavoriteMenu == null || request.FavoriteMenuData == null)
                {
                    result = Constants.ErrorMessage.RecordNotFound;
                }
                else
                {
                    using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            tkdFavoriteMenu.SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            tkdFavoriteMenu.MenuTitle = request.FavoriteMenuData.FavoriteMenu_MenuTitle;
                            tkdFavoriteMenu.MenuUrl = request.FavoriteMenuData.FavoriteMenu_MenuUrl;
                            tkdFavoriteMenu.DisplayOrder = request.FavoriteMenuData.FavoriteMenu_DisplayOrder;
                            tkdFavoriteMenu.UpdTime = DateTime.Now.ToString(Formats.HHmmss);
                            tkdFavoriteMenu.UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd);
                            tkdFavoriteMenu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            tkdFavoriteMenu.UpdPrgId = Common.UpdPrgId;
                            _context.Entry(tkdFavoriteMenu).State = EntityState.Modified;

                            await _context.SaveChangesAsync();
                            await dbTran.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            result = ex.Message;
                            await dbTran.RollbackAsync();
                        }
                    }
                }
                return result;
            }
        }
    }

}
