using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_LockTableDataService
    {
        Task<List<TkdLockTable>> GetData(int tenantId);
    }

    public class TKD_LockTableDataService : ITKD_LockTableDataService
    {
        private readonly KobodbContext _dbContext;

        public TKD_LockTableDataService(KobodbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<TkdLockTable>> GetData(int tenantId)
        {
            try
            {
                return await _dbContext.TkdLockTable.Where(l => l.TenantCdSeq == tenantId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
