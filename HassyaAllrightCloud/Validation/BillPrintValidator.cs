
using DevExpress.ClipboardSource.SpreadsheetML;
using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BillPrintValidator : AbstractValidator<BillPrintInput>
    {
        public BillPrintValidator()
        {
            When(x => x.StartRcpNum != null && x.EndRcpNum != null && x.StartRcpNum > x.EndRcpNum && x.PrintMode == 1, () =>
            {
                RuleFor(data => data.StartRcpNum).Empty().WithMessage(Constants.ErrorMessage.InvalidRcpNum);
                RuleFor(data => data.EndRcpNum).Empty().WithMessage(Constants.ErrorMessage.InvalidRcpNum);
            });

            // 請求先
            When(x => (x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && (x.startCustomerComponentGyosyaData.GyosyaCd > x.endCustomerComponentGyosyaData.GyosyaCd)), () =>
            {
                RuleFor(m => m.startCustomerComponentGyosyaData).Empty().WithMessage(Constants.ErrorMessage.InvalidBillAdd);
                RuleFor(m => m.endCustomerComponentGyosyaData).Empty().WithMessage(Constants.ErrorMessage.InvalidBillAdd);
            });
            When(x => (x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && x.startCustomerComponentTokiskData != null && x.endCustomerComponentTokiskData != null && (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd > x.endCustomerComponentTokiskData.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.startCustomerComponentTokiskData).Empty().WithMessage(Constants.ErrorMessage.InvalidBillAdd);
                    RuleFor(m => m.endCustomerComponentTokiskData).Empty().WithMessage(Constants.ErrorMessage.InvalidBillAdd);
                });
            When(x => ((x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && x.startCustomerComponentTokiskData != null && x.endCustomerComponentTokiskData != null && x.startCustomerComponentTokiStData != null && x.endCustomerComponentTokiStData != null)
                    && ((x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd > x.endCustomerComponentTokiStData.SitenCd)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq > x.endCustomerComponentGyosyaData.GyosyaCdSeq)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq == x.endCustomerComponentGyosyaData.GyosyaCdSeq && x.startCustomerComponentTokiskData.TokuiSeq > x.endCustomerComponentTokiskData.TokuiSeq)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq == x.endCustomerComponentGyosyaData.GyosyaCdSeq && x.startCustomerComponentTokiskData.TokuiSeq == x.endCustomerComponentTokiskData.TokuiSeq && x.startCustomerComponentTokiStData.SitenCdSeq > x.endCustomerComponentTokiStData.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.startCustomerComponentTokiStData).Empty().WithMessage(Constants.ErrorMessage.InvalidBillAdd);
                        RuleFor(m => m.endCustomerComponentTokiStData).Empty().WithMessage(Constants.ErrorMessage.InvalidBillAdd);
                    });

            // ​予約区分
            When(x => x.StartRsrCatDropDown != null && x.EndRsrCatDropDown != null
            && x.StartRsrCatDropDown.YoyaKbn > x.EndRsrCatDropDown.YoyaKbn, () => {
                RuleFor(m => m.EndRsrCatDropDown).Empty().WithMessage(Constants.ErrorMessage.InvalidRsrCat);
                RuleFor(m => m.StartRsrCatDropDown).Empty().WithMessage(Constants.ErrorMessage.InvalidRsrCat);
            });
            When(x => x.PrintMode == 4, () =>
            {
                RuleFor(data => data.InvoiceOutNum).NotEmpty().WithMessage(Constants.ErrorMessage.InvalidInvoice);
                RuleFor(data => data.InvoiceSerNum).NotEmpty().WithMessage(Constants.ErrorMessage.InvalidInvoice);
            });
            When(x => x.BillingOffice == 0 && x.PrintMode == 1, () =>
            {
                RuleFor(data => data.BillingOfficeDropDown).NotEmpty().WithMessage(Constants.ErrorMessage.InvalidBillingOffice);
            });
            When(x => x.InvoiceYm == null && x.PrintMode == 1, () =>
            {
                RuleFor(data => data.InvoiceYm).NotEmpty().WithMessage(Constants.ErrorMessage.InvalidInvoiceYm);
            });
            When(x => x.IssueYmd == null && x.PrintMode == 1, () =>
            {
                RuleFor(data => data.IssueYmd).NotEmpty().WithMessage(Constants.ErrorMessage.InvalidIssueYmd);
            });
        }
    }
}
