using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.KasSet.Commands
{
    public class DeleteTkmKasSet : IRequest<bool>
    {
        public int CompanyCdSeq { get; set; }
        public class Hanlder : IRequestHandler<DeleteTkmKasSet, bool>
        {
            private readonly KobodbContext _context;
            public Hanlder(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteTkmKasSet request, CancellationToken cancellationToken)
            {
                try
                {
                    var kasset = _context.TkmKasSet.FirstOrDefault(_ => _.CompanyCdSeq == request.CompanyCdSeq);
                    if(kasset != null)
                    {
                        _context.TkmKasSet.Remove(kasset);
                        _context.SaveChanges();
                        return true;
                    }

                    return false;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
