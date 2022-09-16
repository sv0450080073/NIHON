using MediatR;
using System.Threading.Tasks;
using System.Data;
using HassyaAllrightCloud.Application.GeneralOutput.Queries;

namespace HassyaAllrightCloud.IService
{
    public interface IGeneralOutPutService
    {
        Task<DataTable> GetDataTable(string sqlQuery);
    }
    public class GeneralOutPutService : IGeneralOutPutService
    {
        private readonly IMediator _mediator;
        public GeneralOutPutService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<DataTable> GetDataTable(string sqlQuery)
        {
            return await _mediator.Send(new GetGeneralOutputQuery(sqlQuery));
        }
    }
}
