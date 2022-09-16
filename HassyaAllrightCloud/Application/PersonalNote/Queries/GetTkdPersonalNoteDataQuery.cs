using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PersonalNote.Queries
{
    public class GetTkdPersonalNoteDataQuery : IRequest<TKD_PersonalNoteData>
    {
        public class Handler : IRequestHandler<GetTkdPersonalNoteDataQuery, TKD_PersonalNoteData>
        {
            private readonly KobodbContext _kobodbContext;

            public Handler(KobodbContext context)
            {
                _kobodbContext = context;
            }

            public async Task<TKD_PersonalNoteData> Handle(GetTkdPersonalNoteDataQuery request, CancellationToken cancellationToken)
            {
                
                var query = (from record in _kobodbContext.TkdPersonalNote.Where(e => e.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq)
                             select new TKD_PersonalNoteData()
                             {
                                 PersonalNote_SyainCdSeq = record.SyainCdSeq,
                                 PersonalNote_Note = record.Note
                             });
                return query.FirstOrDefault();
            }
        }
    }
}
