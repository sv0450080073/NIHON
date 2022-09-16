using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HyperData.Queries
{
    public class GetSaleByDayQuery : IRequest<List<SalePerTime>>
    {
        public List<HyperGraphData> graphData { get; set; }
        public string typeDate { get; set; }
        public DateTime startDate { set; get; }
        public DateTime endDate { get; set; }
        public class Handler : IRequestHandler<GetSaleByDayQuery, List<SalePerTime>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<SalePerTime>> Handle(GetSaleByDayQuery request, CancellationToken cancellationToken)
            {
                int NumberOfDay = (request.endDate - request.startDate).Days + 1;
                List<SalePerTime> result = new List<SalePerTime>(new SalePerTime[NumberOfDay]);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i] = new SalePerTime
                    {
                        Time = request.startDate.AddDays(i),
                        Sale = 0,
                        Count = 0
                    };
                }

                foreach (HyperGraphData data in request.graphData)
                {
                    DateTime CurrentDate = (DateTime)data.GetType().GetProperty(request.typeDate).GetValue(data, null);
                    int index = (CurrentDate - request.startDate).Days;
                    result[index].Sale += data.SyaRyoUnc;
                    result[index].Count += 1;
                }
                return await Task.FromResult(result);
            }
        }
    }
}
