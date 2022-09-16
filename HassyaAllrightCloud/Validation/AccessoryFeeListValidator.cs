using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Validation
{
    public class AccessoryFeeListValidator : AbstractValidator<AccessoryFeeListData>
    {
        public AccessoryFeeListValidator()
        {
            // RuleFor(_ => _.CustomerStart).Must(customer => customer != null).WithMessage(Constants.ErrorMessage.AFL_CustomerEmpty);
            // RuleFor(_ => _.CustomerEnd).Must(customer => customer != null).WithMessage(Constants.ErrorMessage.AFL_CustomerEmpty);
            // RuleFor(_ => _.Company).Must(company => company != null).WithMessage(Constants.ErrorMessage.AFL_CompanyEmpty);
            // RuleFor(_ => _.BranchStart).Must(branch => branch != null).WithMessage(Constants.ErrorMessage.AFL_BranchEmpty);
            // RuleFor(_ => _.BranchEnd).Must(branch => branch != null).WithMessage(Constants.ErrorMessage.AFL_BranchEmpty);
            // RuleFor(_ => _.FutaiStart).Must(branch => branch != null).WithMessage(Constants.ErrorMessage.AFL_FutaiEmpty);
            // RuleFor(_ => _.FutaiEnd).Must(branch => branch != null).WithMessage(Constants.ErrorMessage.AFL_FutaiEmpty);
            //RuleFor(_ => _.BookingTypes).Must(type => type != null).WithMessage(Constants.ErrorMessage.AFL_BookingTypeEmpty);

            RuleFor(_ => _.StartDate).Must((model, date) => date.CompareTo(model.EndDate) <= 0)
                                     .WithMessage(Constants.ErrorMessage.AFL_EndDateEarlyThanStartDate);
            When(_ => _.SelectedGyosyaStart != null && _.SelectedGyosyaEnd != null, () =>
            {
                RuleFor(_ => _.SelectedGyosyaStart).Must((obj, value) => obj.SelectedGyosyaEnd.GyosyaCd >= value.GyosyaCd)
                                                   .WithMessage(Constants.ErrorMessage.AFL_CustomerFromLargerThanCustomerTo);
                RuleFor(_ => _.SelectedGyosyaEnd).Must((obj, value) => obj.SelectedGyosyaStart.GyosyaCd <= value.GyosyaCd)
                                                 .WithMessage(Constants.ErrorMessage.AFL_CustomerFromLargerThanCustomerTo);
            });

            When(_ => _.SelectedGyosyaStart != null && _.SelectedGyosyaEnd != null && _.SelectedGyosyaStart.GyosyaCd == _.SelectedGyosyaEnd.GyosyaCd && _.SelectedTokiskStart != null && _.SelectedTokiskEnd != null, () =>
            {
                RuleFor(_ => _.SelectedTokiskStart).Must((obj, value) => obj.SelectedTokiskEnd.TokuiCd >= value.TokuiCd)
                                                   .WithMessage(Constants.ErrorMessage.AFL_CustomerFromLargerThanCustomerTo);
                RuleFor(_ => _.SelectedTokiskEnd).Must((obj, value) => obj.SelectedTokiskStart.TokuiCd <= value.TokuiCd)
                                                 .WithMessage(Constants.ErrorMessage.AFL_CustomerFromLargerThanCustomerTo);
            });

            When(_ => _.SelectedGyosyaStart != null && _.SelectedGyosyaEnd != null && _.SelectedGyosyaStart.GyosyaCd == _.SelectedGyosyaEnd.GyosyaCd && _.SelectedTokiskStart != null && _.SelectedTokiskEnd != null && _.SelectedTokiskStart.TokuiCd == _.SelectedTokiskEnd.TokuiCd && _.SelectedTokistStart != null && _.SelectedTokistEnd != null, () =>
            {
                RuleFor(_ => _.SelectedTokistStart).Must((obj, value) => obj.SelectedTokistEnd.SitenCd >= value.SitenCd)
                                                   .WithMessage(Constants.ErrorMessage.AFL_CustomerFromLargerThanCustomerTo);
                RuleFor(_ => _.SelectedTokistEnd).Must((obj, value) => obj.SelectedTokistStart.SitenCd <= value.SitenCd)
                                                 .WithMessage(Constants.ErrorMessage.AFL_CustomerFromLargerThanCustomerTo);
            });

            When(_ => _.SelectedGyosyaStart != null && _.SelectedGyosyaEnd != null, () =>
            {
                RuleFor(_ => _.SelectedGyosyaStart).Must((obj, value) => obj.SelectedGyosyaEnd.GyosyaCd >= value.GyosyaCd)
                                                   .WithMessage(Constants.ErrorMessage.AFL_CustomerFromLargerThanCustomerTo);
                RuleFor(_ => _.SelectedGyosyaEnd).Must((obj, value) => obj.SelectedGyosyaStart.GyosyaCd <= value.GyosyaCd)
                                                 .WithMessage(Constants.ErrorMessage.AFL_CustomerFromLargerThanCustomerTo);
            });

            When(_ => _.BookingTypeStart != null && _.BookingTypeEnd != null, () => {
                RuleFor(_ => _.BookingTypeStart).Must((obj, value) => obj.BookingTypeEnd.YoyaKbn >= value.YoyaKbn)
                                                .WithMessage(Constants.ErrorMessage.AFL_BookingTypeFromLargerThanBookingTypeTo);
                RuleFor(_ => _.BookingTypeEnd).Must((obj, value) => obj.BookingTypeStart.YoyaKbn <= value.YoyaKbn)
                                              .WithMessage(Constants.ErrorMessage.AFL_BookingTypeFromLargerThanBookingTypeTo);
            });

            When(_ => _.BranchStart != null && _.BranchEnd != null, () =>
            {
                RuleFor(_ => _.BranchStart).Must((model, branch) => CommonHelper.IsBetween((0, 0), (0, model.BranchEnd.EigyoCd), (0, branch.EigyoCd)))
                                          .WithMessage(Constants.ErrorMessage.AFL_BranchFromLargerThanBranchTo);
            });

            When(_ => (!string.IsNullOrEmpty(_.UkeCdFrom) && !string.IsNullOrEmpty(_.UkeCdTo)), () =>
            {
                RuleFor(_ => _.UkeCdTo).Must((model, uke) => int.Parse(uke) >= int.Parse(model.UkeCdFrom))
                                   .WithMessage(Constants.ErrorMessage.AFL_UkeToLessThanUkeFrom);
            });

            When(_ => _.FutaiStart != null && _.FutaiEnd != null, () =>
            {
                RuleFor(_ => _.FutaiStart).Must((model, futai) => CommonHelper.IsBetween((0, 0), (0, (int)model.FutaiEnd.Futaicd), (0, (int)futai.Futaicd)))
                                          .WithMessage(Constants.ErrorMessage.AFL_FutaiFromLargerThanFutaiTo);
            });
            When(_ => _.CsvConfigOption.Delimiter.Option == CSV_Delimiter.Other, () => {
                RuleFor(_ => _.CsvConfigOption.DelimiterSymbol).NotEmpty()
                .When(_ => string.IsNullOrEmpty(_.CsvConfigOption.DelimiterSymbol))
                .WithMessage(Constants.ErrorMessage.AFL_CsvSeparatorIsEmpty);
            });
        }
    }
}
