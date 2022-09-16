using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Validation
{
    public class RepairListValidator : AbstractValidator<RepairListData>
    {
        public RepairListValidator()
        {
            When(x => (x.StartDate > x.EndDate), () =>
            {
                RuleFor(m => m.EndDate).Empty()
                .WithMessage(Constants.ErrorMessage.StartDateTimeGreaterThanEndDateTimeReport);
            });
            When(x => (x.BranchFrom !=null &&  x.BranchTo !=null && x.BranchFrom.EigyoCd > x.BranchTo.EigyoCd && x.BranchTo.EigyoCd != 0), () =>
            {
                RuleFor(m => m.BranchTo).Empty().
                WithMessage(Constants.ErrorMessage.BranchFromGreaterThanBranchToReportRepair);
            });
            When(x => (x.VehicleFrom !=null && x.VehicleTo !=null && x.VehicleFrom.SyaRyoCd > x.VehicleTo.SyaRyoCd && x.VehicleTo.SyaRyoCd != 0), () =>
            {
                RuleFor(m => m.VehicleTo).Empty().
                WithMessage(Constants.ErrorMessage.VehicleFromGreaterThanVehicleToReportRepair);
            });
            When(x => (x.RepairFrom!=null && x.RepairTo !=null && x.RepairFrom.RepairCd > x.RepairTo.RepairCd && x.RepairTo.RepairCd != 0), () =>
            {
                RuleFor(m => m.RepairTo).Empty().
                WithMessage(Constants.ErrorMessage.RepairFromGreaterThanRepairToReportRepair);
            }); 
        }
    }
}
