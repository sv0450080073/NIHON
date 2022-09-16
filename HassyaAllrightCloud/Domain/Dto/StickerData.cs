using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StickerData
    {
        public int Index { get; set; }
        public string ProcessingDivision { get; set; }
        public DateTime DateOfDispatch { get; set; }
        public List<ReservationData> BookingTypes { get; set; }
        public LoadSaleBranch ReceptionOffice { get; set; }
        public LoadCustomerList CustomerFrom { get; set; }
        public LoadCustomerList CustomerTo { get; set; }
        public string CategoryName { get; set; }
        public string DisplayOrder { get; set; }        
        public string StickerUsed { get; set; }
        public string FontName { get; set; }
        public string FontStyle { get; set; }
        public string FontSize { get; set; }
        public string FontScript { get; set; }
        public int NumberOfSheets { get; set; }
        public string CarNo { get; set; }
        public string CourseNumber { get; set; }
        public string Sticker { get; set; }
    }

    public class LoadSticker
    {
        public string Organization { get; set; }
        public string CarNumber { get; set; }
        public string DeliveryTime { get; set; }
        public string Customer { get; set; }
        public string ReservationGroupName { get; set; }
        public string BasicSizeUsed { get; set; }
        public int StickerNumber { get; set; }
        public string Sticker { get; set; }
        public string StickerSize { get; set; }
        public string SideSticker { get; set; }
        public string SideUseSize { get; set; }
        public string Detail { get; set; }
    }
}
