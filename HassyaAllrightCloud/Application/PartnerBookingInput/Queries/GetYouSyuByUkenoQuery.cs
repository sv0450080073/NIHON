using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetYouSyuByUkenoQuery : IRequest<Dictionary<CommandMode, List<TkdYouSyu>>>
    {
        public string Ukeno { get; set; }
        public YouShaDataInsert YouShaDataInsert { get; set; }
        public class Handler : IRequestHandler<GetYouSyuByUkenoQuery, Dictionary<CommandMode, List<TkdYouSyu>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetYouSyuByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetYouSyuByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<Dictionary<CommandMode, List<TkdYouSyu>>> Handle(GetYouSyuByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdYouSyu>> result = new Dictionary<CommandMode, List<TkdYouSyu>>();
                    List<TkdYouSyu> listYouSyu = _context.TkdYouSyu.Where(e => e.UkeNo == request.Ukeno
                                                                           && e.UnkRen == request.YouShaDataInsert.YouShaDataPopup.YOUSHA_UnkRen
                                                                           && e.YouTblSeq == request.YouShaDataInsert.YouShaDataPopup.YOUSHA_YouTblSeq).ToList();
                    List<TkdYouSyu> addNewYouSyuList = new List<TkdYouSyu>();
                    List<TkdYouSyu> removeYouSyuList = new List<TkdYouSyu>();
                    List<TkdYouSyu> updateYouSyuList = new List<TkdYouSyu>();
                    if (listYouSyu != null)
                    {
                        foreach (var item in request.YouShaDataInsert.YyKSyuDataPopups)
                        {
                            var youSyu = listYouSyu.FirstOrDefault(x => x.UkeNo == item.YYKSYU_UkeNo
                            && x.UnkRen == item.YYKSYU_UnkRen
                            && x.SyaSyuRen == item.YYKSYU_SyaSyuRen);
                            if (youSyu == null)
                            {
                                youSyu = new TkdYouSyu();
                                youSyu.UkeNo = request.Ukeno;
                                youSyu.UnkRen = request.YouShaDataInsert.YouShaDataPopup.YOUSHA_UnkRen;
                                youSyu.YouTblSeq = request.YouShaDataInsert.YouShaDataPopup.YOUSHA_YouTblSeq;
                                youSyu.SyaSyuRen = item.YYKSYU_SyaSyuRen;
                                youSyu.SiyoKbn = 1;
                                youSyu.HenKai = 0;
                                addNewYouSyuList.Add(youSyu);
                            }
                            else
                            {
                                youSyu.HenKai++;
                                updateYouSyuList.Add(youSyu);
                            }
                            youSyu.YouKataKbn = (byte)item.BusTypeDataPartner.Id;
                            youSyu.SyaSyuDai = item.YOUSYU_SyaSyuDai;
                            youSyu.SyaSyuTan = item.YOUSYU_SyaSyuTan;
                            youSyu.SyaRyoUnc = item.YOUSYU_SyaRyoUnc;
                            youSyu.SiyoKbn = 1;
                            youSyu.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                            youSyu.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                            youSyu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            youSyu.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                        }
                    }
                    result.Add(CommandMode.Create, addNewYouSyuList);
                    result.Add(CommandMode.Update, updateYouSyuList);
                    result.Add(CommandMode.Delete, removeYouSyuList);
                    return Task.FromResult(result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}