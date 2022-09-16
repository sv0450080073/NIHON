using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.Unkobi.Queries
{
    public class CollectDataUnkobiQuery : IRequest<TkdUnkobi>
    {
        public BusscheduleData ScheduleFormData { get; set; }

        public class Handler : IRequestHandler<CollectDataUnkobiQuery, TkdUnkobi>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<TkdUnkobi> Handle(CollectDataUnkobiQuery request, CancellationToken cancellationToken)
            {
                TkmKasSet tkmKasSet = this._context.TkmKasSet.Where(t => t.CompanyCdSeq == request.ScheduleFormData.CompanyID).FirstOrDefault();
                short UriKbn = tkmKasSet.UriKbn;
                TkdUnkobi tkdunkobi = new TkdUnkobi();
                tkdunkobi.UnkRen = 1;
                tkdunkobi.HenKai = 0;
                tkdunkobi.HaiSymd = request.ScheduleFormData.Itembus.Min(t => t.TimeStartString).ToString().Substring(0, 8);
                tkdunkobi.HaiStime = request.ScheduleFormData.Itembus.Min(t => t.TimeStartString).ToString().Substring(8, 4);
                tkdunkobi.TouYmd = request.ScheduleFormData.Itembus.Max(t => t.TimeEndString).ToString().Substring(0, 8);
                tkdunkobi.TouChTime = request.ScheduleFormData.Itembus.Max(t => t.TimeEndString).ToString().Substring(8, 4);
                tkdunkobi.SyuPaTime = tkdunkobi.HaiStime; // 出発時間

                // 売上年月日
                if (UriKbn == 1)
                {
                    tkdunkobi.UriYmd = tkdunkobi.HaiSymd; // 配車日時
                }
                else if (UriKbn == 2)
                {
                    tkdunkobi.UriYmd = tkdunkobi.TouYmd; // 終日予定
                }

                tkdunkobi.KanJnm = "";
                tkdunkobi.KanjJyus1 = "";
                tkdunkobi.KanjJyus2 = "";
                tkdunkobi.KanjTel = "";
                tkdunkobi.KanjFax = "";
                tkdunkobi.KanjKeiNo = "";
                tkdunkobi.KanjMail = "";
                tkdunkobi.KanDmhflg = 0;
                tkdunkobi.DanTaNm = request.ScheduleFormData.BookingName; // 団体名
                tkdunkobi.DanTaKana = request.ScheduleFormData.BookingName;
                tkdunkobi.IkMapCdSeq = 0;
                tkdunkobi.IkNm = "";
                tkdunkobi.HaiScdSeq = 0;
                tkdunkobi.HaiSnm = "";
                tkdunkobi.HaiSjyus1 = "";
                tkdunkobi.HaiSjyus2 = "";
                tkdunkobi.HaiSkouKcdSeq = 0;
                tkdunkobi.HaiSbinCdSeq = 0;
                tkdunkobi.HaiSsetTime = "";
                tkdunkobi.TouCdSeq = 0;
                tkdunkobi.TouNm = "";
                tkdunkobi.TouJyusyo1 = "";
                tkdunkobi.TouJyusyo2 = "";
                tkdunkobi.TouKouKcdSeq = 0;
                tkdunkobi.TouBinCdSeq = 0;
                tkdunkobi.TouSetTime = "";
                tkdunkobi.AreaMapSeq = 0;
                tkdunkobi.AreaNm = "";
                tkdunkobi.HasMapCdSeq = 0;
                tkdunkobi.HasNm = "";
                tkdunkobi.JyoKyakuCdSeq = 0;
                tkdunkobi.JyoSyaJin = 0;
                tkdunkobi.PlusJin = 0;
                tkdunkobi.DrvJin = (short)request.ScheduleFormData.BusdriverNum; // 運転手数
                tkdunkobi.GuiSu = (short)request.ScheduleFormData.BusGuideNum; // ガイド数
                tkdunkobi.OthJinKbn1 = 99;
                tkdunkobi.OthJin1 = 0;
                tkdunkobi.OthJinKbn2 = 99;
                tkdunkobi.OthJin2 = 0;
                tkdunkobi.Kskbn = 2; // 仮車区分 1:未仮車 2:仮車済 3:一部済
                tkdunkobi.KhinKbn = 1;
                tkdunkobi.HaiSkbn = 1;
                tkdunkobi.HaiIkbn = 1;
                tkdunkobi.GuiWnin = 0;
                tkdunkobi.NippoKbn = 1;
                tkdunkobi.YouKbn = 1;
                tkdunkobi.UkeJyKbnCd = 99;
                tkdunkobi.SijJoKbn1 = 99;
                tkdunkobi.SijJoKbn2 = 99;
                tkdunkobi.SijJoKbn3 = 99;
                tkdunkobi.SijJoKbn4 = 99;
                tkdunkobi.SijJoKbn5 = 99;
                tkdunkobi.RotCdSeq = 0;
                tkdunkobi.ZenHaFlg = 0;
                tkdunkobi.KhakFlg = 0;
                tkdunkobi.UnkoJkbn = 5;
                tkdunkobi.SyuKoTime = tkdunkobi.HaiStime;
                tkdunkobi.KikTime = tkdunkobi.TouChTime;
                tkdunkobi.BikoTblSeq = 0;
                tkdunkobi.SiyoKbn = 1;
                tkdunkobi.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                tkdunkobi.UpdTime = DateTime.Now.ToString("HHmmss");
                tkdunkobi.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                tkdunkobi.UpdPrgId = "KU0100";
                tkdunkobi.DispTouYmd = tkdunkobi.TouYmd;
                tkdunkobi.DispTouChTime = tkdunkobi.TouChTime;
                tkdunkobi.DispKikYmd = tkdunkobi.TouYmd;
                tkdunkobi.DispKikTime = tkdunkobi.TouChTime;
                tkdunkobi.DispSyuPaYmd = tkdunkobi.HaiSymd;
                tkdunkobi.DispSyuPaTime = tkdunkobi.HaiStime;
                tkdunkobi.KikYmd = tkdunkobi.TouYmd;
                tkdunkobi.SyukoYmd = tkdunkobi.HaiSymd;
                tkdunkobi.SyuPaYmd = tkdunkobi.HaiSymd;
                return await Task.FromResult(tkdunkobi);
            }
        }
    }
}
