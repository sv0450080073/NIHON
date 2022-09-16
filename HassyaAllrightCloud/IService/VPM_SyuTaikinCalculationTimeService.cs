using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IVPM_SyuTaikinCalculationTimeService
    {
        List<VPM_SyuTaikinCalculationTime> GetSyuTaikinCalculationTime();
    }
    public class VPM_SyuTaikinCalculationTimeService : IVPM_SyuTaikinCalculationTimeService
    {
        private readonly KobodbContext _dbContext;

        public VPM_SyuTaikinCalculationTimeService(KobodbContext context)
        {
            _dbContext = context;
        }
        public List<VPM_SyuTaikinCalculationTime> GetSyuTaikinCalculationTime()
        {
            return  (from SyuTaikinCalculationTime in _dbContext.VpmSyuTaikinCalculationTime
                          select new VPM_SyuTaikinCalculationTime()
                          {
                              CompanyCdSeq = SyuTaikinCalculationTime.CompanyCdSeq,
                              SyugyoKbn = SyuTaikinCalculationTime.SyugyoKbn,
                              KouZokPtnKbn = SyuTaikinCalculationTime.KouZokPtnKbn,
                              SyukinCalculationTimeMinutes = SyuTaikinCalculationTime.SyukinCalculationTimeMinutes,
                              TaikinCalculationTimeMinutes = SyuTaikinCalculationTime.TaikinCalculationTimeMinutes
                          }).ToList();
        }

    }
}
