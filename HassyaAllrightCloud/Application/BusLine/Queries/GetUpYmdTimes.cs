using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusLine.Queries
{
    public class GetUpYmdTimes : IRequest<List<ResponseHaiTaCheck>>
    {
        public List<ParamHaiTaCheck> DataParam = new List<ParamHaiTaCheck>();
        public class Handler : IRequestHandler<GetUpYmdTimes, List<ResponseHaiTaCheck>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ResponseHaiTaCheck>> Handle(GetUpYmdTimes request, CancellationToken cancellationToken)
            {
                var tkdYyksho = new TkdYyksho();
                var tKdUnkobi = new TkdUnkobi();
                var tKdYykSyu = new TkdYykSyu();
                var tKdHaisha = new TkdHaisha();
                var responseHaiTaChecks = new List<ResponseHaiTaCheck>();

                foreach (var item in request.DataParam)
                {
                    var upYmdTimes = new List<UpYmdTime>();

                    tkdYyksho = _context.TkdYyksho.FirstOrDefault(x => x.UkeNo == item.UkeNo);
                    tKdUnkobi = _context.TkdUnkobi.FirstOrDefault(x => x.UkeNo == item.UkeNo && x.UnkRen == item.UnkRen);
                    tKdYykSyu = _context.TkdYykSyu.FirstOrDefault(x => x.UkeNo == item.UkeNo && x.UnkRen == item.UnkRen && x.SyaSyuRen == item.SyaSyuRen);
                    tKdHaisha = _context.TkdHaisha.FirstOrDefault(x => x.UkeNo == item.UkeNo && x.UnkRen == item.UnkRen && x.TeiDanNo == item.TeiDanNo && x.BunkRen == item.BunkRen);

                    upYmdTimes.Add(new UpYmdTime
                    {
                        UkeNo = item?.UkeNo,
                        UpdYmd = tkdYyksho?.UpdYmd,
                        UpdTime = tkdYyksho?.UpdTime
                    });

                    upYmdTimes.Add(new UpYmdTime
                    {
                        UkeNo = item?.UkeNo,
                        UpdYmd = tKdUnkobi?.UpdYmd,
                        UpdTime = tKdUnkobi?.UpdTime
                    });

                    upYmdTimes.Add(new UpYmdTime
                    {
                        UkeNo = item?.UkeNo,
                        UpdYmd = tKdYykSyu?.UpdYmd,
                        UpdTime = tKdYykSyu?.UpdTime
                    });

                    upYmdTimes.Add(new UpYmdTime
                    {
                        UkeNo = item?.UkeNo,
                        UpdYmd = tKdHaisha?.UpdYmd,
                        UpdTime = tKdHaisha?.UpdTime
                    });

                    responseHaiTaChecks.Add(new ResponseHaiTaCheck
                    {
                        UpYmdTimes = upYmdTimes,
                        BunkRen = item.BunkRen,
                        TeiDanNo = item.TeiDanNo,
                        UnkRen = item.UnkRen,
                        UkeNo= item?.UkeNo
                    });
                }
                return responseHaiTaChecks;
            }
        }
    }
}
