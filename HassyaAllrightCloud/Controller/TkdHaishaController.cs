using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using HassyaAllrightCloud.Application.Haisha.Commands;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TkdHaishaController : ApiController
    {
        private readonly KobodbContext _context;

        public TkdHaishaController(KobodbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<TkdHaisha>> PostTkdHaiSha(TkdHaisha tkdHaisha)
        {
            if (await Mediator.Send(new PostTkdHaishaCommand { tkdHaisha = tkdHaisha }))
            {
                return CreatedAtAction("GetTkdHaisha", new { id = tkdHaisha.UkeNo }, tkdHaisha);
            } else
            {
                return Conflict();
            }
        }
       


    }
}