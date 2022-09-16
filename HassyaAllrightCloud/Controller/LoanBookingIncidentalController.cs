using HassyaAllrightCloud.Application.LoanBookingIncidental.Commands;
using HassyaAllrightCloud.Application.LoanBookingIncidental.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanBookingIncidentalController : ApiController
    {
        private readonly ILogger<BookingIncidentalController> _logger;
        public LoanBookingIncidentalController(ILogger<BookingIncidentalController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<LoanBookingIncidentalData>> GetLoanIncidentalBooking(int tenantId, string ukeNo, short unkRen, int youTblSeq, IncidentalViewMode viewMode)
        {
            try
            {
                var result = await Mediator.Send(new GetLoanIncidentalBooking(tenantId, ukeNo, unkRen, youTblSeq, viewMode));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpPut]
        public async Task<IActionResult> SaveLoanBookingIncidental([FromBody] LoanBookingIncidentalData incidentalData)
        {
            try
            {
                await Mediator.Send(new SaveLoanBookingIncidentalDataCommand(incidentalData));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("LoadYFutai")]
        public async Task<ActionResult<List<LoadYFutai>>> GetLoadYFutai(int tenantId)
        {
            try
            {
                var result = await Mediator.Send(new GetLoadYFutaiQuery(tenantId));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("LoadYTsumi")]
        public async Task<ActionResult<List<LoadYTsumi>>> GetLoadYTsumi()
        {
            try
            {
                var result = await Mediator.Send(new GetLoadYTsumiQuery());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("LoadYRyokin")]
        public async Task<ActionResult<List<LoadYRyokin>>> GetLoadYRyokin()
        {
            try
            {
                var result = await Mediator.Send(new GetLoadYRyokinQuery());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("LoadYSeisan")]
        public async Task<ActionResult<List<LoadYSeisan>>> GetLoadYSeisan(int tenantId)
        {
            try
            {
                var result = await Mediator.Send(new GetLoadYSeisanQuery(tenantId));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("TaxTypeList")]
        public async Task<ActionResult<List<TaxTypeList>>> GetTaxTypeList(int tenantId)
        {
            try
            {
                return Ok(await Mediator.Send(new GetTaxTypeListQuery(tenantId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
