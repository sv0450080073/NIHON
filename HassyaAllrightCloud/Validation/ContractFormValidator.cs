using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.Validation
{
    public class ContractFormValidator : AbstractValidator<TransportationContractFormData>
    {

        public ContractFormValidator()
        {

            When(x => (x.DateFrom > x.DateTo), () =>
            {
                RuleFor(m => m.DateTo).Empty().WithMessage(Constants.ErrorMessage.EndDateGreaterThanStartDate);
                RuleFor(m => m.DateFrom).Empty().WithMessage(Constants.ErrorMessage.EndDateGreaterThanStartDate);
            });

        }
    }
}
