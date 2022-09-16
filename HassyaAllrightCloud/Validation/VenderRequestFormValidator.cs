using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class VenderRequestFormValidator : AbstractValidator<VenderRequestFormData>
    {
        public VenderRequestFormValidator()
        {
            When(_ => (!string.IsNullOrEmpty(_.UkeCdFrom) && !string.IsNullOrEmpty(_.UkeCdTo)), () =>
            {
                RuleFor(_ => _.UkeCdFrom).Must((model, uke) => int.Parse(uke) <= int.Parse(model.UkeCdTo))
                                   .WithMessage(Constants.ErrorMessage.VR_UkeCdIsWrong);
            });

            RuleFor(_ => _.StartDate).Must((obj, to) => obj.StartDate.CompareTo(obj.EndDate) <= 0)
                                   .WithMessage(Constants.ErrorMessage.VR_DateIsWrong);

            When(_ => _.SelectedGyosyaFrom != null && _.SelectedGyosyaTo != null, () =>
            {
                RuleFor(_ => _.SelectedGyosyaFrom).Must((obj, value) => obj.SelectedGyosyaTo.GyosyaCd >= value.GyosyaCd)
                                                  .WithMessage(Constants.ErrorMessage.VR_CustomerIsWrong);
                RuleFor(_ => _.SelectedGyosyaTo).Must((obj, value) => obj.SelectedGyosyaFrom.GyosyaCd <= value.GyosyaCd)
                                                .WithMessage(Constants.ErrorMessage.VR_CustomerIsWrong);
            });

            When(_ => _.SelectedGyosyaFrom != null && _.SelectedGyosyaTo != null && _.SelectedTokiskFrom != null && _.SelectedTokiskTo != null, () =>
            {
                RuleFor(_ => _.SelectedTokiskFrom).Must((obj, value) => obj.SelectedGyosyaFrom.GyosyaCd != obj.SelectedGyosyaTo.GyosyaCd || obj.SelectedTokiskTo.TokuiCd >= value.TokuiCd)
                                                  .WithMessage(Constants.ErrorMessage.VR_CustomerIsWrong);
                RuleFor(_ => _.SelectedTokiskTo).Must((obj, value) => obj.SelectedGyosyaFrom.GyosyaCd != obj.SelectedGyosyaTo.GyosyaCd || obj.SelectedTokiskFrom.TokuiCd <= value.TokuiCd)
                                                .WithMessage(Constants.ErrorMessage.VR_CustomerIsWrong);
            });

            When(_ => _.SelectedGyosyaFrom != null && _.SelectedGyosyaTo != null && _.SelectedTokiskFrom != null && _.SelectedTokiskTo != null && _.SelectedTokiStFrom != null && _.SelectedTokiStTo != null, () =>
            {
                RuleFor(_ => _.SelectedTokiStFrom).Must((obj, value) => obj.SelectedGyosyaFrom.GyosyaCd != obj.SelectedGyosyaTo.GyosyaCd || obj.SelectedTokiskFrom.TokuiCd != obj.SelectedTokiskTo.TokuiCd || obj.SelectedTokiStTo.SitenCd >= value.SitenCd)
                                                  .WithMessage(Constants.ErrorMessage.VR_CustomerIsWrong);
                RuleFor(_ => _.SelectedTokiStTo).Must((obj, value) => obj.SelectedGyosyaFrom.GyosyaCd != obj.SelectedGyosyaTo.GyosyaCd || obj.SelectedTokiskFrom.TokuiCd != obj.SelectedTokiskTo.TokuiCd || obj.SelectedTokiStFrom.SitenCd <= value.SitenCd)
                                                .WithMessage(Constants.ErrorMessage.VR_CustomerIsWrong);
            });

            When(_ => _.BookingTypeStart != null && _.BookingTypeEnd != null, () => {
                RuleFor(_ => _.BookingTypeStart).Must((obj, value) => obj.BookingTypeEnd.YoyaKbn >= value.YoyaKbn)
                                                .WithMessage(Constants.ErrorMessage.VR_ReservationIsWrong);
                RuleFor(_ => _.BookingTypeEnd).Must((obj, value) => obj.BookingTypeStart.YoyaKbn <= value.YoyaKbn)
                                              .WithMessage(Constants.ErrorMessage.VR_ReservationIsWrong);
            });
        } 
    }
}
