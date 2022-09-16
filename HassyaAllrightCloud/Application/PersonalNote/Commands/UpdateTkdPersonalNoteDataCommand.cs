using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PersonalNote.Commands
{
    public class UpdateTkdPersonalNoteDataCommand : IRequest<string>
    {
        public TKD_PersonalNoteData PersonalNoteData { get; set; }
        public class Handler : IRequestHandler<UpdateTkdPersonalNoteDataCommand, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<string> Handle(UpdateTkdPersonalNoteDataCommand request, CancellationToken cancellationToken)
            {
                string result = null;
                if (request.PersonalNoteData == null)
                {
                    result = "Data can't be null";
                    return result;
                }
                
                TkdPersonalNote tkdPersonalNote = _context.TkdPersonalNote.Find(request.PersonalNoteData.PersonalNote_SyainCdSeq);

                try
                {
                    if (tkdPersonalNote == null)
                    {
                        tkdPersonalNote = new TkdPersonalNote();
                        tkdPersonalNote.SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdPersonalNote.Note = request.PersonalNoteData.PersonalNote_Note;
                        tkdPersonalNote.UpdTime = DateTime.Now.ToString("HHmmss");
                        tkdPersonalNote.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        tkdPersonalNote.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdPersonalNote.UpdPrgId = Common.UpdPrgId;
                        _context.TkdPersonalNote.Add(tkdPersonalNote);
                    } else
                    {
                        tkdPersonalNote.Note = request.PersonalNoteData.PersonalNote_Note;
                        tkdPersonalNote.UpdTime = DateTime.Now.ToString("HHmmss");
                        tkdPersonalNote.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        tkdPersonalNote.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdPersonalNote.UpdPrgId = Common.UpdPrgId;
                        _context.TkdPersonalNote.Update(tkdPersonalNote);
                    }
                    await _context.SaveChangesAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    return result;
                }
            }
        }
    }
}
