using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HassyaAllrightCloud.Application.VehicleDailyReport.Commands
{
    public class VehicleDailyReportSaveCommand : IRequest<bool>
    {
        public VehicleDailyReportData dto { get; set; }
        public class Handler : IRequestHandler<VehicleDailyReportSaveCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(VehicleDailyReportSaveCommand request, CancellationToken cancellationToken)
            {
                const string updPrgId = "KU1900";
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var currentDateTime = DateTime.Now;
                        var model = _context.TkdShabni.FirstOrDefault(_ => _.UkeNo == request.dto.UkeNo
                                                                        && _.UnkRen == request.dto.UnkRen
                                                                        && _.TeiDanNo == request.dto.TeiDanNo
                                                                        && _.BunkRen == request.dto.BunkRen
                                                                        && _.UnkYmd == request.dto.UnkYmd
                                                                     );

                        if (model == null && !request.dto.isUpdate)
                        {
                            model = new TkdShabni();
                            model.UkeNo = request.dto.UkeNo;
                            model.UnkRen = request.dto.UnkRen;
                            model.TeiDanNo = request.dto.TeiDanNo;
                            model.BunkRen = request.dto.BunkRen;
                            model.UnkYmd = request.dto.UnkYmd;
                        }
                        MapObject(model, request.dto);
                        EntityState state = EntityState.Unchanged;
                        if (request.dto.isUpdate)
                        {
                            state = _context.Entry(model).State;
                            if(state != EntityState.Unchanged)
                            {
                                model.UpdYmd = currentDateTime.ToString(CommonConstants.FormatYMD);
                                model.UpdTime = currentDateTime.ToString(CommonConstants.FormatHMS);
                                model.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                                model.UpdPrgId = updPrgId;
                            }
                        }
                        else
                        {
                            model.UpdYmd = currentDateTime.ToString(CommonConstants.FormatYMD);
                            model.UpdTime = currentDateTime.ToString(CommonConstants.FormatHMS);
                            model.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            model.UpdPrgId = updPrgId;
                            _context.TkdShabni.Add(model);
                        }
                        await _context.SaveChangesAsync();

                        if (!request.dto.isUpdate || state != EntityState.Unchanged)
                        {
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
                            
                            if (haisha != null)
                            {
                                if (countShabni == request.dto.totalDays)
                                {
                                    haisha.NippoKbn = 2;
                                }
                                else if(countShabni == 0)
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
                        }
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                        
            }

            private static void MapObject(TkdShabni model, VehicleDailyReportData dto)
            {
                model.HenKai = (short)(model.HenKai + 1);
                model.StMeter = decimal.Parse(dto.StMeter);
                model.EndMeter = decimal.Parse(dto.EndMeter);
                model.JisaIpkm = decimal.Parse(dto.JisaIPKm);
                model.JisaKskm = decimal.Parse(dto.JisaKSKm);
                model.KisoIpkm = decimal.Parse(dto.KisoIPKm);
                model.KisoKokm = decimal.Parse(dto.KisoKOKm);
                model.OthKm = decimal.Parse(dto.OthKm);
                model.Nenryo1 = decimal.Parse(dto.Nenryo1);
                model.Nenryo2 = decimal.Parse(dto.Nenryo2);
                model.Nenryo3 = decimal.Parse(dto.Nenryo3);
                model.JyoSyaJin = short.Parse(dto.JyoSyaJin);
                model.PlusJin = short.Parse(dto.PlusJin);
                model.SyuKoTime = dto.SyuKoTime.Replace(":", "");
                model.SyuPaTime = dto.SyuPaTime.Replace(":", "");
                model.KikTime = dto.KikTime.Replace(":", "");
                model.TouChTime = dto.TouChTime.Replace(":", "");
                model.KoskuTime = dto.KoskuTime.Replace(":", "");
                model.JisTime = dto.JisTime.Replace(":", "");
                model.UnkKai = byte.Parse(dto.UnkKai);
                model.InspectionTime = dto.InspectionTime.Replace(":", "");
                model.FeeMaxAmount = dto.FeeMaxAmount;
                model.FeeMinAmount = dto.FeeMinAmount;
                model.FareMaxAmount = dto.FareMaxAmount;
                model.FareMinAmount = dto.FareMinAmount;
                model.FareFeeMaxAmount = dto.FareFeeMaxAmount;
                model.FareFeeMinAmount = dto.FareFeeMinAmount;

                if (!dto.isUpdate)
                {
                    model.NenryoDai = 0;
                    model.ZeiKbn = 0;
                    model.Zeiritsu = 0.0M;
                    model.SyaRyoSyo = 0;
                    model.SiyoKbn = 1;
                    model.UnkYmd = DateTime.ParseExact(dto.UnkYmd, Formats.SlashyyyyMMdd, CultureInfo.InvariantCulture).ToString(CommonConstants.FormatYMD);
                    model.NenryoCd1Seq = dto.NenryoCd1Seq;
                    model.NenryoCd2Seq = dto.NenryoCd2Seq;
                    model.NenryoCd3Seq = dto.NenryoCd3Seq;
                }
            }
        }
    }
}
