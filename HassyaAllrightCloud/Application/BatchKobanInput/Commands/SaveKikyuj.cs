using MediatR;
using System;
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
    public class SaveKikyuj : IRequest<(bool,int)>
    {
        public CellModel Cell { get; set; }
        public WorkHolidayTypeDataModel HolidayType { get; set; }
        public MyTime StartTime { get; set; }
        public MyTime EndTime { get; set; }
        public class Handler : IRequestHandler<SaveKikyuj, (bool,int)>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<(bool,int)> Handle(SaveKikyuj request, CancellationToken cancellationToken = default)
            {
                bool result = true;

                TkdKikyuj data = new TkdKikyuj()
                {
                    HenKai = 0,
                    SyainCdSeq = request.Cell.SyainCdSeq,
                    KinKyuSymd = request.Cell.Date,
                    KinKyuStime = request.StartTime.Str.Replace(":", string.Empty),
                    KinKyuEymd = request.Cell.Date,
                    KinKyuEtime = request.EndTime.Str.Replace(":", string.Empty),
                    KinKyuCdSeq = request.HolidayType.KinKyuCdSeq,
                    BikoNm = string.Empty,
                    SiyoKbn = 1,
                    UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty),
                    UpdTime = DateTime.Now.ToString().Substring(11).Replace(":", string.Empty),
                    UpdPrgId = "KU1200P",
                    UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq
                };
                try
                {
                    _dbcontext.TkdKikyuj.Add(data);
                    _dbcontext.SaveChanges();
                }
                catch
                {
                    result = false;
                }
                var KinKyuTblCdSeq = data.KinKyuTblCdSeq;
                return (result, KinKyuTblCdSeq);
            }
        }
    }
}
