using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BookingFormValidator : AbstractValidator<BookingFormData>
    {
        public BookingFormValidator()
        {
            RuleFor(bookingdata => bookingdata.CurrentBookingType).Empty()
                .When(t => Convert.ToInt64(t.CurrentBookingType.YoyaKbnSeq) == -1).WithMessage(Constants.ErrorMessage.BookingTypeEmpty);
            RuleFor(bookingdata => bookingdata.SelectedSaleBranch).Empty()
                .When(t => Convert.ToInt64(t.SelectedSaleBranch.EigyoCdSeq) == -1).WithMessage(Constants.ErrorMessage.SaleBranchEmpty);
            RuleFor(bookingdata => bookingdata.SelectedStaff).Empty()
                .When(t => Convert.ToInt64(t.SelectedStaff.SyainCdSeq) == -1).WithMessage(Constants.ErrorMessage.StaffEmpty);
            When(x => (new MyDate(x.BusEndDate, x.BusEndTime).ConvertedDate - new MyDate(x.BusStartDate, x.BusStartTime).ConvertedDate).TotalMinutes < 15, () =>
            {
                RuleFor(m => m.BusEndDate).Empty().WithMessage(Constants.ErrorMessage.BusEndDateTimeGreaterThanBusStartDateTime);
                RuleFor(m => m.BusEndTime.Str).Empty().WithMessage(Constants.ErrorMessage.BusEndDateTimeGreaterThanBusStartDateTime);
            });
            RuleFor(bookingdata => bookingdata.BusStartTime.Str).Empty()
                .When(t => (t.BusStartTime.myHour < 0 || t.BusStartTime.myHour > 23) || (t.BusStartTime.myMinute < 0 || t.BusStartTime.myMinute > 59))
                .WithMessage(Constants.ErrorMessage.BusEndDateTimeGreaterThanBusStartDateTime);
            RuleFor(bookingdata => bookingdata.BusEndTime.Str).Empty()
                .When(t => (t.BusEndTime.myHour < 0 || t.BusEndTime.myHour > 23) || (t.BusEndTime.myMinute < 0 || t.BusEndTime.myMinute > 59))
                .WithMessage(Constants.ErrorMessage.BusEndDateTimeGreaterThanBusStartDateTime);
            RuleFor(bookingdata => bookingdata.BusStartTime.Str)
                .Must((booking, timestr) => booking.BusStartTime <= new MyTime(23, 40))
                .When((booking, timestr) => Convert.ToInt32(booking.ReservationTabData.MovementStatus?.CodeKbn) == 1)
                .WithMessage(Constants.ErrorMessage.StartTimeWhenSplitOneDate);
            RuleFor(bookingdata => bookingdata.BusEndTime.Str)
                .Must((booking, timestr) => booking.BusEndTime >= new MyTime(0, 15) && booking.BusEndTime < new MyTime(24, 00)
                    || booking.BusEndTime >= new MyTime(24, 15))
                .When((booking, timestr) => Convert.ToInt32(booking.ReservationTabData.MovementStatus?.CodeKbn) == 1)
                .WithMessage(Constants.ErrorMessage.EndTimeWhenSplitOneDate);
            RuleForEach(bookingdata => bookingdata.VehicleGridDataList).SetValidator(new HOGridDataValidator());
            RuleFor(bookingdata => bookingdata.ReservationTabData.SyuKoTime.Str).Empty()
                .When(t => new MyDate(t.BusStartDate, t.BusStartTime).ConvertedDate < new MyDate(t.ReservationTabData.GarageLeaveDate, t.ReservationTabData.SyuKoTime).ConvertedDate)
                .WithMessage(Constants.ErrorMessage.GarageLeaveTimeLargerThanBusStartTime);
            RuleFor(bookingdata => bookingdata.ReservationTabData.SyuPatime.Str).Empty()
                .When(t => (new MyDate(t.BusStartDate, t.BusStartTime).ConvertedDate > new MyDate(t.ReservationTabData.GoDate, t.ReservationTabData.SyuPatime).ConvertedDate))
                .WithMessage(Constants.ErrorMessage.DepartureStartTimeLowerThanBusStartTime);
            RuleFor(bookingdata => bookingdata.ReservationTabData.SyuPatime.Str).Empty()
                .When(t => (new MyDate(t.BusEndDate, t.BusEndTime).ConvertedDate <= new MyDate(t.BusStartDate, t.ReservationTabData.SyuPatime).ConvertedDate) && (new MyDate(t.BusEndDate, t.BusEndTime).ConvertedDate - new MyDate(t.BusStartDate, t.BusStartTime).ConvertedDate).TotalMinutes >= 15)
                .WithMessage(Constants.ErrorMessage.DepartureStartTimeLargerThanOrEqualBusEndTime);
            RuleFor(bookingdata => bookingdata.ReservationTabData.KikTime.Str).Empty()
                .When(t => (new MyDate(t.BusEndDate, t.BusEndTime).ConvertedDate > new MyDate(t.ReservationTabData.GarageReturnDate, t.ReservationTabData.KikTime).ConvertedDate))
                .WithMessage(Constants.ErrorMessage.GarageReturnTimeLowerThanBusEndTime);
            RuleFor(bookingdata => bookingdata.ReservationTabData.IkNm).MaximumLength(50)
                .WithMessage(Constants.ErrorMessage.MaxLengthIkNm);
            RuleFor(bookingdata => bookingdata.ReservationTabData.HaiSNm).MaximumLength(50)
                .WithMessage(Constants.ErrorMessage.MaxLengthHaiSNm);
            RuleFor(bookingdata => bookingdata.ReservationTabData.TouNm).MaximumLength(50)
                .WithMessage(Constants.ErrorMessage.MaxLengthTouNm);
            RuleFor(bookingdata => bookingdata.ReservationTabData.GarageLeaveDate)
                .Must((booking, leaveDate) => (booking.BusStartDate - leaveDate).TotalDays < 2 && (booking.BusStartDate - leaveDate).TotalDays >= 0)
                .WithMessage(Constants.ErrorMessage.GarageLeaveDateError);
            RuleFor(bookingdata => bookingdata.ReservationTabData.GarageReturnDate)
                .Must((booking, returnDate) => (returnDate - booking.BusEndDate).TotalDays < 2 && (returnDate - booking.BusEndDate).TotalDays >= 0)
                .WithMessage(Constants.ErrorMessage.GarageReturnDateError);
            RuleFor(bookingdata => bookingdata.ReservationTabData.GoDate)
                .Must((booking, goDate) => goDate >= booking.BusStartDate && goDate <= booking.BusEndDate)
                .WithMessage(Constants.ErrorMessage.DepartureStartTimeLargerThanOrEqualBusEndTime);
            RuleFor(bookingdata => bookingdata.BikoNm).MaximumLength(250)
                .WithMessage(Constants.ErrorMessage.MaxLengthBikoNm);
            RuleForEach(bookingdata => bookingdata.ConfirmationTabDataList).SetValidator(new ConfirmationDataListValidator());
            RuleFor(bookingdata => bookingdata.CancelTickedData.CancelDate)
                .Empty()
                .When(t => (t.CancelTickedData.Status == 2 
                                //|| (t.CancelTickedData.Status == 3 && t.CancelTickedData.CancelStatus)
                                || ((t.CancelTickedData.Status == 1 || t.CancelTickedData.Status == 3) && t.CancelTickedData.CancelStatus)
                           ) 
                           && (t.BusStartDate <= t.CancelTickedData.CancelDate))
                .WithMessage(Constants.ErrorMessage.CancelDateAfterOrEqualStartDate);
            RuleFor(bookingdata => bookingdata.CancelTickedData.CanceledSettingStaffData).NotEmpty()
                .When(t => (t.CancelTickedData.Status == 2 || (t.CancelTickedData.Status == 1 && t.CancelTickedData.CancelStatus)) && (t.CancelTickedData.CanceledSettingStaffData == null))
                .WithMessage(Constants.ErrorMessage.SelectCancellationStaff);           
            RuleFor(bookingdata => bookingdata.CancelTickedData.ReusedSettingStaffData).NotEmpty()
                .When(t => (t.CancelTickedData.Status == 3 || (t.CancelTickedData.Status == 2 && t.CancelTickedData.ReusedStatus)) && (t.CancelTickedData.ReusedSettingStaffData == null))
                .WithMessage(Constants.ErrorMessage.SelectReusedStaff);
            RuleFor(bookingdata => bookingdata.ReservationTabData.MovementStatus).Must(t => t?.CodeKbn != "1")
                .When(t => 
                t.BusStartDate.Day != t.ReservationTabData.GarageLeaveDate.Day 
                || (new MyDate(t.BusEndDate, t.BusEndTime).ConvertedDate.Day != new MyDate(t.ReservationTabData.GarageReturnDate, t.ReservationTabData.KikTime).ConvertedDate.Day))
                .WithMessage(Constants.ErrorMessage.DayBeforeAfterCannotDividedByDay);
            RuleFor(bookingdata => bookingdata.PassengerTypeData).Must(t => t != null).WithMessage(Constants.ErrorMessage.PassengerEmpty);

            RuleFor(bookingdata => bookingdata.TextOrganizationName).MaximumLength(100).WithMessage("BI_T030");

            RuleFor(bookingdata => bookingdata.CancelTickedData.ReusedReason).MaximumLength(50);
            RuleFor(bookingdata => bookingdata.CancelTickedData.CancelReason).MaximumLength(50);
            RuleFor(bookingdata => bookingdata.customerComponentTokiskData).NotEmpty().WithMessage(Constants.ErrorMessage.TokiskNull);
            RuleFor(bookingdata => bookingdata.customerComponentTokiStData).NotEmpty().WithMessage(Constants.ErrorMessage.TokiStNull);
            RuleFor(bookingdata => bookingdata.SupervisorTabData.customerComponentTokiskData).NotEmpty().WithMessage(Constants.ErrorMessage.TokiskNull);
            RuleFor(bookingdata => bookingdata.SupervisorTabData.customerComponentTokiStData).NotEmpty().WithMessage(Constants.ErrorMessage.TokiStNull);
        }
    }
    public class HOGridDataValidator : AbstractValidator<VehicleGridData>
    {
        public HOGridDataValidator()
        {
            RuleFor(hodata => hodata.busTypeData).Empty().WithMessage(Constants.ErrorMessage.BusTypeEmpty)
                .When(t => Convert.ToInt64(t.busTypeData.Katakbn) == -1).WithMessage(Constants.ErrorMessage.BusTypeEmpty);
            RuleFor(hodata => hodata.BusNum).Empty()
                .When(x => (int.Parse(x.BusNum) < 1 || int.Parse(x.BusNum) > 99)).WithMessage(Constants.ErrorMessage.UnitOfBusNotGreaterThanZero);
            When(x => (int.Parse(x.BusNum) != 0) && (int.Parse(x.DriverNum) % int.Parse(x.BusNum) != 0), () =>
            {
                RuleFor(m => m.DriverNum).Equal("0").WithMessage(Constants.ErrorMessage.DriverNotDivisibleIntoBus);
            });
            RuleFor(hodata => hodata.DriverNum).Empty()
                .When(x => (int.Parse(x.DriverNum) < 1 || int.Parse(x.DriverNum) > 99)).WithMessage(Constants.ErrorMessage.DriverNotGreaterThanZero);
            RuleFor(hodata => hodata.UnitBusPrice).Matches("^[0-9]+$");
            RuleFor(hodata => hodata.UnitBusFee).Matches("^[0-9]+$");
            RuleFor(hodata => hodata.UnitGuiderFee).Matches("^[0-9]+$");
            RuleFor(hodata => hodata.UnitBusPrice).Empty().
                When(hodata => hodata.minMaxForm.getMaxUnitBusPriceDiscount() != 0 && 
                    ((hodata.minMaxForm.getMaxUnitBusPriceDiscount() < int.Parse(hodata.UnitBusPrice)) || (hodata.minMaxForm.getMinUnitBusPriceDiscount() > int.Parse(hodata.UnitBusPrice)))).WithMessage(Constants.ErrorMessage.UnitBusPriceNotInMinMaxRange);
            RuleFor(hodata => hodata.UnitBusFee).Empty().
                When(hodata => hodata.minMaxForm.getMaxUnitBusFeeDiscount() != 0 && 
                    ((hodata.minMaxForm.getMaxUnitBusFeeDiscount() < int.Parse(hodata.UnitBusFee)) || (hodata.minMaxForm.getMinUnitBusFeeDiscount() > int.Parse(hodata.UnitBusFee)))).WithMessage(Constants.ErrorMessage.UnitBusFeeNotInMinMaxRange);

            RuleFor(hodata => hodata.BusNum).MaximumLength(2).WithMessage("BI_T030");
            RuleFor(hodata => hodata.DriverNum).MaximumLength(2).WithMessage("BI_T030");
            RuleFor(hodata => hodata.UnitBusPrice).MaximumLength(7).WithMessage("BI_T030");
            RuleFor(hodata => hodata.UnitBusFee).MaximumLength(7).WithMessage("BI_T030");
            RuleFor(hodata => hodata.GuiderNum).MaximumLength(2).WithMessage("BI_T030");
            RuleFor(hodata => hodata.UnitGuiderFee).MaximumLength(7).WithMessage("BI_T030");
            RuleFor(hodata => hodata.BusNum).Empty()
                .When(x => (int.Parse(x.BusNum) > 1 && int.Parse(x.BusNum) < x.BusNumNippoKbn)).WithMessage("BI_T031");
        }
    }
    public class MinMaxSettingFormValidate : AbstractValidator<MinMaxSettingFormData>
    {
        public MinMaxSettingFormValidate()
        {
            RuleFor(t => t.SelectedTranSportationOfficePlace).NotEmpty().WithMessage(Constants.ErrorMessage.TranSportationOfficePlaceEmpty);
            RuleFor(t => t.UnitPrice).Empty().
                When(t => ((t.getMaxUnitBusPriceDiscount() < int.Parse(t.UnitPrice)) || (t.getMinUnitBusPriceDiscount() > int.Parse(t.UnitPrice)))).WithMessage(Constants.ErrorMessage.UnitPriceNotInMinMaxRange);
            RuleFor(t => t.UnitFee).Empty().
                When(t => ((t.getMaxUnitBusFeeDiscount() < int.Parse(t.UnitFee)) || (t.getMinUnitBusFeeDiscount() > int.Parse(t.UnitFee)))).WithMessage(Constants.ErrorMessage.UnitFeeNotInMinMaxRange);
            RuleForEach(minmaxdata => minmaxdata.minMaxGridData).SetValidator(new MinMaxGridDataValidator());
            RuleFor(t => t.TotalKmRunning)
                .LessThanOrEqualTo(Constants.MaximumOfKmRunning)
                .WithMessage(Constants.ErrorMessage.SumKmRunningOutOfRange);
            RuleFor(t => t.TotalExactKmRunning)
                .LessThanOrEqualTo(Constants.MaximumOfKmRunning)
                .WithMessage(Constants.ErrorMessage.SumExactKmRunningOutOfRange);
            RuleFor(t => t.TotalKmRunningWithChangeDriver)
                .LessThanOrEqualTo(Constants.MaximumOfKmRunning)
                .WithMessage(Constants.ErrorMessage.SumKmRunningOutOfRange);
        }
    }
    public class MinMaxGridDataValidator : AbstractValidator<MinMaxGridData>
    {
        public MinMaxGridDataValidator()
        {
            When(v => v.BusInspectionStartDate.ConvertedDate > v.BusInspectionEndDate.ConvertedDate,() => {
                RuleFor(v => v.BusInspectionStartDate.inpTime.Str).Empty()
                .WithMessage(Constants.ErrorMessage.BusInspectionStartDateGreaterThanEndDate);
            });

            RuleFor(v => v.BusInspectionStartDate.inpTime.Str).Empty()
                .When(v => v.BusInspectionStartDate.inpTime.Str == "--:--")
                 .WithMessage(Constants.ErrorMessage.BusInspectionStartDateGreaterThanEndDate);

            RuleFor(v => v.BusInspectionEndDate.inpTime.Str).Empty()
                .When(v => v.BusInspectionEndDate.inpTime.Str == "--:--")
                 .WithMessage(Constants.ErrorMessage.BusInspectionStartDateGreaterThanEndDate);

            //RuleFor(VehicleGridDataList => VehicleGridDataList.ExactKmRunning).LessThanOrEqualTo(VehicleGridDataList => VehicleGridDataList.KmRunning)
            //    .WithMessage(Constants.ErrorMessage.KmRunningLessThanExactKmRunning);

            RuleFor(v => v.ExactKmRunningStr).Empty().
                 When(v => v.KmRunning < v.ExactKmRunning).WithMessage(Constants.ErrorMessage.KmRunningLessThanExactKmRunning);

            RuleFor(v => v.ExactTimeRunning.Str).Empty().
                 When(v => v.TimeRunning < v.ExactTimeRunning).WithMessage(Constants.ErrorMessage.TimeRunningLessThanExactTimeRunning);
            //RuleFor(minMaxGridData => minMaxGridData.KmRunningwithChgDriver).InclusiveBetween(1, 9999).WithMessage("KmRunningwithChgDriver empty");
            //  When(x => x.isChangeDriver == true, () => {
            //       RuleFor(m => m.KmRunningwithChgDriver).Empty().WithMessage("KmRunningwithChgDriver empty");
            //       RuleFor(m => m.TimeRunningwithChgDriver).Empty().WithMessage("TimeRunningwithChgDriver e,pty");
            //       RuleFor(m => m.SpecialTimeRunningwithChgDriver).Empty().WithMessage("SpecialTimeRunningwithChgDriver empty");
            //   });

        }
    }

    public class ReservationTabValidator : AbstractValidator<ReservationTabData>
    {
        public ReservationTabValidator()
        {
        }
    }
    public class ConfirmationDataListValidator : AbstractValidator<ConfirmationTabData>
    {
        public ConfirmationDataListValidator()
        {
            RuleFor(confirmData => confirmData.KaknAit).NotEmpty().WithMessage(Constants.ErrorMessage.InputConfirmationRecipient);
        }
    }
}
