using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.CustomItem.Queries
{
    public class GetCustomItemQuery : IRequest<List<CustomFieldConfigs>>
    {
        private int _tenant { get; set; }

        public GetCustomItemQuery(int tenant)
        {
            _tenant = tenant;
        }

        public class Handler : IRequestHandler<GetCustomItemQuery, List<CustomFieldConfigs>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCustomItemQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetCustomItemQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<CustomFieldConfigs>> Handle(GetCustomItemQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await
                            (from c in _context.VpmCustomItems
                             where c.TenantCdSeq == request._tenant
                             select new CustomFieldConfigs()
                             {
                                 id = c.CustomItemsNo,
                                 Label = c.CustomName,
                                 Description = c.Description,
                                 CustomFieldType = (FieldType)c.FormatKbn,
                                 IsRequired = c.RequiredItemFlg == 1,
                                 TextLength = c.TextLengh == 0 ? "" : c.TextLengh.ToString(),
                                 NumMax = c.NumberRangeLimitFlg == 0 ? "" : c.NumberRangeOrMore.ToString(),
                                 NumMin = c.NumberRangeLimitFlg == 0 ? "" : c.NumberRangeOrLess.ToString(),
                                 NumInitialValue = c.NumberInitialValueFlg == 0 ? "" : c.NumberInitialValue.ToString(),
                                 NumUnit = c.NumberUnitFlg == 0 ? "" : c.NumberUnit,
                                 IsRounded = c.NumberRoundKbn != 0,
                                 NumRoundSettings = (RoundSettings)c.NumberRoundKbn,
                                 NumScale = c.NumberScale.ToString(),
                                 DateMax = c.DatetimeRangeLimitFlg == 0 ? "" : c.DatetimeRangeLimitEndYmd,
                                 DateMin = c.DatetimeRangeLimitFlg == 0 ? "" : c.DatetimeRangeLimitStartYmd,
                                 DateInitial = "", // TODO codekbn not exist
                                 DateFormat = "yyyy/MM/dd", // TODO codekbn not exist
                                 TimeMax = c.TimeRangeLimitFlg == 0 ? "" : c.TimeRangeLimitEndTime,
                                 TimeMin = c.TimeRangeLimitFlg == 0 ? "" : c.TimeRangeLimitStartTime,
                                 TimeInitial = "", // TODO codekbn not exist
                                 ComboboxSource = "YOYASYU", // TODO column not exist in DB
                                 CombobxInitial = "", // TODO column not exist in DB
                                 CustomKoumokuSource = c.CodeSyu, //"CUSTOMKOUMOKU" + c.CustomItemsNo.ToString("D2"),
                             }).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return null;
                }


            }
        }
    }
}
