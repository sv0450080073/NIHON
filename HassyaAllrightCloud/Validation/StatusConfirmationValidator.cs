using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class StatusConfirmationValidator : AbstractValidator<StatusConfirmationData>
    {
        public StatusConfirmationValidator()
        {            
            RuleFor(_ => _.StartDate).Empty()
                .When(_ => _.StartDate.CompareTo(_.EndDate) > 0)
                .WithMessage(Constants.ErrorMessage.SC_StartDateLargerThanEndDate);

            // RuleFor(_ => _.EndDate).Empty()
            //     .When(_ => _.EndDate.CompareTo(_.StartDate) == 0)
            //     .WithMessage(Constants.ErrorMessage.SC_EndDateEqualToStartDate);
            When(_ => _.BranchStart != null && _.BranchEnd != null, 
            () => 
            {
                RuleFor(_ => _.BranchStart).Must((model, start)=> CommonHelper.IsBetween((0, 0), (model.BranchEnd.CompanyCd, model.BranchEnd.EigyoCd), (start.CompanyCd, start.EigyoCd)))
                                           .WithMessage(Constants.ErrorMessage.SC_BranchStartLargerThanBranchEnd);

            });
            //RuleFor(_ => _.BranchStart).Empty()
            //    .When(_ => (_.BranchStart != null && _.BranchEnd != null) && (_.BranchStart.EigyoCd > 0 && _.BranchEnd.EigyoCd != 0 && _.BranchEnd.EigyoCd < _.BranchStart.EigyoCd))
            //    .WithMessage(Constants.ErrorMessage.SC_BranchStartLargerThanBranchEnd);

            //When(_ => _.CustomerStart != null && _.CustomerEnd != null, () => {
            //    RuleFor(_ => _.CustomerStart).Must((model, customer) => CommonHelper.IsBetween((0, 0), (model.CustomerEnd.TokuiCd, model.CustomerEnd.SitenCd), (customer.TokuiCd, customer.SitenCd)))
            //                                 .WithMessage(Constants.ErrorMessage.SC_CustomerStartLargerThanCustomerEnd);
            //});
            // 得意先
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd > x.GyosyaTokuiSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.SC_CustomerStartLargerThanCustomerEnd);
                RuleFor(m => m.GyosyaTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.SC_CustomerStartLargerThanCustomerEnd);
            });
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd > x.TokiskTokuiSakiTo.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.TokiskTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.SC_CustomerStartLargerThanCustomerEnd);
                    RuleFor(m => m.TokiskTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.SC_CustomerStartLargerThanCustomerEnd);
                });
            When(x => ((x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && x.TokiStTokuiSakiFrom != null && x.TokiStTokuiSakiTo != null)
                    && ((x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd > x.TokiStTokuiSakiTo.SitenCd)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq > x.GyosyaTokuiSakiTo.GyosyaCdSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq > x.TokiskTokuiSakiTo.TokuiSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq == x.TokiskTokuiSakiTo.TokuiSeq && x.TokiStTokuiSakiFrom.SitenCdSeq > x.TokiStTokuiSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.SC_CustomerStartLargerThanCustomerEnd);
                        RuleFor(m => m.TokiStTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.SC_CustomerStartLargerThanCustomerEnd);
                    });

            When(_ => (_.CsvConfigOption.Delimiter != null && _.CsvConfigOption.Delimiter.Option == CSV_Delimiter.Other), ()=> {
                RuleFor(_ => _.CsvConfigOption.DelimiterSymbol).NotEmpty()
                .When(_ => string.IsNullOrEmpty(_.CsvConfigOption.DelimiterSymbol))
                .WithMessage(Constants.ErrorMessage.SC_CsvSeparatorIsEmpty);
            });
        }
    }
}
