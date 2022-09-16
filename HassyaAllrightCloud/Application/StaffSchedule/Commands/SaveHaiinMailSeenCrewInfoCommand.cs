using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StaffSchedule.Commands
{
    public class SaveHaiinMailSeenCrewInfoCommand : IRequest<bool>
    {
        public AppointmentList staffInfo { get; set; }
        public class Handler : IRequestHandler<SaveHaiinMailSeenCrewInfoCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(SaveHaiinMailSeenCrewInfoCommand request, CancellationToken cancellationToken)
            {
                TkdHaiinMail HaiinMail = (from haiinmail in _context.TkdHaiinMail
                                                                  .Where(x => x.UkeNo == request.staffInfo.HaiinInfo.UkeNo && x.UnkRen == request.staffInfo.HaiinInfo.UnkRen
                                                                  && x.TeiDanNo == request.staffInfo.HaiinInfo.TeiDanNo && x.BunkRen == request.staffInfo.HaiinInfo.BunkRen
                                                                  && x.HaiInRen == request.staffInfo.HaiinInfo.HaiInRen)
                                          select haiinmail).FirstOrDefault();
                using (IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (HaiinMail == null)
                        {
                            TkdHaiinMail itemHaiinMail = new TkdHaiinMail()
                            {
                                UkeNo = request.staffInfo.HaiinInfo.UkeNo,
                                UnkRen = (short)request.staffInfo.HaiinInfo.UnkRen,
                                TeiDanNo = (short)request.staffInfo.HaiinInfo.TeiDanNo,
                                BunkRen = (short)request.staffInfo.HaiinInfo.BunkRen,
                                HaiInRen = (byte)request.staffInfo.HaiinInfo.HaiInRen,
                                SyainCdSeq = 0,
                                KinKyuTblCdSeq = 0,
                                UnkYmd = "",
                                ControlNo = "",
                                MailExeCnt = 0,
                                LineExeCnt = 0,
                                SchReadKbn = 1,
                                Biko = "",
                                UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                                UpdTime = DateTime.Now.ToString("HHmmss"),
                                UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq

                            };
                            _context.TkdHaiinMail.Add(itemHaiinMail);
                        }
                        else
                        {
                            HaiinMail.SchReadKbn = 1;
                            HaiinMail.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            HaiinMail.UpdTime = DateTime.Now.ToString("HHmmss");
                            HaiinMail.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;

                        }
                        await _context.SaveChangesAsync();
                        dbTran.Commit();
                        return true;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                }
            }

        }
    }
}
