using HassyaAllrightCloud.Application.PersonalNote.Commands;
using HassyaAllrightCloud.Application.PersonalNote.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_PersonalNoteDataService
    {
        Task<TKD_PersonalNoteData> Get();
        Task<string> UpdatePersonalNote(TKD_PersonalNoteData personalNoteData);

    }
    public class TKD_PersonalNoteDataService : ITKD_PersonalNoteDataService
    {
        private IMediator _mediator;
        public TKD_PersonalNoteDataService(IMediator mediatR)
        {
            _mediator = mediatR;
        }

        public async Task<TKD_PersonalNoteData> Get()
        {
            return await _mediator.Send(new GetTkdPersonalNoteDataQuery());
        }

        public async Task<string> UpdatePersonalNote(TKD_PersonalNoteData personalNoteData)
        {
            return await _mediator.Send(new UpdateTkdPersonalNoteDataCommand() { PersonalNoteData = personalNoteData});
        }
    }
}
