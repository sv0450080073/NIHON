using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FaresUpperAndLowerLimits.Queries
{
    public class GetCommonCombobox : IRequest<CommonListCombobox>
    {
        public class Handler : IRequestHandler<GetCommonCombobox, CommonListCombobox>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<CommonListCombobox> Handle(GetCommonCombobox request, CancellationToken cancellationToken)
            {
                var currentUser = new ClaimModel();

                var lstSaleOffices = (from e in _context.VpmEigyos
                                      join c in _context.VpmCompny
                                      on e.CompanyCdSeq equals c.CompanyCdSeq
                                      join t in _context.VpmTenant
                                      on c.TenantCdSeq equals t.TenantCdSeq
                                      where c.SiyoKbn == 1 && t.SiyoKbn == 1
                                            && t.TenantCdSeq == currentUser.TenantID /*ログインユーザのテナントのTenantCdSeq*/
                                            && c.CompanyCdSeq == currentUser.CompanyID  /*ログインユーザの会社のCompanyCdSeq*/
                                            && e.SiyoKbn == 1
                                      select new SaleOffice()
                                      {
                                          EigyoCd = e.EigyoCd,      /*営業所コード*/
                                          RyakuNm = e.RyakuNm,      /*省略名*/
                                          EigyoCdSeq = e.EigyoCdSeq,
                                      }).ToList();

                var now = DateTime.Now.ToString(Formats.yyyyMMdd);
                var lstSalePersonInCharge = (from s in _context.VpmSyain
                                             join k in _context.VpmKyoShe on s.SyainCdSeq equals k.SyainCdSeq into ktmp
                                             from k in ktmp.DefaultIfEmpty()
                                             join e in _context.VpmEigyos on k.EigyoCdSeq equals e.EigyoCdSeq into etmp
                                             from e in etmp.DefaultIfEmpty()
                                             join c in _context.VpmCompny on e.CompanyCdSeq equals c.CompanyCdSeq into ctmp
                                             from c in ctmp.DefaultIfEmpty()
                                             join t in _context.VpmTenant on c.TenantCdSeq equals t.TenantCdSeq into ttmp
                                             from t in ttmp.DefaultIfEmpty()
                                             where e.SiyoKbn == CommonConstants.SiyoKbn && c.SiyoKbn == CommonConstants.SiyoKbn &&
                                                   t.SiyoKbn == CommonConstants.SiyoKbn && k.StaYmd.CompareTo(now) <= 0 &&
                                                   k.EndYmd.CompareTo(now) >= 0 && c.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID &&
                                                   t.TenantCdSeq == new ClaimModel().TenantID
                                             select new SalePersonInCharge
                                             {
                                                 StaYmd = k.StaYmd,
                                                 EigyoCdSeq = k.EigyoCdSeq,
                                                 EndYmd = k.EndYmd,
                                                 SyainCd = s.SyainCd,
                                                 SyainCdSeq = s.SyainCdSeq,
                                                 SyainNm = s.SyainNm,
                                                 TenkoNo = k.TenkoNo,
                                             }).ToList();

                return new CommonListCombobox()
                {
                    SaleOffices = lstSaleOffices,
                    SalePersonInCharges = lstSalePersonInCharge
                };
            }
        }
    }
}
