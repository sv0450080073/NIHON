using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.IService
{
    public interface ICustomerCLassificationListService
    {
        Task<List<CustomerClassification>> Get(int Tenant);
    }
    public class CustomerCLassificationService : ICustomerCLassificationListService
    {
        private readonly KobodbContext _dbContext;

        public CustomerCLassificationService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<CustomerClassification>> Get(int Tenant)
        {
            return await (from jyoKya in _dbContext.VpmJyoKya
                          where jyoKya.SiyoKbn == 1 && jyoKya.TenantCdSeq == Tenant
                          orderby jyoKya.JyoKyakuCd, jyoKya.JyoKyakuCdSeq
                          select new CustomerClassification(jyoKya)).ToListAsync();
        }
    }
}
