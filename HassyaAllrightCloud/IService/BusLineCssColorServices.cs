using HassyaAllrightCloud.Application.BusLineScheduleColor.Queries;
using HassyaAllrightCloud.Commons.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IBusLineCssColorServices
    {
        Task<Dictionary<string,string>> GetAllCssColor();
        Task<(string cssColor, bool isLock)> GetCssColor(string ukeNo, short unkRen, short teiDanNo, short bunkRen);
    }

    public class BusLineCssColorServices : IBusLineCssColorServices
    {
        private IMediator _mediatR;
        private readonly ILogger<BusLineCssColorServices> _logger;

        public BusLineCssColorServices(IMediator mediatR, ILogger<BusLineCssColorServices> logger)
        {
            _mediatR = mediatR;
            _logger = logger;
        }

        public async Task<(string cssColor, bool isLock)> GetCssColor(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {   
            try
            {
                return await _mediatR.Send(new GetBusLineScheduleColorQuery(ukeNo, unkRen, teiDanNo, bunkRen));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (BusCssClass.KarishaColor, false);
            }
        }

        public async Task<Dictionary<string, string>> GetAllCssColor()
        {
            try
            {
                return await _mediatR.Send(new GetAllBusLineScheduleColorQuery());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Dictionary<string, string>();
            }
        }
    }
}
