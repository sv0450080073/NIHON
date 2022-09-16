using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using HassyaAllrightCloud.Application.KasSet.Queries;
using System;
using HassyaAllrightCloud.Application.KasSet.Commands;
using System.Net;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TkmKasSetController : ApiController
    {
        /// <summary>
        /// Get method URL: api/TkmKasSet 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TkmKasSet>>> GetTkmKasSet()
        {
            return Ok(await Mediator.Send(new GetKasSetQuery()));
        }

        /// <summary>
        /// Get method URL. Default URL param: current value is 7. ex: api/TkmKasSet/7
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TkmKasSet>> GetTkmKasSet(int id)
        {            
            var result = await Mediator.Send(new GetKasSetByIdQuery() { Id = id });
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Post method URL: api/TkmKasSet 
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TkmKasSet>> SaveTkmKasSetAsync(TkmKasSet tkmKasSet)
        {
            return Ok(await Mediator.Send(new SaveKasSetAsyncQuery() { tkmKasSet = tkmKasSet }));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTkmKasSetAsync(int companyId)
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteTkmKasSet() { CompanyCdSeq = companyId }));
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}