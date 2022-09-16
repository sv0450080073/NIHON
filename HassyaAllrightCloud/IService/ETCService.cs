using HassyaAllrightCloud.Application.ETC.Commands;
using HassyaAllrightCloud.Application.ETC.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IETCService
    {
        Task<List<ETCCompany>> GetListETCCompany(int tenantCdSeq);
        Task<List<ETCEigyo>> GetListETCEigyo(int tenantCdSeq);
        Task<List<ETCSyaRyo>> GetListETCSyaRyo(int tenantCdSeq);
        Task<List<ETCFutai>> GetListETCFutai(int tenantCdSeq);
        Task<List<ETCSeisan>> GetListETCSeisan();
        Task<List<ETCData>> GetListETC(ETCSearchParam searchParam);
        Task<List<ETCYoyakuData>> GetListYoyaku(ETCSearchParam searchParam);
        Task<bool> UpdateListETC(List<ETCData> listETC);
        Task<List<ETCKyoSet>> GetListKyoSet();
        Task<bool> DeleteETC(ETCData model);
        Task<bool> TransferETC(List<ETCData> listTransfer);
        Task<TkmKasSetModel> GetTkmKasSet(int companyId);
    }

    public class ETCService : IETCService
    {
        private readonly IMediator _mediator;
        public ETCService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<ETCCompany>> GetListETCCompany(int tenantCdSeq)
        {
            return await _mediator.Send(new GetListCompanyForSearchQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<ETCEigyo>> GetListETCEigyo(int tenantCdSeq)
        {
            return await _mediator.Send(new GetListEigyoForSearchQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<ETCSyaRyo>> GetListETCSyaRyo(int tenantCdSeq)
        {
            return await _mediator.Send(new GetListSyaRyoForSearchQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<ETCFutai>> GetListETCFutai(int tenantCdSeq)
        {
            return await _mediator.Send(new GetListFutaiForSearchQuery() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<ETCSeisan>> GetListETCSeisan()
        {
            return await _mediator.Send(new GetListSeisanForSearchQuery());
        }

        public async Task<List<ETCData>> GetListETC(ETCSearchParam searchParam)
        {
            return await _mediator.Send(new GetListETCDataQuery() { SearchParam = searchParam });
        }

        public async Task<List<ETCYoyakuData>> GetListYoyaku(ETCSearchParam searchParam)
        {
            return await _mediator.Send(new GetListYoyakuDataQuery() { SearchParam = searchParam });
        }

        public async Task<bool> UpdateListETC(List<ETCData> listETC)
        {
            return await _mediator.Send(new UpdateListETCCommand() { ListModel = listETC });
        }

        public async Task<List<ETCKyoSet>> GetListKyoSet()
        {
            return await _mediator.Send(new GetListETCKyoSetQuery());
        }

        public async Task<bool> DeleteETC(ETCData model)
        {
            return await _mediator.Send(new DeleteETCCommand() { Model = model });
        }

        public async Task<bool> TransferETC(List<ETCData> listTransfer)
        {
            return await _mediator.Send(new TransferListETCCommand() { ListModel = listTransfer, TenantCdSeq = new ClaimModel().TenantID });
        }

        public async Task<TkmKasSetModel> GetTkmKasSet(int companyId)
        {
            return await _mediator.Send(new GetTkmKasSet() { CompanyId = companyId });
        }
    }
}
