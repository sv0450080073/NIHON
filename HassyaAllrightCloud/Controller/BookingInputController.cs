using HassyaAllrightCloud.Application.BookingIncidental.Queries;
using HassyaAllrightCloud.Application.BookingArrangement.Queries;
using HassyaAllrightCloud.Application.BookingInput.Commands;
using HassyaAllrightCloud.Application.BookingInput.Queries;
using HassyaAllrightCloud.Application.UpdateBookingInput.Commands;
using HassyaAllrightCloud.Application.UpdateBookingInput.Queries;
using HassyaAllrightCloud.Application.VehicalData.Queries;
using HassyaAllrightCloud.Application.VehicleData.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using HassyaAllrightCloud.Application.CustomItem.Queries;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingInputController : ApiController
    {
        private readonly ILogger<BookingInputController> _logger;
        public BookingInputController(ILogger<BookingInputController> logger)
        {
            _logger = logger;
        }

        // GET: api/BookingInput/{ukeNo}
        [HttpGet("{ukeNo}")]
        public async Task<ActionResult<BookingFormData>> GetBookingInput(string ukeNo)
        {
            try
            {
                var bookingData = await Mediator.Send(new GetBookingInputQuery(ukeNo));
                if (bookingData == null) return null;
                bookingData.ConfirmationTabDataList = (await Mediator.Send(new GetConfirmationTabQuery(ukeNo))).ToList();
                // Get data for 予約車種
                bookingData.VehicleGridDataList = (await Mediator.Send(new GetVehicleGridDataQuery(ukeNo))).ToList();
                // Get data for futai
                bookingData.FuttumViewList = (await Mediator.Send(new GetLoadFuttumListQuery(ukeNo, IncidentalViewMode.All))).ToList();
                // Get data for Tehai for common car
                bookingData.ArrangementTabList = (await Mediator.Send(new GetBookingArrangementListQuery(ukeNo, unkRen:1, teiDanNo:0, bunkRen:1)));
                // check ediable booking
                var checkEdit = await Mediator.Send(new GetDisabledBookingStateList(ukeNo));
                bookingData.IsPaidOrCoupon = checkEdit.Contains(BookingDisableEditState.PaidOrCoupon);
                bookingData.IsLock = checkEdit.Contains(BookingDisableEditState.Locked);
                var fieldConfigs = await Mediator.Send(new GetCustomItemQuery(new ClaimModel(HttpContext).TenantID));
                bookingData.CustomData = await Mediator.Send(new GetCustomFieldQuery(fieldConfigs, ukeNo));
                return bookingData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookingInput(string id, [FromBody] BookingFormData bookingdata)
        {
            try
            {
                var garageDateTime = bookingdata.GetBusGarageDateTime();
                IEnumerable<Vehical> vehicles = await Mediator.Send(new GetVehicleByCompanyIdQuery(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, garageDateTime.Leave.ConvertedDate, garageDateTime.Return.ConvertedDate));
                IEnumerable<Vehical> vehiclesAssigned = await Mediator.Send(
                    new GetVehiclesAssignedQuery(garageDateTime.Leave.ConvertedDate.AddMinutes(-Common.BusCheckBeforeOrAfterRunningDuration),
                                                 garageDateTime.Return.ConvertedDate.AddMinutes(Common.BusCheckBeforeOrAfterRunningDuration)));
                // refactor this later
                await Mediator.Send(new InsertConfirmationTabCommand { UkeNo = id, ConfirmationTabDataList = bookingdata.ConfirmationTabDataList });

                TkdYyksho yyksho = await Mediator.Send(new GetYykshoByUkenoQuery { Ukeno = id, BookingData = bookingdata });
                if (id != yyksho.UkeNo)
                {
                    return new BadRequestResult();
                }
                TkdUnkobi unkobi = await Mediator.Send(new GetUnkobiByUkenoQuery { Ukeno = id, BookingData = bookingdata });
                Dictionary<CommandMode, List<TkdMishum>> mishums = await Mediator.Send(new GetMishumByUkenoQuery { Ukeno = id, BookingData = bookingdata });
                Dictionary<CommandMode, List<TkdYykSyu>> yykSyus = await Mediator.Send(new GetYyksyuByUkenoQuery { Ukeno = id, BookingData = bookingdata });
                Dictionary<CommandMode, List<TkdHaisha>> haishas =
                    await Mediator.Send(new GetHaishaByUkenoQuery { Ukeno = id, BookingData = bookingdata, VehiclesInfo = (vehicles.ToList(), vehiclesAssigned.ToList()) });
                Dictionary<CommandMode, List<TkdKariei>> karieis = await Mediator.Send(new GetKarieiByHaishaQuery(id, bookingdata, haishas));
                Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalc>> maxMinFareFeeCalcs =
                    await Mediator.Send(new GetMaxMinFareFeeCalcByUkeno { Ukeno = id, BookingData = bookingdata });
                Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalcMeisai>> maxMinFareFeeCalcMeisais =
                    await Mediator.Send(new GetMaxMinFareFeeCalcMeisaiByUkenoQuery { Ukeno = id, BookingData = bookingdata });
                Domain.Dto.BookingInputData.BookingSoftRemoveEntitiesData softRemoveEntitiesData = 
                    await Mediator.Send(new GetRemoveRequireDataByUkenoQuery { Ukeno = id, BookingData = bookingdata });
                return await Mediator.Send(new UpdateBookingInputCommand
                {
                    Ukeno = id,
                    BookingData = bookingdata,
                    Yyksho = yyksho,
                    Unkobi = unkobi,
                    YykSyus = yykSyus,
                    Mishums = mishums,
                    Haishas = haishas,
                    Karieis = karieis,
                    MaxMinFareFeeCalcs = maxMinFareFeeCalcs,
                    MaxMinFareFeeCalcMeisais = maxMinFareFeeCalcMeisais,
                    SoftRemoveEntitiesData = softRemoveEntitiesData
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookingFormData>> PostBookingInput([FromBody] BookingFormData bookingFormData)
        {
            try
            {
                var garageDateTime = bookingFormData.GetBusGarageDateTime();
                IEnumerable<Vehical> vehicles = await Mediator.Send(new GetVehicleByCompanyIdQuery(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, garageDateTime.Leave.ConvertedDate, garageDateTime.Return.ConvertedDate));
                IEnumerable<Vehical> vehiclesAssigned = await Mediator.Send(
                    new GetVehiclesAssignedQuery(garageDateTime.Leave.ConvertedDate.AddMinutes(-Common.BusCheckBeforeOrAfterRunningDuration),
                                                 garageDateTime.Return.ConvertedDate.AddMinutes(Common.BusCheckBeforeOrAfterRunningDuration)));

                var ukeNo = await Mediator.Send(new InsertBookingInputCommand(bookingFormData, (vehicles.ToList(), vehiclesAssigned.ToList())));

                if (ukeNo == "-1")
                {
                    return BadRequest();
                }
                else if (ukeNo == Constants.ErrorMessage.UkenoCdIsFull)
                {
                    return BadRequest(ukeNo);
                }

                if (bookingFormData.IsCopyMode)
                {
                    await Mediator.Send(new CopyTkdKoteiCommand(bookingFormData.UkeNo, ukeNo));
                    await Mediator.Send(new CopyTkdKoteiKCommand(bookingFormData.UkeNo, ukeNo));

                    await Mediator.Send(new CopyTkdFutTumCommand(bookingFormData.UkeNo, ukeNo));
                    await Mediator.Send(new CopyTkdMFutTuCommand(bookingFormData.UkeNo, ukeNo));
                    await Mediator.Send(new CopyTkdTehaiCommand(bookingFormData.UkeNo, ukeNo));
                }
                return Content(ukeNo.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpPut("updateConfirmList/{ukeNo}")]
        public async Task<IActionResult> UpdateConfirmationDataList(string ukeNo, [FromBody] List<ConfirmationTabData> confirmationTabDataList)
        {
            try
            {
                await Mediator.Send(new InsertConfirmationTabCommand { UkeNo = ukeNo, ConfirmationTabDataList = confirmationTabDataList });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("updateCancel/{ukeNo}")]
        public async Task<IActionResult> UpdateCancelData(string ukeNo, [FromBody] CancelTickedData cancelData)
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

                await Mediator.Send(new UpdateCancelCommand(ukeNo, cancelData, (vehicles.ToList(), vehiclesAssigned.ToList())));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Supervisor/Supplier")]
        public async Task<ActionResult<List<SupplierData>>> GetAllSupplierData()
        {
            try
            {
                return (await Mediator.Send(new GetSupplierDataQuery() { TenantId = new ClaimModel(HttpContext).TenantID })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("Reservation/Destination")]
        public async Task<ActionResult<List<DestinationData>>> GetAllDestinationData()
        {
            try
            {
                return (await Mediator.Send(new GetDestinationDataQuery() { TenantId = new ClaimModel(HttpContext).TenantID })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet]
        [Route("Reservation/Place")]
        public async Task<ActionResult<List<PlaceData>>> GetAllPlaceData()
        {
            try
            {
                return (await Mediator.Send(new GetPlaceDataQuery() { TenantId = new ClaimModel(HttpContext).TenantID })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("Reservation/GetCodeKbnByCodeSyu/{codeSyu}")]
        public async Task<ActionResult<List<TPM_CodeKbCodeSyuData>>> GetCodeKbnByCodeSyu(string codeSyu)
        {
            try
            {
                return (await Mediator.Send(new GetCodeKbnCodeSyuDataQuery(codeSyu, new ClaimModel(HttpContext).TenantID))).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("PassengerType")]
        public async Task<ActionResult<List<PassengerType>>> GetAllPassengerType()
        {
            try
            {
                return (await Mediator.Send(new GetPassengerTypeQuery() { TenantId = new ClaimModel(HttpContext).TenantID })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("Cancellation/SettingStaff")]
        public async Task<ActionResult<List<SettingStaff>>> GetAllSettingStaff()
        {
            try
            {
                return (await Mediator.Send(new GetSettingStaffQuery() { TenantId = new ClaimModel(HttpContext).TenantID })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("InvoiceType")]
        public async Task<ActionResult<List<InvoiceType>>> GetAllInvoiceType()
        {
            try
            {
                var result = (await Mediator.Send(new GetInvoiceTypeQuery() { TenantId = new ClaimModel(HttpContext).TenantID })).ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("DisabledBookingStateList")]
        public async Task<ActionResult<List<BookingDisableEditState>>> GetDisabledBookingStateList(string ukeNo)
        {
            try
            {
                var result = await Mediator.Send(new GetDisabledBookingStateList(ukeNo));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
}
        }

        [HttpGet("GetBikoNm")]
        public async Task<IActionResult> GetBikoNm(string ukeNo, bool isUnkobi, short unkRen)
        {
            try
            {
                string bikoNm = await Mediator.Send(new GetBikoNmQuery(ukeNo, isUnkobi, unkRen));
                return Ok(bikoNm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("SaveBikoNm")]
        public async Task<IActionResult> SaveBikoNm(string ukeNo, bool isUnkobi, short unkRen,[FromBody] string bikoNm)
        {
            try
            {
                await Mediator.Send(new SaveBikoNmCommand(ukeNo, bikoNm, unkRen, isUnkobi));
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