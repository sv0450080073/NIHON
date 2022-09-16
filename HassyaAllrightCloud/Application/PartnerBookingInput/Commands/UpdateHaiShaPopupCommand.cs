using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Commands
{
    public class UpdateHaiShaPopupCommand : IRequest<IActionResult>
    {
        public string Ukeno { get; set; }
        public TkdHaisha TkdHaisha { get; set; }
        public class Handler : IRequestHandler<UpdateHaiShaPopupCommand, IActionResult>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<UpdateHaiShaPopupCommand> _logger;
            public Handler(KobodbContext context, ILogger<UpdateHaiShaPopupCommand> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<IActionResult> Handle(UpdateHaiShaPopupCommand command, CancellationToken cancellationToken)
            {
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {                     
                        _context.TkdHaisha.Update(command.TkdHaisha);
                        await _context.SaveChangesAsync();
                        dbTran.Commit();
                    }
                    catch (DbUpdateConcurrencyException Ex)
                    {
                        _logger.LogError(Ex, Ex.Message);
                        dbTran.Rollback();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogTrace(ex.ToString());
                        dbTran.Rollback();
                        return new BadRequestResult();
                    }
                }
                return new ContentResult { Content = command.TkdHaisha.UkeNo };
            }
        }
    }
}
