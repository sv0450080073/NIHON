using HassyaAllrightCloud.Commons.Constants;
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
    public class SaveCustomFilter : IRequest<bool>
    {
        public int FilterID { get; set; }
        public int EmployeeId { get; set; }
        public string FormName { get; set; }
        public string FilterName { get; set; }

        public class Handler : IRequestHandler<SaveCustomFilter, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<bool> Handle(SaveCustomFilter request, CancellationToken cancellationToken)
            {
                var existFilter = _dbcontext.TkdFilter.Where(x => x.SyainCdSeq == request.EmployeeId && x.FormNm == request.FormName && x.FilterId == request.FilterID && x.SiyoKbn == 1).FirstOrDefault();
                if(existFilter == null)
                {
                    TkdFilter tkdFilter = new TkdFilter()
                    {
                        FilterId = request.FilterID,
                        FilterName = request.FilterName,
                        FormNm = request.FormName,
                        SiyoKbn = 1,
                        SyainCdSeq = request.EmployeeId,
                        UpdPrgId = "",
                        UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                        UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                        UpdTime = DateTime.Now.ToString("HHmm"),
                    };
                    _dbcontext.TkdFilter.Add(tkdFilter);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    existFilter.FilterName = request.FilterName;
                    _dbcontext.TkdFilter.Update(existFilter);
                    _dbcontext.SaveChanges();
                }
                return true;
            }
        }
    }
}
