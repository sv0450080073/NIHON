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

namespace HassyaAllrightCloud.Application.FavoriteSite.Commands
{
    public class SaveFavoriteSiteOrderCommand : IRequest<bool>
    {
        public ListOrderDto[] OrderedList { get; set; }

        public class Handler : IRequestHandler<SaveFavoriteSiteOrderCommand, bool>
        {
            private readonly KobodbContext context;

            public Handler(KobodbContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(SaveFavoriteSiteOrderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (var siteOrder in request.OrderedList)
                    {
                        var site = context.TkdFavoriteSite.FirstOrDefault(x => x.FavoriteSiteCdSeq == int.Parse(siteOrder.Id));
                        if (site != null)
                        {
                            site.DisplayOrder = (short)(short.Parse(siteOrder.Order) + (short)1);
                        }
                        context.TkdFavoriteSite.Update(site);
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
