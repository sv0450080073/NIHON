using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using System;

namespace HassyaAllrightCloud.Validation
{
    public class QuotationWithJourneyValidator : AbstractValidator<SimpleQuotationData>
    {
        public QuotationWithJourneyValidator()
        {
            RuleFor(_ => _.StartArrivalDate)
            .Must((model, date) => 
            {
                var result = DateTimeUkeCdSettingIsValid(model);
                return !result.AllIsEmpty;
            })
            .WithMessage("BI_T013");

            When(model => (model.StartArrivalDate.HasValue || model.EndArrivalDate.HasValue),
            () =>
            {
                RuleFor(_ => _.StartArrivalDate).Must((model, date) => model.StartArrivalDate <= model.EndArrivalDate)
                .WithMessage(Constants.ErrorMessage.QWJ_DateEndGreaterThanStart);
            });

            RuleFor(_ => _.StartPickupDate)
           .Must((model, date) =>
           {
               var result = DateTimeUkeCdSettingIsValid(model);
               return !result.AllIsEmpty;
           })
            .WithMessage("BI_T013");
            When(model => (model.StartPickupDate.HasValue || model.EndPickupDate.HasValue),
            () =>
            {
                RuleFor(_ => _.StartPickupDate).Must((model, date) => model.StartPickupDate <= model.EndPickupDate).WithMessage(Constants.ErrorMessage.QWJ_DateEndGreaterThanStart);
            });

            // Booking types
            When(model => model.YoyakuFrom != null
                         && model.YoyakuTo != null
                         && model.YoyakuFrom.YoyaKbnSeq > model.YoyakuTo.YoyaKbnSeq,
             () =>
             {
                RuleFor(_ => _.YoyakuFrom).Null().WithMessage(Constants.ErrorMessage.QWJ_BookingTypeEndGreaterThanStart);
                RuleFor(_ => _.YoyakuTo).Null().WithMessage(Constants.ErrorMessage.QWJ_BookingTypeEndGreaterThanStart);
            });

            // UkeCd
            RuleFor(_ => _.UkeCdFrom)
           .Must((model, date) =>
           {
               var result = DateTimeUkeCdSettingIsValid(model);
               return !result.AllIsEmpty;
           })
            .WithMessage("BI_T013");
            When(model => (!string.IsNullOrEmpty(model.UkeCdFrom)
                            || !string.IsNullOrEmpty(model.UkeCdTo)), 
            () =>
            {
                RuleFor(_ => _.UkeCdFrom).Must(
                    (model, ukeCd) => {
                        return 
                           (double.TryParse(model.UkeCdFrom, out double ukeFrom))
                           && (double.TryParse(model.UkeCdTo, out double ukeTo))
                           && ukeFrom <= ukeTo;
                    })
                .WithMessage(Constants.ErrorMessage.QWJ_UkeCdEndGreaterThanStart);
            });
            RuleFor(_ => _.UkeCdFrom).MaximumLength(10);//.WithMessage(Constants.ErrorMessage.QWJ_UkeCdEndGreaterThanStart);
            RuleFor(_ => _.UkeCdTo).MaximumLength(10);//.WithMessage(Constants.ErrorMessage.QWJ_UkeCdEndGreaterThanStart);

            // Customer
            //When(model => model.CustomerStart != null
            //            && model.CustomerEnd != null
            //            && !CommonHelper.IsBetween((0, 0), (model.CustomerEnd.TokuiCd, model.CustomerEnd.SitenCd), (model.CustomerStart.TokuiCd, model.CustomerStart.SitenCd)), () =>
            //{
            //    RuleFor(_ => _.CustomerStart).Null().WithMessage(Constants.ErrorMessage.QWJ_CustomerEndGreaterThanStart);
            //    RuleFor(_ => _.CustomerEnd).Null().WithMessage(Constants.ErrorMessage.QWJ_CustomerEndGreaterThanStart);
            //});
            // 仕入先
            When(x => (x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && (x.GyosyaShiireSakiFrom.GyosyaCd > x.GyosyaShiireSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.QWJ_CustomerEndGreaterThanStart);
                RuleFor(m => m.GyosyaShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.QWJ_CustomerEndGreaterThanStart);
            });
            When(x => (x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && x.TokiskShiireSakiFrom != null && x.TokiskShiireSakiTo != null && (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd > x.TokiskShiireSakiTo.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.TokiskShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.QWJ_CustomerEndGreaterThanStart);
                    RuleFor(m => m.TokiskShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.QWJ_CustomerEndGreaterThanStart);
                });
            When(x => ((x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && x.TokiskShiireSakiFrom != null && x.TokiskShiireSakiTo != null && x.TokiStShiireSakiFrom != null && x.TokiStShiireSakiTo != null)
                    && ((x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd > x.TokiStShiireSakiTo.SitenCd)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq > x.GyosyaShiireSakiTo.GyosyaCdSeq)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq == x.GyosyaShiireSakiTo.GyosyaCdSeq && x.TokiskShiireSakiFrom.TokuiSeq > x.TokiskShiireSakiTo.TokuiSeq)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq == x.GyosyaShiireSakiTo.GyosyaCdSeq && x.TokiskShiireSakiFrom.TokuiSeq == x.TokiskShiireSakiTo.TokuiSeq && x.TokiStShiireSakiFrom.SitenCdSeq > x.TokiStShiireSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.QWJ_CustomerEndGreaterThanStart);
                        RuleFor(m => m.TokiStShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.QWJ_CustomerEndGreaterThanStart);
                    });

            // Branch
            When(model => model.BranchStart != null
                        && model.BranchEnd != null
                        && !CommonHelper.IsBetween((0, 0), (model.BranchEnd.CompanyCd, model.BranchEnd.EigyoCd), (model.BranchStart.CompanyCd, model.BranchStart.EigyoCd)), () =>
            {
                RuleFor(_ => _.BranchStart).Null().WithMessage(Constants.ErrorMessage.QWJ_BranchEndGreaterThanStart);
                RuleFor(_ => _.BranchEnd).Null().WithMessage(Constants.ErrorMessage.QWJ_BranchEndGreaterThanStart);
            });
        }

        private static (bool IsPassed, bool AllIsEmpty ) DateTimeUkeCdSettingIsValid(SimpleQuotationData model)
        {
            return DateTimeUkeCdSettingIsValid(model.StartArrivalDate, model.EndArrivalDate,
                            model.StartPickupDate, model.EndPickupDate,
                            model.UkeCdFrom, model.UkeCdTo);
        }

        private static (bool IsPassed, bool AllIsEmpty) DateTimeUkeCdSettingIsValid(DateTime? startArrivalDate, DateTime? endArrivalDate
           , DateTime? startPickupDate, DateTime? endPickupDate
           , String ukeCdFrom, String ukeCdTo)
        {
            bool arrivalDateHadOrEmptyData = !(startArrivalDate.HasValue ^ endArrivalDate.HasValue);
            bool arrivalDateIsEmpty = arrivalDateHadOrEmptyData ? !startArrivalDate.HasValue && !endArrivalDate.HasValue : false;

            bool pickupDateHadOrEmptyData = !(startPickupDate.HasValue ^ endPickupDate.HasValue);
            bool pickupDateIsEmpty = pickupDateHadOrEmptyData ? !startPickupDate.HasValue && !endPickupDate.HasValue : false;

            bool ukeCdHadOrEmptyData = !(!string.IsNullOrWhiteSpace(ukeCdFrom) ^ !string.IsNullOrWhiteSpace(ukeCdTo));
            bool ukeCdIsEmpty = ukeCdHadOrEmptyData ? string.IsNullOrWhiteSpace(ukeCdFrom) && string.IsNullOrWhiteSpace(ukeCdTo) : false;

            var isPassed = ((arrivalDateHadOrEmptyData || arrivalDateIsEmpty)
                && (pickupDateHadOrEmptyData || pickupDateIsEmpty)
                && (ukeCdHadOrEmptyData || ukeCdIsEmpty)
                && !(arrivalDateIsEmpty && pickupDateIsEmpty && ukeCdIsEmpty));

            var allIsEmpty = arrivalDateIsEmpty && pickupDateIsEmpty && ukeCdIsEmpty;

            return (isPassed, allIsEmpty);
        }
    }
}
