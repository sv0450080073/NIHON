using HassyaAllrightCloud.Application.BookingIncidental.Commands;
using HassyaAllrightCloud.Application.BookingIncidental.Queries;
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
    public class BookingIncidentalController : ApiController
    {
        private readonly ITPM_CodeSyService _codeSyuService;
        private readonly ILogger<BookingIncidentalController> _logger;
        public BookingIncidentalController(ILogger<BookingIncidentalController> logger,
            ITPM_CodeSyService codeSyuService)
        {
            _logger = logger;
            _codeSyuService = codeSyuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetIncidentalBooking(string ukeNo, IncidentalViewMode viewMode)
        {
            try
            {
                int tenantId = new ClaimModel(HttpContext).TenantID;
                int companyID = new ClaimModel(HttpContext).CompanyID;
                var result = await Mediator.Send(new GetBookingIncidentalQuery(_codeSyuService, ukeNo, tenantId, companyID, viewMode));//TODO
                result.LoadFuttumList = await Mediator.Send(new GetLoadFuttumListQuery(ukeNo, viewMode, result));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Get for futai tab only, not for component futai
        /// </summary>
        /// <param name="ukeNo"></param>
        /// <param name="viewMode"></param>
        /// <returns></returns>
        [HttpGet("LoadFuttumList")]
        public async Task<IActionResult> GetLoadFuttmList(string ukeNo, IncidentalViewMode viewMode)
        {
            try
            {
                var result = await Mediator.Send(new GetLoadFuttumListQuery(ukeNo, viewMode));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("{ukeNo}")]
        public async Task<IActionResult> SaveIncidentalBooking(string ukeNo, [FromBody] IncidentalBooking IncidentalBookingData)
        {
            try
            {
                await Mediator.Send(new InsertIncidentalBookingCommand(ukeNo, IncidentalBookingData));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("{ukeNo}")]
        public async Task<IActionResult> UpdateIncidentalBooking(string ukeNo, [FromBody] IncidentalBooking IncidentalBookingData)
        {
            try
            {
                await Mediator.Send(new UpdateIncidentalBookingCommand(ukeNo, IncidentalBookingData));
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("LoadTsumi")]
        public async Task<ActionResult<List<LoadTsumi>>> GetLoadTsumi(int tenantId)
        {
            try
            {
                var result = await Mediator.Send(new GetLoadTsumiQuery(_codeSyuService, tenantId));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("LoadFutai")]
        public async Task<ActionResult<List<LoadFutai>>> GetLoadFutai(int tenantId)
        {
            try
            {
                var result = await Mediator.Send(new GetLoadFutaiQuery(tenantId));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("LoadNyuRyokinName")]
        public async Task<ActionResult<List<LoadNyuRyokinName>>> GetLoadNyuRyokinName()
        {
            try
            {
                var result = await Mediator.Send(new GetLoadNyuRyokinNameQuery());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("LoadDouro")]
        public async Task<ActionResult<List<LoadDouro>>> GetLoadDouro()
        {
            try
            {
                int tenantId = new ClaimModel(HttpContext).TenantID;
                var result = await Mediator.Send(new GetLoadDouroListQuery(_codeSyuService, tenantId));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("LoadSeisanCd")]
        public async Task<ActionResult<List<LoadSeisanCd>>> GetLoadSeisanCd()
        {
            try
            {
                var result = await Mediator.Send(new GetLoadSeisanCdQuery());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("LoadSeisanKbn")]
        public async Task<ActionResult<List<LoadSeisanKbn>>> GetLoadSeisanKbn()
        {
            try
            {
                int tenantId = new ClaimModel(HttpContext).TenantID;
                var result = await Mediator.Send(new GetLoadSeisanKbnQuery(_codeSyuService, tenantId));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("SettingTaxRate")]
        public async Task<ActionResult<SettingTaxRate>> GetSettingTaxRate()
        {
            try
            {
                var result = await Mediator.Send(new GetSettingTaxRateQuery());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
