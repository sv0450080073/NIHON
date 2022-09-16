using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using System;
using System.Collections.Generic;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class VenderRequestFormData
    {
        public int _ukeCdFrom { get; set; } = -1;
        public string UkeCdFrom
        {
            get
            {
                if (_ukeCdFrom == -1)
                    return string.Empty;
                return _ukeCdFrom.ToString("D10");
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _ukeCdFrom = -1;
                }
                else if (value.IsIntLargerThanZero())
                {
                    _ukeCdFrom = int.Parse(value);
                }
            }
        }

        public int _ukeCdTo { get; set; } = -1;
        public string UkeCdTo
        {
            get
            {
                if (_ukeCdTo == -1)
                    return string.Empty;
                return _ukeCdTo.ToString("D10");
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _ukeCdTo = -1;
                }
                else if (value.IsIntLargerThanZero())
                {
                    _ukeCdTo = int.Parse(value);
                }
            }
        }

        public ReservationClassComponentData BookingTypeStart { get; set; }
        public ReservationClassComponentData BookingTypeEnd { get; set; }
        public List<ReservationClassComponentData> BookingTypes { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public LoadSaleBranch Branch { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaFrom { get; set; }
        public CustomerComponentTokiskData SelectedTokiskFrom { get; set; }
        public CustomerComponentTokiStData SelectedTokiStFrom { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaTo { get; set; }
        public CustomerComponentTokiskData SelectedTokiskTo { get; set; }
        public CustomerComponentTokiStData SelectedTokiStTo { get; set; }
        public OutputInstruction OutputSetting { get; set; } = OutputInstruction.Preview;
    }
}
