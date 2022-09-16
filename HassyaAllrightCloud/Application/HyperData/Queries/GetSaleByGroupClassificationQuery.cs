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
    public class GetSaleByGroupClassificationQuery : IRequest<List<SalePerGroupClassification>>
    {
        public List<HyperGraphData> graphData { get; set; }
        public string typeDate { get; set; }
        public class Handler : IRequestHandler<GetSaleByGroupClassificationQuery, List<SalePerGroupClassification>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<SalePerGroupClassification>> Handle(GetSaleByGroupClassificationQuery request, CancellationToken cancellationToken)
            {
                Dictionary<string, int> GroupClassificationDictionary = new Dictionary<string, int>();
                int CurrentIndex = 0;
                List<SalePerGroupClassification> result = new List<SalePerGroupClassification>();

                foreach (HyperGraphData data in request.graphData)
                {
                    if (!GroupClassificationDictionary.ContainsKey(data.DantaiKbn))
                    {
                        GroupClassificationDictionary.Add(data.DantaiKbn, CurrentIndex);
                        result.Add(new SalePerGroupClassification
                        {
                            GroupClassificationCd = data.DantaiKbn,
                            GroupClassificationName = string.IsNullOrEmpty(data.DantaiKbn) ? "その他" : data.DantaiKbnRyakuNm,
                            Sale = 0,
                            Time = (DateTime)data.GetType().GetProperty(request.typeDate).GetValue(data, null)
                        });
                        CurrentIndex++;
                    }

                    int IndexToBeAdd = GroupClassificationDictionary[data.DantaiKbn];
                    result[IndexToBeAdd].Sale += data.SyaRyoUnc;
                }
                if (GroupClassificationDictionary.ContainsKey(""))
                {
                    int IndexNoClassification = GroupClassificationDictionary[""];
                    SalePerGroupClassification NoClassificationItem = result[IndexNoClassification];
                    result.RemoveAt(IndexNoClassification);
                    result.Add(NoClassificationItem);
                }
                return await Task.FromResult(result);
            }
        }
    }
}
