using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetVATHaiShaData : IRequest<VATDataPopup>
    {
        public List<HaiShaDataTable> HaiShaDataList { get; set; }
        public class Handler : IRequestHandler<GetVATHaiShaData, VATDataPopup>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<VATDataPopup> Handle(GetVATHaiShaData request, CancellationToken cancellationToken)
            {
                var haiSYmd = "";
                if (request.HaiShaDataList != null)
                {
                    haiSYmd = request.HaiShaDataList.OrderBy(x => x.HAISHA_HaiSYmd).First().HAISHA_HaiSYmd;
                }
                var result = new VATDataPopup();
                try
                {
                    var KyosetData = new VPM_KyoSetData();
                    KyosetData = (from VPM_KyoSet in _context.VpmKyoSet
                                  select new VPM_KyoSetData()
                                  {
                                      Zeiritsu1 = VPM_KyoSet.Zeiritsu1,
                                      Zeiritsu2 = VPM_KyoSet.Zeiritsu2,
                                      Zeiritsu3 = VPM_KyoSet.Zeiritsu3,
                                      Zei1StaYmd = VPM_KyoSet.Zei1StaYmd,
                                      Zei2StaYmd = VPM_KyoSet.Zei2StaYmd,
                                      Zei3StaYmd = VPM_KyoSet.Zei3StaYmd,
                                      Zei1EndYmd = VPM_KyoSet.Zei1EndYmd,
                                      Zei2EndYmd = VPM_KyoSet.Zei2EndYmd,
                                      Zei3EndYmd = VPM_KyoSet.Zei3EndYmd
                                  }).FirstOrDefault();
                    if (haiSYmd.CompareTo(KyosetData.Zei1StaYmd) >= 0
                        && haiSYmd.CompareTo(KyosetData.Zei1EndYmd) <= 0)
                    {
                        result.VAT_HaiSha = KyosetData.Zeiritsu1;
                    }
                    else if (haiSYmd.CompareTo(KyosetData.Zei2StaYmd) >= 0
                         && haiSYmd.CompareTo(KyosetData.Zei2EndYmd) <= 0)
                    {
                        result.VAT_HaiSha = KyosetData.Zeiritsu2;
                    }
                    else if (haiSYmd.CompareTo(KyosetData.Zei3StaYmd) >= 0
                       && haiSYmd.CompareTo(KyosetData.Zei3EndYmd) <= 0)
                    {
                        result.VAT_HaiSha = KyosetData.Zeiritsu3;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
