using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using HassyaAllrightCloud.Application.YoyKbn.Queries;
using HassyaAllrightCloud.Application.YoyKbn.Commands;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VpmYoyKbnsController : ApiController
    {
        // GET: api/VpmYoyKbns
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<VpmYoyKbn>>> GetVpmYoyKbn()
        {
            return Ok(await Mediator.Send(new GetVpmYoyKbnQuery()));
        }

        // GET: api/VpmYoyKbns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VpmYoyKbn>> GetVpmYoyKbn(int id)
        {
            var result = await Mediator.Send(new GetVpmYoyKbnByIdQuery() { Id = id });

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: api/VpmYoyKbns/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVpmYoyKbn(int id, VpmYoyKbn VpmYoyKbn)
        {
            if (id != VpmYoyKbn.YoyaKbnSeq)
            {
                return BadRequest();
            }

            if(await Mediator.Send(new PutVpmYoyKbnCommand() { Id = id, VpmYoyKbn = VpmYoyKbn}))
            {
                return NoContent();
            } else
            {
                return NotFound();
            }
        }

        // POST: api/VpmYoyKbns
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<VpmYoyKbn>> PostVpmYoyKbn(VpmYoyKbn VpmYoyKbn)
        {
            await Mediator.Send(new PostVpmYoyKbnCommand { VpmYoyKbn = VpmYoyKbn });
            return CreatedAtAction("GetVpmYoyKbn", new { id = VpmYoyKbn.YoyaKbnSeq }, VpmYoyKbn);
        }

        // DELETE: api/VpmYoyKbns/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VpmYoyKbn>> DeleteVpmYoyKbn(int id)
        {
            var result = await Mediator.Send(new DeleteVpmYoyKbnCommand() { Id = id });
            if(result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}
