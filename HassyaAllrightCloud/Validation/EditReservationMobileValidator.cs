using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Validation
{
    public class EditReservationMobileValidator : AbstractValidator<ReservationMobileData>
    {
        public EditReservationMobileValidator()
        {
            RuleFor(_ => _.Tokisk).NotNull().WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T001);
            RuleFor(_ => _.CodeKb).NotNull().WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T002);
            //RuleFor(_ => _.DispatchDate).Must((obj, value) => {
            //    var startTime = obj.DispatchTime.Replace(":", string.Empty);
            //    var start = value.AddHours(int.Parse(startTime.Substring(0, 2))).AddMinutes(int.Parse(startTime.Substring(2)));
            //    var endTime = obj.ArrivalTime.Replace(":", string.Empty);
            //    var end = obj.ArrivalDate.AddHours(int.Parse(endTime.Substring(0, 2))).AddMinutes(int.Parse(endTime.Substring(2)));
            //    return (end - start).TotalMinutes >= 15;
            //}).WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T007);
            RuleFor(_ => _.ArrivalDate).Must((obj, value) => {
                var startTime = obj.DispatchTime.Replace(":", string.Empty);
                var start = obj.DispatchDate.AddHours(int.Parse(startTime.Substring(0, 2))).AddMinutes(int.Parse(startTime.Substring(2)));
                var endTime = obj.ArrivalTime.Replace(":", string.Empty);
                var end = value.AddHours(int.Parse(endTime.Substring(0, 2))).AddMinutes(int.Parse(endTime.Substring(2)));
                return (end - start).TotalMinutes >= 15;
            }).WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T007);
            //RuleFor(_ => _.DispatchTime).Must((obj, value) =>
            //{
            //    var startTime = value.Replace(":", string.Empty);
            //    var start = obj.DispatchDate.AddHours(int.Parse(startTime.Substring(0, 2))).AddMinutes(int.Parse(startTime.Substring(2)));
            //    var endTime = obj.ArrivalTime.Replace(":", string.Empty);
            //    var end = obj.ArrivalDate.AddHours(int.Parse(endTime.Substring(0, 2))).AddMinutes(int.Parse(endTime.Substring(2)));
            //    return (end - start).TotalMinutes >= 15;
            //}).WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T007);
            RuleFor(_ => _.ArrivalTime).Must((obj, value) =>
            {
                var startTime = obj.DispatchTime.Replace(":", string.Empty);
                var start = obj.DispatchDate.AddHours(int.Parse(startTime.Substring(0, 2))).AddMinutes(int.Parse(startTime.Substring(2)));
                var endTime = value.Replace(":", string.Empty);
                var end = obj.ArrivalDate.AddHours(int.Parse(endTime.Substring(0, 2))).AddMinutes(int.Parse(endTime.Substring(2)));
                return (end - start).TotalMinutes >= 15;
            }).WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T007);
        }
    }

    public class EditReservationMobileChildItemValidator : AbstractValidator<ReservationMobileChildItemData>
    {
        public EditReservationMobileChildItemValidator()
        {
            RuleFor(_ => _.SyaSyu).NotNull().WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T003);
            RuleFor(_ => _.DriverCount).Must((obj, value) => int.Parse(value) % int.Parse(obj.BusCount) == 0).WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T004);
            RuleFor(_ => _.BusCount).Must((obj, value) => int.Parse(obj.DriverCount) % int.Parse(value) == 0).WithMessage(Constants.ErrorMessage.EditReservationMobile_BI_T004);
        }
    }
}
