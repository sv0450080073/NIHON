using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Koteik.Commands
{
    public class PostTkdKoteikCommand : IRequest<TkdKoteik>
    {
        public TKD_KoteikData TKD_KoteiFormData;
        public class Handler : IRequestHandler<PostTkdKoteikCommand, TkdKoteik>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkdKoteik> Handle(PostTkdKoteikCommand request, CancellationToken cancellationToken)
            {
                TkdKoteik koteik = CollectDatakoteik(request.TKD_KoteiFormData);
                List<TkdKotei> listKotei = CollectDataYykSyu(request.TKD_KoteiFormData);
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (_context.TkdKoteik.Find(koteik.UkeNo, koteik.UnkRen, koteik.TeiDanNo, koteik.BunkRen, koteik.TomKbn, koteik.Nittei) == null)
                        {
                            await _context.TkdKoteik.AddAsync(koteik);
                        }
                        else
                        {
                            var updatekoteik = _context.TkdKoteik.Find(koteik.UkeNo, koteik.UnkRen, koteik.TeiDanNo, koteik.BunkRen, koteik.TomKbn, koteik.Nittei);
                            updatekoteik.SyuPaTime = koteik.SyuPaTime;
                            updatekoteik.TouChTime = koteik.TouChTime;
                            updatekoteik.JisaIpkm = koteik.JisaIpkm;
                            updatekoteik.JisaKskm = koteik.JisaKskm;
                            updatekoteik.KisoIpkm = koteik.KisoIpkm;
                            updatekoteik.KisoKokm = koteik.KisoKokm;
                            updatekoteik.HenKai++;
                        }

                        await _context.SaveChangesAsync();
                        foreach (TkdKotei item in listKotei)
                        {
                            item.UkeNo = koteik.UkeNo;
                            item.UnkRen = koteik.UnkRen;
                            item.TeiDanNo = koteik.TeiDanNo;
                            item.BunkRen = koteik.BunkRen;
                            item.TomKbn = koteik.TomKbn;
                            item.Nittei = koteik.Nittei;
                            item.TeiDanNittei = koteik.Nittei;
                            item.TeiDanTomKbn = koteik.TeiDanTomKbn;
                            if (_context.TkdKotei.Find(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen, item.TomKbn, item.Nittei, item.KouRen) == null)
                            {
                                await _context.TkdKotei.AddAsync(item);
                            }
                            else
                            {
                                var updatekotei = _context.TkdKotei.Find(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen, item.TomKbn, item.Nittei, item.KouRen);
                                updatekotei.Koutei = item.Koutei;
                                updatekotei.HenKai++;
                                updatekotei.SiyoKbn = 1;
                            }
                            if (item == listKotei.Last())
                            {
                                var getdisable = _context.TkdKotei.Where(t => t.UkeNo == item.UkeNo && t.UnkRen == item.UnkRen && t.TeiDanNo == item.TeiDanNo && t.BunkRen == item.BunkRen && t.TomKbn == item.TomKbn && t.Nittei == item.Nittei && t.KouRen > item.KouRen && t.SiyoKbn == 1).ToList();
                                foreach (var items in getdisable)
                                {
                                    var updatekotei = _context.TkdKotei.Find(items.UkeNo, items.UnkRen, items.TeiDanNo, items.BunkRen, items.TomKbn, items.Nittei, items.KouRen);
                                    updatekotei.SiyoKbn = 2;
                                    updatekotei.HenKai++;
                                }
                            }

                        }
                        await _context.SaveChangesAsync();
                        dbTran.Commit();

                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs  
                        dbTran.Rollback();
                    }
                }
                return koteik;
            }
            private TkdKoteik CollectDatakoteik(TKD_KoteikData TKD_KoteiFormData)
            {
                TkdKoteik koteik = new TkdKoteik();
                koteik.UkeNo = TKD_KoteiFormData.UkeNo;
                koteik.UnkRen = TKD_KoteiFormData.UnkRen;
                koteik.TeiDanNo = TKD_KoteiFormData.TeiDanNo;
                koteik.BunkRen = TKD_KoteiFormData.BunkRen;
                koteik.TomKbn = 1;
                koteik.Nittei = TKD_KoteiFormData.Nittei;
                koteik.TeiDanNittei = TKD_KoteiFormData.Nittei;
                koteik.TeiDanTomKbn = 1;
                if (TKD_KoteiFormData.isZenHaFlg)
                {
                    koteik.TomKbn = 2;
                    koteik.Nittei = 1;
                    koteik.TeiDanNittei = 0;
                    koteik.TeiDanTomKbn = 2;
                }
                if (TKD_KoteiFormData.isKhakFlg)
                {
                    koteik.TomKbn = 3;
                    koteik.Nittei = 1;
                    koteik.TeiDanNittei = 0;
                    koteik.TeiDanTomKbn = 3;
                }
                if (TKD_KoteiFormData.isCommom)
                {
                    koteik.TeiDanTomKbn = 9;
                }
                koteik.SyuEigCdSeq = 0;
                koteik.SyukoNm = "";
                koteik.HaiStime = "";
                koteik.SyuPaCdSeq = 0;
                koteik.SyuPaNm = "";
                koteik.SyuPaTime = TKD_KoteiFormData.SyuPaTime;
                koteik.KeiyuMapCdSeq = 0;
                koteik.KeiyuNm = "";
                koteik.TouCdSeq = 1;
                koteik.TouNm = "";
                koteik.TouChTime = TKD_KoteiFormData.TouChTime;
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
                koteik.JisaIpkm = TKD_KoteiFormData.JisaIPKm;
                koteik.JisaKskm = TKD_KoteiFormData.JisaKSKm;
                koteik.KisoIpkm = TKD_KoteiFormData.KisoIPkm;
                koteik.KisoKokm = TKD_KoteiFormData.KisoKOKm;
                koteik.SiyoKbn = 1;
                koteik.HenKai = 0;
                koteik.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                koteik.UpdTime = DateTime.Now.ToString("HHmmss");
                koteik.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                koteik.UpdPrgId = Common.UpdPrgId;
                return koteik;
            }
            private List<TkdKotei> CollectDataYykSyu(TKD_KoteikData TKD_KoteiFormData)
            {
                List<TkdKotei> listKotei = new List<TkdKotei>();
                var journeydata = TKD_KoteiFormData.JourneyLst;
                foreach (var journeyitem in journeydata)
                {
                    TkdKotei Kotei = new TkdKotei();
                    Kotei.Koutei = journeyitem.Koutei;
                    Kotei.KouRen = journeyitem.KouRen;
                    Kotei.SiyoKbn = 1;
                    Kotei.HenKai = 0;
                    Kotei.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    Kotei.UpdTime = DateTime.Now.ToString("HHmmss");
                    Kotei.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    Kotei.UpdPrgId = Common.UpdPrgId;
                    listKotei.Add(Kotei);
                }
                return listKotei;
            }
        }
    }
}
