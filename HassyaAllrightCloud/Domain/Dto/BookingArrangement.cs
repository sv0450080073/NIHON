using DevExpress.Web.Internal;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Globalization;
using System.Linq.Expressions;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ArrangementCar
    {
        public ArrangementCar()
        {
            UnkRen = 1;
            BunkRen = 1;
        }

        public short UnkRen { get; set; }
        public short SyaSyuRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        public bool IsCut { get; set; }
        public string Gosya { get; set; }
        public bool IsCommon { get => TeiDanNo == 0; }
        public short TehRenMax { get; set; }

        #region properties for schedule
        public bool IsPreviousDate { get; set; }
        public bool IsAfterDate { get; set; }
        public string StartDateString { get; set; }
        public DateTime StartDate
        {
            get
            {
                try
                {
                    return DateTime.ParseExact(StartDateString, "yyyyMMdd", null, DateTimeStyles.None);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public string EndDateString { get; set; }
        public DateTime EndDate
        {
            get
            {
                try
                {
                    return DateTime.ParseExact(EndDateString, "yyyyMMdd", null, DateTimeStyles.None);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        public string Text
        {
            get
            {
                if (IsCommon)
                {
                    return "共通";
                }
                if (IsCut)
                {
                    return string.Format("{0} 号車ー{1:D2}", Gosya, BunkRen);
                }
                return string.Format("{0} 号車", Gosya);
            }
        }
    }

    public class BookingArrangementData
    {
        public BookingArrangementData()
        {
            ArrivalTime = new MyTime(0, 0);
            DepartureTime = new MyTime(0, 0);
            EditState = FormEditState.None;
        }

        public short No { get; set; } // === TehRen
        public ScheduleSelectorModel Schedule { get; set; }
        public ArrangementType SelectedArrangementType { get; set; }
        public ArrangementCode SelectedArrangementCode { get; set; }
        public ArrangementPlaceType SelectedArrangementPlaceType { get; set; }
        public ArrangementLocation SelectedArrangementLocation { get; set; }
        public string LocationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string InchargeStaff { get; set; }
        public string Comment { get; set; }
        public MyTime ArrivalTime { get; set; }
        public MyTime DepartureTime { get; set; }
        public bool Editing { get; set; }
        public FormEditState EditState { get; set; }

        #region property to filter by selected car
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }
        #endregion

        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
    }

    public class ArrangementType
    {
        public static ArrangementType CreateDefault()
        {
            var result = new ArrangementType();
            result.TypeCode = 0;
            result.TypeName = "指定なし";
            return result;
        }

        public string TypeName { get; set; }
        public int TypeCode { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0}", TypeName);
            }
        }
    }

    public class ArrangementCode
    {
        public static ArrangementCode CreateDefault()
        {
            var result = new ArrangementCode();
            result.CodeKbnSeq = 0;
            result.CodeKbnName = "指定なし";
            return result;
        }

        public int CodeKbnSeq { get; set; }
        public string CodeKbnName { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0}", CodeKbnName);
            }
        }
    }

    public class ArrangementPlaceType
    {
        public static ArrangementPlaceType CreateDefault()
        {
            var result = new ArrangementPlaceType();
            result.CodeKbnSeq = 0;
            result.CodeKbnName = "指定なし";
            return result;
        }

        public string CodeKbn { get; set; }
        public string CodeKbnName { get; set; }
        public int CodeKbnSeq { get; set; }

        public string Text
        {
            get
            {
                if(CodeKbnSeq == 0)
                {
                    return string.Format("{0}", CodeKbnName);
                }
                return string.Format("{0:D2}:{1}", CodeKbn, CodeKbnName);
            }
        }
    }

    public class ArrangementLocation
    {
        public int BasyoKenCdSeq { get; set; }
        public int BasyoMapCdSeq { get; set; }
        public string BasyoMapCd { get; set; }
        public int BunruiCdSeq { get; set; }
        public string BasyoNm { get; set; }

        public string Text
        {
            get
            {
                return BasyoKenCdSeq>0?string.Format("{0}:{1}", BasyoMapCd, BasyoNm):"";
            }
        }
    }
}
