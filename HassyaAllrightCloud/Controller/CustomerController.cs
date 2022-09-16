using HassyaAllrightCloud.Application.Customer.Queries;
using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ApiController
    {
        // GET: api/Customer
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<LoadCustomerList>>> GetCustomer()
        {
            return Ok(await Mediator.Send(new GetCustomerQuery()));
        }

        // GET: api/Customer/getbytenantid/{tenantId}
        [HttpGet]
        [Route("getbytenantid/{tenantId}")]
        public async Task<ActionResult<IEnumerable<LoadCustomerList>>> GetCustomersByTenantId(int tenantId)
        {
            return Ok(await Mediator.Send(new GetCustomersByTenantIdQuery(tenantId)));
        }


        // GET: api/Customer/getbytenantiddate/{tenantId}/{date}
        [HttpGet]
        [Route("getbytenantiddate/{tenantId}/{date}")]
        public async Task<ActionResult<IEnumerable<LoadCustomerList>>> GetCustomersByTenantId(int tenantId,string date)
        {
            return Ok(await Mediator.Send(new GetCustomersByTenantIdandDateQuery(tenantId,date)));
        }
    }
}
