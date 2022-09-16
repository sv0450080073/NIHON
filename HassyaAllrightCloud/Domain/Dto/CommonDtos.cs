using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CustomerModel
    {
        public CustomerComponentGyosyaData SelectedGyosya { get; set; }
        public CustomerComponentTokiskData SelectedTokisk { get; set; }
        public CustomerComponentTokiStData SelectedTokiSt { get; set; }
    }

    public class DefaultCustomerData
    {
        public int? GyosyaCdSeq { get; set; }
        public int TokiskCdSeq { get; set; }
        public int TokiStCdSeq { get; set; }
    }
}
