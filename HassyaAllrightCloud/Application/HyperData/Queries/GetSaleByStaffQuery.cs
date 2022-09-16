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
    public class GetSaleByStaffQuery : IRequest<List<SalePerStaff>>
    {
        public List<HyperGraphData> graphData { get; set; }
        public string typeDate { get; set; }
        public class Handler : IRequestHandler<GetSaleByStaffQuery, List<SalePerStaff>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<SalePerStaff>> Handle(GetSaleByStaffQuery request, CancellationToken cancellationToken)
            {
                Dictionary<int, int> StaffDictionary = new Dictionary<int, int>();
                int CurrentIndex = 0;
                List<SalePerStaff> result = new List<SalePerStaff>();

                foreach (HyperGraphData data in request.graphData)
                {
                    if (!StaffDictionary.ContainsKey(data.EigTanCdSeq))
                    {
                        StaffDictionary.Add(data.EigTanCdSeq, CurrentIndex);
                        result.Add(new SalePerStaff
                        {
                            StaffSeq = data.EigTanCdSeq,
                            StaffCd = data.EigTanSyainCd,
                            StaffName = data.EigTanSyainNm,
                            Sale = 0,
                            Time = (DateTime)data.GetType().GetProperty(request.typeDate).GetValue(data, null)
                    });
                        CurrentIndex++;
                    }

                    int IndexToBeAdd = StaffDictionary[data.EigTanCdSeq];
                    result[IndexToBeAdd].Sale += data.SyaRyoUnc;
                }
                return await Task.FromResult(result);
            }
        }
    }
}
