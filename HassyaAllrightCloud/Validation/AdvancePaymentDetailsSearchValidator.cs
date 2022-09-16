using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class AdvancePaymentDetailsSearchValidator : AbstractValidator<AdvancePaymentDetailsSearchParam>
    {
        public AdvancePaymentDetailsSearchValidator()
        {
            RuleFor(t => t.StartAddress).
                Must((obj, v) => obj.EndAddress == null || v == null || v.TokuiCd < obj.EndAddress.TokuiCd || (v.TokuiCd == obj.EndAddress.TokuiCd && v.SitenCd <= obj.EndAddress.SitenCd)).
                WithMessage(Constants.ErrorMessage.BillingDesError);
            RuleFor(t => t.EndAddress).
                Must((obj, v) => obj.StartAddress == null || v == null || v.TokuiCd > obj.StartAddress.TokuiCd || (v.TokuiCd == obj.EndAddress.TokuiCd && v.SitenCd >= obj.StartAddress.SitenCd)).
                WithMessage(Constants.ErrorMessage.BillingDesError);
        }
    }
}
