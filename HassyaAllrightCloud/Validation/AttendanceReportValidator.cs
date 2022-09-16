using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class AttendanceReportValidator : AbstractValidator<AttendanceReportSearchModel>
    {
        public AttendanceReportValidator()
        {
            When(_ => _.EigyoTo != null, () =>
            {
                RuleFor(e => e.EigyoFrom).Must((obj, value) => obj.EigyoFrom == null || (value != null && value.EigyoCd <= obj.EigyoTo.EigyoCd)).WithMessage("BI_T001");
            });
            When(_ => _.EigyoFrom != null, () =>
            {
                RuleFor(e => e.EigyoTo).Must((obj, value) => obj.EigyoTo == null || (value != null && value.EigyoCd >= obj.EigyoFrom.EigyoCd)).WithMessage("BI_T001");
            });
            When(_ => _.RegistrationTypeTo != null, () =>
            {
                RuleFor(e => e.RegistrationTypeFrom).Must((obj, value) => obj.RegistrationTypeFrom == null || value.YoyaKbn <= obj.RegistrationTypeTo.YoyaKbn).WithMessage("BI_T002");
            });
            When(_ => _.RegistrationTypeFrom != null, () =>
            {
                RuleFor(e => e.RegistrationTypeTo).Must((obj, value) => obj.RegistrationTypeTo == null || value.YoyaKbn >= obj.RegistrationTypeFrom.YoyaKbn).WithMessage("BI_T002");
            });
        }
    }
}
