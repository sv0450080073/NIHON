using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Validation
{
    public class BatchPoliceInputValidator : AbstractValidator<BatchKobanInputFilterModel>
    {
        public BatchPoliceInputValidator()
        {
            RuleFor(m => m.KinmuYmd).NotEmpty().WithMessage("BI_T001");
            When(x => x.EigyoStart != null && x.EigyoEnd != null && x.EigyoStart.EigyoCd > x.EigyoEnd.EigyoCd, () =>
            {
                RuleFor(m => m.EigyoEnd).Empty().WithMessage("BI_T002");
            });
            When(x => x.SyainStart != null && x.SyainEnd != null && x.SyainStart.SyainCd.CompareTo(x.SyainEnd.SyainCd) > 0, () =>
            {
                RuleFor(m => m.SyainEnd).Empty().WithMessage("BI_T003");
            });
            When(x => x.SyokumuStart != null && x.SyokumuEnd != null && x.SyokumuStart.SyokumuCd.CompareTo(x.SyokumuEnd.SyokumuCd) > 0, () =>
            {
                RuleFor(m => m.SyokumuEnd).Empty().WithMessage("BI_T004");
            });
        }
    }
}
