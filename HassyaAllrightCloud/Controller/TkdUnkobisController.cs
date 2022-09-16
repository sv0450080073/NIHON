using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using HassyaAllrightCloud.Application.Unkobi.Commands;
using HassyaAllrightCloud.Application.Unkobi.Queries;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TkdUnkobisController : ApiController
    {
        // GET: api/TkdUnkobis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TkdUnkobi>>> GetTkdUnkobi()
        {
            return Ok(await Mediator.Send(new GetTkdUnkobiQuery()));
        }

        // GET: api/TkdUnkobis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TkdUnkobi>> GetTkdUnkobi(string id)
        {
            var result = await Mediator.Send(new GetTkdUnkobiByIdQuery() { Id = id });
            if (result == null)
            {
                return NotFound();
            } else
            {
                return result;
            }
        }

        // PUT: api/TkdUnkobis/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTkdUnkobi(string id, TkdUnkobi tkdUnkobi)
        {
            if (id != tkdUnkobi.UkeNo)
            {
                return BadRequest();
            }

            if(await Mediator.Send(new PutTkdUnkobiCommand() { Id = id, TkdUnkobi = tkdUnkobi }))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/TkdUnkobis
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TkdUnkobi>> PostTkdUnkobi(TkdUnkobi tkdUnkobi)
        {
            if (await Mediator.Send(new PostTkdUnkobiCommand { TkdUnkobi = tkdUnkobi }))
            {
                return CreatedAtAction("GetTkdUnkobi", new { id = tkdUnkobi.UkeNo }, tkdUnkobi);
            } else
            {
                return Conflict();
            }
        }

        // DELETE: api/TkdUnkobis/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TkdUnkobi>> DeleteTkdUnkobi(string id)
        {
            var result = await Mediator.Send(new DeleteTkdUnkobiCommand() { Id = id });
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        
    }
}
