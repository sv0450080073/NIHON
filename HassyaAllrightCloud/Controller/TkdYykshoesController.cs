using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Application.TkdYykshoes.Commands;
using HassyaAllrightCloud.Application.TkdYykshoes.Queries;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TkdYykshoesController : ApiController
    {
        // GET: api/TkdYykshoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TkdYyksho>>> GetTkdYyksho()
        {
            return Ok(await Mediator.Send(new GetTkdYykshoesQuery()));
        }

        // GET: api/TkdYykshoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TkdYyksho>> GetTkdYyksho(int id)
        {
            var tkdYyksho =  await Mediator.Send(new GetTkdYykshoQuery() { Id = id });
            if (tkdYyksho == null)
            {
                return NotFound();
            }
            return tkdYyksho;
        }

        // PUT: api/TkdYykshoes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTkdYyksho(string id, TkdYyksho tkdYyksho)
        {
            if (id != tkdYyksho.UkeNo)
            {
                return BadRequest();
            }
            if (await Mediator.Send(new PutTkdYykshoCommand() { Id = id, TkdYyksho = tkdYyksho}))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/TkdYykshoes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<TkdYyksho>> PostTkdYyksho([FromBody]TkdYyksho tkdYyksho)
        //{
        //    _context.TkdYyksho.Add(tkdYyksho);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTkdYyksho", new { id = tkdYyksho.UkeNo }, tkdYyksho);
        //}

        // POST: api/TkdYykshoes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TkdYyksho>> PostTkdYyksho([FromBody]TkdYyksho tkdYyksho)
        {
            await Mediator.Send(new PostTkdYykshoCommand() { TkdYyksho = tkdYyksho });
            return CreatedAtAction("GetTkdYyksho", new { id = tkdYyksho.UkeNo }, tkdYyksho);
        }

        // DELETE: api/TkdYykshoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TkdYyksho>> DeleteTkdYyksho(int id)
        {
            var tkdYyksho = await Mediator.Send(new DeleteTkdYykshoCommand() { Id = id });
            if (tkdYyksho == null)
            {
                return NotFound();
            }

            return tkdYyksho;
        }
    }
}
