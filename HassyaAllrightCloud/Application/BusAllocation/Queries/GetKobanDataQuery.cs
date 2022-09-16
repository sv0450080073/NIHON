using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetKobanDataQuery : IRequest<Dictionary<CommandMode, List<TkdKoban>>>
    {
        public string Ukeno { get; set; }
        public short UnkRen { get; set; }
        public short BunkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public short TaiDanNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int CompanyCdSeq { get; set; }

        public class Handler : IRequestHandler<GetKobanDataQuery, Dictionary<CommandMode, List<TkdKoban>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetKobanDataQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetKobanDataQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<Dictionary<CommandMode, List<TkdKoban>>> Handle(GetKobanDataQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<CommandMode, List<TkdKoban>> result = new Dictionary<CommandMode, List<TkdKoban>>();
                    List<TkdKoban> addNewKobanList = new List<TkdKoban>();
                    List<TkdKoban> removeKobanList = new List<TkdKoban>();
                    List<TkdKoban> updateKobanList = new List<TkdKoban>();
                    var kobanDataList = _context.TkdKoban.Where(x => x.UkeNo == request.Ukeno
                                                       && x.UnkRen == request.UnkRen
                                                       && x.TeiDanNo == request.TaiDanNo
                                                       && x.BunkRen == request.BunkRen).ToList();
                    var haishaDataOld = _context.TkdHaisha.Where(x => x.UkeNo == request.Ukeno
                                                       && x.UnkRen == request.UnkRen
                                                       && x.TeiDanNo == request.TaiDanNo
                                                       && x.BunkRen == request.BunkRen
                                                       && x.SyaSyuRen == request.SyaSyuRen).FirstOrDefault();
                    DateTime SyukoOld = ParseStringToDateTime(haishaDataOld.SyuKoYmd, haishaDataOld.SyuKoTime);
                    DateTime KikOld = ParseStringToDateTime(haishaDataOld.KikYmd, haishaDataOld.KikTime);
                    int compareSyukoTime = DateTime.Compare(SyukoOld, ParseTimeStringToDate(request.StartDate, request.StartTime));
                    int compareKikTime = DateTime.Compare(KikOld, ParseTimeStringToDate(request.EndDate, request.EndTime));
                    if (kobanDataList.Count != 0 && (compareSyukoTime != 0 || compareKikTime != 0))
                    {
                        var unkobiItem = _context.TkdUnkobi.Where(x => x.UkeNo == request.Ukeno && x.UnkRen == request.UnkRen).FirstOrDefault();
                        removeKobanList = kobanDataList;
                        var employeeCode = kobanDataList.GroupBy(x => new { x.SyainCdSeq, x.UkeNo , x.UnkRen, x.BunkRen,x.TeiDanNo}).Select(x => x.First()).ToList();
                        foreach (var item in employeeCode)
                        {
                            var dataKobanNew = await HandleInsertKoban(kobanDataList, request, item.SyainCdSeq, unkobiItem);
                            addNewKobanList.AddRange(dataKobanNew);
                        }
                    }
                    result.Add(CommandMode.Create, addNewKobanList);
                    result.Add(CommandMode.Update, updateKobanList);
                    result.Add(CommandMode.Delete, removeKobanList);
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private async Task<List<TkdKoban>> HandleInsertKoban(List<TkdKoban> kobanDataOldList, GetKobanDataQuery request, int syainCdSeq, TkdUnkobi unkobiItem)
            {
                List<TkdKoban> tkdKobanList = new List<TkdKoban>();
                List<VPM_SyuTaikinCalculationTime> vpm_SyuTaikinCalculationTimeList = await GetSyuTaikinCalculationTime();
                int syukinCalculationTimeMinute = 0;
                int taikinCalculationTimeMinutes = 0;
                var dataOldList = kobanDataOldList.Where(x => x.SyainCdSeq == syainCdSeq).ToList();
                double totaldate = (request.EndDate - request.StartDate).TotalDays;
                for (DateTime date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
                {
                    var dataOldItem = dataOldList.Where(x => x.UnkYmd == ParseDateToString(date)).FirstOrDefault();
                    DateTime SyukinDate = ParseTimeStringToDate(date, "0000");
                    DateTime TaikinDate = ParseTimeStringToDate(date, "2359");
                    if (date == request.StartDate)
                    {
                        SyukinDate = ParseTimeStringToDate(request.StartDate, request.StartTime);
                    }
                    if (date == request.EndDate)
                    {
                        TaikinDate = ParseTimeStringToDate(request.EndDate, request.EndTime);
                    }
                    TkdKoban newitem = new TkdKoban();
                    newitem.UnkYmd = ParseDateToString(date);
                    newitem.SyainCdSeq = syainCdSeq;
                    newitem.KouBnRen = 0;
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = 0;
                    newitem.UkeNo = request.Ukeno;
                    newitem.UnkRen = request.UnkRen;
                    newitem.SyaSyuRen = request.SyaSyuRen;
                    newitem.TeiDanNo = request.TaiDanNo;
                    newitem.BunkRen = request.BunkRen;
                    newitem.RotCdSeq = 0;
                    newitem.RenEigCd = 0;
                    newitem.SigySyu = 0;
                    newitem.KitYmd = "";
                    newitem.SigyKbn = 0;
                    newitem.SiyoKbn = 1;
                    newitem.SigyCd = "";
                    newitem.KouZokPtnKbn = SetKouZokPtnKbn(newitem.UnkYmd, unkobiItem);
                    syukinCalculationTimeMinute = Convert.ToInt32(vpm_SyuTaikinCalculationTimeList.Where(t => t.CompanyCdSeq == request.CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault()?.SyukinCalculationTimeMinutes);
                    taikinCalculationTimeMinutes = Convert.ToInt32(vpm_SyuTaikinCalculationTimeList.Where(t => t.CompanyCdSeq == request.CompanyCdSeq && t.KouZokPtnKbn == newitem.KouZokPtnKbn).FirstOrDefault()?.TaikinCalculationTimeMinutes);
                    newitem.SyukinYmd = ParseDateToString(SyukinDate.AddMinutes(-syukinCalculationTimeMinute));
                    newitem.TaikinYmd = ParseDateToString(TaikinDate.AddMinutes(taikinCalculationTimeMinutes));
                    newitem.SyukinTime = SyukinDate.AddMinutes(-syukinCalculationTimeMinute).ToString("HHmm");
                    newitem.TaiknTime = TaikinDate.AddMinutes(taikinCalculationTimeMinutes).ToString("HHmm");
                    newitem.FuriYmd = "";
                    newitem.RouTime = "";
                    newitem.KouStime = "";
                    newitem.TaikTime = "";
                    newitem.KyuKtime = "";
                    newitem.JitdTime = "";
                    newitem.ZangTime = "";
                    newitem.UsinyTime = "";
                    newitem.Syukinbasy = dataOldItem != null ? dataOldItem.Syukinbasy : "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = dataOldItem != null ? dataOldItem.TaiknBasy : "";
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("HHmm");
                    newitem.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    newitem.UpdPrgId = "KU0600";
                    tkdKobanList.Add(newitem);
                }
                return tkdKobanList;
            }
            private async Task<List<VPM_SyuTaikinCalculationTime>> GetSyuTaikinCalculationTime()
            {
                try
                {
                    List<VPM_SyuTaikinCalculationTime> result = new List<VPM_SyuTaikinCalculationTime>();
                    result = (from SyuTaikinCalculationTime in _context.VpmSyuTaikinCalculationTime
                              select new VPM_SyuTaikinCalculationTime()
                              {
                                  CompanyCdSeq = SyuTaikinCalculationTime.CompanyCdSeq,
                                  SyugyoKbn = SyuTaikinCalculationTime.SyugyoKbn,
                                  KouZokPtnKbn = SyuTaikinCalculationTime.KouZokPtnKbn,
                                  SyukinCalculationTimeMinutes = SyuTaikinCalculationTime.SyukinCalculationTimeMinutes,
                                  TaikinCalculationTimeMinutes = SyuTaikinCalculationTime.TaikinCalculationTimeMinutes
                              }).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            private byte SetKouZokPtnKbn(string unkYmd, TkdUnkobi unkobiItem)
            {
                byte kouZokPtnKbn = 99;
                if (unkobiItem.SyukoYmd.CompareTo(unkobiItem.KikYmd) == 0)
                {
                    kouZokPtnKbn = 1;
                    return kouZokPtnKbn;
                }
                if ((unkobiItem.UnkoJkbn != 3 && unkobiItem.UnkoJkbn != 4) && (unkobiItem.SyukoYmd.CompareTo(unkobiItem.KikYmd) != 0) && (unkobiItem.SyukoYmd.CompareTo(unkYmd) == 0))
                {
                    kouZokPtnKbn = 2;
                    return kouZokPtnKbn;
                }
                if ((unkobiItem.SyukoYmd.CompareTo(unkobiItem.KikYmd) != 0) && (unkobiItem.SyukoYmd.CompareTo(unkYmd) != 0) && (unkobiItem.KikYmd.CompareTo(unkYmd) != 0))
                {
                    kouZokPtnKbn = 3;
                    return kouZokPtnKbn;
                }
                if ((unkobiItem.UnkoJkbn != 3 && unkobiItem.UnkoJkbn != 4) && (unkobiItem.HaiSymd.CompareTo(unkobiItem.TouYmd) != 0) && (unkobiItem.KikYmd.CompareTo(unkYmd) == 0))
                {
                    kouZokPtnKbn = 4;
                    return kouZokPtnKbn;
                }
                if ((unkobiItem.UnkoJkbn == 3 || unkobiItem.UnkoJkbn == 4) && (unkobiItem.SyukoYmd.CompareTo(unkobiItem.KikYmd) != 0) && (unkobiItem.SyukoYmd.CompareTo(unkYmd) == 0))
                {
                    kouZokPtnKbn = 5;
                    return kouZokPtnKbn;
                }
                if ((unkobiItem.UnkoJkbn == 3 || unkobiItem.UnkoJkbn == 4) && (unkobiItem.SyukoYmd.CompareTo(unkobiItem.KikYmd) != 0) && (unkobiItem.KikYmd.CompareTo(unkYmd) == 0))
                {
                    kouZokPtnKbn = 6;
                    return kouZokPtnKbn;
                }
                return kouZokPtnKbn;
            }
            private string ParseDateToString(DateTime valueDate)
            {
                return valueDate.ToString("yyyyMMdd");
            }
            private string ParseTimeToString(TimeSpan valueTime)
            {
                return valueTime.ToString("HHmm");
            }
            private DateTime ParseTimeStringToDate(DateTime dateTime, string Time)
            {
                DateTime result;
                DateTime.TryParseExact(ParseDateToString(dateTime) + Time, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
                return result;
            }
            private DateTime ParseStringToDateTime(string dateValue, string timeValue)
            {
                DateTime result;
                DateTime.TryParseExact(dateValue + timeValue, "yyyyMMddHHmm", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
                return result;
            }
        }
    }
}
