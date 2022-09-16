using FluentValidation;
using HassyaAllrightCloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class LineUserValidator : AbstractValidator<LineUser>
    {
        public LineUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstNameRequired");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastNameRequired");
            RuleFor(x => x.AccessToken).NotEmpty().WithMessage("AccessTokenRequired");
        }
    }
}
