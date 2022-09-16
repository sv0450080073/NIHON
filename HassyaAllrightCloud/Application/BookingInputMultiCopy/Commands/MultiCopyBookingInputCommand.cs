using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Application.BookingInputMultiCopy.Commands
{
    public class MultiCopyBookingInputCommand : IRequest<List<string>>
    {
        private readonly string _ukeNoToCopy;
        private readonly BusBookingMultiCopyData _bookingCopyData;
        private readonly (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) _vehiclesInfo;

        public MultiCopyBookingInputCommand(BusBookingMultiCopyData bookingCopyData, string ukeNoToCopy, 
            (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) vehiclesInfo)
        {
            _bookingCopyData = bookingCopyData ?? throw new ArgumentNullException(nameof(bookingCopyData));
            _ukeNoToCopy = ukeNoToCopy ?? throw new ArgumentNullException(nameof(ukeNoToCopy));
            _vehiclesInfo.Vehicles = vehiclesInfo.Vehicles ?? throw new ArgumentNullException(nameof(vehiclesInfo.Vehicles));
            _vehiclesInfo.VehiclesAssigned = vehiclesInfo.VehiclesAssigned ?? throw new ArgumentNullException(nameof(vehiclesInfo.VehiclesAssigned));
        }

        public class Handler : IRequestHandler<MultiCopyBookingInputCommand, List<string>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<MultiCopyBookingInputCommand> _logger;
            private readonly ITKD_YykshoListService _yykshoService;

            public Handler(KobodbContext context, ILogger<MultiCopyBookingInputCommand> logger, ITKD_YykshoListService yykshoService)
            {
                _context = context;
                _logger = logger;
                _yykshoService = yykshoService;
            }

            public async Task<List<string>> Handle(MultiCopyBookingInputCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    int numberOfBookingCopy = request._bookingCopyData.BookingDataChangedList.Count;
                    int maxUkeCd;
                    string[] newUkeNoList;
                    (maxUkeCd, newUkeNoList) = await _yykshoService.GetNewUkeNo(new ClaimModel().TenantID, numberOfBookingCopy);

                    if (maxUkeCd + numberOfBookingCopy > int.MaxValue) // max value of ukenocd in integer type is 2147483647
                    {
                        return new List<string>() { Constants.ErrorMessage.UkenoCdIsFull };
                    }

                    int index = 0;
                    foreach (var bookingCopy in request._bookingCopyData.BookingDataChangedList)
                    {
                        // set new datetime, invoice... in grid into booking data
                        SetNewDataBookingForm(request._bookingCopyData, request._bookingCopyData.BookingDataChangedList.IndexOf(bookingCopy));
                        string newUkeNo = newUkeNoList[index++];

                        TkdYyksho yyksho = CopyDataYyksho(request._bookingCopyData, maxUkeCd, newUkeNo);
                        TkdUnkobi unkobi = CopyDataUnkobi(request._bookingCopyData, newUkeNo);
                        List<TkdMishum> mishumList = CopyDataMishum(request._bookingCopyData, newUkeNo);
                        List<TkdYykSyu> yykSyuList = CopyDataYykSyu(request._bookingCopyData, newUkeNo);
                        List<TkdHaisha> haiShaList = new List<TkdHaisha>();
                        if(request._bookingCopyData.CopyType == BookingMultiCopyType.Reservation)
                        {
                            haiShaList = CollectDataHaishaAutoAssign(request._bookingCopyData.BookingDataToCopy, newUkeNo, request._vehiclesInfo);

                            int totalBookingWithAsignBus = haiShaList.Where(e => e.HaiSsryCdSeq > 0).Count();
                            if (haiShaList.Count == totalBookingWithAsignBus)
                            {
                                yyksho.Kskbn = 2;
                            }
                            else if (totalBookingWithAsignBus == 0)
                            {
                                yyksho.Kskbn = 1;
                            }
                            else
                            {
                                yyksho.Kskbn = 3;
                            }
                            unkobi.Kskbn = yyksho.Kskbn;
                        }
                        else if(request._bookingCopyData.CopyType == BookingMultiCopyType.Vehicle)
                        {
                            haiShaList = CopyDataHaisha(request._bookingCopyData, newUkeNo);
                        }
                        List<TkdBookingMaxMinFareFeeCalc> bookingFareFeeList = CopyDataBookingMaxMinFareFeeCals(request._bookingCopyData, newUkeNo);
                        List<TkdBookingMaxMinFareFeeCalcMeisai> bookingFareFeeMesaiList = CopyDataBookingMaxMinFareFeeCalsDetails(request._bookingCopyData, newUkeNo);

                        await _context.TkdYyksho.AddAsync(yyksho);
                        await _context.TkdUnkobi.AddAsync(unkobi);
                        await _context.TkdMishum.AddRangeAsync(mishumList);
                        await _context.TkdYykSyu.AddRangeAsync(yykSyuList);
                        await _context.TkdHaisha.AddRangeAsync(haiShaList);
                        await _context.TkdBookingMaxMinFareFeeCalc.AddRangeAsync(bookingFareFeeList);
                        await _context.TkdBookingMaxMinFareFeeCalcMeisai.AddRangeAsync(bookingFareFeeMesaiList);

                        // copy additional table
                        double dateChange = request._bookingCopyData.NumberOfDateChanged;
                        List<TkdKotei> koteiList = CopyDataKotei(request._bookingCopyData, newUkeNo);
                        List<TkdKoteik> koteiKList = CopyDataKoteiK(request._bookingCopyData, newUkeNo);
                        List<TkdTehai> tehaiList = CopyDataTehai(request._bookingCopyData, newUkeNo);
                        List<TkdFutTum> futaiList = CopyDataFutum(
                            request._bookingCopyData.IsCopyIncludedServices,
                            newUkeNo,
                            request._bookingCopyData.FutaiList,
                            dateChange
                        );
                        List<TkdMfutTu> mfutaiList = CopyDataMFutTu(request._bookingCopyData.IsCopyIncludedServices, newUkeNo, request._bookingCopyData.MFutaiList);
                        List<TkdFutTum> tsumiList = CopyDataFutum(
                            request._bookingCopyData.IsCopyIncludedGoods,
                            newUkeNo,
                            request._bookingCopyData.TsumiList,
                            dateChange
                        );
                        List<TkdMfutTu> mtsumiList = CopyDataMFutTu(request._bookingCopyData.IsCopyIncludedServices, newUkeNo, request._bookingCopyData.MTsumiList);

                        await _context.TkdKotei.AddRangeAsync(koteiList);
                        await _context.TkdKoteik.AddRangeAsync(koteiKList);
                        await _context.TkdTehai.AddRangeAsync(tehaiList);
                        await _context.TkdFutTum.AddRangeAsync(futaiList);
                        await _context.TkdMfutTu.AddRangeAsync(mfutaiList);
                        await _context.TkdFutTum.AddRangeAsync(tsumiList);
                        await _context.TkdMfutTu.AddRangeAsync(mtsumiList);

                        maxUkeCd += 1;
                        await _context.SaveChangesAsync();
                    }

                    trans.Commit();
                    return newUkeNoList.Select(_ => _.Substring(5)).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }

            /// <summary>
            /// Set data about bus start date, end date, ... in new grid to booking data
            /// </summary>
            /// <param name="multiCopyData"></param>
            /// <param name="position">index of grid item in list</param>
            private void SetNewDataBookingForm(BusBookingMultiCopyData multiCopyData, int position)
            {
                var bookingNewData = multiCopyData.BookingDataChangedList[position];

                multiCopyData.NumberOfDateChanged = (bookingNewData.StartDate - multiCopyData.BookingDataToCopy.BusStartDate).TotalDays;

                multiCopyData.BookingDataToCopy.BusStartDate = bookingNewData.StartDate;
                multiCopyData.BookingDataToCopy.BusStartTime = bookingNewData.StartTime;
                multiCopyData.BookingDataToCopy.BusEndDate = bookingNewData.EndDate;
                multiCopyData.BookingDataToCopy.BusEndTime = bookingNewData.EndTime;

                multiCopyData.BookingDataToCopy.ReservationTabData.SyuKoTime = bookingNewData.StartTime;
                multiCopyData.BookingDataToCopy.ReservationTabData.SyuPatime = bookingNewData.StartTime;
                multiCopyData.BookingDataToCopy.ReservationTabData.KikTime = bookingNewData.EndTime;

                if(multiCopyData.BookingDataToCopy.PreDaySetting)
                {
                    multiCopyData.BookingDataToCopy.ReservationTabData.GarageLeaveDate = bookingNewData.StartDate.AddDays(-1);
                }
                else
                {
                    multiCopyData.BookingDataToCopy.ReservationTabData.GarageLeaveDate = bookingNewData.StartDate;
                }

                if (multiCopyData.BookingDataToCopy.AftDaySetting)
                {
                    multiCopyData.BookingDataToCopy.ReservationTabData.GarageReturnDate = bookingNewData.EndDate.AddDays(1);
                }
                else
                {
                    multiCopyData.BookingDataToCopy.ReservationTabData.GarageReturnDate = bookingNewData.EndDate;
                }

                multiCopyData.BookingDataToCopy.InvoiceDate = bookingNewData.InvoiceDate;
                multiCopyData.BookingDataToCopy.InvoiceMonth = bookingNewData.InvoiceMonth.ToString("yyyyMM");
            }

            private TkdYyksho CopyDataYyksho(BusBookingMultiCopyData multiCopyData, int maxUkeCd, string newUkeNo)
            {
                // set old data will not change
                var yyksho = new TkdYyksho();
                yyksho.SimpleCloneProperties(multiCopyData.YykshoCopyData);

                // set new data here
                yyksho.TenantCdSeq = new ClaimModel().TenantID;
                yyksho.UkeNo = newUkeNo;
                yyksho.UkeCd = maxUkeCd + 1;
                yyksho.HenKai = 0;
                yyksho.UkeYmd = DateTime.Today.ToString("yyyyMMdd");
                // not copy cancel status
                if(multiCopyData.YykshoCopyData.YoyaSyu == 1 || multiCopyData.YykshoCopyData.YoyaSyu == 2)
                {
                    yyksho.YoyaSyu = 1;
                }
                // status for estimate
                else if(multiCopyData.YykshoCopyData.YoyaSyu == 3 || multiCopyData.YykshoCopyData.YoyaSyu == 4)
                {
                    yyksho.YoyaSyu = 3;
                }
                yyksho.YoyaNm = multiCopyData.BookingDataToCopy.TextOrganizationName;
                yyksho.TokuiSeq = multiCopyData.BookingDataToCopy.customerComponentTokiStData.TokuiSeq;
                yyksho.SitenCdSeq = multiCopyData.BookingDataToCopy.customerComponentTokiStData.SitenCdSeq;
                yyksho.SeikYm = multiCopyData.BookingDataToCopy.InvoiceMonth;
                yyksho.SeiTaiYmd = multiCopyData.BookingDataToCopy.InvoiceDate.ToString("yyyyMMdd");
                yyksho.Zeiritsu = Convert.ToDecimal(multiCopyData.BookingDataToCopy.TaxRate);
                yyksho.TaxTypeforGuider = (byte)multiCopyData.BookingDataToCopy.TaxTypeforGuider.IdValue;
                yyksho.ZeiKbn = (byte)multiCopyData.BookingDataToCopy.TaxTypeforBus.IdValue;
                yyksho.CanRit = 0;
                yyksho.CanUnc = 0;
                yyksho.CanZkbn = 0;
                yyksho.CanSyoR = 0;
                yyksho.CanSyoG = 0;
                yyksho.CanYmd = string.Empty;
                yyksho.CanTanSeq = 0;
                yyksho.CanRiy = string.Empty;
                yyksho.CanFuYmd = string.Empty;
                yyksho.CanFuTanSeq = 0;
                yyksho.CanFuRiy = string.Empty;
                yyksho.KaktYmd = string.Empty;
                yyksho.HaiSkbn = 1;
                yyksho.HaiSkbn = 1;
                yyksho.SiyoKbn = 1;
                yyksho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                yyksho.UpdTime = DateTime.Now.ToString("HHmmss");
                yyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                yyksho.UpdPrgId = "KJ3300";

                return yyksho;
            }

            private TkdUnkobi CopyDataUnkobi(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var unkobi = new TkdUnkobi();
                BookingFormData bookingData = multiCopyData.BookingDataToCopy;
                TkmKasSet tkmKasSet = _context.TkmKasSet.Where(t => t.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).FirstOrDefault();
                short uriKbn = tkmKasSet.UriKbn;

                // set old data will not change
                unkobi.SimpleCloneProperties(multiCopyData.UnkobiCopyData);

                // set new data here
                unkobi.UkeNo = newUkeNo;
                unkobi.HenKai = 0;
                unkobi.HaiSymd = bookingData.BusStartDate.ToString("yyyyMMdd");
                unkobi.HaiStime = bookingData.BusStartTime.ToStringWithoutDelimiter();

                var touDate = new BookingInputHelper.MyDate(bookingData.BusEndDate, bookingData.BusEndTime);
                unkobi.TouYmd = touDate.ConvertedDate.ToString("yyyyMMdd");
                unkobi.TouChTime = touDate.ConvertedTime.ToStringWithoutDelimiter();
                unkobi.DispTouYmd = bookingData.BusEndDate.ToString("yyyyMMdd");
                unkobi.DispTouChTime = bookingData.BusEndTime.ToStringWithoutDelimiter();
                unkobi.SyuPaTime = bookingData.ReservationTabData.SyuPatime.ToStringWithoutDelimiter();
                if (uriKbn == 1)
                {
                    unkobi.UriYmd = unkobi.HaiSymd;
                }
                else if (uriKbn == 2)
                {
                    unkobi.UriYmd = unkobi.TouYmd;
                }
                unkobi.DanTaNm = multiCopyData.BookingDataToCopy.TextOrganizationName;
                unkobi.DanTaKana = string.Empty;
                if(multiCopyData.BookingDataToCopy.ReservationTabData.Destination!=null)
                {
                    unkobi.IkMapCdSeq = multiCopyData.BookingDataToCopy.ReservationTabData.Destination.BasyoMapCdSeq;
                }
                else
                {
                    unkobi.IkMapCdSeq =0;
                }
                if (multiCopyData.BookingDataToCopy.ReservationTabData != null)
                {
                     unkobi.IkNm = multiCopyData.BookingDataToCopy.ReservationTabData.IkNm;
                }
                else
                {
                     unkobi.IkNm = "";
                }
                if ( multiCopyData.BookingDataToCopy.ReservationTabData.DespatchingPlace != null)
                {
                    unkobi.HaiScdSeq = multiCopyData.BookingDataToCopy.ReservationTabData.DespatchingPlace.HaiSCdSeq;
                }
                else
                {
                     unkobi.HaiScdSeq = 0;
                }
                if (multiCopyData.BookingDataToCopy.ReservationTabData != null)
                {
                    unkobi.HaiSnm = multiCopyData.BookingDataToCopy.ReservationTabData.HaiSNm;
                }
                else
                {
                   unkobi.HaiSnm = "";
                }
                
                unkobi.HaiSjyus1 = string.Empty;
                unkobi.HaiSjyus2 = string.Empty;
                unkobi.HaiSkouKcdSeq = 0;
                unkobi.HaiSbinCdSeq = 0;
                unkobi.HaiSsetTime = string.Empty;
                if (multiCopyData.BookingDataToCopy.ReservationTabData.ArrivePlace != null)
                {
                    unkobi.TouCdSeq = multiCopyData.BookingDataToCopy.ReservationTabData.ArrivePlace.HaiSCdSeq;
                }
                else
                {
                    unkobi.TouCdSeq = 0;
                }
                if ( multiCopyData.BookingDataToCopy.ReservationTabData != null)
                {
                    unkobi.TouNm = multiCopyData.BookingDataToCopy.ReservationTabData.TouNm;
                }
                else
                {
                   unkobi.TouNm = "";
                }
                unkobi.TouJyusyo1 = string.Empty;
                unkobi.TouJyusyo2 = string.Empty;
                unkobi.TouKouKcdSeq = 0;
                unkobi.TouBinCdSeq = 0;
                unkobi.TouSetTime = string.Empty;
                unkobi.AreaMapSeq = 0;
                unkobi.AreaNm = string.Empty;
                unkobi.HasMapCdSeq = 0;
                unkobi.HasNm = string.Empty;
                unkobi.OthJinKbn1 = 99;
                unkobi.OthJin1 = 0;
                unkobi.OthJinKbn2 = 99;
                unkobi.OthJin2 = 0;
                unkobi.KhinKbn = 1;
                unkobi.HaiSkbn = 1;
                unkobi.HaiIkbn = 1;
                unkobi.GuiWnin = 0;
                unkobi.NippoKbn = 1;
                unkobi.YouKbn = 1;
                unkobi.RotCdSeq = 0;
                unkobi.SyukoYmd = bookingData.BusStartDate.ToString("yyyyMMdd");
                unkobi.SyuKoTime = bookingData.ReservationTabData.SyuKoTime.ToStringWithoutDelimiter();
                var kikDate = new BookingInputHelper.MyDate(bookingData.ReservationTabData.GarageReturnDate, bookingData.ReservationTabData.KikTime);
                unkobi.KikYmd = kikDate.ConvertedDate.ToString("yyyyMMdd");
                unkobi.KikTime = kikDate.ConvertedTime.ToStringWithoutDelimiter();
                unkobi.DispKikYmd = bookingData.ReservationTabData.GarageReturnDate.ToString("yyyyMMdd");
                unkobi.DispKikTime = bookingData.ReservationTabData.KikTime.ToStringWithoutDelimiter();
                unkobi.BikoTblSeq = 0;
                unkobi.SiyoKbn = 1;
                unkobi.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                unkobi.UpdTime = DateTime.Now.ToString("HHmmss");
                unkobi.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                unkobi.UpdPrgId = "KJ3300";

                return unkobi;
            }

            private List<TkdMishum> CopyDataMishum(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var mishumList = new List<TkdMishum>();

                foreach (var mishumToCopy in multiCopyData.MishumCopyDataList)
                {
                    var mishum = new TkdMishum();
                    mishum.SimpleCloneProperties(mishumToCopy);

                    mishum.UkeNo = newUkeNo;
                    mishum.HenKai = 0;
                    mishum.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    mishum.UpdTime = DateTime.Now.ToString("HHmmss");
                    mishum.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    mishum.UpdPrgId = "KJ3300";

                    mishumList.Add(mishum);
                }

                return mishumList;
            }

            private List<TkdYykSyu> CopyDataYykSyu(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var yykSyuList = new List<TkdYykSyu>();
                foreach (var yyksyuToCopy in multiCopyData.YykSyuCopyDataList)
                {
                    var yyksyu = new TkdYykSyu();
                    yyksyu.SimpleCloneProperties(yyksyuToCopy);

                    yyksyu.UkeNo = newUkeNo;
                    yyksyu.HenKai = 0;
                    yyksyu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    yyksyu.UpdTime = DateTime.Now.ToString("HHmmss");
                    yyksyu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    yyksyu.UpdPrgId = "KJ3300";

                    yykSyuList.Add(yyksyu);
                }
                return yykSyuList;
            }

            private List<TkdHaisha> CollectDataHaishaAutoAssign(BookingFormData bookingdata, string newUkeNo,
                (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) vehiclesInfo)
            {
                List<TkdHaisha> haishaList = new List<TkdHaisha> { };

                short AutoIncrementVehicleID = 0;

                var busDateTime = bookingdata.GetBusDateTime();
                var garageDateTime = bookingdata.GetBusGarageDateTime();

                List<Vehical> vehiclesAssigned = vehiclesInfo.VehiclesAssigned;
                List<Vehical> vehicles = vehiclesInfo.Vehicles;

                foreach (var vehicleTypeRow in bookingdata.VehicleGridDataList.OrderByDescending((v) => v.busTypeData.SyaSyuCd)) // Loop each row in grid of Booking Screen
                {
                    List<Vehical> masterVehicle = BookingInputHelper.GetMasterVehicals(vehicleTypeRow, vehicles);

                    int driverNumInt = int.Parse(vehicleTypeRow.DriverNum);
                    int busNumInt = int.Parse(vehicleTypeRow.BusNum);
                    int guiderNumInt = int.Parse(vehicleTypeRow.GuiderNum);

                    int OverDriverPerBus = driverNumInt % busNumInt;
                    short DriverPerBus = Convert.ToInt16(driverNumInt / busNumInt);

                    int OverGuiderPerBus = guiderNumInt % busNumInt;
                    short GuiderPerBus = Convert.ToInt16(guiderNumInt / busNumInt);
                    // Set AutoIncrement to new busNum / new row
                    for (int i = 0; i < busNumInt; i++) // Loop number of Vehicle in each row
                    {
                        AutoIncrementVehicleID++;
                        var haiSha = new TkdHaisha();
                        haiSha.UkeNo = newUkeNo;
                        haiSha.SyuKoYmd = bookingdata.ReservationTabData.GarageLeaveDate.ToString("yyyyMMdd");
                        haiSha.SyuKoTime = bookingdata.ReservationTabData.SyuKoTime.ToStringWithoutDelimiter();
                        haiSha.SyuPaTime = bookingdata.ReservationTabData.SyuPatime.ToStringWithoutDelimiter();
                        haiSha.HaiSymd = bookingdata.BusStartDate.ToString("yyyyMMdd");
                        haiSha.HaiStime = bookingdata.BusStartTime.ToStringWithoutDelimiter();
                        var kikDate = new BookingInputHelper.MyDate(bookingdata.ReservationTabData.GarageReturnDate, bookingdata.ReservationTabData.KikTime);
                        haiSha.KikYmd = kikDate.ConvertedDate.ToString("yyyyMMdd");
                        haiSha.KikTime = kikDate.ConvertedTime.ToStringWithoutDelimiter();
                        var touDate = new BookingInputHelper.MyDate(bookingdata.BusEndDate, bookingdata.BusEndTime);
                        haiSha.TouYmd = touDate.ConvertedDate.ToString("yyyyMMdd");
                        haiSha.TouChTime = touDate.ConvertedTime.ToStringWithoutDelimiter();
                        SetDefaultValueHaiSha(bookingdata, haiSha);

                        haiSha.UnkRen = 1;
                        haiSha.SyaSyuRen = short.Parse(vehicleTypeRow.RowID);
                        haiSha.TeiDanNo = AutoIncrementVehicleID;
                        haiSha.BunkRen = 1;
                        haiSha.GoSya = string.Format("{0:00}", AutoIncrementVehicleID);
                        haiSha.GoSyaJyn = AutoIncrementVehicleID;
                        haiSha.JyoSyaJin = 0;
                        haiSha.PlusJin = 0;
                        // 運転手数
                        if (i < OverDriverPerBus)
                        {
                            haiSha.DrvJin = Convert.ToInt16(DriverPerBus + 1);
                        }
                        else
                        {
                            haiSha.DrvJin = DriverPerBus;
                        }
                        // ガイド数
                        if (i < OverGuiderPerBus)
                        {
                            haiSha.GuiSu = Convert.ToInt16(GuiderPerBus + 1);
                        }
                        else
                        {
                            haiSha.GuiSu = GuiderPerBus;
                        }
                        haiSha.OthJinKbn1 = 99;
                        haiSha.OthJin1 = 0;
                        haiSha.OthJinKbn2 = 99;
                        haiSha.OthJin2 = 0;
                        haiSha.KhinKbn = 1;
                        haiSha.HaiSkbn = 1; // 配車区分 1:未仮車  2:仮車済 auto assigned bus will update this update it
                        haiSha.HaiIkbn = 1; // 配員区分 
                        haiSha.GuiWnin = 0;
                        haiSha.NippoKbn = 1;
                        haiSha.YouTblSeq = 0;
                        haiSha.YouKataKbn = 9; // 傭車型区分
                        haiSha.SyaRyoUnc = Convert.ToInt32(vehicleTypeRow.UnitPrice);
                        haiSha.SyaRyoSyo = (int)Math.Ceiling(haiSha.SyaRyoUnc * decimal.Parse(bookingdata.TaxRate) / 100);
                        haiSha.SyaRyoTes = (int)Math.Ceiling((haiSha.SyaRyoUnc * decimal.Parse(bookingdata.FeeBusRate)) / 100);

                        // Auto asign vehicle
                        Vehical vehicleWillAssign = null;
                        foreach (var vehicle in masterVehicle)
                        {
                            if (vehiclesAssigned.Select(t => t.VehicleModel.SyaRyoCdSeq).ToList().Contains(vehicle.VehicleModel.SyaRyoCdSeq)) { continue; }
                            else
                            {
                                vehiclesAssigned.Add(vehicle);
                                vehicleWillAssign = vehicle;
                                break;
                            }
                        }
                        if (vehicleWillAssign != null)
                        {
                            haiSha.HaiSsryCdSeq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                            haiSha.SyuEigCdSeq = vehicleWillAssign.EigyoCdSeq;
                            haiSha.KikEigSeq = vehicleWillAssign.EigyoCdSeq;
                            haiSha.KssyaRseq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                            haiSha.Kskbn = 2; // 1:未仮車  2:仮車済
                        }
                        else
                        {
                            haiSha.SyuEigCdSeq = 0;
                            haiSha.KikEigSeq = 0;
                            haiSha.HaiSsryCdSeq = 0;
                            haiSha.KssyaRseq = 0;
                            haiSha.Kskbn = 1; // 1:未仮車  2:仮車済
                        }
                        
                        haishaList.Add(haiSha);
                    }
                }
                return haishaList;
            }

            private List<TkdHaisha> CopyDataHaisha(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var haiShaList = new List<TkdHaisha>();
                BookingFormData bookingData = multiCopyData.BookingDataToCopy;

                double dateChange = multiCopyData.NumberOfDateChanged;

                foreach (var haiShaToCopy in multiCopyData.HaishaCopyDataList)
                {
                    var haiSha = new TkdHaisha();
                    haiSha.SimpleCloneProperties(haiShaToCopy);

                    haiSha.UkeNo = newUkeNo;
                    haiSha.SyuKoYmd = DateTime.ParseExact(haiShaToCopy.SyuKoYmd, "yyyyMMdd", null).AddDays(dateChange).ToString("yyyyMMdd");
                    haiSha.HaiSymd = DateTime.ParseExact(haiShaToCopy.HaiSymd, "yyyyMMdd", null).AddDays(dateChange).ToString("yyyyMMdd");
                    haiSha.KikYmd = DateTime.ParseExact(haiShaToCopy.KikYmd, "yyyyMMdd", null).AddDays(dateChange).ToString("yyyyMMdd");
                    haiSha.TouYmd = DateTime.ParseExact(haiShaToCopy.TouYmd, "yyyyMMdd", null).AddDays(dateChange).ToString("yyyyMMdd");
                    SetDefaultValueHaiSha(bookingData, haiSha);
                    if(multiCopyData.BookingDataToCopy.ReservationTabData.Destination!=null)
                    {
                        haiSha.IkMapCdSeq = multiCopyData.BookingDataToCopy.ReservationTabData.Destination.BasyoMapCdSeq;
                    }
                    else
                    {
                        haiSha.IkMapCdSeq =0;
                    }
                    if (multiCopyData.BookingDataToCopy.ReservationTabData != null)
                    {
                        haiSha.IkNm = multiCopyData.BookingDataToCopy.ReservationTabData.IkNm;
                    }
                    else
                    {
                        haiSha.IkNm = "";
                    }
                    if ( multiCopyData.BookingDataToCopy.ReservationTabData.DespatchingPlace != null)
                    {
                        haiSha.HaiScdSeq = multiCopyData.BookingDataToCopy.ReservationTabData.DespatchingPlace.HaiSCdSeq;
                    }
                    else
                    {
                        haiSha.HaiScdSeq = 0;
                    }
                    if (multiCopyData.BookingDataToCopy.ReservationTabData != null)
                    {
                        haiSha.HaiSnm = multiCopyData.BookingDataToCopy.ReservationTabData.HaiSNm;
                    }
                    else
                    {
                        haiSha.HaiSnm = "";
                    }
                    if (multiCopyData.BookingDataToCopy.ReservationTabData.ArrivePlace != null)
                    {
                        haiSha.TouCdSeq = multiCopyData.BookingDataToCopy.ReservationTabData.ArrivePlace.HaiSCdSeq;
                    }
                    else
                    {
                        haiSha.TouCdSeq = 0;
                    }
                    if ( multiCopyData.BookingDataToCopy.ReservationTabData != null)
                    {
                        haiSha.TouNm = multiCopyData.BookingDataToCopy.ReservationTabData.TouNm;
                    }
                    else
                    {
                        haiSha.TouNm = "";
                    }

                    haiShaList.Add(haiSha);
                }
                
                return haiShaList;
            }

            /// <summary>
            /// Set default value for haiSha model, every properties not copy from old ukeNo and not effected by CopyType is default value
            /// </summary>
            /// <param name="bookingData"></param>
            /// <param name="haiSha"></param>
            private void SetDefaultValueHaiSha(BookingFormData bookingData, TkdHaisha haiSha)
            {
                haiSha.HenKai = 0;
                haiSha.BunKsyuJyn = 0;
                haiSha.DanTaNm2 = string.Empty;
                haiSha.IkMapCdSeq = 0;
                haiSha.IkNm = string.Empty;
                // haiSha.SyuKoYmd = bookingData.ReservationTabData.GarageLeaveDate.ToString("yyyyMMdd");
                // haiSha.SyuKoTime = bookingData.ReservationTabData.SyuKoTime.ToStringWithoutDelimiter();
                // haiSha.SyuPaTime = bookingData.ReservationTabData.SyuPatime.ToStringWithoutDelimiter();
                // haiSha.HaiSymd = bookingData.BusStartDate.ToString("yyyyMMdd");
                // haiSha.HaiStime = bookingData.BusStartTime.ToStringWithoutDelimiter();
                haiSha.HaiScdSeq = 0;
                haiSha.HaiSnm = string.Empty;
                haiSha.HaiSjyus1 = string.Empty;
                haiSha.HaiSjyus2 = string.Empty;
                haiSha.HaiSkigou = string.Empty;
                haiSha.HaiSkouKcdSeq = 0;
                haiSha.HaiSbinCdSeq = 0;
                haiSha.HaiSsetTime = string.Empty;
                // var kikDate = new BookingInputHelper.MyDate(bookingData.ReservationTabData.GarageReturnDate, bookingData.ReservationTabData.KikTime);
                // haiSha.KikYmd = kikDate.ConvertedDate.ToString("yyyyMMdd");
                // haiSha.KikTime = kikDate.ConvertedTime.ToStringWithoutDelimiter();
                // var touDate = new BookingInputHelper.MyDate(bookingData.BusEndDate, bookingData.BusEndTime);
                // haiSha.TouYmd = touDate.ConvertedDate.ToString("yyyyMMdd");
                // haiSha.TouChTime = touDate.ConvertedTime.ToStringWithoutDelimiter();
                haiSha.TouCdSeq = 0;
                haiSha.TouNm = string.Empty;
                haiSha.TouJyusyo1 = string.Empty;
                haiSha.TouJyusyo2 = string.Empty;
                haiSha.TouKigou = string.Empty;
                haiSha.TouKouKcdSeq = 0;
                haiSha.TouBinCdSeq = 0;
                haiSha.TouSetTime = string.Empty;
                haiSha.YoushaUnc = 0;
                haiSha.YoushaSyo = 0;
                haiSha.YoushaTes = 0;
                haiSha.PlatNo = string.Empty;
                haiSha.UkeJyKbnCd = 99;
                haiSha.SijJoKbn1 = 99;
                haiSha.SijJoKbn2 = 99;
                haiSha.SijJoKbn3 = 99;
                haiSha.SijJoKbn4 = 99;
                haiSha.SijJoKbn5 = 99;
                haiSha.RotCdSeq = 0;
                haiSha.BikoTblSeq = 0;
                haiSha.HaiCom = string.Empty;
                haiSha.SiyoKbn = 1;
                haiSha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                haiSha.UpdTime = DateTime.Now.ToString("HHmmss");
                haiSha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                haiSha.UpdPrgId = "KJ3300";
            }

            private List<TkdBookingMaxMinFareFeeCalc> CopyDataBookingMaxMinFareFeeCals(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var bookingFareFeeList = new List<TkdBookingMaxMinFareFeeCalc>();

                foreach (var bookingFareFeeToCopy in multiCopyData.BookingFareFeeCopyDataList) // Loop each row in grid of Booking Screen
                {
                    var bookingFareFee = new TkdBookingMaxMinFareFeeCalc();
                    bookingFareFee.SimpleCloneProperties(bookingFareFeeToCopy);
                    bookingFareFee.UkeNo = newUkeNo;

                    bookingFareFee.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    bookingFareFee.UpdTime = DateTime.Now.ToString("HHmmss");
                    bookingFareFee.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    bookingFareFee.UpdPrgId = "KJ3300";

                    bookingFareFeeList.Add(bookingFareFee);
                }

                return bookingFareFeeList;
            }

            private List<TkdBookingMaxMinFareFeeCalcMeisai> CopyDataBookingMaxMinFareFeeCalsDetails(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var bookingFareFeeMesaiList = new List<TkdBookingMaxMinFareFeeCalcMeisai>();

                foreach (var bookingFareFeeMesaiToCopy in multiCopyData.BookingFareFeeMeisaiCopyDataList)
                {
                    var bookingFareFeeMesai = new TkdBookingMaxMinFareFeeCalcMeisai();
                    bookingFareFeeMesai.SimpleCloneProperties(bookingFareFeeMesaiToCopy);
                    bookingFareFeeMesai.UkeNo = newUkeNo;

                    bookingFareFeeMesai.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    bookingFareFeeMesai.UpdTime = DateTime.Now.ToString("HHmmss");
                    bookingFareFeeMesai.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    bookingFareFeeMesai.UpdPrgId = "KJ3300";

                    bookingFareFeeMesaiList.Add(bookingFareFeeMesai);
                }

                return bookingFareFeeMesaiList;
            }

            private List<TkdKotei> CopyDataKotei(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var koteiList = new List<TkdKotei>();
                foreach (var koteiToCopy in multiCopyData.KoteiList)
                {
                    if(multiCopyData.IsCopyBusRoute && koteiToCopy.TeiDanNo == 0)
                    {
                        var kotei = new TkdKotei();
                        kotei.SimpleCloneProperties(koteiToCopy);

                        kotei.UkeNo = newUkeNo;
                        kotei.HenKai = 0;
                        kotei.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        kotei.UpdTime = DateTime.Now.ToString("HHmmss");
                        kotei.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        kotei.UpdPrgId = "KJ3300";

                        koteiList.Add(kotei);
                    }
                }
                return koteiList;
            }

            private List<TkdKoteik> CopyDataKoteiK(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var koteiKList = new List<TkdKoteik>();
                foreach (var koteiKToCopy in multiCopyData.KoteiKList)
                {
                    if (multiCopyData.IsCopyBusRoute && koteiKToCopy.TeiDanNo == 0)
                    {
                        var kotei = new TkdKoteik();
                        kotei.SimpleCloneProperties(koteiKToCopy);

                        kotei.UkeNo = newUkeNo;
                        kotei.HenKai = 0;
                        kotei.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        kotei.UpdTime = DateTime.Now.ToString("HHmmss");
                        kotei.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        kotei.UpdPrgId = "KJ3300";

                        koteiKList.Add(kotei);
                    }
                }
                return koteiKList;
            }

            private List<TkdTehai> CopyDataTehai(BusBookingMultiCopyData multiCopyData, string newUkeNo)
            {
                var tehaiList = new List<TkdTehai>();
                foreach (var tehaiToCopy in multiCopyData.TehaiList)
                {
                    if (multiCopyData.IsCopyArrangement && tehaiToCopy.TeiDanNo == 0)
                    {
                        var tehai = new TkdTehai();
                        tehai.SimpleCloneProperties(tehaiToCopy);

                        tehai.UkeNo = newUkeNo;
                        tehai.HenKai = 0;
                        tehai.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        tehai.UpdTime = DateTime.Now.ToString("HHmmss");
                        tehai.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tehai.UpdPrgId = "KJ3300";

                        tehaiList.Add(tehai);
                    }
                }
                return tehaiList;
            }

            private List<TkdFutTum> CopyDataFutum(bool isCopy, string newUkeNo, List<TkdFutTum> futumList, double dateChange)
            {
                var futums = new List<TkdFutTum>();
                if (isCopy)
                {
                    futums = futumList.Select(f => 
                    {
                        f.HasYmd = DateTime.ParseExact(f.HasYmd, "yyyyMMdd", null).AddDays(dateChange).ToString("yyyyMMdd");
                        f.UkeNo = newUkeNo;
                        f.HenKai = 0;
                        f.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        f.UpdTime = DateTime.Now.ToString("HHmmss");
                        f.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        f.UpdPrgId = "KJ3300";
                        return f;
                    }).ToList();
                }
                return futums;
            }

            private List<TkdMfutTu> CopyDataMFutTu(bool isCopy, string newUkeNo, List<TkdMfutTu> mfutTuList)
            {
                var mFutTus = new List<TkdMfutTu>();
                if (isCopy)
                {
                    mFutTus = mfutTuList.Select(f =>
                    {
                        f.UkeNo = newUkeNo;
                        f.HenKai = 0;
                        f.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        f.UpdTime = DateTime.Now.ToString("HHmmss");
                        f.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        f.UpdPrgId = "KJ3300";
                        return f;
                    }).ToList();
                }
                
                return mFutTus;
            }        }
    }
}
