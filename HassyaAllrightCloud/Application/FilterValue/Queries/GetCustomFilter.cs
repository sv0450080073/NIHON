using HassyaAllrightCloud.Domain.Dto;
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
    public class GetCustomFilter : IRequest<List<CustomFilerModel>>
    {
        public string FormName { get; set; }
        public int EmployeeId { get; set; }

        public class Handler : IRequestHandler<GetCustomFilter, List<CustomFilerModel>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<CustomFilerModel>> Handle(GetCustomFilter request, CancellationToken cancellationToken)
            {
                var result = new List<CustomFilerModel>();
                var filters = _context.TkdFilter.Where(x => x.FormNm == request.FormName && x.SyainCdSeq == request.EmployeeId && x.SiyoKbn == (byte)1).ToList();
                int id = 0;
                foreach (var item in filters)
                {
                    result.Add(new CustomFilerModel()
                    {
                        Id = id,
                        EmployeeId = item.SyainCdSeq,
                        FilterName = item.FilterName,
                        FormName = item.FormNm,
                        FilterId = item.FilterId
                    });
                    id++;
                }
                return result;
            }
        }
    }
}
