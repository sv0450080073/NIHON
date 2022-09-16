using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Validation
{
    public class VehicleStatisticsSurveyValidator : AbstractValidator<VehicleStatisticsSurveySearchParam>
    {
        public VehicleStatisticsSurveyValidator()
        {
            RuleFor(_ => _.EigyoFrom).Must((obj, value) => value == null || obj.EigyoTo == null || value.EigyoCd <= obj.EigyoTo.EigyoCd)
                                     .WithMessage(Constants.ErrorMessage.VehicleStatisticsSurvey_BI_T001);

            RuleFor(_ => _.EigyoTo).Must((obj, value) => value == null || obj.EigyoFrom == null || value.EigyoCd >= obj.EigyoFrom.EigyoCd)
                                   .WithMessage(Constants.ErrorMessage.VehicleStatisticsSurvey_BI_T001);

            RuleFor(_ => _.ShippingFrom).Must((obj, value) => value == null || obj.ShippingTo == null || value.CodeKbn <= obj.ShippingTo.CodeKbn)
                                        .WithMessage(Constants.ErrorMessage.VehicleStatisticsSurvey_BI_T002);

            RuleFor(_ => _.ShippingTo).Must((obj, value) => value == null || obj.ShippingFrom == null || value.CodeKbn >= obj.ShippingFrom.CodeKbn)
                                      .WithMessage(Constants.ErrorMessage.VehicleStatisticsSurvey_BI_T002);
        }
    }
}
