using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BusBookingDataAllocation
    {
        public int row { get; set; }
        public int Yyksho_UkeCd { get; set; }
        public string Haisha_UkeNo { get; set; } = "0";
        public short Haisha_UnkRen { get; set; }
        public short Haisha_SyaSyuRen { get; set; }
        public short Haisha_TeiDanNo { get; set; }
        public short Haisha_BunkRen { get; set; }
        public int Haisha_GoSya { get; set; }
        public int Haisha_HaiSSryCdSeq { get; set; }
        public int Haisha_SyuEigCdSeq { get; set; }
        public int Haisha_KikEigSeq { get; set; }
        public int Haisha_IkMapCdSeq { get; set; }
        public string Haisha_IkNm { get; set; }= "";
        public string Haisha_SyuKoYmd { get; set; }= "";
        public string Haisha_SyuKoTime { get; set; }= "";
        public string Haisha_SyuPaTime { get; set; }= "";
        public string Haisha_HaiSYmd { get; set; }= "";
        public string Haisha_HaiSTime { get; set; }= "";
        public int Haisha_HaiSCdSeq { get; set; }
        public string Haisha_HaiSNm { get; set; }= "";
        public string Haisha_HaiSJyus1 { get; set; }= "";
        public string Haisha_HaiSJyus2 { get; set; }= "";
        public string Haisha_HaiSKigou { get; set; }= "";
        public int Haisha_HaiSKouKCdSeq { get; set; }
        public int Haisha_HaiSBinCdSeq { get; set; }
        public TimeSpan Haisha_HaiSSetTime { get; set; }
        public string Haisha_KikYmd { get; set; }= "";
        public string Haisha_KikTime { get; set; } = "";
        public string Haisha_TouYmd { get; set; }= "";
        public string Haisha_TouChTime { get; set; }= "";
        public int Haisha_TouCdSeq { get; set; }
        public string Haisha_TouNm { get; set; }= "";
        public string Haisha_TouJyusyo1 { get; set; }= "";
        public string Haisha_TouJyusyo2 { get; set; }= "";
        public string Haisha_TouKigou { get; set; }= "";
        public int Haisha_TouKouKCdSeq { get; set; }
        public int Haisha_TouBinCdSeq { get; set; }
        public TimeSpan Haisha_TouSetTime { get; set; }
        public short Haisha_JyoSyaJin { get; set; }
        public short Haisha_PlusJin { get; set; }
        public short Haisha_DrvJin { get; set; }
        public short Haisha_GuiSu { get; set; }
        public byte Haisha_OthJinKbn1 { get; set; }
        public short Haisha_OthJin1 { get; set; }
        public byte Haisha_OthJinKbn2 { get; set; }
        public short Haisha_OthJin2 { get; set; }
        public string Haisha_PlatNo { get; set; }= "";
        public int Yyksho_TokuiSeq { get; set; }
        public string Haisha_DanTaNm2 { get; set; }= "";
        public string Unkobi_DanTaNm { get; set; }= "";
        public string Unkobi_HaiSYmd { get; set; }= "";
        public string Unkobi_HaiSTime { get; set; }= "";
        public string Unkobi_TouYmd { get; set; }= "";

        public string Unkobi_TouChTime { get; set; }= "";

        public int SyaRyo_SyaRyoCd { get; set; }
        public string SyaRyo_SyaRyoNm { get; set; }= "";
        public int SyaRyo_SyainCdSeq { get; set; }
        public string SyaSyu_SyaSyuNm { get; set; }= "";
        public int YykSyu_SyaSyuCdSeq { get; set; }
        public byte YykSyu_KataKbn { get; set; }
        public string SyaSyu_SyaSyuNm_YykSyu { get; set; }= "";
        public int YykSyu_SyaSyuCdSeq_YykSyu { get; set; }
        public byte YykSyu_KataKbn_YykSyu { get; set; }
        public short YykSyu_SyaSyuDai { get; set; }
        public string TokiSt_RyakuNm { get; set; }= "";
        public string CodeKb_RyakuNm { get; set; }= "";
        public string CodeKb_YykSyu_RyakuNm { get; set; }= "";
        public string Eigyos_RyakuNm { get; set; }= "";
        public string HaishaExp_HaiSKouKNm { get; set; }= "";
        public string HaishaExp_HaisBinNm { get; set; }= "";
        public string HaishaExp_TouSKouKNm { get; set; }= "";
        public string HaishaExp_TouSBinNm { get; set; }= "";
        public string HenSya_TenkoNo { get; set; }= "";
        public List<Driverlst> Driverlstitem { get; set; } = new List<Driverlst>();
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public TimeSpan ArrivalTime{get;set;}
        public TimeSpan ReturnTime { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public TPM_SyokumData driverchartItem { get; set; } = new TPM_SyokumData();
        public TPM_SyokumData driverchartItem1 { get; set; } = new TPM_SyokumData();
        public TPM_SyokumData driverchartItem2 { get; set; } = new TPM_SyokumData();
        public TPM_SyokumData driverchartItem3 { get; set; } = new TPM_SyokumData();
        public TPM_SyokumData driverchartItem4 { get; set; } = new TPM_SyokumData();
        public BusInfoData busnameItem { get; set; } =new BusInfoData();
        public TPM_CodeKbDataKenCD codeKbDataKenCDlstItem{ get; set; }= new TPM_CodeKbDataKenCD();
        public TPM_CodeKbDataBunruiCD depotNamelstItem{ get; set; }= new TPM_CodeKbDataBunruiCD();
        public TPM_CodeKbDataBunruiCD depotNamelstItem1{ get; set; }= new TPM_CodeKbDataBunruiCD();
        public BranchChartData branchchartlstbycurrentcpnItem{ get; set; } = new BranchChartData();
        public BranchChartData branchchartlstbycurrentcpnItem1{ get; set; } = new BranchChartData();
        public TPM_CodeKbDataOTHJINKBN codeKbDataOTHJINKBNlstItem{ get; set; } = new TPM_CodeKbDataOTHJINKBN();
        public TPM_CodeKbDataOTHJINKBN codeKbDataOTHJINKBNlstItem1{ get; set; } = new TPM_CodeKbDataOTHJINKBN();
        public TPM_CodeKbDataDepot depotConnectionItem { get; set; } = new TPM_CodeKbDataDepot();
        public TPM_CodeKbDataDepot depotConnectionItem1 { get; set; } = new TPM_CodeKbDataDepot();
        public string JyoSyaJin { get; set; } = "";
        public string PlusJin { get; set; }= "";
        public string DrvJin { get; set; }= "";
        public string GuiSu { get; set; }= "";
        public string OthJin1 { get; set; }= "";
        public string OthJin2 { get; set; }= "";
        public string GoSya { get; set; }= "";
        public string Tokisk_RyakuNm { get; set; } = "";
        public string Text => $"{Tokisk_RyakuNm}  {TokiSt_RyakuNm}";
        public Dictionary<string, string> CustomData { get; set; } = new Dictionary<string, string>();

    }
}
