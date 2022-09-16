using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BusReportValidator : AbstractValidator<BusReportData>
    {
        public BusReportValidator()
        {

            RuleFor(report => report.CompanyChartData).NotEmpty()
               .When(x => x.CompanyChartData?.Count == 0)
               .WithMessage(Constants.ErrorMessage.CompanyReport);
            When(x => (x.VehicleDispatchOffice1.EigyoCd > x.VehicleDispatchOffice2.EigyoCd && x.VehicleDispatchOffice2.EigyoCd != 0), () =>
            {
                RuleFor(m => m.VehicleDispatchOffice2).Empty().
                WithMessage(Constants.ErrorMessage.BranchFromGreaterThanBranchToReport);
            });
            When(x => x.BookingFrom.YoyaKbn != 0 && x.BookingTo.YoyaKbn != 0 && x.BookingFrom.YoyaKbn > x.BookingTo.YoyaKbn, () =>
            {
                RuleFor(x => x.BookingFrom).Empty().WithMessage("BI_T004");
                RuleFor(x => x.BookingTo).Empty().WithMessage("BI_T004");
            });
        }
    }
}
