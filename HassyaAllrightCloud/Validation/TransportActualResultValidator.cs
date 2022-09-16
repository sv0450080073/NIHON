using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class TransportActualResultValidator : AbstractValidator<TransportActualResultSearchModel>
    {
        public TransportActualResultValidator()
        {
            RuleFor(e => e.ProcessingYear).NotEmpty().WithMessage("BI_T001");
            When(_ => _.EigyoTo != null, () =>
            {
                RuleFor(e => e.EigyoFrom).Must((obj, value) => obj.EigyoFrom == null || (value != null && value.EigyoCd <= obj.EigyoTo.EigyoCd)).WithMessage("BI_T002");
            });
            When(_ => _.EigyoFrom != null, () =>
            {
                RuleFor(e => e.EigyoTo).Must((obj, value) => obj.EigyoTo == null || (value != null && value.EigyoCd >= obj.EigyoFrom.EigyoCd)).WithMessage("BI_T002");
            });
            When(_ => _.ShippingTo != null, () =>
            {
                RuleFor(e => e.ShippingFrom).Must((obj, value) => obj.ShippingFrom == null || (value != null && int.Parse(value.CodeKbn) <= int.Parse(obj.ShippingTo.CodeKbn))).WithMessage("BI_T003");
            });
            When(_ => _.ShippingFrom != null, () =>
            {
                RuleFor(e => e.ShippingTo).Must((obj, value) => obj.ShippingTo == null || (value != null && int.Parse(value.CodeKbn) >= int.Parse(obj.ShippingFrom.CodeKbn))).WithMessage("BI_T003");
            });
        }
    }
}
