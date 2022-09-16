
using DevExpress.ClipboardSource.SpreadsheetML;
using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.InvoiceIssueRelease;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Constants.Constants;

namespace HassyaAllrightCloud.Application.Validation
{
    public class InvoiceIssueReleaseValidator : AbstractValidator<InvoiceIssueFilter>
    {
        public InvoiceIssueReleaseValidator()
        {
            When(x => x.StartBillIssuedDate != null && x.EndBillIssuedDate != null && x.StartBillIssuedDate > x.EndBillIssuedDate, () =>
            {
                RuleFor(m => m.StartBillIssuedDate).Empty().WithMessage(ErrorMessage.IIRBillIssueDate);
            });

            //When(x => x.StartBillAddress != null && x.EndBillAddress != null &&
            //         ((x.StartBillAddress.GyoSyaCd > x.EndBillAddress.GyoSyaCd)
            //       || (x.StartBillAddress.GyoSyaCd == x.EndBillAddress.GyoSyaCd && x.StartBillAddress.TokuiCd > x.EndBillAddress.TokuiCd)
            //       || (x.StartBillAddress.GyoSyaCd == x.EndBillAddress.GyoSyaCd && x.StartBillAddress.TokuiCd == x.EndBillAddress.TokuiCd && x.StartBillAddress.SitenCd > x.EndBillAddress.SitenCd)
            //       || (x.StartBillAddress.GyoSyaCd == x.EndBillAddress.GyoSyaCd && x.StartBillAddress.TokuiCd == x.EndBillAddress.TokuiCd && x.StartBillAddress.SitenCd == x.EndBillAddress.SitenCd && x.StartBillAddress.GyoSysSeq > x.EndBillAddress.GyoSysSeq)
            //       || (x.StartBillAddress.GyoSyaCd == x.EndBillAddress.GyoSyaCd && x.StartBillAddress.TokuiCd == x.EndBillAddress.TokuiCd && x.StartBillAddress.SitenCd == x.EndBillAddress.SitenCd && x.StartBillAddress.GyoSysSeq == x.EndBillAddress.GyoSysSeq && x.StartBillAddress.TokuiSeq > x.EndBillAddress.TokuiSeq)
            //       || (x.StartBillAddress.GyoSyaCd == x.EndBillAddress.GyoSyaCd && x.StartBillAddress.TokuiCd == x.EndBillAddress.TokuiCd && x.StartBillAddress.SitenCd == x.EndBillAddress.SitenCd && x.StartBillAddress.GyoSysSeq == x.EndBillAddress.GyoSysSeq && x.StartBillAddress.TokuiSeq == x.EndBillAddress.TokuiSeq && x.StartBillAddress.SitenCdSeq > x.EndBillAddress.SitenCdSeq)), () =>
            //       {
            //           RuleFor(m => m.StartBillAddress).Empty().WithMessage(ErrorMessage.IIRBillAddress);
            //       });

            When(x => x.SelectedGyosyaFrom != null && x.SelectedGyosyaTo != null, () =>
            {
                RuleFor(e => e.SelectedGyosyaTo).Must((obj, value) => value == null || value.GyosyaCd >= obj.SelectedGyosyaFrom.GyosyaCd).WithMessage(ErrorMessage.IIRBillAddress);
                RuleFor(e => e.SelectedGyosyaFrom).Must((obj, value) => value == null || value.GyosyaCd <= obj.SelectedGyosyaTo.GyosyaCd).WithMessage(ErrorMessage.IIRBillAddress);
            });

            When(x => x.SelectedTokiskFrom != null && x.SelectedTokiskTo != null &&
                    (x.SelectedGyosyaFrom?.GyosyaCd ?? 0) == (x.SelectedGyosyaTo?.GyosyaCd ?? 999), () =>
            {
                RuleFor(e => e.SelectedTokiskTo).Must((obj, value) => value == null || value.TokuiCd >= obj.SelectedTokiskFrom.TokuiCd).WithMessage(ErrorMessage.IIRBillAddress);
                RuleFor(e => e.SelectedTokiskFrom).Must((obj, value) => value == null || value.TokuiCd <= obj.SelectedTokiskTo.TokuiCd).WithMessage(ErrorMessage.IIRBillAddress);
            });

            When(x => x.SelectedTokiStFrom != null && x.SelectedTokiStTo != null &&
                    (x.SelectedGyosyaFrom?.GyosyaCd ?? 0) == (x.SelectedGyosyaTo?.GyosyaCd ?? 999) &&
                    (x.SelectedTokiskFrom?.TokuiCd ?? 0) == (x.SelectedTokiskTo?.TokuiCd ?? 9999), () =>
            {
                RuleFor(e => e.SelectedTokiStTo).Must((obj, value) => value == null || value.SitenCd >= obj.SelectedTokiStFrom.SitenCd).WithMessage(ErrorMessage.IIRBillAddress);
                RuleFor(e => e.SelectedTokiStFrom).Must((obj, value) => value == null || value.SitenCd <= obj.SelectedTokiStTo.SitenCd).WithMessage(ErrorMessage.IIRBillAddress);
            });
        }
    }
}
