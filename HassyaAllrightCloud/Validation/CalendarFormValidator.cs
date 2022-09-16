using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Validation
{
    public class CalendarFormValidator : AbstractValidator<CalendarSetModel>
    {
        public CalendarFormValidator()
        {
            RuleFor(m => m.CalendarName).NotEmpty().WithMessage("NoCalendarNameError");
        }
    }
}
