using HassyaAllrightCloud.Application.TaxType.Queries;
using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxTypeController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxTypeList>>> GetTaxTypeList()
        {
            return Ok(await Mediator.Send(new GetTaxTypeListQuery()));
        }
    }
}
