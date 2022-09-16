
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
    public class OperatingInstructionReportValidator : AbstractValidator<OperatingInstructionReportData>
    {
        public OperatingInstructionReportValidator()
        {
            //When(v => int.Parse(v.ReceiptNumberFrom==""?"0": v.ReceiptNumberFrom) > int.Parse(v.ReceiptNumberTo==""?"0": v.ReceiptNumberTo), () =>
            //{
            //    RuleFor(v => v.ReceiptNumberTo).Empty()
            //    .WithMessage(Constants.ErrorMessage.CompairDate);
            //});
          /* RuleFor(v => v.ReceiptNumberFrom).Empty()
                .When(v=>v.ReceiptNumberFrom=="").WithMessage(Constants.ErrorMessage.UkenoNull);
            RuleFor(v => v.ReceiptNumberTo).Empty()
                 .When(v => v.ReceiptNumberTo == "").WithMessage(Constants.ErrorMessage.UkenoNull);*/
            //When(_ => (!string.IsNullOrEmpty(_.ReceiptNumberFrom) || !string.IsNullOrEmpty(_.ReceiptNumberTo)), () =>
            //{
            //    RuleFor(_ => _.ReceiptNumberTo).Must((model, uke) => !string.IsNullOrEmpty(uke) &&
            //                                             !string.IsNullOrEmpty(model.ReceiptNumberFrom) &&
            //                                             int.Parse(uke) >= int.Parse(model.ReceiptNumberFrom))
            //    .WithMessage(Constants.ErrorMessage.CompairDate);
            //});
            // 受付番号
            When(x => !String.IsNullOrEmpty(x.ReceiptNumberFrom) && !String.IsNullOrEmpty(x.ReceiptNumberTo) && long.Parse(x.ReceiptNumberFrom) > long.Parse(x.ReceiptNumberTo), () =>
            {
                RuleFor(m => m.ReceiptNumberFrom).Empty().WithMessage(Constants.ErrorMessage.CompairDate);
                RuleFor(m => m.ReceiptNumberTo).Empty().WithMessage(Constants.ErrorMessage.CompairDate);
            });
            // ​予約区分
            When(x => x.YoyakuFrom != null && x.YoyakuTo != null && x.YoyakuFrom.YoyaKbn > x.YoyakuTo.YoyaKbn, () =>
            {
                RuleFor(m => m.YoyakuFrom).Empty().WithMessage(Constants.ErrorMessage.CompairCategory);
                RuleFor(m => m.YoyakuTo).Empty().WithMessage(Constants.ErrorMessage.CompairCategory);
            });
        }
    }
}
