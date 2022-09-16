using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Domain.Dto;
using System;

namespace HassyaAllrightCloud.Validation
{
    public class StaffChartSearchValidator:AbstractValidator<ConfigStaffsChart>
    {
        public StaffChartSearchValidator()
        {
            When(x => (x.DepartureDateTimeEnd < x.DepartureDateTimeStart), () =>
            {
                RuleFor(m => m.DepartureDateTimeEnd).Empty()
                .WithMessage(Constants.ErrorMessage.Departurefromto);
            });
            When(x => (x.DepartureDateTimeEnd < x.DepartureDateTimeStart), () =>
           {
               RuleFor(m => m.DepartureTimeEnd).Empty()
               .WithMessage(Constants.ErrorMessage.Departurefromto);
           });
            When(x => (x.ArrivalDateTimeEnd < x.ArrivalDateTimeStart), () =>
           {
               RuleFor(m => m.ArrivalDateTimeEnd).Empty()
               .WithMessage(Constants.ErrorMessage.Arrivalfromto);
           });
            When(x => (x.ArrivalDateTimeEnd < x.ArrivalDateTimeStart), () =>
           {
               RuleFor(m => m.ArrivalTimeEnd).Empty()
               .WithMessage(Constants.ErrorMessage.Arrivalfromto);
           });
            When(x => (x.DeliveryDateTimeEnd < x.DeliveryDateTimeStart), () =>
           {
               RuleFor(m => m.DeliveryDateTimeEnd).Empty()
               .WithMessage(Constants.ErrorMessage.Deliveryfromto);
           });
            When(x => (x.DeliveryDateTimeEnd < x.DeliveryDateTimeStart), () =>
          {
              RuleFor(m => m.DeliveryTimeEnd).Empty()
              .WithMessage(Constants.ErrorMessage.Deliveryfromto);
          });
            When(x => (x.ReturnDateTimeEnd < x.ReturnDateTimeStart), () =>
          {
              RuleFor(m => m.ReturnDateTimeEnd).Empty()
              .WithMessage(Constants.ErrorMessage.Returnfromto);
          });
            When(x => (x.ReturnDateTimeEnd < x.ReturnDateTimeStart), () =>
          {
              RuleFor(m => m.ReturnTimeEnd).Empty()
              .WithMessage(Constants.ErrorMessage.Returnfromto);
          });
        }
    }
}
