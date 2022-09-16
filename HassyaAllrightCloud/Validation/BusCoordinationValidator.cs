using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BusCoordinationValidator : AbstractValidator<BusCoordinationSearchParam>
    {
        public BusCoordinationValidator()
        {
            //startdate
            When(x => (x.StartDate > x.EndDate), () =>
            {
                RuleFor(m => m.EndDate).Empty()
                .WithMessage(Constants.ErrorMessage.StartDateTimeGreaterThanEndDateTimeReport);
            });
            //Customer
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd > x.GyosyaTokuiSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CustomerToGreaterThanCustomerFromReport);
                RuleFor(m => m.GyosyaTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.CustomerToGreaterThanCustomerFromReport);
            });
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd > x.TokiskTokuiSakiTo.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.TokiskTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CustomerToGreaterThanCustomerFromReport);
                    RuleFor(m => m.TokiskTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.CustomerToGreaterThanCustomerFromReport);
                });
            When(x => ((x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && x.TokiStTokuiSakiFrom != null && x.TokiStTokuiSakiTo != null)
                    && ((x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd > x.TokiStTokuiSakiTo.SitenCd)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq > x.GyosyaTokuiSakiTo.GyosyaCdSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq > x.TokiskTokuiSakiTo.TokuiSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq == x.TokiskTokuiSakiTo.TokuiSeq && x.TokiStTokuiSakiFrom.SitenCdSeq > x.TokiStTokuiSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CustomerToGreaterThanCustomerFromReport);
                        RuleFor(m => m.TokiStTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.CustomerToGreaterThanCustomerFromReport);
                    });

            //Customer01// 仕入先 
            When(x => (x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && (x.GyosyaShiireSakiFrom.GyosyaCd > x.GyosyaShiireSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CustomerTo01GreaterThanCustomerFrom01Report);
                RuleFor(m => m.GyosyaShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.CustomerTo01GreaterThanCustomerFrom01Report);
            });
            When(x => (x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && x.TokiskShiireSakiFrom != null && x.TokiskShiireSakiTo != null && (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd > x.TokiskShiireSakiTo.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.TokiskShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CustomerTo01GreaterThanCustomerFrom01Report);
                    RuleFor(m => m.TokiskShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.CustomerTo01GreaterThanCustomerFrom01Report);
                });
            When(x => ((x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && x.TokiskShiireSakiFrom != null && x.TokiskShiireSakiTo != null && x.TokiStShiireSakiFrom != null && x.TokiStShiireSakiTo != null)
                    && ((x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd > x.TokiStShiireSakiTo.SitenCd)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq > x.GyosyaShiireSakiTo.GyosyaCdSeq)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq == x.GyosyaShiireSakiTo.GyosyaCdSeq && x.TokiskShiireSakiFrom.TokuiSeq > x.TokiskShiireSakiTo.TokuiSeq)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq == x.GyosyaShiireSakiTo.GyosyaCdSeq && x.TokiskShiireSakiFrom.TokuiSeq == x.TokiskShiireSakiTo.TokuiSeq && x.TokiStShiireSakiFrom.SitenCdSeq > x.TokiStShiireSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CustomerTo01GreaterThanCustomerFrom01Report);
                        RuleFor(m => m.TokiStShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.CustomerTo01GreaterThanCustomerFrom01Report);
                    });
            //Booking
            When(v => long.Parse(v.BookingFrom == "" ? "0" : v.BookingFrom) > long.Parse(v.BookingTo == "" ? "0" : v.BookingTo), () =>
            {
                RuleFor(v => v.BookingTo).Empty()
                .WithMessage(Constants.ErrorMessage.BookingToGreaterThanBookingFromReport);
                RuleFor(v => v.BookingFrom).Empty()
                .WithMessage(Constants.ErrorMessage.BookingToGreaterThanBookingFromReport);
            });

            //booking type
            When(x => x.YoyakuFrom != null && x.YoyakuTo != null && x.YoyakuFrom.YoyaKbn > x.YoyakuTo.YoyaKbn, () =>
            {
                RuleFor(m => m.YoyakuFrom).Empty().WithMessage(Constants.ErrorMessage.BookingTypeToGreaterThanBookingTypeFromReportBusCoordina);
                RuleFor(m => m.YoyakuTo).Empty().WithMessage(Constants.ErrorMessage.BookingTypeToGreaterThanBookingTypeFromReportBusCoordina);
            });
        }
    }
}