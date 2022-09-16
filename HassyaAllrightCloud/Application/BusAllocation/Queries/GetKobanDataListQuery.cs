using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetKobanDataListQuery : IRequest<Dictionary<CommandMode, List<TkdKoban>>>
    {
       public  List<TkdHaisha> TkdHaishaList { get; set; }
        public int  CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetKobanDataListQuery, Dictionary<CommandMode, List<TkdKoban>>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Dictionary<CommandMode, List<TkdKoban>>> Handle(GetKobanDataListQuery request, CancellationToken cancellationToken)
            {              
                try
                {
                    var dataHaiShaList = request.TkdHaishaList;
                    Dictionary<CommandMode, List<TkdKoban>> result = new Dictionary<CommandMode, List<TkdKoban>>();
                    List<TkdKoban> addNewKobanList = new List<TkdKoban>();
                    List<TkdKoban> removeKobanList = new List<TkdKoban>();
                    List<TkdKoban> updateKobanList = new List<TkdKoban>();

                    foreach (var item in dataHaiShaList)
                    {
                        var kobanDataList = _context.TkdKoban.Where(x => x.UkeNo == item.UkeNo
                                                     && x.UnkRen == item.UnkRen
                                                     && x.TeiDanNo == item.TeiDanNo
                                                     && x.BunkRen == item.BunkRen).ToList();
                        if (kobanDataList.Count != 0)
                        {
                            var unkobiItem = _context.TkdUnkobi.Where(x => x.UkeNo == item.UkeNo && x.UnkRen == item.UnkRen).FirstOrDefault();
                            removeKobanList.AddRange(kobanDataList);
                            List<int> employeeCode = kobanDataList.Distinct().Select(x => x.SyainCdSeq).ToList();
                            for (int i = 0; i < employeeCode.Count; i++)
                            {
                                var dataKobanNew = await HandleInsertKoban(kobanDataList, item, employeeCode[i], unkobiItem, request);
                                addNewKobanList.AddRange(dataKobanNew);
                            }
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

            private async Task<List<TkdKoban>> HandleInsertKoban(List<TkdKoban> kobanDataOldList, TkdHaisha tkdHaisha, int syainCdSeq, TkdUnkobi unkobiItem, GetKobanDataListQuery request)
            {
                List<TkdKoban> tkdKobanList = new List<TkdKoban>();
                List<VPM_SyuTaikinCalculationTime> vpm_SyuTaikinCalculationTimeList = await GetSyuTaikinCalculationTime();
                int syukinCalculationTimeMinute = 0;
                int taikinCalculationTimeMinutes = 0;
                var kobandataOldList = kobanDataOldList.Where(x => x.SyainCdSeq == syainCdSeq).ToList();
                DateTime StartDate = ParseStringToDate(tkdHaisha.SyuKoYmd);
                string StartTime = tkdHaisha.SyuKoTime;
                DateTime EndDate = ParseStringToDate(tkdHaisha.KikYmd);
                string EndTime = tkdHaisha.KikTime;
                for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
                {
                    var listKobanOldItems = kobandataOldList.Where(x => x.UnkYmd == ParseDateToString(date)).ToList();
                    var kobanDataOldItem = listKobanOldItems.FirstOrDefault();
                    DateTime SyukinDate = ParseTimeStringToDate(date, "0000");
                    DateTime TaikinDate = ParseTimeStringToDate(date, "2359");
                    if (date == StartDate)
                    {
                        SyukinDate = ParseTimeStringToDate(StartDate, StartTime);
                    }
                    if (date == EndDate)
                    {
                        TaikinDate = ParseTimeStringToDate(EndDate, EndTime);
                    }
                    TkdKoban newitem = new TkdKoban();
                    newitem.UnkYmd = ParseDateToString(date);
                    newitem.SyainCdSeq = syainCdSeq;
                    //newitem.KouBnRen = (short)(1 - kobanDataOldItem?.KouBnRen ?? 0);
                    newitem.KouBnRen = (short)(listKobanOldItems == null ? 0 : (listKobanOldItems.Max(k => k.KouBnRen) + 1));
                    newitem.HenKai = 0;
                    newitem.SyugyoKbn = 1;
                    newitem.KinKyuTblCdSeq = 0;
                    newitem.UkeNo = tkdHaisha.UkeNo;
                    newitem.UnkRen = tkdHaisha.UnkRen;
                    newitem.SyaSyuRen = tkdHaisha.SyaSyuRen;
                    newitem.TeiDanNo = tkdHaisha.TeiDanNo;
                    newitem.BunkRen = tkdHaisha.BunkRen;
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
                    newitem.Syukinbasy = kobanDataOldItem != null ? kobanDataOldItem.Syukinbasy : "";
                    newitem.SsinTime = "";
                    newitem.BikoNm = "";
                    newitem.TaiknBasy = kobanDataOldItem != null ? kobanDataOldItem.TaiknBasy : "";
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
            private DateTime ParseStringToDate(string valueStr)
            {
                DateTime result;
                DateTime.TryParseExact(valueStr, "yyyyMMdd", new CultureInfo("ja-JP"), DateTimeStyles.None, out result);
                return result;
            }         
        }
    }
}
