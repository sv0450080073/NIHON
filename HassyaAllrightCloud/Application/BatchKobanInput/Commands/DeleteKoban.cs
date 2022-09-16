using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BatchKobanInput.Commands
{
    public class DeleteKoban : IRequest<bool>
    {
        public string TargetYmd { get; set; }
        public int SyainCdSeq { get; set; }
        public class Handler : IRequestHandler<DeleteKoban, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<bool> Handle(DeleteKoban request, CancellationToken cancellationToken = default)
            {
                bool result = true;

                var kobanData = _dbcontext.TkdKoban.Where(x => x.SiyoKbn == 1 && x.UkeNo.Trim() == "0" && x.UnkYmd == request.TargetYmd && x.SyainCdSeq == request.SyainCdSeq).ToList();

                try
                {
                    foreach(var item in kobanData)
                    {
                        _dbcontext.TkdKoban.Remove(item);
                    }
                    _dbcontext.SaveChanges();
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }
    }
}
