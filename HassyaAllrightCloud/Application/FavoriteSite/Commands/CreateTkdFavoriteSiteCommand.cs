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

namespace HassyaAllrightCloud.Application.FavoriteSite
{
    public class CreateTkdFavoriteSiteCommand : IRequest<string>
    {
        public TKD_FavoriteSiteData FavoriteSiteData;

        public class Handler : IRequestHandler<CreateTkdFavoriteSiteCommand, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(CreateTkdFavoriteSiteCommand request, CancellationToken cancellationToken)
            {
                string result = null;
                var existTkdFavoriteSite = _context.TkdFavoriteSite.FirstOrDefault(x => x.FavoriteSiteCdSeq == request.FavoriteSiteData.FavoriteSite_FavoriteSiteCdSeq);
                var maxDisplayOrder = _context.TkdFavoriteSite.Any(x => x.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq) ? _context.TkdFavoriteSite.Where(x => x.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Max(x => x.DisplayOrder) : 0;
                if (existTkdFavoriteSite == null && request.FavoriteSiteData != null)
                {
                    using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            TkdFavoriteSite tkdFavoriteSite = new TkdFavoriteSite
                            {
                                SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                                SiteTitle = request.FavoriteSiteData.FavoriteSite_SiteTitle,
                                SiteUrl = request.FavoriteSiteData.FavoriteSite_SiteUrl,
                                DisplayOrder = (short) (maxDisplayOrder + 1),
                                UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                                UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd),
                                UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                                UpdPrgId = Common.UpdPrgId
                            };

                            await _context.TkdFavoriteSite.AddAsync(tkdFavoriteSite);
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
