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

namespace HassyaAllrightCloud.Application.Busschedule.Commands
{
    public class PostBusscheduleCommand : IRequest<Unit>
    {
        public BusscheduleData ScheduleFormData;
        public class Handler : IRequestHandler<PostBusscheduleCommand, Unit>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(PostBusscheduleCommand request, CancellationToken cancellationToken)
            {
                string ukeNo = "-1";
                TkdYyksho yyksho = CollectDataYyksho(request.ScheduleFormData);
                TkdUnkobi tkdunkobi = CollectDataUnkobi(request.ScheduleFormData);
                List<TkdYykSyu> listYykSyu = CollectDataYykSyu(request.ScheduleFormData);
                List<TkdHaisha> listHaisha = CollectDataHaisha(request.ScheduleFormData, listYykSyu);
                var hodata = request.ScheduleFormData.Itembus.Select(t => t.SyaSyuCdSeq).Distinct();
                foreach (var vehicleTypeRow in hodata)
                {
                    var a = listYykSyu.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).Select(t => t.SyaSyuRen).ToList();
                    int countbusdriver = listHaisha.Where(t => a.Contains(t.SyaSyuRen)).Sum(t => t.DrvJin);
                    int countbusgui = listHaisha.Where(t => a.Contains(t.SyaSyuRen)).Sum(t => t.GuiSu);
                    listYykSyu.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).First().DriverNum = (byte)countbusdriver;
                    listYykSyu.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).First().GuiderNum = (byte)countbusgui;
                }
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _context.TkdYyksho.AddAsync(yyksho);
                        await _context.SaveChangesAsync();
                        ukeNo = yyksho.UkeNo;

                        tkdunkobi.UkeNo = ukeNo;
                        tkdunkobi.Kskbn = yyksho.Kskbn;
                        await _context.TkdUnkobi.AddAsync(tkdunkobi);
                        foreach (TkdYykSyu item in listYykSyu)
                        {
                            item.UkeNo = ukeNo;
                            await _context.TkdYykSyu.AddAsync(item);
                        }

                        foreach (TkdHaisha item in listHaisha)
                        {
                            item.UkeNo = ukeNo;
                            await _context.TkdHaisha.AddAsync(item);
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
                return Unit.Value;
            }

            private List<TkdHaisha> CollectDataHaisha(BusscheduleData scheduleFormData, List<TkdYykSyu> listYykSyu)
            {
                List<TkdHaisha> listTkdHaisha = new List<TkdHaisha> { };
                short i = 1;
                foreach (var vehicleTypeRow in scheduleFormData.Itembus.OrderBy(t => t.TimeStartString))
                {
                    TkdHaisha tkdhaisha = new TkdHaisha();
                    tkdhaisha.UnkRen = 1;
                    tkdhaisha.SyaSyuRen = listYykSyu.Where(t => t.SyaSyuCdSeq == vehicleTypeRow.SyaSyuCdSeq).First().SyaSyuRen;
                    tkdhaisha.TeiDanNo = i;
                    tkdhaisha.BunkRen = 1;
                    tkdhaisha.HenKai = 0;
                    tkdhaisha.GoSya = i.ToString("D2");
                    tkdhaisha.GoSyaJyn = i;
                    tkdhaisha.BunKsyuJyn = 0;
                    tkdhaisha.SyuEigCdSeq = vehicleTypeRow.SyuEigCdSeq;
                    tkdhaisha.KikEigSeq = vehicleTypeRow.KikEigSeq;
                    tkdhaisha.HaiSsryCdSeq = int.Parse(vehicleTypeRow.BusLine);
                    tkdhaisha.KssyaRseq = int.Parse(vehicleTypeRow.BusLine);
                    tkdhaisha.DanTaNm2 = "";
                    tkdhaisha.IkMapCdSeq = 0;
                    tkdhaisha.IkNm = "";
                    tkdhaisha.SyuKoYmd = vehicleTypeRow.StartDate;
                    tkdhaisha.SyuKoTime = vehicleTypeRow.TimeStart.ToString("D4");
                    tkdhaisha.SyuPaTime = vehicleTypeRow.TimeStart.ToString("D4");
                    tkdhaisha.HaiSymd = vehicleTypeRow.StartDate;
                    tkdhaisha.HaiStime = vehicleTypeRow.TimeStart.ToString("D4");
                    tkdhaisha.HaiScdSeq = 0;
                    tkdhaisha.HaiSnm = "";
                    tkdhaisha.HaiSjyus1 = "";
                    tkdhaisha.HaiSjyus2 = "";
                    tkdhaisha.HaiSkigou = "";
                    tkdhaisha.HaiSkouKcdSeq = 0;
                    tkdhaisha.HaiSbinCdSeq = 0;
                    tkdhaisha.HaiSsetTime = "";
                    tkdhaisha.KikYmd = vehicleTypeRow.EndDate;
                    tkdhaisha.KikTime = vehicleTypeRow.TimeEnd.ToString("D4");
                    tkdhaisha.TouYmd = vehicleTypeRow.EndDate;
                    tkdhaisha.TouChTime = vehicleTypeRow.TimeEnd.ToString("D4");
                    tkdhaisha.TouCdSeq = 0;
                    tkdhaisha.TouNm = "";
                    tkdhaisha.TouJyusyo1 = "";
                    tkdhaisha.TouJyusyo2 = "";
                    tkdhaisha.TouKigou = "";
                    tkdhaisha.TouKouKcdSeq = 0;
                    tkdhaisha.TouBinCdSeq = 0;
                    tkdhaisha.TouSetTime = "";
                    tkdhaisha.JyoSyaJin = 0;
                    tkdhaisha.PlusJin = 0;
                    tkdhaisha.DrvJin = 0;
                    tkdhaisha.GuiSu = 0;
                    tkdhaisha.OthJinKbn1 = 99;
                    tkdhaisha.OthJin1 = 0;
                    tkdhaisha.OthJinKbn2 = 99;
                    tkdhaisha.OthJin2 = 0;
                    tkdhaisha.Kskbn = 2;
                    tkdhaisha.KhinKbn = 1;
                    tkdhaisha.HaiSkbn = 1;
                    tkdhaisha.HaiIkbn = 1;
                    tkdhaisha.GuiWnin = 0;
                    tkdhaisha.NippoKbn = 1;
                    tkdhaisha.YouTblSeq = 0;
                    tkdhaisha.YouKataKbn = 9;
                    tkdhaisha.SyaRyoUnc = 0;
                    tkdhaisha.SyaRyoSyo = 0;
                    tkdhaisha.SyaRyoTes = 0;
                    tkdhaisha.YoushaUnc = 0;
                    tkdhaisha.YoushaSyo = 0;
                    tkdhaisha.YoushaTes = 0;
                    tkdhaisha.PlatNo = "";
                    tkdhaisha.UkeJyKbnCd = 99;
                    tkdhaisha.SijJoKbn1 = 99;
                    tkdhaisha.SijJoKbn2 = 99;
                    tkdhaisha.SijJoKbn3 = 99;
                    tkdhaisha.SijJoKbn4 = 99;
                    tkdhaisha.SijJoKbn5 = 99;
                    tkdhaisha.RotCdSeq = 0;
                    tkdhaisha.BikoTblSeq = 0;
                    tkdhaisha.HaiCom = "";
                    tkdhaisha.SiyoKbn = 1;
                    tkdhaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdhaisha.UpdTime = DateTime.Now.ToString("HHmmss");
                    tkdhaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdhaisha.UpdPrgId = Common.UpdPrgId;
                    listTkdHaisha.Add(tkdhaisha);
                    i++;
                }
                int x = listTkdHaisha.Count;
                int rowdriver = 0;
                for (int j = 0; j < scheduleFormData.BusdriverNum; j++)
                {

                    if (rowdriver == x)
                    {
                        rowdriver = 0;
                        listTkdHaisha[rowdriver].DrvJin++;
                    }
                    else if (j > x)
                    {
                        listTkdHaisha[rowdriver].DrvJin++;
                    }
                    else
                    {
                        listTkdHaisha[j].DrvJin++;
                    }
                    rowdriver++;
                }

                int rowguide = 0;
                for (int j = 0; j < scheduleFormData.BusGuideNum; j++)
                {

                    if (rowguide == x)
                    {
                        rowguide = 0;
                        listTkdHaisha[rowguide].GuiSu++;
                    }
                    else if (j > x)
                    {
                        listTkdHaisha[rowguide].GuiSu++;
                    }
                    else
                    {
                        listTkdHaisha[j].GuiSu++;
                    }
                    rowguide++;
                }
                return listTkdHaisha;

            }

            private List<TkdYykSyu> CollectDataYykSyu(BusscheduleData scheduleFormData)
            {
                List<TkdYykSyu> listTkdYykSyu = new List<TkdYykSyu> { };
                var hodata = scheduleFormData.Itembus.Select(t => t.SyaSyuCdSeq).Distinct();
                short i = 1;
                foreach (var vehicleTypeRow in hodata)
                {
                    int countofbuscate = scheduleFormData.Itembus.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).Count();
                    TkdYykSyu yyksyu = new TkdYykSyu();
                    yyksyu.UnkRen = 1;
                    yyksyu.SyaSyuRen = i;
                    yyksyu.HenKai = 0;
                    yyksyu.SyaSyuCdSeq = vehicleTypeRow;
                    yyksyu.KataKbn = (byte)scheduleFormData.Itembus.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).First().YykSyu_KataKbn;
                    yyksyu.SyaSyuDai = 0;
                    yyksyu.SyaSyuTan = 0;
                    yyksyu.SyaRyoUnc = 0;
                    yyksyu.DriverNum = 0;
                    yyksyu.UnitBusPrice = 0;
                    yyksyu.UnitBusFee = 0;
                    yyksyu.GuiderNum = 0;
                    yyksyu.UnitGuiderPrice = 0;
                    yyksyu.UnitGuiderFee = 0;
                    yyksyu.SiyoKbn = 1;
                    yyksyu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    yyksyu.UpdTime = DateTime.Now.ToString("HHmmss");
                    yyksyu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    yyksyu.UpdPrgId = Common.UpdPrgId;
                    listTkdYykSyu.Add(yyksyu);
                    i++;
                }

                return listTkdYykSyu;
            }

            private TkdUnkobi CollectDataUnkobi(BusscheduleData bookingFormData)
            {

                TkmKasSet tkmKasSet = this._context.TkmKasSet.Where(t => t.CompanyCdSeq == bookingFormData.CompanyID).FirstOrDefault();
                short UriKbn = tkmKasSet.UriKbn;
                TkdUnkobi tkdunkobi = new TkdUnkobi();
                tkdunkobi.UnkRen = 1;
                tkdunkobi.HenKai = 0;
                tkdunkobi.HaiSymd = bookingFormData.Itembus.Min(t => t.TimeStartString).ToString().Substring(0, 8);
                tkdunkobi.HaiStime = bookingFormData.Itembus.Min(t => t.TimeStartString).ToString().Substring(8, 4);
                tkdunkobi.TouYmd = bookingFormData.Itembus.Max(t => t.TimeEndString).ToString().Substring(0, 8);
                tkdunkobi.TouChTime = bookingFormData.Itembus.Max(t => t.TimeEndString).ToString().Substring(8, 4);
                tkdunkobi.SyuPaTime = tkdunkobi.HaiStime; // 出発時間

                // 売上年月日
                if (UriKbn == 1)
                {
                    tkdunkobi.UriYmd = tkdunkobi.HaiSymd; // 配車日時
                }
                else if (UriKbn == 2)
                {
                    tkdunkobi.UriYmd = tkdunkobi.TouYmd; // 終日予定
                }

                tkdunkobi.KanJnm = "";
                tkdunkobi.KanjJyus1 = "";
                tkdunkobi.KanjJyus2 = "";
                tkdunkobi.KanjTel = "";
                tkdunkobi.KanjFax = "";
                tkdunkobi.KanjKeiNo = "";
                tkdunkobi.KanjMail = "";
                tkdunkobi.KanDmhflg = 0;
                tkdunkobi.DanTaNm = bookingFormData.BookingName; // 団体名
                tkdunkobi.DanTaKana = bookingFormData.BookingName;
                tkdunkobi.IkMapCdSeq = 0;
                tkdunkobi.IkNm = "";
                tkdunkobi.HaiScdSeq = 0;
                tkdunkobi.HaiSnm = "";
                tkdunkobi.HaiSjyus1 = "";
                tkdunkobi.HaiSjyus2 = "";
                tkdunkobi.HaiSkouKcdSeq = 0;
                tkdunkobi.HaiSbinCdSeq = 0;
                tkdunkobi.HaiSsetTime = "";
                tkdunkobi.TouCdSeq = 0;
                tkdunkobi.TouNm = "";
                tkdunkobi.TouJyusyo1 = "";
                tkdunkobi.TouJyusyo2 = "";
                tkdunkobi.TouKouKcdSeq = 0;
                tkdunkobi.TouBinCdSeq = 0;
                tkdunkobi.TouSetTime = "";
                tkdunkobi.AreaMapSeq = 0;
                tkdunkobi.AreaNm = "";
                tkdunkobi.HasMapCdSeq = 0;
                tkdunkobi.HasNm = "";
                tkdunkobi.JyoKyakuCdSeq = 0;
                tkdunkobi.JyoSyaJin = 0;
                tkdunkobi.PlusJin = 0;
                tkdunkobi.DrvJin = (short)bookingFormData.BusdriverNum; // 運転手数
                tkdunkobi.GuiSu = (short)bookingFormData.BusGuideNum; // ガイド数
                tkdunkobi.OthJinKbn1 = 99;
                tkdunkobi.OthJin1 = 0;
                tkdunkobi.OthJinKbn2 = 99;
                tkdunkobi.OthJin2 = 0;
                tkdunkobi.Kskbn = 2; // 仮車区分 1:未仮車 2:仮車済 3:一部済
                tkdunkobi.KhinKbn = 1;
                tkdunkobi.HaiSkbn = 1;
                tkdunkobi.HaiIkbn = 1;
                tkdunkobi.GuiWnin = 0;
                tkdunkobi.NippoKbn = 1;
                tkdunkobi.YouKbn = 1;
                tkdunkobi.UkeJyKbnCd = 99;
                tkdunkobi.SijJoKbn1 = 99;
                tkdunkobi.SijJoKbn2 = 99;
                tkdunkobi.SijJoKbn3 = 99;
                tkdunkobi.SijJoKbn4 = 99;
                tkdunkobi.SijJoKbn5 = 99;
                tkdunkobi.RotCdSeq = 0;
                tkdunkobi.ZenHaFlg = 0;
                tkdunkobi.KhakFlg = 0;
                tkdunkobi.UnkoJkbn = 5;
                tkdunkobi.SyuKoTime = tkdunkobi.HaiStime;
                tkdunkobi.KikTime = tkdunkobi.TouChTime;
                tkdunkobi.BikoTblSeq = 0;
                tkdunkobi.SiyoKbn = 1;
                tkdunkobi.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                tkdunkobi.UpdTime = DateTime.Now.ToString("HHmmss");
                tkdunkobi.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                tkdunkobi.UpdPrgId = Common.UpdPrgId;
                return tkdunkobi;
            }

            private TkdYyksho CollectDataYyksho(BusscheduleData scheduleFormData)
            {
                TkdYyksho yyksho = new TkdYyksho();
                yyksho.HenKai = 0;
                yyksho.UkeYmd = DateTime.Now.ToString("yyyyMMdd");
                yyksho.YoyaSyu = 1; //1:予約 2:キャンセル
                yyksho.YoyaKbnSeq = 1;
                yyksho.KikakuNo = 0;
                yyksho.TourCd = "0";
                yyksho.KasTourCdSeq = scheduleFormData.BranchID;
                yyksho.UkeEigCdSeq = new ClaimModel().EigyoCdSeq;
                yyksho.SeiEigCdSeq = new ClaimModel().EigyoCdSeq;
                yyksho.IraEigCdSeq = new ClaimModel().EigyoCdSeq;
                yyksho.EigTanCdSeq = new ClaimModel().SyainCdSeq;
                yyksho.InTanCdSeq = 0;
                yyksho.YoyaNm = scheduleFormData.BookingName;
                yyksho.YoyaKana = scheduleFormData.BookingName;
                yyksho.TokuiSeq = scheduleFormData.Customerlst.TokuiSeq;
                yyksho.SitenCdSeq = scheduleFormData.Customerlst.SitenCdSeq;
                yyksho.SirCdSeq = scheduleFormData.Customerlst.SitenCd;
                yyksho.SirSitenCdSeq = scheduleFormData.Customerlst.SitenCdSeq;
                yyksho.TokuiTel = "";
                yyksho.TokuiTanNm = "";
                yyksho.TokuiFax = "";
                yyksho.TokuiMail = "";
                yyksho.UntKin = 0;
                yyksho.ZeiKbn = 1;
                yyksho.Zeiritsu = 10;
                yyksho.ZeiRui = 0;
                yyksho.TaxTypeforGuider = 3;
                yyksho.TaxGuider = 0;
                yyksho.TesuRitu = scheduleFormData.Customerlst.TesuRitu;
                yyksho.TesuRyoG = 0;
                yyksho.FeeGuiderRate = 0;
                yyksho.FeeGuider = 0;
                yyksho.SeiKyuKbnSeq = 0;
                yyksho.SeikYm = DateTime.Now.ToString("yyyyMM");
                yyksho.SeiTaiYmd = DateTime.Now.ToString("yyyyMMdd");
                yyksho.CanRit = 0;
                yyksho.CanUnc = 0;
                yyksho.CanZkbn = 3;
                yyksho.CanSyoR = 0;
                yyksho.CanSyoG = 0;
                yyksho.CanYmd = "";
                yyksho.CanTanSeq = 0;
                yyksho.CanRiy = "";
                yyksho.CanFuYmd = "";
                yyksho.CanFuTanSeq = 0;
                yyksho.CanFuRiy = "";
                yyksho.BikoTblSeq = 0;
                yyksho.Kskbn = 2;
                yyksho.KhinKbn = 1;
                yyksho.KaknKais = 0;
                yyksho.KaktYmd = "";
                yyksho.HaiSkbn = 1;
                yyksho.HaiIkbn = 1;
                yyksho.GuiWnin = 0;
                yyksho.NippoKbn = 1;
                yyksho.YouKbn = 1;
                yyksho.NyuKinKbn = 1;
                yyksho.NcouKbn = 1;
                yyksho.SihKbn = 1;
                yyksho.ScouKbn = 1;
                yyksho.SiyoKbn = 1;
                yyksho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                yyksho.UpdTime = DateTime.Now.ToString("HHmmss");
                yyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                yyksho.UpdPrgId = Common.UpdPrgId;
                return yyksho;
            }
        }
    }
}
