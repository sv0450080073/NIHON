using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.Haisha.Queries
{
    public class CollectDataHaishaQuery : IRequest<List<TkdHaisha>>
    {
        public BusscheduleData ScheduleFormData { get; set; }
        public List<TkdYykSyu> ListYykSyu { get; set; }

        public class Handler : IRequestHandler<CollectDataHaishaQuery, List<TkdHaisha>>
        {
            public object CommonConstants { get; private set; }

            public async Task<List<TkdHaisha>> Handle(CollectDataHaishaQuery request, CancellationToken cancellationToken)
            {
                List<TkdHaisha> listTkdHaisha = new List<TkdHaisha> { };
                short i = 1;
                foreach (var vehicleTypeRow in request.ScheduleFormData.Itembus.OrderBy(t => t.TimeStartString))
                {
                    TkdHaisha tkdhaisha = new TkdHaisha();
                    tkdhaisha.UnkRen = 1;
                    tkdhaisha.SyaSyuRen = request.ListYykSyu.Where(t => t.SyaSyuCdSeq == vehicleTypeRow.SyaSyuCdSeq).First().SyaSyuRen;
                    tkdhaisha.TeiDanNo = i;
                    tkdhaisha.BunkRen = 1;
                    tkdhaisha.HenKai = 0;
                    tkdhaisha.GoSya = i.ToString("D2");
                    tkdhaisha.GoSyaJyn = i;
                    tkdhaisha.BunKsyuJyn = 0;
                    tkdhaisha.SyuEigCdSeq = vehicleTypeRow.SyuEigCdSeq;
                    tkdhaisha.KikEigSeq = vehicleTypeRow.KikEigSeq;
                    tkdhaisha.HaiSsryCdSeq = int.Parse(vehicleTypeRow.BusLine);
                    tkdhaisha.KssyaRseq = int.Parse(vehicleTypeRow.BusLine);
                    tkdhaisha.DanTaNm2 = "";
                    tkdhaisha.IkMapCdSeq = 0;
                    tkdhaisha.IkNm = "";
                    tkdhaisha.SyuKoYmd = vehicleTypeRow.StartDate;
                    tkdhaisha.SyuKoTime = vehicleTypeRow.TimeStart.ToString("D4");
                    tkdhaisha.SyuPaTime = vehicleTypeRow.TimeStart.ToString("D4");
                    tkdhaisha.HaiSymd = vehicleTypeRow.StartDate;
                    tkdhaisha.HaiStime = vehicleTypeRow.TimeStart.ToString("D4");
                    tkdhaisha.HaiScdSeq = 0;
                    tkdhaisha.HaiSnm = "";
                    tkdhaisha.HaiSjyus1 = "";
                    tkdhaisha.HaiSjyus2 = "";
                    tkdhaisha.HaiSkigou = "";
                    tkdhaisha.HaiSkouKcdSeq = 0;
                    tkdhaisha.HaiSbinCdSeq = 0;
                    tkdhaisha.HaiSsetTime = "";
                    tkdhaisha.KikYmd = vehicleTypeRow.EndDate;
                    tkdhaisha.KikTime = vehicleTypeRow.TimeEnd.ToString("D4");
                    tkdhaisha.TouYmd = vehicleTypeRow.EndDate;
                    tkdhaisha.TouChTime = vehicleTypeRow.TimeEnd.ToString("D4");
                    tkdhaisha.TouCdSeq = 0;
                    tkdhaisha.TouNm = "";
                    tkdhaisha.TouJyusyo1 = "";
                    tkdhaisha.TouJyusyo2 = "";
                    tkdhaisha.TouKigou = "";
                    tkdhaisha.TouKouKcdSeq = 0;
                    tkdhaisha.TouBinCdSeq = 0;
                    tkdhaisha.TouSetTime = "";
                    tkdhaisha.JyoSyaJin = 0;
                    tkdhaisha.PlusJin = 0;
                    tkdhaisha.DrvJin = 0;
                    tkdhaisha.GuiSu = 0;
                    tkdhaisha.OthJinKbn1 = 99;
                    tkdhaisha.OthJin1 = 0;
                    tkdhaisha.OthJinKbn2 = 99;
                    tkdhaisha.OthJin2 = 0;
                    tkdhaisha.Kskbn = 2;
                    tkdhaisha.KhinKbn = 1;
                    tkdhaisha.HaiSkbn = 1;
                    tkdhaisha.HaiIkbn = 1;
                    tkdhaisha.GuiWnin = 0;
                    tkdhaisha.NippoKbn = 1;
                    tkdhaisha.YouTblSeq = 0;
                    tkdhaisha.YouKataKbn = 9;
                    tkdhaisha.SyaRyoUnc = 0;
                    tkdhaisha.SyaRyoSyo = 0;
                    tkdhaisha.SyaRyoTes = 0;
                    tkdhaisha.YoushaUnc = 0;
                    tkdhaisha.YoushaSyo = 0;
                    tkdhaisha.YoushaTes = 0;
                    tkdhaisha.PlatNo = "";
                    tkdhaisha.UkeJyKbnCd = 99;
                    tkdhaisha.SijJoKbn1 = 99;
                    tkdhaisha.SijJoKbn2 = 99;
                    tkdhaisha.SijJoKbn3 = 99;
                    tkdhaisha.SijJoKbn4 = 99;
                    tkdhaisha.SijJoKbn5 = 99;
                    tkdhaisha.RotCdSeq = 0;
                    tkdhaisha.BikoTblSeq = 0;
                    tkdhaisha.HaiCom = "";
                    tkdhaisha.SiyoKbn = 1;
                    tkdhaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdhaisha.UpdTime = DateTime.Now.ToString("HHmmss");
                    tkdhaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdhaisha.UpdPrgId = "KU0100";
                    listTkdHaisha.Add(tkdhaisha);
                    i++;
                }
                int x = listTkdHaisha.Count;
                int rowdriver = 0;
                for (int j = 0; j < request.ScheduleFormData.BusdriverNum; j++)
                {

                    if (rowdriver == x)
                    {
                        rowdriver = 0;
                        listTkdHaisha[rowdriver].DrvJin++;
                    }
                    else if (j > x)
                    {
                        listTkdHaisha[rowdriver].DrvJin++;
                    }
                    else
                    {
                        listTkdHaisha[j].DrvJin++;
                    }
                    rowdriver++;
                }

                int rowguide = 0;
                for (int j = 0; j < request.ScheduleFormData.BusGuideNum; j++)
                {

                    if (rowguide == x)
                    {
                        rowguide = 0;
                        listTkdHaisha[rowguide].GuiSu++;
                    }
                    else if (j > x)
                    {
                        listTkdHaisha[rowguide].GuiSu++;
                    }
                    else
                    {
                        listTkdHaisha[j].GuiSu++;
                    }
                    rowguide++;
                }
                return listTkdHaisha;
            }
        }
    }
}
