using HassyaAllrightCloud.Application.KasSet.Queries;
using HassyaAllrightCloud.Commons.Constants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public class HasuSettings
    {
        public RoundSettings TaxSetting { get; set; }
        public RoundSettings FeeSetting { get; set; }
    }

    public interface IRoundSettingsService
    {
        public Task<HasuSettings> GetHasuSettings(int company);
    }

    public class RoundSettingsService : IRoundSettingsService
    {
        private IMediator mediatR;
        public RoundSettingsService(IMediator mediator)
        {
            mediatR = mediator;
        }
        public async Task<HasuSettings> GetHasuSettings(int company)
        {
            var kasSet = await mediatR.Send(new GetKasSetByIdQuery { Id = company });
            if (kasSet == null)
            {
                return new HasuSettings { TaxSetting = RoundSettings.Round, FeeSetting = RoundSettings.Round };
            }
            return new HasuSettings { TaxSetting = (RoundSettings)kasSet.SyohiHasu, FeeSetting = (RoundSettings)kasSet.TesuHasu };
        }
    }
}
