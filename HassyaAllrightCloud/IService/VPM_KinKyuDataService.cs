using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.IService
{

    public interface IVPM_KinKyuDataService
    {
        Task<IEnumerable<VpmKinKyuData>> GetDataKinKyu();
    }
    public class VPM_KinKyuDataService : IVPM_KinKyuDataService
    {
        private readonly KobodbContext _dbContext;
        public VPM_KinKyuDataService(KobodbContext context)
        {
            _dbContext = context;
        }
        public async Task<IEnumerable<VpmKinKyuData>> GetDataKinKyu()
        {
            return await (from VPM_KinKyu in _dbContext.VpmKinKyu
                          where
                            VPM_KinKyu.SiyoKbn == 1 && VPM_KinKyu.TenantCdSeq == new ClaimModel().TenantID
                          select new VpmKinKyuData()
                          {
                              KinKyuCdSeq = VPM_KinKyu.KinKyuCdSeq,
                              KinKyuCd = VPM_KinKyu.KinKyuCd,
                              KinKyuNm = VPM_KinKyu.KinKyuNm,
                              RyakuNm = VPM_KinKyu.RyakuNm,
                              KinKyuKbn = VPM_KinKyu.KinKyuKbn,
                              ColKinKyu = VPM_KinKyu.ColKinKyu,
                              KyuSyukinNm = VPM_KinKyu.KyuSyukinNm,
                              KyuSyukinRyaku = VPM_KinKyu.KyuSyukinRyaku,
                              DefaultSyukinTime = VPM_KinKyu.DefaultSyukinTime,
                              DefaultTaiknTime = VPM_KinKyu.DefaultTaiknTime,
                              KyusyutsuKbn = VPM_KinKyu.KyusyutsuKbn,
                          }
                         ).ToListAsync();
        }
    }
}
