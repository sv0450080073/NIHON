using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System.Linq;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BookingIncidentalValidator : AbstractValidator<IncidentalBooking>
    {
        public BookingIncidentalValidator()
        {
            RuleForEach(i => i.LoadFuttumList).SetValidator(new LoadFuttumValidator());
        }
    }

    public class LoadFuttumValidator : AbstractValidator<LoadFuttum>
    {
        public LoadFuttumValidator()
        {
            When(f => f.FuttumKbnMode == IncidentalViewMode.Futai, () =>
            {
                RuleFor(f => f.SelectedLoadFutai).NotEmpty().WithMessage(Constants.ErrorMessage.NotSelectedIncidentalCode);
            });
            When(f => f.FuttumKbnMode == IncidentalViewMode.Tsumi, () =>
            {
                RuleFor(f => f.SelectedLoadTsumi).NotEmpty().WithMessage(Constants.ErrorMessage.NotSelectedLoadingCode);
            });

            RuleFor(f => f.SelectedLoadSeisanCd).NotEmpty().WithMessage(Constants.ErrorMessage.NotSelectedClearingCode);
            //RuleFor(f => f.SelectedCustomer).NotEmpty().WithMessage(Constants.ErrorMessage.NotSelectedCustomerIncidental);

            RuleForEach(f => f.SettingQuantityList).SetValidator(futtum => new SettingQuantityValidator(futtum));
        }
    }

    public class SettingQuantityValidator : AbstractValidator<SettingQuantity>
    {
        public SettingQuantityValidator(LoadFuttum futtum)
        {
            var sumQuantityList = futtum.SettingQuantityList.Sum(s => int.Parse(s.Suryo));
            RuleFor(e => e.Suryo).Empty()
                .When(e => sumQuantityList > 0 && sumQuantityList != int.Parse(futtum.Suryo))
                .WithMessage(Constants.ErrorMessage.NotMatchQuantityBusIncidental);
        }

        public SettingQuantityValidator(LoadYFutTu yFutTu)
        {
            var sumQuantityList = yFutTu.SettingQuantityList.Sum(s => int.Parse(s.Suryo));
            RuleFor(e => e.Suryo).Empty()
                .When(e => sumQuantityList > 0 && sumQuantityList != int.Parse(yFutTu.Suryo))
                .WithMessage(Constants.ErrorMessage.NotMatchQuantityBusLoanIncidental);
            RuleFor(e => e.Suryo).MaximumLength(3);
        }
    }
}
