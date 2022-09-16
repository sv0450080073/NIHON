using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ILeaveDayTypeService
    {
        Task<List<LoadLeaveDayType>> Get();
    }

    public class LeaveDayTypeService : ILeaveDayTypeService
    {
        private readonly KobodbContext _dbContext;

        public LeaveDayTypeService(KobodbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<LoadLeaveDayType>> Get()
        {
            return await (
                from k in _dbContext.VpmKinKyu
                where k.SiyoKbn == 1
                    && k.KinKyuKbn == 2
                    && k.TenantCdSeq == new ClaimModel().TenantID
                orderby k.KinKyuCd, k.KinKyuCdSeq
                select new LoadLeaveDayType()
                {
                    TypeKbnSeq = k.KinKyuCdSeq,
                    TypeName = k.KinKyuNm
                }).ToListAsync();
        }
    }
}
