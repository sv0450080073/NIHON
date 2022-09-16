using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FavoriteSite.Commands
{
    public class DeleteTkdFavoriteSiteCommand : IRequest<string>
    {
        public int ID;

        public class Handler : IRequestHandler<DeleteTkdFavoriteSiteCommand, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(DeleteTkdFavoriteSiteCommand request, CancellationToken cancellationToken)
            {
                string result = "";
                TkdFavoriteSite tkdFavoriteSite = await _context.TkdFavoriteSite.FindAsync(request.ID);
                if (tkdFavoriteSite == null || request.ID == null)
                {
                    result = Constants.ErrorMessage.RecordNotFound;
                } else
                {
                    try
                    {
                        _context.TkdFavoriteSite.Remove(tkdFavoriteSite);
                        await _context.SaveChangesAsync();
                    } catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                }
                return result;
            }
        }
    }
}
