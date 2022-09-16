using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class AppointmentList
    {
        public int SyainCdSeq { get; set; }
        public int KankSyainCdSeq { get; set; }
        public List<int> KankSya { get; set; }
        public int DataType { get; set; }
        public int DisplayType { get; set; }
        public string Text { get; set; }
        public LabelList Typelabel { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime StartDateInDatetimeType { get; set; }
        public DateTime EndDateInDatetimeType { get; set; }
        public DateTime StartDateDisplay { get; set; }
        public DateTime EndDateDisplay { get; set; }
        public int AllDayKbn { get; set; }
        public int KuriKbn { get; set; }
        public string Description { get; set; }
        public bool AllDay { get; set; }
        public int IsPublic { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public YoteiInfo YoteiInfo { get; set; }
        public KinKyuInfo KinKyuInfo { get; set; }
        public HaiinInfo HaiinInfo { get; set; }
        public BirthDayInfo BirthDayInfo { get; set; }
        public DateCommentInfo DateCommentInfo { get; set; }
        public int VacationType { get; set; }
        public List<int> Staffs { get; set; }
        public List<int> ScheduleLabel { get; set; }
        public int ScheduleId { get; set; }
        public bool IsGroupData { get; set; }
        public int CalendarSeq { get; set; }
        public string Syainnm { get; set; }
        public string ScheduleIdMobileDP { get; set; }
        public List<LabelList> AttachedLable { get; set; }
        public string DetailAppoitment { get; set; }
        public string SyainnmDisplay { get; set; }
        public DateTime StartTargetedDate { get; set; }
        public DateTime EndTargetedDate { get; set; }
        public bool isStaff { get; set; }
        public AppointmentList()
        {

        }
        public AppointmentList(AppointmentList data)
        {
            SyainCdSeq = data.SyainCdSeq;
            KankSyainCdSeq = data.KankSyainCdSeq;
            KankSya = data.KankSya;
            DataType = data.DataType;
            DisplayType = data.DisplayType;
            Text = data.Text;
            Typelabel = data.Typelabel;
            StartDate = data.StartDate;
            EndDate = data.EndDate;
            StartDateInDatetimeType = data.StartDateInDatetimeType;
            EndDateInDatetimeType = data.EndDateInDatetimeType;
            StartDateDisplay = data.StartDateDisplay;
            EndDateDisplay = data.EndDateDisplay;
            AllDayKbn = data.AllDayKbn;
            KuriKbn = data.KuriKbn;
            Description = data.Description;
            AllDay = data.AllDay;
            IsPublic = data.IsPublic;
            RecurrenceRule = data.RecurrenceRule;
            RecurrenceException = data.RecurrenceException;
            YoteiInfo = data.YoteiInfo;
            KinKyuInfo = data.KinKyuInfo;
            HaiinInfo = data.HaiinInfo;
            BirthDayInfo = data.BirthDayInfo;
            DateCommentInfo = data.DateCommentInfo;
            VacationType = data.VacationType;
            Staffs = data.Staffs;
            ScheduleLabel = data.ScheduleLabel;
            ScheduleId = data.ScheduleId;
            IsGroupData = data.IsGroupData;
            CalendarSeq = data.CalendarSeq;
            Syainnm = data.Syainnm;
            ScheduleIdMobileDP = data.ScheduleIdMobileDP;
            AttachedLable = data.AttachedLable;
            DetailAppoitment = data.DetailAppoitment;
            SyainnmDisplay = data.SyainnmDisplay;
            StartTargetedDate = data.StartTargetedDate;
            EndTargetedDate = data.EndTargetedDate;
            isStaff = data.isStaff;
    }
    }

    public class YoteiInfo
    {
        public int YoteiSeq { get; set; }
        public int CalendarSeq { get; set; }
        public int CreatorCdSeq { get; set; }
        public string CreatorNm { get; set; }
        public int YoteiType { get; set; }
        public int YoteiTypeKbn { get; set; }
        public int KinKyuCdSeq { get; set; }
        public int KinKyuTblCdSeq { get; set; }
        public List<LabelList> TukiLabelArray { get; set; }
        public string Note { get; set; }
        public string YoteiShoKbn { get; set; }
        public int ShoSyainCdSeq { get; set; }
        public string ShoSyainNm { get; set; }
        public string ShoDateTime { get; set; }
        public string ShoRejBiko { get; set; }
        public List<ParticipantByTime> ParticipantByTimeArray { get; set; }
        public int isPublic { get; set; }
        
    }

    public class KinKyuInfo
    {
        public int KinKyuCdSeq { get; set; }
        public int KinKyuTblCdSeq { get; set; }
        public string SyainNm { get; set; }
        public string KinkyuKbnNm { get; set; }
        public string KinkyuNm { get; set; }
        public string BikoNm { get; set; }
        public int ReadKbn { get; set; }
    }

    public class HaiinInfo
    {
        public string UkeNo { get; set; }
        public int UkeCd { get; set; }
        public int UnkRen { get; set; }
        public int TeiDanNo { get; set; }
        public int BunkRen { get; set; }
        public int HaiInRen { get; set; }
        public string SyainNm { get; set; }
        public string Gosya { get; set; }
        public string SyaSyuNm { get; set; }
        public string HaiSInfo { get; set; }
        public string TouInfo { get; set; }
        public string DantaNm { get; set; }
        public string IkNm { get; set; }
        public int TeiCnt { get; set; }
        public int ReadKbn { get; set; }
        public List<AttachFile> AttachFileArray { get; set; }
    }

    public class DateCommentInfo
    {
        public string CalenYmd { get; set; }
        public string CalenCom { get; set; }
    }

    public class BirthDayInfo
    {
        public string SyainCd { get; set; }
        public string SyainNm { get; set; }
        public string BirthYmd { get; set; }
        public int SyainSyokumuKbn { get; set; }
    }

    public class AttachFile
    {
        public int FileNo { get; set; }
        public string FileName { get; set; }
        public string FileLink { get; set; }
    }

    public class LabelList
    {
        public int LabelType { get; set; }
        public string LabelText { get; set; }
    }
    public class ParticipantByTime
    {
        public string YoteiSYmd { get; set; }
        public string YoteiSTime { get; set; }
        public int KuriKbn { get; set; }
        public List<Participant> ParticipantArray { get; set; }
    }
    public class Participant
    {
        public int SyainCdSeq { get; set; }
        public string SyainNm { get; set; }
        public int AcceptKbn { get; set; }
    }

    public class AppointmentLabel
    {
        public int CodeKbnSeq { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class PlanType
    {
        public int CodeKbnSeq { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
