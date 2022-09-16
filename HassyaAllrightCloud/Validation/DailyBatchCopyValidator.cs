using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Validation
{
    public class DailyBatchCopyValidator : AbstractValidator<DailyBatchCopySearchModel>
    {
        public DailyBatchCopyValidator()
        {
            RuleFor(_ => _.RepeatEnd).Must((obj, value) =>
            {
                if (!obj.IsDayOfWeek) return true;
                var compareDate = obj.StartDate.AddMonths(3);
                return value <= compareDate && value >= obj.StartDate;
            }).WithMessage(Constants.ErrorMessage.DAILYBATCHCOPY_BI_T001);
        }
    }
}
