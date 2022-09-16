using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.Validation
{
    public class TransportDailyReportSearchValidator : AbstractValidator<TransportDailyReportSearchParams>
    {
        public TransportDailyReportSearchValidator()
        {
            RuleFor(e => e.selectedDate).NotEmpty().WithMessage(Constants.ErrorMessage.UnkoDateError);
            RuleFor(e => e.aggregation).Must(v => v != null).WithMessage(Constants.ErrorMessage.SyuKbnError);
            RuleFor(e => e.selectedEigyoFrom).Must((obj, v) => obj.selectedEigyoTo == null || v == null || v.EigyoCd <= obj.selectedEigyoTo.EigyoCd)
                                             .WithMessage(Constants.ErrorMessage.EigyoError);
            RuleFor(e => e.selectedEigyoTo).Must((obj, v) => obj.selectedEigyoFrom == null || v == null || v.EigyoCd >= obj.selectedEigyoFrom.EigyoCd)
                                           .WithMessage(Constants.ErrorMessage.EigyoError);
        }
    }
}
