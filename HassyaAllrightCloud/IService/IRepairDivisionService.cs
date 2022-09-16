using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IRepairDivisionService
    {
        List<RepairDivision> GetRepairDivisionList(int tenantID);
    }
    public class RepairDivisionService : IRepairDivisionService
    {
        private readonly KobodbContext _dbContext;
        private readonly IMediator _mediator;
        public RepairDivisionService(KobodbContext context, IMediator mediator)
        {
            _dbContext = context;
            _mediator = mediator;
        }
        public List<RepairDivision> GetRepairDivisionList(int tenantID)
        {
            var data = new List<RepairDivision>();
            data = (from REPAIR in _dbContext.VpmRepair
                    where REPAIR.TenantCdSeq == tenantID
                    orderby REPAIR.RepairCd
                    select new RepairDivision()
                    {
                        RepairCdSeq = REPAIR.RepairCdSeq,
                        RepairCd = REPAIR.RepairCd,
                        RepairRyakuNm = REPAIR.RepairRyakuNm
                    }).ToList();
            return data;
        }
    }

}
