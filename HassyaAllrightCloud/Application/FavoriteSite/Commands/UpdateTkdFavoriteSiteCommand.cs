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

namespace HassyaAllrightCloud.Application.FavoriteSite.Commands
{
    public class UpdateTkdFavoriteSiteCommand : IRequest<string>
    {

        public TKD_FavoriteSiteData FavoriteSiteData { get; set; }

        public class Handler : IRequestHandler<UpdateTkdFavoriteSiteCommand, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(UpdateTkdFavoriteSiteCommand request, CancellationToken cancellationToken)
            {
                string result = null;
                TkdFavoriteSite tkdFavoriteSite = await _context.TkdFavoriteSite.FindAsync(request.FavoriteSiteData.FavoriteSite_FavoriteSiteCdSeq);
                
                if (tkdFavoriteSite == null || request.FavoriteSiteData == null)
                {
                    result = Constants.ErrorMessage.RecordNotFound;
                } else
                {
                    using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            tkdFavoriteSite.SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            tkdFavoriteSite.SiteTitle = request.FavoriteSiteData.FavoriteSite_SiteTitle;
                            tkdFavoriteSite.SiteUrl = request.FavoriteSiteData.FavoriteSite_SiteUrl;
                            tkdFavoriteSite.DisplayOrder = request.FavoriteSiteData.FavoriteSite_DisplayOrder;
                            tkdFavoriteSite.UpdTime = DateTime.Now.ToString(Formats.HHmmss);
                            tkdFavoriteSite.UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd);
                            tkdFavoriteSite.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            tkdFavoriteSite.UpdPrgId = Common.UpdPrgId;
                            _context.Entry(tkdFavoriteSite).State = EntityState.Modified;

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
