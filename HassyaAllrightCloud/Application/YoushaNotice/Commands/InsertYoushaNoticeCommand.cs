using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.YoushaNotice.Commands
{
    public class InsertYoushaNoticeCommand : IRequest<IActionResult>
    {
        public TkdYoushaNotice youshaNotice { get; set; }
        public class Handler : IRequestHandler<InsertYoushaNoticeCommand, IActionResult>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(InsertYoushaNoticeCommand command, CancellationToken cancellationToken)
            {
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //if (_context.TkdYoushaNotice.Find(command.youshaNotice.MotoTenantCdSeq, command.youshaNotice.MotoUkeNo, command.youshaNotice.MotoUnkRen, command.youshaNotice.MotoYouTblSeq) == null)
                        //{
                            await _context.TkdYoushaNotice.AddAsync(command.youshaNotice);
                        //}
                        //else
                        //{
                        //}
                        await _context.SaveChangesAsync();
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs  
                        dbTran.Rollback();
                        return new BadRequestResult();
                    }
                }
                return new ContentResult { Content = "OK" };
            }
        }
    }
}
