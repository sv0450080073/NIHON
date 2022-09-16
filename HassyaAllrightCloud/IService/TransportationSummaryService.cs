using HassyaAllrightCloud.Application.TransportationSummary.Queries;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ITransportationSummaryService
    {
        Task<IEnumerable<CompanyListItem>> GetCompanyListItems(int companyCdSeq);
        Task<IEnumerable<EigyoListItem>> GetEigyoListItems(int companyCdSeq, int tenantCdSeq);
        Task<IEnumerable<TransportationSummaryItem>> GetTableRowModels(TransportationSummarySearchModel searchModel, bool getOnly);
        Task<bool> IsValidCompanyCode(int codeSeq);
    }
    public class TransportationSummaryService : ITransportationSummaryService
    {
        private readonly IMediator _mediator;
        public TransportationSummaryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<CompanyListItem>> GetCompanyListItems(int companyCdSeq)
        {
            return await _mediator.Send(new GetCompanyByCompanyCdSeq() { CompanyCdSeq = companyCdSeq });
        }

        public async Task<IEnumerable<EigyoListItem>> GetEigyoListItems(int companyCdSeq, int tenantCdSeq)
        {
            return await _mediator.Send(new GetEigyosByCompanyCdSeq() { CompanyCdSeq = companyCdSeq, TenantCdSeq = tenantCdSeq });
        }

        public async Task<IEnumerable<TransportationSummaryItem>> GetTableRowModels(TransportationSummarySearchModel searchModel, bool getOnly)
        {
            return await _mediator.Send(new GetTransportationSummary() { Model = searchModel, GetOnly = getOnly });
        }

        public async Task<bool> IsValidCompanyCode(int codeSeq)
        {
            return await _mediator.Send(new IsCompanyCodeSeqValid() { CompanyCdSeq = codeSeq });
        }
    }
}
