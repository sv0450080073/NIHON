using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AttendanceConfirm.Queries
{
    public class GetListTeHaiByItemHaiSha : IRequest<List<TehaiReport>>
    {
        public string Ukeno { get; set; }
        public int UnkRen { get; set; }
        public int TeiDanNo { get; set; }
        public int BunkRen { get; set; }
        public int Nittei { get; set; }
        public int TomKbn { get; set; }
    }
    public class Handler : IRequestHandler<GetListTeHaiByItemHaiSha, List<TehaiReport>>
    {
        private readonly KobodbContext _context;
        public Handler(KobodbContext context)
        {
            _context = context;
        }

        public async Task<List<TehaiReport>> Handle(GetListTeHaiByItemHaiSha request, CancellationToken cancellationToken)
        {
            List<TehaiReport> result = new List<TehaiReport>();
            try
            {
                result = (from Tehai in _context.TkdTehai
                          where Tehai.SiyoKbn == 1
                          && Tehai.TehaiCdSeq == 1
                          && Tehai.Nittei == request.Nittei
                          && Tehai.UkeNo == request.Ukeno
                          && Tehai.UnkRen == request.UnkRen
                          && Tehai.BunkRen == request.BunkRen
                          && Tehai.TeiDanNo == request.TeiDanNo
                          && Tehai.TomKbn == request.TomKbn
                          orderby Tehai.TehRen
                          select new TehaiReport()
                          {
                              Ukeno = Tehai.UkeNo,
                              UnkRen = Tehai.UnkRen,
                              BunkRen = Tehai.BunkRen,
                              TeiDanNo = Tehai.TeiDanNo,
                              TehNm = Tehai.TehNm
                          }).ToList();
                return result;
            }
            catch(Exception ex)
            {
                return result;
            }

        }
    }



}
