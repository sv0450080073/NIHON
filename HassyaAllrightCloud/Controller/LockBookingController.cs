using HassyaAllrightCloud.Application.LockBooking.Commands;
using HassyaAllrightCloud.Application.LockBooking.Queries;
using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockBookingController : ApiController
    {
        private readonly ILogger<LockBookingController> _logger;
        public LockBookingController(ILogger<LockBookingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingArrangement(int page, byte size)
        {
            try
            {
                var result = await Mediator.Send(new GetLockBookingQuery(page, size));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLockTableData([FromBody] LockBookingData lockBooking)
        {
            try
            {
                await Mediator.Send(new UpdateLockBookingCommand(lockBooking));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
