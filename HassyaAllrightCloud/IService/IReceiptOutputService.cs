using HassyaAllrightCloud.Application.ReceiptOutput.Queries;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IReceiptOutputService
    {
        Task<ReceiptOuputCommonItems> GetCommonListItems(ReceiptOutputFormSeachModel searchModel);
        Task<string> GetInvoiceYearMonth();
        Task<List<ReceiptOutputReport>> GetReceiptOutputReport(ReceiptOutputFormSeachModel searchModel);
        Task<List<Invoice>> GetInvoiceListData(ReceiptOutputFormSeachModel searchModel);
    }
    public class ReceiptOutputService : IReceiptOutputService
    {
        private readonly IMediator _mediator;
        private readonly KobodbContext _context;

        public ReceiptOutputService(IMediator mediator, KobodbContext context)
        {
            _mediator = mediator;
            _context = context;
        }
        public async Task<List<ReceiptOutputReport>> GetReceiptOutputReport(ReceiptOutputFormSeachModel searchModel)
        {
            return await _mediator.Send(new GetReceiptOutputReport { SearchModel = searchModel });
        }
        public async Task<ReceiptOuputCommonItems> GetCommonListItems(ReceiptOutputFormSeachModel searchModel)
        {
            return await _mediator.Send(new GetCommonListItems { SearchModel = searchModel });
        }
        public async Task<string> GetInvoiceYearMonth()
        {
            return await _mediator.Send(new GetInvoiceYearMonth());
        }
        public async Task<List<Invoice>> GetInvoiceListData(ReceiptOutputFormSeachModel searchModel)
        {
            return await _mediator.Send(new GetInvoiceListData { SearchModel = searchModel });
        }
    }
}
