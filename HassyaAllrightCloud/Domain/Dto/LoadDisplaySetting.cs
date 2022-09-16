using HassyaAllrightCloud.Commons.Constants;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoadDisplaySetting
    {
        public int StaffCdSeq { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public NumberWithStringDisplay DefaultDisplayType { get; set; }
        public NumberWithStringDisplay WeekStartDay { get; set; }
        public StringWithStringDisplay DayStartTime { get; set; }
        public LoadDisplaySetting()
        {
            StaffCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
            TimeZone = TimeZoneInfo.Local;
            DefaultDisplayType = new NumberWithStringDisplay() { Value = DisplaySettingConstants.DisplayTypeMonth };
            WeekStartDay = new NumberWithStringDisplay() { Value = 0 };
            DayStartTime = new StringWithStringDisplay() { Value = DisplaySettingConstants.DefaultDayStartTime };
        }

        public LoadDisplaySetting(LoadDisplaySetting Clone)
        {
            StaffCdSeq = Clone.StaffCdSeq;
            TimeZone = Clone.TimeZone;
            DefaultDisplayType = Clone.DefaultDisplayType;
            WeekStartDay = Clone.WeekStartDay;
            DayStartTime = Clone.DayStartTime;
        }
    }

    public class DisplaySettingConstants
    {
        public const string DefaultDayStartTime = "Now";
        public const int DisplayTypeMonth = 0;
        public const int DisplayTypeWeek = 1;
    }

    public class NumberWithStringDisplay
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class StringWithStringDisplay
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
