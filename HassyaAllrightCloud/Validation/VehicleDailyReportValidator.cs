using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class VehicleDailyReportValidator : AbstractValidator<VehicleDailyReportData>
    {
        public VehicleDailyReportValidator()
        {
            RuleFor(e => e.TotalMile).Must((obj, v) => decimal.Parse(v) >= decimal.Parse(obj.JisaIPKm) + decimal.Parse(obj.JisaKSKm) + decimal.Parse(obj.KisoIPKm) + decimal.Parse(obj.KisoKOKm))
                                     .WithMessage(Constants.ErrorMessage.InputKiloError);

            RuleFor(e => e.StMeter).Must((obj, v) => v == string.Empty || v == "0.00" || decimal.Parse(obj.EndMeter) >= decimal.Parse(v)).WithMessage(Constants.ErrorMessage.MeterError);
            RuleFor(e => e.EndMeter).Must((obj, v) => obj.StMeter == string.Empty || obj.StMeter == "0.00" || decimal.Parse(obj.StMeter) <= decimal.Parse(v)).WithMessage(Constants.ErrorMessage.MeterError);

            RuleFor(e => e.SyuKoTime).Must((obj, v) => v == string.Empty || v == "00:00" || int.Parse(v.Replace(":", "")) < int.Parse(obj.KikTime.Replace(":", "")))
                                     .WithMessage(Constants.ErrorMessage.KoskuTimeError);
            RuleFor(e => e.KikTime).Must((obj, v) => obj.SyuKoTime == string.Empty || obj.SyuKoTime == "00:00" || int.Parse(v.Replace(":", "")) > int.Parse(obj.SyuKoTime.Replace(":", "")))
                                   .WithMessage(Constants.ErrorMessage.KoskuTimeError);

            RuleFor(e => e.SyuPaTime).Must((obj, v) => v == string.Empty || v == "00:00" || int.Parse(v.Replace(":", "")) < int.Parse(obj.TouChTime.Replace(":", "")))
                                     .WithMessage(Constants.ErrorMessage.JisTimeError);
            RuleFor(e => e.TouChTime).Must((obj, v) => obj.SyuPaTime == string.Empty || obj.SyuPaTime == "00:00" || int.Parse(v.Replace(":", "")) > int.Parse(obj.SyuPaTime.Replace(":", "")))
                                     .WithMessage(Constants.ErrorMessage.JisTimeError);
        }
    }

    public class VehicleDailyReportSearchValidator : AbstractValidator<VehicleDailyReportSearchParam>
    {
        public VehicleDailyReportSearchValidator()
        {
            RuleFor(e => e.ScheduleYmdStart).Must((obj, v) => obj.ScheduleYmdEnd == null || v == null || v <= obj.ScheduleYmdEnd)
                                            .WithMessage(Constants.ErrorMessage.ScheduleYmdError);

            RuleFor(e => e.ScheduleYmdEnd).Must((obj, v) => obj.ScheduleYmdStart == null || v == null || v >= obj.ScheduleYmdStart)
                                            .WithMessage(Constants.ErrorMessage.ScheduleYmdError);

            RuleFor(e => e.selectedBusSaleStart).Must((obj, v) => obj.selectedBusSaleEnd == null || v == null || v.EigyoCd <= obj.selectedBusSaleEnd.EigyoCd)
                                                .WithMessage(Constants.ErrorMessage.BusSaleBranchError);

            RuleFor(e => e.selectedBusSaleEnd).Must((obj, v) => obj.selectedBusSaleStart == null || v == null || v.EigyoCd >= obj.selectedBusSaleStart.EigyoCd)
                                              .WithMessage(Constants.ErrorMessage.BusSaleBranchError);

            RuleFor(e => e.selectedBusCodeStart).Must((obj, v) => obj.selectedBusCodeEnd == null || v == null || v.SyaRyoCd <= obj.selectedBusCodeEnd.SyaRyoCd)
                                                .WithMessage(Constants.ErrorMessage.BusCodeError);

            RuleFor(e => e.selectedBusCodeEnd).Must((obj, v) => obj.selectedBusCodeStart == null || v == null || v.SyaRyoCd >= obj.selectedBusCodeStart.SyaRyoCd)
                                              .WithMessage(Constants.ErrorMessage.BusCodeError);
            
            RuleFor(e => e.ReceptionStart).Must((obj, v) => string.IsNullOrEmpty(obj.ReceptionEnd) || string.IsNullOrEmpty(v) || int.Parse(v) <= int.Parse(obj.ReceptionEnd))
                                          .WithMessage(Constants.ErrorMessage.ReceptionError);

            RuleFor(e => e.ReceptionEnd).Must((obj, v) => string.IsNullOrEmpty(obj.ReceptionStart) || string.IsNullOrEmpty(v) || int.Parse(v) >= int.Parse(obj.ReceptionStart))
                                        .WithMessage(Constants.ErrorMessage.ReceptionError);

            RuleFor(e => e.selectedReservationStart).Must((obj, v) => obj.selectedReservationEnd == null || v == null || v.YoyaKbn <= obj.selectedReservationEnd.YoyaKbn)
                                                    .WithMessage(Constants.ErrorMessage.ReservationError);

            RuleFor(e => e.selectedReservationEnd).Must((obj, v) => obj.selectedReservationStart == null || v == null || v.YoyaKbn >= obj.selectedReservationStart.YoyaKbn)
                                                  .WithMessage(Constants.ErrorMessage.ReservationError);
        }
    }
}
