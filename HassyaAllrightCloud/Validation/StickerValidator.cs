using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class StickerValidator : AbstractValidator<StickerData>
    {
        public StickerValidator()
        {
        }
    }
}
