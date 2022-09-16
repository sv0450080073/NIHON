using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System;

namespace HassyaAllrightCloud.Application.Validation
{
    public class FaresUpperAndLowerLimitSearchValidator : AbstractValidator<FaresUpperAndLowerLimitsFormSearch>
    {
        public FaresUpperAndLowerLimitSearchValidator()
        {
            When(t => t.OutputEndDate != null && t.OutputStartDate != null, () =>
            {
                RuleFor(e => e.OutputStartDate).Must((obj, v) => v <= obj.OutputEndDate)
                                      .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T001);
                RuleFor(e => e.OutputEndDate).Must((obj, v) => v >= obj.OutputStartDate)
                                     .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T001);

                RuleFor(e => e.OutputEndDate).Must((obj, v) => v.Value.Subtract(obj.OutputStartDate.Value).Days - 1 <= 30)
                                    .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T003);
                RuleFor(e => e.OutputStartDate).Must((obj, v) => obj.OutputEndDate.Value.Subtract(v.Value).Days - 1 <= 30)
                                   .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T003);
            });

            When(t => !string.IsNullOrEmpty(t.ReservationNumberStart) && !string.IsNullOrEmpty(t.ReservationNumberEnd), () =>
            {
                RuleFor(e => e.ReservationNumberEnd).Must((obj, v) => Convert.ToUInt64(v) >= Convert.ToUInt64(obj.ReservationNumberStart))
                                     .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T002);
                RuleFor(e => e.ReservationNumberStart).Must((obj, v) => Convert.ToUInt64(v) <= Convert.ToUInt64(obj.ReservationNumberEnd))
                                  .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T002);
            });
        }
    }
}
