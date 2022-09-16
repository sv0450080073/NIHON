using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Commands
{
    public class VehicleDailyReportDeleteCommand : IRequest<bool>
    {
        public VehicleDailyReportData dto { get; set; }
        public class Handler : IRequestHandler<VehicleDailyReportDeleteCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(VehicleDailyReportDeleteCommand request, CancellationToken cancellationToken)
            {
                const string updPrgId = "KU1900";
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var currentDateTime = DateTime.Now;
                        var model = await _context.TkdShabni.FirstOrDefaultAsync(_ => _.UkeNo == request.dto.UkeNo
                                                                                   && _.UnkRen == request.dto.UnkRen
                                                                                   && _.TeiDanNo == request.dto.TeiDanNo
                                                                                   && _.BunkRen == request.dto.BunkRen
                                                                                   && _.UnkYmd == request.dto.UnkYmd
                                                                                );

                        if (model != null)
                        {
                            MapObject(model, request.dto, currentDateTime, updPrgId);
                        }
                        await _context.SaveChangesAsync();

                        var countShabni = _context.TkdShabni.Count(_ => _.UkeNo == request.dto.UkeNo
                                                                     && _.UnkRen == request.dto.UnkRen
                                                                     && _.TeiDanNo == request.dto.TeiDanNo
                                                                     && _.BunkRen == request.dto.BunkRen
                                                                     && (_.StMeter != 0.00M || _.EndMeter != 0.00M
                                                                      || _.SyuKoTime != CommonConstants.DefaultTime || _.KikTime != CommonConstants.DefaultTime || _.KoskuTime != CommonConstants.DefaultTime
                                                                      || _.SyuPaTime != CommonConstants.DefaultTime || _.TouChTime != CommonConstants.DefaultTime || _.JisTime != CommonConstants.DefaultTime
                                                                      || _.InspectionTime != "0200" || _.JisaIpkm != 0.00M || _.JisaKskm != 0.00M
                                                                      || _.KisoIpkm != 0.00M || _.KisoKokm != 0.00M || _.OthKm != 0.00M
                                                                      || _.Nenryo1 != 0.00M || _.Nenryo2 != 0.00M || _.Nenryo3 != 0.00M
                                                                      || _.JyoSyaJin != 0 || _.PlusJin != 0 || _.UnkKai != 1)
                                                                  );

                        var haisha = _context.TkdHaisha.FirstOrDefault(_ => _.UkeNo == request.dto.UkeNo
                                                                             && _.UnkRen == request.dto.UnkRen
                                                                             && _.TeiDanNo == request.dto.TeiDanNo
                                                                             && _.BunkRen == request.dto.BunkRen);

                        if(haisha != null)
                        {
                            if (countShabni == request.dto.totalDays)
                            {
                                haisha.NippoKbn = 2;
                            }
                            else if (countShabni == 0)
                            {
                                haisha.NippoKbn = 1;
                            }
                            else
                            {
                                haisha.NippoKbn = 3;
                            }

                            if (_context.Entry(haisha).State != EntityState.Unchanged)
                            {
                                haisha.UpdYmd = currentDateTime.ToString(CommonConstants.FormatYMD);
                                haisha.UpdTime = currentDateTime.ToString(CommonConstants.FormatHMS);
                                haisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                haisha.UpdPrgId = updPrgId;
                                _context.SaveChanges();
                            }

                            var yyksho = await _context.TkdYyksho.FirstOrDefaultAsync(_ => _.UkeNo == request.dto.UkeNo);
                            if (yyksho != null)
                            {
                                if (_context.TkdHaisha.Where(_ => _.UkeNo == request.dto.UkeNo).All(_ => _.NippoKbn == 1))
                                {
                                    yyksho.NippoKbn = 1;
                                }
                                else if (_context.TkdHaisha.Where(_ => _.UkeNo == request.dto.UkeNo).All(_ => _.NippoKbn == 2))
                                {
                                    yyksho.NippoKbn = 2;
                                }
                                else
                                {
                                    yyksho.NippoKbn = 3;
                                }

                                if (_context.Entry(yyksho).State != EntityState.Unchanged)
                                {
                                    yyksho.UpdYmd = currentDateTime.ToString(CommonConstants.FormatYMD);
                                    yyksho.UpdTime = currentDateTime.ToString(CommonConstants.FormatHMS);
                                    yyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                    yyksho.UpdPrgId = updPrgId;
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            private static void MapObject(TkdShabni model, VehicleDailyReportData dto, DateTime currentDateTime, string updPrgId)
            {
                model.StMeter = 0.00M;
                model.EndMeter = 0.00M;
                model.JisaIpkm = 0.00M;
                model.JisaKskm = 0.00M;
                model.KisoIpkm = 0.00M;
                model.KisoKokm = 0.00M;
                model.OthKm = 0.00M;
                model.Nenryo1 = 0.00M;
                model.Nenryo2 = 0.00M;
                model.Nenryo3 = 0.00M;
                model.JyoSyaJin = 0;
                model.PlusJin = 0;
                model.SyuKoTime = CommonConstants.DefaultTime;
                model.SyuPaTime = CommonConstants.DefaultTime;
                model.KikTime = CommonConstants.DefaultTime;
                model.TouChTime = CommonConstants.DefaultTime;
                model.KoskuTime = CommonConstants.DefaultTime;
                model.JisTime = CommonConstants.DefaultTime;
                model.UnkKai = 1;
                model.InspectionTime = "0200";
                model.FeeMaxAmount = dto.FeeMaxAmount;
                model.FeeMinAmount = dto.FeeMinAmount;
                model.FareMaxAmount = dto.FareMaxAmount;
                model.FareMinAmount = dto.FareMinAmount;
                model.FareFeeMaxAmount = dto.FareFeeMaxAmount;
                model.FareFeeMinAmount = dto.FareFeeMinAmount;

                model.UpdYmd = currentDateTime.ToString(CommonConstants.FormatYMD);
                model.UpdTime = currentDateTime.ToString(CommonConstants.FormatHMS);
                model.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                model.UpdPrgId = updPrgId;
            }
        }
    }
}
