using HassyaAllrightCloud.Application.HikiukeshoReport;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HikiukeshoReport.Commands
{
    public class UpdateHikiukeshoExportDateCommand : IRequest<bool>
    {
        public TransportationContractFormData TransportationContract { get; set; }
        public class Handler : IRequestHandler<UpdateHikiukeshoExportDateCommand, bool>
        {
            private readonly KobodbContext context;
            private readonly HikiukeshoHelper hikiukeshoHelper;

            public Handler(KobodbContext _context, HikiukeshoHelper _hikiukeshoHelper)
            {
                this.context = _context;
                this.hikiukeshoHelper = _hikiukeshoHelper;
            }

            public async Task<bool> Handle(UpdateHikiukeshoExportDateCommand request, CancellationToken cancellationToken)
            {
                if (request.TransportationContract == null) return false;

                try
                {
                    var storedBuilder = hikiukeshoHelper.CreateStoredBuilder("dbo.PK_dHkUkYykExportDate_E", context, request.TransportationContract);
                    storedBuilder.ExecNonQuery();
                    return true;
                } catch(Exception e)
                {
                    return false;
                }
                
            }
        }
    }
}
