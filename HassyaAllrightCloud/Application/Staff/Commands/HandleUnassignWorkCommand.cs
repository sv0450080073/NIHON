using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Staff.Commands
{
    public class HandleUnassignWorkCommand : IRequest<bool>
    {
        public JobItem job { get; set; }
        public class Handler : IRequestHandler<HandleUnassignWorkCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(HandleUnassignWorkCommand request, CancellationToken cancellationToken)
            {
                using(var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var haiin = _context.TkdHaiin.FirstOrDefault(_ => _.UkeNo == request.job.UkeNo && _.UnkRen == request.job.UnkRen && _.HaiInRen == request.job.HaiInRen
                                                                       && _.TeiDanNo == request.job.TeiDanNo && _.BunkRen == request.job.BunkRen);

                        var koban = _context.TkdKoban.Where(_ => _.UkeNo == request.job.UkeNo && _.UnkRen == request.job.UnkRen && _.SyainCdSeq == request.job.SyainCdSeq
                                                                       && _.TeiDanNo == request.job.TeiDanNo && _.BunkRen == request.job.BunkRen).ToList();

                        if(haiin != null)
                        {
                            haiin.HenKai = 0;
                            haiin.SiyoKbn = 2;
                            haiin.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatYMD);
                            haiin.UpdTime = DateTime.Now.ToString(CommonConstants.FormatHMS);
                            haiin.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            haiin.UpdPrgId = Common.UpdPrgId;
                            _context.SaveChanges();
                        }

                        if (koban.Count > 0)
                        {
                            _context.TkdKoban.RemoveRange(koban);
                        }

                        var haisha = _context.TkdHaisha.FirstOrDefault(_ => _.UkeNo == request.job.UkeNo && _.UnkRen == request.job.UnkRen
                                                                         && _.TeiDanNo == request.job.TeiDanNo && _.BunkRen == request.job.BunkRen);

                        if (haisha != null)
                        {
                            var data = _context.TkdHaiin.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn
                                                                 && _.UkeNo == request.job.UkeNo && _.UnkRen == request.job.UnkRen
                                                                 && _.TeiDanNo == request.job.TeiDanNo && _.BunkRen == request.job.BunkRen);

                            var drvJin = data.Count(_ => _.SyokumuKbn == 1 || _.SyokumuKbn == 2);
                            var guiSu = data.Count(_ => _.SyokumuKbn == 3 || _.SyokumuKbn == 4);

                            if (haisha.HaiSkbn == 1 && haisha.Kskbn == 2)
                            {
                                if (haisha.DrvJin >= drvJin && haisha.GuiSu >= guiSu)
                                {
                                    haisha.KhinKbn = 2;
                                }
                                else if (haisha.DrvJin <= drvJin && haisha.GuiSu <= guiSu)
                                {
                                    haisha.KhinKbn = 3;
                                }
                            }
                            if (haisha.HaiSkbn == 2)
                            {
                                if (haisha.DrvJin >= drvJin && haisha.GuiSu >= guiSu)
                                {
                                    haisha.HaiIkbn = 2;
                                }
                                if (haisha.DrvJin <= drvJin && haisha.GuiSu <= guiSu)
                                {
                                    haisha.HaiIkbn = 3;
                                }
                            }
                            haisha.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatYMD);
                            haisha.UpdTime = DateTime.Now.ToString(CommonConstants.FormatHMS);
                            haisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            haisha.UpdPrgId = Common.UpdPrgId;
                        }

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
