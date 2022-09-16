using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System;

namespace HassyaAllrightCloud.Application.Validation
{
    public class ReceiptOutputSearchValidator : AbstractValidator<ReceiptOutputFormSeachModel>
    {
        public ReceiptOutputSearchValidator()
        {
            When(t => string.IsNullOrEmpty(t.BillOffice?.Text), () =>
            {
                RuleFor(t => t.BillOffice).NotEmpty().WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T001);
            });

            When(t => t.BillAddressFrom != null && t.BillAddressTo != null, () =>
            {
                RuleFor(e => e.BillAddressFrom).Must((obj, v) => v.TokuiCd <= obj.BillAddressTo.TokuiCd)
                                         .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T002);
                RuleFor(e => e.BillAddressTo).Must((obj, v) => v.TokuiCd >= obj.BillAddressFrom.TokuiCd)
                                    .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T002);
            });

            When(t => t.StaInvoicingDate != null && t.EndInvoicingDate != null, () =>
            {
                RuleFor(e => e.StaInvoicingDate).Must((obj, v) => v <= obj.EndInvoicingDate)
                                           .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T004);
                RuleFor(e => e.EndInvoicingDate).Must((obj, v) => v >= obj.StaInvoicingDate)
                                                .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T004);
            });

            When(t => !string.IsNullOrEmpty(t.StaInvoiceOutNum) && !string.IsNullOrEmpty(t.StaInvoiceSerNum) && !string.IsNullOrEmpty(t.EndInvoiceOutNum) && !string.IsNullOrEmpty(t.EndInvoiceSerNum), () =>
            {
                RuleFor(t => t.StaInvoiceOutNum).Must((obj, v) => CheckValid(v, obj.StaInvoiceSerNum, obj.EndInvoiceOutNum, obj.EndInvoiceSerNum))
                                    .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T005);
                RuleFor(t => t.StaInvoiceSerNum).Must((obj, v) => CheckValid(obj.StaInvoiceOutNum, v, obj.EndInvoiceOutNum, obj.EndInvoiceSerNum))
                                   .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T005);
                RuleFor(t => t.EndInvoiceOutNum).Must((obj, v) => CheckValid(obj.StaInvoiceOutNum, obj.StaInvoiceSerNum, v, obj.EndInvoiceSerNum))
                                       .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T005);
                RuleFor(t => t.EndInvoiceSerNum).Must((obj, v) => CheckValid(obj.StaInvoiceOutNum, obj.StaInvoiceSerNum, obj.EndInvoiceOutNum, v))
                                      .WithMessage(Constants.ErrorMessage.ReceiptOutput_BI_T005);
            });
        }

        protected bool CheckValid(string staInvoiceOutNum, string staInvoiceSerNum, string endInvoiceOutNum, string endInvoiceSerNum)
        {
            if (Convert.ToUInt64($"{staInvoiceOutNum}{staInvoiceSerNum}") <= Convert.ToUInt64($"{endInvoiceOutNum}{endInvoiceSerNum}"))
                return true;
            else
                return false;
        }
    }
}
