using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace HassyaAllrightCloud.Application.BusSchedule.Queries
{
    public class GetBookingRemarkHaiCheckQuery : IRequest<BookingRemarkHaitaCheck>
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }

        public class Handler : IRequestHandler<GetBookingRemarkHaiCheckQuery, BookingRemarkHaitaCheck>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<BookingRemarkHaitaCheck> Handle(GetBookingRemarkHaiCheckQuery request, CancellationToken cancellationToken)
            {
                var tkdYyksho = _context.TkdYyksho.Where(x => x.UkeNo == request.UkeNo).FirstOrDefault();
                var tkdUnkobi = _context.TkdUnkobi.Where(x => x.UkeNo == request.UkeNo && x.UnkRen == request.UnkRen).FirstOrDefault();
                return new BookingRemarkHaitaCheck()
                {
                    UkeNo = request.UkeNo,
                    UnkRen = request.UnkRen,
                    UnkobiUpdYmdTIme = tkdUnkobi?.UpdYmd + tkdUnkobi?.UpdTime,
                    YykshoUpdYmdTIme = tkdYyksho?.UpdYmd + tkdYyksho?.UpdTime
                };
            }
        }
    }
}