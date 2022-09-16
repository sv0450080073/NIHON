using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Queries
{
    public class GetHaishaByUkenoQuery : IRequest<Dictionary<CommandMode, List<TkdHaisha>>>
    {
        public string Ukeno { get; set; }
        public BookingFormData BookingData { get; set; }
        /// <summary>
        /// Gets/Sets vehicles information for auto assign bus
        /// </summary>
        public (List<Vehical> Vehicles, List<Vehical> VehiclesAssigned) VehiclesInfo { get; set; }

        public class Handler : IRequestHandler<GetHaishaByUkenoQuery, Dictionary<CommandMode, List<TkdHaisha>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetHaishaByUkenoQuery> _logger;
            private readonly IUpdateBookingInputService _updateBookingInputService;
            public Handler(KobodbContext context, ILogger<GetHaishaByUkenoQuery> logger, IUpdateBookingInputService updateBookingInputService)
            {
                _context = context;
                _logger = logger;
                _updateBookingInputService = updateBookingInputService;
            }
            public Task<Dictionary<CommandMode, List<TkdHaisha>>> Handle(GetHaishaByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdHaisha>> result = new Dictionary<CommandMode, List<TkdHaisha>>();
                    List<TkdHaisha> haishaList = _context.TkdHaisha.Where(e => e.UkeNo == request.Ukeno && e.UnkRen == request.BookingData.UnkRen && e.SiyoKbn == 1).ToList();
                    List<TkdHaisha> addNewHaishaList = new List<TkdHaisha>();
                    List<TkdHaisha> removeHaishaList = new List<TkdHaisha>();
                    List<TkdHaisha> updateHaishaList = new List<TkdHaisha>();
                    List<TkdHaiin> haiinList = _context.TkdHaiin.Where(e => e.UkeNo == request.Ukeno && e.UnkRen == request.BookingData.UnkRen && e.SiyoKbn == 1).ToList();
                    var allbuses = _updateBookingInputService.GetHaishaInfoDatas(request.Ukeno, request.BookingData.UnkRen).Result;
                    //var listYouTblSeq = request.BookingData?.ListToRemove?.SelectMany(l => l.YouTblSeqList) ?? new List<int>();

                    var dataTkdHaisha = _context.TkdHaisha.Where(e => e.UkeNo == request.Ukeno && e.UnkRen == request.BookingData.UnkRen);
                    short AutoIncrementVehicleID = dataTkdHaisha.Any() ? dataTkdHaisha.Max(t => t.TeiDanNo) : (short)0;
                    BookingInputHelper.MyDate busStartTime = new BookingInputHelper.MyDate(request.BookingData.BusStartDate, request.BookingData.BusStartTime);
                    BookingInputHelper.MyDate busEndTime = new BookingInputHelper.MyDate(request.BookingData.BusEndDate, request.BookingData.BusEndTime);

                    List<Vehical> listAssignedVehicleEditMode = request.VehiclesInfo.VehiclesAssigned.Where(_ => _.Ukeno != request.Ukeno)
                                                                                                     .ToList();
                    //delete row case
                    var haishaGroups = haishaList.GroupBy(_ => _.SyaSyuRen).ToList();
                    haishaGroups.ForEach(row => {
                        var vehicleTypeRow = request.BookingData.VehicleGridDataList.FirstOrDefault(x => x.RowID == row.Key.ToString());
                        if (vehicleTypeRow == null)
                        {
                            haishaList.RemoveAll(_ => _.SyaSyuRen == row.Key);
                            request.BookingData.ListToRemove.AddRange(allbuses.Where(b => b.SyaSyuRen == row.Key).ToList());
                            removeHaishaList.AddRange(row);
                        }
                    });
                    var listYouTblSeq = request.BookingData?.ListToRemove?.SelectMany(l => l.YouTblSeqList) ?? new List<int>();
                    foreach (var vehicleTypeRow in request.BookingData.VehicleGridDataList.OrderByDescending((v) => v.busTypeData.SyaSyuCd))
                    {
                        List<Vehical> masterVehicle = BookingInputHelper.GetMasterVehicals(vehicleTypeRow, request.VehiclesInfo.Vehicles);

                        List<TkdHaisha> tkdhaishaListBaseOnSyaSyuRen = haishaList.Where(x => x.SyaSyuRen.ToString() == vehicleTypeRow.RowID).ToList();
                        int oldHaishasCount = tkdhaishaListBaseOnSyaSyuRen?.Count ?? 0;
                        int maxOfHaisha = Math.Max(oldHaishasCount, int.Parse(vehicleTypeRow.BusNum));

                        #region Experimental Logic
                        if (false)
                        {
#pragma warning disable CS0162 // Unreachable code detected
                            List<short> currentTeidanNos = tkdhaishaListBaseOnSyaSyuRen.Select(h => h.TeiDanNo).Distinct().ToList();
#pragma warning restore CS0162 // Unreachable code detected
                            int numOfCurrentTeidanNo = tkdhaishaListBaseOnSyaSyuRen?.Count(h => h.BunkRen == 1) ?? 0;
                            int numOfNewTeidanNo = int.Parse(vehicleTypeRow.BusNum);
                            int indexForCalGui = 0;

                            //update and delete record case
                            foreach (var teidan in currentTeidanNos.OrderBy(c => c))
                            {
                                var currentHaishas = tkdhaishaListBaseOnSyaSyuRen.Where(h => h.TeiDanNo == teidan);
                                if (request.BookingData.ListToRemove.Exists(info =>
                                            info.UkeNo == currentHaishas.First().UkeNo
                                            && info.UnkRen == currentHaishas.First().UnkRen
                                            && info.SyaSyuRen == currentHaishas.First().SyaSyuRen
                                            && info.TeidanNo == currentHaishas.First().TeiDanNo
                                            ))
                                {
                                    //delete record case
                                    removeHaishaList.AddRange(currentHaishas);
                                }
                                else
                                {
                                    //update record case
                                    var haishasOfTeidan = tkdhaishaListBaseOnSyaSyuRen.Where(h => h.TeiDanNo == teidan).OrderBy(h => h.BunkRen).ToList();
                                    var tkdhaisha = haishasOfTeidan.First(h => h.BunkRen == 1);
                                    tkdhaisha.HenKai++;
                                    tkdhaisha.GuiSu = CalculateGuisu(vehicleTypeRow, indexForCalGui);
                                    DifferentPartForSaveHaisha(request.BookingData, tkdhaisha, vehicleTypeRow, busStartTime, busEndTime);
                                    if (tkdhaisha.HaiSkbn == 2)
                                    {
                                        updateHaishaList.Add(tkdhaisha);
                                        continue;
                                    }
                                    // if (haishasOfTeidan.All(h => h.HaiSkbn == 2))
                                    // {
                                    //     updateHaishaList.AddRange(haishasOfTeidan);
                                    //     continue;
                                    // }
                                    // haishasOfTeidan.ForEach(tkdhaisha =>
                                    // {
                                    //     tkdhaisha.HenKai++;
                                    //     tkdhaisha.GuiSu = CalculateGuisu(vehicleTypeRow, indexForCalGui);
                                    //     DifferentPartForSaveHaisha(request.BookingData, tkdhaisha, vehicleTypeRow, busStartTime, busEndTime);
                                    // });
                                    // var tkdhaisha = haishasOfTeidan.First(h => h.BunkRen == 1);
                                    if (request.BookingData.IsUpdateWithAutoAssign)
                                    {

                                        Vehical vehicleCurrentAssigned = request.VehiclesInfo.Vehicles.Find(v => v.VehicleModel.SyaRyoCdSeq == tkdhaisha.HaiSsryCdSeq);
                                        var priorityBranch = vehicleTypeRow.PriorityAutoAssignBranch != null ? vehicleTypeRow.PriorityAutoAssignBranch.EigyoCdSeq : vehicleCurrentAssigned?.EigyoCdSeq;
                                        Vehical vehicleWillAssign = null;
                                        if (vehicleCurrentAssigned == null || (vehicleCurrentAssigned != null && (tkdhaisha.HaiSsryCdSeq == 0 ||
                                            listAssignedVehicleEditMode.Exists(v => v.VehicleModel.SyaRyoCdSeq == vehicleCurrentAssigned.VehicleModel.SyaRyoCdSeq) ||
                                            vehicleCurrentAssigned.KataKbn.ToString() != vehicleTypeRow.busTypeData.Katakbn ||
                                            (vehicleCurrentAssigned.VehicleModel.SyaSyuCdSeq != vehicleTypeRow.busTypeData.SyaSyuCdSeq && vehicleTypeRow.busTypeData.SyaSyuCdSeq != 0) ||
                                            vehicleCurrentAssigned.EigyoCdSeq != priorityBranch //priority auto assign for branch changed
                                            )))
                                        {
                                            var availableVehicles = masterVehicle.Where(v => !request.VehiclesInfo.VehiclesAssigned.Exists(a => a.VehicleModel.SyaRyoCdSeq == v.VehicleModel.SyaRyoCdSeq));
                                            if (availableVehicles.Any())
                                            {
                                                vehicleWillAssign = availableVehicles.First();
                                                tkdhaisha.HaiSsryCdSeq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                                tkdhaisha.SyuEigCdSeq = vehicleWillAssign.EigyoCdSeq;
                                                tkdhaisha.KikEigSeq = vehicleWillAssign.EigyoCdSeq;
                                                tkdhaisha.KssyaRseq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                                tkdhaisha.Kskbn = 2; // 1:未仮車  2:仮車済
                                                request.VehiclesInfo.VehiclesAssigned.Add(vehicleWillAssign);
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
                                            request.VehiclesInfo.VehiclesAssigned.Add(vehicleCurrentAssigned);
                                        }
                                    }
                                    tkdhaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                    tkdhaisha.UpdTime = DateTime.Now.ToString("HHmmss");
                                    tkdhaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                    tkdhaisha.UpdPrgId = "KJ1000";
                                    updateHaishaList.Add(tkdhaisha);
                                    removeHaishaList.AddRange(haishasOfTeidan.Where(h => h.BunkRen != 1));
                                }
                            }

                            //add new record case
                            if (numOfNewTeidanNo > numOfCurrentTeidanNo)
                            {
                                for (int i = 0; i < (numOfNewTeidanNo - numOfCurrentTeidanNo); i++)
                                {
                                    AutoIncrementVehicleID++;
                                    var tkdhaisha = new TkdHaisha();
                                    tkdhaisha.UkeNo = request.Ukeno;
                                    tkdhaisha.HenKai = 0;
                                    tkdhaisha.SyaSyuRen = short.Parse(vehicleTypeRow.RowID);
                                    tkdhaisha.TeiDanNo = AutoIncrementVehicleID;
                                    tkdhaisha.GoSya = String.Format("{0:00}", AutoIncrementVehicleID);
                                    tkdhaisha.GoSyaJyn = AutoIncrementVehicleID;
                                    CommonPartForSaveHaisha(tkdhaisha);
                                    DifferentPartForSaveHaisha(request.BookingData, tkdhaisha, vehicleTypeRow, busStartTime, busEndTime);

                                    // Auto asign vehicle
                                    if (request.BookingData.IsUpdateWithAutoAssign)
                                    {
                                        Vehical vehicleWillAssign = null;
                                        foreach (var vehicle in masterVehicle)
                                        {
                                            if (request.VehiclesInfo.VehiclesAssigned.Select(t => t.VehicleModel.SyaRyoCdSeq).ToList().Contains(vehicle.VehicleModel.SyaRyoCdSeq)) { continue; }
                                            else
                                            {
                                                request.VehiclesInfo.VehiclesAssigned.Add(vehicle);
                                                vehicleWillAssign = vehicle;
                                                break;
                                            }
                                        }
                                        if (vehicleWillAssign != null)
                                        {
                                            tkdhaisha.HaiSsryCdSeq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                            tkdhaisha.SyuEigCdSeq = vehicleWillAssign.EigyoCdSeq;
                                            tkdhaisha.KikEigSeq = vehicleWillAssign.EigyoCdSeq;
                                            tkdhaisha.KssyaRseq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                            tkdhaisha.Kskbn = 2; // 1:未仮車  2:仮車済
                                        }
                                    }
                                    tkdhaisha.GuiSu = CalculateGuisu(vehicleTypeRow, numOfCurrentTeidanNo + i);
                                    haishaList.Add(tkdhaisha);
                                    addNewHaishaList.Add(tkdhaisha);
                                }
                            }
                        }
                        #endregion

                        for (int h = 0; h < maxOfHaisha; h++)
                        {
                            if (h < oldHaishasCount)
                            {
                                var currentHaisha = tkdhaishaListBaseOnSyaSyuRen[h];
                                if (listYouTblSeq.Contains(currentHaisha.YouTblSeq))
                                {
                                    currentHaisha.YouTblSeq = 0;
                                }
                                if (request.BookingData.ListToRemove != null && request.BookingData.ListToRemove.Exists(info =>
                                        info.UkeNo == currentHaisha.UkeNo
                                        && info.UnkRen == currentHaisha.UnkRen
                                        && info.SyaSyuRen == currentHaisha.SyaSyuRen
                                        && info.TeidanNo == currentHaisha.TeiDanNo
                                        ))
                                {
                                    //delete record case
                                    removeHaishaList.Add(tkdhaishaListBaseOnSyaSyuRen[h]);
                                }
                                else
                                {
                                    //update record case
                                    TkdHaisha tkdhaisha = tkdhaishaListBaseOnSyaSyuRen[h];
                                    tkdhaisha.HenKai++;
                                    tkdhaisha.GuiSu = CalculateGuisu(vehicleTypeRow, h);
                                    DifferentPartForSaveHaisha(request.BookingData, tkdhaisha, vehicleTypeRow, busStartTime, busEndTime);
                                    if (tkdhaisha.HaiSkbn == 2)
                                    {
                                        updateHaishaList.Add(tkdhaisha);
                                        continue;
                                    }
                                    if (request.BookingData.IsUpdateWithAutoAssign)
                                    {
                                        Vehical vehicleCurrentAssigned = request.VehiclesInfo.Vehicles.Find(v => v.VehicleModel.SyaRyoCdSeq == tkdhaisha.HaiSsryCdSeq);
                                        var priorityBranch = vehicleTypeRow.PriorityAutoAssignBranch != null ? vehicleTypeRow.PriorityAutoAssignBranch.EigyoCdSeq : vehicleCurrentAssigned?.EigyoCdSeq;
                                        Vehical vehicleWillAssign = null;
                                        if ((vehicleCurrentAssigned == null && tkdhaisha.YouTblSeq == 0) || (vehicleCurrentAssigned != null && (tkdhaisha.HaiSsryCdSeq == 0 ||
                                            listAssignedVehicleEditMode.Exists(v => v.VehicleModel.SyaRyoCdSeq == vehicleCurrentAssigned.VehicleModel.SyaRyoCdSeq) ||
                                            vehicleCurrentAssigned.KataKbn.ToString() != vehicleTypeRow.busTypeData.Katakbn ||
                                            (vehicleCurrentAssigned.VehicleModel.SyaSyuCdSeq != vehicleTypeRow.busTypeData.SyaSyuCdSeq && vehicleTypeRow.busTypeData.SyaSyuCdSeq != 0) ||
                                            vehicleCurrentAssigned.EigyoCdSeq != priorityBranch //priority auto assign for branch changed
                                            )))
                                        {
                                            var availableVehicles = masterVehicle.Where(v => !request.VehiclesInfo.VehiclesAssigned.Exists(a => a.VehicleModel.SyaRyoCdSeq == v.VehicleModel.SyaRyoCdSeq));
                                            if (availableVehicles.Any())
                                            {
                                                vehicleWillAssign = availableVehicles.First();
                                                tkdhaisha.HaiSsryCdSeq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                                tkdhaisha.SyuEigCdSeq = vehicleWillAssign.EigyoCdSeq;
                                                tkdhaisha.KikEigSeq = vehicleWillAssign.EigyoCdSeq;
                                                tkdhaisha.KssyaRseq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                                tkdhaisha.Kskbn = 2; // 1:未仮車  2:仮車済
                                                request.VehiclesInfo.VehiclesAssigned.Add(vehicleWillAssign);
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
                                            request.VehiclesInfo.VehiclesAssigned.Add(vehicleCurrentAssigned);
                                        }
                                    }
                                    tkdhaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                    tkdhaisha.UpdTime = DateTime.Now.ToString("HHmmss");
                                    tkdhaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                    tkdhaisha.UpdPrgId = "KJ1000";
                                    updateHaishaList.Add(tkdhaisha);
                                }
                            }
                            else
                            {
                                //add new record case
                                AutoIncrementVehicleID++;
                                var tkdhaisha = new TkdHaisha();
                                tkdhaisha.UkeNo = request.Ukeno;
                                tkdhaisha.HenKai = 0;
                                tkdhaisha.SyaSyuRen = short.Parse(vehicleTypeRow.RowID);
                                tkdhaisha.TeiDanNo = AutoIncrementVehicleID;
                                tkdhaisha.GoSya = String.Format("{0:00}", AutoIncrementVehicleID);
                                tkdhaisha.GoSyaJyn = AutoIncrementVehicleID;
                                CommonPartForSaveHaisha(tkdhaisha);
                                DifferentPartForSaveHaisha(request.BookingData, tkdhaisha, vehicleTypeRow, busStartTime, busEndTime);

                                // Auto asign vehicle
                                if (request.BookingData.IsUpdateWithAutoAssign)
                                {
                                    Vehical vehicleWillAssign = null;
                                    foreach (var vehicle in masterVehicle)
                                    {
                                        if (request.VehiclesInfo.VehiclesAssigned.Select(t => t.VehicleModel.SyaRyoCdSeq).ToList().Contains(vehicle.VehicleModel.SyaRyoCdSeq)) { continue; }
                                        else
                                        {
                                            request.VehiclesInfo.VehiclesAssigned.Add(vehicle);
                                            vehicleWillAssign = vehicle;
                                            break;
                                        }
                                    }
                                    if (vehicleWillAssign != null)
                                    {
                                        tkdhaisha.HaiSsryCdSeq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                        tkdhaisha.SyuEigCdSeq = vehicleWillAssign.EigyoCdSeq;
                                        tkdhaisha.KikEigSeq = vehicleWillAssign.EigyoCdSeq;
                                        tkdhaisha.KssyaRseq = vehicleWillAssign.VehicleModel.SyaRyoCdSeq;
                                        tkdhaisha.Kskbn = 2; // 1:未仮車  2:仮車済
                                    }
                                }
                                tkdhaisha.GuiSu = CalculateGuisu(vehicleTypeRow, h);
                                haishaList.Add(tkdhaisha);
                                addNewHaishaList.Add(tkdhaisha);
                            }
                        }
                    }

                    int y = updateHaishaList.Count;
                    updateHaishaList.ToList().ForEach(c => c.JyoSyaJin = 0);
                    updateHaishaList.ToList().ForEach(c => c.PlusJin = 0);
                    updateHaishaList = updateHaishaList.OrderBy(t => t.TeiDanNo).ToList();
                    int x = addNewHaishaList.Count;
                    addNewHaishaList.ToList().ForEach(c => c.JyoSyaJin = 0);
                    addNewHaishaList.ToList().ForEach(c => c.PlusJin = 0);
                    updateHaishaList = updateHaishaList.OrderBy(t => t.TeiDanNo).ToList();
                    int rowdriver = 0;
                    for (int j = 0; j < Convert.ToInt16(request.BookingData.SupervisorTabData.JyoSyaJin); j++)
                    {

                        if (rowdriver == (x+y))
                        {
                            rowdriver = 0;                            
                            if (y!=0&&rowdriver <= y-1)
                            {
                                updateHaishaList[rowdriver].JyoSyaJin++;
                            }
                            else
                            {
                                addNewHaishaList[rowdriver-y].JyoSyaJin++;
                            }
                        }
                        else if (j > (x+y))
                        {
                            if (y!=0&&rowdriver <= y-1)
                            {
                                updateHaishaList[rowdriver].JyoSyaJin++;
                            }
                            else
                            {
                                addNewHaishaList[rowdriver - y].JyoSyaJin++;
                            }
                        }
                        else
                        {
                            if (y!=0&&j <= y-1)
                            {
                                updateHaishaList[j].JyoSyaJin++;
                            }
                            else
                            {
                                addNewHaishaList[j - y].JyoSyaJin++;
                            }
                        }
                        rowdriver++;
                    }

                    int rowguide = 0;
                    for (int j = 0; j < Convert.ToInt16(request.BookingData.SupervisorTabData.PlusJin); j++)
                    {

                        if (rowguide == (x+y))
                        {
                            rowguide = 0;
                            if(y!=0&&rowguide<=y-1)
                            {
                                 updateHaishaList[rowguide].PlusJin++;
                            }
                            else
                            {
                                addNewHaishaList[rowguide-y].PlusJin++;
                            }
                           
                        }
                        else if (j > (x+y))
                        {
                            if (y!=0&&rowguide <= y-1)
                            {
                                updateHaishaList[rowguide].PlusJin++;
                            }
                            else
                            {
                                addNewHaishaList[rowguide - y].PlusJin++;
                            }
                        }
                        else
                        {
                            if (y!=0&&j <= y-1)
                            {
                                updateHaishaList[j].PlusJin++;
                            }
                            else
                            {
                                addNewHaishaList[j - y].PlusJin++;
                            }
                        }
                        rowguide++;
                    }
                    removeHaishaList.ForEach(h => h.SiyoKbn = 2);
                    result.Add(CommandMode.Create, addNewHaishaList);
                    result.Add(CommandMode.Update, updateHaishaList);
                    result.Add(CommandMode.Delete, removeHaishaList);
                    if (request.BookingData.ReservationTabData.MovementStatus.CodeKbn == "1")
                    {
                        result = BookingInputHelper.ArrangeHaishaDaily(result);
                    }
                    ReArrangeGoSya(result);
                    CalculateHaiIkbn(result, haiinList);
                    return Task.FromResult(result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Initial state for haisha
            /// </summary>
            /// <param name="tkdhaisha"></param>
            private void CommonPartForSaveHaisha(TkdHaisha tkdhaisha)
            {
                tkdhaisha.UnkRen = 1;
                tkdhaisha.BunkRen = 1;
                tkdhaisha.HenKai = 0;
                tkdhaisha.BunKsyuJyn = 0;
                tkdhaisha.SyuEigCdSeq = 0; // auto assigned bus will update this update it
                tkdhaisha.KikEigSeq = 0; // auto assigned bus will update this update it
                tkdhaisha.HaiSsryCdSeq = 0; // auto assigned bus will update this update it
                tkdhaisha.KssyaRseq = 0; // auto assigned bus will update this update it
                tkdhaisha.DanTaNm2 = "";
                //tkdhaisha.IkMapCdSeq = 0;
                //tkdhaisha.IkNm = "";
                tkdhaisha.SyuPaTime = "";
                //tkdhaisha.HaiScdSeq = 0;
                //tkdhaisha.HaiSnm = "";
                //tkdhaisha.HaiSjyus1 = "";
                //tkdhaisha.HaiSjyus2 = "";
                tkdhaisha.HaiSkigou = "";
                tkdhaisha.HaiSkouKcdSeq = 0;
                tkdhaisha.HaiSbinCdSeq = 0;
                tkdhaisha.HaiSsetTime = "";
                //tkdhaisha.TouCdSeq = 0;
                //tkdhaisha.TouNm = "";
                //tkdhaisha.TouJyusyo1 = "";
                //tkdhaisha.TouJyusyo2 = "";
                tkdhaisha.TouKigou = "";
                tkdhaisha.TouKouKcdSeq = 0;
                tkdhaisha.TouBinCdSeq = 0;
                tkdhaisha.TouSetTime = "";
                //tkdhaisha.JyoSyaJin = 0;
                //tkdhaisha.PlusJin = 0;
                // 運転手数
                // ガイド数
                tkdhaisha.OthJinKbn1 = 99;
                tkdhaisha.OthJin1 = 0;
                tkdhaisha.OthJinKbn2 = 99;
                tkdhaisha.OthJin2 = 0;
                tkdhaisha.Kskbn = 1; // 1:未仮車  2:仮車済 auto assigned bus will update this update it
                tkdhaisha.KhinKbn = 1;
                tkdhaisha.HaiSkbn = 1; // 配車区分 1:未仮車  2:仮車済 auto assigned bus will update this update it
                tkdhaisha.HaiIkbn = 1; // 配員区分 
                tkdhaisha.GuiWnin = 0;
                tkdhaisha.NippoKbn = 1;
                tkdhaisha.YouTblSeq = 0;
                tkdhaisha.YouKataKbn = 9; // 傭車型区分
                tkdhaisha.YoushaUnc = 0;
                tkdhaisha.YoushaSyo = 0;
                tkdhaisha.YoushaTes = 0;
                tkdhaisha.PlatNo = "";
                // tkdhaisha.UkeJyKbnCd = 99;
                // tkdhaisha.SijJoKbn1 = 99;
                // tkdhaisha.SijJoKbn2 = 99;
                // tkdhaisha.SijJoKbn3 = 99;
                // tkdhaisha.SijJoKbn4 = 99;
                // tkdhaisha.SijJoKbn5 = 99;
                tkdhaisha.RotCdSeq = 0;
                tkdhaisha.BikoTblSeq = 0;
                tkdhaisha.HaiCom = "";
                tkdhaisha.SiyoKbn = 1;
                tkdhaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                tkdhaisha.UpdTime = DateTime.Now.ToString("HHmmss");
                tkdhaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                tkdhaisha.UpdPrgId = "KJ1000";
            }

            /// <summary>
            /// Set data from bookingdata to haisha
            /// </summary>
            /// <param name="bookingdata"></param>
            /// <param name="tkdhaisha"></param>
            /// <param name="vehicleTypeRow"></param>
            /// <param name="busStartTime"></param>
            /// <param name="busEndTime"></param>
            private void DifferentPartForSaveHaisha(BookingFormData bookingdata, TkdHaisha tkdhaisha, VehicleGridData vehicleTypeRow, BookingInputHelper.MyDate busStartTime, BookingInputHelper.MyDate busEndTime)
            {
                decimal taxRate = (decimal)Math.Round(decimal.Parse(bookingdata.TaxRate) / 100, 3);
                var busTaxPrice = Convert.ToInt32(vehicleTypeRow.UnitPrice) * taxRate / (1 + taxRate);
                int minusBusTax = bookingdata.TaxTypeforBus.IdValue == Constants.InTax.IdValue
                    ? (int)BookingInputHelper.RoundHelper[bookingdata.HasuSet.TaxSetting].Invoke(busTaxPrice)
                    : 0;
                var garageDateTime = bookingdata.GetBusGarageDateTime();
                BookingInputHelper.MyDate garageReturnDateTime = new BookingInputHelper.MyDate(bookingdata.BusEndDate, bookingdata.ReservationTabData.KikTime);
                tkdhaisha.SyuKoYmd = garageDateTime.Leave.ConvertedDate.AddMinutes(-Common.BusCheckBeforeOrAfterRunningDuration).ToString("yyyyMMdd");
                tkdhaisha.SyuKoTime = garageDateTime.Leave.ConvertedTime.ToStringWithoutDelimiter(); // 出庫時間
                
                tkdhaisha.SyuKoTime = bookingdata.ReservationTabData.SyuKoTime.ToStringWithoutDelimiter(); // 出庫時間
                tkdhaisha.SyuPaTime = bookingdata.ReservationTabData.SyuPatime.ToStringWithoutDelimiter();
                tkdhaisha.HaiSymd = busStartTime.inpDate.ToString("yyyyMMdd");
                tkdhaisha.HaiStime = busStartTime.inpTime.ToStringWithoutDelimiter();
                tkdhaisha.KikYmd = garageDateTime.Return.ConvertedDate.ToString("yyyyMMdd");
                tkdhaisha.KikTime = garageDateTime.Return.ConvertedTime.ToStringWithoutDelimiter(); // 帰庫時間
                tkdhaisha.TouYmd = busEndTime.ConvertedDate.ToString("yyyyMMdd");
                tkdhaisha.TouChTime = busEndTime.ConvertedTime.ToStringWithoutDelimiter();
                tkdhaisha.DrvJin = Convert.ToInt16(int.Parse(vehicleTypeRow.DriverNum) / int.Parse(vehicleTypeRow.BusNum));
                //tkdhaisha.GuiSu = Convert.ToInt16(int.Parse(vehicleTypeRow.GuiderNum) / int.Parse(vehicleTypeRow.BusNum));
                tkdhaisha.SyaRyoUnc = Convert.ToInt32(vehicleTypeRow.UnitPrice);
                // #7979 start
                int taxbus = 0;
                if (bookingdata.TaxTypeforBus.IdValue == Constants.NoTax.IdValue)
                {
                    taxbus = 0;
                }
                else
                {
                    if (bookingdata.TaxTypeforBus.IdValue == Constants.ForeignTax.IdValue)
                    {
                        taxbus = (int)BookingInputHelper.RoundHelper[bookingdata.HasuSet.TaxSetting](tkdhaisha.SyaRyoUnc * taxRate);
                    }
                    else // InTax
                    {
                        taxbus = (int)BookingInputHelper.RoundHelper[bookingdata.HasuSet.TaxSetting]((tkdhaisha.SyaRyoUnc * taxRate) / (1 + taxRate));
                    }
                }

                tkdhaisha.SyaRyoSyo = taxbus;
                int feebus = 0;
                var feeRate = (decimal)Math.Round(decimal.Parse(bookingdata.FeeBusRate) / 100, 3);
                if (bookingdata.TaxTypeforBus.IdValue == Constants.ForeignTax.IdValue)
                {
                    feebus = (int)BookingInputHelper.RoundHelper[bookingdata.HasuSet.FeeSetting]((tkdhaisha.SyaRyoUnc + taxbus) * feeRate);
                }
                else
                {
                    feebus = (int)BookingInputHelper.RoundHelper[bookingdata.HasuSet.FeeSetting](tkdhaisha.SyaRyoUnc * feeRate);
                }
                tkdhaisha.SyaRyoTes = feebus;
                // #7979 end
                tkdhaisha.SyaRyoUnc = Convert.ToInt32(vehicleTypeRow.UnitPrice) - minusBusTax;

                tkdhaisha.IkMapCdSeq = bookingdata.ReservationTabData.Destination == null ? 0 : bookingdata.ReservationTabData.Destination.BasyoMapCdSeq;
                tkdhaisha.IkNm = bookingdata.ReservationTabData.IkNm ?? string.Empty;
                tkdhaisha.HaiScdSeq = bookingdata.ReservationTabData.DespatchingPlace == null ? 0 : bookingdata.ReservationTabData.DespatchingPlace.HaiSCdSeq;
                tkdhaisha.HaiSnm = bookingdata.ReservationTabData.HaiSNm ?? string.Empty;
                tkdhaisha.HaiSjyus1 = bookingdata.ReservationTabData.HaiSjyus1 ?? string.Empty;
                tkdhaisha.HaiSjyus2 = bookingdata.ReservationTabData.HaiSjyus2 ?? string.Empty;
                tkdhaisha.TouCdSeq = bookingdata.ReservationTabData.ArrivePlace == null ? 0 : bookingdata.ReservationTabData.ArrivePlace.HaiSCdSeq;
                tkdhaisha.TouNm = bookingdata.ReservationTabData.TouNm ?? string.Empty;
                tkdhaisha.TouJyusyo1 = bookingdata.ReservationTabData.TouJyusyo1 ?? string.Empty;
                tkdhaisha.TouJyusyo2 = bookingdata.ReservationTabData.TouJyusyo2 ?? string.Empty;
                tkdhaisha.JyoSyaJin = Convert.ToInt16(bookingdata.SupervisorTabData.JyoSyaJin); ;
                tkdhaisha.PlusJin = Convert.ToInt16(bookingdata.SupervisorTabData.PlusJin);
                tkdhaisha.UkeJyKbnCd = Convert.ToByte(bookingdata.ReservationTabData.AcceptanceConditions.CodeKbn);
                tkdhaisha.SijJoKbn1 = Convert.ToByte(bookingdata.ReservationTabData.RainyMeasure.CodeKbn);
                tkdhaisha.SijJoKbn2 = Convert.ToByte(bookingdata.ReservationTabData.PaymentMethod.CodeKbn);
                tkdhaisha.SijJoKbn3 = Convert.ToByte(bookingdata.ReservationTabData.MovementForm.CodeKbn);
                tkdhaisha.SijJoKbn4 = Convert.ToByte(bookingdata.ReservationTabData.GuiderSetting.CodeKbn);
                tkdhaisha.SijJoKbn5 = Convert.ToByte(bookingdata.ReservationTabData.EstimateSetting.CodeKbn);
            }


            private short CalculateGuisu(VehicleGridData rowData, int index)
            {
                int busNumInt = int.Parse(rowData.BusNum);
                int guiderNumInt = int.Parse(rowData.GuiderNum);
                int OverGuiderPerBus = guiderNumInt % busNumInt;
                short GuiderPerBus = Convert.ToInt16(guiderNumInt / busNumInt);
                short result;
                if (index < OverGuiderPerBus)
                {
                    result = Convert.ToInt16(GuiderPerBus + 1);
                }
                else
                {
                    result = GuiderPerBus;
                }
                return result;
            }

            private void ReArrangeGoSya(Dictionary<CommandMode, List<TkdHaisha>> haishas)
            {
                var currentHaisha = haishas[CommandMode.Create].Union(haishas[CommandMode.Update]).OrderBy(t => t.TeiDanNo).ToList();
                for (int i = 0; i < currentHaisha.Count; i++)
                {
                    currentHaisha[i].GoSya = String.Format("{0:00}", i + 1);
                }
            }

            /// <summary>
            /// Check HaiInkbn #8231
            /// </summary>
            /// <param name="haishas"></param>
            /// <param name="haiins"></param>
            private void CalculateHaiIkbn(Dictionary<CommandMode, List<TkdHaisha>> haishas, List<TkdHaiin> haiins)
            {
                var currentHaisha = haishas[CommandMode.Create].Union(haishas[CommandMode.Update]).OrderBy(t => t.TeiDanNo).ToList();
                for (int i = 0; i < currentHaisha.Count; i++)
                {
                    var currentHaiins = haiins.Where(h => h.TeiDanNo == currentHaisha[i].TeiDanNo && h.BunkRen == currentHaisha[i].BunkRen).ToList() ?? new List<TkdHaiin>();
                    if (currentHaisha[i].DrvJin <= currentHaiins.Count)
                    {
                        currentHaisha[i].HaiIkbn = 2;
                    }
                    else if (currentHaiins.Count == 0)
                    {
                        currentHaisha[i].HaiIkbn = 1;
                    }
                    else
                    {
                        currentHaisha[i].HaiIkbn = 3;
                    }
                }
            }
        }
    }
}
