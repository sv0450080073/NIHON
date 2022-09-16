using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Application.TkdYykSyus.Queries;
using HassyaAllrightCloud.Application.TkdYykSyus.Commands;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TkdYykSyusController : ApiController
    {
        // GET: api/TkdYykSyus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TkdYykSyu>>> GetTkdYykSyu()
        {
            return Ok(await Mediator.Send(new GetTkdYykSyusQuery()));
        }

        // GET: api/TkdYykSyus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TkdYykSyu>> GetTkdYykSyu(int id)
        {
            var tkdYykSyu = await Mediator.Send(new GetTkdYykSyuQuery() { Id = id });
            if (tkdYykSyu == null)
            {
                return NotFound();
            }

            return tkdYykSyu;
        }

        // PUT: api/TkdYykSyus/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTkdYykSyu(string id, TkdYykSyu tkdYykSyu)
        {
            if (id != tkdYykSyu.UkeNo)
            {
                return BadRequest();
            }
            if (await Mediator.Send(new PutTkdYykSyuCommand() { Id = id, TkdYykSyu = tkdYykSyu }))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/TkdYykSyus
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TkdYykSyu>> PostTkdYykSyu(TkdYykSyu tkdYykSyu)
        {
            if (await Mediator.Send(new PostTkdYykSyuCommand() { TkdYykSyu = tkdYykSyu }))
            {
                return CreatedAtAction("GetTkdYykSyu", new { id = tkdYykSyu.UkeNo }, tkdYykSyu);
            }
            else
            {
                return Conflict();
            }
            
        }

        // DELETE: api/TkdYykSyus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TkdYykSyu>> DeleteTkdYykSyu(int id)
        {
            var tkdYykSyu = await Mediator.Send(new DeleteTkdYykSyuCommand() { Id = id });
            if (tkdYykSyu == null)
            {
                return NotFound();
            }

            return tkdYykSyu;
        }
    }
}
