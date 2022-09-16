using DevExpress.Xpo;
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
    public class SaveFavoriteMenuOrderCommand : IRequest<bool>
    {
        public ListOrderDto[] OrderedList { get; set; }

        public class Handler : IRequestHandler<SaveFavoriteMenuOrderCommand, bool>
        {
            private readonly KobodbContext context;

            public Handler(KobodbContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(SaveFavoriteMenuOrderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (var menuOrder in request.OrderedList)
                    {
                        var menu = context.TkdFavoriteMenu.FirstOrDefault(x => x.FavoriteMenuCdSeq == int.Parse(menuOrder.Id));
                        if (menu != null)
                        {
                            menu.DisplayOrder = (short)(short.Parse(menuOrder.Order) + (short)1);
                        }
                        context.TkdFavoriteMenu.Update(menu);
                    }

                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    return false;
                }
            }
        }
    }
}
