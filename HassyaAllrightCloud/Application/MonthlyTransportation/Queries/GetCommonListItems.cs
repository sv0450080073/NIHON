using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportationSummary.Queries
{
    public class GetCommonListItems : IRequest<CommonListitems>
    {
        public class Handler : IRequestHandler<GetCommonListItems, CommonListitems>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<CommonListitems> Handle(GetCommonListItems request, CancellationToken cancellationToken)
            {
                var currentUser = new ClaimModel();

                var lstCompany = (from c in _context.VpmCompny
                                  join t in _context.VpmTenant
                                  on c.TenantCdSeq equals t.TenantCdSeq
                                  where t.SiyoKbn == 1
                                  && c.CompanyCdSeq == currentUser.CompanyID
                                  && c.TenantCdSeq == currentUser.TenantID      /*ログインユーザの会社のCompanyCdSeq*/
                                  select new CompanyItem()
                                  {
                                      CompanyCd = c.CompanyCd,       /*会社コード*/
                                      RyakuNm = c.RyakuNm,           /*省略名*/
                                      CompanyCdSeq = c.CompanyCdSeq,
                                      CompanyNm = c.CompanyNm,
                                      TenantCdSeq = c.TenantCdSeq,
                                  }).ToList();

                var lstEigyo = (from e in _context.VpmEigyos
                                join c in _context.VpmCompny
                                on e.CompanyCdSeq equals c.CompanyCdSeq
                                join t in _context.VpmTenant
                                on c.TenantCdSeq equals t.TenantCdSeq
                                where c.SiyoKbn == 1 && t.SiyoKbn == 1
                                      && t.TenantCdSeq == currentUser.TenantID /*ログインユーザのテナントのTenantCdSeq*/
                                      && c.CompanyCdSeq == currentUser.CompanyID  /*ログインユーザの会社のCompanyCdSeq*/
                                      && e.SiyoKbn == 1
                                select new EigyoItem()
                                {
                                    EigyoCd = e.EigyoCd,      /*営業所コード*/
                                    RyakuNm = e.RyakuNm,      /*省略名*/
                                    EigyoCdSeq = e.EigyoCdSeq,
                                }).ToList();

                var data = _context.VpmCodeKb.Where(x => x.CodeSyu == "UNSOUKBN" && x.SiyoKbn == 1 && x.TenantCdSeq == currentUser.TenantID);

                var lstShipping = _context.VpmCodeKb.Where(x => x.CodeSyu == "UNSOUKBN"
                                                  && x.SiyoKbn == 1
                                                  && x.TenantCdSeq == (data.Any() ? currentUser.TenantID : 0)  /*ログインしたユーザーのTenantCdSeq*/
                                                    ).Select(x => new ShippingItem
                                                    {
                                                        CodeKbn = int.Parse(x.CodeKbn),
                                                        CodeKbnNm = x.CodeKbnNm,
                                                        CodeKbnSeq = x.CodeKbnSeq
                                                    }).ToList();
                var lstCommonItem = new List<CommonListitems>();
                return new CommonListitems
                {
                    Companys = lstCompany,
                    Eigyos = lstEigyo,
                    Shippings = lstShipping
                };
            }
        }
    }
}
