using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TransportationContractFormData : ICloneable
    {

        public int PrintMode { get; set; }
        public int OutputUnit { get; set; }
        public bool IsUpdateExportDate { get; set; }
        public int DateTypeContract { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int OutputSelection { get; set; }
        public int YearlyContract { get; set; }
        // 受付営業所
        public SaleBranchData UketsukeEigyoJo { get; set; }
        
        // 営業担当
        public StaffsData EigyoTantoSha { get; set; }

        // 入力担当
        public StaffsData InpSyainCd { get; set; }
        // 受付番号
        public string UkeNumber { get; set; }
        // 運行連番
        public string UnkRen { get; set; } = "1";

        // 得意先
        public CustomerComponentGyosyaData Gyosya { get; set; }
        public CustomerComponentTokiskData TokuiSaki { get; set; }
        public CustomerComponentTokiStData TokuiSiten { get; set; }
        public List<BookingTypeData> YoyakuKbnList { get; set; }

        public TransportationContractFormData()
        {

        }

        public TransportationContractFormData(TransportationContractFormData transportationContractFormData)
        {
            PrintMode = transportationContractFormData.PrintMode;
            OutputUnit = transportationContractFormData.OutputUnit;
            IsUpdateExportDate = transportationContractFormData.IsUpdateExportDate;
            DateTypeContract = transportationContractFormData.DateTypeContract;
            DateFrom = transportationContractFormData.DateFrom;
            DateTo = transportationContractFormData.DateTo;
            OutputSelection = transportationContractFormData.OutputSelection;
            YearlyContract = transportationContractFormData.YearlyContract;
            UketsukeEigyoJo = transportationContractFormData.UketsukeEigyoJo;
            EigyoTantoSha = transportationContractFormData.EigyoTantoSha;
            InpSyainCd = transportationContractFormData.InpSyainCd;
            UkeNumber = transportationContractFormData.UkeNumber;
            Gyosya = transportationContractFormData.Gyosya;
            TokuiSaki = transportationContractFormData.TokuiSaki;
            TokuiSiten = transportationContractFormData.TokuiSiten;
            YoyakuKbnList = transportationContractFormData.YoyakuKbnList;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    
}
