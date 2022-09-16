using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusScheduleFilter
    {
        public BusScheduleFilter()
        {
            CompanyCdSeqList = new List<int>();
            EigyoCdSeqList = new List<int>();
        }

        // tab 1
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// <see cref="Commons.Constants.LineDrawMode"/>
        /// </summary>
        public int LineDrawMode { get; set; }

        /// <summary>
        /// <see cref="Commons.Constants.GroupMode"/>
        /// </summary>
        public int GroupMode { get; set; }

        /// <summary>
        /// <see cref="Commons.Constants.DayMode"/>
        /// </summary>
        public int DayMode { get; set; }

        /// <summary>
        /// <see cref="Commons.Constants.TimeMode"/>
        /// </summary>
        public int TimeMode { get; set; }

        // tab 2
        /// <summary>
        /// <see cref="Commons.Constants.SortVehicleLineMode"/>
        /// </summary>
        public int SortVehicleLineMode { get; set; }

        /// <summary>
        /// <see cref="Commons.Constants.SortVehicleNameMode"/>
        /// </summary>
        public int SortVehicleNameMode { get; set; }

        /// <summary>
        /// <see cref="Commons.Constants.DisplayLineMode"/>
        /// </summary>
        public int DisplayLineMode { get; set; }

        /// <summary>
        /// <see cref="Commons.Constants.ViewMode"/>
        /// </summary>
        public int ViewMode { get; set; }

        // tab 3
        public List<int> CompanyCdSeqList { get; set; }
        public List<int> EigyoCdSeqList { get; set; }
        public int ReservationYoyaKbnSeq { get; set; }
    }
}
