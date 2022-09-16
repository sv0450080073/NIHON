using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BookingArrangementListValidator : AbstractValidator<List<BookingArrangementData>>
    {
        public BookingArrangementListValidator()
        {
            RuleForEach(a => a).SetValidator(new BookingArrangementValidator());
        }
    }

    public class BookingArrangementValidator : AbstractValidator<BookingArrangementData>
    {
        public BookingArrangementValidator()
        {
            //task 7501 change
            //RuleFor(a => a.SelectedArrangementLocation).NotEmpty().WithMessage(Constants.ErrorMessage.NotselectedLocationName);
            RuleFor(a => a.LocationName).NotEmpty().WithMessage(Constants.ErrorMessage.NotInputTehaiName);
        }
    }
}
