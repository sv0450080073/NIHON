using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetBookingInputQuery : IRequest<BookingFormData>
    {
        private readonly string _ukeNo;

        public GetBookingInputQuery(string ukeNo)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
        }

        public class Handler : IRequestHandler<GetBookingInputQuery, BookingFormData>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            //TODO Temporary load data from old field to new field #1570
            public async Task<BookingFormData> Handle(GetBookingInputQuery request, CancellationToken cancellationToken)
            {
                // Get data for 予約書
                var yyksho = _context.TkdYyksho.Where(x => x.UkeNo == request._ukeNo && x.SiyoKbn == 1).FirstOrDefault();
                string YykSyuUpdYmdTime = _context.TkdYykSyu.Where(x => x.UkeNo == request._ukeNo && x.SiyoKbn == 1).Select(x => x.UpdYmd + x.UpdTime).Max();
                string HaishaUpdYmdTime = _context.TkdHaisha.Where(x => x.UkeNo == request._ukeNo && x.SiyoKbn == 1).Select(x => x.UpdYmd + x.UpdTime).Max();
                string MishumUpdYmdTime = _context.TkdMishum.Where(x => x.UkeNo == request._ukeNo && x.SiyoKbn == 1 && (x.SeiFutSyu == 1 || x.SeiFutSyu == 7)).Select(x => x.UpdYmd + x.UpdTime).Max();
                string KakninUpdYmdTime = _context.TkdKaknin.Where(x => x.UkeNo == request._ukeNo && x.SiyoKbn == 1).Select(x => x.UpdYmd + x.UpdTime).Max();
                string BookingMaxMinFareFeeCalcUpdYmdTime = _context.TkdBookingMaxMinFareFeeCalc.Where(x => x.UkeNo == request._ukeNo).Select(x => x.UpdYmd + x.UpdTime).Max();
                string BookingMaxMinFareFeeCalcMeisaiUpdYmdTime = _context.TkdBookingMaxMinFareFeeCalcMeisai.Where(x => x.UkeNo == request._ukeNo).Select(x => x.UpdYmd + x.UpdTime).Max();
                string UnkobiFileUpdYmdTime = _context.TkdUnkobiFile.Where(x => x.UkeNo == request._ukeNo).Select(x => x.UpdYmd + x.UpdTime).Max();
                if (yyksho == null) return null;

                // Get data for 運行日
                var unkobi = _context.TkdUnkobi.Where(x => x.UkeNo == request._ukeNo && x.SiyoKbn == 1).FirstOrDefault();
                var bookingFormData = new BookingFormData()
                {
                    IsDailyReportRegisted = yyksho.NippoKbn != 1,
                    BookingStatus = new TPM_CodeKbCodeSyuData() { CodeKbn = yyksho.YoyaSyu.ToString("D2") },
                    KaktYmd = yyksho.KaktYmd.Trim(),
                    HaiSKbn = yyksho.HaiSkbn,
                    SelectedInvoiceType = new InvoiceType() { CodeKbnSeq = yyksho.SeiKyuKbnSeq },
                    CurrentBookingType = new ReservationClassComponentData { YoyaKbnSeq = yyksho.YoyaKbnSeq },
                    SelectedSaleBranch = new LoadSaleBranch { EigyoCdSeq = yyksho.UkeEigCdSeq },
                    SelectedStaff = new LoadStaff { SyainCdSeq = yyksho.EigTanCdSeq },
                    customerComponentTokiStData = new CustomerComponentTokiStData { TokuiSeq = yyksho.TokuiSeq, SitenCdSeq = yyksho.SitenCdSeq },
                    customerComponentTokiskData = new CustomerComponentTokiskData { TokuiSeq = yyksho.TokuiSeq },
                    SupervisorTabData = new SupervisorTabData()
                    {
                        JyoSyaJin = unkobi.JyoSyaJin.ToString(),
                        KanDMHFlg = Convert.ToBoolean(unkobi.KanDmhflg),
                        KanjFax = unkobi.KanjFax.TrimEnd(),
                        KanjJyus1 = unkobi.KanjJyus1,
                        KanjJyus2 = unkobi.KanjJyus2,
                        KanjKeiNo = unkobi.KanjKeiNo.TrimEnd(),
                        KanjMail = unkobi.KanjMail,
                        KanJNm = unkobi.KanJnm,
                        KanjTel = unkobi.KanjTel.TrimEnd(),
                        PlusJin = unkobi.PlusJin.ToString(),
                        TokuiFax = yyksho.TokuiFax.TrimEnd(),
                        TokuiMail = yyksho.TokuiMail,
                        TokuiTanNm = yyksho.TokuiTanNm,
                        TokuiTel = yyksho.TokuiTel.TrimEnd()
                    },
                    ReservationTabData = new ReservationTabData()
                    {
                        IkNm = unkobi.IkNm,
                        HaiSNm = unkobi.HaiSnm,
                        TouNm = unkobi.TouNm,
                        HaiSjyus1 = unkobi.HaiSjyus1,
                        HaiSjyus2 = unkobi.HaiSjyus2,
                        TouJyusyo1 = unkobi.TouJyusyo1,
                        TouJyusyo2 = unkobi.TouJyusyo2,
                        MovementStatus = new TPM_CodeKbCodeSyuData() { CodeKbn = unkobi.UnkoJkbn.ToString() },
                        AcceptanceConditions = new TPM_CodeKbCodeSyuData() { CodeKbn = unkobi.UkeJyKbnCd.ToString() },
                        RainyMeasure = new TPM_CodeKbCodeSyuData() { CodeKbn = unkobi.SijJoKbn1.ToString() },
                        PaymentMethod = new TPM_CodeKbCodeSyuData() { CodeKbn = unkobi.SijJoKbn2.ToString() },
                        MovementForm = new TPM_CodeKbCodeSyuData() { CodeKbn = unkobi.SijJoKbn3.ToString() },
                        GuiderSetting = new TPM_CodeKbCodeSyuData() { CodeKbn = unkobi.SijJoKbn4.ToString() },
                        EstimateSetting = new TPM_CodeKbCodeSyuData() { CodeKbn = unkobi.SijJoKbn5.ToString() },
                        GarageLeaveDate = DateTime.ParseExact(unkobi.SyukoYmd ?? unkobi.HaiSymd, "yyyyMMdd", null),
                        GarageReturnDate = DateTime.ParseExact(unkobi.KikYmd ?? unkobi.TouYmd, "yyyyMMdd", null),
                        GoDate = DateTime.ParseExact(unkobi.SyuPaYmd ?? unkobi.HaiSymd, "yyyyMMdd", null)
                    },
                    UnkRen=unkobi.UnkRen,
                    BikoNm = yyksho.BikoNm,
                    TextOrganizationName = yyksho.YoyaNm,
                    InvoiceDate = DateTime.ParseExact(yyksho.SeiTaiYmd, "yyyyMMdd", new CultureInfo("ja-JP")),
                    InvoiceMonth = DateTime.ParseExact(yyksho.SeiTaiYmd, "yyyyMMdd", new CultureInfo("ja-JP")).ToString("yyyy年MM月"),
                    TaxTypeforBus = TaxTypeListData.taxTypeList.Where(x => x.IdValue == yyksho.ZeiKbn).FirstOrDefault(),
                    TaxRate = yyksho.Zeiritsu.ToString(),
                    TaxBus = yyksho.ZeiRui.ToString(),
                    TaxTypeforGuider = TaxTypeListData.taxTypeList.Where(x => x.IdValue == yyksho.TaxTypeforGuider).FirstOrDefault(),
                    TaxGuider = yyksho.TaxGuider.ToString(),
                    FeeBusRate = yyksho.TesuRitu.ToString(),
                    FeeBus = yyksho.TesuRyoG.ToString(),
                    FeeGuiderRate = yyksho.FeeGuiderRate.ToString(),
                    FeeGuider = yyksho.FeeGuider.ToString(),
                    YykshoUpdYmdTime = yyksho.UpdYmd + yyksho.UpdTime,
                    UnkoUpdYmdTime = unkobi.UpdYmd + unkobi.UpdTime,
                    YykSyuUpdYmdTime = YykSyuUpdYmdTime,
                    HaishaUpdYmdTime = HaishaUpdYmdTime,
                    MishumUpdYmdTime = MishumUpdYmdTime,
                    KakninUpdYmdTime = KakninUpdYmdTime,
                    BookingMaxMinFareFeeCalcUpdYmdTime = BookingMaxMinFareFeeCalcUpdYmdTime,
                    BookingMaxMinFareFeeCalcMeisaiUpdYmdTime = BookingMaxMinFareFeeCalcMeisaiUpdYmdTime,
                    UnkobiFileUpdYmdTime = UnkobiFileUpdYmdTime,
                    BusStartDate = DateTime.ParseExact(unkobi.HaiSymd, "yyyyMMdd", new CultureInfo("ja-JP")),
                    BusStartTime = new BookingInputHelper.MyTime(Convert.ToInt32(unkobi.HaiStime.Substring(0, 2)), Convert.ToInt32(unkobi.HaiStime.Substring(2))),
                    BusEndDate = DateTime.ParseExact(unkobi.DispTouYmd ?? unkobi.TouYmd, "yyyyMMdd", new CultureInfo("ja-JP")),
                    BusEndTime = new BookingInputHelper.MyTime(Convert.ToInt32((unkobi.DispTouChTime ?? unkobi.TouChTime).Substring(0, 2)), Convert.ToInt32((unkobi.DispTouChTime ?? unkobi.TouChTime).Substring(2))),
                    PreDaySetting = Convert.ToBoolean(unkobi.ZenHaFlg),
                    AftDaySetting = Convert.ToBoolean(unkobi.KhakFlg),
                    PassengerTypeData = new PassengerType() { JyoKyakuCdSeq = unkobi.JyoKyakuCdSeq },
                };

                // cancel
                if (yyksho.YoyaSyu == 2)
                {
                    bookingFormData.CancelTickedData = new CancelTickedData()
                    {
                        IsSetDefaultFee = false,
                        CancelStatus = true,
                        ReusedStatus = false,
                        CanceledSettingStaffData = new SettingStaff() { SyainCdSeq = yyksho.CanTanSeq },
                        CancelReason = yyksho.CanRiy,
                        CancelDate = DateTime.ParseExact(yyksho.CanYmd, "yyyyMMdd", null),
                        CancelFeeRate = yyksho.CanRit.ToString(),
                        CancelFee = yyksho.CanUnc.ToString(),
                        CancelTaxType = TaxTypeListData.taxTypeList.FirstOrDefault(t => Convert.ToByte(t.IdValue) == yyksho.CanZkbn) ?? new TaxTypeList(),
                        CancelTaxRate = yyksho.CanSyoR.ToString(),
                        CancelTaxFee = yyksho.CanSyoG.ToString(),
                    };
                    bookingFormData.CancelTickedData.Status = 2;
                }
                //reused
                else if (yyksho.YoyaSyu == 1 && yyksho.CanFuTanSeq != 0)
                {
                    bookingFormData.CancelTickedData = new CancelTickedData()
                    {
                        IsSetDefaultFee = false,
                        CancelStatus = true,
                        ReusedStatus = true,
                        CanceledSettingStaffData = new SettingStaff() { SyainCdSeq = yyksho.CanTanSeq },
                        CancelReason = yyksho.CanRiy,
                        CancelDate = DateTime.ParseExact(yyksho.CanYmd, "yyyyMMdd", null),
                        CancelFeeRate = yyksho.CanRit.ToString(),
                        CancelFee = yyksho.CanUnc.ToString(),
                        CancelTaxType = TaxTypeListData.taxTypeList.FirstOrDefault(t => Convert.ToByte(t.IdValue) == yyksho.CanZkbn) ?? new TaxTypeList(),
                        CancelTaxRate = yyksho.CanSyoR.ToString(),
                        CancelTaxFee = yyksho.CanSyoG.ToString(),
                        ReusedDate = DateTime.ParseExact(yyksho.CanFuYmd, "yyyyMMdd", null),
                        ReusedReason = yyksho.CanFuRiy,
                        ReusedSettingStaffData = new SettingStaff() { SyainCdSeq = yyksho.CanFuTanSeq }
                    };
                    bookingFormData.CancelTickedData.Status = 3;
                }
                else
                {
                    bookingFormData.CancelTickedData.Status = 1;
                }
                // get uriGakKin
                var fee = _context.TkdMishum
                    .Where(m => m.UkeNo == request._ukeNo && m.SiyoKbn == 1 && m.SeiFutSyu == Convert.ToByte(1))
                    .Select(m => new
                    {
                        m.UriGakKin,
                        m.SeiKin
                    }).FirstOrDefault();
                bookingFormData.CancelTickedData.UriGakKin = fee?.UriGakKin ?? 0;
                bookingFormData.CancelTickedData.BusPriceIncludeTaxFee = fee?.SeiKin.ToString();
                //bookingFormData.CancelTickedData.Status = yyksho.YoyaSyu;

                if (yyksho.SirCdSeq != 0 && yyksho.SirSitenCdSeq != 0)
                    bookingFormData.SupervisorTabData.customerComponentTokiStData = new CustomerComponentTokiStData()
                    {
                        TokuiSeq = yyksho.SirCdSeq,
                        SitenCdSeq = yyksho.SirSitenCdSeq
                    };

                if (yyksho.SirCdSeq != 0)
                    bookingFormData.SupervisorTabData.customerComponentTokiskData = new CustomerComponentTokiskData()
                    {
                        TokuiSeq = yyksho.SirCdSeq
                    };
                if (unkobi.IkMapCdSeq > 0) bookingFormData.ReservationTabData.Destination = new DestinationData() { BasyoMapCdSeq = unkobi.IkMapCdSeq };
                if (unkobi.HaiScdSeq > 0) bookingFormData.ReservationTabData.DespatchingPlace = new PlaceData() { HaiSCdSeq = unkobi.HaiScdSeq };
                if (unkobi.TouCdSeq > 0) bookingFormData.ReservationTabData.ArrivePlace = new PlaceData() { HaiSCdSeq = unkobi.TouCdSeq };

                BookingInputHelper.MyTime tempTime = new BookingInputHelper.MyTime();
                BookingInputHelper.MyTime.TryParse(unkobi.SyuKoTime, out tempTime);
                bookingFormData.ReservationTabData.SyuKoTime = tempTime;
                BookingInputHelper.MyTime.TryParse(unkobi.DispSyuPaTime ?? unkobi.SyuPaTime, out tempTime);
                bookingFormData.ReservationTabData.SyuPatime = tempTime;
                BookingInputHelper.MyTime.TryParse(unkobi.DispKikTime ?? unkobi.KikTime, out tempTime);
                bookingFormData.ReservationTabData.KikTime = tempTime;
                bookingFormData.UkeNo = request._ukeNo;
                bookingFormData.CurrentMaxSyaSyuRen = _context.TkdYykSyu.Where(x => x.UkeNo == request._ukeNo).Max(y => y.SyaSyuRen);
                return await Task.FromResult(bookingFormData);
            }
        }
    }
}
