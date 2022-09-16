using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HyperData.Queries
{
    public class GetDataReservationToCheckQuery : IRequest<List<ReservationDataToCheck>>
    {
        public List<string> receiptNumber { get; set; }
        public class Handler : IRequestHandler<GetDataReservationToCheckQuery, List<ReservationDataToCheck>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<ReservationDataToCheck>> Handle(GetDataReservationToCheckQuery request, CancellationToken cancellationToken)
            {
                return await (from yksho in _dbContext.TkdYyksho
                              join unkobi in _dbContext.TkdUnkobi on new { yksho.UkeNo } equals new { unkobi.UkeNo }
                              join eigyos in _dbContext.VpmEigyos on yksho.UkeEigCdSeq equals eigyos.EigyoCdSeq
                              join company in _dbContext.VpmCompny on eigyos.CompanyCdSeq equals company.CompanyCdSeq
                              join uke_futTum1 in
                                 from futTum in _dbContext.TkdFutTum
                                 where futTum.SiyoKbn == 1
                                 && futTum.SeisanKbn == 1
                                 && futTum.NyuKinKbn == 2
                                 && futTum.UriGakKin != 0
                                 group futTum by futTum.UkeNo into uke_group
                                 select new UkeCount()
                                 {
                                     UkeNo = uke_group.Key,
                                     UkeNoCount = uke_group.Count()
                                 }
                               on yksho.UkeNo equals uke_futTum1.UkeNo into uke_futTum1_join
                              from uke_futTum1 in uke_futTum1_join.DefaultIfEmpty()
                              join uke_futTum2 in
                                 from futTum in _dbContext.TkdFutTum
                                 where futTum.SiyoKbn == 1
                                 && futTum.SeisanKbn == 1
                                 && futTum.NyuKinKbn == 3
                                 && futTum.UriGakKin != 0
                                 group futTum by futTum.UkeNo into uke_group
                                 select new UkeCount()
                                 {
                                     UkeNo = uke_group.Key,
                                     UkeNoCount = uke_group.Count()
                                 }
                               on yksho.UkeNo equals uke_futTum2.UkeNo into uke_futTum2_join
                              from uke_futTum2 in uke_futTum2_join.DefaultIfEmpty()
                              join uke_futTum3 in
                                from futTum in _dbContext.TkdFutTum
                                where futTum.SiyoKbn == 1
                                && futTum.SeisanKbn == 1
                                && futTum.NyuKinKbn == 4
                                && futTum.UriGakKin != 0
                                group futTum by futTum.UkeNo into uke_group
                                select new UkeCount()
                                {
                                    UkeNo = uke_group.Key,
                                    UkeNoCount = uke_group.Count()
                                }
                              on yksho.UkeNo equals uke_futTum3.UkeNo into uke_futTum3_join
                              from uke_futTum3 in uke_futTum3_join.DefaultIfEmpty()
                              join uke_futTum4 in
                                from futTum in _dbContext.TkdFutTum
                                where futTum.SiyoKbn == 1
                                && futTum.SeisanKbn == 1
                                && futTum.NcouKbn == 2
                                && futTum.UriGakKin != 0
                                group futTum by futTum.UkeNo into uke_group
                                select new UkeCount()
                                {
                                    UkeNo = uke_group.Key,
                                    UkeNoCount = uke_group.Count()
                                }
                              on yksho.UkeNo equals uke_futTum4.UkeNo into uke_futTum4_join
                              from uke_futTum4 in uke_futTum4_join.DefaultIfEmpty()
                              join uke_futTum5 in
                                from futTum in _dbContext.TkdFutTum
                                where futTum.SiyoKbn == 1
                                && futTum.SeisanKbn == 1
                                && futTum.NcouKbn == 3
                                && futTum.UriGakKin != 0
                                group futTum by futTum.UkeNo into uke_group
                                select new UkeCount()
                                {
                                    UkeNo = uke_group.Key,
                                    UkeNoCount = uke_group.Count()
                                }
                              on yksho.UkeNo equals uke_futTum5.UkeNo into uke_futTum5_join
                              from uke_futTum5 in uke_futTum5_join.DefaultIfEmpty()
                              join uke_yfutTu1 in
                                 from yfutTu in _dbContext.TkdYfutTu
                                 where yfutTu.SiyoKbn == 1
                                 && yfutTu.SeisanKbn == 1
                                 && yfutTu.SihKbn == 2
                                 && yfutTu.HaseiKin != 0
                                 group yfutTu by yfutTu.UkeNo into uke_group
                                 select new UkeCount()
                                 {
                                     UkeNo = uke_group.Key,
                                     UkeNoCount = uke_group.Count()
                                 }
                               on yksho.UkeNo equals uke_yfutTu1.UkeNo into uke_yfutTu1_join
                              from uke_yfutTu1 in uke_yfutTu1_join.DefaultIfEmpty()
                              join uke_yfutTu2 in
                                 from yfutTu in _dbContext.TkdYfutTu
                                 where yfutTu.SiyoKbn == 1
                                 && yfutTu.SeisanKbn == 1
                                 && yfutTu.SihKbn == 3
                                 && yfutTu.HaseiKin != 0
                                 group yfutTu by yfutTu.UkeNo into uke_group
                                 select new UkeCount()
                                 {
                                     UkeNo = uke_group.Key,
                                     UkeNoCount = uke_group.Count()
                                 }
                               on yksho.UkeNo equals uke_yfutTu2.UkeNo into uke_yfutTu2_join
                              from uke_yfutTu2 in uke_yfutTu2_join.DefaultIfEmpty()
                              join uke_yfutTu3 in
                                from yfutTu in _dbContext.TkdYfutTu
                                where yfutTu.SiyoKbn == 1
                                && yfutTu.SeisanKbn == 1
                                && yfutTu.SihKbn == 4
                                && yfutTu.HaseiKin != 0
                                group yfutTu by yfutTu.UkeNo into uke_group
                                select new UkeCount()
                                {
                                    UkeNo = uke_group.Key,
                                    UkeNoCount = uke_group.Count()
                                }
                              on yksho.UkeNo equals uke_yfutTu3.UkeNo into uke_yfutTu3_join
                              from uke_yfutTu3 in uke_yfutTu3_join.DefaultIfEmpty()
                              join uke_yfutTu4 in
                                from yfutTu in _dbContext.TkdYfutTu
                                where yfutTu.SiyoKbn == 1
                                && yfutTu.SeisanKbn == 1
                                && yfutTu.SihKbn == 2
                                && yfutTu.HaseiKin != 0
                                group yfutTu by yfutTu.UkeNo into uke_group
                                select new UkeCount()
                                {
                                    UkeNo = uke_group.Key,
                                    UkeNoCount = uke_group.Count()
                                }
                              on yksho.UkeNo equals uke_yfutTu4.UkeNo into uke_yfutTu4_join
                              from uke_yfutTu4 in uke_yfutTu4_join.DefaultIfEmpty()
                              join uke_yfutTu5 in
                                from yfutTu in _dbContext.TkdYfutTu
                                where yfutTu.SiyoKbn == 1
                                && yfutTu.SeisanKbn == 1
                                && yfutTu.SihKbn == 3
                                && yfutTu.HaseiKin != 0
                                group yfutTu by yfutTu.UkeNo into uke_group
                                select new UkeCount()
                                {
                                    UkeNo = uke_group.Key,
                                    UkeNoCount = uke_group.Count()
                                }
                              on yksho.UkeNo equals uke_yfutTu5.UkeNo into uke_yfutTu5_join
                              from uke_yfutTu5 in uke_yfutTu5_join.DefaultIfEmpty()
                              where request.receiptNumber.Contains(yksho.UkeNo)
                             && unkobi.SiyoKbn == 1
                             && eigyos.SiyoKbn == 1
                             && company.SiyoKbn == 1
                              select new ReservationDataToCheck()
                              {
                                  ReceiptNumber = yksho.UkeNo,
                                  CompanySeq = company.CompanyCdSeq,
                                  NippoKbn = unkobi.NippoKbn,
                                  DepositClassificationStatus = yksho.NyuKinKbn != 1
                                                                || yksho.NcouKbn != 1
                                                                || uke_futTum1.UkeNoCount > 0
                                                                || uke_futTum2.UkeNoCount > 0
                                                                || uke_futTum3.UkeNoCount > 0
                                                                || uke_futTum4.UkeNoCount > 0
                                                                || uke_futTum5.UkeNoCount > 0,
                                  PaymentClassificationStatus = yksho.SihKbn != 1
                                                                || yksho.ScouKbn != 1
                                                                || uke_yfutTu1.UkeNoCount > 0
                                                                || uke_yfutTu2.UkeNoCount > 0
                                                                || uke_yfutTu3.UkeNoCount > 0
                                                                || uke_yfutTu4.UkeNoCount > 0
                                                                || uke_yfutTu5.UkeNoCount > 0,
                                  ClosedStatus = string.Compare(company.SyoriYm + company.SimeD.ToString("D2"), unkobi.HaiSymd) > 0
                                                && string.Compare(company.SyoriYm + company.SimeD.ToString("D2"), unkobi.TouYmd) > 0,
                                  HaiSymd = unkobi.HaiSymd

                              }).ToListAsync();
            }
        }
    }
}
