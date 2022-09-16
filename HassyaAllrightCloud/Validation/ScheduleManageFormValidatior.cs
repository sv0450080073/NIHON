using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class ScheduleManageFormValidatior : AbstractValidator<ScheduleManageForm>
    {
        public ScheduleManageFormValidatior()
        {
            // ​請求対象期間
            When(x => x.StartDate != null && x.EndDate != null && x.StartDate > x.EndDate, () => {
                RuleFor(m => m.EndDate).Empty().WithMessage("BI_T001");
            });
        }
    }
}
