using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using HassyaAllrightCloud.Application.Mishum.Commands;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TkdMishumController : ApiController
    {
        // POST: api/TkdUnkobis
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TkdMishum>> PostTkdMiShum(TkdMishum tkdMishum)
        {
            if(await Mediator.Send(new PostTkdMishumCommand { tkdMishum = tkdMishum }))
            {
                return CreatedAtAction("GetTkdMishum", new { id = tkdMishum.UkeNo }, tkdMishum);
            } else
            {
                return Conflict();
            }
        }
    }
}