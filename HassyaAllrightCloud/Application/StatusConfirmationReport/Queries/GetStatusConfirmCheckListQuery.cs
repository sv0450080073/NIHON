using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StatusConfirmationReport.Queries
{
    public class GetStatusConfirmCheckListQuery : IRequest<List<BookingKeyData>>
    {
        public StatusConfirmationData searchOption;

        public GetStatusConfirmCheckListQuery(StatusConfirmationData option)
        {
            searchOption = option;
        }

        public class Handler : IRequestHandler<GetStatusConfirmCheckListQuery, List<BookingKeyData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetStatusConfirmCheckListQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetStatusConfirmCheckListQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<BookingKeyData>> Handle(GetStatusConfirmCheckListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var kaknin =
                        from kak in
                            from k in _context.TkdKaknin
                            group k by k.UkeNo into g
                            select new
                            {
                                CountKak = g.Count(),
                                KaknRen = g.Max(_ => _.KaknRen),
                                KaknAit = g.Max(_ => _.KaknAit),
                                SaikFlg = g.Max(_ => _.SaikFlg),
                                DaiSuFlg = g.Max(_ => _.DaiSuFlg),
                                KingFlg = g.Max(_ => _.KingFlg),
                                NitteiFlg = g.Max(_ => _.NitteiFlg),
                                UkeNo = g.Key
                            }

                        join kk in _context.TkdKaknin on new { kak.UkeNo, kak.KaknRen } equals new { kk.UkeNo, kk.KaknRen }
                        select new
                        {
                            kk.UkeNo,
                            kk.KaknYmd,
                            kk.KaknNin,
                            kk.KaknAit,
                            kak.SaikFlg,
                            kak.DaiSuFlg,
                            kak.KingFlg,
                            kak.NitteiFlg,
                            kak.CountKak
                        };

                    int startDate = int.Parse(request.searchOption.StartDate.ToString(CommonConstants.FormatYMD));
                    int endDate = int.Parse(request.searchOption.EndDate.ToString(CommonConstants.FormatYMD));

                    var tokuifrom = request.searchOption.TokiStTokuiSakiFrom.TokuiSeq;
                    var tokuito = request.searchOption.TokiskTokuiSakiTo.TokuiSeq;
                    var sitenfrom = request.searchOption.TokiStTokuiSakiFrom.SitenCdSeq;
                    var sitento = request.searchOption.TokiStTokuiSakiTo.SitenCdSeq;

                    var result = await
                          (from yyksyu in _context.TkdYykSyu
                           join yyksho in _context.TkdYyksho on yyksyu.UkeNo equals yyksho.UkeNo
                           join unkobi in _context.TkdUnkobi on yyksho.UkeNo equals unkobi.UkeNo
                           join kak in kaknin on yyksho.UkeNo equals kak.UkeNo into kaks
                           from sub in kaks.DefaultIfEmpty()
                           where
                              startDate.CompareTo(Convert.ToInt32(unkobi.HaiSymd)) <= 0
                              &&
                              endDate.CompareTo(Convert.ToInt32(unkobi.TouYmd)) >= 0
                              &&
                              (request.searchOption.BranchStart.BranchInfo.Equals(Constants.SelectedAll) ? true : yyksho.UkeEigCdSeq >= request.searchOption.BranchStart.EigyoCdSeq)
                              &&
                              (request.searchOption.BranchEnd.BranchInfo.Equals(Constants.SelectedAll) ? true : yyksho.UkeEigCdSeq <= request.searchOption.BranchEnd.EigyoCdSeq)
                              //&&
                              //(tokuifrom == 0 && tokuito == 0)
                              //      || (tokuifrom != tokuito && yyksho.TokuiSeq == tokuifrom && yyksho.SitenCdSeq >= sitenfrom)
                              //      || (tokuifrom != tokuito && yyksho.TokuiSeq == tokuito && yyksho.SitenCdSeq <= sitento)
                              //      || (tokuifrom == tokuito && yyksho.TokuiSeq == tokuifrom && yyksho.SitenCdSeq >= sitenfrom && yyksho.SitenCdSeq <= sitento)
                              //      || (tokuifrom == 0 && tokuito != 0 && ((yyksho.TokuiSeq == tokuito && yyksho.SitenCdSeq <= sitento) || yyksho.TokuiSeq < tokuito))
                              //      || (tokuito == 0 && tokuifrom != 0 && ((yyksho.TokuiSeq == tokuifrom && yyksho.SitenCdSeq >= sitenfrom) || yyksho.TokuiSeq > tokuifrom))
                              //      || (yyksho.TokuiSeq < tokuito && yyksho.TokuiSeq > tokuifrom)
                              &&
                              (request.searchOption.FixedStatus == ConfirmStatus.Fixed ? !string.IsNullOrEmpty(yyksho.KaktYmd) : string.IsNullOrEmpty(yyksho.KaktYmd))
                              &&
                              (request.searchOption.ConfirmedStatus == ConfirmStatus.Confirmed ? request.searchOption.ConfirmedTimes.Option == NumberOfConfirmed.Unknown ? sub.CountKak > 0 : request.searchOption.ConfirmedTimes.Option == NumberOfConfirmed.Other ? sub.CountKak >= 10 : sub.CountKak == (int)request.searchOption.ConfirmedTimes.Option : sub.CountKak == null)
                              &&
                              (request.searchOption.Saikou.Option == ConfirmStatus.Unknown ? true : request.searchOption.Saikou.Option == ConfirmStatus.Confirmed ? sub.SaikFlg == 1 : sub.SaikFlg == 0)
                              &&
                              (request.searchOption.SumDai.Option == ConfirmStatus.Unknown ? true : request.searchOption.SumDai.Option == ConfirmStatus.Confirmed ? sub.DaiSuFlg == 1 : sub.DaiSuFlg == 0)
                              &&
                              (request.searchOption.Ammount.Option == ConfirmStatus.Unknown ? true : request.searchOption.Ammount.Option == ConfirmStatus.Confirmed ? sub.KingFlg == 1 : sub.KingFlg == 0)
                              &&
                              (request.searchOption.ScheduleDate.Option == ConfirmStatus.Unknown ? true : request.searchOption.ScheduleDate.Option == ConfirmStatus.Confirmed ? sub.NitteiFlg == 1 : sub.NitteiFlg == 0)

                           select new BookingKeyData
                           {
                               UkeNo = unkobi.UkeNo,
                               UnkRen = unkobi.UnkRen,
                           }
                         ).Distinct().ToListAsync();

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return new List<BookingKeyData>();
                }
            }
        }
    }
}
