using FluentValidation;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Constants.Constants;

namespace HassyaAllrightCloud.Application.Validation
{
    public class DepositCouponValidator : AbstractValidator<DepositCouponFilter>
    {
        public DepositCouponValidator()
        {
            // ​請求対象期間
            When(x => x.BillPeriodFrom != null && x.BillPeriodTo != null && x.BillPeriodFrom > x.BillPeriodTo, () => {
                RuleFor(m => m.BillPeriodTo).Empty().WithMessage(ErrorMessage.InvalidDepositCouponBillPeriod);
                RuleFor(m => m.BillPeriodFrom).Empty().WithMessage(ErrorMessage.InvalidDepositCouponBillPeriod);
            });

            // 請求先
            When(x => (x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && (x.startCustomerComponentGyosyaData.GyosyaCd > x.endCustomerComponentGyosyaData.GyosyaCd)), () =>
            {
                RuleFor(m => m.startCustomerComponentGyosyaData).Empty().WithMessage(ErrorMessage.InvalidDepositCouponBillPeriod);
                RuleFor(m => m.endCustomerComponentGyosyaData).Empty().WithMessage(ErrorMessage.InvalidDepositCouponBillPeriod);
            });
            When(x => (x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && x.startCustomerComponentTokiskData != null && x.endCustomerComponentTokiskData != null && (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd > x.endCustomerComponentTokiskData.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.startCustomerComponentTokiskData).Empty().WithMessage(ErrorMessage.InvalidDepositCouponBillPeriod);
                    RuleFor(m => m.endCustomerComponentTokiskData).Empty().WithMessage(ErrorMessage.InvalidDepositCouponBillPeriod);
                });
            When(x => ((x.startCustomerComponentGyosyaData != null && x.endCustomerComponentGyosyaData != null && x.startCustomerComponentTokiskData != null && x.endCustomerComponentTokiskData != null && x.startCustomerComponentTokiStData != null && x.endCustomerComponentTokiStData != null)
                    && ((x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd > x.endCustomerComponentTokiStData.SitenCd)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq > x.endCustomerComponentGyosyaData.GyosyaCdSeq)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq == x.endCustomerComponentGyosyaData.GyosyaCdSeq && x.startCustomerComponentTokiskData.TokuiSeq > x.endCustomerComponentTokiskData.TokuiSeq)
                    || (x.startCustomerComponentGyosyaData.GyosyaCd == x.endCustomerComponentGyosyaData.GyosyaCd && x.startCustomerComponentTokiskData.TokuiCd == x.endCustomerComponentTokiskData.TokuiCd && x.startCustomerComponentTokiStData.SitenCd == x.endCustomerComponentTokiStData.SitenCd && x.startCustomerComponentGyosyaData.GyosyaCdSeq == x.endCustomerComponentGyosyaData.GyosyaCdSeq && x.startCustomerComponentTokiskData.TokuiSeq == x.endCustomerComponentTokiskData.TokuiSeq && x.startCustomerComponentTokiStData.SitenCdSeq > x.endCustomerComponentTokiStData.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.startCustomerComponentTokiStData).Empty().WithMessage(ErrorMessage.InvalidDepositCouponBillPeriod);
                        RuleFor(m => m.endCustomerComponentTokiStData).Empty().WithMessage(ErrorMessage.InvalidDepositCouponBillPeriod);
                    });

            // ​予約区分
            When(x => x.StartReservationClassification != null && x.EndReservationClassification != null 
            && x.StartReservationClassification.YoyaKbn > x.EndReservationClassification.YoyaKbn, () => {
                RuleFor(m => m.EndReservationClassification).Empty().WithMessage(ErrorMessage.InvalidDepositCouponReservationClassification);
                RuleFor(m => m.StartReservationClassification).Empty().WithMessage(ErrorMessage.InvalidDepositCouponReservationClassification);
            });
        }
    }

    public class DepositPaymentValidator : AbstractValidator<DepositCouponPayment>
    {
        public DepositPaymentValidator()
        {
            When(x => x.DepositMethod != "07" && x.DepositAmount == null, () =>
            {
                RuleFor(m => m.DepositAmount).NotEmpty().WithMessage(ErrorMessage.DCPDepositAmount);
            });

            When(x => x.DepositMethod != "07" && x.DepositAmount.GetValueOrDefault() == 0, () =>
            {
                RuleFor(m => m.DepositAmount).Empty().WithMessage(ErrorMessage.DCPDepositAmount);
            });

            When(x => x.DepositMethod == "03" && string.IsNullOrWhiteSpace(x.CardApprovalNumber), () =>
            {
                RuleFor(m => m.CardApprovalNumber).NotEmpty().WithMessage(ErrorMessage.DCPCardApprovalNumber);
            });

            When(x => x.DepositMethod == "03" && string.IsNullOrWhiteSpace(x.CardSlipNumber), () =>
            {
                RuleFor(m => m.CardSlipNumber).NotEmpty().WithMessage(ErrorMessage.DCPCardSlipNumber);
            });

            When(x => x.DepositMethod == "04" && string.IsNullOrWhiteSpace(x.BillNo), () =>
            {
                RuleFor(m => m.BillNo).NotEmpty().WithMessage(ErrorMessage.DCPBillNo);
            });

            When(x => x.DepositMethod == "07", () =>
            {
                RuleForEach(m => m.OffsetPaymentTables).SetValidator(new OffsetTableValidator());
            });

            When(x => x.DepositMethod == "07" && x.IsEditOffsetTable, () =>
            {
                RuleForEach(m => m.OffsetPaymentTables).SetValidator(new OffsetTableValidator2());
                RuleForEach(m => m.OffsetPaymentTables).Must(x => x.ApplicationAmount.GetValueOrDefault() != 0
                && x.ApplicationAmount.GetValueOrDefault() < (int.Parse(x.BillAmount, System.Globalization.NumberStyles.Currency) - int.Parse(x.SumCouponsApplied, System.Globalization.NumberStyles.Currency))).WithMessage(ErrorMessage.DCPValidApplicationAmount);
            });
        }
    }

    public class DepositPaymentLumpValidator : AbstractValidator<DepositCouponPayment>
    {
        public DepositPaymentLumpValidator()
        {
            When(x => x.DepositMethod == "03" && string.IsNullOrWhiteSpace(x.CardApprovalNumber), () => {
                RuleFor(m => m.CardApprovalNumber).NotEmpty().WithMessage(ErrorMessage.DCPCardApprovalNumber);
            });

            When(x => x.DepositMethod == "03" && string.IsNullOrWhiteSpace(x.CardSlipNumber), () => {
                RuleFor(m => m.CardSlipNumber).NotEmpty().WithMessage(ErrorMessage.DCPCardSlipNumber);
            });

            When(x => x.DepositMethod == "04" && string.IsNullOrWhiteSpace(x.BillNo), () => {
                RuleFor(m => m.BillNo).NotEmpty().WithMessage(ErrorMessage.DCPBillNo);
            });

            When(x => x.DepositMethod == "07", () => {
                RuleForEach(m => m.OffsetPaymentTables).SetValidator(new OffsetTableValidator());
            });

            When(x => x.DepositMethod == "07" && x.IsEditOffsetTable, () => {
                RuleForEach(m => m.OffsetPaymentTables).SetValidator(new OffsetTableValidator2());
                RuleForEach(m => m.OffsetPaymentTables).Must(x => x.ApplicationAmount != null && x.ApplicationAmount != 0
                && x.ApplicationAmount.GetValueOrDefault() < (int.Parse(x.BillAmount, System.Globalization.NumberStyles.Currency) - int.Parse(x.SumCouponsApplied, System.Globalization.NumberStyles.Currency))).WithMessage(ErrorMessage.DCPValidApplicationAmount);
            });
        }
    }

    public class OffsetTableValidator : AbstractValidator<OffsetPaymentTable>
    {
        public OffsetTableValidator()
        {
            RuleFor(m => m.CouponNo).NotEmpty().WithMessage(ErrorMessage.DCPCouponNo);
            When(x => x.FaceValue == null, () =>
            {
                RuleFor(m => m.FaceValue).NotEmpty().WithMessage(ErrorMessage.DCPFaceValue);
            });
            When(x => x.FaceValue.GetValueOrDefault() == 0, () => {
                RuleFor(m => m.FaceValue).Empty().WithMessage(ErrorMessage.DCPFaceValue);
            });
            When(x => x.ApplicationAmount == 0, () => {
                RuleFor(m => m.ApplicationAmount).Empty().WithMessage(ErrorMessage.DCPZeroApplicationAmount);
            });
        }
    }

    public class OffsetTableValidator2 : AbstractValidator<OffsetPaymentTable>
    {
        public OffsetTableValidator2()
        {
            When(x => x.ApplicationAmount == null, () => {
                RuleFor(m => m.ApplicationAmount).NotEmpty().WithMessage(ErrorMessage.DCPApplicationAmount);
            });
            When(x => x.ApplicationAmount.GetValueOrDefault() == 0, () => {
                RuleFor(m => m.ApplicationAmount).Empty().WithMessage(ErrorMessage.DCPApplicationAmount);
            });
        }
    }
}
