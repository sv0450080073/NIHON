using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class LockBookingValidator : AbstractValidator<LockBookingData>
    {
        public LockBookingValidator()
        {
            RuleFor(l => l.SalesOffice).NotNull().WithMessage(Constants.ErrorMessage.NotSelectedBranchToLock);
        }
    }
}
