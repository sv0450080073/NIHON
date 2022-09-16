using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BookingMultiCopyValidator : AbstractValidator<BusBookingMultiCopyData>
    {
        public BookingMultiCopyValidator()
        {
            When(x => (x.IsApplyAll && x.BookingDataToCopy.BusStartDate == x.BookingDataToCopy.BusEndDate && (x.ArrivalTime - x.DispatchTime).TotalMinutes() < 15), () =>
            {
                RuleFor(m => m.ArrivalTime.Str).Empty().WithMessage(Constants.ErrorMessage.CopyJourneyTimeInvalid);
            });
            RuleForEach(x => x.BookingDataChangedList).SetValidator(new BookingChangedListValidator());
            RuleFor(x => x.BookingDataToCopy.customerComponentGyosyaData).NotEmpty().WithMessage(Constants.ErrorMessage.CopyCustomerEmpty);
            RuleFor(x => x.BookingDataToCopy.customerComponentTokiskData).NotEmpty().WithMessage(Constants.ErrorMessage.CopyCustomerEmpty);
            RuleFor(x => x.BookingDataToCopy.customerComponentTokiStData).NotEmpty().WithMessage(Constants.ErrorMessage.CopyCustomerEmpty);
            //RuleFor(x => x.BookingDataToCopy.ReservationTabData.Destination).NotEmpty().WithMessage(Constants.ErrorMessage.CopyDestinationEmpty);
            //RuleFor(x => x.BookingDataToCopy.ReservationTabData.DespatchingPlace).NotEmpty().WithMessage(Constants.ErrorMessage.CopyDispatchPlaceEmpty);
            //RuleFor(x => x.BookingDataToCopy.ReservationTabData.ArrivePlace).NotEmpty().WithMessage(Constants.ErrorMessage.CopyArrivePlaceEmpty);
        }
    }

    public class BookingChangedListValidator : AbstractValidator<MultiCopyDataGrid>
    {
        public BookingChangedListValidator()
        {
            When(x => (new MyDate(x.EndDate, x.EndTime).ConvertedDate - new MyDate(x.StartDate, x.StartTime).ConvertedDate).TotalMinutes < 15, () =>
            {
                RuleFor(m => m.EndTime.Str).Empty().WithMessage(Constants.ErrorMessage.CopyJourneyTimeInvalid);
            });
        }
    }
}
