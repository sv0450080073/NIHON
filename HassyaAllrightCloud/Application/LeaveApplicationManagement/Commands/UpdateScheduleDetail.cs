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

namespace HassyaAllrightCloud.Application.LeaveApplicationManagement.Commands
{
    public class UpdateScheduleDetail : IRequest<bool>
    {
        public ScheduleDetail scheduleDetail { get; set; }
        public class Handler : IRequestHandler<UpdateScheduleDetail, bool>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;
            public async Task<bool> Handle(UpdateScheduleDetail request, CancellationToken cancellationToken)
            {
                int kinKyuTblCdSeq = 0;

                var schedule = _dbContext.TkdSchYotei.Where(x => x.YoteiSeq == request.scheduleDetail.YoteiSeq).FirstOrDefault();
                schedule.ShoRejBiko = request.scheduleDetail.ShoRejBiko;
                schedule.YoteiShoKbn = request.scheduleDetail.ApprovalStatus.status == StaffScheduleConstants.Pending ? (byte)1 : request.scheduleDetail.ApprovalStatus.status == StaffScheduleConstants.Accept ? (byte)2 : (byte)3;
                schedule.ShoUpdYmd = DateTime.Now.ToString("yyyyMMdd");
                schedule.ShoUpdTime = DateTime.Now.ToString("HHmmss");
                schedule.ShoSyainCdSeq = request.scheduleDetail.SyainCdSeq;

                var kikyuj = _dbContext.TkdKikyuj.Where(x => x.KinKyuTblCdSeq == schedule.KinKyuTblCdSeq).FirstOrDefault();
                if (kikyuj != null)
                {
                    kikyuj.SyainCdSeq = request.scheduleDetail.SyainCdSeq;
                    kikyuj.KinKyuSymd = request.scheduleDetail.YoteiSYmd;
                    kikyuj.KinKyuStime = request.scheduleDetail.YoteiSTime.Substring(0, 4);
                    if(schedule.AllDayKbn == 1)
                    {
                        kikyuj.KinKyuEtime = "2359";
                    }
                    else
                    {
                        kikyuj.KinKyuEtime = request.scheduleDetail.YoteiETime.Substring(0, 4);
                    }
                    kikyuj.KinKyuEymd = request.scheduleDetail.YoteiEYmd;
                    if (request.scheduleDetail.ApprovalStatus.status == StaffScheduleConstants.Accept)
                    {
                        kikyuj.SiyoKbn = 1;
                    }
                    if (request.scheduleDetail.ApprovalStatus.status == StaffScheduleConstants.Refuse || request.scheduleDetail.ApprovalStatus.status == StaffScheduleConstants.Pending)
                    {
                        kikyuj.SiyoKbn = 2;
                        List<TkdKoban> deletedKobans = _dbContext.TkdKoban.Where(x => x.KinKyuTblCdSeq == kikyuj.KinKyuTblCdSeq && x.SyainCdSeq == request.scheduleDetail.SyainCdSeq).ToList();
                        if(deletedKobans.Count > 0)
                        {
                            _dbContext.TkdKoban.RemoveRange(deletedKobans);
                        }
                    }
                    kikyuj.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    kikyuj.UpdTime = DateTime.Now.ToString("HHmmss");
                    kikyuj.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    kikyuj.KinKyuCdSeq = schedule.KinKyuCdSeq;
                    kikyuj.UpdPrgId = "CNV";
                    kinKyuTblCdSeq = kikyuj.KinKyuTblCdSeq;
                    _dbContext.TkdKikyuj.Update(kikyuj);
                }
                else
                {
                    var kinKyuEtime = string.Empty;
                    var status = 2;
                    if (request.scheduleDetail.ApprovalStatus.status == StaffScheduleConstants.Accept)
                    {
                        status = 1;
                    }
                    else
                    {
                        status = 2;
                    }
                    if (schedule.AllDayKbn == 1)
                    {
                        kinKyuEtime = "2359";
                    }
                    else
                    {
                        kinKyuEtime = request.scheduleDetail.YoteiETime.Substring(0, 4);
                    }
                    TkdKikyuj tkdKikyuj = new TkdKikyuj()
                    {
                        HenKai = Convert.ToInt16("1"),
                        SyainCdSeq = request.scheduleDetail.SyainCdSeq,
                        KinKyuSymd = request.scheduleDetail.YoteiSYmd,
                        KinKyuStime = request.scheduleDetail.YoteiSTime.Substring(0, 4),
                        KinKyuEymd = request.scheduleDetail.YoteiEYmd,
                        KinKyuEtime = kinKyuEtime,
                        SiyoKbn = (byte)status,
                        UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                        UpdTime = DateTime.Now.ToString("HHmmss"),
                        UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                        UpdPrgId = "CNV",
                        KinKyuCdSeq = schedule.KinKyuCdSeq,
                        BikoNm = string.Empty
                    };

                    _dbContext.TkdKikyuj.Add(tkdKikyuj);
                    _dbContext.SaveChanges();
                    kinKyuTblCdSeq = tkdKikyuj.KinKyuTblCdSeq;
                    schedule.KinKyuTblCdSeq = kinKyuTblCdSeq;
                }

                if (request.scheduleDetail.ApprovalStatus.status.Equals(StaffScheduleConstants.Accept))
                {
                    List<DateTimeModel> dateTimes = new List<DateTimeModel>();
                    
                    var startDate = DateTime.ParseExact(request.scheduleDetail.YoteiSYmd, "yyyyMMdd", null);
                    var endDate = DateTime.ParseExact(request.scheduleDetail.YoteiEYmd, "yyyyMMdd", null);
                    var startTime = request.scheduleDetail.YoteiSTime;
                    var endTime = request.scheduleDetail.YoteiETime;
                    var registedKobanSingle = _dbContext.TkdKoban.Where(x => x.UnkYmd == request.scheduleDetail.YoteiSYmd && x.SyainCdSeq == request.scheduleDetail.SyainCdSeq).ToList().Count() + 1;
                    if (startDate == endDate || ((endDate - startDate).Days == 1 && startTime == "000000" && endTime == "000000"))
                    {
                        TkdKoban tkdKoban = new TkdKoban() {
                            UnkYmd = request.scheduleDetail.YoteiSYmd,
                            SyukinTime = "0000",
                            TaiknTime = "2359",
                            SyainCdSeq = request.scheduleDetail.SyainCdSeq,
                            KouBnRen = (short)registedKobanSingle,
                            HenKai = 0,
                            SyugyoKbn = 1,
                            KinKyuTblCdSeq = kinKyuTblCdSeq,
                            UnkRen = 0,
                            SyaSyuRen = 0,
                            TeiDanNo = 0,
                            BunkRen = 0,
                            RotCdSeq = 0,
                            RenEigCd = 0,
                            SigySyu = 0,
                            SigyKbn = 0,
                            KouZokPtnKbn = 8,
                            SiyoKbn = 1,
                            UpdPrgId = Common.UpdPrgId,
                            UpdTime = DateTime.Now.ToString("HHmmss"),
                            UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                            UkeNo = string.Empty,
                            KitYmd = string.Empty,
                            SigyCd = string.Empty,
                            Syukinbasy = string.Empty,
                            TaiknBasy = string.Empty,
                            SyukinYmd = string.Empty,
                            TaikinYmd = string.Empty,
                            FuriYmd = string.Empty,
                            RouTime = string.Empty,
                            KouStime = string.Empty,
                            TaikTime = string.Empty,
                            KyuKtime = string.Empty,
                            JitdTime = string.Empty,
                            ZangTime = string.Empty,
                            UsinyTime = string.Empty,
                            SsinTime = string.Empty,
                            BikoNm = string.Empty
                        };
                        var existKoban = _dbContext.TkdKoban.Where(x => x.UnkYmd == tkdKoban.UnkYmd).FirstOrDefault();
                        if(existKoban == null)
                        {
                            _dbContext.TkdKoban.Add(tkdKoban);
                        }
                    }
                    else
                    {
                        if((endDate - startDate).Days == 1)
                        {
                            dateTimes.Add(new DateTimeModel()
                            {
                                Date = request.scheduleDetail.YoteiSYmd,
                                StartTime = request.scheduleDetail.YoteiSTime,
                                EndTime = "2339"
                            });
                            dateTimes.Add(new DateTimeModel()
                            {
                                Date = request.scheduleDetail.YoteiEYmd,
                                StartTime = "0000",
                                EndTime = request.scheduleDetail.YoteiETime
                            });
                        }
                        else
                        {
                            dateTimes.Add(new DateTimeModel()
                            {
                                Date = request.scheduleDetail.YoteiSYmd,
                                StartTime = request.scheduleDetail.YoteiSTime.Substring(0, 4),
                                EndTime = "2339"
                            });

                            for (int i = 1; i < (endDate - startDate).Days; i++)
                            {
                                dateTimes.Add(new DateTimeModel()
                                {
                                    Date = startDate.AddDays(i).ToString().Substring(0,10).Replace("/", string.Empty),
                                    StartTime = "0000",
                                    EndTime = "2339"
                                });
                            }

                            dateTimes.Add(new DateTimeModel()
                            {
                                Date = request.scheduleDetail.YoteiEYmd,
                                StartTime = "0000",
                                EndTime = request.scheduleDetail.YoteiETime.Substring(0,4)
                            });
                        }
                    }
                    foreach(var item in dateTimes)
                    {
                        if(item.StartTime == item.EndTime)
                        {
                            continue;
                        }
                        var existKoban = _dbContext.TkdKoban.Where(x => x.UnkYmd == item.Date && x.SyainCdSeq == request.scheduleDetail.SyainCdSeq && x.KinKyuTblCdSeq == kinKyuTblCdSeq).FirstOrDefault();
                        var registedKoban = _dbContext.TkdKoban.Where(x => x.UnkYmd == item.Date && x.SyainCdSeq == request.scheduleDetail.SyainCdSeq).ToList().Count() + 1;
                        if (existKoban == null)
                        {
                            _dbContext.TkdKoban.Add(new TkdKoban()
                            {
                                UnkYmd = item.Date,
                                SyukinTime = item.StartTime.Substring(0,4),
                                TaiknTime = item.EndTime.Substring(0, 4),
                                SyainCdSeq = request.scheduleDetail.SyainCdSeq,
                                KouBnRen = (short)registedKoban,
                                HenKai = 0,
                                SyugyoKbn = 1,
                                KinKyuTblCdSeq = kinKyuTblCdSeq,
                                UnkRen = 0,
                                SyaSyuRen = 0,
                                TeiDanNo = 0,
                                BunkRen = 0,
                                RotCdSeq = 0,
                                RenEigCd = 0,
                                SigySyu = 0,
                                SigyKbn = 0,
                                KouZokPtnKbn = 8,
                                SiyoKbn = 1,
                                UpdPrgId = Common.UpdPrgId,
                                UpdTime = DateTime.Now.ToString("HHmmss"),
                                UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                                UkeNo = string.Empty,
                                KitYmd = string.Empty,
                                SigyCd = string.Empty,
                                Syukinbasy = string.Empty,
                                TaiknBasy = string.Empty,
                                SyukinYmd = string.Empty,
                                TaikinYmd = string.Empty,
                                FuriYmd = string.Empty,
                                RouTime = string.Empty,
                                KouStime = string.Empty,
                                TaikTime = string.Empty,
                                KyuKtime = string.Empty,
                                JitdTime = string.Empty,
                                ZangTime = string.Empty,
                                UsinyTime = string.Empty,
                                SsinTime = string.Empty,
                                BikoNm = string.Empty
                            });
                        }
                    }
                }
                _dbContext.TkdSchYotei.Update(schedule);
                _dbContext.SaveChanges();
                return true;

            }
        }
    }
}
