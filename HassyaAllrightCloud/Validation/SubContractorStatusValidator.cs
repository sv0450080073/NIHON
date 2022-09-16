using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using System.Linq;

namespace HassyaAllrightCloud.Validation
{
    public class SubContractorStatusValidator : AbstractValidator<SubContractorStatusData>
    {
        public SubContractorStatusValidator()
        {
            RuleFor(_ => _.Companies).Must(cp => cp != null && cp.Any())
                                     .WithMessage(Constants.ErrorMessage.SCS_CompanyIsEmpty);

            RuleFor(_ => _.StartDate).Must((model, date) => date.CompareTo(model.EndDate) <= 0)
                                    .WithMessage(Constants.ErrorMessage.SCS_EndDateEarlyThanStartDate);

            When(_ => _.CustomerStart != null && _.CustomerEnd != null, () =>
            {
                RuleFor(_ => _.CustomerStart).Must((model, customer) => CommonHelper.IsBetween((0, 0), (model.CustomerEnd.TokuiCd, model.CustomerEnd.SitenCd), (customer.TokuiCd, customer.SitenCd)))
                                             .WithMessage(Constants.ErrorMessage.SCS_CustomerFromLargerThanCustomerTo);
            });

            When(_ => (!string.IsNullOrEmpty(_.UkeCdFrom) && !string.IsNullOrEmpty(_.UkeCdTo)), () =>
            {
                RuleFor(_ => _.UkeCdTo).Must((model, uke) => int.Parse(uke) >= int.Parse(model.UkeCdFrom))
                                   .WithMessage(Constants.ErrorMessage.SCS_UkeToLessThanUkeFrom);
            });

            When(_ => _.Companies != null, () => {
                RuleFor(_ => _.Companies).Must((model, company) => company.Any())
                                         .WithMessage(Constants.ErrorMessage.SCS_CompanyIsRequired);
            });

            When(_ => _.BranchStart != null && _.BranchEnd != null, () =>
            {
                RuleFor(_ => _.BranchStart).Must((model, branch) => CommonHelper.IsBetween((0, 0), (0, model.BranchEnd.EigyoCd), (0, branch.EigyoCd)))
                                          .WithMessage(Constants.ErrorMessage.SCS_BranchFromLargerThanBranchTo);
            });

            When(_ => _.StaffStart != null && _.StaffEnd != null, () => {
                RuleFor(_ => _.StaffStart).Must((model, staff) => CommonHelper.IsBetween((0, 0), (0, model.StaffEnd.SyainCd), (0, staff.SyainCd)))
                                          .WithMessage(Constants.ErrorMessage.SCS_StaffFromLargerThanStaffTo);
            });

            When(_ => _.ExportType == OutputReportType.CSV && _.CsvConfigOption.Delimiter != null && _.CsvConfigOption.Delimiter.Option == CSV_Delimiter.Other, () => {
                RuleFor(_ => _.CsvConfigOption.DelimiterSymbol).NotEmpty()
                .When(_ => string.IsNullOrEmpty(_.CsvConfigOption.DelimiterSymbol))
                .WithMessage(Constants.ErrorMessage.SCS_CsvSeparatorIsEmpty);
            });

            When(x => x.RegistrationTypeFrom != null && x.RegistrationTypeTo != null && x.RegistrationTypeFrom.YoyaKbn > x.RegistrationTypeTo.YoyaKbn, () =>
            {
                RuleFor(m => m.RegistrationTypeFrom).Empty().WithMessage("BI_T013");
                RuleFor(m => m.RegistrationTypeTo).Empty().WithMessage("BI_T013");
            });

            When(x => x.SelectedGyosyaFrom != null && x.SelectedGyosyaTo != null, () =>
            {
                RuleFor(e => e.SelectedGyosyaTo).Must((obj, value) => value == null || value.GyosyaCd >= obj.SelectedGyosyaFrom.GyosyaCd).WithMessage("BI_T002");
                RuleFor(e => e.SelectedGyosyaFrom).Must((obj, value) => value == null || value.GyosyaCd <= obj.SelectedGyosyaTo.GyosyaCd).WithMessage("BI_T002");
            });

            When(x => x.SelectedTokiskFrom != null && x.SelectedTokiskTo != null &&
                    (x.SelectedGyosyaFrom?.GyosyaCd ?? 0) == (x.SelectedGyosyaTo?.GyosyaCd ?? 999), () =>
                    {
                        RuleFor(e => e.SelectedTokiskTo).Must((obj, value) => value == null || value.TokuiCd >= obj.SelectedTokiskFrom.TokuiCd).WithMessage("BI_T002");
                        RuleFor(e => e.SelectedTokiskFrom).Must((obj, value) => value == null || value.TokuiCd <= obj.SelectedTokiskTo.TokuiCd).WithMessage("BI_T002");
                    });

            When(x => x.SelectedTokiStFrom != null && x.SelectedTokiStTo != null &&
                    (x.SelectedGyosyaFrom?.GyosyaCd ?? 0) == (x.SelectedGyosyaTo?.GyosyaCd ?? 999) &&
                    (x.SelectedTokiskFrom?.TokuiCd ?? 0) == (x.SelectedTokiskTo?.TokuiCd ?? 9999), () =>
                    {
                        RuleFor(e => e.SelectedTokiStTo).Must((obj, value) => value == null || value.SitenCd >= obj.SelectedTokiStFrom.SitenCd).WithMessage("BI_T002");
                        RuleFor(e => e.SelectedTokiStFrom).Must((obj, value) => value == null || value.SitenCd <= obj.SelectedTokiStTo.SitenCd).WithMessage("BI_T002");
                    });
        }
    }
}
