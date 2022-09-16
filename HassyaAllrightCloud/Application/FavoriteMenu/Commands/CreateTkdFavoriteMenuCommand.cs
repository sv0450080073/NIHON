using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FavoriteMenu.Commands
{
    public class CreateTkdFavoriteMenuCommand : IRequest<string>
    {
        public TKD_FavoriteMenuData FavoriteMenuData;

        public class Handler : IRequestHandler<CreateTkdFavoriteMenuCommand, string>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(CreateTkdFavoriteMenuCommand request, CancellationToken cancellationToken)
            {
                string result = null;
                var existTkdFavoriteMenu = _context.TkdFavoriteMenu.FirstOrDefault(x => x.FavoriteMenuCdSeq == request.FavoriteMenuData.FavoriteMenu_FavoriteMenuCdSeq);
                var maxDisplayOrder = _context.TkdFavoriteMenu.Any(x => x.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq) ? _context.TkdFavoriteMenu.Where(x => x.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Max(x => x.DisplayOrder) : 0;
                if (existTkdFavoriteMenu == null && request.FavoriteMenuData != null)
                {
                    using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            TkdFavoriteMenu tkdFavoriteMenu = new TkdFavoriteMenu
                            {
                                SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                                MenuTitle = request.FavoriteMenuData.FavoriteMenu_MenuTitle,
                                MenuUrl = request.FavoriteMenuData.FavoriteMenu_MenuUrl,
                                DisplayOrder = (short)(maxDisplayOrder + 1),
                                UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                                UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                                UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                                UpdPrgId = Common.UpdPrgId
                            };

                            await _context.TkdFavoriteMenu.AddAsync(tkdFavoriteMenu);
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
