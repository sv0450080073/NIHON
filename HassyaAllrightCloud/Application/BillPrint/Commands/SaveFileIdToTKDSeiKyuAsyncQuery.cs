using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class SaveFileIdToTKDSeiKyuAsyncQuery : IRequest<string>
    {
        public List<PaymentRequestGroup> paymentRequestGroups {get; set;}
        public class Handler : IRequestHandler<SaveFileIdToTKDSeiKyuAsyncQuery, string>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(SaveFileIdToTKDSeiKyuAsyncQuery request, CancellationToken cancellationToken = default)
            {
                try
                {
                    if (!request.paymentRequestGroups.Any())
                        return null;
                    var tkdSeiKyuList = new List<TkdSeikyu>();
                    foreach (var item in request.paymentRequestGroups)
                    {
                        var tkdSeikyu = _context.TkdSeikyu.Where(x => x.SeiOutSeq == item.SeiOutSeq && x.SeiRen == item.SeiRen && x.TenantCdSeq == new ClaimModel().TenantID).FirstOrDefault();
                        if (tkdSeikyu != null)
                        {
                            tkdSeikyu.SeiFileId = item.SeiFileId;
                            tkdSeiKyuList.Add(tkdSeikyu);
                        }
                    }
                    _context.TkdSeikyu.UpdateRange(tkdSeiKyuList);
                    _context.SaveChanges();
                    return null;
                } catch(Exception e)
                {
                    return null;
                }
            }
        }
    }
}
