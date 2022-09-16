using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Koteik.Commands
{
    public class InsertTkdKoteikCommand : IRequest<IActionResult>
    {
        public TkdKoteik koteik { get; set; }
        public List<TkdKotei> listKotei { get; set; }

        public class Handler : IRequestHandler<InsertTkdKoteikCommand, IActionResult>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(InsertTkdKoteikCommand command, CancellationToken cancellationToken)
            {
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // check paid or coupon
                        var yyksho = await _context.TkdYyksho.Where(x => x.UkeNo == command.koteik.UkeNo).FirstOrDefaultAsync();
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

                        if (_context.TkdKoteik.Find(command.koteik.UkeNo, command.koteik.UnkRen, command.koteik.TeiDanNo, command.koteik.BunkRen, command.koteik.TomKbn, command.koteik.Nittei) == null)
                        {
                            await _context.TkdKoteik.AddAsync(command.koteik);
                        }
                        else
                        {
                            var updatekoteik = _context.TkdKoteik.Find(command.koteik.UkeNo, command.koteik.UnkRen, command.koteik.TeiDanNo, command.koteik.BunkRen, command.koteik.TomKbn, command.koteik.Nittei);
                            updatekoteik.SyuPaTime = command.koteik.SyuPaTime;
                            updatekoteik.TouChTime = command.koteik.TouChTime;
                            updatekoteik.SyukoTime = command.koteik.SyukoTime;
                            updatekoteik.HaiStime = command.koteik.HaiStime;
                            updatekoteik.KikTime = command.koteik.KikTime;
                            updatekoteik.JisaIpkm = command.koteik.JisaIpkm;
                            updatekoteik.JisaKskm = command.koteik.JisaKskm;
                            updatekoteik.KisoIpkm = command.koteik.KisoIpkm;
                            updatekoteik.KisoKokm = command.koteik.KisoKokm;
                            updatekoteik.UpdYmd = command.koteik.UpdYmd;
                            updatekoteik.UpdTime = command.koteik.UpdTime;
                            updatekoteik.UpdSyainCd = command.koteik.UpdSyainCd;
                            updatekoteik.UpdPrgId = command.koteik.UpdPrgId;
                            updatekoteik.HenKai++;
                        }

                        await _context.SaveChangesAsync();
                        foreach (TkdKotei item in command.listKotei)
                        {
                            item.UkeNo = command.koteik.UkeNo;
                            item.UnkRen = command.koteik.UnkRen;
                            item.TeiDanNo = command.koteik.TeiDanNo;
                            item.BunkRen = command.koteik.BunkRen;
                            item.TomKbn = command.koteik.TomKbn;
                            item.Nittei = command.koteik.Nittei;
                            //new 
                            item.TeiDanNittei = command.koteik.TeiDanNittei;
                            item.TeiDanTomKbn = command.koteik.TeiDanTomKbn;
                            if (_context.TkdKotei.Find(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen, item.TomKbn, item.Nittei, item.KouRen) == null)
                            {
                                await _context.TkdKotei.AddAsync(item);
                            }
                            else
                            {
                                var updatekotei = _context.TkdKotei.Find(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen, item.TomKbn, item.Nittei, item.KouRen);
                                updatekotei.Koutei = item.Koutei;
                                updatekotei.HenKai++;
                                updatekotei.SiyoKbn = 1;
                                updatekotei.UpdYmd = item.UpdYmd;
                                updatekotei.UpdTime = item.UpdTime;
                                updatekotei.UpdSyainCd = item.UpdSyainCd;
                                updatekotei.UpdPrgId = item.UpdPrgId;
                            }
                            if (item == command.listKotei.Last())
                            {
                                var getdisable = _context.TkdKotei.Where(t => t.UkeNo == item.UkeNo && t.UnkRen == item.UnkRen && t.TeiDanNo == item.TeiDanNo && t.BunkRen == item.BunkRen && t.TomKbn == item.TomKbn && t.Nittei == item.Nittei && t.KouRen > item.KouRen && t.SiyoKbn == 1).ToList();
                                foreach (var items in getdisable)
                                {
                                    var updatekotei = _context.TkdKotei.Find(items.UkeNo, items.UnkRen, items.TeiDanNo, items.BunkRen, items.TomKbn, items.Nittei, items.KouRen);
                                    updatekotei.SiyoKbn = 2;
                                    updatekotei.HenKai++;
                                    updatekotei.UpdYmd = item.UpdYmd;
                                    updatekotei.UpdTime = item.UpdTime;
                                    updatekotei.UpdSyainCd = item.UpdSyainCd;
                                    updatekotei.UpdPrgId = item.UpdPrgId;
                                }
                            }

                        }
                        await _context.SaveChangesAsync();
                        dbTran.Commit();

                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs  
                        dbTran.Rollback();
                        return new BadRequestResult();
                    }
                }
                return new ContentResult { Content = "OK" };
            }
        }
    }
}
