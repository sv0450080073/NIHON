using System;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.BatchKobanInput.Commands
{
    public class SaveKoban : IRequest<bool>
    {
        public CellModel Cell { get; set; }
        public WorkHolidayTypeDataModel HolidayType { get; set; }
        public MyTime StartTime { get; set; }
        public MyTime EndTime { get; set; }
        public int KinKyuTblCdSeq { get; set; }
        public class Handler : IRequestHandler<SaveKoban, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<bool> Handle(SaveKoban request, CancellationToken cancellationToken = default)
            {
                bool result = true;
                var KouBnRen = 1;
                var KouBnRens = _dbcontext.TkdKoban.Where(x => x.UnkYmd == request.Cell.Date && x.SyainCdSeq == request.Cell.SyainCdSeq && x.SiyoKbn == 1).OrderByDescending(x => x.KouBnRen).ToList();

                if(KouBnRens.Count != 0 && KouBnRens != null)
                {
                    KouBnRen = KouBnRens[0].KouBnRen + 1;
                }
                else
                {
                    KouBnRen = 1;
                }

                TkdKoban data = new TkdKoban()
                {
                    UnkYmd = request.Cell.Date,
                    SyainCdSeq = request.Cell.SyainCdSeq,
                    KouBnRen = (byte)KouBnRen,
                    HenKai = 0,
                    SyugyoKbn = 1,
                    KinKyuTblCdSeq = request.KinKyuTblCdSeq,
                    UkeNo = "0",
                    UnkRen = 0,
                    SyaSyuRen = 0,
                    TeiDanNo = 0,
                    BunkRen = 0,
                    RotCdSeq = 0,
                    RenEigCd = 0,
                    SigySyu = 0,
                    KitYmd = string.Empty,
                    SigyKbn = 0,
                    SigyCd = string.Empty,
                    SyukinYmd = request.Cell.Date,
                    SyukinTime = request.StartTime.Str.Replace(":", string.Empty),
                    TaikinYmd = request.Cell.Date,
                    TaiknTime = request.EndTime.Str.Replace(":", string.Empty),
                    Syukinbasy = string.Empty,
                    TaiknBasy = string.Empty,
                    KouZokPtnKbn = request.HolidayType.KinKyuKbn == 1 ? (byte)8 : (byte)7,
                    FuriYmd = string.Empty,
                    RouTime = string.Empty,
                    KouStime = string.Empty,
                    TaikTime = string.Empty,
                    KyuKtime = string.Empty,
                    JitdTime = string.Empty,
                    ZangTime = string.Empty,
                    UsinyTime = string.Empty,
                    SsinTime = string.Empty,
                    BikoNm = string.Empty,
                    SiyoKbn = 1,
                    UpdYmd = DateTime.Now.ToString().Substring(0,10).Replace("/", string.Empty),
                    UpdTime = DateTime.Now.ToString().Substring(11).Replace(":", string.Empty),
                    UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                    UpdPrgId = "KU1200P"
                };


                try
                {
                    _dbcontext.TkdKoban.Add(data);
                    _dbcontext.SaveChanges();
                }
                catch(Exception e)
                {
                    result = false;
                }
                return result;
            }
        }
    }
}
