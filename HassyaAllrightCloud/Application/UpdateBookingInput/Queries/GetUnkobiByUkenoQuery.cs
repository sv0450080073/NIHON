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
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Queries
{
    public class GetUnkobiByUkenoQuery : IRequest<TkdUnkobi>
    {
        public string Ukeno { get; set; }
        public BookingFormData BookingData { get; set; }
        public class Handler : IRequestHandler<GetUnkobiByUkenoQuery, TkdUnkobi>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetUnkobiByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetUnkobiByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public Task<TkdUnkobi> Handle(GetUnkobiByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    TkdUnkobi unkobi = _context.TkdUnkobi.FirstOrDefault(e => e.UkeNo == request.Ukeno && e.UnkRen == request.BookingData.UnkRen && e.SiyoKbn == 1);
                    if (unkobi != null)
                    {
                        SetUnkobiData(ref unkobi, request.BookingData);
                    }
                    return Task.FromResult(unkobi);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            /// <summary>
            /// Set data from booking data to unkobi model
            /// </summary>
            /// <param name="unkobi"></param>
            /// <param name="bookingFormData"></param>
            /// <param name="isUpdate"></param>
            private void SetUnkobiData(ref TkdUnkobi unkobi, BookingFormData bookingFormData)
            {
                if (unkobi != null && bookingFormData != null)
                {
                    TkmKasSet tkmKasSet = this._context.TkmKasSet.Where(t => t.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).FirstOrDefault();
                    short UriKbn = tkmKasSet.UriKbn;
                    unkobi.HenKai++;
                    unkobi.KanjJyus2 = bookingFormData.SupervisorTabData.KanjJyus2 ?? string.Empty;
                    //unkobi.DanTaKana = "";
                    unkobi.HaiSjyus1 = bookingFormData.ReservationTabData.HaiSjyus1 ?? string.Empty;
                    unkobi.HaiSjyus2 = bookingFormData.ReservationTabData.HaiSjyus2 ?? string.Empty;
                    //unkobi.HaiSkouKcdSeq = 0;
                    //unkobi.HaiSbinCdSeq = 0;
                    //unkobi.HaiSsetTime = "";
                    unkobi.TouJyusyo1 = bookingFormData.ReservationTabData.TouJyusyo1 ?? string.Empty;
                    unkobi.TouJyusyo2 = bookingFormData.ReservationTabData.TouJyusyo2 ?? string.Empty;
                    //unkobi.TouKouKcdSeq = 0;
                    //unkobi.TouBinCdSeq = 0;
                    //unkobi.TouSetTime = "";
                    //unkobi.AreaMapSeq = 0;
                    //unkobi.AreaNm = "";
                    //unkobi.HasMapCdSeq = 0;
                    //unkobi.HasNm = "";
                    unkobi.JyoKyakuCdSeq = bookingFormData.PassengerTypeData == null ? 0 : bookingFormData.PassengerTypeData.JyoKyakuCdSeq;
                    unkobi.OthJinKbn1 = 99;
                    unkobi.OthJin1 = 0;
                    unkobi.OthJinKbn2 = 99;
                    unkobi.OthJin2 = 0;
                    //unkobi.Kskbn = 1; // 仮車区分 1:未仮車 2:仮車済 3:一部済
                    //unkobi.KhinKbn = 1;
                    //unkobi.HaiSkbn = 1;
                    //unkobi.HaiIkbn = 1;
                    //unkobi.GuiWnin = 0;
                    //unkobi.NippoKbn = 1;
                    //unkobi.YouKbn = 1;
                    //unkobi.RotCdSeq = 0;
                    //unkobi.BikoTblSeq = 0;
                    unkobi.SiyoKbn = 1;
                    //unkobi.UnkRen = 1; // 運行日連番

                    unkobi.DanTaNm = (String)bookingFormData.TextOrganizationName; // 団体名
                    unkobi.DrvJin = Convert.ToInt16(bookingFormData.VehicleGridDataList.Sum(t => short.Parse(t.DriverNum))); // 運転手数
                    unkobi.GuiSu = Convert.ToInt16(bookingFormData.VehicleGridDataList.Sum(t => short.Parse(t.GuiderNum))); // ガイド数
                    unkobi.ZenHaFlg = Convert.ToByte(bookingFormData.PreDaySetting); // 前泊フラグ
                    unkobi.KhakFlg = Convert.ToByte(bookingFormData.AftDaySetting); // 後泊フラグ

                    BookingInputHelper.MyDate busStartDay = new BookingInputHelper.MyDate(bookingFormData.BusStartDate, bookingFormData.BusStartTime);
                    BookingInputHelper.MyDate busEndDay = new BookingInputHelper.MyDate(bookingFormData.BusEndDate, bookingFormData.BusEndTime);

                    unkobi.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    unkobi.UpdTime = DateTime.Now.ToString("HHmmss");
                    unkobi.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    unkobi.UpdPrgId = "KJ1000";

                    unkobi.KanJnm = bookingFormData.SupervisorTabData.KanJNm ?? string.Empty;
                    unkobi.KanjJyus1 = bookingFormData.SupervisorTabData.KanjJyus1 ?? string.Empty;
                    unkobi.KanjTel = bookingFormData.SupervisorTabData.KanjTel ?? string.Empty;
                    unkobi.KanjFax = bookingFormData.SupervisorTabData.KanjFax ?? string.Empty;
                    unkobi.KanjKeiNo = bookingFormData.SupervisorTabData.KanjKeiNo ?? string.Empty;
                    unkobi.KanjMail = bookingFormData.SupervisorTabData.KanjMail ?? string.Empty;
                    unkobi.KanDmhflg = Convert.ToByte(bookingFormData.SupervisorTabData.KanDMHFlg);
                    unkobi.JyoSyaJin = Convert.ToInt16(bookingFormData.SupervisorTabData.JyoSyaJin);
                    unkobi.PlusJin = Convert.ToInt16(bookingFormData.SupervisorTabData.PlusJin);

                    unkobi.IkMapCdSeq = bookingFormData.ReservationTabData.Destination == null ? 0 : bookingFormData.ReservationTabData.Destination.BasyoMapCdSeq;
                    unkobi.IkNm = bookingFormData.ReservationTabData.IkNm ?? "";
                    unkobi.HaiScdSeq = bookingFormData.ReservationTabData.DespatchingPlace == null ? 0 : bookingFormData.ReservationTabData.DespatchingPlace.HaiSCdSeq;
                    unkobi.HaiSnm = bookingFormData.ReservationTabData.HaiSNm ?? "";
                    unkobi.TouCdSeq = bookingFormData.ReservationTabData.ArrivePlace == null ? 0 : bookingFormData.ReservationTabData.ArrivePlace.HaiSCdSeq;
                    unkobi.TouNm = bookingFormData.ReservationTabData.TouNm ?? "";
                    unkobi.UnkoJkbn = Convert.ToByte(bookingFormData.ReservationTabData.MovementStatus.CodeKbn);
                    unkobi.UkeJyKbnCd = Convert.ToByte(bookingFormData.ReservationTabData.AcceptanceConditions.CodeKbn);
                    unkobi.SijJoKbn1 = Convert.ToByte(bookingFormData.ReservationTabData.RainyMeasure.CodeKbn);
                    unkobi.SijJoKbn2 = Convert.ToByte(bookingFormData.ReservationTabData.PaymentMethod.CodeKbn);
                    unkobi.SijJoKbn3 = Convert.ToByte(bookingFormData.ReservationTabData.MovementForm.CodeKbn);
                    unkobi.SijJoKbn4 = Convert.ToByte(bookingFormData.ReservationTabData.GuiderSetting.CodeKbn);
                    unkobi.SijJoKbn5 = Convert.ToByte(bookingFormData.ReservationTabData.EstimateSetting.CodeKbn);
                    unkobi.BikoNm = string.Empty;

                    unkobi.HaiSymd = bookingFormData.BusStartDate.ToString("yyyyMMdd"); // 配車年月日
                    unkobi.HaiStime = bookingFormData.BusStartTime.ToStringWithoutDelimiter(); // 配車時間
                    unkobi.SyukoYmd = bookingFormData.ReservationTabData.GarageLeaveDate.ToString("yyyyMMdd");
                    unkobi.SyuKoTime = bookingFormData.ReservationTabData.SyuKoTime.ToStringWithoutDelimiter(); // 出庫時間
                    unkobi.SyuPaYmd = bookingFormData.ReservationTabData.GoDate.ToString("yyyyMMdd");
                    unkobi.SyuPaTime = bookingFormData.ReservationTabData.SyuPatime.ToStringWithoutDelimiter();
                    unkobi.DispSyuPaYmd = bookingFormData.ReservationTabData.GoDate.ToString("yyyyMMdd");
                    unkobi.DispSyuPaTime = bookingFormData.ReservationTabData.SyuPatime.ToStringWithoutDelimiter();

                    unkobi.DispTouYmd = bookingFormData.BusEndDate.ToString("yyyyMMdd");
                    unkobi.DispTouChTime = bookingFormData.BusEndTime.ToStringWithoutDelimiter();
                    unkobi.DispKikYmd = bookingFormData.ReservationTabData.GarageReturnDate.ToString("yyyyMMdd");
                    unkobi.DispKikTime = bookingFormData.ReservationTabData.KikTime.ToStringWithoutDelimiter();

                    var touDate = new BookingInputHelper.MyDate(bookingFormData.BusEndDate, bookingFormData.BusEndTime);
                    unkobi.TouYmd = touDate.ConvertedDate.ToString("yyyyMMdd"); // 到着年月日
                    unkobi.TouChTime = touDate.ConvertedTime.ToStringWithoutDelimiter();

                    // 売上年月日
                    if (UriKbn == 1)
                    {
                        unkobi.UriYmd = unkobi.HaiSymd; // 配車日時
                    }
                    else if (UriKbn == 2)
                    {
                        unkobi.UriYmd = unkobi.TouYmd; // 終日予定
                    }

                    unkobi.KikYmd = bookingFormData.ReservationTabData.GarageReturnDate.ToString("yyyyMMdd");
                    unkobi.KikTime = bookingFormData.ReservationTabData.KikTime.ToStringWithoutDelimiter();

                    if (bookingFormData.CustomData != null && bookingFormData.CustomData.Count() > 0)
                    {
                        foreach (var fieldValue in bookingFormData.CustomData)
                        {
                            _context.Entry(unkobi).Property($"CustomItems{fieldValue.Key}").CurrentValue = fieldValue.Value;
                        }
                    }

                    unkobi.UnitPriceIndex = decimal.Parse(bookingFormData.GetAverageUnitPriceIndex());
                }
            }
        }
    }
}
