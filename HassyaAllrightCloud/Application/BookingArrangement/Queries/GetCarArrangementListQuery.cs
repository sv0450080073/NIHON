using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingArrangement.Queries
{
    public class GetCarArrangementListQuery : IRequest<List<ArrangementCar>>
    {
        private readonly string _ukeNo;

        public GetCarArrangementListQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetCarArrangementListQuery, List<ArrangementCar>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ArrangementCar>> Handle(GetCarArrangementListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new List<ArrangementCar>();
                    List<TkdTehai> tehaiList = new List<TkdTehai>();
                    tehaiList = await _context.TkdTehai
                                    .Where(t => t.UkeNo == request._ukeNo)
                                    .ToListAsync();

                    result = await (from unkobi in _context.TkdUnkobi
                                    join haisha in _context.TkdHaisha
                                    on new { unkobi.UkeNo, unkobi.UnkRen } equals new { haisha.UkeNo, haisha.UnkRen } into haishaGr
                                    from haishaSub in haishaGr.DefaultIfEmpty()
                                    where haishaSub.UkeNo == request._ukeNo && haishaSub.SiyoKbn == 1
                                    orderby haishaSub.TeiDanNo, haishaSub.BunkRen
                                    select new ArrangementCar()
                                    {
                                        BunkRen = haishaSub.BunkRen,
                                        Gosya = haishaSub.GoSya,
                                        SyaSyuRen = haishaSub.SyaSyuRen,
                                        TeiDanNo = haishaSub.TeiDanNo,
                                        UnkRen = haishaSub.UnkRen,
                                        IsAfterDate = haishaSub.KikYmd != haishaSub.TouYmd,
                                        IsPreviousDate = haishaSub.SyuKoYmd != haishaSub.HaiSymd,
                                        StartDateString = haishaSub.HaiSymd,
                                        EndDateString = haishaSub.TouYmd
                                    }).ToListAsync();

                    // check if car is splited
                    foreach (var haishaCut in result.Where(h => h.BunkRen > 1))
                    {
                        result.ForEach(t =>
                        {
                            if(t.TeiDanNo == haishaCut.TeiDanNo && t.UnkRen == haishaCut.UnkRen)
                            {
                                t.IsCut = true;
                            }
                        });
                    }

                    // get tehRenMax of car
                    result.ForEach(t => 
                    {
                        var tehais = tehaiList.Where(h => h.UnkRen == t.UnkRen && h.TeiDanNo == t.TeiDanNo && h.BunkRen == t.BunkRen).ToList();
                        if(tehais.Count > 0)
                        {
                            t.TehRenMax = tehais.Max(x => x.TehRen);
                        }
                    });

                    // get common car
                    var commonCar = await (_context.TkdUnkobi.Where(u => u.UkeNo == request._ukeNo)
                                            .Select(u => new ArrangementCar()
                                            {
                                                TeiDanNo = 0,
                                                BunkRen = 0,
                                                IsPreviousDate = u.HaiSymd != u.SyukoYmd,
                                                IsAfterDate = u.TouYmd != u.KikYmd,
                                                StartDateString = u.HaiSymd,
                                                EndDateString = u.TouYmd
                                            })
                                            .SingleOrDefaultAsync());
                    tehaiList = tehaiList.Where(h => h.TeiDanNo == 0).ToList();
                    if(tehaiList.Count > 0)
                    {
                        commonCar.TehRenMax = tehaiList.Max(x => x.TehRen);
                    }
                    
                    result.Insert(0, commonCar);
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
