using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class MonthlyTransportationValidator : AbstractValidator<MonthlyTransportationFormSearch>
    {
        public MonthlyTransportationValidator()
        {
            When(t => t.OutputInstructionMode == (int)OutputInstruction.Show, () =>
            {
                RuleFor(t => t.EigyoFrom).NotEmpty().WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T001);
                RuleFor(t => t.ShippingFrom).NotEmpty().WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T004);
            });

            When(t => (t.OutputInstructionMode == (int)OutputInstruction.Preview || t.OutputInstructionMode == (int)OutputInstruction.Pdf) &&
                       (t.ShippingFrom != null && t.ShippingTo != null), () =>
            {
                RuleFor(e => e.ShippingFrom).Must((obj, v) => v.CodeKbn <= obj.ShippingTo.CodeKbn)
                                           .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T003);
                RuleFor(e => e.ShippingTo).Must((obj, v) => v.CodeKbn >= obj.ShippingFrom.CodeKbn)
                                    .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T003);
            });

            When(t => (t.OutputInstructionMode == (int)OutputInstruction.Preview || t.OutputInstructionMode == (int)OutputInstruction.Pdf) &&
                    (t.EigyoFrom != null && t.EigyoTo != null), () =>
                    {
                        RuleFor(e => e.EigyoFrom).Must((obj, v) => v.EigyoCd <= obj.EigyoTo.EigyoCd)
                                            .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T002);
                        RuleFor(e => e.EigyoTo).Must((obj, v) => v.EigyoCd >= obj.EigyoFrom.EigyoCd)
                                            .WithMessage(Constants.ErrorMessage.FaresUpperAndLowerLimits_BI_T002);
                    });

        }
    }
}
