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
    public class GetSaleByCustomerQuery : IRequest<List<SalePerCustomer>>
    {
        public List<HyperGraphData> graphData { get; set; }
        public string typeDate { get; set; }
        public class Handler : IRequestHandler<GetSaleByCustomerQuery, List<SalePerCustomer>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<SalePerCustomer>> Handle(GetSaleByCustomerQuery request, CancellationToken cancellationToken)
            {
                Dictionary<int, int> CustomerDictionary = new Dictionary<int, int>();
                int CurrentIndex = 0;
                List<SalePerCustomer> result = new List<SalePerCustomer>();

                foreach (HyperGraphData data in request.graphData)
                {
                    if (!CustomerDictionary.ContainsKey(data.SitenCdSeq))
                    {
                        CustomerDictionary.Add(data.SitenCdSeq, CurrentIndex);
                        result.Add(new SalePerCustomer
                        {
                            CustomerSeq = data.TokuiSeq,
                            CustomerCd = data.TokuiCd,
                            CustomerName = data.TokuiRyakuNm,
                            BranchSeq = data.SitenCdSeq,
                            BranchCd = data.SitenCd,
                            BranchName = data.SitenRyakuNm,
                            GyosyaCdSeq = data.GyosyaCdSeq,
                            GyosyaCd = data.GyosyaCd,
                            Sale = 0,
                            Time = (DateTime)data.GetType().GetProperty(request.typeDate).GetValue(data, null)
                        });
                        CurrentIndex++;
                    }

                    int IndexToBeAdd = CustomerDictionary[data.SitenCdSeq];
                    result[IndexToBeAdd].Sale += data.SyaRyoUnc;
                }
                return await Task.FromResult(result);
            }
        }
    }
}
