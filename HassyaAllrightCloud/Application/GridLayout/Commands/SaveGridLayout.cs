using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.GridLayout.Commands
{
    public class SaveGridLayout : IRequest<bool>
    {
        public List<TkdGridLy> datas { get; set; }

        public class Handler : IRequestHandler<SaveGridLayout, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                this._dbcontext = context;
            }

            public async Task<bool> Handle(SaveGridLayout request, CancellationToken cancellationToken)
            {
                using(var trans = _dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in request.datas)
                        {
                            var existGridLayout = _dbcontext.TkdGridLy.FirstOrDefault(x => x.SyainCdSeq == item.SyainCdSeq 
                                                                                            && x.GridNm == item.GridNm 
                                                                                            && x.FormNm == item.FormNm 
                                                                                            && x.DspNo == item.DspNo);

                            if (existGridLayout == null)
                            {
                                _dbcontext.TkdGridLy.Add(item);
                                _dbcontext.SaveChanges();
                            }
                            else
                            {
                                existGridLayout.ItemNm = item.ItemNm;
                                existGridLayout.Width = item.Width;
                                _dbcontext.TkdGridLy.Update(existGridLayout);
                                _dbcontext.SaveChanges();
                            }
                        }
                        
                        await trans.CommitAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        await trans.RollbackAsync();
                        return false;
                    }
                }
            }
        }
    }
}
