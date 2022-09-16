using HassyaAllrightCloud.Application.BookingInput.Queries;
using HassyaAllrightCloud.Application.BookingInputMultiCopy.Commands;
using HassyaAllrightCloud.Application.BookingInputMultiCopy.Queries;
using HassyaAllrightCloud.Application.VehicalData.Queries;
using HassyaAllrightCloud.Application.VehicleData.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingInputMultiCopyController : ApiController
    {
        private readonly ILogger<BookingInputMultiCopyController> _logger;

        public BookingInputMultiCopyController(ILogger<BookingInputMultiCopyController> logger)
        {
            _logger = logger;
        }

        [HttpPost("{ukeNo}")]
        public async Task<IActionResult> CopyBookingInput(string ukeNo, [FromBody] BusBookingMultiCopyData bookingCopyData)
        {
            try
            {
                var bookingData = await Mediator.Send(new GetBookingInputQuery(ukeNo));
                if (bookingData == null)
                    return BadRequest();

                var garageDateTime = bookingData.GetBusGarageDateTime();
                IEnumerable<Vehical> vehicles = await Mediator.Send(new GetVehicleByCompanyIdQuery(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, garageDateTime.Leave.ConvertedDate, garageDateTime.Return.ConvertedDate));
                IEnumerable<Vehical> vehiclesAssigned = await Mediator.Send(
                    new GetVehiclesAssignedQuery(garageDateTime.Leave.ConvertedDate.AddMinutes(-Common.BusCheckBeforeOrAfterRunningDuration),
                                                 garageDateTime.Return.ConvertedDate.AddMinutes(Common.BusCheckBeforeOrAfterRunningDuration)));

                var newUkeCdList = await Mediator.Send(new MultiCopyBookingInputCommand(bookingCopyData, ukeNo, (vehicles.ToList(), vehiclesAssigned.ToList())));
                if (newUkeCdList.Any(e => e == Constants.ErrorMessage.UkenoCdIsFull))
                {
                    return BadRequest();
                }

                return Ok(newUkeCdList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("yyksho")]
        public async Task<IActionResult> GetYykSho(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetYykshoByUkeNoQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("unkobi")]
        public async Task<IActionResult> GetUnkobi(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetUnkobiByUkeNoQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("mishumList")]
        public async Task<IActionResult> GetMishumList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetMishumListByUkeNoQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("yykSyuList")]
        public async Task<IActionResult> GetYykSyuList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetYykSyuListByUkeNoQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("haishaList")]
        public async Task<IActionResult> GetHaishaList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetHaishaListByUkeNoQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("bookingMaxMinFareFeeCalsList")]
        public async Task<IActionResult> GetBookingMaxMinFareFeeCalsList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetBookingFareFeeListByUkeNoQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("bookingMaxMinFareFeeCalcMeisaiList")]
        public async Task<IActionResult> GetBookingMaxMinFareFeeCalcMeisaiList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetBookingFareFeeMeisaiListByUkeNoQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("koteiList")]
        public async Task<IActionResult> GetKoteiList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetKoteiListQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("koteiKList")]
        public async Task<IActionResult> GetKoteiKList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetKoteiKListQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("tehaiList")]
        public async Task<IActionResult> GetTehaiList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetTehaiListQuery(ukeNo));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("futumList")]
        public async Task<IActionResult> GetFutumList(string ukeNo, byte futumKbn)
        {
            try
            {
                var result = await Mediator.Send(new GetFutumListQuery(ukeNo, futumKbn));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("mfutuList")]
        public async Task<IActionResult> GetMFutuList(string ukeNo, byte futumKbn)
        {
            try
            {
                var result = await Mediator.Send(new GetFutumListQuery(ukeNo, futumKbn));
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
