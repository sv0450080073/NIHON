using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FareFeeCorrection.Commands
{
    public class SaveOrUpdateVehicleAllocation : IRequest<bool>
    {
        public ReservationGrid ReservationItem;
        public class Handler : IRequestHandler<SaveOrUpdateVehicleAllocation, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(SaveOrUpdateVehicleAllocation request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request?.ReservationItem != null)
                    {
                        var lstUpdate = new List<TkdHaisha>();
                        var curentUser = new ClaimModel();

                        foreach (var item in request.ReservationItem?.ReservationChange)
                        {
                            var tkdHaisha = _context.TkdHaisha.FirstOrDefault(x => x.UkeNo == request.ReservationItem.UkeNo && x.UnkRen == item.UnkRen &&
                                                                              x.TeiDanNo == item.TeiDanNo && x.BunkRen == item.BunkRen &&
                                                                              x.UpdYmd == item.UpdYmd && x.UpdTime == item.UpdTime);

                            if (tkdHaisha != null)
                            {
                                tkdHaisha.SyaRyoUnc = item.SyaRyoUnc;
                                tkdHaisha.SyaRyoSyo = item.SyaRyoSyo;
                                tkdHaisha.SyaRyoTes = item.SyaRyoTes;
                                tkdHaisha.YoushaUnc = item.YouUnc;
                                tkdHaisha.YoushaSyo = item.YouZei;
                                tkdHaisha.YoushaTes = item.YouTes;
                                tkdHaisha.UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd);
                                tkdHaisha.UpdTime = DateTime.Now.ToString(Formats.HHmmss);
                                tkdHaisha.UpdPrgId = Common.UpdPrgId;
                                lstUpdate.Add(tkdHaisha);
                            }
                        }
                        if (lstUpdate.Any())
                            _context.UpdateRange(lstUpdate);

                        var tkdYousha = _context.TkdYousha.FirstOrDefault(x => x.UkeNo == request.ReservationItem.UkeNo && x.UnkRen == request.ReservationItem.UnkRen &&
                                                                           x.YouTblSeq == request.ReservationItem.YouTblSeq &&
                                                                           x.UpdYmd == request.ReservationItem.UpdYmd && x.UpdTime == request.ReservationItem.UpdTime);

                        var tkdYyksho = _context.TkdYyksho.FirstOrDefault(x => x.UkeNo == request.ReservationItem.UkeNo && x.UpdYmd == request.ReservationItem.UpdYmd
                                                                        && x.UpdTime == request.ReservationItem.UpdTime);

                        if (request.ReservationItem.IsExistVehicleData)
                        {
                            if (tkdYousha != null)
                            {
                                tkdYousha.SyaRyoUnc = request.ReservationItem.YouUnc;
                                tkdYousha.SyaRyoSyo = request.ReservationItem.YouZei;
                                tkdYousha.SyaRyoTes = request.ReservationItem.YouTes;
                                tkdYousha.UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd);
                                tkdYousha.UpdTime = DateTime.Now.ToString(Formats.HHmmss);
                                tkdYousha.UpdPrgId = Common.UpdPrgId;
                                _context.Update(tkdYousha);
                            }
                        }

                        if (tkdYyksho != null)
                        {
                            tkdYyksho.UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd);
                            tkdYyksho.UpdTime = DateTime.Now.ToString(Formats.HHmmss);
                            tkdYyksho.UpdPrgId = Common.UpdPrgId;
                            tkdYyksho.UpdSyainCd = curentUser.SyainCdSeq;
                            _context.Update(tkdYyksho);
                        }

                        await _context.SaveChangesAsync();

                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
