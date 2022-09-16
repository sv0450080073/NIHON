using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Domain.Dto;
using System;
using HassyaAllrightCloud.Commons.Helpers;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BusAllocationValidatorSearch : AbstractValidator<BusAllocationData.BusAllocationSearch>
    {
        public BusAllocationValidatorSearch()
        {
            /* When(v => v.startTime.CompareTo(v.endTime) > 0, () =>
             {
                 RuleFor(v => v.endTime).Empty()
                 .WithMessage(Constants.ErrorMessage.CheckStartTimeAndEndTimeCondition);
             });

             When(v => int.Parse(v.bookingfrom) >int.Parse(v.bookingto), () =>
             {
                 RuleFor(v => v.bookingto).Empty()
                 .WithMessage(Constants.ErrorMessage.CheckStartReceptionNumberAndEndReceptionNumberCondition);
             });
             When(v => v.ReservationClassification1!=null && v.ReservationClassification2!=null, () =>
              {
                  When(v => v.ReservationClassification1.YoyaKbn > v.ReservationClassification2.YoyaKbn, () =>
              {
                  RuleFor(v => v.ReservationClassification2).Empty()
                  .WithMessage(Constants.ErrorMessage.CheckingTheStartAndEndReservationClassification);
              });
             });*/
            When(_ => _.BookingTo!=null && _.BookingFrom!=null, () =>
            {
                RuleFor(_ => _.BookingFrom).Must((model, booking) => CommonHelper.IsBetween((0, 0), (0, model.BookingTo.YoyaKbn), (0, booking.YoyaKbn)))
                                          .WithMessage(Constants.ErrorMessage.CheckReservation);
                RuleFor(_ => _.BookingTo).Must((model, booking) => CommonHelper.IsBetween((0, 0), (0, booking.YoyaKbn), (0, model.BookingFrom.YoyaKbn)))
                                          .WithMessage(Constants.ErrorMessage.CheckReservation);
            });
        }
    }
    public class BusAllocationValidator : AbstractValidator<BusBookingDataAllocation>
    {
        public BusAllocationValidator()
        {
            RuleFor(v => v.Haisha_DanTaNm2).MaximumLength(100);
            RuleFor(v => v.HaishaExp_HaisBinNm).MaximumLength(20);
            RuleFor(v => v.HaishaExp_TouSBinNm).MaximumLength(20);
            RuleFor(v => v.Haisha_IkNm).MaximumLength(50);
            RuleFor(v => v.Haisha_HaiSNm).MaximumLength(50);
            RuleFor(v => v.Haisha_HaiSJyus1).MaximumLength(30);
            RuleFor(v => v.Haisha_HaiSJyus1).MaximumLength(30);
            RuleFor(v => v.Haisha_HaiSKigou).MaximumLength(6);

            RuleFor(v => v.Haisha_TouNm).MaximumLength(50);
            RuleFor(v => v.Haisha_TouJyusyo1).MaximumLength(30);
            RuleFor(v => v.Haisha_TouJyusyo2).MaximumLength(30);
            RuleFor(v => v.Haisha_TouKigou).MaximumLength(6);

            RuleFor(v => v.Haisha_PlatNo).MaximumLength(20);
            When(v => v.Haisha_HaiSYmd.CompareTo(v.Unkobi_HaiSYmd) < 0 || (v.Haisha_HaiSYmd.CompareTo(v.Unkobi_HaiSYmd) == 0 && v.Haisha_HaiSTime.CompareTo(v.Unkobi_HaiSTime) < 0), () =>
            {
                RuleFor(v => v.DeliveryDate).Empty()
                .WithMessage(Constants.ErrorMessage.CheckHaishaSymdandUnkobiSymd);
                RuleFor(v => v.Haisha_HaiSTime).Empty()
               .WithMessage(Constants.ErrorMessage.CheckHaishaSymdandUnkobiSymd);
            });
            When(v => v.Haisha_SyuKoYmd.CompareTo(v.Haisha_HaiSYmd) > 0 || (v.Haisha_SyuKoYmd.CompareTo(v.Haisha_HaiSYmd) == 0 && v.Haisha_SyuKoTime.CompareTo(v.Haisha_HaiSTime) > 0), () =>
            {
                RuleFor(v => v.DeliveryDate).NotEmpty()
                .WithMessage(Constants.ErrorMessage.CheckingTheDeliveryDateTimeAndDispatchTime);
                RuleFor(v => v.Haisha_HaiSTime).Empty()
               .WithMessage(Constants.ErrorMessage.CheckingTheDeliveryDateTimeAndDispatchTime);
            });

            When(v => v.Haisha_TouYmd.CompareTo(v.Haisha_KikYmd) > 0 || (v.Haisha_TouYmd.CompareTo(v.Haisha_KikYmd) == 0 && v.Haisha_TouChTime.CompareTo(v.Haisha_KikTime) > 0), () =>
            {
                RuleFor(v => v.ArrivalDate).NotEmpty()
                .WithMessage(Constants.ErrorMessage.CheckingTheDateAndtTimeOfReturnAndArrival);
                RuleFor(v => v.Haisha_TouChTime).Empty()
               .WithMessage(Constants.ErrorMessage.CheckingTheDateAndtTimeOfReturnAndArrival);
            });

            When(v => v.Haisha_TouYmd.CompareTo(v.Unkobi_TouYmd) > 0 || (v.Haisha_TouYmd.CompareTo(v.Unkobi_TouYmd) == 0 && v.Haisha_TouChTime.CompareTo(v.Unkobi_TouChTime) > 0), () =>
            {
                RuleFor(v => v.ArrivalDate).NotEmpty()
                .WithMessage(Constants.ErrorMessage.CheckHaishaTouYmdandUnkobiTouYmd);
                RuleFor(v => v.Haisha_TouChTime).Empty()
               .WithMessage(Constants.ErrorMessage.CheckHaishaTouYmdandUnkobiTouYmd);
            });

            When(v => (v.Haisha_TouYmd.CompareTo(v.Haisha_SyuKoYmd) < 0 || ((v.Haisha_TouYmd.CompareTo(v.Haisha_SyuKoYmd) == 0 && v.Haisha_TouChTime.CompareTo(v.Haisha_SyuKoTime) <= 0))) ||
                       (v.Haisha_TouYmd.CompareTo(v.Haisha_HaiSYmd) < 0 || ((v.Haisha_TouYmd.CompareTo(v.Haisha_HaiSYmd) == 0 && v.Haisha_TouChTime.CompareTo(v.Haisha_HaiSTime) <= 0))), () =>
                       {
                           RuleFor(v => v.ArrivalDate).NotEmpty()
                           .WithMessage(Constants.ErrorMessage.CheckArrivalTimeWithDispatchTimeAndDepartureTime);
                           RuleFor(v => v.Haisha_TouChTime).Empty()
                          .WithMessage(Constants.ErrorMessage.CheckArrivalTimeWithDispatchTimeAndDepartureTime);
                       });

            When(v => (v.Haisha_KikYmd.CompareTo(v.Haisha_SyuKoYmd) < 0 || ((v.Haisha_KikYmd.CompareTo(v.Haisha_SyuKoYmd) == 0 && v.Haisha_KikTime.CompareTo(v.Haisha_SyuKoTime) <= 0))) ||
                       (v.Haisha_KikYmd.CompareTo(v.Haisha_HaiSYmd) < 0 || ((v.Haisha_KikYmd.CompareTo(v.Haisha_HaiSYmd) == 0 && v.Haisha_KikTime.CompareTo(v.Haisha_HaiSTime) <= 0))), () =>
                       {
                           RuleFor(v => v.ReturnDate).NotEmpty()
                           .WithMessage(Constants.ErrorMessage.CheckingTheReturnTimeTogetherWithTheDispatchTimeAndDepartureTime);
                           RuleFor(v => v.Haisha_KikTime).Empty()
                          .WithMessage(Constants.ErrorMessage.CheckingTheReturnTimeTogetherWithTheDispatchTimeAndDepartureTime);
                       });
        }
    }
    public class BusAllocationUpdateValidator : AbstractValidator<BusAllocationDataUpdate>
    {
        public BusAllocationUpdateValidator()
        {
            When(x => ((x.CheckHaiSHaiSha - x.CheckSyuKoHaiSha).TotalDays>1), () =>
            {
                RuleFor(m => m.HaiSYmd).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiShaWithSyuKoHaiSha);
                RuleFor(m => m.HaiSTime).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiShaWithSyuKoHaiSha);
            });
            When(x => ((x.CheckKikHaiSha - x.CheckTouHaiSha).TotalDays>1), () =>
            {
                RuleFor(m => m.KikTime).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithTouChHaiSha);
                RuleFor(m => m.KikYmd).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithTouChHaiSha);
            });
            When(x => (x.CheckSyuKoHaiSha < x.CheckSyuKoUnkobi), () =>
            {
                RuleFor(m => m.SyuKoYmd).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiShaWithSyuKoUnkoBi);
                RuleFor(m=>m.SyuKoTime).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiShaWithSyuKoUnkoBi);
            });
            When(x => (x.CheckSyuKoHaiSha > x.CheckHaiSHaiSha), () =>
            {
                RuleFor(m => m.SyuKoYmd).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiSha);
                RuleFor(m => m.SyuKoTime).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiSha);
            });
            When(x => (x.CheckHaiSHaiSha < x.CheckHaiSUnkobi), () =>
            {
                RuleFor(m => m.HaiSYmd).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiShaWithHaiSUnkoBi);
                RuleFor(m => m.HaiSTime).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiShaWithHaiSUnkoBi);
            });
            When(x => (x.CheckHaiSHaiSha > x.CheckTouHaiSha), () =>
            {
                RuleFor(m => m.HaiSYmd).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiSha);
                RuleFor(m => m.HaiSTime).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiSha);
            });
            When(x => (x.CheckHaiSHaiSha < x.CheckSyuKoHaiSha), () =>
            {
                RuleFor(m => m.HaiSYmd).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiSha);
                RuleFor(m => m.HaiSTime).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiSha);
            });
            When(x => (x.CheckTouHaiSha > x.CheckTouUnkobi), () =>
            {
                RuleFor(m => m.TouYmd).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiShaWithTouChUnkoBi);
                RuleFor(m => m.TouChTime).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiShaWithTouChUnkoBi);
            });
            When(x => (x.CheckTouHaiSha < x.CheckHaiSHaiSha), () =>
            {
                RuleFor(m => m.TouYmd).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiSha);
                RuleFor(m => m.TouChTime).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiSha);
            });
            When(x => (x.CheckTouHaiSha > x.CheckKikHaiSha), () =>
            {
                RuleFor(m => m.TouYmd).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiSha);
                RuleFor(m => m.TouChTime).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiSha);
            });
            When(x => (x.CheckKikHaiSha > x.CheckKikUnkobi), () =>
            {
                RuleFor(m => m.KikYmd).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithKikoUnkoBi);
                RuleFor(m => m.KikTime).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithKikoUnkoBi);
            });
            When(x => (x.CheckKikHaiSha < x.CheckTouHaiSha), () =>
            {
                RuleFor(m => m.KikYmd).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiSha);
                RuleFor(m => m.KikTime).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiSha);
            });
            When(x => ((x.CheckTouHaiSha - x.CheckHaiSHaiSha).TotalMinutes < 15), () =>
            {
                RuleFor(m => m.TouYmd).Empty().WithMessage(Constants.ErrorMessage.CheckTouchHaiShaSubtractionHaiSHaiSha);
                RuleFor(m => m.TouChTime).Empty().WithMessage(Constants.ErrorMessage.CheckTouchHaiShaSubtractionHaiSHaiSha);
            });
        }
    }
    public class BusAllocationDataUpdateAllValidator : AbstractValidator<BusAllocationDataUpdateAll>
    {
        public BusAllocationDataUpdateAllValidator()
        {
            When(x => ((x.CheckHaiSHaiSha - x.CheckSyuKoHaiSha).TotalDays >1), () =>
              {
                  RuleFor(m => m.HaiSYmd).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiShaWithSyuKoHaiSha);
                  RuleFor(m => m.HaiSTime).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiShaWithSyuKoHaiSha);
              });
            When(x => ((x.CheckKikHaiSha - x.CheckTouHaiSha).TotalDays > 1), () =>
              {
                  RuleFor(m => m.KikTime).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithTouChHaiSha);
                  RuleFor(m => m.KikYmd).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithTouChHaiSha);
              });
            When(x => (x.CheckSyuKoHaiSha < x.CheckSyuKoUnkobi && !x.DisableSyukoDateTime), () =>
            {
                RuleFor(m => m.SyuKoYmd).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiShaWithSyuKoUnkoBi);
                RuleFor(m => m.SyuKoTime).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiShaWithSyuKoUnkoBi);
            });
            When(x => (x.CheckHaiSHaiSha < x.CheckHaiSUnkobi && !x.DisableHaiSDateTime), () =>
            {
                RuleFor(m => m.HaiSYmd).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiShaWithHaiSUnkoBi);
                RuleFor(m => m.HaiSTime).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiShaWithHaiSUnkoBi);
            });
            When(x => (x.CheckTouHaiSha > x.CheckTouUnkobi && !x.DisableTouDateTime), () =>
            {
                RuleFor(m => m.TouYmd).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiShaWithTouChUnkoBi);
                RuleFor(m => m.TouChTime).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiShaWithTouChUnkoBi);
            });
            When(x => (x.CheckKikHaiSha > x.CheckKikUnkobi && !x.DisableKikDateTime), () =>
            {
                RuleFor(m => m.KikYmd).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithKikoUnkoBi);
                RuleFor(m => m.KikTime).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithKikoUnkoBi);
            });
            When(x => ((x.CheckTouHaiSha - x.CheckHaiSHaiSha).TotalMinutes < 15 && !x.DisableTouDateTime && !x.DisableHaiSDateTime), () =>
            {
                RuleFor(m => m.TouYmd).Empty().WithMessage(Constants.ErrorMessage.CheckTouchHaiShaSubtractionHaiSHaiSha);
                RuleFor(m => m.TouChTime).Empty().WithMessage(Constants.ErrorMessage.CheckTouchHaiShaSubtractionHaiSHaiSha);
            });

            When(x => (x.CheckSyuKoHaiSha > x.CheckHaiSHaiSha && !x.DisableHaiSDateTime && !x.DisableSyukoDateTime), () =>
            {
                RuleFor(m => m.HaiSYmd).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiSha);
                RuleFor(m => m.HaiSTime).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiSha);
            });

            When(x => (x.CheckHaiSHaiSha > x.CheckSyuPaHaiSha && x.CheckSyuPaHaiSha > x.CheckTouHaiSha && !x.DisableHaiSDateTime && !x.DisableSyaPaTime), () =>
            {
                RuleFor(m => m.SyuPaTime).Empty().WithMessage(Constants.ErrorMessage.CheckTouchHaiShaSubtractionHaiSHaiSha);
            });

            When(x => (x.CheckTouHaiSha > x.CheckKikHaiSha && !x.DisableKikDateTime && !x.DisableTouDateTime), () =>
            {
                RuleFor(m => m.KikYmd).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithKikoUnkoBi);
                RuleFor(m => m.KikTime).Empty().WithMessage(Constants.ErrorMessage.CheckKikoHaiShaWithKikoUnkoBi);
            });
            //RuleForEach(haishaAll => haishaAll.DateTimeHaiShaList).SetValidator(new DateHaiShaListValidator());
        }
    }
    public class DateHaiShaListValidator : AbstractValidator<DateTimeHaiShaItem>
    {
        public DateHaiShaListValidator()
        {
            When(x => (x.CheckSyukoHaiShaForm > x.CheckHaiSHaiSha && !x.DisableSyukoDateTime), () =>
            {
                RuleFor(x => x.CheckSyukoHaiShaForm).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiSha);
            });
            When(x => (x.CheckHaiSHaiShaForm < x.CheckSyukoHaiSha && !x.DisableHaiSDateTime), () =>
            {
                RuleFor(x => x.CheckHaiSHaiShaForm).Empty().WithMessage(Constants.ErrorMessage.CheckSyuKoHaiSha);
            });
            When(x => (x.CheckHaiSHaiShaForm > x.CheckTouHaiSha && !x.DisableHaiSDateTime), () =>
            {
                RuleFor(x => x.CheckHaiSHaiShaForm).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiSha);
            });
            When(x => (x.CheckTouHaiShaForm < x.CheckHaiSHaiSha && !x.DisableTouDateTime), () =>
            {
                RuleFor(x => x.CheckTouHaiShaForm).Empty().WithMessage(Constants.ErrorMessage.CheckHaiSHaiSha);
            });
            When(x => (x.CheckTouHaiShaForm > x.CheckKikHaiSha && !x.DisableTouDateTime), () =>
            {
                RuleFor(x => x.CheckTouHaiShaForm).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiSha);
            });
            When(x => (x.CheckKikHaiShaForm < x.CheckTouHaiSha && !x.DisableKikDateTime), () =>
            {
                RuleFor(x => x.CheckKikHaiShaForm).Empty().WithMessage(Constants.ErrorMessage.CheckTouChHaiSha);
            });
            When(x => ((x.CheckTouHaiShaForm - x.CheckHaiSHaiSha).TotalMinutes < 15 && (!x.DisableTouDateTime || !x.DisableHaiSDateTime)), () =>
            {
                RuleFor(x => x.CheckTouHaiShaForm).Empty().WithMessage(Constants.ErrorMessage.CheckTouchHaiShaSubtractionHaiSHaiSha);
            });
        }
    }
}
