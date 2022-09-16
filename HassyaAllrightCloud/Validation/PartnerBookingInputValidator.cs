
using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Validation
{
    public class PartnerBookingInputValidator : AbstractValidator<YouShaDataInsert>
    {
        public PartnerBookingInputValidator()
        {
        }
    }
    public class HaiShaPopupUpdateValidator : AbstractValidator<HaiShaDataUpdate>
    {
        public HaiShaPopupUpdateValidator()
        {
            When(x => (x.CheckHaiSHaiSha < x.CheckHaiSUnkobi), () =>
            {
                RuleFor(x => x.CheckHaiSUnkobi).Empty().WithMessage("messageHaiSDate");
            });
            When(x => (x.CheckTouHaiSha > x.CheckTouUnkobi), () =>
            {
                RuleFor(x => x.CheckTouUnkobi).Empty().WithMessage("messageTouYDate");
            });
            When(x => (x.CheckTouHaiSha < x.CheckHaiSHaiSha), () =>
            {
                RuleFor(x => x.CheckHaiSHaiSha).Empty().WithMessage("messageTouChTimeGreaterThanHaisTime");
            });
        }
    }
}
