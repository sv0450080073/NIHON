using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System;

namespace HassyaAllrightCloud.Application.Validation
{
    public class AnnualTransportationRecordValidator : AbstractValidator<AnnualTransportationRecordFormSearch>
    {
        public AnnualTransportationRecordValidator()
        {
            RuleFor(e => e.ProcessingDateFrom).Must((obj, v) => v <= obj.ProcessingDateTo)
                                             .WithMessage(Constants.ErrorMessage.AnnualTransportation_BI_T001);
            RuleFor(e => e.ProcessingDateTo).Must((obj, v) => v >= obj.ProcessingDateFrom)
                                            .WithMessage(Constants.ErrorMessage.AnnualTransportation_BI_T001);

            When(t => (t.ShippingFrom != null && t.ShippingTo != null), () =>
                       {
                           RuleFor(e => e.ShippingFrom).Must((obj, v) => v.CodeKbn <= obj.ShippingTo.CodeKbn)
                                                      .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T003);
                           RuleFor(e => e.ShippingTo).Must((obj, v) => v.CodeKbn >= obj.ShippingFrom.CodeKbn)
                                               .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T003);
                       }
            );

            When(t => (t.EigyoFrom != null && t.EigyoTo != null), () =>
                     {
                         RuleFor(e => e.EigyoFrom).Must((obj, v) => v.EigyoCd <= obj.EigyoTo.EigyoCd)
                                             .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T002);
                         RuleFor(e => e.EigyoTo).Must((obj, v) => v.EigyoCd >= obj.EigyoFrom.EigyoCd)
                                             .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T002);
                     });
        }
    }
}
