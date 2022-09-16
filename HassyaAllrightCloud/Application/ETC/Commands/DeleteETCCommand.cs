using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETC.Commands
{
    public class DeleteETCCommand : IRequest<bool>
    {
        public ETCData Model { get; set; }
        public class Handler : IRequestHandler<DeleteETCCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteETCCommand request, CancellationToken cancellationToken)
            {
                var model = await _context.TkdEtcImport.FirstOrDefaultAsync(_ => _.FileName == request.Model.FileName
                                                                              && _.CardNo == request.Model.CardNo
                                                                              && _.EtcRen == request.Model.EtcRen
                                                                              && _.TenantCdSeq == new ClaimModel().TenantID);

                if (model != null)
                {
                    _context.Remove(model);
                    _context.SaveChanges();
                }

                return true;
            }
        }
    }
}
