using HassyaAllrightCloud.Domain.Dto.RegulationSetting;
using HassyaAllrightCloud.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using HassyaAllrightCloud.Application.RegulationSetting.Queries;
using HassyaAllrightCloud.Application.RegulationSetting.Commands;

namespace HassyaAllrightCloud.IService.RegulationSetting
{
    public interface IRegulationSettingService
    {
        Task<List<SPModel>> GetRegulationSettingsAsync(RegulationSettingModel model, CancellationToken token);
        Task<List<WorkHolidayType>> GetWorkHolidayTypes(int companyCdSeq);
        Task<List<WorkHolidayType>> GetSpecificWorkHolidayTypes(int companyCdSeq);
        Task<bool> SaveJiskin(byte jiskinCd,List<WorkHolidayType> workHolidayTypes, int companyCdSeq);
        Task<List<EiygoItem>> GetFormCharacters();
        Task<bool> SaveKasSet(RegulationSettingFormModel model);
        Task<bool> GetExitKasSet(int companyCdSeq);
    }

    public class RegulationSettingService : IRegulationSettingService
    {
        private readonly IMediator _mediator;
        public RegulationSettingService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> GetExitKasSet(int companyCdSeq)
        {
            return await _mediator.Send(new GetExistKasSet() { CompanyCdSeq = companyCdSeq });
        }

        public async Task<List<EiygoItem>> GetFormCharacters()
        {
            return await _mediator.Send(new GetFormCharacter());
        }

        public async Task<List<SPModel>> GetRegulationSettingsAsync(RegulationSettingModel model, CancellationToken token)
        {
            return await _mediator.Send(new GetRegulationSettings() { model = model }, token);
        }

        public async Task<List<WorkHolidayType>> GetSpecificWorkHolidayTypes(int companyCdSeq)
        {
            return await _mediator.Send(new GetSpecificWorkHolidayType() { CompanyCdSeq = companyCdSeq });
        }

        public async Task<List<WorkHolidayType>> GetWorkHolidayTypes(int companyCdSeq)
        {
            return await _mediator.Send(new GetWorkHolidayType() { CompanyCdSeq = companyCdSeq });
        }

        public async Task<bool> SaveJiskin(byte jiskinCd, List<WorkHolidayType> workHolidayTypes, int companyCdSeq)
        {
            return await _mediator.Send(new SaveJiskin() { JiskinCd = jiskinCd, WorkHolidayTypes = workHolidayTypes, CompanyCdSeq = companyCdSeq });
        }

        public async Task<bool> SaveKasSet(RegulationSettingFormModel model)
        {
            return await _mediator.Send(new SaveSetting() { Model = model });
        }
    }
}
