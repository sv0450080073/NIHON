using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Validation
{
    public class ETCFormValidator : AbstractValidator<ETCFormModel>
    {
        public ETCFormValidator()
        {
            RuleFor(e => e.SyaRyo).NotEmpty().WithMessage("BI_T001_1");
            RuleFor(e => e.UnkYmd).NotEmpty().WithMessage("BI_T001_2");
            RuleFor(e => e.Futai).NotEmpty().WithMessage("BI_T001_3");
            RuleFor(e => e.Seisan).NotEmpty().WithMessage("BI_T001_4");
        }
    }
}
