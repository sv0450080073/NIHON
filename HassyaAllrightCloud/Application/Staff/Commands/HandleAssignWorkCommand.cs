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

namespace HassyaAllrightCloud.Application.Staff.Commands
{
    public class HandleAssignWorkCommand : IRequest<bool>
    {
        public JobItem job { get; set; }
        public TkdHaiin haiin { get; set; }
        public List<TkdKoban> listKoban { get; set; }
        public class Handler : IRequestHandler<HandleAssignWorkCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(HandleAssignWorkCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var currentDate = DateTime.Now;
                        int countTempHaiin = _context.TkdHaiin.Count(_ => _.UkeNo == request.haiin.UkeNo && _.UnkRen == request.haiin.UnkRen
                                                                       && _.TeiDanNo == request.haiin.TeiDanNo && _.BunkRen == request.haiin.BunkRen);

                        if (countTempHaiin > 0)
                        {
                            countTempHaiin = _context.TkdHaiin.Where(_ => _.UkeNo == request.haiin.UkeNo && _.UnkRen == request.haiin.UnkRen
                                                                       && _.TeiDanNo == request.haiin.TeiDanNo && _.BunkRen == request.haiin.BunkRen).Max(_ => _.HaiInRen);
                        }

                        request.job.HaiInRen = (byte)(countTempHaiin + 1);
                        request.haiin.HaiInRen = (byte)(countTempHaiin + 1);
                        request.haiin.SiyoKbn = CommonConstants.SiyoKbn;
                        request.haiin.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                        request.haiin.UpdTime = currentDate.ToString(CommonConstants.FormatHMS);
                        request.haiin.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        request.haiin.UpdPrgId = Common.UpdPrgId;
                        _context.TkdHaiin.Add(request.haiin);
                        _context.SaveChanges();

                        foreach (var koban in request.listKoban)
                        {
                            int countTempKoban = _context.TkdKoban.Count(_ => _.UnkYmd == koban.UnkYmd && _.SyainCdSeq == koban.SyainCdSeq);

                            if (countTempKoban > 0)
                            {
                                countTempKoban = _context.TkdKoban.Where(_ => _.UnkYmd == koban.UnkYmd && _.SyainCdSeq == koban.SyainCdSeq).Max(_ => _.KouBnRen);
                            }

                            koban.KouBnRen = (short)(countTempKoban + 1);
                            koban.SiyoKbn = CommonConstants.SiyoKbn;
                            koban.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                            koban.UpdTime = currentDate.ToString(CommonConstants.FormatHMS);
                            koban.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            koban.UpdPrgId = Common.UpdPrgId;
                            _context.TkdKoban.Add(koban);
                            _context.SaveChanges();
                        }

                        var haisha = _context.TkdHaisha.FirstOrDefault(_ => _.UkeNo == request.job.UkeNo && _.UnkRen == request.job.UnkRen
                                                                         && _.TeiDanNo == request.job.TeiDanNo && _.BunkRen == request.job.BunkRen);

                        if(haisha != null)
                        {
                            var haiin = _context.TkdHaiin.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn
                                                                  && _.UkeNo == request.job.UkeNo && _.UnkRen == request.job.UnkRen
                                                                  && _.TeiDanNo == request.job.TeiDanNo && _.BunkRen == request.job.BunkRen);

                            var drvJin = haiin.Count(_ => _.SyokumuKbn == 1 || _.SyokumuKbn == 2);
                            var guiSu = haiin.Count(_ => _.SyokumuKbn == 3 || _.SyokumuKbn == 4);

                            if (haisha.HaiSkbn == 1 && haisha.Kskbn == 2)
                            {
                                if(haisha.DrvJin >= drvJin && haisha.GuiSu >= guiSu)
                                {
                                    haisha.KhinKbn = 2;
                                }
                                else if(haisha.DrvJin <= drvJin && haisha.GuiSu <= guiSu)
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
                            haisha.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                            haisha.UpdTime = currentDate.ToString(CommonConstants.FormatHMS);
                            haisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            haisha.UpdPrgId = Common.UpdPrgId;
                        }

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
