using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HyperData.Queries
{
    public class GetSaleByMonthQuery : IRequest<List<SalePerTime>>
    {
        public List<HyperGraphData> graphData { get; set; }
        public string typeDate { get; set; }
        public DateTime startDate { set; get; }
        public DateTime endDate { get; set; }
        public class Handler : IRequestHandler<GetSaleByMonthQuery, List<SalePerTime>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<SalePerTime>> Handle(GetSaleByMonthQuery request, CancellationToken cancellationToken)
            {
                int NumberOfMonth = (request.endDate.Year - request.startDate.Year) * 12 + request.endDate.Month - request.startDate.Month + 1;
                DateTime FirstDate = new DateTime(request.startDate.Year, request.startDate.Month, 1);
                List<SalePerTime> result = new List<SalePerTime>(new SalePerTime[NumberOfMonth]);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i] = new SalePerTime
                    {
                        Time = FirstDate.AddMonths(i),
                        Sale = 0,
                        Count = 0
                    };
                }

                foreach (HyperGraphData data in request.graphData)
                {
                    DateTime CurrentDate = (DateTime)data.GetType().GetProperty(request.typeDate).GetValue(data, null);
                    int index = (CurrentDate.Year - FirstDate.Year) * 12 + CurrentDate.Month - FirstDate.Month;
                    result[index].Sale += data.SyaRyoUnc;
                    result[index].Count += 1;
                }
                return await Task.FromResult(result);
            }
        }
    }
}
