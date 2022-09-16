using HassyaAllrightCloud.Commons.Constants;
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
    public class GetYykshoByUkenoQuery : IRequest<TkdYyksho>
    {
        public string Ukeno { get; set; }
        public BookingFormData BookingData { get; set; }
        public class Handler : IRequestHandler<GetYykshoByUkenoQuery, TkdYyksho>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetYykshoByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetYykshoByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public Task<TkdYyksho> Handle(GetYykshoByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    TkdYyksho yyksho = _context.TkdYyksho.FirstOrDefault(e => e.UkeNo == request.Ukeno && e.SiyoKbn == 1);
                    SetYykshoData(ref yyksho, request.BookingData);
                    return Task.FromResult(yyksho);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            /// <summary>
            /// Set data from booking data to yyksho model
            /// </summary>
            /// <param name="yyksho"></param>
            /// <param name="bookingFormData"></param>
            /// <param name="isUpdate"></param>
            private void SetYykshoData(ref TkdYyksho yyksho, BookingFormData bookingFormData)
            {
                // #8091
                var minusTax = bookingFormData.TaxTypeforBus.IdValue == 2 ? Convert.ToInt32(bookingFormData.TaxBus) : 0;
                var minusGuiTax = bookingFormData.TaxTypeforGuider.IdValue == 2 ? Convert.ToInt32(bookingFormData.TaxGuider) : 0;
                if (yyksho != null && bookingFormData != null)
                {
                    yyksho.HenKai++;
                    //yyksho.KikakuNo = 0;
                    //yyksho.TourCd = "0";
                    //yyksho.KasTourCdSeq = 0;
                    yyksho.SeiEigCdSeq = bookingFormData.SelectedSaleBranch.EigyoCdSeq;
                    yyksho.IraEigCdSeq = bookingFormData.SelectedSaleBranch.EigyoCdSeq;
                    yyksho.InTanCdSeq = new ClaimModel().SyainCdSeq;
                    //yyksho.YoyaKana = "";// 予約書名（カナ）
                    yyksho.TokuiTel = bookingFormData.SupervisorTabData.TokuiTel ?? string.Empty;
                    yyksho.TokuiTanNm = bookingFormData.SupervisorTabData.TokuiTanNm ?? string.Empty;
                    yyksho.TokuiFax = bookingFormData.SupervisorTabData.TokuiFax ?? string.Empty;
                    yyksho.TokuiMail = bookingFormData.SupervisorTabData.TokuiMail ?? string.Empty;
                    yyksho.SeiKyuKbnSeq = bookingFormData.SelectedInvoiceType?.CodeKbnSeq ?? 0;
                    //yyksho.BikoTblSeq = 0;
                    //yyksho.KhinKbn = 1;
                    //yyksho.KaknKais = 0;
                    //yyksho.HaiSkbn = 1;
                    //yyksho.HaiIkbn = 1;
                    //yyksho.GuiWnin = 0;
                    //yyksho.NippoKbn = 1;
                    //yyksho.YouKbn = 1;
                    //yyksho.NyuKinKbn = 1;
                    //yyksho.NcouKbn = 1;
                    //yyksho.SihKbn = 1;
                    //yyksho.ScouKbn = 1;
                    yyksho.SiyoKbn = 1;
                    //yyksho.UkeYmd = DateTime.Now.ToString("yyyyMMdd");
                    //yyksho.YoyaSyu = 1; //1:予約 2:キャンセル
                    yyksho.YoyaKbnSeq = (int)bookingFormData.CurrentBookingType.YoyaKbnSeq;
                    yyksho.UkeEigCdSeq = (int)bookingFormData.SelectedSaleBranch.EigyoCdSeq;
                    yyksho.EigTanCdSeq = (int)bookingFormData.SelectedStaff.SyainCdSeq;
                    yyksho.YoyaNm = (String)bookingFormData.TextOrganizationName; // 予約書名
                    yyksho.TokuiSeq = (int)bookingFormData.customerComponentTokiStData.TokuiSeq; // 得意先ＳＥＱ
                    yyksho.SitenCdSeq = (int)bookingFormData.customerComponentTokiStData.SitenCdSeq; // 支店コードＳＥＱ
                    yyksho.SirCdSeq = bookingFormData.SupervisorTabData.customerComponentTokiStData == null ? 0 : bookingFormData.SupervisorTabData.customerComponentTokiStData.TokuiSeq; // 仕入先コードＳＥＱ
                    yyksho.SirSitenCdSeq = bookingFormData.SupervisorTabData.customerComponentTokiStData == null ? 0 : bookingFormData.SupervisorTabData.customerComponentTokiStData.SitenCdSeq; // 仕入先支店コードＳＥＱ
                    yyksho.UntKin = bookingFormData.VehicleGridDataList.Sum(t => int.Parse(t.BusPrice)) - minusTax; // バス代
                    yyksho.ZeiKbn = (byte)(int)bookingFormData.TaxTypeforBus.IdValue; // 消費税タイプ（バス用）
                    yyksho.Zeiritsu = decimal.Parse(bookingFormData.TaxRate); // 消費税率（バス用）
                    yyksho.ZeiRui = Convert.ToInt32(bookingFormData.TaxBus); // バス代税（バス用）
                    yyksho.TaxTypeforGuider = (byte)(int)bookingFormData.TaxTypeforGuider.IdValue; // 消費税タイプ（ガイド用）
                    yyksho.TaxGuider = Convert.ToInt32(bookingFormData.TaxGuider); // ガイド税（ガイド用）
                    yyksho.TesuRitu = decimal.Parse(bookingFormData.FeeBusRate); // 手数料率（バス用）
                    yyksho.TesuRyoG = Convert.ToInt32(bookingFormData.FeeBus); // 手数料税（バス用）
                    yyksho.FeeGuiderRate = decimal.Parse(bookingFormData.FeeGuiderRate); // 手数料率（ガイド用）
                    yyksho.FeeGuider = Convert.ToInt32(bookingFormData.FeeGuider);// 手数料税（ガイド用）
                    yyksho.SeikYm = (string)bookingFormData.InvoiceDate.ToString("yyyyMM"); // 請求年月
                    yyksho.SeiTaiYmd = (string)bookingFormData.InvoiceDate.ToString("yyyyMMdd"); // 請求年月日
                    yyksho.Kskbn = 1; // 仮車区分 1:未仮車 2:仮車済 3:一部済
                    yyksho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    yyksho.UpdTime = DateTime.Now.ToString("HHmmss");
                    yyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    yyksho.UpdPrgId = "KJ1000";
                    yyksho.GuitKin = bookingFormData.TotalGuiderFee - minusGuiTax;
                    yyksho.BikoNm = bookingFormData.BikoNm;

                    //Yoyasyu logics
                    if (bookingFormData.BookingStatus.CodeKbn == Constants.BookingStatus)
                    {
                        if (yyksho.YoyaSyu == 3)
                        {
                            yyksho.YoyaSyu = 1;
                        }
                        else if (yyksho.YoyaSyu == 4)
                        {
                            yyksho.YoyaSyu = 2;
                        }
                    }
                    else if (bookingFormData.BookingStatus.CodeKbn == Constants.EstimateStatus)
                    {
                        if (yyksho.YoyaSyu == 1)
                        {
                            yyksho.YoyaSyu = 3;
                        }
                        if (yyksho.YoyaSyu == 2)
                        {
                            yyksho.YoyaSyu = 4;
                        }
                    }
                }
            }
        }
    }
}
