using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;
using StoredProcedureEFCore;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetReservationCategory : IRequest<List<ReservationCategoryModel>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetReservationCategory, List<ReservationCategoryModel>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<List<ReservationCategoryModel>> Handle(GetReservationCategory request, CancellationToken cancellationToken)
            {
                var data = new List<ReservationDataModel>();
                var result = new List<ReservationCategoryModel>();
                _dbContext.LoadStoredProc("PK_dReservation_R")
                    .AddParam("@TenantCdSeq", request.TenantCdSeq)
                    .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                    .Exec(r => data = r.ToList<ReservationDataModel>());
                foreach(var item in data)
                {
                    result.Add(new ReservationCategoryModel()
                    {
                        PriorityNum = item.YoyaKbnSeq.ToString(),
                        YoyaKbnName = item.YoyaKbnNm
                    });
                }
                return result;
            }
        }
    }
}
