using FluentValidation;
using HassyaAllrightCloud.Domain.Dto.RegulationSetting;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;

namespace HassyaAllrightCloud.Application.Validation
{
    public class RegulationSettingFormValidator : AbstractValidator<RegulationSettingFormModel>
    {
        public RegulationSettingFormValidator()
        {
            When(x => x.Company == null, () =>
            {
                RuleFor(x => x.Company).NotEmpty().WithMessage("BI_T002");
            });
            When(x => x.Company != null && x.IsDuplicateCompany, () =>
            {
                RuleFor(x => x.Company).Empty().WithMessage("BI_T003");
            });
            RuleFor(x => x.EditFormCancelRate1StartTime).NotEmpty().WithMessage("BI_T004");
            RuleFor(x => x.EditFormCancelRate1EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate1StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate1EndTime) && decimal.Parse(x.EditFormCancelRate1StartTime) > decimal.Parse(x.EditFormCancelRate1EndTime)).WithMessage("BI_T005");
            RuleFor(x => x.EditFormCancelRate1StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate1StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate1EndTime) && decimal.Parse(x.EditFormCancelRate1StartTime) > decimal.Parse(x.EditFormCancelRate1EndTime)).WithMessage("BI_T005");
            RuleFor(x => x.EditFormCancelRate1EndTime).NotEmpty().WithMessage("BI_T006");
            RuleFor(x => x.EditFormCancelRate1EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate1EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate2StartTime) && decimal.Parse(x.EditFormCancelRate1EndTime) > decimal.Parse(x.EditFormCancelRate2StartTime)).WithMessage("BI_T007");
            RuleFor(x => x.EditFormCancelRate2StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate1EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate2StartTime) && decimal.Parse(x.EditFormCancelRate1EndTime) > decimal.Parse(x.EditFormCancelRate2StartTime)).WithMessage("BI_T007");

            RuleFor(x => x.EditFormCancelRate1).NotEmpty().WithMessage("BI_T008");
            RuleFor(x => x.EditFormCancelRate1).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate1) && decimal.Parse(x.EditFormCancelRate1) > decimal.Parse("100.0")).WithMessage("BI_T009");
            RuleFor(x => x.EditFormCancelRate2StartTime).NotEmpty().WithMessage("BI_T010");
            RuleFor(x => x.EditFormCancelRate2StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate2StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate2EndTime) && decimal.Parse(x.EditFormCancelRate2StartTime) > decimal.Parse(x.EditFormCancelRate2EndTime)).WithMessage("BI_T011");
            RuleFor(x => x.EditFormCancelRate2EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate2StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate2EndTime) && decimal.Parse(x.EditFormCancelRate2StartTime) > decimal.Parse(x.EditFormCancelRate2EndTime)).WithMessage("BI_T011");

            RuleFor(x => x.EditFormCancelRate2EndTime).NotEmpty().WithMessage("BI_T012");
            RuleFor(x => x.EditFormCancelRate2EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate2EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate3StartTime) && decimal.Parse(x.EditFormCancelRate2EndTime) > decimal.Parse(x.EditFormCancelRate3StartTime)).WithMessage("BI_T013");
            RuleFor(x => x.EditFormCancelRate3StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate2EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate3StartTime) && decimal.Parse(x.EditFormCancelRate2EndTime) > decimal.Parse(x.EditFormCancelRate3StartTime)).WithMessage("BI_T013");

            RuleFor(x => x.EditFormCancelRate2).NotEmpty().WithMessage("BI_T014");
            RuleFor(x => x.EditFormCancelRate2).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate2) && decimal.Parse(x.EditFormCancelRate2) > decimal.Parse("100.0")).WithMessage("BI_T015");
            RuleFor(x => x.EditFormCancelRate3StartTime).NotEmpty().WithMessage("BI_T016");
            RuleFor(x => x.EditFormCancelRate3StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate3StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate3EndTime) && decimal.Parse(x.EditFormCancelRate3StartTime) > decimal.Parse(x.EditFormCancelRate3EndTime)).WithMessage("BI_T017");
            RuleFor(x => x.EditFormCancelRate3EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate3StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate3EndTime) && decimal.Parse(x.EditFormCancelRate3StartTime) > decimal.Parse(x.EditFormCancelRate3EndTime)).WithMessage("BI_T017");

            RuleFor(x => x.EditFormCancelRate3EndTime).NotEmpty().WithMessage("BI_T018");
            RuleFor(x => x.EditFormCancelRate3EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate3EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate4StartTime) && decimal.Parse(x.EditFormCancelRate3EndTime) > decimal.Parse(x.EditFormCancelRate4StartTime)).WithMessage("BI_T019");
            RuleFor(x => x.EditFormCancelRate4StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate3EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate4StartTime) && decimal.Parse(x.EditFormCancelRate3EndTime) > decimal.Parse(x.EditFormCancelRate4StartTime)).WithMessage("BI_T019");

            RuleFor(x => x.EditFormCancelRate3).NotEmpty().WithMessage("BI_T020");
            RuleFor(x => x.EditFormCancelRate3).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate3) && decimal.Parse(x.EditFormCancelRate3) > decimal.Parse("100.0")).WithMessage("BI_T021");
            RuleFor(x => x.EditFormCancelRate4StartTime).NotEmpty().WithMessage("BI_T022");
            RuleFor(x => x.EditFormCancelRate4StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate4StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate4EndTime) && decimal.Parse(x.EditFormCancelRate4StartTime) > decimal.Parse(x.EditFormCancelRate4EndTime)).WithMessage("BI_T023");
            RuleFor(x => x.EditFormCancelRate4EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate4StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate4EndTime) && decimal.Parse(x.EditFormCancelRate4StartTime) > decimal.Parse(x.EditFormCancelRate4EndTime)).WithMessage("BI_T023");

            RuleFor(x => x.EditFormCancelRate4EndTime).NotEmpty().WithMessage("BI_T024");
            RuleFor(x => x.EditFormCancelRate4EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate4EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate5StartTime) && decimal.Parse(x.EditFormCancelRate4EndTime) > decimal.Parse(x.EditFormCancelRate5StartTime)).WithMessage("BI_T025");
            RuleFor(x => x.EditFormCancelRate5StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate4EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate5StartTime) && decimal.Parse(x.EditFormCancelRate4EndTime) > decimal.Parse(x.EditFormCancelRate5StartTime)).WithMessage("BI_T025");

            RuleFor(x => x.EditFormCancelRate4).NotEmpty().WithMessage("BI_T026");
            RuleFor(x => x.EditFormCancelRate4).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate4) && decimal.Parse(x.EditFormCancelRate4) > decimal.Parse("100.0")).WithMessage("BI_T027");
            RuleFor(x => x.EditFormCancelRate5StartTime).NotEmpty().WithMessage("BI_T028");
            RuleFor(x => x.EditFormCancelRate5StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate5StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate5EndTime) && decimal.Parse(x.EditFormCancelRate5StartTime) > decimal.Parse(x.EditFormCancelRate5EndTime)).WithMessage("BI_T029");
            RuleFor(x => x.EditFormCancelRate5EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate5StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate5EndTime) && decimal.Parse(x.EditFormCancelRate5StartTime) > decimal.Parse(x.EditFormCancelRate5EndTime)).WithMessage("BI_T029");

            RuleFor(x => x.EditFormCancelRate5EndTime).NotEmpty().WithMessage("BI_T030");
            RuleFor(x => x.EditFormCancelRate5EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate5EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate6StartTime) && decimal.Parse(x.EditFormCancelRate5EndTime) > decimal.Parse(x.EditFormCancelRate6StartTime)).WithMessage("BI_T031");
            RuleFor(x => x.EditFormCancelRate6StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate5EndTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate6StartTime) && decimal.Parse(x.EditFormCancelRate5EndTime) > decimal.Parse(x.EditFormCancelRate6StartTime)).WithMessage("BI_T031");

            RuleFor(x => x.EditFormCancelRate5).NotEmpty().WithMessage("BI_T032");
            RuleFor(x => x.EditFormCancelRate5).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate5) && decimal.Parse(x.EditFormCancelRate5) > decimal.Parse("100.0")).WithMessage("BI_T033");
            RuleFor(x => x.EditFormCancelRate6StartTime).NotEmpty().WithMessage("BI_T034");
            RuleFor(x => x.EditFormCancelRate6StartTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate6StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate6EndTime) && decimal.Parse(x.EditFormCancelRate6StartTime) > decimal.Parse(x.EditFormCancelRate6EndTime)).WithMessage("BI_T031");
            RuleFor(x => x.EditFormCancelRate6EndTime).Empty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate6StartTime) && !string.IsNullOrWhiteSpace(x.EditFormCancelRate6EndTime) && decimal.Parse(x.EditFormCancelRate6StartTime) > decimal.Parse(x.EditFormCancelRate6EndTime)).WithMessage("BI_T031");

            RuleFor(x => x.EditFormCancelRate6EndTime).NotEmpty().WithMessage("BI_T036");
            RuleFor(x => x.EditFormCancelRate6).NotEmpty().WithMessage("BI_T037");
            RuleFor(x => x.EditFormCancelRate6).NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.EditFormCancelRate6) && decimal.Parse(x.EditFormCancelRate6) > decimal.Parse("100.0")).WithMessage("BI_T038");
        }
    }
}
