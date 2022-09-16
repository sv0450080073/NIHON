using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.IService
{
    public interface ITranSportationFreeRuleService
    {
        Task<List<VpmTransportationFeeRule>> Get();
        Task<VpmTransportationFeeRule> Get(int id);
    }
    public class TranSportationFreeRuleService : ITranSportationFreeRuleService
    {
        private readonly KobodbContext _dbContext;

        public TranSportationFreeRuleService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<VpmTransportationFeeRule>> Get()
        {
            return await _dbContext.VpmTransportationFeeRule.ToListAsync();
        }
        public async Task<VpmTransportationFeeRule> Get(int id)
        {
            var toDo = await _dbContext.VpmTransportationFeeRule.FindAsync(id);
            return toDo;
        }
    }
}
