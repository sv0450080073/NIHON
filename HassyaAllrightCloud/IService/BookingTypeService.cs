using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using HassyaAllrightCloud.Application.YoyKbn.Queries;

namespace HassyaAllrightCloud.IService
{
    public interface IBookingTypeListService
    {
        Task<List<VpmYoyKbn>> Get();
        Task<List<BookingTypeData>> GetBySiyoKbn();
        Task<List<BookingTypeData>> GetBookingTypeData();
    }
    public class BookingTypeService : IBookingTypeListService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediator;
        public IMemoryCache MemoryCache { get; }

        public BookingTypeService(KobodbContext context, IMemoryCache memoryCache, IMediator mediator)
        {
            _dbContext = context;
            MemoryCache = memoryCache;
            _mediator = mediator;
        }
        public async Task<List<VpmYoyKbn>> Get()
        {
            return await _dbContext.VpmYoyKbn.ToListAsync();
        }

        public async Task<List<BookingTypeData>> GetBySiyoKbn()
        {
            return await (from yoyKbn in _dbContext.VpmYoyKbn
                          where yoyKbn.SiyoKbn == 1
                          && yoyKbn.TenantCdSeq == new ClaimModel().TenantID
                          orderby yoyKbn.YoyaKbn, yoyKbn.YoyaKbnSeq
                          select new BookingTypeData(yoyKbn)).ToListAsync();
        }

        public Task<List<BookingTypeData>> GetBookingTypeData()
        {
            return MemoryCache.GetOrCreateAsync("AllBookingType", async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(10)
                });

                //var data = await (from VPM_YoyKbn in _dbContext.VpmYoyKbn
                //            join VPM_YoyaKbnSort in _dbContext.VpmYoyaKbnSort
                //            on new { VPM_YoyKbn.YoyaKbnSeq, T1 = new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID }
                //            equals new { VPM_YoyaKbnSort.YoyaKbnSeq, T1 = VPM_YoyaKbnSort.TenantCdSeq }
                //            into VPM_YoyaKbnSort_join
                //            from VPM_YoyaKbnSort in VPM_YoyaKbnSort_join.DefaultIfEmpty()
                //            where VPM_YoyKbn.SiyoKbn == 1
                //            select new ReservationData()
                //            {
                //                YoyaKbnSeq = VPM_YoyKbn.YoyaKbnSeq,
                //                YoyaKbn = VPM_YoyKbn.YoyaKbn,
                //                YoyaKbnNm = VPM_YoyKbn.YoyaKbnNm,
                //                PriorityNum = VPM_YoyaKbnSort.PriorityNum != null ? VPM_YoyaKbnSort.PriorityNum.ToString("D2") : "99"
                //                + VPM_YoyKbn.YoyaKbn.ToString("D2"),
                //            }).ToListAsync();
                var data = await _mediator.Send(new GetTpmYoyKbnWithPriorityNumQuery());
                var listReservation = data.OrderBy(x => x.PriorityNum).ToList();

                return listReservation.Select(r => new BookingTypeData(r)).ToList();
            });
        }
    }
}
