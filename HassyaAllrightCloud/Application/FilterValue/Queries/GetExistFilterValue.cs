using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FilterValue.Queries
{
    public class GetExistFilterValue : IRequest<TkdInpCon>
    {
        public string formName { get; set; }
        public int employeeId { get; set; }
        public string fieldName { get; set; }
        public int filterId { get; set; }
        public class Handler : IRequestHandler<GetExistFilterValue, TkdInpCon>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<TkdInpCon> Handle(GetExistFilterValue request, CancellationToken cancellationToken)
            {
                var exsitValue = _dbcontext.TkdInpCon.Where(x => x.SyainCdSeq == request.employeeId && x.FormNm == request.formName && x.ItemNm == request.fieldName && x.FilterId == request.filterId).FirstOrDefault();
                if(exsitValue == null)
                {
                    return null;
                }
                return exsitValue;
            }
        }
    }
}
