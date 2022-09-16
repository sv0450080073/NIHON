using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Validation
{
    public class BusTypeListValidator : AbstractValidator<BusTypeListData>
    {
        public BusTypeListValidator()
        {
            //Branches
            When(x => (x.BranchStart != null && x.BranchEnd != null && x.BranchStart.EigyoCd > x.BranchEnd.EigyoCd && x.BranchEnd.EigyoCd != 0), () =>
            {
                RuleFor(m => m.BranchEnd).Empty().
                WithMessage(Constants.ErrorMessage.BranchFromGreaterThanBranchToReportBusTypeList);
            });
            //SaleStaff Start
            When(x => (x.SalesStaffStart != null && x.SalesStaffEnd != null && int.Parse(x.SalesStaffStart.SyainCd ??"0") > int.Parse(x.SalesStaffEnd.SyainCd ??"0") && int.Parse(x.SalesStaffEnd.SyainCd ?? "0") != 0), () =>
            {
                RuleFor(m => m.SalesStaffEnd).Empty().
                WithMessage(Constants.ErrorMessage.SaleStaffFromGreaterThanSaleStaffToReportBusTypeList);
            });
            //SaleStaff person input
            When(x => (x.PersonInputStart != null && x.PersonInputEnd != null && int.Parse(x.PersonInputStart.SyainCd ?? "0" )  > int.Parse(x.PersonInputEnd.SyainCd ?? "0") && int.Parse(x.PersonInputEnd.SyainCd ?? "0") != 0), () =>
            {
                RuleFor(m => m.PersonInputEnd).Empty().
                WithMessage(Constants.ErrorMessage.SaleStaffFromGreaterThanSaleStaffToReportBusTypeList);
            });
            //VehicleTypes
            When(x => (x.VehicleFrom != null && x.VehicleTo != null && x.VehicleFrom.SyaSyuCd > x.VehicleTo.SyaSyuCd && x.VehicleTo.SyaSyuCd != 0), () =>
            {
                RuleFor(m => m.VehicleTo).Empty().
                WithMessage(Constants.ErrorMessage.VehicleTypeFromGreaterThanVehicleTypeToReportBusTypeList);
            });
            //Destination
            When(x => (x.DestinationStart != null && x.DestinationEnd != null && x.DestinationStart.CodeKbnSeq != 0 && x.DestinationEnd.CodeKbnSeq != 0), () =>
            {
                RuleFor(m => m.DestinationStart).Must((obj, value) => obj.DestinationEnd.BasyoKenCdSeq > value.BasyoKenCdSeq || (obj.DestinationEnd.BasyoKenCdSeq == value.BasyoKenCdSeq && int.Parse(obj.DestinationEnd.BasyoMapCd ?? "0") >= int.Parse(value.BasyoMapCd ?? "0")))
                                                .WithMessage(Constants.ErrorMessage.DestinationStartGreaterThanDestinationToReportBusTypeList);
                RuleFor(m => m.DestinationEnd).Must((obj, value) => obj.DestinationStart.BasyoKenCdSeq < value.BasyoKenCdSeq || (obj.DestinationStart.BasyoKenCdSeq == value.BasyoKenCdSeq && int.Parse(obj.DestinationStart.BasyoMapCd ?? "0") <= int.Parse(value.BasyoMapCd ?? "0")))
                                              .WithMessage(Constants.ErrorMessage.DestinationStartGreaterThanDestinationToReportBusTypeList);
            });
            //Booking type
            When(_ => _.BookingTypeFrom != null && _.BookingTypeTo != null, () => {
                RuleFor(_ => _.BookingTypeFrom).Must((obj, value) => obj.BookingTypeTo.YoyaKbn >= value.YoyaKbn)
                                               .WithMessage(Constants.ErrorMessage.BookingTypeFromGreaterThanBookingTypeToReportBusTypeList);
                RuleFor(_ => _.BookingTypeTo).Must((obj, value) => obj.BookingTypeFrom.YoyaKbn <= value.YoyaKbn)
                                             .WithMessage(Constants.ErrorMessage.BookingTypeFromGreaterThanBookingTypeToReportBusTypeList);
            });



        }
    }
}
