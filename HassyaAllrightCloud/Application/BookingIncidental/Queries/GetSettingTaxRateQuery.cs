using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingIncidental.Queries
{
    public class GetSettingTaxRateQuery : IRequest<SettingTaxRate>
    {
        public class Handler : IRequestHandler<GetSettingTaxRateQuery, SettingTaxRate>
        {
            private KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<SettingTaxRate> Handle(GetSettingTaxRateQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var setting = await _context.VpmKyoSet.AsNoTracking().FirstOrDefaultAsync();

                    var result = new SettingTaxRate()
                    {
                        Zeiritsu1 = setting.Zeiritsu1,
                        Zeiritsu2 = setting.Zeiritsu2,
                        Zeiritsu3 = setting.Zeiritsu3,
                    };
                    DateTime dt = new DateTime();
                    if (DateTime.TryParseExact(setting.Zei1StaYmd, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                    {
                        result.Zei1StartDate = dt;
                    }
                    if (DateTime.TryParseExact(setting.Zei1EndYmd, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                    {
                        result.Zei1EndDate = dt;
                    }
                    if (DateTime.TryParseExact(setting.Zei2StaYmd, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                    {
                        result.Zei2StartDate = dt;
                    }
                    if (DateTime.TryParseExact(setting.Zei2EndYmd, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                    {
                        result.Zei2EndDate = dt;
                    }
                    if (DateTime.TryParseExact(setting.Zei3StaYmd, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                    {
                        result.Zei3StartDate = dt;
                    }
                    if (DateTime.TryParseExact(setting.Zei3EndYmd, "yyyyMMdd", null, DateTimeStyles.None, out dt))
                    {
                        result.Zei3EndDate = dt;
                    }

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
