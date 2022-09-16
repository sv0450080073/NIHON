using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;

namespace HassyaAllrightCloud.Application.Validation
{
    public class ReceivableListValidator : AbstractValidator<ReceivableFilterModel>
    {
        public ReceivableListValidator()
        {
            When(x => x.StartSaleBranchList != null && x.EndSaleBranchList != null && x.StartSaleBranchList.EigyoCd > x.EndSaleBranchList.EigyoCd, () =>
            {
                RuleFor(m => m.EndSaleBranchList).Empty().WithMessage("BI_T001");
                RuleFor(m => m.StartSaleBranchList).Empty().WithMessage("BI_T001");
            });
            When(x => x.StartPaymentDate != null && x.EndPaymentDate != null && x.StartPaymentDate > x.EndPaymentDate, () =>
            {
                RuleFor(m => m.EndPaymentDate).Empty().WithMessage("BI_T002");
                RuleFor(m => m.StartPaymentDate).Empty().WithMessage("BI_T002");
            });
            When(x => x.StartReceiptNumber != null && x.EndReceiptNumber != null && long.Parse(x.StartReceiptNumber) > long.Parse(x.EndReceiptNumber), () =>
            {
                RuleFor(x => x.EndReceiptNumber).Empty().WithMessage("BI_T004");
                RuleFor(x => x.StartReceiptNumber).Empty().WithMessage("BI_T004");
            });
            When(x => (x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && (x.startCustomerComponentGyosyaData.GyosyaCd > x.endCustomerComponentGyosyaData.GyosyaCd)), () =>
            {
                RuleFor(m => m.startCustomerComponentGyosyaData).Empty().WithMessage("BI_T003");
                RuleFor(m => m.endCustomerComponentGyosyaData).Empty().WithMessage("BI_T003");
            });
            When(x => (x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && x.startCustomerComponentTokiskData != null && x.endCustomerComponentTokiskData != null && (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd > x.endCustomerComponentTokiskData.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.startCustomerComponentTokiskData).Empty().WithMessage("BI_T003");
                    RuleFor(m => m.endCustomerComponentTokiskData).Empty().WithMessage("BI_T003");
                });
            When(x => ((x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && x.startCustomerComponentTokiskData != null && x.endCustomerComponentTokiskData != null && x.startCustomerComponentTokiStData != null && x.endCustomerComponentTokiStData != null)
                    && ((x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd > x.endCustomerComponentTokiStData.SitenCd)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq > x.endCustomerComponentGyosyaData.GyosyaCdSeq)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq == x.endCustomerComponentGyosyaData.GyosyaCdSeq && x.startCustomerComponentTokiskData.TokuiSeq > x.endCustomerComponentTokiskData.TokuiSeq)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq == x.endCustomerComponentGyosyaData.GyosyaCdSeq && x.startCustomerComponentTokiskData.TokuiSeq == x.endCustomerComponentTokiskData.TokuiSeq && x.startCustomerComponentTokiStData.SitenCdSeq > x.endCustomerComponentTokiStData.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.startCustomerComponentTokiStData).Empty().WithMessage("BI_T003");
                        RuleFor(m => m.endCustomerComponentTokiStData).Empty().WithMessage("BI_T003");
                    });

            // ​予約区分
            When(x => x.StartReservationClassification != null && x.EndReservationClassification != null
            && x.StartReservationClassification.YoyaKbn > x.EndReservationClassification.YoyaKbn, () => {
                RuleFor(m => m.EndReservationClassification).Empty().WithMessage("BI_T005");
                RuleFor(m => m.StartReservationClassification).Empty().WithMessage("BI_T005");
            });
        }
    }
}
