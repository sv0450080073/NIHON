using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class SelectedPaymentAddressModel
    {
        public int Id { get; set; }
        public string CSeiGyosyaCd { get; set; }
        public string CSeiCd { get; set; }
        public string CSeiSitenCd { get; set; }
        public string CSeiCdSeq { get; set; }
        public string CSeiSitenCdSeq { get; set; }
        public string CGyosyaNm { get; set; }
        public string CSeiRyakuNm { get; set; }
        public string CSitRyakuNm { get; set; }
        public string CSeiSiyoStaYmd { get; set; }
        public string CSeiSiyoEndYmd { get; set; }
        public string CSitSiyoStaYmd { get; set; }
        public string CSitSiyoEndYmd { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Text
        {
            get
            {
                return string.Format("{0}-{1}-{2}-{3} : {4}", Int32.Parse(CSeiGyosyaCd).ToString("D3"), Int32.Parse(CSeiCd).ToString("D4"), Int32.Parse(CSeiSitenCd).ToString("D4"), CSeiRyakuNm, CSitRyakuNm);
            }
        }
    }
}
