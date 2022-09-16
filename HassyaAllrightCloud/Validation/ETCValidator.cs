using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Validation
{
    public class ETCValidator : AbstractValidator<ETCSearchParam>
    {
        public ETCValidator()
        {
            RuleFor(e => e.SelectedSyaRyoFrom).Must((obj, value) =>
                value == null || obj.SelectedSyaRyoTo == null || value.SyaRyoCd <= obj.SelectedSyaRyoTo.SyaRyoCd
            ).WithMessage(Constants.ErrorMessage.ETC_BI_T003);

            RuleFor(e => e.SelectedSyaRyoTo).Must((obj, value) =>
                value == null || obj.SelectedSyaRyoFrom == null || value.SyaRyoCd >= obj.SelectedSyaRyoFrom.SyaRyoCd
            ).WithMessage(Constants.ErrorMessage.ETC_BI_T003);

            RuleFor(e => e.ETCDateFrom).NotEmpty().WithMessage(Constants.ErrorMessage.ETC_UsageYmdError)
                                       .Must((obj, value) =>
                                           value == null || obj.ETCDateTo == null || value.Value <= obj.ETCDateTo.Value
                                       ).WithMessage(Constants.ErrorMessage.ETC_BI_T002);

            RuleFor(e => e.ETCDateTo).Must((obj, value) =>
                                        value == null || obj.ETCDateFrom == null || value.Value >= obj.ETCDateFrom.Value
                                     ).WithMessage(Constants.ErrorMessage.ETC_BI_T002);

            RuleFor(e => e.ReturnDateFrom).Must((obj, value) =>
                value == null || obj.ReturnDateTo == null || value.Value <= obj.ReturnDateTo.Value
            ).WithMessage(Constants.ErrorMessage.ETC_BI_T004);

            RuleFor(e => e.ReturnDateTo).Must((obj, value) =>
                value == null || obj.ReturnDateFrom == null || value.Value >= obj.ReturnDateFrom.Value
            ).WithMessage(Constants.ErrorMessage.ETC_BI_T004);


            RuleFor(e => e.SelectedFutai).Must((obj, val) => obj.ScreenType == 0 || (obj.ScreenType == 1 && val != null)).WithMessage(Constants.ErrorMessage.ETC_FutaiError);

            RuleFor(e => e.SelectedSeisan).Must((obj, val) => obj.ScreenType == 0 || (obj.ScreenType == 1 && val != null)).WithMessage(Constants.ErrorMessage.ETC_SeisanError);
        }
    }
}
