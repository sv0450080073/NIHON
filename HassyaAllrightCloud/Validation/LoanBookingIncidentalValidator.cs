using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class LoanBookingIncidentalValidator : AbstractValidator<LoanBookingIncidentalData>
    {
        public LoanBookingIncidentalValidator()
        {
            RuleForEach(i => i.LoadYFutTuList).SetValidator(new LoadYFutTuValidator());
        }
    }

    public class LoadYFutTuValidator : AbstractValidator<LoadYFutTu>
    {
        public LoadYFutTuValidator()
        {
            When(f => f.FuttumKbnMode == IncidentalViewMode.Futai, () =>
            {
                RuleFor(f => f.SelectedLoadYFutai).NotEmpty().WithMessage(Constants.ErrorMessage.NotSelectedLoanIncidentalCode);
                RuleFor(f => f.SelectedLoadYSeisan).NotEmpty().WithMessage(Constants.ErrorMessage.NotSelectedLoanClearingCode);
                RuleFor(f => f.RyokinNm).MaximumLength(10);
                RuleFor(f => f.ShuRyokinNm).MaximumLength(10);
            });
            When(f => f.FuttumKbnMode == IncidentalViewMode.Tsumi, () =>
            {
                RuleFor(f => f.SelectedLoadYTsumi).NotEmpty().WithMessage(Constants.ErrorMessage.NotSelectedYTsumiRequired);
                RuleFor(f => f.SelectedLoadYSeisan).NotEmpty().WithMessage(Constants.ErrorMessage.NotSelectedYTsumiRequired);
            });
            RuleFor(f => f.YFutTuNm).MaximumLength(30);
            RuleFor(f => f.SeisanNm).MaximumLength(50);
            RuleFor(f => f.Suryo).MaximumLength(3).WithMessage(Constants.ErrorMessage.NotSelectedQuantity);
            RuleFor(f => f.Tanka).MaximumLength(6);
            RuleForEach(f => f.SettingQuantityList).SetValidator(yFutTu => new SettingQuantityValidator(yFutTu));
        }
    }
}
