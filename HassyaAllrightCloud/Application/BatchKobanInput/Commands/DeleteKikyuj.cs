using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Application.BatchKobanInput.Commands
{
    public class DeleteKikyuj : IRequest<bool>
    {
        public string TargetYmd { get; set; }
        public int SyainCdSeq { get; set; }
        public class Handler : IRequestHandler<DeleteKikyuj, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<bool> Handle(DeleteKikyuj request, CancellationToken cancellationToken = default)
            {
                bool result = true;

                var kikoJoinData = (from ki in _dbcontext.TkdKikyuj
                                    join ko in _dbcontext.TkdKoban
                                    on new { key1 = ki.KinKyuTblCdSeq, key2 = "0", key3 = request.TargetYmd, key4 = request.SyainCdSeq } equals new { key1 = ko.KinKyuTblCdSeq, key2 = ko.UkeNo, key3 = ko.UnkYmd, key4 = ko.SyainCdSeq }
                                    where ki.SiyoKbn == (byte)1
                                    select new TkdKikyuj()
                                    {
                                        KinKyuTblCdSeq = ki.KinKyuTblCdSeq,
                                        BikoNm = ki.BikoNm,
                                        HenKai = ki.HenKai,
                                        KinKyuCdSeq = ki.KinKyuCdSeq,
                                        KinKyuEtime = ki.KinKyuEtime,
                                        KinKyuEymd = ki.KinKyuEymd,
                                        KinKyuStime = ki.KinKyuStime,
                                        KinKyuSymd = ki.KinKyuSymd,
                                        SiyoKbn = ki.SiyoKbn,
                                        SyainCdSeq = ki.SyainCdSeq,
                                        UpdPrgId = ki.UpdPrgId,
                                        UpdSyainCd = ki.UpdSyainCd,
                                        UpdTime = ki.UpdTime,
                                        UpdYmd = ki.UpdYmd
                                    }
                                    ).ToList();

                try
                {
                    foreach(var item in kikoJoinData)
                    {
                        _dbcontext.TkdKikyuj.Remove(item);
                        _dbcontext.SaveChanges();
                    }
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }
    }
}
