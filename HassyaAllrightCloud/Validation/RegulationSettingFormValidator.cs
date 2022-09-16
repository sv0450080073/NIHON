using FluentValidation;
using HassyaAllrightCloud.Domain.Dto.RegulationSetting;

namespace HassyaAllrightCloud.Application.Validation
{
    public class RegulationSettingValidator : AbstractValidator<RegulationSettingModel>
    {
        public RegulationSettingValidator()
        {
            When(_ => _.CompanyTo != null, () =>
            {
                RuleFor(e => e.CompanyFrom).Must((obj, value) => obj.CompanyFrom == null || (value != null && value.CompanyCd <= obj.CompanyTo.CompanyCd)).WithMessage("BI_T001");
            });
            When(_ => _.CompanyFrom != null, () =>
            {
                RuleFor(e => e.CompanyTo).Must((obj, value) => obj.CompanyTo == null || (value != null && value.CompanyCd >= obj.CompanyFrom.CompanyCd)).WithMessage("BI_T001");
            });
        }
    }
}
