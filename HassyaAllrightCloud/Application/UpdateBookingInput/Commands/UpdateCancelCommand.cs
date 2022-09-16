using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Commands
{
    public class UpdateCancelCommand : IRequest<Unit>
    {
        private readonly string _ukeNo;
        private readonly CancelTickedData _cancelTickedData;
        private readonly (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) _vehiclesInfo;

        public UpdateCancelCommand(string ukeNo, CancelTickedData cancelTickedData,
            (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) vehiclesInfo)
        {
            _cancelTickedData = cancelTickedData ?? throw new ArgumentNullException(nameof(cancelTickedData));
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _vehiclesInfo.Vehicles = vehiclesInfo.Vehicles ?? throw new ArgumentNullException(nameof(vehiclesInfo.Vehicles));
            _vehiclesInfo.VehiclesAssigned = vehiclesInfo.VehiclesAssigned ?? throw new ArgumentNullException(nameof(vehiclesInfo.VehiclesAssigned));
        }

        public class Handler : IRequestHandler<UpdateCancelCommand, Unit>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<UpdateCancelCommand> _logger;

            public Handler(KobodbContext dbContext, ILogger<UpdateCancelCommand> logger)
            {
                _context = dbContext;
                _logger = logger;
            }

            public async Task<Unit> Handle(UpdateCancelCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    // check paid or coupon
                    var yyksho = await _context.TkdYyksho.Where(x => x.UkeNo == request._ukeNo).FirstOrDefaultAsync();
                    // check lock table
                    var lockTable = _context.TkdLockTable.SingleOrDefault(l => l.TenantCdSeq == new ClaimModel().TenantID
                                                                                && l.EigyoCdSeq == yyksho.SeiEigCdSeq);
                    var checkEdit = BookingInputHelper.CheckEditable(yyksho, lockTable);
                    if (checkEdit.Contains(BookingDisableEditState.PaidOrCoupon))
                    {
                        throw new Exception("Booking has been paid or coupon");
                    }
                    if (checkEdit.Contains(BookingDisableEditState.Locked))
                    {
                        throw new Exception("Booking has been lock");
                    }

                    SetYykshoData(ref yyksho, request._cancelTickedData);
                    _context.TkdYyksho.Update(yyksho);

                    if(request._cancelTickedData != null && request._cancelTickedData.CancelStatus && request._cancelTickedData.ReusedStatus)
                    {
                        List<TkdHaisha> haishaList = _context.TkdHaisha.Where(e => e.UkeNo == request._ukeNo).ToList();
                        if(haishaList != null)
                        {
                            ReAssignVehicel(ref haishaList, request._ukeNo, request._vehiclesInfo);
                            _context.TkdHaisha.UpdateRange(haishaList);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
                return await Task.FromResult(Unit.Value);
            }

            /// <summary>
            /// Update data cancel in yyksho
            /// </summary>
            /// <param name="yyksho"></param>
            /// <param name="cancelData"></param>
            private void SetYykshoData(ref TkdYyksho yyksho, CancelTickedData cancelData)
            {
                if (yyksho != null && cancelData != null)
                {
                    yyksho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    yyksho.UpdTime = DateTime.Now.ToString("HHmmss");
                    yyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    yyksho.UpdPrgId = "KJ1000";
                    yyksho.UkeYmd = DateTime.Now.ToString("yyyyMMdd");
                    
                    if (cancelData.CancelStatus)
                    {
                        // reused
                        if (cancelData.ReusedStatus)
                        {
                            yyksho.YoyaSyu = 1;
                            yyksho.CanFuYmd = cancelData.ReusedDate.ToString("yyyyMMdd");
                            yyksho.CanFuTanSeq = cancelData.ReusedSettingStaffData == null ? 0 : cancelData.ReusedSettingStaffData.SyainCdSeq;
                            yyksho.CanFuRiy = cancelData.ReusedReason;
                        }
                        // cancel
                        else
                        {
                            yyksho.YoyaSyu = 2;
                            yyksho.CanTanSeq = cancelData.CanceledSettingStaffData == null ? 0 : cancelData.CanceledSettingStaffData.SyainCdSeq;
                            yyksho.CanRiy = cancelData.CancelReason;
                            yyksho.CanYmd = cancelData.CancelDate.ToString("yyyyMMdd");
                            yyksho.CanRit = decimal.Parse(cancelData.CancelFeeRate);
                            yyksho.CanUnc = int.Parse(cancelData.CancelFee);
                            yyksho.CanZkbn = Convert.ToByte(cancelData.CancelTaxType.IdValue);
                            yyksho.CanSyoR = decimal.Parse(cancelData.CancelTaxRate);
                            yyksho.CanSyoG = int.Parse(cancelData.CancelTaxFee);
                        }
                    }
                    // booking
                    else
                    {
                        yyksho.YoyaSyu = 1;
                    }
                }
            }

            /// <summary>
            /// Assign lại xe đã cancel
            /// </summary>
            /// <param name="haishaList"></param>
            /// <param name="ukeNo"></param>
            private void ReAssignVehicel(ref List<TkdHaisha> haishaList, string ukeNo,
                (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) vehiclesInfo)
            {
                try
                {
                    List<Vehical> listVehicle = vehiclesInfo.Vehicles;
                    var yykSyuList = _context.TkdYykSyu.Where(e => e.UkeNo == ukeNo).Select(e => new { e.KataKbn, e.SyaSyuCdSeq, e.SyaSyuRen });

                    List<Vehical> listAsignedVehicle = vehiclesInfo.VehiclesAssigned;

                    // List<Vehical> listAssignedVehicleEditMode = GetAsignedVehical($"{haishaList[0].SyuKoYmd}{haishaList[0].SyuKoTime}",
                    //                                                     $"{haishaList[0].KikYmd}{haishaList[0].KikTime}", ukeNo);

                    foreach (var tkdhaisha in haishaList)
                    {
                        tkdhaisha.HenKai++;

                        if (tkdhaisha.HaiSkbn == 2)
                        {
                            continue;
                        }

                        var yykSyu = yykSyuList.SingleOrDefault(e => e.SyaSyuRen == tkdhaisha.SyaSyuRen);
                        
                        List<Vehical> masterVehicle = BookingInputHelper.FilterVehical(listVehicle, yykSyu.KataKbn, yykSyu.SyaSyuCdSeq);
                        Vehical vehicleCurrentAssigned = listVehicle.Find(v => v.VehicleModel.SyaRyoCdSeq == tkdhaisha.HaiSsryCdSeq);
                        Vehical vehicleWillAssign = null;
                        // điều kiện xe chưa được assign hoặc đã assign mà khi cancel bị assign mất
                        if (tkdhaisha.HaiSsryCdSeq == 0 || listAsignedVehicle.Exists(v => v.VehicleModel.SyaRyoCdSeq == vehicleCurrentAssigned.VehicleModel.SyaRyoCdSeq))
                        {
                            var availableVehicles = masterVehicle.Where(v => !listAsignedVehicle.Exists(a => a.VehicleModel.SyaRyoCdSeq == v.VehicleModel.SyaRyoCdSeq));
                            if (availableVehicles.Any())
                            {
                                vehicleWillAssign = availableVehicles.First();
                                tkdhaisha.HaiSsryCdSeq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                tkdhaisha.SyuEigCdSeq = vehicleWillAssign.EigyoCdSeq;
                                tkdhaisha.KikEigSeq = vehicleWillAssign.EigyoCdSeq;
                                tkdhaisha.KssyaRseq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                tkdhaisha.Kskbn = 2; // 1:未仮車  2:仮車済
                                listAsignedVehicle.Add(vehicleWillAssign);
                            }
                            else
                            {
                                tkdhaisha.HaiSsryCdSeq = 0;
                                tkdhaisha.SyuEigCdSeq = 0;
                                tkdhaisha.KikEigSeq = 0;
                                tkdhaisha.KssyaRseq = 0;
                                tkdhaisha.Kskbn = 1; // 1:未仮車  2:仮車済
                            }
                        }
                        else
                        {
                            // ưu tiên assign lại xe đã assign sau khi reuse
                            listAsignedVehicle.Add(vehicleCurrentAssigned);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
