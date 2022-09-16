using DevExpress.Data.Linq.Helpers;
using DevExpress.Data.ODataLinq.Helpers;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace HassyaAllrightCloud.Application.YoushaNotice.Queries
{
    public class GetUnreadNotificationsQuery:IRequest<int>
    {
        private readonly int _tenantCdSeq;
        public GetUnreadNotificationsQuery(int tenantCdSeq)
        {
            _tenantCdSeq = tenantCdSeq;
        }
        public class Handler : IRequestHandler<GetUnreadNotificationsQuery, int>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                   return (from TKD_YoushaNotice in
                            (from TKD_YoushaNotice in _context.TkdYoushaNotice
                             where
                               TKD_YoushaNotice.UkeTenantCdSeq == request._tenantCdSeq &&
                               TKD_YoushaNotice.SiyoKbn == 1 &&
                               TKD_YoushaNotice.UnReadKbn == 1
                             select new
                             {
                                 Dummy = "x"
                             })
                            group TKD_YoushaNotice by new { TKD_YoushaNotice.Dummy } into g
                            select new
                            {
                                Column1 = g.Count()
                            }).Count();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            }
        }
    }
