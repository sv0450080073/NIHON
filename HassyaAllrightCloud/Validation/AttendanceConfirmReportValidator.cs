
using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class AttendanceConfirmReportValidator : AbstractValidator<AttendanceConfirmReportData>
    {
        public AttendanceConfirmReportValidator()
        {
         
            RuleFor(report => report.CompanyChartData).NotEmpty()
               .When(x => x.CompanyChartData?.Count == 0)
               .WithMessage(Constants.ErrorMessage.CompanyReport);
            When(x => (x.VehicleDispatchOffice1.EigyoCd > x.VehicleDispatchOffice2.EigyoCd && x.VehicleDispatchOffice2.EigyoCd != 0), () =>
            {
                RuleFor(m => m.VehicleDispatchOffice1).Empty().
                WithMessage(Constants.ErrorMessage.BranchFromGreaterThanBranchToReport);
                RuleFor(m => m.VehicleDispatchOffice2).Empty().
                WithMessage(Constants.ErrorMessage.BranchFromGreaterThanBranchToReport);
            });
            When(_ => _.BookingTypeFrom != null && _.BookingTypeTo != null && _.BookingTypeFrom.YoyaKbn > _.BookingTypeTo.YoyaKbn, () => {
                RuleFor(m => m.BookingTypeFrom).Empty().WithMessage(Constants.ErrorMessage.BookingTypeFromGreaterThanBookingTypeToReport);
                RuleFor(m => m.BookingTypeTo).Empty().WithMessage(Constants.ErrorMessage.BookingTypeFromGreaterThanBookingTypeToReport);
            });
            RuleFor(report => report.TxtKeyObjectives).MaximumLength(25).WithMessage("Update Message");
            RuleFor(report => report.TxtInstructions).MaximumLength(25).WithMessage("Update Message");
        }
    }
}
