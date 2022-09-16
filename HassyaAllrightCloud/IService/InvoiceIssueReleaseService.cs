using System.Collections.Generic;
using System.Threading.Tasks;
using HassyaAllrightCloud.Application.BillPrint.Queries;
using HassyaAllrightCloud.Domain.Dto.InvoiceIssueRelease;
using MediatR;

namespace HassyaAllrightCloud.IService
{
    public interface IInvoiceIssueReleaseService
    {
        Task <List<InvoiceIssueGrid>> GetInvoiceIssueReleasesAsync(InvoiceIssueFilter invoiceIssueFilter);
        Task<string> ReleaseInvoicesAsync(List<InvoiceIssueGrid> invoiceIssueGrids);
    }

    public class InvoiceIssueReleaseService : IInvoiceIssueReleaseService
    {
        private IMediator mediatR;
        public InvoiceIssueReleaseService(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        public async Task<List<InvoiceIssueGrid>> GetInvoiceIssueReleasesAsync(InvoiceIssueFilter invoiceIssueFilter)
        {
            return await mediatR.Send(new GetInvoiceIssueReleasesAsyncQuery { invoiceIssueFilter = invoiceIssueFilter});
        }

        public async Task<string> ReleaseInvoicesAsync(List<InvoiceIssueGrid> invoiceIssueGrids)
        {
            return await mediatR.Send(new ReleaseInvoiceAsyncCommand { invoiceIssueGrids = invoiceIssueGrids });
        }
    }
}
