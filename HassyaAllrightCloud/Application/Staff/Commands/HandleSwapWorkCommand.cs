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
    public class HandleSwapWorkCommand : IRequest<bool>
    {
        public TkdHaiin haiin { get; set; }
        public List<TkdKoban> listKoban { get; set; }
        public JobItem job { get; set; }

        public TkdHaiin haiin1 { get; set; }
        public List<TkdKoban> listKoban1 { get; set; }
        public JobItem job1 { get; set; }
        public bool isSwapJob { get; set; } = false;

        public class Handler : IRequestHandler<HandleSwapWorkCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(HandleSwapWorkCommand request, CancellationToken cancellationToken)
            {
                using(var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        byte HaiInRen = 0, HaiInRen1 = 0;
                        var currentDate = DateTime.Now;

                        HaiInRen1 = HandleSwap(request.job, request.haiin, request.listKoban, currentDate);
                        if (request.isSwapJob)
                        {
                            HaiInRen = HandleSwap(request.job1, request.haiin1, request.listKoban1, currentDate);
                        }
                        request.job.HaiInRen = HaiInRen;
                        if(request.job1 != null)
                        {
                            request.job1.HaiInRen = HaiInRen1;
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

            private byte HandleSwap(JobItem job, TkdHaiin Rhaiin, List<TkdKoban> listKoban, DateTime currentDate)
            {
                var haiin = _context.TkdHaiin.FirstOrDefault(_ => _.UkeNo == job.UkeNo && _.UnkRen == job.UnkRen && _.HaiInRen == job.HaiInRen
                                                               && _.TeiDanNo == job.TeiDanNo && _.BunkRen == job.BunkRen);

                if (haiin != null)
                {
                    haiin.HenKai = 0;
                    haiin.SiyoKbn = 2;
                    haiin.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                    haiin.UpdTime = currentDate.ToString(CommonConstants.FormatHMS);
                    haiin.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    haiin.UpdPrgId = Common.UpdPrgId;
                    _context.SaveChanges();
                }

                var oldKoban = _context.TkdKoban.Where(_ => _.UkeNo == job.UkeNo && _.UnkRen == job.UnkRen && _.SyainCdSeq == job.SyainCdSeq
                                                                  && _.TeiDanNo == job.TeiDanNo && _.BunkRen == job.BunkRen).ToList();

                if (oldKoban.Count > 0)
                {
                    _context.TkdKoban.RemoveRange(oldKoban);
                }

                int countTempHaiin = _context.TkdHaiin.Count(_ => _.UkeNo == Rhaiin.UkeNo && _.UnkRen == Rhaiin.UnkRen
                                                               && _.TeiDanNo == Rhaiin.TeiDanNo && _.BunkRen == Rhaiin.BunkRen);

                if (countTempHaiin > 0)
                {
                    countTempHaiin = _context.TkdHaiin.Where(_ => _.UkeNo == Rhaiin.UkeNo && _.UnkRen == Rhaiin.UnkRen
                                                               && _.TeiDanNo == Rhaiin.TeiDanNo && _.BunkRen == Rhaiin.BunkRen).Max(_ => _.HaiInRen);
                }

                Rhaiin.HaiInRen = (byte)(countTempHaiin + 1);
                Rhaiin.SiyoKbn = CommonConstants.SiyoKbn;
                Rhaiin.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                Rhaiin.UpdTime = currentDate.ToString(CommonConstants.FormatHMS);
                Rhaiin.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                Rhaiin.UpdPrgId = Common.UpdPrgId;
                _context.TkdHaiin.Add(Rhaiin);
                _context.SaveChanges();

                foreach (var koban in listKoban)
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

                var haisha = _context.TkdHaisha.FirstOrDefault(_ => _.UkeNo == job.UkeNo && _.UnkRen == job.UnkRen
                                                                  & _.TeiDanNo == job.TeiDanNo && _.BunkRen == job.BunkRen);

                if (haisha != null)
                {
                    var data = _context.TkdHaiin.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn
                                                         && _.UkeNo == job.UkeNo && _.UnkRen == job.UnkRen
                                                         && _.TeiDanNo == job.TeiDanNo && _.BunkRen == job.BunkRen);

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

                return (byte)(countTempHaiin + 1); ;
            }
        }
    }
}
