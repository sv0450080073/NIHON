using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.Koteik.Queries
{
    public class CollectDatakoteikQuery : IRequest<TkdKoteik>
    {
        public TKD_KoteikData TKD_KoteiFormData { get; set; }
        public class Handler : IRequestHandler<CollectDatakoteikQuery, TkdKoteik>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            { 
                _context = context;
            }
            public async Task<TkdKoteik> Handle(CollectDatakoteikQuery request, CancellationToken cancellationToken)
            {
                TkdKoteik koteik = new TkdKoteik();
                koteik.UkeNo = request.TKD_KoteiFormData.UkeNo;
                koteik.UnkRen = request.TKD_KoteiFormData.UnkRen;
                koteik.TeiDanNo = request.TKD_KoteiFormData.TeiDanNo;
                koteik.BunkRen = request.TKD_KoteiFormData.BunkRen;
                koteik.TomKbn = 1;
                koteik.Nittei = request.TKD_KoteiFormData.Nittei;
                koteik.TeiDanNittei = request.TKD_KoteiFormData.Nittei;

                koteik.TeiDanTomKbn = 1;
                if (request.TKD_KoteiFormData.isZenHaFlg)
                {
                    koteik.TomKbn = 2;
                    koteik.Nittei = 1;
                    koteik.TeiDanNittei = 1;
                    koteik.TeiDanTomKbn = 2;
                }
                if (request.TKD_KoteiFormData.isKhakFlg)
                {
                    koteik.TomKbn = 3;
                    koteik.Nittei = 1;
                    koteik.TeiDanNittei = 1;
                    koteik.TeiDanTomKbn = 3;
                }
                if (request.TKD_KoteiFormData.isCommom)
                {
                    koteik.TeiDanNittei = 0;
                    koteik.TeiDanTomKbn = 9;
                }
                koteik.SyuEigCdSeq = 0;
                koteik.SyukoNm = "";
                koteik.SyuPaCdSeq = 0;
                koteik.SyuPaNm = "";
                koteik.SyuPaTime = request.TKD_KoteiFormData.SyuPaTime;
                koteik.SyukoTime = request.TKD_KoteiFormData.SyukoTime;
                koteik.HaiStime = request.TKD_KoteiFormData.HaiSTime;
                koteik.KikTime = request.TKD_KoteiFormData.KikTime;
                koteik.KeiyuMapCdSeq = 0;
                koteik.KeiyuNm = "";
                koteik.TouCdSeq = 1;
                koteik.TouNm = "";
                koteik.TouChTime = request.TKD_KoteiFormData.TouChTime;
                koteik.KikEigSeq = 0;
                koteik.KikoNm = "";
                koteik.ShakuMapCdSeq = 0;
                koteik.ShakuNm = "";
                koteik.TaikTime = "";
                koteik.KyuKmapCdSeq = 1;
                koteik.KyuKnm = "";
                koteik.KyuKtime = "";
                koteik.KyuKstaTime = "";
                koteik.KyuKendTime = "";
                koteik.BikoNm = "";
                koteik.JisaIpkm = request.TKD_KoteiFormData.JisaIPKm;
                koteik.JisaKskm = request.TKD_KoteiFormData.JisaKSKm;
                koteik.KisoIpkm = request.TKD_KoteiFormData.KisoIPkm;
                koteik.KisoKokm = request.TKD_KoteiFormData.KisoKOKm;
                koteik.SiyoKbn = 1;
                koteik.HenKai = 0;
                koteik.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                koteik.UpdTime = DateTime.Now.ToString("HHmmss");
                koteik.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                koteik.UpdPrgId = "KJ1100";
                return await Task.FromResult(koteik);
            }
        }
    }
}
