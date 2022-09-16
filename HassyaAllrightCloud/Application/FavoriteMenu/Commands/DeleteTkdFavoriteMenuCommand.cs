using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FavoriteMenu.Commands
{
    public class DeleteTkdFavoriteMenuCommand : IRequest<string>
    {
        public int ID;

        public class Handler : IRequestHandler<DeleteTkdFavoriteMenuCommand, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(DeleteTkdFavoriteMenuCommand request, CancellationToken cancellationToken)
            {
                string result = "";
                TkdFavoriteMenu tkdFavoriteMenu = await _context.TkdFavoriteMenu.FindAsync(request.ID);
                if (tkdFavoriteMenu == null || request.ID == null)
                {
                    result = Constants.ErrorMessage.RecordNotFound;
                }
                else
                {
                    try
                    {
                        _context.TkdFavoriteMenu.Remove(tkdFavoriteMenu);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                }
                return result;
            }
        }
    }
}
