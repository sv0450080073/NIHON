using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace HassyaAllrightCloud.IService
{
    public interface IDisplaySettingService
    {
        Task<LoadDisplaySetting> Get(int StaffCdSeq);
        Task<bool> Save(LoadDisplaySetting DisplaySettingForm);

        Task<List<AppointmentLabel>> GetAppointmentLabel(int tenant);
        Task<List<PlanType>> GetPlanType(int tenant);
    }

    public class DisplaySettingService : IDisplaySettingService
    {
        private readonly KobodbContext _dbContext;
        public DisplaySettingService(KobodbContext context)
        {
            _dbContext = context;
        }

        public async Task<LoadDisplaySetting> Get(int StaffCdSeq)
        {
            LoadDisplaySetting Result = new LoadDisplaySetting();
            var userSetVals = await (
                from u in _dbContext.VpmUserSetItm
                join uv in _dbContext.VpmUserSetItmVal
                on new { key1 = u.ItemSeq, key2 = StaffCdSeq } equals new { key1 = uv.ItemSeq, key2 = uv.SyainCdSeq } into uuv
                from uuvTemp in uuv.DefaultIfEmpty()
                where(u.ItemCd == "TIMEZONE" || u.ItemType == "SCHEDULE")
                select new UserSettingItem()
                {
                    ItemCd = u.ItemCd,
                    SetVal = string.IsNullOrEmpty(uuvTemp.SetVal) ? u.DefaultVal : uuvTemp.SetVal
                }).ToListAsync();
            if (userSetVals != null)
            {
                Result.StaffCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                if (TimeZoneInfo.GetSystemTimeZones().Select(tz => tz.Id).Contains(userSetVals.Where(x => x.ItemCd.Equals("TIMEZONE")).FirstOrDefault()?.SetVal))
                {
                    Result.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(userSetVals.Where(x => x.ItemCd.Equals("TIMEZONE")).FirstOrDefault()?.SetVal);
                }
                else
                {
                    TZConvert.TryWindowsToIana(userSetVals.Where(x => x.ItemCd.Equals("TIMEZONE")).FirstOrDefault()?.SetVal, out string IanaName);
                    TZConvert.TryIanaToWindows(userSetVals.Where(x => x.ItemCd.Equals("TIMEZONE")).FirstOrDefault()?.SetVal, out string WindowsName);
                    string TimeZoneName = !string.IsNullOrEmpty(IanaName) ? IanaName : WindowsName;
                    if (!(string.IsNullOrEmpty(TimeZoneName)))
                    {
                        Result.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName);
                    }
                }
                var initDisplaySetting = userSetVals.Where(x => x.ItemCd.Equals("INITDISPLAYTYPE")).FirstOrDefault().SetVal;
                var initSDOWSetting = userSetVals.Where(x => x.ItemCd.Equals("WEEKSTARTDAY")).FirstOrDefault().SetVal;
                var initDisplayTimeSetting = userSetVals.Where(x => x.ItemCd.Equals("DISPLAYTIME")).FirstOrDefault().SetVal;
                Result.DefaultDisplayType = new NumberWithStringDisplay() { Value = Int32.Parse(string.IsNullOrEmpty(initDisplaySetting) ? "0" : initDisplaySetting) };
                Result.WeekStartDay = new NumberWithStringDisplay() { Value = Int32.Parse(string.IsNullOrEmpty(initSDOWSetting) ? "0" : initSDOWSetting) };
                Result.DayStartTime = new StringWithStringDisplay() { Value = string.IsNullOrEmpty(initDisplayTimeSetting) ? "0" : initDisplayTimeSetting };
            }
            return Result;
        }

        public async Task<List<AppointmentLabel>> GetAppointmentLabel(int tenant)
        {
            var codeSy = await (
                from c in _dbContext.VpmCodeSy
                where c.CodeSyu == "YOTEILABKBN" && c.KanriKbn != 1
                select c.CodeSyu
                ).ToListAsync();
            return await (
                from c in _dbContext.VpmCodeKb
                where c.CodeSyu == "YOTEILABKBN" && c.SiyoKbn == 1 && c.TenantCdSeq == (codeSy.Count() > 0 ? tenant : 0)
                select new AppointmentLabel()
                {
                    CodeKbnSeq = c.CodeKbnSeq,
                    Id = c.CodeKbn,
                    Text = c.RyakuNm
                }
                ).ToListAsync();
        }

        public async Task<List<PlanType>> GetPlanType(int tenant)
        {
            var codeSy = await(
                from c in _dbContext.VpmCodeSy
                where c.CodeSyu == "YOTEITYPE" && c.KanriKbn != 1
                select c.CodeSyu
                ).ToListAsync();
            return await(
                from c in _dbContext.VpmCodeKb
                where c.CodeSyu == "YOTEITYPE" && c.SiyoKbn == 1 && c.TenantCdSeq == (codeSy.Count() > 0 ? tenant : 0)
                select new PlanType()
                {
                    CodeKbnSeq = c.CodeKbnSeq,
                    Id = c.CodeKbn,
                    Text = c.RyakuNm
                }
                ).ToListAsync();
        }

        public async Task<bool> Save(LoadDisplaySetting DisplaySettingForm)
        {
            TkdSchHyoSet DisplaySetting = await (
                from s in _dbContext.TkdSchHyoSet
                where s.SyainCdSeq == DisplaySettingForm.StaffCdSeq
                select s).FirstOrDefaultAsync();
            bool IsAdd = false;
            if (DisplaySetting == null)
            {
                IsAdd = true;
                DisplaySetting = new TkdSchHyoSet();
                DisplaySetting.SyainCdSeq = DisplaySettingForm.StaffCdSeq;
            }
            DisplaySetting.TimeZn = DisplaySettingForm.TimeZone.Id;
            DisplaySetting.DefltDispTyp = (byte)DisplaySettingForm.DefaultDisplayType.Value;
            DisplaySetting.WeekStrDay = (byte)DisplaySettingForm.WeekStartDay.Value;
            DisplaySetting.StrTimeOfDay = DisplaySettingForm.DayStartTime.Value == DisplaySettingConstants.DefaultDayStartTime ?
                DisplaySettingForm.DayStartTime.Value : string.Format("{0:D2}0000", int.Parse(DisplaySettingForm.DayStartTime.Value));
            DisplaySetting.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            DisplaySetting.UpdTime = DateTime.Now.ToString("HHmmss");
            DisplaySetting.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
            DisplaySetting.UpdPrgId = Common.UpdPrgId;

            using (IDbContextTransaction dbTran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (IsAdd)
                    {
                        _dbContext.TkdSchHyoSet.Add(DisplaySetting);
                    }
                    else
                    {
                        _dbContext.TkdSchHyoSet.Update(DisplaySetting);
                    }
                    await _dbContext.SaveChangesAsync();
                    dbTran.Commit();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    dbTran.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    return false;
                }
            }
        }
    }
}
