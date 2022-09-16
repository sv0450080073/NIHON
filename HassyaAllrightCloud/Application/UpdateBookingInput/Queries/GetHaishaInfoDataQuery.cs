using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BookingInputData;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Queries
{
    public class GetHaishaInfoDataQuery: IRequest<List<HaishaInfoData>>
    {
        public string Ukeno { get; set; }
        public short Unkren { get; set; }

        public class Handler : IRequestHandler<GetHaishaInfoDataQuery, List<HaishaInfoData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetHaishaByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetHaishaByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public Task<List<HaishaInfoData>> Handle(GetHaishaInfoDataQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var eigyos = _context.VpmEigyos.Join(
                        _context.VpmCompny,
                        e => e.CompanyCdSeq,
                        c => c.CompanyCdSeq,
                        (e, c) => new { e, c })
                        .Where(a => a.c.TenantCdSeq == new ClaimModel().TenantID)
                        .Select(a => a.e).ToList();
                    List<TkdHaisha> haishas = _context.TkdHaisha.Where(h => h.UkeNo == request.Ukeno && h.UnkRen == request.Unkren && h.SiyoKbn == 1).ToList();
                    List<TkdYykSyu> yykSyus = _context.TkdYykSyu.Where(h => h.UkeNo == request.Ukeno && h.UnkRen == request.Unkren && h.SiyoKbn == 1).ToList();
                    //Get Syasyu Info
                    var syaSyuRenMapCdSeq = yykSyus.ToDictionary(y => y.SyaSyuRen, y => y.SyaSyuCdSeq);
                    var syaSyuCdSeqMapNm = _context.VpmSyaSyu.Where(s => syaSyuRenMapCdSeq.Values.Contains(s.SyaSyuCdSeq) && s.TenantCdSeq == new ClaimModel().TenantID).Distinct().ToDictionary(s => s.SyaSyuCdSeq, s => s.SyaSyuNm);
                    syaSyuCdSeqMapNm[0] = "指定なし";
                    //End Syasyu Info

                    //Get BorrowBranch Info
                    var youtbl = haishas.Select(h => h.YouTblSeq).Where(y => y != 0);
                    var borrowBranchMap = _context.TkdYousha.Join(
                        _context.VpmTokisk,
                        yousha => yousha.YouCdSeq,
                        tokisk => tokisk.TokuiSeq,
                        (yousha, tokisk) => new { yousha, tokisk })
                        .Join(_context.VpmTokiSt, yt => yt.tokisk.TokuiSeq, tokist => tokist.TokuiSeq, (yt, tokist) => new { yt, tokist })
                        .Where(tb => tb.yt.yousha.UkeNo == request.Ukeno && 
                        tb.yt.yousha.UnkRen == request.Unkren && 
                        youtbl.Contains(tb.yt.yousha.YouTblSeq) && tb.yt.yousha.YouSitCdSeq == tb.tokist.SitenCdSeq).ToDictionary(tb => tb.yt.yousha.YouTblSeq, tb => tb);
                    //End BorrowBranch Info

                    //Get SyaRyo info
                    var syaRyoCdSeqMapNm = _context.VpmSyaRyo.Where(s => haishas.Select(h => h.HaiSsryCdSeq).Contains(s.SyaRyoCdSeq)).ToDictionary(s => s.SyaRyoCdSeq, s => s.SyaRyoNm);
                    //End SyaRyo info

                    //Get MFuttu info
                    var teidannos = haishas.Select(h => h.TeiDanNo).ToList();
                    var tsukomi = _context.TkdMfutTu
                        .Where(m => m.UkeNo == request.Ukeno &&
                                    m.UnkRen == request.Unkren && 
                                    teidannos.Contains(m.TeiDanNo) &&
                                    m.FutTumKbn == 2).Any();
                    var futai = _context.TkdMfutTu
                        .Where(m => m.UkeNo == request.Ukeno &&
                                    m.UnkRen == request.Unkren &&
                                    teidannos.Contains(m.TeiDanNo) &&
                                    m.FutTumKbn == 1).Any();
                    //End MFuttu info

                    List<HaishaInfoData> result = new List<HaishaInfoData>();
                    result = haishas
                        .GroupBy(h => h.TeiDanNo, h => h, (key, h) => new HaishaInfoData
                        {
                            UkeNo = h.First().UkeNo,
                            UnkRen = h.First().UnkRen,
                            SyaSyuRen = h.First().SyaSyuRen,
                            TeidanNo = h.First().TeiDanNo,
                            YouTblSeqList = h.Select(h => h.YouTblSeq).ToList(),
                            GoSya = h.First().GoSya,
                            HaiSYmd = h.Select(h => h.HaiSymd).OrderBy(s => s).ToList().First(),
                            TouYmd = h.Select(h => h.HaiSymd).OrderBy(s => s).ToList().Last(),
                            SyaSyuNm = syaSyuCdSeqMapNm.GetValueOrDefault(syaSyuRenMapCdSeq[h.First().SyaSyuRen]) ?? "",
                            BranchName = eigyos.Where(e => e.EigyoCdSeq == h.First().SyuEigCdSeq).FirstOrDefault()?.EigyoNm ?? "",
                            BorrowBranch = h.Select(h => h.YouTblSeq).Any(y => y != 0) ? borrowBranchMap[h.FirstOrDefault(h => h.YouTblSeq != 0).YouTblSeq].tokist.SitenNm : "",
                            SyaRyoNm = syaRyoCdSeqMapNm.GetValueOrDefault(h.First().HaiSsryCdSeq) ?? "",
                            Tsukomi = tsukomi ? "〇" : "",
                            Futai = futai ? "〇" : "",
                        }).OrderBy(h => h.SyaSyuRen).ToList();

                    return Task.FromResult(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogTrace(ex, ex.Message);
                    throw;
                }
            }
        }
    }
}
