using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITPM_YoyKbnDataListService
    {
        Task<List<ReservationData>> GetYoyKbnbySiyoKbn();
        Task<List<ReservationData>> GetYoyKbn();
    }
    public class TPM_YoyKbnDataList : ITPM_YoyKbnDataListService
    {
        private readonly KobodbContext _dbContext;

        public TPM_YoyKbnDataList(KobodbContext context)
        {
            _dbContext = context;
        }

        public Task<List<ReservationData>> GetYoyKbn()
        {
            var tenantCdSeq = new ClaimModel().TenantID;
            return _dbContext.VpmYoyKbn.Where(e => e.SiyoKbn == 1 && e.TenantCdSeq == tenantCdSeq).Select(e => new ReservationData()
            {
                YoyaKbnSeq = e.YoyaKbnSeq,
                YoyaKbn = e.YoyaKbn,
                YoyaKbnNm = e.YoyaKbnNm
            }).OrderBy(e => e.YoyaKbn).ToListAsync();
        }

        //get data KbnbySiyoKbn
        public Task<List<ReservationData>> GetYoyKbnbySiyoKbn()
        {
            var tenantCdSeq = new ClaimModel().TenantID;
            var data = (from VPM_YoyKbn in _dbContext.VpmYoyKbn
                        where VPM_YoyKbn.SiyoKbn == 1  && VPM_YoyKbn.TenantCdSeq == tenantCdSeq
                        orderby VPM_YoyKbn.YoyaKbn
                        select new ReservationData()
                        {
                            YoyaKbnSeq = VPM_YoyKbn.YoyaKbnSeq,
                            YoyaKbn = VPM_YoyKbn.YoyaKbn,
                            YoyaKbnNm = VPM_YoyKbn.YoyaKbnNm,
                        }).ToList();
            return Task.FromResult(data.OrderBy(x => x.PriorityNum).ToList());
        }
    }
}
