using HassyaAllrightCloud.Application.BookingArrangement.Commands;
using HassyaAllrightCloud.Application.BookingArrangement.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingArrangementController : ApiController
    {
        private readonly ILogger<BookingArrangementController> _logger;
        private readonly ITPM_CodeSyService _codeSyuService;

        public BookingArrangementController(ILogger<BookingArrangementController> logger, 
            ITPM_CodeSyService codeSyuService)
        {
            _logger = logger;
            _codeSyuService = codeSyuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingArrangement(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            try
            {
                var result = await Mediator.Send(new GetBookingArrangementListQuery(ukeNo, unkRen, teiDanNo, bunkRen));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> SaveBookingArrangement(string ukeNo, short unkRen, short teiDanNo, short bunkRen, [FromBody] List<BookingArrangementData> arrangementList)
        {
            try
            {
                await Mediator.Send(new UpdateBookingArrangementCommand(ukeNo, unkRen, teiDanNo, bunkRen, arrangementList));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("ArrangementCar")]
        public async Task<IActionResult> GetCarArrangement(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetCarArrangementListQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("ArrangementType")]
        public async Task<IActionResult> GetArrangementType(string ukeNo)
        {
            try
            {
                int tenantId = new ClaimModel(HttpContext).TenantID;
                var result = await Mediator.Send(new GetArrangementTypeListQuery(_codeSyuService, ukeNo, tenantId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("ArrangementCode")]
        public async Task<IActionResult> GetArrangementCode(string ukeNo)
        {
            try
            {
                int tenantId = new ClaimModel(HttpContext).TenantID;
                var result = await Mediator.Send(new GetArrangementCodeListQuery(_codeSyuService, ukeNo, tenantId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("ArrangementPlaceType")]
        public async Task<IActionResult> GetArrangementPlaceType(string ukeNo)
        {
            try
            {
                int tenantId = new ClaimModel(HttpContext).TenantID;
                var result = await Mediator.Send(new GetArrangementPlaceTypeListQuery(_codeSyuService, ukeNo, tenantId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("ArrangementLocation")]
        public async Task<IActionResult> GetArrangementLocation(string ukeNo)
        {
            try
            {
                int tenantId = new ClaimModel(HttpContext).TenantID;
                var result = await Mediator.Send(new GetArrangementLocationListQuery(_codeSyuService, ukeNo, tenantId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
