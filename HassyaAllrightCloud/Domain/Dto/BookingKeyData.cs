using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BookingKeyData
    {
        /// <summary>
        /// Mark this item is selected. Default values is false
        /// </summary>
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// <see cref="TkdUnkobi.UkeNo"/>
        /// </summary>
        public string UkeNo { get; set; }
        /// <summary>
        /// <see cref="TkdUnkobi.UnkRen"/>
        /// </summary>
        public int UnkRen { get; set; }

        public BookingKeyData()
        {

        }

        public BookingKeyData(string ukeNo, int unkRen, bool isSelected = false)
        {
            UkeNo = ukeNo;
            UnkRen = unkRen;
            IsSelected = isSelected;
        }
    }
}
