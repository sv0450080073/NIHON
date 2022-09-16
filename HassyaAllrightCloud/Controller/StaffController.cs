using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HassyaAllrightCloud.Application.Staff.Queries;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ApiController
    {
        // GET: api/Staff
        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<LoadStaffList>>> GetStaff(int id)
        //{
        //    return Ok(await Mediator.Send(new GetStaffQuery { Id = id }));
        //}

        [HttpGet("{tenantId}")]
        public async Task<ActionResult<IEnumerable<LoadStaff>>> GetStaffByTenant(int tenantId)
        {
            try
            {
                return Ok(await Mediator.Send(new GetStaffByTenantQuery(tenantId)));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}