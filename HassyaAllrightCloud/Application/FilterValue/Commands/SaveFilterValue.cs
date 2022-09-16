using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FilterValue.Commands
{
    public class SaveFilterValue : IRequest<bool>
    {
        public List<TkdInpCon> tkdInpCon { get; set; }
        public class Handler : IRequestHandler<SaveFilterValue, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<bool> Handle(SaveFilterValue request, CancellationToken cancellationToken)
            {
                foreach(var item in request.tkdInpCon)
                {
                    var ExistedItem = _dbcontext.TkdInpCon.Where(x => x.SyainCdSeq == item.SyainCdSeq && x.FormNm == item.FormNm && x.FilterId == item.FilterId && x.ItemNm == item.ItemNm).FirstOrDefault();
                    if (ExistedItem != null)
                    {
                        ExistedItem.JoInput = item.JoInput;
                        _dbcontext.TkdInpCon.Update(ExistedItem);
                    }
                    else
                    {
                        _dbcontext.TkdInpCon.Add(item);
                    }
                }
                _dbcontext.SaveChanges();
                return true;
            }
        }
    }
}
