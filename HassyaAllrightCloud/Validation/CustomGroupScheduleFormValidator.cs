using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class CustomGroupScheduleFormValidator : AbstractValidator<CustomGroupScheduleForm>
    {
        public CustomGroupScheduleFormValidator()
        {
            RuleFor(m => m.GroupName).NotEmpty().WithMessage("NoGroupNameError");
            RuleFor(m => m.StaffList).Must(value => value.Count > 0).WithMessage("NoMemberError");
        }
    }
}
