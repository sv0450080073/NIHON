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
    public class DeleteYouSyuByUkenoQuery : IRequest<Dictionary<CommandMode, List<TkdYouSyu>>>
    {
        public string Ukeno { get; set; }
        public YouShaDataTable YouShaItemUpdate { get; set; }
        public class Handler : IRequestHandler<DeleteYouSyuByUkenoQuery, Dictionary<CommandMode, List<TkdYouSyu>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<DeleteYouSyuByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<DeleteYouSyuByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<Dictionary<CommandMode, List<TkdYouSyu>>> Handle(DeleteYouSyuByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdYouSyu>> result = new Dictionary<CommandMode, List<TkdYouSyu>>();
                    List<TkdYouSyu> listYouSyu = _context.TkdYouSyu.Where(e => e.UkeNo == request.Ukeno
                                                                           && e.UnkRen == request.YouShaItemUpdate.YOUSHA_UnkRen
                                                                           && e.YouTblSeq == request.YouShaItemUpdate.YOUSHA_YouTblSeq).ToList();
                    List<TkdYouSyu> addNewYouSyuList = new List<TkdYouSyu>();
                    List<TkdYouSyu> removeYouSyuList = new List<TkdYouSyu>();
                    List<TkdYouSyu> updateYouSyuList = new List<TkdYouSyu>();
                    if (listYouSyu != null)
                    {
                        foreach (var item in listYouSyu)
                        {
                            item.HenKai++;
                            item.SiyoKbn = 2;

                            item.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                            item.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                            item.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            item.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                            updateYouSyuList.Add(item);
                        }
                    }
                    result.Add(CommandMode.Update, updateYouSyuList);
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
