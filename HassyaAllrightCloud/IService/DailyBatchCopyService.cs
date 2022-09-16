using HassyaAllrightCloud.Application.DailyBatchCopy.Commands;
using HassyaAllrightCloud.Application.DailyBatchCopy.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IDailyBatchCopyService
    {
        Task<List<DailyBatchCopyData>> GetListDailyBatchCopy(int TenantCdSeq, string UkeNo);
        Task<bool> ExecuteCopy(List<DailyBatchCopyData> listData, List<string> listDate, DailyBatchCopySearchModel searchModel);
    }

    public class DailyBatchCopyService : IDailyBatchCopyService
    {
        private readonly IMediator _mediator;
        public DailyBatchCopyService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<DailyBatchCopyData>> GetListDailyBatchCopy(int TenantCdSeq, string UkeNo)
        {
            return await _mediator.Send(new GetListDailyBatchCopyQuery() { TenantCdSeq = TenantCdSeq, UkeNo = UkeNo });
        }

        public async Task<bool> ExecuteCopy(List<DailyBatchCopyData> listData, List<string> listDate, DailyBatchCopySearchModel searchModel)
        {
            return await _mediator.Send(new ExecuteCopyCommand() { listData = listData, listDate = listDate, searchModel = searchModel });
        }
    }
}
