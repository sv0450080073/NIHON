using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.IService;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Application.BookingInput.Commands
{
    public class InsertBookingInputCommand : IRequest<string>
    {
        private readonly BookingFormData _bookingData;
        private readonly (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) _vehiclesInfo;

        public InsertBookingInputCommand(BookingFormData bookingData, (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) vehiclesInfo)
        {
            _bookingData = bookingData ?? throw new ArgumentNullException(nameof(bookingData));
            _vehiclesInfo.Vehicles = vehiclesInfo.Vehicles ?? throw new ArgumentNullException(nameof(vehiclesInfo.Vehicles));
            _vehiclesInfo.VehiclesAssigned = vehiclesInfo.VehiclesAssigned ?? throw new ArgumentNullException(nameof(vehiclesInfo.VehiclesAssigned));
        }

        public class Handler : IRequestHandler<InsertBookingInputCommand, string>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<InsertBookingInputCommand> _logger;
            private readonly int _maxUkeCd;
            private readonly string _newUkeNo;

            public Handler(KobodbContext context, ILogger<InsertBookingInputCommand> logger, ITKD_YykshoListService yykshoService)
            {
                _context = context;
                _logger = logger;
                (_maxUkeCd, _newUkeNo) = yykshoService.GetNewUkeNo(new ClaimModel().TenantID).Result;
            }

            public async Task<string> Handle(InsertBookingInputCommand command, CancellationToken cancellationToken)
            {
                if(_maxUkeCd == int.MaxValue) // max value of ukenocd in integer type is 2147483647
                {
                    return Constants.ErrorMessage.UkenoCdIsFull;
                }

                TkdYyksho tkdYyksho = CollectDataYyksho(command._bookingData);
                TkdUnkobi tkdUnkobi = CollectDataUnkobi(command._bookingData);
                
                List<TkdYykSyu> listYykSyu = CollectDataYykSyu(command._bookingData);
                List<TkdMishum> listMishum = new List<TkdMishum>();
                List<TkdHaisha> listHaisha = new List<TkdHaisha>();
                List<TkdBookingMaxMinFareFeeCalc> listBookingMaxMinFareFeeCals = new List<TkdBookingMaxMinFareFeeCalc>();
                List<TkdBookingMaxMinFareFeeCalcMeisai> listBookingMaxMinFareFeeCalsMeisai = new List<TkdBookingMaxMinFareFeeCalcMeisai>();
                List<TkdKariei> listKariei = new List<TkdKariei>();

                listBookingMaxMinFareFeeCals = CollectDataBookingMaxMinFareFeeCals(command._bookingData);
                listBookingMaxMinFareFeeCalsMeisai = CollectDataBookingMaxMinFareFeeCalsDetails(command._bookingData);
                if (command._bookingData.BookingStatus.CodeKbn == Constants.BookingStatus)
                {
                    listMishum = CollectDataMishum(command._bookingData);
                    listHaisha = CollectDataHaisha(command, listMishum[0]);
                    listKariei = CollectDataKariei(command._bookingData, listHaisha);
                    // yyksho.Kskbn 仮車区分
                    // 1:未仮車 2:仮車済 3:一部済
                    int totalBookingWithAsignBus = 0;
                    foreach (TkdHaisha item in listHaisha)
                    {
                        if (item.HaiSsryCdSeq > 0)
                        {
                            totalBookingWithAsignBus++;
                        }
                    }
                    if (listHaisha.Count == totalBookingWithAsignBus)
                    {
                        tkdYyksho.Kskbn = 2;
                    }
                    else if (totalBookingWithAsignBus == 0)
                    {
                        tkdYyksho.Kskbn = 1;
                    }
                    else
                    {
                        tkdYyksho.Kskbn = 3;
                    }
                    tkdYyksho.YoyaSyu = 1;
                }
                if (command._bookingData.BookingStatus.CodeKbn == Constants.EstimateStatus) 
                {
                    tkdYyksho.YoyaSyu = 3;
                }
                //string ukeNo = "-1";

                //Add custom field to Unkobi
                if (command._bookingData.CustomData.Count() > 0)
                {
                    foreach (var fieldValue in command._bookingData.CustomData)
                    {
                        _context.Entry(tkdUnkobi).Property($"CustomItems{fieldValue.Key}").CurrentValue = fieldValue.Value;
                    }
                }
                using (var dbTran = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _context.TkdYyksho.AddAsync(tkdYyksho);
                        await _context.SaveChangesAsync();
                        //ukeNo = tkdYyksho.UkeNo; //get the Ukeno after inserted to db

                        //tkdUnkobi.UkeNo = ukeNo;
                        tkdUnkobi.Kskbn = tkdYyksho.Kskbn;
                        await _context.TkdUnkobi.AddAsync(tkdUnkobi);

                        foreach (TkdYykSyu item in listYykSyu)
                        {
                            //item.UkeNo = ukeNo;
                            await _context.TkdYykSyu.AddAsync(item);
                        }
                        if (command._bookingData.BookingStatus.CodeKbn == Constants.BookingStatus)
                        {
                            // foreach (TkdMishum item in listMishum)
                            // {
                            //     //item.UkeNo = ukeNo;
                            //     await _context.TkdMishum.AddAsync(item);
                            // }
                            await _context.TkdMishum.AddRangeAsync(listMishum);

                            // foreach (TkdHaisha item in listHaisha)
                            // {
                            //     //item.UkeNo = ukeNo;
                            //     await _context.TkdHaisha.AddAsync(item);
                            // }
                            await _context.TkdHaisha.AddRangeAsync(listHaisha);

                            await _context.TkdKariei.AddRangeAsync(listKariei);
                        }

                        // foreach (TkdBookingMaxMinFareFeeCalc item in listBookingMaxMinFareFeeCals)
                        // {
                        //     //item.UkeNo = ukeNo;
                        //     await _context.TkdBookingMaxMinFareFeeCalc.AddAsync(item);
                        // }
                        await _context.TkdBookingMaxMinFareFeeCalc.AddRangeAsync(listBookingMaxMinFareFeeCals);

                        // foreach (TkdBookingMaxMinFareFeeCalcMeisai item in listBookingMaxMinFareFeeCalsMeisai)
                        // {
                        //     //item.UkeNo = ukeNo;
                        //     await _context.TkdBookingMaxMinFareFeeCalcMeisai.AddAsync(item);
                        // }
                        await _context.TkdBookingMaxMinFareFeeCalcMeisai.AddRangeAsync(listBookingMaxMinFareFeeCalsMeisai);

                        await _context.SaveChangesAsync();
                        await dbTran.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        await dbTran.RollbackAsync();
                        return "-1";
                    }
                }

                return _newUkeNo;
            }

            private TkdYyksho CollectDataYyksho(BookingFormData bookingdata)
            {
                // #8091
                var minusTax = bookingdata.TaxTypeforBus.IdValue == 2 ? Convert.ToInt32(bookingdata.TaxBus) : 0;
                var minusGuiTax = bookingdata.TaxTypeforGuider.IdValue == 2 ? Convert.ToInt32(bookingdata.TaxGuider) : 0;
                TkdYyksho yyksho = new TkdYyksho()
                {
                    HenKai = 0,
                    UkeYmd = DateTime.Now.ToString("yyyyMMdd"),
                    YoyaSyu = 1, //1:予約 2:キャンセル
                    YoyaKbnSeq = (int)bookingdata.CurrentBookingType.YoyaKbnSeq,
                    KikakuNo = 0, // So ke hoach doanh nghiep
                    TourCd = "0", // Khi chon tour se phai nhap ma tour vd: SGN-NT
                    KasTourCdSeq = 0, // Kashikiri Tour id
                    UkeEigCdSeq = (int)bookingdata.SelectedSaleBranch.EigyoCdSeq,
                    SeiEigCdSeq = bookingdata.SelectedSaleBranch.EigyoCdSeq,
                    IraEigCdSeq = bookingdata.SelectedSaleBranch.EigyoCdSeq, // y chang SeiEig
                    EigTanCdSeq = (int)bookingdata.SelectedStaff.SyainCdSeq,
                    InTanCdSeq = new ClaimModel().SyainCdSeq, //TODO signed in user
                    YoyaNm = (String)bookingdata.TextOrganizationName, // 予約書名
                    YoyaKana = "", // 予約書名（カナ）trash
                    TokuiSeq = (int)bookingdata.customerComponentTokiStData.TokuiSeq, // 得意先ＳＥＱ
                    SitenCdSeq = (int)bookingdata.customerComponentTokiStData.SitenCdSeq, // 支店コードＳＥＱ
                    SirCdSeq = bookingdata.SupervisorTabData.customerComponentTokiStData == null ? 0 : bookingdata.SupervisorTabData.customerComponentTokiStData.TokuiSeq, // 仕入先コードＳＥＱ
                    SirSitenCdSeq = bookingdata.SupervisorTabData.customerComponentTokiStData == null ? 0 : bookingdata.SupervisorTabData.customerComponentTokiStData.SitenCdSeq, // 仕入先支店コードＳＥＱ
                    TokuiTel = bookingdata.SupervisorTabData.TokuiTel ?? string.Empty,
                    TokuiTanNm = bookingdata.SupervisorTabData.TokuiTanNm ?? string.Empty,
                    TokuiFax = bookingdata.SupervisorTabData.TokuiFax ?? string.Empty,
                    TokuiMail = bookingdata.SupervisorTabData.TokuiMail ?? string.Empty,
                    UntKin = bookingdata.VehicleGridDataList.Sum(t => decimal.Parse(t.BusPrice)) - minusTax, // バス代
                    ZeiKbn = (byte)(int)bookingdata.TaxTypeforBus.IdValue, // 消費税タイプ（バス用）
                    Zeiritsu = Convert.ToDecimal(bookingdata.TaxRate), // 消費税率（バス用）
                    ZeiRui = Convert.ToDecimal(bookingdata.TaxBus), // バス代税（バス用）
                    TaxTypeforGuider = (byte)(int)bookingdata.TaxTypeforGuider.IdValue, // 消費税タイプ（ガイド用）
                    TaxGuider = Convert.ToDecimal(bookingdata.TaxGuider), // ガイド税（ガイド用）
                    TesuRitu = decimal.Parse(bookingdata.FeeBusRate), // 手数料率（バス用）
                    TesuRyoG = Convert.ToDecimal(bookingdata.FeeBus), // 手数料税（バス用）
                    FeeGuiderRate = decimal.Parse(bookingdata.FeeGuiderRate), // 手数料率（ガイド用）
                    FeeGuider = Convert.ToDecimal(bookingdata.FeeGuider),// 手数料税（ガイド用）
                    SeiKyuKbnSeq = bookingdata.SelectedInvoiceType?.CodeKbnSeq ?? 0,
                    SeikYm = (string)bookingdata.InvoiceDate.ToString("yyyyMM"), // 請求年月
                    SeiTaiYmd = (string)bookingdata.InvoiceDate.ToString("yyyyMMdd"), // 請求年月日
                    CanRit = 0,
                    CanUnc = 0,
                    CanZkbn = 0,
                    CanSyoR = 0,
                    CanSyoG = 0,
                    CanYmd = "",
                    CanTanSeq = 0,
                    CanRiy = "",
                    CanFuYmd = "0",
                    CanFuTanSeq = 0,
                    CanFuRiy = "",
                    BikoTblSeq = 0,
                    Kskbn = 1, // 仮車区分 1:未仮車 2:仮車済 3:一部済
                    KhinKbn = 1,
                    KaknKais = 0,
                    KaktYmd = "",
                    HaiSkbn = 1,
                    HaiIkbn = 1,
                    GuiWnin = 0,
                    NippoKbn = 1,
                    YouKbn = 1,
                    NyuKinKbn = 1,
                    NcouKbn = 1,
                    SihKbn = 1,
                    ScouKbn = 1,
                    SiyoKbn = 1,
                    UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                    UpdTime = DateTime.Now.ToString("HHmmss"),
                    UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                    UpdPrgId = "KJ1000",
                    GuitKin = bookingdata.TotalGuiderFee - minusGuiTax,

                    TenantCdSeq = new ClaimModel().TenantID,
                    UkeNo = _newUkeNo,
                    UkeCd = _maxUkeCd + 1,
                    BikoNm = bookingdata.BikoNm
                };

                return yyksho;
            }

            private TkdUnkobi CollectDataUnkobi(BookingFormData bookingdata)
            {
                TkdUnkobi tkdunkobi = new TkdUnkobi();

                TkmKasSet tkmKasSet = this._context.TkmKasSet.Where(t => t.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).FirstOrDefault();
                short UriKbn = tkmKasSet.UriKbn;

                tkdunkobi.UnkRen = 1; // 運行日連番
                tkdunkobi.HenKai = 0;
                tkdunkobi.HaiSymd = bookingdata.BusStartDate.ToString("yyyyMMdd"); // 配車年月日
                tkdunkobi.HaiStime = bookingdata.BusStartTime.ToStringWithoutDelimiter(); // 配車時間
                tkdunkobi.SyukoYmd = bookingdata.ReservationTabData.GarageLeaveDate.ToString("yyyyMMdd");
                tkdunkobi.SyuKoTime = bookingdata.ReservationTabData.SyuKoTime.ToStringWithoutDelimiter(); // 出庫時間

                tkdunkobi.SyuPaYmd = bookingdata.ReservationTabData.GoDate.ToString("yyyyMMdd");
                tkdunkobi.SyuPaTime = bookingdata.ReservationTabData.SyuPatime.ToStringWithoutDelimiter(); // 出発時間

                tkdunkobi.DispSyuPaYmd = bookingdata.ReservationTabData.GoDate.ToString("yyyyMMdd");
                tkdunkobi.DispSyuPaTime = bookingdata.ReservationTabData.SyuPatime.ToStringWithoutDelimiter();
                tkdunkobi.DispTouYmd = bookingdata.BusEndDate.ToString("yyyyMMdd");
                tkdunkobi.DispTouChTime = bookingdata.BusEndTime.ToStringWithoutDelimiter();
                tkdunkobi.DispKikYmd = bookingdata.ReservationTabData.GarageReturnDate.ToString("yyyyMMdd");
                tkdunkobi.DispKikTime = bookingdata.ReservationTabData.KikTime.ToStringWithoutDelimiter();

                var touDate = new BookingInputHelper.MyDate(bookingdata.BusEndDate, bookingdata.BusEndTime);
                tkdunkobi.TouYmd = touDate.ConvertedDate.ToString("yyyyMMdd"); // 到着年月日
                tkdunkobi.TouChTime = touDate.ConvertedTime.ToStringWithoutDelimiter();

                tkdunkobi.KikYmd = bookingdata.ReservationTabData.GarageReturnDate.ToString("yyyyMMdd"); // 到着年月日
                tkdunkobi.KikTime = bookingdata.ReservationTabData.KikTime.ToStringWithoutDelimiter();

                // 売上年月日
                if (UriKbn == 1)
                {
                    tkdunkobi.UriYmd = tkdunkobi.HaiSymd; // 配車日時
                }
                else if (UriKbn == 2)
                {
                    tkdunkobi.UriYmd = tkdunkobi.TouYmd; // 終日予定
                }

                tkdunkobi.KanJnm = bookingdata.SupervisorTabData.KanJNm ?? string.Empty;
                tkdunkobi.KanjJyus1 = bookingdata.SupervisorTabData.KanjJyus1 ?? string.Empty;
                tkdunkobi.KanjJyus2 = bookingdata.SupervisorTabData.KanjJyus2 ?? string.Empty;
                tkdunkobi.KanjTel = bookingdata.SupervisorTabData.KanjTel ?? string.Empty;
                tkdunkobi.KanjFax = bookingdata.SupervisorTabData.KanjFax ?? string.Empty;
                tkdunkobi.KanjKeiNo = bookingdata.SupervisorTabData.KanjKeiNo ?? string.Empty;
                tkdunkobi.KanjMail = bookingdata.SupervisorTabData.KanjMail ?? string.Empty;
                tkdunkobi.KanDmhflg = Convert.ToByte(bookingdata.SupervisorTabData.KanDMHFlg);
                tkdunkobi.DanTaNm = (String)bookingdata.TextOrganizationName; // 団体名
                tkdunkobi.DanTaKana = "";
                tkdunkobi.IkMapCdSeq = bookingdata.ReservationTabData.Destination == null ? 0 : bookingdata.ReservationTabData.Destination.BasyoMapCdSeq;
                tkdunkobi.IkNm = bookingdata.ReservationTabData.IkNm ?? string.Empty;
                tkdunkobi.HaiScdSeq = bookingdata.ReservationTabData.DespatchingPlace == null ? 0 : bookingdata.ReservationTabData.DespatchingPlace.HaiSCdSeq;
                tkdunkobi.HaiSnm = bookingdata.ReservationTabData.HaiSNm ?? string.Empty;
                tkdunkobi.HaiSjyus1 = bookingdata.ReservationTabData.HaiSjyus1 ?? string.Empty;
                tkdunkobi.HaiSjyus2 = bookingdata.ReservationTabData.HaiSjyus2 ?? string.Empty;
                tkdunkobi.HaiSkouKcdSeq = 0;
                tkdunkobi.HaiSbinCdSeq = 0;
                tkdunkobi.HaiSsetTime = "";
                tkdunkobi.TouCdSeq = bookingdata.ReservationTabData.ArrivePlace == null ? 0 : bookingdata.ReservationTabData.ArrivePlace.HaiSCdSeq;
                tkdunkobi.TouNm = bookingdata.ReservationTabData.TouNm ?? string.Empty;
                tkdunkobi.TouJyusyo1 = bookingdata.ReservationTabData.TouJyusyo1 ?? string.Empty;
                tkdunkobi.TouJyusyo2 = bookingdata.ReservationTabData.TouJyusyo2 ?? string.Empty;
                tkdunkobi.TouKouKcdSeq = 0;
                tkdunkobi.TouBinCdSeq = 0;
                tkdunkobi.TouSetTime = "";
                tkdunkobi.AreaMapSeq = 0;
                tkdunkobi.AreaNm = "";
                tkdunkobi.HasMapCdSeq = 0;
                tkdunkobi.HasNm = "";
                tkdunkobi.JyoKyakuCdSeq = bookingdata.PassengerTypeData == null ? 0 : bookingdata.PassengerTypeData.JyoKyakuCdSeq;
                tkdunkobi.JyoSyaJin = Convert.ToInt16(bookingdata.SupervisorTabData.JyoSyaJin);
                tkdunkobi.PlusJin = Convert.ToInt16(bookingdata.SupervisorTabData.PlusJin);
                tkdunkobi.DrvJin = Convert.ToInt16(bookingdata.VehicleGridDataList.Sum(t => short.Parse(t.DriverNum))); // 運転手数
                tkdunkobi.GuiSu = Convert.ToInt16(bookingdata.VehicleGridDataList.Sum(t => short.Parse(t.GuiderNum))); // ガイド数
                tkdunkobi.OthJinKbn1 = 99;
                tkdunkobi.OthJin1 = 0;
                tkdunkobi.OthJinKbn2 = 99;
                tkdunkobi.OthJin2 = 0;
                tkdunkobi.Kskbn = 1; // 仮車区分 1:未仮車 2:仮車済 3:一部済
                tkdunkobi.KhinKbn = 1;
                tkdunkobi.HaiSkbn = 1;
                tkdunkobi.HaiIkbn = 1;
                tkdunkobi.GuiWnin = 0;
                tkdunkobi.NippoKbn = 1;
                tkdunkobi.YouKbn = 1;
                tkdunkobi.UkeJyKbnCd = Convert.ToByte(bookingdata.ReservationTabData.AcceptanceConditions.CodeKbn);
                tkdunkobi.SijJoKbn1 = Convert.ToByte(bookingdata.ReservationTabData.RainyMeasure.CodeKbn);
                tkdunkobi.SijJoKbn2 = Convert.ToByte(bookingdata.ReservationTabData.PaymentMethod.CodeKbn);
                tkdunkobi.SijJoKbn3 = Convert.ToByte(bookingdata.ReservationTabData.MovementForm.CodeKbn);
                tkdunkobi.SijJoKbn4 = Convert.ToByte(bookingdata.ReservationTabData.GuiderSetting.CodeKbn);
                tkdunkobi.SijJoKbn5 = Convert.ToByte(bookingdata.ReservationTabData.EstimateSetting.CodeKbn);
                tkdunkobi.RotCdSeq = 0;
                tkdunkobi.ZenHaFlg = Convert.ToByte(bookingdata.PreDaySetting); // 前泊フラグ
                tkdunkobi.KhakFlg = Convert.ToByte(bookingdata.AftDaySetting); // 後泊フラグ
                tkdunkobi.UnkoJkbn = Convert.ToByte(bookingdata.ReservationTabData.MovementStatus.CodeKbn);

                BookingInputHelper.MyDate busStartDay = new BookingInputHelper.MyDate(bookingdata.BusStartDate, bookingdata.BusStartTime);
                BookingInputHelper.MyDate busEndDay = new BookingInputHelper.MyDate(bookingdata.BusEndDate, bookingdata.BusEndTime);

                tkdunkobi.BikoTblSeq = 0;
                tkdunkobi.SiyoKbn = 1;
                tkdunkobi.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                tkdunkobi.UpdTime = DateTime.Now.ToString("HHmmss");
                tkdunkobi.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                tkdunkobi.UpdPrgId = "KJ1000";
                tkdunkobi.BikoNm = string.Empty;
                tkdunkobi.UnitPriceIndex = decimal.Parse(bookingdata.GetAverageUnitPriceIndex());

                tkdunkobi.UkeNo = _newUkeNo;

                return tkdunkobi;
            }

            private List<TkdMishum> CollectDataMishum(BookingFormData bookingdata)
            {
                List<TkdMishum> listMishum = new List<TkdMishum> { };
                TkdMishum mishumForBus = new TkdMishum();
                mishumForBus.MisyuRen = 1; // 未収明細連番
                mishumForBus.HenKai = 0; // 変更回数
                mishumForBus.SeiFutSyu = (byte)1; // 請求付帯種別 : 1:運賃2:付帯3:通行料4:手配料5ガイド料6:積込品7:キャンセル料

                // #8091
                mishumForBus.SyaRyoSyo = Convert.ToInt32(bookingdata.TaxBus); // 消費税額
                mishumForBus.SyaRyoTes = Convert.ToInt32(bookingdata.FeeBus); // 手数料額
                mishumForBus.UriGakKin = bookingdata.VehicleGridDataList.Sum(t => int.Parse(t.BusPrice));
                if (bookingdata.TaxTypeforBus.IdValue == 2)
                {
                    mishumForBus.UriGakKin -= mishumForBus.SyaRyoSyo;
                }
                mishumForBus.SeiKin = mishumForBus.UriGakKin + mishumForBus.SyaRyoSyo;
                if (bookingdata.customerComponentTokiStData.TesKbn == 2)
                {
                    mishumForBus.SeiKin -= mishumForBus.SyaRyoTes;
                }

                mishumForBus.NyuKinRui = 0;
                mishumForBus.CouKesRui = 0;
                mishumForBus.FutuUnkRen = 0;
                mishumForBus.FutTumRen = 0;
                mishumForBus.SiyoKbn = 1;
                mishumForBus.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                mishumForBus.UpdTime = DateTime.Now.ToString("HHmmss");
                mishumForBus.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mishumForBus.UpdPrgId = "KJ1000";
                mishumForBus.UkeNo = _newUkeNo;

                listMishum.Add(mishumForBus);
                if (Convert.ToInt16(bookingdata.VehicleGridDataList.Sum(t => short.Parse(t.GuiderNum))) < 1)
                {
                    return listMishum;
                }
                TkdMishum mishumForGuider = new TkdMishum();
                mishumForGuider.MisyuRen = 2; // 未収明細連番
                mishumForGuider.HenKai = 0; // 変更回数
                mishumForGuider.SeiFutSyu = (byte)5; // 請求付帯種別 : 1:運賃2:付帯3:通行料4:手配料5ガイド料6:積込品7:キャンセル料

                // #8091
                mishumForGuider.UriGakKin = bookingdata.VehicleGridDataList.Sum(t => int.Parse(t.GuiderFee)); // 売上額
                mishumForGuider.SyaRyoSyo = Convert.ToInt32(bookingdata.TaxGuider); // 消費税額
                mishumForGuider.SyaRyoTes = Convert.ToInt32(bookingdata.FeeGuider); // 手数料額
                if (bookingdata.TaxTypeforGuider.IdValue == 2)
                {
                    mishumForGuider.UriGakKin -= mishumForGuider.SyaRyoSyo;
                }
                mishumForGuider.SeiKin = mishumForGuider.UriGakKin + mishumForGuider.SyaRyoSyo;
                if (bookingdata.customerComponentTokiStData.TesKbn == 2)
                {
                    mishumForGuider.SeiKin -= mishumForGuider.SyaRyoTes;
                }

                mishumForGuider.NyuKinRui = 0;
                mishumForGuider.CouKesRui = 0;
                mishumForGuider.FutuUnkRen = 0;
                mishumForGuider.FutTumRen = 0;
                mishumForGuider.SiyoKbn = 1;
                mishumForGuider.UpdYmd = DateTime.Now.ToString("yyyyMMdd"); ;
                mishumForGuider.UpdTime = DateTime.Now.ToString("HHmmss"); ;
                mishumForGuider.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                mishumForGuider.UpdPrgId = "KJ1000";

                mishumForGuider.UkeNo = _newUkeNo;
                listMishum.Add(mishumForGuider);

                return listMishum;
            }

            private List<TkdYykSyu> CollectDataYykSyu(BookingFormData bookingdata)
            {
                List<TkdYykSyu> listTkdYykSyu = new List<TkdYykSyu> { };
                foreach (var vehicleTypeRow in bookingdata.VehicleGridDataList) // Loop each row in grid of Booking Screen
                {
                    TkdYykSyu yyksyu = new TkdYykSyu()
                    {
                        UnkRen = 1,
                        SyaSyuRen = Int16.Parse(vehicleTypeRow.RowID),
                        HenKai = 0,
                        SyaSyuCdSeq = (byte)(vehicleTypeRow.busTypeData.SyaSyuCdSeq),
                        KataKbn = (byte)Convert.ToInt32(vehicleTypeRow.busTypeData.Katakbn),
                        SyaSyuDai = Convert.ToInt16(vehicleTypeRow.BusNum),
                        SyaSyuTan = Convert.ToInt32(vehicleTypeRow.UnitPrice),
                        SyaRyoUnc = Convert.ToInt32(vehicleTypeRow.BusPrice),
                        DriverNum = byte.Parse(vehicleTypeRow.DriverNum),
                        UnitBusPrice = Convert.ToInt32(vehicleTypeRow.UnitBusPrice),
                        UnitBusFee = Convert.ToInt32(vehicleTypeRow.UnitBusFee),
                        GuiderNum = byte.Parse(vehicleTypeRow.GuiderNum),
                        UnitGuiderPrice = Convert.ToInt32(vehicleTypeRow.UnitGuiderFee),
                        UnitGuiderFee = Convert.ToInt32(vehicleTypeRow.GuiderFee),
                        SiyoKbn = 1,
                        UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                        UpdTime = DateTime.Now.ToString("HHmmss"),
                        UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                        UpdPrgId = "KJ1000",

                        UkeNo = _newUkeNo
                    };

                    listTkdYykSyu.Add(yyksyu);
                }
                return listTkdYykSyu;
            }

            private List<TkdHaisha> CollectDataHaisha(InsertBookingInputCommand command, TkdMishum mishumForBus)
            {
                var bookingdata = command._bookingData;
                List<TkdHaisha> listTkdHaisha = new List<TkdHaisha>();
                short AutoIncrementVehicleID = 0;
                decimal taxRate = (decimal)Math.Round(decimal.Parse(command._bookingData.TaxRate) / 100, 3);
                var busDateTime = command._bookingData.GetBusDateTime();
                var garageDateTime = command._bookingData.GetBusGarageDateTime();

                List<Vehical> vehiclesAssigned = command._vehiclesInfo.VehiclesAssigned;

                foreach (var vehicleTypeRow in command._bookingData.VehicleGridDataList.OrderByDescending((v) => v.busTypeData.SyaSyuCd)) // Loop each row in grid of Booking Screen
                {
                    List<Vehical> masterVehicle = BookingInputHelper.GetMasterVehicals(vehicleTypeRow, command._vehiclesInfo.Vehicles.ToList());

                    int driverNumInt = int.Parse(vehicleTypeRow.DriverNum);
                    int busNumInt = int.Parse(vehicleTypeRow.BusNum);
                    int guiderNumInt = int.Parse(vehicleTypeRow.GuiderNum);

                    int OverDriverPerBus = driverNumInt % busNumInt;
                    short DriverPerBus = Convert.ToInt16(driverNumInt / busNumInt);

                    int OverGuiderPerBus = guiderNumInt % busNumInt;
                    short GuiderPerBus = Convert.ToInt16(guiderNumInt / busNumInt);

                    var busTaxPrice = Convert.ToInt32(vehicleTypeRow.UnitPrice) * taxRate / (1 + taxRate);
                    int minusBusTax = bookingdata.TaxTypeforBus.IdValue == Constants.InTax.IdValue
                        ? (int)RoundHelper[bookingdata.HasuSet.TaxSetting].Invoke(busTaxPrice)
                        : 0;
                    // Set AutoIncrement to new busNum / new row
                    for (int i = 0; i < busNumInt; i++) // Loop number of Vehicle in each row
                    {
                        AutoIncrementVehicleID++;
                        TkdHaisha tkdhaisha = new TkdHaisha();
                        tkdhaisha.UnkRen = 1;
                        tkdhaisha.SyaSyuRen = Int16.Parse(vehicleTypeRow.RowID);
                        tkdhaisha.TeiDanNo = AutoIncrementVehicleID;
                        tkdhaisha.BunkRen = 1;
                        tkdhaisha.HenKai = 0;
                        tkdhaisha.GoSya = String.Format("{0:00}", AutoIncrementVehicleID);
                        tkdhaisha.GoSyaJyn = AutoIncrementVehicleID;
                        tkdhaisha.BunKsyuJyn = 0;
                        tkdhaisha.SyuEigCdSeq = 0; // auto assigned bus will update this update it
                        tkdhaisha.KikEigSeq = 0; // auto assigned bus will update this update it
                        tkdhaisha.HaiSsryCdSeq = 0; // auto assigned bus will update this update it
                        tkdhaisha.KssyaRseq = 0; // auto assigned bus will update this update it
                        tkdhaisha.DanTaNm2 = "";
                        tkdhaisha.IkMapCdSeq = command._bookingData.ReservationTabData.Destination == null ? 0 : command._bookingData.ReservationTabData.Destination.BasyoMapCdSeq;
                        tkdhaisha.IkNm = command._bookingData.ReservationTabData.IkNm ?? string.Empty;
                        // doesn't need to check if after date is checked, already update garageReturnDate in UI
                        tkdhaisha.SyuKoYmd = garageDateTime.Leave.ConvertedDate.AddMinutes(-Common.BusCheckBeforeOrAfterRunningDuration).ToString("yyyyMMdd");
                        tkdhaisha.SyuKoTime = command._bookingData.ReservationTabData.SyuKoTime.ToStringWithoutDelimiter(); // 出庫時間
                        tkdhaisha.SyuPaTime = command._bookingData.ReservationTabData.SyuPatime.ToStringWithoutDelimiter();
                        tkdhaisha.HaiSymd = busDateTime.Start.inpDate.ToString("yyyyMMdd");
                        tkdhaisha.HaiStime = busDateTime.Start.inpTime.ToStringWithoutDelimiter();
                        tkdhaisha.HaiScdSeq = command._bookingData.ReservationTabData.DespatchingPlace == null ? 0 : command._bookingData.ReservationTabData.DespatchingPlace.HaiSCdSeq;
                        tkdhaisha.HaiSnm = command._bookingData.ReservationTabData.HaiSNm ?? string.Empty;
                        tkdhaisha.HaiSjyus1 = command._bookingData.ReservationTabData.HaiSjyus1 ?? string.Empty;
                        tkdhaisha.HaiSjyus2 = command._bookingData.ReservationTabData.HaiSjyus2 ?? string.Empty;
                        tkdhaisha.HaiSkigou = "";
                        tkdhaisha.HaiSkouKcdSeq = 0;
                        tkdhaisha.HaiSbinCdSeq = 0;
                        tkdhaisha.HaiSsetTime = "";
                        
                        tkdhaisha.KikYmd = garageDateTime.Return.ConvertedDate.ToString("yyyyMMdd");
                        tkdhaisha.KikTime = garageDateTime.Return.ConvertedTime.ToStringWithoutDelimiter(); // 帰庫時間
                        tkdhaisha.TouYmd = busDateTime.End.ConvertedDate.ToString("yyyyMMdd");
                        tkdhaisha.TouChTime = busDateTime.End.ConvertedTime.ToStringWithoutDelimiter();

                        tkdhaisha.TouCdSeq = command._bookingData.ReservationTabData.ArrivePlace == null ? 0 : command._bookingData.ReservationTabData.ArrivePlace.HaiSCdSeq;
                        tkdhaisha.TouNm = command._bookingData.ReservationTabData.TouNm ?? string.Empty;
                        tkdhaisha.TouJyusyo1 = command._bookingData.ReservationTabData.TouJyusyo1 ?? string.Empty;
                        tkdhaisha.TouJyusyo2 = command._bookingData.ReservationTabData.TouJyusyo2 ?? string.Empty;
                        tkdhaisha.TouKigou = "";
                        tkdhaisha.TouKouKcdSeq = 0;
                        tkdhaisha.TouBinCdSeq = 0;
                        tkdhaisha.TouSetTime = "";
                        tkdhaisha.JyoSyaJin = 0;
                        tkdhaisha.PlusJin = 0;
                        // 運転手数
                        if (i < OverDriverPerBus)
                        {
                            tkdhaisha.DrvJin = Convert.ToInt16(DriverPerBus + 1);
                        }
                        else
                        {
                            tkdhaisha.DrvJin = DriverPerBus;
                        }
                        // ガイド数
                        if (i < OverGuiderPerBus)
                        {
                            tkdhaisha.GuiSu = Convert.ToInt16(GuiderPerBus + 1);
                        }
                        else
                        {
                            tkdhaisha.GuiSu = GuiderPerBus;
                        }

                        tkdhaisha.OthJinKbn1 = 99;
                        tkdhaisha.OthJin1 = 0;
                        tkdhaisha.OthJinKbn2 = 99;
                        tkdhaisha.OthJin2 = 0;
                        tkdhaisha.Kskbn = 1; // 1:未仮車  2:仮車済 auto assigned bus will update this update it
                        tkdhaisha.KhinKbn = 1;
                        tkdhaisha.HaiSkbn = 1; // 配車区分 1:未仮車  2:仮車済 auto assigned bus will update this update it
                        tkdhaisha.HaiIkbn = 1; // 配員区分 
                        tkdhaisha.GuiWnin = 0;
                        tkdhaisha.NippoKbn = 1;
                        tkdhaisha.YouTblSeq = 0;
                        tkdhaisha.YouKataKbn = 9; // 傭車型区分
                        tkdhaisha.SyaRyoUnc = Convert.ToInt32(vehicleTypeRow.UnitPrice);
                        // #7979 start
                        int taxbus = 0;
                        if (command._bookingData.TaxTypeforBus.IdValue == Constants.NoTax.IdValue)
                        {
                            taxbus = 0;
                        }
                        else
                        {
                            if (command._bookingData.TaxTypeforBus.IdValue == Constants.ForeignTax.IdValue)
                            {
                                taxbus = (int)BookingInputHelper.RoundHelper[command._bookingData.HasuSet.TaxSetting](tkdhaisha.SyaRyoUnc * taxRate);
                            }
                            else // InTax
                            {
                                taxbus = (int)BookingInputHelper.RoundHelper[command._bookingData.HasuSet.TaxSetting]((tkdhaisha.SyaRyoUnc * taxRate) / (1 + taxRate));
                            }
                        }

                        tkdhaisha.SyaRyoSyo = taxbus;
                        int feebus = 0;
                        var feeRate = (decimal)Math.Round(decimal.Parse(command._bookingData.FeeBusRate) / 100, 3);
                        if (command._bookingData.TaxTypeforBus.IdValue == Constants.ForeignTax.IdValue)
                        {
                            feebus = (int)BookingInputHelper.RoundHelper[command._bookingData.HasuSet.FeeSetting]((tkdhaisha.SyaRyoUnc + taxbus) * feeRate);
                        }
                        else
                        {
                            feebus = (int)BookingInputHelper.RoundHelper[command._bookingData.HasuSet.FeeSetting](tkdhaisha.SyaRyoUnc * feeRate);
                        }
                        tkdhaisha.SyaRyoTes = feebus;
                        // #7979 end
                        tkdhaisha.SyaRyoUnc = Convert.ToInt32(vehicleTypeRow.UnitPrice) - minusBusTax;
                        tkdhaisha.YoushaUnc = 0;
                        tkdhaisha.YoushaSyo = 0;
                        tkdhaisha.YoushaTes = 0;
                        tkdhaisha.PlatNo = "";
                        tkdhaisha.UkeJyKbnCd = Convert.ToByte(bookingdata.ReservationTabData.AcceptanceConditions.CodeKbn);
                        tkdhaisha.SijJoKbn1 = Convert.ToByte(bookingdata.ReservationTabData.RainyMeasure.CodeKbn);
                        tkdhaisha.SijJoKbn2 = Convert.ToByte(bookingdata.ReservationTabData.PaymentMethod.CodeKbn);
                        tkdhaisha.SijJoKbn3 = Convert.ToByte(bookingdata.ReservationTabData.MovementForm.CodeKbn);
                        tkdhaisha.SijJoKbn4 = Convert.ToByte(bookingdata.ReservationTabData.GuiderSetting.CodeKbn);
                        tkdhaisha.SijJoKbn5 = Convert.ToByte(bookingdata.ReservationTabData.EstimateSetting.CodeKbn);
                        tkdhaisha.RotCdSeq = 0;
                        tkdhaisha.BikoTblSeq = 0;
                        tkdhaisha.HaiCom = "";
                        tkdhaisha.SiyoKbn = 1;
                        tkdhaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        tkdhaisha.UpdTime = DateTime.Now.ToString("HHmmss");
                        tkdhaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdhaisha.UpdPrgId = "KJ1000";

                        // Auto asign vehicle
                        Vehical vehicleWillAssign = null;
                        foreach (var vehicle in masterVehicle)
                        {
                            if (vehiclesAssigned.FindIndex(_=>_.VehicleModel.SyaRyoCdSeq == vehicle.VehicleModel.SyaRyoCdSeq) >= 0) 
                            { 
                                continue; 
                            }
                            else
                            {
                                vehiclesAssigned.Add(vehicle);
                                vehicleWillAssign = vehicle;
                                break;
                            }
                        }

                        if (vehicleWillAssign != null)
                        {
                            tkdhaisha.HaiSsryCdSeq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                            tkdhaisha.SyuEigCdSeq = vehicleWillAssign.EigyoCdSeq;
                            tkdhaisha.KikEigSeq = vehicleWillAssign.EigyoCdSeq;
                            tkdhaisha.KssyaRseq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                            tkdhaisha.Kskbn = 2; // 1:未仮車  2:仮車済
                        }
                        tkdhaisha.UkeNo = _newUkeNo;
                        listTkdHaisha.Add(tkdhaisha);
                    }

                }
                //#8091
                int diffUnc = mishumForBus.UriGakKin - listTkdHaisha.Sum(f => f.SyaRyoUnc);
                int assingeIndex = 0;
                int assingeUnit = 0;
                if (diffUnc != 0)
                {
                    while(diffUnc != 0)
                    {
                        assingeUnit = diffUnc < 0 ? -1 : 1;
                        listTkdHaisha[assingeIndex++].SyaRyoUnc += assingeUnit;
                        diffUnc -= assingeUnit;
                        if(assingeIndex == listTkdHaisha.Count) assingeIndex = 0;
                    }
                }
                int diffSyaRyoSyo = mishumForBus.SyaRyoSyo - listTkdHaisha.Sum(f => f.SyaRyoSyo);
                if (diffSyaRyoSyo != 0)
                {
                    assingeIndex = 0;
                    while(diffSyaRyoSyo != 0)
                    {
                        assingeUnit = diffSyaRyoSyo < 0 ? -1 : 1;
                        listTkdHaisha[assingeIndex++].SyaRyoSyo += assingeUnit;
                        diffSyaRyoSyo -= assingeUnit;
                        if(assingeIndex == listTkdHaisha.Count) assingeIndex = 0;
                    }
                }
                int diffSyaRyoTes = mishumForBus.SyaRyoTes - listTkdHaisha.Sum(f => f.SyaRyoTes);
                if (diffSyaRyoTes != 0)
                {
                    assingeIndex = 0;
                    while(diffSyaRyoTes != 0)
                    {
                        assingeUnit = diffSyaRyoTes < 0 ? -1 : 1;
                        listTkdHaisha[assingeIndex++].SyaRyoTes += assingeUnit;
                        diffSyaRyoTes -= assingeUnit;
                        if(assingeIndex == listTkdHaisha.Count) assingeIndex = 0;
                    }
                }
                //#8091

                int x = listTkdHaisha.Count;
                int rowdriver = 0;
                for (int j = 0; j < Convert.ToInt16(command._bookingData.SupervisorTabData.JyoSyaJin); j++)
                {

                    if (rowdriver == x)
                    {
                        rowdriver = 0;
                        listTkdHaisha[rowdriver].JyoSyaJin++;
                    }
                    else if (j > x)
                    {
                        listTkdHaisha[rowdriver].JyoSyaJin++;
                    }
                    else
                    {
                        listTkdHaisha[j].JyoSyaJin++;
                    }
                    rowdriver++;
                }

                int rowguide = 0;
                for (int j = 0; j < Convert.ToInt16(command._bookingData.SupervisorTabData.PlusJin); j++)
                {

                    if (rowguide == x)
                    {
                        rowguide = 0;
                        listTkdHaisha[rowguide].PlusJin++;
                    }
                    else if (j > x)
                    {
                        listTkdHaisha[rowguide].PlusJin++;
                    }
                    else
                    {
                        listTkdHaisha[j].PlusJin++;
                    }
                    rowguide++;
                }
                if (command._bookingData.ReservationTabData.MovementStatus.CodeKbn == "1")
                {
                    var arrangeHaisha = new Dictionary<CommandMode, List<TkdHaisha>>();
                    arrangeHaisha[CommandMode.Create] = listTkdHaisha;
                    var result = BookingInputHelper.ArrangeHaishaDaily(arrangeHaisha);
                    listTkdHaisha = result[CommandMode.Create];
                }
                return listTkdHaisha;
            }

            private List<TkdBookingMaxMinFareFeeCalc> CollectDataBookingMaxMinFareFeeCals(BookingFormData bookingdata)
            {
                List<TkdBookingMaxMinFareFeeCalc> listTkdBookingMaxMinFareFeeCalc = new List<TkdBookingMaxMinFareFeeCalc>();

                foreach (var vehicleTypeRow in bookingdata.VehicleGridDataList) // Loop each row in grid of Booking Screen
                {
                    MinMaxSettingFormData minMaxSettingFormData = vehicleTypeRow.minMaxForm;
                    if (minMaxSettingFormData.typeBus != -1)
                    {
                        TkdBookingMaxMinFareFeeCalc tkdBookingMaxMinFareFeeCals = new TkdBookingMaxMinFareFeeCalc();
                        tkdBookingMaxMinFareFeeCals.UnkRen = 1;
                        tkdBookingMaxMinFareFeeCals.SyaSyuRen = Int16.Parse(vehicleTypeRow.RowID);
                        tkdBookingMaxMinFareFeeCals.TransportationPlaceCodeSeq = minMaxSettingFormData.SelectedTranSportationOfficePlace.TransportationPlaceCodeSeq;
                        tkdBookingMaxMinFareFeeCals.KataKbn = (byte)minMaxSettingFormData.typeBus;
                        tkdBookingMaxMinFareFeeCals.ZeiKbn = (byte)minMaxSettingFormData.TaxType.IdValue;
                        tkdBookingMaxMinFareFeeCals.Zeiritsu = (decimal)minMaxSettingFormData.TaxRate;
                        tkdBookingMaxMinFareFeeCals.RunningKmSum = minMaxSettingFormData.minMaxGridData.Sum(t => t.KmRunning);
                        tkdBookingMaxMinFareFeeCals.RunningKmCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Sum(t => t.KmRunning));
                        tkdBookingMaxMinFareFeeCals.RestraintTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunning).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.RestraintTimeCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunning)).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.ServiceKmSum = minMaxSettingFormData.minMaxGridData.Sum(t => t.ExactKmRunning);
                        tkdBookingMaxMinFareFeeCals.ServiceTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.ExactTimeRunning).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.MidnightEarlyMorningTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunning).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.MidnightEarlyMorningTimeCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunning)).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.ChangeDriverRunningKmSum = minMaxSettingFormData.minMaxGridData.Sum(t => t.KmRunningwithChgDriver);
                        tkdBookingMaxMinFareFeeCals.ChangeDriverRunningKmCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Sum(t => t.KmRunningwithChgDriver));
                        tkdBookingMaxMinFareFeeCals.ChangeDriverRestraintTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunningwithChgDriver).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.ChangeDriverRestraintTimeCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.TimeRunningwithChgDriver)).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.ChangeDriverMidnightEarlyMorningTimeSum = minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunningwithChgDriver).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.ChangeDriverMidnightEarlyMorningTimeCalc = BookingInputHelper.Round(minMaxSettingFormData.minMaxGridData.Aggregate<MinMaxGridData, BookingInputHelper.MyTime>(new BookingInputHelper.MyTime(0, 0), (t, s) => t = t + s.SpecialTimeRunningwithChgDriver)).ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCals.WaribikiKbn = (byte)minMaxSettingFormData.DiscountOption;
                        tkdBookingMaxMinFareFeeCals.AnnualContractFlag = (byte)minMaxSettingFormData.AnnualContractOption;
                        tkdBookingMaxMinFareFeeCals.SpecialFlg = (byte)minMaxSettingFormData.SpecialVehicleOption;
                        tkdBookingMaxMinFareFeeCals.FareMaxAmount = minMaxSettingFormData.getMaxUnitBusPriceDiscount();
                        tkdBookingMaxMinFareFeeCals.FareMinAmount = minMaxSettingFormData.getMinUnitBusPriceDiscount();
                        tkdBookingMaxMinFareFeeCals.FeeMaxAmount = minMaxSettingFormData.getMaxUnitBusFeeDiscount();
                        tkdBookingMaxMinFareFeeCals.FeeMinAmount = minMaxSettingFormData.getMinUnitBusFeeDiscount();
                        tkdBookingMaxMinFareFeeCals.UnitPriceMaxAmount = minMaxSettingFormData.getMaxUnitBusPriceDiscount() + minMaxSettingFormData.getMaxUnitBusFeeDiscount();
                        tkdBookingMaxMinFareFeeCals.UnitPriceMinAmount = minMaxSettingFormData.getMinUnitBusPriceDiscount() + minMaxSettingFormData.getMinUnitBusFeeDiscount();
                        tkdBookingMaxMinFareFeeCals.FareMaxAmountforKm = minMaxSettingFormData.getMaxUnitPriceForkm();
                        tkdBookingMaxMinFareFeeCals.FareMinAmountforKm = minMaxSettingFormData.getMinUnitPriceForkm();
                        tkdBookingMaxMinFareFeeCals.FareMaxAmountforHour = minMaxSettingFormData.getMaxUnitPriceForHour();
                        tkdBookingMaxMinFareFeeCals.FareMinAmountforHour = minMaxSettingFormData.getMinUnitPriceForHour();
                        tkdBookingMaxMinFareFeeCals.ChangeDriverFareMaxAmountforKm = minMaxSettingFormData.getMaxUnitBusFeeForKmWithChgDriver();
                        tkdBookingMaxMinFareFeeCals.ChangeDriverFareMinAmountforKm = minMaxSettingFormData.getMinUnitBusFeeForKmWithChgDriver();
                        tkdBookingMaxMinFareFeeCals.ChangeDriverFareMaxAmountforHour = minMaxSettingFormData.getMaxUnitBusFeeforHourWithChgDriver();
                        tkdBookingMaxMinFareFeeCals.ChangeDriverFareMinAmountforHour = minMaxSettingFormData.getMinUnitBusFeeforHourWithChgDriver();
                        tkdBookingMaxMinFareFeeCals.MidnightEarlyMorningFeeMaxAmount = minMaxSettingFormData.getMaxUnitBusSumFeeforMid9();
                        tkdBookingMaxMinFareFeeCals.MidnightEarlyMorningFeeMinAmount = minMaxSettingFormData.getMinUnitBusSumFeeforMid9();
                        tkdBookingMaxMinFareFeeCals.SpecialVehicalFeeMaxAmount = minMaxSettingFormData.getMaxSpecialVehicalFee();
                        tkdBookingMaxMinFareFeeCals.SpecialVehicalFeeMinAmount = minMaxSettingFormData.getMinSpecialVehicalFee();
                        tkdBookingMaxMinFareFeeCals.UnitPriceIndex = decimal.Parse(vehicleTypeRow.UnitPriceIndex);
                        tkdBookingMaxMinFareFeeCals.FareIndex = decimal.Parse(minMaxSettingFormData.FareIndex);
                        tkdBookingMaxMinFareFeeCals.FeeIndex = decimal.Parse(minMaxSettingFormData.FeeIndex);
                        tkdBookingMaxMinFareFeeCals.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        tkdBookingMaxMinFareFeeCals.UpdTime = DateTime.Now.ToString("HHmmss");
                        tkdBookingMaxMinFareFeeCals.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdBookingMaxMinFareFeeCals.UpdPrgId = "KJ1000";

                        tkdBookingMaxMinFareFeeCals.UkeNo = _newUkeNo;
                        listTkdBookingMaxMinFareFeeCalc.Add(tkdBookingMaxMinFareFeeCals);
                    }
                }
                return listTkdBookingMaxMinFareFeeCalc;
            }

            private List<TkdBookingMaxMinFareFeeCalcMeisai> CollectDataBookingMaxMinFareFeeCalsDetails(BookingFormData bookingdata)
            {
                List<TkdBookingMaxMinFareFeeCalcMeisai> listTkdBookingMaxMinFareFeeCalsMesai = new List<TkdBookingMaxMinFareFeeCalcMeisai>();
                foreach (var vehicleTypeRow in bookingdata.VehicleGridDataList) // Loop each row in grid of Booking Screen
                {
                    MinMaxSettingFormData minMaxSettingFormData = vehicleTypeRow.minMaxForm;
                    foreach (var minMaxGridData in minMaxSettingFormData.minMaxGridData) // Loop each row in grid of Min-Max popup
                    {
                        TkdBookingMaxMinFareFeeCalcMeisai tkdBookingMaxMinFareFeeCalsMeisai = new TkdBookingMaxMinFareFeeCalcMeisai();

                        tkdBookingMaxMinFareFeeCalsMeisai.UnkRen = 1;
                        tkdBookingMaxMinFareFeeCalsMeisai.SyaSyuRen = Int16.Parse(vehicleTypeRow.RowID);
                        tkdBookingMaxMinFareFeeCalsMeisai.Nittei = (byte)(minMaxGridData.rowID);
                        tkdBookingMaxMinFareFeeCalsMeisai.UnkYmd = minMaxGridData.DateofScheduler.ToString("yyyyMMdd");
                        tkdBookingMaxMinFareFeeCalsMeisai.RunningKm = minMaxGridData.KmRunning;
                        tkdBookingMaxMinFareFeeCalsMeisai.InspectionStartYmd = minMaxGridData.BusInspectionStartDate.ConvertedDate.ToString("yyyyMMdd");
                        tkdBookingMaxMinFareFeeCalsMeisai.InspectionStartTime = minMaxGridData.BusInspectionStartDate.inpTime.ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCalsMeisai.InspectionEndYmd = minMaxGridData.BusInspectionEndDate.inpDate.ToString("yyyyMMdd");
                        tkdBookingMaxMinFareFeeCalsMeisai.InspectionEndTime = minMaxGridData.BusInspectionEndDate.inpTime.ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCalsMeisai.RestraintTime = minMaxGridData.TimeRunning.ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCalsMeisai.ServiceKm = minMaxGridData.ExactKmRunning;
                        tkdBookingMaxMinFareFeeCalsMeisai.ServiceTime = minMaxGridData.ExactTimeRunning.ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCalsMeisai.MidnightEarlyMorningTime = minMaxGridData.SpecialTimeRunning.ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCalsMeisai.ChangeDriverFeeFlag = Convert.ToByte(minMaxGridData.isChangeDriver);
                        tkdBookingMaxMinFareFeeCalsMeisai.ChangeDriverRunningKm = minMaxGridData.KmRunningwithChgDriver;
                        tkdBookingMaxMinFareFeeCalsMeisai.ChangeDriverRestraintTime = minMaxGridData.TimeRunningwithChgDriver.ToStringWithoutDelimiter();
                        tkdBookingMaxMinFareFeeCalsMeisai.ChangeDriverMidnightEarlyMorningTime = minMaxGridData.SpecialTimeRunningwithChgDriver.ToStringWithoutDelimiter(); ;
                        tkdBookingMaxMinFareFeeCalsMeisai.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        tkdBookingMaxMinFareFeeCalsMeisai.UpdTime = DateTime.Now.ToString("HHmmss");
                        tkdBookingMaxMinFareFeeCalsMeisai.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdBookingMaxMinFareFeeCalsMeisai.UpdPrgId = "KJ1000";

                        tkdBookingMaxMinFareFeeCalsMeisai.UkeNo = _newUkeNo;
                        listTkdBookingMaxMinFareFeeCalsMesai.Add(tkdBookingMaxMinFareFeeCalsMeisai);
                    }
                }
                return listTkdBookingMaxMinFareFeeCalsMesai;
            }

            private List<TkdKariei> CollectDataKariei(BookingFormData bookingData, List<TkdHaisha> haishas)
            {
                if(bookingData is null)
                    throw new ArgumentNullException(nameof(bookingData));
                if(haishas is null)
                    throw new ArgumentNullException(nameof(haishas));

                List<TkdKariei> karieis = new List<TkdKariei>();
                
                bookingData.VehicleGridDataList.ForEach(item => {
                    if(item.PriorityAutoAssignBranch != null)
                    {
                        int countAssigned = haishas.Where(_ => 
                                _.SyaSyuRen.ToString() == item.RowID && 
                                _.Kskbn == 2 && 
                                _.KikEigSeq == item.PriorityAutoAssignBranch.EigyoCdSeq)
                            .Count();

                        TkdKariei tkdKariei = new TkdKariei
                        {
                            UkeNo = _newUkeNo,
                            UnkRen = 1,
                            SyaSyuRen = Int16.Parse(item.RowID),
                            KarieiRen = 1,
                            HenKai = 0,
                            KseigSeq = item.PriorityAutoAssignBranch.EigyoCdSeq,
                            KariDai = (short)countAssigned, 
                            SiyoKbn = 1,
                            UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                            UpdTime = DateTime.Now.ToString("HHmmss"),
                            UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                            UpdPrgId = "KJ1000",
                        };
                        karieis.Add(tkdKariei);
                    }
                });

                return karieis;
            }

        }
    }
}
