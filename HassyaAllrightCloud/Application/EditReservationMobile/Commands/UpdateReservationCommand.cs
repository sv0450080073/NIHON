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
    public class UpdateReservationCommand : IRequest<bool>
    {
        public ReservationMobileData Data { get; set; }
        public List<ReservationMobileChildItemData> ListDelete { get; set; }
        public int CompanyCdSeq { get; set; }
        public int TenantCdSeq { get; set; }
        public int SyainCdSeq { get; set; }

        public class Handler : IRequestHandler<UpdateReservationCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var currentDate = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);
                        var currentTime = DateTime.Now.ToString(DateTimeFormat.HHmmss);

                        var kasset = await _context.TkmKasSet.FirstOrDefaultAsync(_ => _.CompanyCdSeq == request.CompanyCdSeq);

                        List<short> listDelete = new List<short>();
                        var yyksho = HandleUpdateYyksho(request.TenantCdSeq, request.Data, currentDate, currentTime, request.SyainCdSeq);
                        if (yyksho == null) throw new Exception("yyksho is null");
                        var listSyaSyuRen = HandleUpdateYyksyu(request.Data, request.ListDelete, currentDate, currentTime, request.SyainCdSeq, listDelete);

                        HandleUpdateUnkobi(request.Data, currentDate, currentTime, request.SyainCdSeq, kasset?.UriKbn ?? 0);
                        HandleUpdateMishum(yyksho, request.Data, currentDate, currentTime, request.SyainCdSeq);
                        HandleUpdateHaisha(yyksho, request.Data, listSyaSyuRen, currentDate, currentTime, request.SyainCdSeq);

                        _context.SaveChanges();

                        HandleDeleteAfterUpdate(listDelete, yyksho);

                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            private TkdYyksho HandleUpdateYyksho(int TenantCdSeq, ReservationMobileData input, string currentDate, string currentTime, int SyainCdSeq)
            {
                var data = _context.TkdYyksho.FirstOrDefault(_ => _.UkeCd == input.UkeCd && _.TenantCdSeq == TenantCdSeq);
                if(data != null)
                {
                    MapYyksho(data, input, currentDate, currentTime, SyainCdSeq);
                }
                return data;
            }

            private void MapYyksho(TkdYyksho data, ReservationMobileData input, string currentDate, string currentTime, int SyainCdSeq)
            {
                data.HenKai = (short)(data.HenKai + 1);
                data.UkeYmd = currentDate;
                data.KikakuNo = 0;
                data.TourCd = "0";
                data.KasTourCdSeq = 0;
                data.InTanCdSeq = 0;
                data.YoyaNm = input.Organization;
                data.YoyaKana = string.Empty;
                data.TokuiSeq = input.Tokisk.TokuiSeq;
                data.SitenCdSeq = input.Tokisk.SitenCdSeq;
                data.SirCdSeq = input.Tokisk.TokuiSeq;
                data.SirSitenCdSeq = input.Tokisk.SitenCdSeq;
                int UntKin = 0;
                int GuitKin = 0;
                foreach(var item in input.ListItems)
                {
                    UntKin += (item.SyaSyuTan * int.Parse(item.BusCount));
                    GuitKin += (item.UnitGuiderPrice * int.Parse(item.GuiderCount));
                }
                data.UntKin = UntKin;
                data.GuitKin = GuitKin;
                data.ZeiRui = data.UntKin * data.Zeiritsu / 100;
                data.TaxGuider = data.GuitKin * data.Zeiritsu / 100;
                data.TesuRyoG = data.UntKin * data.TesuRitu / 100;
                data.FeeGuider = data.GuitKin * data.FeeGuiderRate / 100;
                data.BikoTblSeq = 0;
                data.Kskbn = 1;
                data.KhinKbn = 1;
                data.KaknKais = 0;
                data.HaiSkbn = 1;
                data.HaiIkbn = 1;
                data.GuiWnin = 0;
                data.NippoKbn = 1;
                data.YouKbn = 1;
                data.NyuKinKbn = 1;
                data.NcouKbn = 1;
                data.SihKbn = 1;
                data.ScouKbn = 1;
                data.UpdYmd = currentDate;
                data.UpdTime = currentTime;
                data.UpdSyainCd = SyainCdSeq;
                data.UpdPrgId = Common.UpdPrgId;
            }

            private void HandleUpdateUnkobi(ReservationMobileData input, string currentDate, string currentTime, int SyainCdSeq, byte UriKbn)
            {
                var unkobi = _context.TkdUnkobi.FirstOrDefault(_ => _.UkeNo == input.UkeNo && _.UnkRen == 1);
                if(unkobi != null)
                {
                    MapUnkobi(unkobi, input, currentDate, currentTime, SyainCdSeq, UriKbn);
                }
            }

            private void MapUnkobi(TkdUnkobi data, ReservationMobileData input, string currentDate, string currentTime, int SyainCdSeq, byte UriKbn)
            {
                string fromDate = input.DispatchDate.ToString(DateTimeFormat.yyyyMMdd);
                string fromTime = input.DispatchTime.Replace(":", string.Empty);
                string toDate = input.ArrivalDate.ToString(DateTimeFormat.yyyyMMdd);
                string toTime = input.ArrivalTime.Replace(":", string.Empty);
                data.HenKai = (short)(data.HenKai + 1);
                data.HaiSymd = fromDate;
                data.HaiStime = fromTime;
                data.TouYmd = toDate;
                data.TouChTime = toTime;
                data.DispTouYmd = toDate;
                data.DispTouChTime = toTime;
                data.SyuPaYmd = fromDate;
                data.SyuPaTime = fromTime;
                data.DispSyuPaYmd = fromDate;
                data.DispSyuPaTime = fromTime;
                data.UriYmd = UriKbn == 1 ? data.HaiSymd : data.TouYmd;
                data.DanTaNm = input.Organization;
                data.HaiSjyus1 = string.Empty;
                data.HaiSjyus2 = string.Empty;
                data.HaiSkouKcdSeq = 0;
                data.HaiSbinCdSeq = 0;
                data.TouJyusyo1 = string.Empty;
                data.TouJyusyo2 = string.Empty;
                data.TouKouKcdSeq = 0;
                data.TouBinCdSeq = 0;
                data.TouSetTime = string.Empty;
                data.AreaMapSeq = 0;
                data.AreaNm = string.Empty;
                data.HasMapCdSeq = 0;
                data.HasNm = string.Empty;
                data.JyoKyakuCdSeq = input.CodeKb.JyoKyakuCdSeq;
                data.DrvJin = (short)input.ListItems.Sum(_ => int.Parse(_.DriverCount));
                data.GuiSu = (short)input.ListItems.Sum(_ => int.Parse(_.GuiderCount));
                data.OthJinKbn1 = 0;
                data.OthJin1 = 0;
                data.OthJinKbn2 = 0;
                data.OthJin2 = 0;
                data.Kskbn = 1;
                data.KhinKbn = 1;
                data.HaiSkbn = 1;
                data.HaiIkbn = 1;
                data.GuiWnin = 0;
                data.NippoKbn = 1;
                data.YouKbn = 1;
                data.RotCdSeq = 0;
                data.SyukoYmd = fromDate;
                data.SyuKoTime = fromTime;
                data.KikYmd = toDate;
                data.KikTime = toTime;
                data.DispKikYmd = toDate;
                data.DispKikTime = toTime;
                data.BikoTblSeq = 0;
                data.BikoNm = input.Note;
                data.UpdYmd = currentDate;
                data.UpdTime = currentTime;
                data.UpdSyainCd = SyainCdSeq;
                data.UpdPrgId = Common.UpdPrgId;
            }

            private List<(short, int, int)> HandleUpdateYyksyu(ReservationMobileData input, List<ReservationMobileChildItemData> list, string currentDate, string currentTime, int SyainCdSeq,
                List<short> listDelete)
            {
                List<(short, int, int)> listSyaSyuRen = new List<(short, int, int)>();
                var listYyksyu = _context.TkdYykSyu.Where(_ => _.UkeNo == input.UkeNo && _.UnkRen == 1).ToList();
                foreach(var item in list)
                {
                    var data = listYyksyu.FirstOrDefault(_ => _.SyaSyuRen == item.SyaSyuRen);
                    if(data != null)
                    {
                        listDelete.Add(data.SyaSyuRen);
                        _context.TkdYykSyu.Remove(data);
                    }
                }

                foreach(var item in input.ListItems)
                {
                    var data = listYyksyu.FirstOrDefault(_ => _.SyaSyuRen == item.SyaSyuRen);
                    MappYyksyu(data, input, item, currentDate, currentTime, SyainCdSeq);
                    listSyaSyuRen.Add((data.SyaSyuRen, data.SyaSyuTan, data.SyaSyuCdSeq));
                }
                return listSyaSyuRen;
            }

            private void MappYyksyu(TkdYykSyu data, ReservationMobileData input, ReservationMobileChildItemData item, string currentDate, string currentTime, int SyainCdSeq)
            {
                if(data != null)
                {
                    data.HenKai = (short)(data.HenKai + 1);
                }
                else
                {
                    data.UkeNo = input.UkeNo;
                    var syaSyuRen = _context.TkdYykSyu.Where(_ => _.UkeNo == input.UkeNo && _.UnkRen == 1).Max(_ => _.SyaSyuRen) + 1;
                    data.SyaSyuRen = (short)syaSyuRen;
                    data.UnkRen = 1;
                    data.HenKai = 0;
                    data.SyaSyuTan = 0;
                    data.UnitBusPrice = 0;
                    data.UnitBusFee = 0;
                    data.UnitGuiderPrice = 0;
                    data.SiyoKbn = 1;
                }
                data.SyaSyuCdSeq = item.SyaSyu.SyaSyuCdSeq;
                data.KataKbn = item.SyaSyu.KataKbn;
                data.SyaSyuDai = short.Parse(item.BusCount);
                data.SyaRyoUnc = data.SyaSyuDai * data.SyaSyuTan;
                data.DriverNum = byte.Parse(item.DriverCount);
                data.GuiderNum = byte.Parse(item.GuiderCount);
                data.UnitGuiderFee = data.GuiderNum * data.UnitGuiderPrice;
                data.SiyoKbn = 1;
                data.UpdYmd = currentDate;
                data.UpdTime = currentTime;
                data.UpdSyainCd = SyainCdSeq;
                data.UpdPrgId = Common.UpdPrgId;
                _context.SaveChanges();
            }

            private void HandleUpdateMishum(TkdYyksho yyksho, ReservationMobileData input, string currentDate, string currentTime, int SyainCdSeq)
            {
                var mishum = _context.TkdMishum.Where(_ => _.UkeNo == input.UkeNo && (_.SeiFutSyu == 1 || _.SeiFutSyu == 5)).ToList();
                var guider = input.ListItems.Sum(_ => int.Parse(_.GuiderCount));
                var item5 = mishum.FirstOrDefault(_ => _.SeiFutSyu == 5);
                if (guider > 0)
                {
                    if(item5 != null)
                    {
                        _context.TkdMishum.Remove(item5);
                    }
                }
                else
                {
                    var item1 = mishum.FirstOrDefault(_ => _.SeiFutSyu == 1);
                    item1.HenKai = (short)(item1.HenKai + 1);
                    item1.UriGakKin = Convert.ToInt32(yyksho.UntKin);
                    item1.SyaRyoSyo = Convert.ToInt32(yyksho.ZeiRui);
                    item1.SyaRyoTes = Convert.ToInt32(yyksho.TesuRyoG);
                    item1.SeiKin = yyksho.ZeiKbn == 1 ? item1.UriGakKin + item1.SyaRyoSyo : item1.UriGakKin;
                    item1.SiyoKbn = 1;
                    item1.UpdYmd = currentDate;
                    item1.UpdTime = currentTime;
                    item1.UpdSyainCd = SyainCdSeq;
                    item1.UpdPrgId = Common.UpdPrgId;

                    if(item5 != null)
                    {
                        item5.HenKai = (short)(item5.HenKai + 1);
                    }
                    else
                    {
                        var misyuren = _context.TkdMishum.Where(_ => _.UkeNo == yyksho.UkeNo).Max(_ => _.MisyuRen) + 1;
                        item5 = new TkdMishum();
                        item5.MisyuRen = (short)misyuren;
                        item5.HenKai = 0;
                        item5.SeiFutSyu = 5;
                        item5.NyuKinRui = 0;
                        item5.CouKesRui = 0;
                        item5.FutuUnkRen = 0;
                        item5.FutTumRen = 0;
                    }
                    item5.UriGakKin = Convert.ToInt32(yyksho.GuitKin);
                    item5.SyaRyoSyo = Convert.ToInt32(yyksho.TaxGuider);
                    item5.SyaRyoTes = Convert.ToInt32(yyksho.FeeGuider);
                    item5.SeiKin = yyksho.ZeiKbn == 1 ? item5.UriGakKin + item5.SyaRyoSyo : item5.UriGakKin;
                    item5.SiyoKbn = 1;
                    item5.UpdYmd = currentDate;
                    item5.UpdTime = currentTime;
                    item5.UpdSyainCd = SyainCdSeq;
                    item5.UpdPrgId = Common.UpdPrgId;
                }
            }

            private void HandleUpdateHaisha(TkdYyksho yyksho, ReservationMobileData input, List<(short, int, int)> listSyaSyu, string currentDate, string currentTime, int SyainCdSeq)
            {
                var haisha = _context.TkdHaisha.Where(_ => _.UkeNo == yyksho.UkeNo && _.UnkRen == 1).OrderBy(_ => _.TeiDanNo).ToList();
                short maxTeiDanNo = (short)(haisha.Max(_ => _.TeiDanNo) + 1);
                var sumBus = input.ListItems.Sum(_ => int.Parse(_.BusCount));
                if(haisha.Count > sumBus)
                {
                    haisha.RemoveRange(sumBus - 1, haisha.Count - sumBus);
                }

                string fromDate = input.DispatchDate.ToString(DateTimeFormat.yyyyMMdd);
                string fromTime = input.DispatchTime.Replace(":", string.Empty);
                string toDate = input.DispatchDate.ToString(DateTimeFormat.yyyyMMdd);
                string toTime = input.DispatchTime.Replace(":", string.Empty);
                input.ListItems = input.ListItems.OrderByDescending(_ => _.SyaSyuCd).ToList();
                for (int i = 0; i < input.ListItems.Count; i++)
                {
                    var syaSyu = listSyaSyu.FirstOrDefault(_ => _.Item3 == input.ListItems[i].SyaSyuCdSeq);
                    for(int j = 0; j < int.Parse(input.ListItems[i].BusCount); j++)
                    {
                        if (haisha.Count > i)
                        {
                            MapHaisha(haisha[i], yyksho, input.ListItems[i], syaSyu, fromDate, fromTime, toDate, toTime, currentDate, currentTime, SyainCdSeq, maxTeiDanNo);
                        }
                        else
                        {
                            MapHaisha(null, yyksho, input.ListItems[i], syaSyu, fromDate, fromTime, toDate, toTime, currentDate, currentTime, SyainCdSeq, maxTeiDanNo);
                        }
                        maxTeiDanNo++;
                    }
                    
                }
            }

            private void MapHaisha(TkdHaisha haisha, TkdYyksho yyksho, ReservationMobileChildItemData item, (short, int, int) syaSyu, 
                string fromDate, string fromTime, string toDate, string toTime, string currentDate, string currentTime, int SyainCdSeq, short teiDanNo)
            {
                if(haisha == null)
                {
                    haisha = new TkdHaisha();
                    haisha.UkeNo = yyksho.UkeNo;
                    haisha.UnkRen = 1;
                    haisha.TeiDanNo = teiDanNo;
                    haisha.BunkRen = 1;
                    haisha.HenKai = 0;
                    haisha.GoSya = haisha.TeiDanNo.ToString().PadLeft(2, '0');
                    haisha.GoSyaJyn = haisha.TeiDanNo;
                    haisha.SyuEigCdSeq = 0;
                    haisha.KikEigSeq = 0;
                    haisha.HaiSsryCdSeq = 0;
                    haisha.KssyaRseq = 0;
                    haisha.IkMapCdSeq = 0;
                    haisha.IkNm = string.Empty;
                    haisha.HaiScdSeq = 0;
                    haisha.HaiSnm = string.Empty;
                    haisha.TouCdSeq = 0;
                    haisha.CustomItems1 = string.Empty;
                    haisha.CustomItems2 = string.Empty;
                    haisha.CustomItems3 = string.Empty;
                    haisha.CustomItems4 = string.Empty;
                    haisha.CustomItems5 = string.Empty;
                    haisha.CustomItems6 = string.Empty;
                    haisha.CustomItems7 = string.Empty;
                    haisha.CustomItems8 = string.Empty;
                    haisha.CustomItems9 = string.Empty;
                    haisha.CustomItems10 = string.Empty;
                    haisha.CustomItems11 = string.Empty;
                    haisha.CustomItems12 = string.Empty;
                    haisha.CustomItems13 = string.Empty;
                    haisha.CustomItems14 = string.Empty;
                    haisha.CustomItems15 = string.Empty;
                    haisha.CustomItems16 = string.Empty;
                    haisha.CustomItems17 = string.Empty;
                    haisha.CustomItems18 = string.Empty;
                    haisha.CustomItems19 = string.Empty;
                    haisha.CustomItems20 = string.Empty;
                }
                else
                {
                    haisha.HenKai = (short)(haisha.HenKai + 1);
                }
                haisha.SyaSyuRen = syaSyu.Item1;
                haisha.BunKsyuJyn = 0;
                haisha.DanTaNm2 = string.Empty;
                haisha.SyuKoYmd = fromDate;
                haisha.SyuKoTime = fromTime;
                haisha.SyuPaTime = fromTime;
                haisha.HaiSymd = fromDate;
                haisha.HaiStime = fromTime;
                haisha.HaiSjyus1 = string.Empty;
                haisha.HaiSjyus2 = string.Empty;
                haisha.HaiSkigou = string.Empty;
                haisha.HaiSbinCdSeq = 0;
                haisha.HaiSsetTime = string.Empty;
                haisha.KikYmd = toDate;
                haisha.KikTime = toTime;
                haisha.TouYmd = toDate;
                haisha.TouChTime = toTime;
                haisha.TouNm = string.Empty;
                haisha.TouJyusyo1 = string.Empty;
                haisha.TouJyusyo2 = string.Empty;
                haisha.TouKigou = string.Empty;
                haisha.TouKouKcdSeq = 0;
                haisha.TouSetTime = string.Empty;
                haisha.JyoSyaJin = 0;
                haisha.PlusJin = 0;
                haisha.DrvJin = (short)(int.Parse(item.DriverCount) / int.Parse(item.BusCount));
                haisha.GuiSu = (short)(int.Parse(item.GuiderCount) / int.Parse(item.BusCount));
                haisha.OthJinKbn1 = 99;
                haisha.OthJin1 = 0;
                haisha.OthJinKbn2 = 99;
                haisha.OthJin2 = 0;
                haisha.Kskbn = (byte)(haisha.HaiSsryCdSeq != 0 ? 2 : 1);
                haisha.KhinKbn = 1;
                haisha.HaiSkbn = (byte)(haisha.HaiSsryCdSeq != 0 ? 2 : 1);
                haisha.HaiIkbn = 1;
                haisha.GuiWnin = 0;
                haisha.NippoKbn = 0;
                haisha.YouTblSeq = 0;
                haisha.YouKataKbn = 9;
                haisha.SyaRyoUnc = syaSyu.Item2;
                haisha.SyaRyoSyo = (int)(haisha.SyaRyoUnc * yyksho.Zeiritsu / 100);
                haisha.SyaRyoTes = (int)(haisha.SyaRyoUnc * yyksho.TesuRyoG / 100);
                haisha.YoushaUnc = 0;
                haisha.YoushaSyo = 0;
                haisha.YoushaTes = 0;
                haisha.PlatNo = string.Empty;
                haisha.UkeJyKbnCd = 99;
                haisha.SijJoKbn1 = 99;
                haisha.SijJoKbn2 = 99;
                haisha.SijJoKbn3 = 99;
                haisha.SijJoKbn4 = 99;
                haisha.SijJoKbn5 = 99;
                haisha.RotCdSeq = 0;
                haisha.BikoTblSeq = 0;
                haisha.HaiCom = string.Empty;
                haisha.BikoNm = string.Empty;
                haisha.SiyoKbn = 1;
                haisha.UpdYmd = currentDate;
                haisha.UpdTime = currentTime;
                haisha.UpdSyainCd = SyainCdSeq;
                haisha.UpdPrgId = Common.UpdPrgId;
            }

            private void HandleDeleteAfterUpdate(List<short> listDelete, TkdYyksho yyksho)
            {
                _context.TkdHaisha.RemoveRange(_context.TkdHaisha.Where(_ => _.UkeNo == yyksho.UkeNo && _.UnkRen == 1 && listDelete.Contains(_.SyaSyuRen)).AsEnumerable());
                _context.TkdBookingMaxMinFareFeeCalc.RemoveRange(_context.TkdBookingMaxMinFareFeeCalc.Where(_ => _.UkeNo == yyksho.UkeNo && _.UnkRen == 1 && listDelete.Contains(_.SyaSyuRen)).AsEnumerable());
                _context.TkdBookingMaxMinFareFeeCalcMeisai.RemoveRange(_context.TkdBookingMaxMinFareFeeCalcMeisai.Where(_ => _.UkeNo == yyksho.UkeNo && _.UnkRen == 1 && listDelete.Contains(_.SyaSyuRen)).AsEnumerable());
                _context.TkdKariei.RemoveRange(_context.TkdKariei.Where(_ => _.UkeNo == yyksho.UkeNo && _.UnkRen == 1 && listDelete.Contains(_.SyaSyuRen)).AsEnumerable());
            }
        }
    }
}
