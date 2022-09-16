using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BookingInputData;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Commands
{
    public class UpdateBookingInputCommand: IRequest<IActionResult>
    {
        public string Ukeno { get; set; }
        public TkdYyksho Yyksho { get; set; }
        public TkdUnkobi Unkobi { get; set; }
        public Dictionary<CommandMode, List<TkdMishum>> Mishums { get; set; }
        public Dictionary<CommandMode, List<TkdYykSyu>> YykSyus { get; set; }
        public Dictionary<CommandMode, List<TkdHaisha>> Haishas { get; set; }
        public Dictionary<CommandMode, List<TkdKariei>> Karieis { get; set; }
        public Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalc>> MaxMinFareFeeCalcs { get; set; }
        public Dictionary<CommandMode, List<TkdBookingMaxMinFareFeeCalcMeisai>> MaxMinFareFeeCalcMeisais { get; set; }
        public BookingFormData BookingData { get; set; }
        public BookingSoftRemoveEntitiesData SoftRemoveEntitiesData  { get; set; }

        public class Handler : IRequestHandler<UpdateBookingInputCommand, IActionResult>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<UpdateBookingInputCommand> _logger;
            public Handler(KobodbContext context, ILogger<UpdateBookingInputCommand> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<IActionResult> Handle(UpdateBookingInputCommand command, CancellationToken cancellationToken)
            {
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // check paid or coupon
                        var yyksho = await _context.TkdYyksho.AsNoTracking().Where(x => x.UkeNo == command.Ukeno).FirstOrDefaultAsync();
                        // check lock table
                        var lockTable = _context.TkdLockTable.AsNoTracking().SingleOrDefault(l => l.TenantCdSeq == new ClaimModel().TenantID
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

                        // check Kskbn #7978
                        var createHaishas = command.Haishas[CommandMode.Create];
                        var updateHaishas = command.Haishas[CommandMode.Update];
                        var totalHaishas = createHaishas.Count + updateHaishas.Count;
                        var assignedHaishas = createHaishas.Where(s => s.HaiSsryCdSeq > 0).Count() + updateHaishas.Where(s => s.HaiSsryCdSeq > 0).Count();
                        if (command.BookingData.IsUpdateWithAutoAssign)
                        {
                            if (totalHaishas == assignedHaishas)
                            {
                                command.Yyksho.Kskbn = 2;
                                command.Unkobi.Kskbn = 2;
                            }
                            else if (assignedHaishas == 0)
                            {
                                command.Yyksho.Kskbn = 1;
                                command.Unkobi.Kskbn = 1;
                            }
                            else
                            {
                                command.Yyksho.Kskbn = 3;
                                command.Unkobi.Kskbn = 3;
                            }
                        }
                        
                        // check HaiSKbn #8231
                        var haiskbnHaishas = createHaishas.Where(s => s.HaiSkbn == 2).Count() + updateHaishas.Where(s => s.HaiSkbn == 2).Count();
                        if (command.BookingData.IsUpdateWithAutoAssign)
                        {
                            if (totalHaishas == haiskbnHaishas)
                            {
                                command.Yyksho.HaiSkbn = 2;
                                command.Unkobi.HaiSkbn = 2;
                            }
                            else if (haiskbnHaishas == 0)
                            {
                                command.Yyksho.HaiSkbn = 1;
                                command.Unkobi.HaiSkbn = 1;
                            }
                            else
                            {
                                command.Yyksho.HaiSkbn = 3;
                                command.Unkobi.HaiSkbn = 3;
                            }
                        }

                        // check HaiIkbn #8231
                        var haiIkbnHaishas = createHaishas.Where(s => s.HaiIkbn == 2).Count() + updateHaishas.Where(s => s.HaiIkbn == 2).Count();
                        if (command.BookingData.IsUpdateWithAutoAssign)
                        {
                            if (totalHaishas == haiIkbnHaishas)
                            {
                                command.Yyksho.HaiSkbn = 2;
                                command.Unkobi.HaiSkbn = 2;
                            }
                            else if (haiIkbnHaishas == 0)
                            {
                                command.Yyksho.HaiSkbn = 1;
                                command.Unkobi.HaiSkbn = 1;
                            }
                            else
                            {
                                command.Yyksho.HaiSkbn = 3;
                                command.Unkobi.HaiSkbn = 3;
                            }
                        }

                        if (command.BookingData.BookingStatus.CodeKbn == Constants.BookingStatus)
                        {
                            command.Yyksho.YoyaSyu = 1;
                        }

                        if (command.BookingData.BookingStatus.CodeKbn == Constants.EstimateStatus)
                        {
                            command.Yyksho.YoyaSyu = 3;
                        }
                        _context.TkdYyksho.Update(command.Yyksho);
                        _context.TkdUnkobi.Update(command.Unkobi);

                        //Yyksyus
                        if (command.YykSyus[CommandMode.Delete].Count > 0)
                        {
                            _context.TkdYykSyu.UpdateRange(command.YykSyus[CommandMode.Delete]);
                        }
                        if (command.YykSyus[CommandMode.Create].Count > 0)
                        {
                            _context.TkdYykSyu.AddRange(command.YykSyus[CommandMode.Create]);
                        }
                        if (command.YykSyus[CommandMode.Update].Count > 0)
                        {
                            _context.TkdYykSyu.UpdateRange(command.YykSyus[CommandMode.Update]);
                        }

                        if (command.BookingData.BookingStatus.CodeKbn == Constants.BookingStatus)
                        {
                            //Mishums
                            if (command.Mishums[CommandMode.Delete].Count > 0)
                            {
                                _context.TkdMishum.RemoveRange(command.Mishums[CommandMode.Delete]);
                            }
                            if (command.Mishums[CommandMode.Create].Count > 0)
                            {
                                _context.TkdMishum.AddRange(command.Mishums[CommandMode.Create]);
                            }
                            if (command.Mishums[CommandMode.Update].Count > 0)
                            {
                                _context.TkdMishum.UpdateRange(command.Mishums[CommandMode.Update]);
                            }

                            //Haishas
                            AssignPriceTkdHaisha(command.Haishas, command.Mishums, command.BookingData);
                            if (command.Haishas[CommandMode.Delete].Count > 0)
                            {
                                _context.TkdHaisha.UpdateRange(command.Haishas[CommandMode.Delete]);
                            }
                            if (command.Haishas[CommandMode.Create].Count > 0)
                            {
                                _context.TkdHaisha.AddRange(command.Haishas[CommandMode.Create]);
                            }
                            if (command.Haishas[CommandMode.Update].Count > 0)
                            {
                                _context.TkdHaisha.UpdateRange(command.Haishas[CommandMode.Update]);
                            }

                            //Karieis
                            if (command.Karieis[CommandMode.Delete].Count > 0)
                            {
                                _context.TkdKariei.RemoveRange(command.Karieis[CommandMode.Delete]);
                            }
                            if (command.Karieis[CommandMode.Create].Count > 0)
                            {
                                _context.TkdKariei.AddRange(command.Karieis[CommandMode.Create]);
                            }
                            if (command.Karieis[CommandMode.Update].Count > 0)
                            {
                                _context.TkdKariei.UpdateRange(command.Karieis[CommandMode.Update]);
                            }

                            if (command.SoftRemoveEntitiesData.YouSyus.Count > 0)
                            {
                                _context.TkdYouSyu.UpdateRange(command.SoftRemoveEntitiesData.YouSyus);
                            }
                            if (command.SoftRemoveEntitiesData.Youshas.Count > 0)
                            {
                                _context.TkdYousha.UpdateRange(command.SoftRemoveEntitiesData.Youshas);
                            }
                            if (command.SoftRemoveEntitiesData.YmfuTus.Count > 0)
                            {
                                _context.TkdYmfuTu.UpdateRange(command.SoftRemoveEntitiesData.YmfuTus);
                            }
                            if (command.SoftRemoveEntitiesData.YfutTus.Count > 0)
                            {
                                _context.TkdYfutTu.UpdateRange(command.SoftRemoveEntitiesData.YfutTus);
                            }
                            if (command.SoftRemoveEntitiesData.Mihrims.Count > 0)
                            {
                                _context.TkdMihrim.UpdateRange(command.SoftRemoveEntitiesData.Mihrims);
                            }
                            if (command.SoftRemoveEntitiesData.MfutTus.Count > 0)
                            {
                                _context.TkdMfutTu.UpdateRange(command.SoftRemoveEntitiesData.MfutTus);
                            }
                            if (command.SoftRemoveEntitiesData.Tehais.Count > 0)
                            {
                                _context.TkdTehai.UpdateRange(command.SoftRemoveEntitiesData.Tehais);
                            }
                            if (command.SoftRemoveEntitiesData.Koteis.Count > 0)
                            {
                                _context.TkdKotei.UpdateRange(command.SoftRemoveEntitiesData.Koteis);
                            }
                            if (command.SoftRemoveEntitiesData.Koteiks.Count > 0)
                            {
                                _context.TkdKoteik.UpdateRange(command.SoftRemoveEntitiesData.Koteiks);
                            }
                            if (command.SoftRemoveEntitiesData.Haiins.Count > 0)
                            {
                                _context.TkdHaiin.UpdateRange(command.SoftRemoveEntitiesData.Haiins);
                            }
                            if (command.SoftRemoveEntitiesData.Kobans.Count > 0)
                            {
                                _context.TkdKoban.RemoveRange(command.SoftRemoveEntitiesData.Kobans); // Hard delete kobans
                            }
                        }

                        //MaxMinFareFeeCalc
                        if (command.MaxMinFareFeeCalcs[CommandMode.Delete].Count > 0)
                        {
                            _context.TkdBookingMaxMinFareFeeCalc.RemoveRange(command.MaxMinFareFeeCalcs[CommandMode.Delete]);
                        }
                        if (command.MaxMinFareFeeCalcs[CommandMode.Create].Count > 0)
                        {
                            _context.TkdBookingMaxMinFareFeeCalc.AddRange(command.MaxMinFareFeeCalcs[CommandMode.Create]);
                        }
                        if (command.MaxMinFareFeeCalcs[CommandMode.Update].Count > 0)
                        {
                            _context.TkdBookingMaxMinFareFeeCalc.UpdateRange(command.MaxMinFareFeeCalcs[CommandMode.Update]);
                        }

                        //MaxMinFareFeeCalcMeisai
                        if (command.MaxMinFareFeeCalcMeisais[CommandMode.Delete].Count > 0)
                        {
                            _context.TkdBookingMaxMinFareFeeCalcMeisai.RemoveRange(command.MaxMinFareFeeCalcMeisais[CommandMode.Delete]);
                        }
                        if (command.MaxMinFareFeeCalcMeisais[CommandMode.Create].Count > 0)
                        {
                            _context.TkdBookingMaxMinFareFeeCalcMeisai.AddRange(command.MaxMinFareFeeCalcMeisais[CommandMode.Create]);
                        }
                        if (command.MaxMinFareFeeCalcMeisais[CommandMode.Update].Count > 0)
                        {
                            _context.TkdBookingMaxMinFareFeeCalcMeisai.UpdateRange(command.MaxMinFareFeeCalcMeisais[CommandMode.Update]);
                        }

                        await _context.SaveChangesAsync();
                        dbTran.Commit();
                    }
                    catch (DbUpdateConcurrencyException Ex)
                    {
                        if (!YykshoExists(command.Ukeno))
                        {
                            return new NotFoundResult();
                        }
                        else
                        {
                            /// Todo
                            _logger.LogError(Ex, Ex.Message);
                            dbTran.Rollback();
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs  
                        _logger.LogError(ex, ex.Message);
                        dbTran.Rollback();
                        return new BadRequestResult();
                    }
                }
                return new ContentResult { Content = command.Yyksho.UkeNo.ToString() };
            }

            private bool YykshoExists(string id)
            {
                bool yykShoExist = _context.TkdYyksho.Any(e => e.UkeNo == id);

                return yykShoExist;
            }

            private void AssignPriceTkdHaisha(
                Dictionary<CommandMode, List<TkdHaisha>> haishas,
                Dictionary<CommandMode, List<TkdMishum>> mishums,
                BookingFormData bookingdata
            )
            {
                var mishumNew = mishums[CommandMode.Update].Any() ? mishums[CommandMode.Update].FirstOrDefault(m => m.SeiFutSyu == 1)
                    : mishums[CommandMode.Create].FirstOrDefault(m => m.SeiFutSyu == 1);
                //#8091
                var listHaishaForSave = haishas[CommandMode.Create].Union(haishas[CommandMode.Update]).OrderBy(f => f.SyaSyuRen).ToList();
                int diffUnc = mishumNew.UriGakKin - listHaishaForSave.Sum(f => f.SyaRyoUnc);
                int assingeIndex = 0;
                int assingeUnit = 0;
                if (diffUnc != 0)
                {
                    while(diffUnc != 0)
                    {
                        assingeUnit = diffUnc < 0 ? -1 : 1;
                        listHaishaForSave[assingeIndex++].SyaRyoUnc += assingeUnit;
                        diffUnc -= assingeUnit;
                        if(assingeIndex == listHaishaForSave.Count) assingeIndex = 0;
                    }
                }
                int diffSyaRyoSyo = mishumNew.SyaRyoSyo - listHaishaForSave.Sum(f => f.SyaRyoSyo);
                if (diffSyaRyoSyo != 0)
                {
                    assingeIndex = 0;
                    while(diffSyaRyoSyo != 0)
                    {
                        assingeUnit = diffSyaRyoSyo < 0 ? -1 : 1;
                        listHaishaForSave[assingeIndex++].SyaRyoSyo += assingeUnit;
                        diffSyaRyoSyo -= assingeUnit;
                        if(assingeIndex == listHaishaForSave.Count) assingeIndex = 0;
                    }
                }
                int diffSyaRyoTes = mishumNew.SyaRyoTes - listHaishaForSave.Sum(f => f.SyaRyoTes);
                if (diffSyaRyoTes != 0)
                {
                    assingeIndex = 0;
                    while(diffSyaRyoTes != 0)
                    {
                        assingeUnit = diffSyaRyoTes < 0 ? -1 : 1;
                        listHaishaForSave[assingeIndex++].SyaRyoTes += assingeUnit;
                        diffSyaRyoTes -= assingeUnit;
                        if(assingeIndex == listHaishaForSave.Count) assingeIndex = 0;
                    }
                }
                //#8091
            }
        }
    }
}
