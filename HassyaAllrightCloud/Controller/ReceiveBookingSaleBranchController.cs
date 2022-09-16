using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using HassyaAllrightCloud.Application.SaleBranch.Queries;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiveBookingSaleBranchController : ApiController
    {
        // GET: api/ReceiveBookingSaleBranch
        //[HttpGet]
        //[Route("{id}")]
        //public async Task<ActionResult<IEnumerable<LoadSaleBranchList>>> GetReceiveBookingSaleBranch(int id)
        //{
        //    return Ok(await Mediator.Send(new GetSaleBranchQuery { ID = id }));
        //}

        [HttpGet("{tenantId}")]
        public async Task<ActionResult<IEnumerable<LoadSaleBranch>>> GetReceiveBookingSaleBranchByTenant(int tenantId)
        {
            try
            {
                return Ok(await Mediator.Send(new GetSaleBranchByTenantQuery(tenantId)));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}