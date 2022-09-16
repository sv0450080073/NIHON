using HassyaAllrightCloud.Commons.Constants;
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
    public class HandleWorkHolidayCommand : IRequest<bool>
    {
        public int SyainCdSeq { get; set; }
        public string UnkYmd { get; set; }
        public TkdKoban koban { get; set; }
        public TkdKikyuj kikyuj { get; set; }
        public class Handler : IRequestHandler<HandleWorkHolidayCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(HandleWorkHolidayCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var temp = (from ko in _context.TkdKoban
                                     join ki in _context.TkdKikyuj
                                     on ko.KinKyuTblCdSeq equals ki.KinKyuTblCdSeq
                                     where ki.SyainCdSeq == request.SyainCdSeq 
                                     && ki.KinKyuSymd.CompareTo(request.UnkYmd) <= 0 
                                     && ki.KinKyuEymd.CompareTo(request.UnkYmd) >= 0
                                     select new TkdKoban
                                     {
                                         UnkYmd = ko.UnkYmd,
                                         SyainCdSeq = ko.SyainCdSeq,
                                         KouBnRen = ko.KouBnRen,
                                         KinKyuTblCdSeq = ko.KinKyuTblCdSeq
                                     }).FirstOrDefault();

                        if(temp != null)
                        {
                            var koban = _context.TkdKoban.FirstOrDefault(_ => _.UnkYmd == temp.UnkYmd && _.SyainCdSeq == temp.SyainCdSeq && _.KouBnRen == temp.KouBnRen);
                            if(koban != null)
                            {
                                _context.TkdKoban.Remove(koban);
                            }
                            var kikyuj = _context.TkdKikyuj.FirstOrDefault(_ => _.KinKyuTblCdSeq == temp.KinKyuTblCdSeq);
                            if(kikyuj != null)
                            {
                                _context.TkdKikyuj.Remove(kikyuj);
                            }
                            _context.SaveChanges();
                        }

                        var currentDate = DateTime.Now;

                        request.kikyuj.SiyoKbn = CommonConstants.SiyoKbn;
                        request.kikyuj.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                        request.kikyuj.UpdTime = currentDate.ToString(CommonConstants.FormatHMS);
                        request.kikyuj.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        request.kikyuj.UpdPrgId = Common.UpdPrgId;

                        int countTempKoban = _context.TkdKoban.Count(_ => _.UnkYmd == request.koban.UnkYmd && _.SyainCdSeq == request.koban.SyainCdSeq);

                        if (countTempKoban > 0)
                        {
                            countTempKoban = _context.TkdKoban.Where(_ => _.UnkYmd == request.koban.UnkYmd && _.SyainCdSeq == request.koban.SyainCdSeq).Max(_ => _.KouBnRen);
                        }

                        request.koban.KouBnRen = (short)(countTempKoban + 1);
                        request.koban.SiyoKbn = CommonConstants.SiyoKbn;
                        request.koban.UpdYmd = currentDate.ToString(CommonConstants.FormatYMD);
                        request.koban.UpdTime = currentDate.ToString(CommonConstants.FormatHMS);
                        request.koban.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        request.koban.UpdPrgId = Common.UpdPrgId;

                        var entry = _context.TkdKikyuj.Add(request.kikyuj);
                        _context.SaveChanges();
                        request.koban.KinKyuTblCdSeq = entry.Entity.KinKyuTblCdSeq;
                        _context.TkdKoban.Add(request.koban);

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
