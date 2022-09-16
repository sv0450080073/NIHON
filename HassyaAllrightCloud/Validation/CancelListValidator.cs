using FluentValidation;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using System.Linq;

namespace HassyaAllrightCloud.Validation
{
    public class CancelListValidator : AbstractValidator<CancelListData>
    {
        public CancelListValidator()
        {
            // RuleFor(_ => _.BookingTypeFrom).Must(type => type != null).WithMessage(Constants.ErrorMessage.CL_BookingTypeEmpty);
            // RuleFor(_ => _.BookingTypeTo).Must(type => type != null).WithMessage(Constants.ErrorMessage.CL_BookingTypeEmpty);
            //RuleFor(_ => _.Company).Must(company => company != null).WithMessage(Constants.ErrorMessage.CL_CompanyEmpty);
            //RuleFor(_ => _.CancelBookingType).Must(type => type != null).WithMessage(Constants.ErrorMessage.CL_CancelBookingTypeEmpty);
            //RuleFor(_ => _.BranchStart).Must(branch => branch != null).WithMessage(Constants.ErrorMessage.CL_BranchEmpty);
            //RuleFor(_ => _.BranchEnd).Must(branch => branch != null).WithMessage(Constants.ErrorMessage.CL_BranchEmpty);
            // RuleFor(_ => _.StaffStart).Must(staff => staff != null).WithMessage(Constants.ErrorMessage.CL_StaffEmpty);
            // RuleFor(_ => _.StaffEnd).Must(staff => staff != null).WithMessage(Constants.ErrorMessage.CL_StaffEmpty);
            // RuleFor(_ => _.CustomerStart).Must(customer => customer != null).WithMessage(Constants.ErrorMessage.CL_CustomerOrSupplierEmpty);
            // RuleFor(_ => _.CustomerEnd).Must(customer => customer != null).WithMessage(Constants.ErrorMessage.CL_CustomerOrSupplierEmpty);
            // RuleFor(_ => _.SupplierStart).Must(supplier => supplier != null).WithMessage(Constants.ErrorMessage.CL_CustomerOrSupplierEmpty);
            // RuleFor(_ => _.SupplierEnd).Must(supplier => supplier != null).WithMessage(Constants.ErrorMessage.CL_CustomerOrSupplierEmpty);
            // RuleFor(_ => _.CancelStaffStart).Must(staff => staff != null).WithMessage(Constants.ErrorMessage.CL_StaffEmpty);
            // RuleFor(_ => _.CancelStaffEnd).Must(staff => staff != null).WithMessage(Constants.ErrorMessage.CL_StaffEmpty);
            RuleFor(_ => _.EndDate).Must((model, date) => date.CompareTo(model.StartDate) >= 0).WithMessage(Constants.ErrorMessage.CL_EndDateEarlyThanStartDate);
            RuleFor(_ => _.StartDate).Must((model, date) => date.CompareTo(model.EndDate) <= 0).WithMessage(Constants.ErrorMessage.CL_EndDateEarlyThanStartDate);
            
            When(_ => (!string.IsNullOrEmpty(_.UkeCdFrom) || !string.IsNullOrEmpty(_.UkeCdTo)), () =>
            {
                RuleFor(_ => _.UkeCdTo).Must((model, uke) => !string.IsNullOrEmpty(uke) &&
                                                         !string.IsNullOrEmpty(model.UkeCdFrom) &&
                                                         int.Parse(uke) >= int.Parse(model.UkeCdFrom)).WithMessage(Constants.ErrorMessage.CL_UkeToLessThanUkeFrom);
            });
            When(x => x.YoyakuFrom != null && x.YoyakuTo != null && x.YoyakuFrom.YoyaKbn > x.YoyakuTo.YoyaKbn, () =>
            {
                RuleFor(m => m.YoyakuFrom).Empty().WithMessage(Constants.ErrorMessage.CL_BookingTypeFromLargerThanBookingTypeTo);
                RuleFor(m => m.YoyakuTo).Empty().WithMessage(Constants.ErrorMessage.CL_BookingTypeFromLargerThanBookingTypeTo);
            });

            When(_ => _.BranchStart != null && _.BranchEnd != null, ()=> { 
                RuleFor(_=> _.BranchStart).Must((model, branch) => CommonHelper.IsBetween((0, 0), (0, model.BranchEnd.EigyoCd), (0, branch.EigyoCd)))
                                          .WithMessage(Constants.ErrorMessage.CL_BranchFromLargerThanBranchTo);
            });

            When(_ => _.StaffStart != null && _.StaffEnd != null, () => {
                RuleFor(_ => _.StaffStart).Must((model, staff) => CommonHelper.IsBetween((0, 0), (0, long.Parse(model.StaffEnd.SyainCd)), (0, long.Parse(staff.SyainCd))))
                                          .WithMessage(Constants.ErrorMessage.CL_StaffFromLargerThanStaffTo);
            });

            // 得意先
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd > x.GyosyaTokuiSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CL_CustomerFromLargerThanCustomerTo);
                RuleFor(m => m.GyosyaTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.CL_CustomerFromLargerThanCustomerTo);
            });
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd > x.TokiskTokuiSakiTo.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.TokiskTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CL_CustomerFromLargerThanCustomerTo);
                    RuleFor(m => m.TokiskTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.CL_CustomerFromLargerThanCustomerTo);
                });
            When(x => ((x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && x.TokiStTokuiSakiFrom != null && x.TokiStTokuiSakiTo != null)
                    && ((x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd > x.TokiStTokuiSakiTo.SitenCd)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq > x.GyosyaTokuiSakiTo.GyosyaCdSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq > x.TokiskTokuiSakiTo.TokuiSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq == x.TokiskTokuiSakiTo.TokuiSeq && x.TokiStTokuiSakiFrom.SitenCdSeq > x.TokiStTokuiSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStTokuiSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CL_CustomerFromLargerThanCustomerTo);
                        RuleFor(m => m.TokiStTokuiSakiTo).Empty().WithMessage(Constants.ErrorMessage.CL_CustomerFromLargerThanCustomerTo);
                    });

            // 仕入先
            When(x => (x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && (x.GyosyaShiireSakiFrom.GyosyaCd > x.GyosyaShiireSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CL_SupplierFromLargerThanSupplierTo);
                RuleFor(m => m.GyosyaShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.CL_SupplierFromLargerThanSupplierTo);
            });
            When(x => (x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && x.TokiskShiireSakiFrom != null && x.TokiskShiireSakiTo != null && (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd > x.TokiskShiireSakiTo.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.TokiskShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CL_SupplierFromLargerThanSupplierTo);
                    RuleFor(m => m.TokiskShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.CL_SupplierFromLargerThanSupplierTo);
                });
            When(x => ((x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && x.TokiskShiireSakiFrom != null && x.TokiskShiireSakiTo != null && x.TokiStShiireSakiFrom != null && x.TokiStShiireSakiTo != null)
                    && ((x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd > x.TokiStShiireSakiTo.SitenCd)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq > x.GyosyaShiireSakiTo.GyosyaCdSeq)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq == x.GyosyaShiireSakiTo.GyosyaCdSeq && x.TokiskShiireSakiFrom.TokuiSeq > x.TokiskShiireSakiTo.TokuiSeq)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq == x.GyosyaShiireSakiTo.GyosyaCdSeq && x.TokiskShiireSakiFrom.TokuiSeq == x.TokiskShiireSakiTo.TokuiSeq && x.TokiStShiireSakiFrom.SitenCdSeq > x.TokiStShiireSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStShiireSakiFrom).Empty().WithMessage(Constants.ErrorMessage.CL_SupplierFromLargerThanSupplierTo);
                        RuleFor(m => m.TokiStShiireSakiTo).Empty().WithMessage(Constants.ErrorMessage.CL_SupplierFromLargerThanSupplierTo);
                    });
            When(_ => _.CancelStaffStart != null && _.CancelStaffEnd != null, () => {
                RuleFor(_ => _.CancelStaffStart).Must((model, staff) => CommonHelper.IsBetween((0, 0), (0, long.Parse(model.CancelStaffEnd.SyainCd)), (0, long.Parse(staff.SyainCd))))
                                                .WithMessage(Constants.ErrorMessage.CL_StaffFromLargerThanStaffTo);
            });
            When(_ => _.CsvConfigOption.Delimiter.Option == CSV_Delimiter.Other, () => {
                RuleFor(_ => _.CsvConfigOption.DelimiterSymbol).NotEmpty()
                .When(_ => string.IsNullOrEmpty(_.CsvConfigOption.DelimiterSymbol))
                .WithMessage(Constants.ErrorMessage.CL_CsvSeparatorIsEmpty)
                .MaximumLength(1);
            });
        }
    }
}
