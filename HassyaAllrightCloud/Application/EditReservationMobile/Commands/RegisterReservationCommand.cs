using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.EditReservationMobile.Commands
{
    public class RegisterReservationCommand : IRequest<string>
    {
        public ReservationMobileData Data { get; set; }
        public int CompanyCdSeq { get; set; }
        public int SyaRyoCdSeq { get; set; }
        public int TenantCdSeq { get; set; }
        public int SyainCdSeq { get; set; }
        public int EigyoCdSeq { get; set; }

        public class Handler : IRequestHandler<RegisterReservationCommand, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(RegisterReservationCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var kasset = await _context.TkmKasSet.FirstOrDefaultAsync(_ => _.CompanyCdSeq == request.CompanyCdSeq);
                        HenSyaData hensya = null;
                        if (request.SyaRyoCdSeq != 0)
                        {
                            hensya = await (from h in _context.VpmHenSya.Where(_ => _.SyaRyoCdSeq == request.SyaRyoCdSeq)
                                              join e in _context.VpmEigyos.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn)
                                              on h.EigyoCdSeq equals e.EigyoCdSeq into temp
                                              from t in temp.DefaultIfEmpty()
                                              select new HenSyaData()
                                              {
                                                  EigyoCdSeq = t.EigyoCdSeq,
                                                  EigyoCd = t.EigyoCd,
                                                  EigyoName = t.RyakuNm
                                              }).FirstOrDefaultAsync();
                        }

                        string currentDate = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);
                        string currentTime = DateTime.Now.ToString(DateTimeFormat.HHmmss);

                        var yyksho = HandleInsertYyksho(request.Data, request.TenantCdSeq, currentDate, currentTime, request.SyainCdSeq, request.EigyoCdSeq);
                        if (yyksho == null) throw new Exception("yyksho is null");

                        var listSyaSyuRen = HandleInsertYyksyu(yyksho, request.Data, currentDate, currentTime, request.SyainCdSeq);

                        HandleInsertUnkobi(yyksho, request.Data, currentDate, currentTime, request.SyainCdSeq, kasset.UriKbn);
                        HandleInsertMishum(yyksho, request.Data, currentDate, currentTime, request.SyainCdSeq);
                        HandleUpdateHaisha(yyksho, request.Data, listSyaSyuRen, currentDate, currentTime, request.SyainCdSeq, hensya, request.SyaRyoCdSeq);

                        _context.SaveChanges();
                        transaction.Commit();
                        return yyksho.UkeNo;
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            private TkdYyksho HandleInsertYyksho(ReservationMobileData data, int TenantCdSeq, string CurrentDate, string CurrentTime, int SyainCdSeq, int EigyoCdSeq)
            {
                TkdYyksho model = new TkdYyksho();
                var maxUkeCd = _context.TkdYyksho.Where(_ => _.TenantCdSeq == TenantCdSeq).Max(_ => _.UkeCd) + 1;
                model.TenantCdSeq = TenantCdSeq;
                model.UkeNo = string.Format("{0}{1}", TenantCdSeq.ToString().PadLeft(5, '0'), maxUkeCd.ToString().PadLeft(10, '0'));
                model.UkeCd = maxUkeCd;
                model.HenKai = 0;
                model.UkeYmd = CurrentDate;
                model.YoyaSyu = 1;
                model.YoyaKbnSeq = 1;
                model.KikakuNo = 0;
                model.TourCd = "0";
                model.KasTourCdSeq = 0;
                model.UkeEigCdSeq = EigyoCdSeq;
                model.SeiEigCdSeq = EigyoCdSeq;
                model.IraEigCdSeq = EigyoCdSeq;
                model.EigTanCdSeq = SyainCdSeq;
                model.InTanCdSeq = 0;
                model.YoyaNm = data.Organization;
                model.YoyaKana = string.Empty;
                model.TokuiSeq = data.Tokisk.TokuiSeq;
                model.SitenCdSeq = data.Tokisk.SitenCdSeq;
                model.SirCdSeq = data.Tokisk.TokuiSeq;
                model.SirSitenCdSeq = data.Tokisk.SitenCdSeq;
                model.TokuiTel = string.Empty;
                model.TokuiTanNm = string.Empty;
                model.TokuiFax = string.Empty;
                model.TokuiMail = string.Empty;
                model.UntKin = 0;
                model.ZeiKbn = 1;
                model.Zeiritsu = 10.0M;
                model.ZeiRui = 0;
                model.TaxTypeforGuider = 1;
                model.TaxGuider = 0;
                model.TesuRitu = 10.0M;
                model.TesuRyoG = 0;
                model.FeeGuiderRate = 0.0M;
                model.FeeGuider = 0;
                model.GuitKin = 0;
                model.SeiKyuKbnSeq = 0;
                model.SeikYm = CurrentDate.Substring(0, 6);
                model.SeiTaiYmd = CurrentDate;
                model.CanRit = 0;
                model.CanUnc = 0;
                model.CanZkbn = 0;
                model.CanSyoR = 0;
                model.CanSyoG = 0;
                model.CanYmd = string.Empty;
                model.CanTanSeq = 0;
                model.CanRiy = string.Empty;
                model.CanFuYmd = string.Empty;
                model.CanFuTanSeq = 0;
                model.CanFuRiy = string.Empty;
                model.BikoTblSeq = 0;
                model.Kskbn = 1;
                model.KhinKbn = 1;
                model.KaknKais = 0;
                model.KaktYmd = string.Empty;
                model.HaiSkbn = 1;
                model.HaiIkbn = 1;
                model.GuiWnin = 0;
                model.NippoKbn = 1;
                model.YouKbn = 1;
                model.NyuKinKbn = 1;
                model.NcouKbn = 1;
                model.SihKbn = 1;
                model.ScouKbn = 1;
                model.SiyoKbn = 1;
                model.UpdYmd = CurrentDate;
                model.UpdTime = CurrentTime;
                model.UpdSyainCd = SyainCdSeq;
                model.UpdPrgId = Common.UpdPrgId;
                _context.TkdYyksho.Add(model);
                _context.SaveChanges();
                return model;
            }

            private void HandleInsertUnkobi(TkdYyksho data, ReservationMobileData input, string CurrentDate, string CurrentTime, int SyainCdSeq, byte UriKbn)
            {
                string fromDate = input.DispatchDate.ToString(DateTimeFormat.yyyyMMdd);
                string fromTime = input.DispatchTime.Replace(":", string.Empty);
                string toDate = input.ArrivalDate.ToString(DateTimeFormat.yyyyMMdd);
                string toTime = input.ArrivalTime.Replace(":", string.Empty);
                TkdUnkobi model = new TkdUnkobi();
                model.UkeNo = data.UkeNo;
                model.UnkRen = 1;
                model.HenKai = 0;
                model.HaiSymd = fromDate;
                model.HaiStime = fromTime;
                model.TouYmd = toDate;
                model.TouChTime = toTime;
                model.DispTouYmd = toDate;
                model.DispTouChTime = toTime;
                model.SyuPaYmd = fromDate;
                model.SyuPaTime = fromTime;
                model.DispSyuPaYmd = fromDate;
                model.DispSyuPaTime = fromTime;
                model.UriYmd = UriKbn == 1 ? model.HaiSymd : model.TouYmd;
                model.KanJnm = string.Empty;
                model.KanjJyus1 = string.Empty;
                model.KanjJyus2 = string.Empty;
                model.KanjTel = string.Empty;
                model.KanjFax = string.Empty;
                model.KanjKeiNo = string.Empty;
                model.KanjMail = string.Empty;
                model.KanDmhflg = 0;
                model.DanTaNm = input.Organization;
                model.DanTaKana = string.Empty;
                model.IkMapCdSeq = 0;
                model.IkNm = string.Empty;
                model.HaiScdSeq = 0;
                model.HaiSnm = string.Empty;
                model.HaiSjyus1 = string.Empty;
                model.HaiSjyus2 = string.Empty;
                model.HaiSkouKcdSeq = 0;
                model.HaiSbinCdSeq = 0;
                model.HaiSsetTime = string.Empty;
                model.TouCdSeq = 0;
                model.TouNm = string.Empty;
                model.TouJyusyo1 = string.Empty;
                model.TouJyusyo2 = string.Empty;
                model.TouKouKcdSeq = 0;
                model.TouBinCdSeq = 0;
                model.TouSetTime = string.Empty;
                model.AreaMapSeq = 0;
                model.AreaNm = string.Empty;
                model.HasMapCdSeq = 0;
                model.HasNm = string.Empty;
                model.JyoKyakuCdSeq = input.CodeKb.JyoKyakuCdSeq;
                model.JyoSyaJin = 0;
                model.PlusJin = 0;
                model.DrvJin = (short)input.ListItems.Sum(_ => int.Parse(_.DriverCount));
                model.GuiSu = (short)input.ListItems.Sum(_ => int.Parse(_.GuiderCount));
                model.OthJinKbn1 = 0;
                model.OthJin1 = 0;
                model.OthJinKbn2 = 0;
                model.OthJin2 = 0;
                model.Kskbn = 1;
                model.KhinKbn = 1;
                model.HaiSkbn = 1;
                model.GuiWnin = 0;
                model.NippoKbn = 1;
                model.YouKbn = 1;
                model.UkeJyKbnCd = 99;
                model.SijJoKbn1 = 99;
                model.SijJoKbn2 = 99;
                model.SijJoKbn3 = 99;
                model.SijJoKbn4 = 99;
                model.SijJoKbn5 = 99;
                model.RotCdSeq = 0;
                model.ZenHaFlg = 0;
                model.KhakFlg = 0;
                model.UnkoJkbn = 5;
                model.SyukoYmd = fromDate;
                model.SyuKoTime = fromTime;
                model.KikYmd = toDate;
                model.KikTime = toTime;
                model.DispKikYmd = toDate;
                model.DispKikTime = toTime;
                model.BikoTblSeq = 0;
                model.BikoNm = input.Note;
                model.UnsoOutputYmd = string.Empty;
                model.UnsoAllKiro = 0;
                model.UnsoJitKiro = 0;
                model.UnsoAllTim = string.Empty;
                model.UnsoJitTim = string.Empty;
                model.UnsoShinSoTime = string.Empty;
                model.UnsoChangeFlg = 0;
                model.UnsoSpecialFlg = 0;
                model.UnsoSanSyuEigCdSeq = 0;
                model.UnsoUnkStaTime = string.Empty;
                model.UnsoUnkEndTime = string.Empty;
                model.SinSoRyokin = 0;
                model.ChangeRyokin = 0;
                model.SpecialRyokin = 1;
                model.WaribikiKbn = 0;
                model.UnsoNenKeiyakuFlg = 1;
                model.UpperLimitFare = 0;
                model.LowerLimitFare = 0;
                model.UpperLimitFee = 0;
                model.LowerLimitFee = 0;
                model.OtherSystemCoperateCode = string.Empty;
                model.CustomItems1 = string.Empty;
                model.CustomItems2 = string.Empty;
                model.CustomItems3 = string.Empty;
                model.CustomItems4 = string.Empty;
                model.CustomItems5 = string.Empty;
                model.CustomItems6 = string.Empty;
                model.CustomItems7 = string.Empty;
                model.CustomItems8 = string.Empty;
                model.CustomItems9 = string.Empty;
                model.CustomItems10 = string.Empty;
                model.CustomItems11 = string.Empty;
                model.CustomItems12 = string.Empty;
                model.CustomItems13 = string.Empty;
                model.CustomItems14 = string.Empty;
                model.CustomItems15 = string.Empty;
                model.CustomItems16 = string.Empty;
                model.CustomItems17 = string.Empty;
                model.CustomItems18 = string.Empty;
                model.CustomItems19 = string.Empty;
                model.CustomItems20 = string.Empty;
                model.SiyoKbn = 1;
                model.UpdYmd = CurrentDate;
                model.UpdTime = CurrentTime;
                model.UpdSyainCd = SyainCdSeq;
                model.UpdPrgId = Common.UpdPrgId;
                _context.TkdUnkobi.Add(model);
            }

            private List<(short, int, int)> HandleInsertYyksyu(TkdYyksho data, ReservationMobileData input, string currentDate, string currentTime, int SyainCdSeq)
            {
                List<(short, int, int)> listSyaSyuRen = new List<(short, int, int)>();

                foreach (var item in input.ListItems)
                {
                    TkdYykSyu model = new TkdYykSyu();
                    model.UkeNo = data.UkeNo;
                    model.UnkRen = 1;
                    var maxSyaSyuRen = 0;
                    if (_context.TkdYykSyu.Any(_ => _.UkeNo == model.UkeNo && _.UnkRen == model.UnkRen))
                    {
                        maxSyaSyuRen = _context.TkdYykSyu.Where(_ => _.UkeNo == model.UkeNo && _.UnkRen == model.UnkRen).Max(_ => _.SyaSyuRen);
                    }
                    model.SyaSyuRen = (short)(maxSyaSyuRen + 1);
                    model.HenKai = 0;
                    model.SyaSyuCdSeq = item.SyaSyu.SyaSyuCdSeq;
                    model.KataKbn = item.SyaSyu.KataKbn;
                    model.SyaSyuDai = short.Parse(item.BusCount);
                    model.SyaSyuTan = 0;
                    model.SyaRyoUnc = model.SyaSyuDai * model.SyaSyuTan;
                    model.DriverNum = byte.Parse(item.DriverCount);
                    model.UnitBusPrice = 0;
                    model.UnitBusFee = 0;
                    model.GuiderNum = byte.Parse(item.GuiderCount);
                    model.UnitGuiderPrice = 0;
                    model.UnitGuiderFee = model.GuiderNum * model.UnitGuiderPrice;
                    model.SiyoKbn = 1;
                    model.UpdYmd = currentDate;
                    model.UpdTime = currentTime;
                    model.UpdSyainCd = SyainCdSeq;
                    model.UpdPrgId = Common.UpdPrgId;
                    _context.TkdYykSyu.Add(model);
                    _context.SaveChanges();
                    listSyaSyuRen.Add((model.SyaSyuRen, model.SyaSyuTan, model.SyaSyuCdSeq));
                }
                return listSyaSyuRen;
            }

            private void HandleInsertMishum(TkdYyksho yyksho, ReservationMobileData input, string currentDate, string currentTime, int SyainCdSeq)
            {
                var guider = input.ListItems.Sum(_ => int.Parse(_.GuiderCount));
                TkdMishum model1 = new TkdMishum();
                model1.UkeNo = yyksho.UkeNo;
                model1.MisyuRen = 1;
                model1.HenKai = 0;
                model1.SeiFutSyu = 1;
                model1.UriGakKin = 0;
                model1.SyaRyoSyo = 0;
                model1.SyaRyoTes = 0;
                model1.SeiKin = 0;
                model1.NyuKinRui = 0;
                model1.CouKesRui = 0;
                model1.FutuUnkRen = 0;
                model1.FutTumRen = 0;
                model1.SiyoKbn = 1;
                model1.UpdYmd = currentDate;
                model1.UpdTime = currentTime;
                model1.UpdSyainCd = SyainCdSeq;
                model1.UpdPrgId = Common.UpdPrgId;
                _context.TkdMishum.Add(model1);
                if (guider > 0)
                {
                    TkdMishum model5 = new TkdMishum();
                    model5.UkeNo = yyksho.UkeNo;
                    model5.MisyuRen = 2;
                    model5.HenKai = 0;
                    model5.SeiFutSyu = 5;
                    model5.UriGakKin = 0;
                    model5.SyaRyoSyo = 0;
                    model5.SyaRyoTes = 0;
                    model5.SeiKin = 0;
                    model5.NyuKinRui = 0;
                    model5.CouKesRui = 0;
                    model5.FutuUnkRen = 0;
                    model5.FutTumRen = 0;
                    model5.SiyoKbn = 1;
                    model5.UpdYmd = currentDate;
                    model5.UpdTime = currentTime;
                    model5.UpdSyainCd = SyainCdSeq;
                    model5.UpdPrgId = Common.UpdPrgId;
                    _context.TkdMishum.Add(model5);
                }
            }

            private void HandleUpdateHaisha(TkdYyksho yyksho, ReservationMobileData input, List<(short, int, int)> listSyaSyu, string currentDate, string currentTime, int SyainCdSeq, 
                HenSyaData hensya, int SyaRyoCdSeq)
            {
                if(yyksho.YoyaSyu == 1)
                {
                    string fromDate = input.DispatchDate.ToString(DateTimeFormat.yyyyMMdd);
                    string fromTime = input.DispatchTime.Replace(":", string.Empty);
                    string toDate = input.DispatchDate.ToString(DateTimeFormat.yyyyMMdd);
                    string toTime = input.DispatchTime.Replace(":", string.Empty);
                    var list = input.ListItems.OrderByDescending(_ => _.SyaSyuCd).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        var syaSyu = listSyaSyu.FirstOrDefault(_ => _.Item3 == list[i].SyaSyuCdSeq);
                        for (int j = 0; j < int.Parse(list[i].BusCount); j++)
                        {
                            TkdHaisha model = new TkdHaisha();
                            model.UkeNo = yyksho.UkeNo;
                            model.UnkRen = 1;
                            model.SyaSyuRen = syaSyu.Item1;
                            var maxTeiDanNo = 0;
                            if(_context.TkdHaisha.Any(_ => _.UkeNo == model.UkeNo && _.UnkRen == model.UnkRen))
                            {
                                maxTeiDanNo = _context.TkdHaisha.Where(_ => _.UkeNo == model.UkeNo && _.UnkRen == model.UnkRen).Max(_ => _.TeiDanNo);
                            }
                            model.TeiDanNo = (short)(maxTeiDanNo + 1);
                            model.BunkRen = 1;
                            model.HenKai = 0;
                            model.GoSya = model.TeiDanNo.ToString().PadLeft(2, '0');
                            model.GoSyaJyn = model.TeiDanNo;
                            model.BunKsyuJyn = 0;
                            model.SyuEigCdSeq = hensya?.EigyoCdSeq ?? 0;
                            model.KikEigSeq = hensya?.EigyoCdSeq ?? 0;
                            model.HaiSsryCdSeq = SyaRyoCdSeq;
                            model.KssyaRseq = SyaRyoCdSeq;
                            model.DanTaNm2 = string.Empty;
                            model.IkMapCdSeq = 0;
                            model.IkNm = string.Empty;
                            model.SyuKoYmd = fromDate;
                            model.SyuKoTime = fromTime;
                            model.SyuPaTime = fromTime;
                            model.HaiSymd = fromDate;
                            model.HaiStime = fromTime;
                            model.HaiScdSeq = 0;
                            model.HaiSnm = string.Empty;
                            model.HaiSjyus1 = string.Empty;
                            model.HaiSjyus2 = string.Empty;
                            model.HaiSkigou = string.Empty;
                            model.HaiSkouKcdSeq = 0;
                            model.HaiSbinCdSeq = 0;
                            model.HaiSsetTime = string.Empty;
                            model.KikYmd = toDate;
                            model.KikTime = toTime;
                            model.TouYmd = toDate;
                            model.TouChTime = toTime;
                            model.TouCdSeq = 0;
                            model.TouNm = string.Empty;
                            model.TouJyusyo1 = string.Empty;
                            model.TouJyusyo2 = string.Empty;
                            model.TouKigou = string.Empty;
                            model.TouKouKcdSeq = 0;
                            model.TouBinCdSeq = 0;
                            model.TouSetTime = string.Empty;
                            model.JyoSyaJin = 0;
                            model.PlusJin = 0;
                            model.DrvJin = (short)(short.Parse(list[i].DriverCount) / short.Parse(list[i].BusCount));
                            model.GuiSu = (short)(short.Parse(list[i].GuiderCount) / short.Parse(list[i].BusCount));
                            model.OthJinKbn1 = 99;
                            model.OthJin1 = 0;
                            model.OthJinKbn2 = 99;
                            model.OthJin2 = 0;
                            model.Kskbn = (byte)(model.HaiSsryCdSeq != 0 ? 2 : 1);
                            model.KhinKbn = 1;
                            model.HaiSkbn = (byte)(model.HaiSsryCdSeq != 0 ? 2 : 1);
                            model.HaiIkbn = 1;
                            model.GuiWnin = 0;
                            model.NippoKbn = 0;
                            model.YouTblSeq = 0;
                            model.YouKataKbn = 9;
                            model.SyaRyoUnc = 0;
                            model.SyaRyoSyo = 0;
                            model.SyaRyoTes = 0;
                            model.YoushaUnc = 0;
                            model.YoushaSyo = 0;
                            model.YoushaTes = 0;
                            model.PlatNo = string.Empty;
                            model.UkeJyKbnCd = 99;
                            model.SijJoKbn1 = 99;
                            model.SijJoKbn2 = 99;
                            model.SijJoKbn3 = 99;
                            model.SijJoKbn4 = 99;
                            model.SijJoKbn5 = 99;
                            model.RotCdSeq = 0;
                            model.BikoTblSeq = 0;
                            model.HaiCom = string.Empty;
                            model.BikoNm = string.Empty;
                            model.CustomItems1 = string.Empty;
                            model.CustomItems2 = string.Empty;
                            model.CustomItems3 = string.Empty;
                            model.CustomItems4 = string.Empty;
                            model.CustomItems5 = string.Empty;
                            model.CustomItems6 = string.Empty;
                            model.CustomItems7 = string.Empty;
                            model.CustomItems8 = string.Empty;
                            model.CustomItems9 = string.Empty;
                            model.CustomItems10 = string.Empty;
                            model.CustomItems11 = string.Empty;
                            model.CustomItems12 = string.Empty;
                            model.CustomItems13 = string.Empty;
                            model.CustomItems14 = string.Empty;
                            model.CustomItems15 = string.Empty;
                            model.CustomItems16 = string.Empty;
                            model.CustomItems17 = string.Empty;
                            model.CustomItems18 = string.Empty;
                            model.CustomItems19 = string.Empty;
                            model.CustomItems20 = string.Empty;
                            model.SiyoKbn = 1;
                            model.UpdYmd = currentDate;
                            model.UpdTime = currentTime;
                            model.UpdSyainCd = SyainCdSeq;
                            model.UpdPrgId = Common.UpdPrgId;
                            _context.TkdHaisha.Add(model);
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
