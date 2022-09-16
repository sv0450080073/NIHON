using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BillingListValidator : AbstractValidator<BillingListFilter>
    {
        public BillingListValidator()
        {
            //When(x => x.CloseDate != null && (x.CloseDate < 1 || x.CloseDate > 31), () => {
            //    RuleFor(m => m.CloseDate).Empty().WithMessage("BI_T001");
            //});
            When(x => (x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && (x.startCustomerComponentGyosyaData.GyosyaCd > x.endCustomerComponentGyosyaData.GyosyaCd)), () =>
            {
                RuleFor(m => m.startCustomerComponentGyosyaData).Empty().WithMessage("BI_T002");
                RuleFor(m => m.endCustomerComponentGyosyaData).Empty().WithMessage("BI_T002");
            });
            When(x => (x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && x.startCustomerComponentTokiskData != null && x.endCustomerComponentTokiskData != null && (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd > x.endCustomerComponentTokiskData.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.startCustomerComponentTokiskData).Empty().WithMessage("BI_T002");
                    RuleFor(m => m.endCustomerComponentTokiskData).Empty().WithMessage("BI_T002");
                });
            When(x => ((x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && x.startCustomerComponentTokiskData != null && x.endCustomerComponentTokiskData != null && x.startCustomerComponentTokiStData != null && x.endCustomerComponentTokiStData != null)
                    && ((x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd > x.endCustomerComponentTokiStData.SitenCd)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq > x.endCustomerComponentGyosyaData.GyosyaCdSeq)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq == x.endCustomerComponentGyosyaData.GyosyaCdSeq && x.startCustomerComponentTokiskData.TokuiSeq > x.endCustomerComponentTokiskData.TokuiSeq)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq == x.endCustomerComponentGyosyaData.GyosyaCdSeq && x.startCustomerComponentTokiskData.TokuiSeq == x.endCustomerComponentTokiskData.TokuiSeq && x.startCustomerComponentTokiStData.SitenCdSeq > x.endCustomerComponentTokiStData.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.startCustomerComponentTokiStData).Empty().WithMessage("BI_T002");
                        RuleFor(m => m.endCustomerComponentTokiStData).Empty().WithMessage("BI_T002");
                    });

            // ​予約区分
            When(x => x.StartReservationClassification != null && x.EndReservationClassification != null
            && x.StartReservationClassification.YoyaKbn > x.EndReservationClassification.YoyaKbn, () => {
                RuleFor(m => m.EndReservationClassification).Empty().WithMessage("BI_T004");
                RuleFor(m => m.StartReservationClassification).Empty().WithMessage("BI_T004");
            });

            // 受付番号
            When(x => x.StartReceiptNumber != null && x.EndReceiptNumber != null && x.StartReceiptNumber > x.EndReceiptNumber, () => {
                RuleFor(m => m.StartReceiptNumber).Empty().WithMessage("BI_T003");
                RuleFor(m => m.EndReceiptNumber).Empty().WithMessage("BI_T003");
            });

            // ​請求区分
            When(x => x.StartBillClassification != null && x.EndBillClassification != null && x.StartBillClassification.CodeKbnSeq > x.EndBillClassification.CodeKbnSeq, () => {
                RuleFor(m => m.StartBillClassification).Empty().WithMessage("BI_T005");
                RuleFor(m => m.EndBillClassification).Empty().WithMessage("BI_T005");
            });
        }
    }
}
