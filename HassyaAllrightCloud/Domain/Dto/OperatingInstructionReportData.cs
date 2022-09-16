using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class OperatingInstructionReportData
    {
        //public string ReceiptNumberFrom { get; set; } = "";
        //public string ReceiptNumberTo { get; set; } = "";
        // public List<ReservationClassComponentData> ReservationList { get; set; } = new List<ReservationClassComponentData>();
        public DateTime DeliveryDate { get; set; }
        public DepartureOfficeData DepartureOffice { get; set; } = new DepartureOfficeData();        
        public OutputOrderData OutputOrder { get; set; } = new OutputOrderData();
        public int TeiDanNo { get; set; }=1;
        public int UnkRen { get; set; }=1;
        public int BunkRen { get; set; }=1;
        public bool OperationInstructions { get; set; } = true;
        public bool CrewRecordBook { get; set; } = true;
        public string UkenoList { get; set; } = "";
        public int FormOutput { get; set; } = 0;
        public int TenantCdSeq { get; set; }
        public string SyuEigCdSeq => DepartureOffice.EigyoCdSeq.ToString();
        public string SyuKoYmd => DeliveryDate.ToString("yyyyMMdd");
        // public string YoyaKbnSeqList => string.Join('-', ReservationList.Select(_ => _.YoyaKbnSeq));
        // 予約区分
        // New
        public ReservationClassComponentData YoyakuFrom { get; set; }
        public ReservationClassComponentData YoyakuTo { get; set; }
        public string Uri
        {
            get
            {
                {
                    var tenantCdSeq = new ClaimModel().TenantID;
                    return String.Format("TenantCdSeq={0}&" +
                      "SyuKoYmd={1}&" +
                      "UkeCdFrom={2}&" +
                      "UkeCdTo={3}&" +
                      "YoyakuFrom={4}" +
                      "&SyuEigCdSeq={5}" +
                      "&TeiDanNo={6}&" +
                      "UnkRen={7}" +
                      "&BunkRen={8}" +
                      "&SortOrder={9}" +
                      "&UkenoList={10}" +
                      "&FormOutput={11}" +
                      "&YoyakuTo={12}"
                      , new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID,
                      DeliveryDate.ToString("yyyyMMdd"),
                      ReceiptNumberFrom == ""||long.Parse(ReceiptNumberFrom)>=int.MaxValue ? 0 : int.Parse(ReceiptNumberFrom),
                      ReceiptNumberTo == ""|| long.Parse(ReceiptNumberTo)>=int.MaxValue? int.MaxValue : int.Parse(ReceiptNumberTo),
                      (YoyakuFrom == null || YoyakuFrom.YoyaKbnSeq == 0) ? 0 : YoyakuFrom.YoyaKbnSeq,
                      DepartureOffice.EigyoCdSeq,
                      TeiDanNo,
                      UnkRen,
                      BunkRen,
                      OutputOrder.IdValue,
                      UkenoList != "" ? UkenoList : "",
                      FormOutput,
                      (YoyakuTo == null || YoyakuTo.YoyaKbnSeq == 0) ? 0 : YoyakuTo.YoyaKbnSeq
                       );
                }
            }
        }
        public OutputInstruction OutputSetting { get; set; }
        public ReportOutputData ReportOutput { get; set; } = new ReportOutputData();
        
        public ReservationData BookingFrom { get; set; } = new ReservationData();
        public ReservationData BookingTo { get; set; } = new ReservationData();
        public long _ukeCdFrom { get; set; } = -1;
        public string ReceiptNumberFrom
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
                else
                {
                    _ukeCdFrom = long.Parse(value);
                }
            }
        }

        public long _ukeCdTo { get; set; } = -1;
        public string ReceiptNumberTo
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
                else
                {
                    _ukeCdTo = long.Parse(value);
                }
            }
        }
    }

    public class ReportOutputData
    {
        public int IdValue { get; set; }
        public string StringValue { get; set; }
    }
    public class FuttumData
    {
        public string FUTTUM_HasYmd { get; set; }
        public short FUTTUM_UnkRen { get; set; }
        public byte FUTTUM_FutTumKbn { get; set; }
        public short FUTTUM_FutTumRen { get; set; }
        public string FUTTUM_FutTumNm { get; set; }
        public string FUTTUM_SeisanNm { get; set; }
        public int FUTTUM_FutTumCdSeq { get; set; }
        public short MFUTTU_Suryo { get; set; }
        public short MFUTTU_TeiDanNo { get; set; }
        public short MFUTTU_BunkRen { get; set; }
    }
    public class FuttumShowData
    {
        public string FUTTUM_HasYmd1 { get; set; }
        public string FUTTUM_FutTumNm1 { get; set; }
        public string FUTTUM_SeisanNm1 { get; set; }
        public int MFUTTU_Suryo1 { get; set; }
        //
        public string FUTTUM_HasYmd2 { get; set; }
        public string FUTTUM_FutTumNm2 { get; set; }
        public string FUTTUM_SeisanNm2 { get; set; }
        public int MFUTTU_Suryo2 { get; set; }
        //
        public string FUTTUM_HasYmd3 { get; set; }
        public string FUTTUM_FutTumNm3 { get; set; }
        public string FUTTUM_SeisanNm3 { get; set; }
        public int MFUTTU_Suryo3 { get; set; }
        //
        public string FUTTUM_HasYmd4 { get; set; }
        public string FUTTUM_FutTumNm4 { get; set; }
        public string FUTTUM_SeisanNm4 { get; set; }
        public int MFUTTU_Suryo4 { get; set; }
        //
        public string FUTTUM_HasYmd5 { get; set; }
        public string FUTTUM_FutTumNm5 { get; set; }
        public string FUTTUM_SeisanNm5 { get; set; }
        public int MFUTTU_Suryo5 { get; set; }
    }
    public class OperatingInstructionReportPDF
    {
        public CurrentOperatingInstruction currentOperatingInstruction { get; set; } = new CurrentOperatingInstruction();
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
        public string Ukeno { get; set; }
        public List<JourneysDataReport> JourneysShowReport { get; set; } = new List<JourneysDataReport>();
        public FuttumShowData IncidentalData { get; set; } = new FuttumShowData();
        public FuttumShowData LoadedGoodsData { get; set; } = new FuttumShowData();
        public OperatingInstructionReportData SearchData { get; set; } = new OperatingInstructionReportData();

    }
        public class CurrentOperatingInstruction
    {
        public long Row_Num { get; set; }
        public int YYKSHO_UkeCd { get; set; }
        public string YYKSHO_TokuiTel { get; set; }
        public string YYKSHO_TokuiTanNm { get; set; }
        public string UNKOBI_HaiSYmd { get; set; }
        public string UNKOBI_TouYmd { get; set; }
        public string UNKOBI_DanTaNm { get; set; }
        public string UNKOBI_KanjJyus1 { get; set; }
        public string UNKOBI_KanjJyus2 { get; set; }
        public string UNKOBI_KanjTel { get; set; }
        public string UNKOBI_KanJNm { get; set; }
        public string HAISHA_UkeNo { get; set; }
        public short HAISHA_UnkRen { get; set; }
        public short HAISHA_SyaSyuRen { get; set; }
        public short HAISHA_TeiDanNo { get; set; }
        public short HAISHA_BunkRen  { get; set; }
        public string HAISHA_HaiSYmd { get; set; }
        public string HAISHA_HaiSTime { get; set; }
        public string HAISHA_TouYmd { get; set; }
        public string HAISHA_TouChTime { get; set; }
        public string HAISHA_KikYmd { get; set; }
        public string HAISHA_KikTime { get; set; }
        public string HAISHA_DanTaNm2 { get; set; }
        public int HAISHA_OthJinKbn1 { get; set; }
        public string OTHJIN1_CodeKbnNm { get; set; }
        public int HAISHA_OthJin1 { get; set; }
        public int HAISHA_OthJinKbn2 { get; set; }
        public string OTHJIN2_CodeKbnNm { get; set; }
        public int HAISHA_OthJin2 { get; set; }
        public string HAISHA_HaiSNm  { get; set; }
        public string HAISHA_HaiSJyus1 { get; set; }
        public string HAISHA_HaiSJyus2 { get; set; }
        public string HAISHA_HaiSSetTime { get; set; }
        public string HAISHA_IkNm { get; set; }
        public string HAISHA_SyuKoYmd { get; set; }
        public string HAISHA_SyuKoTime { get; set; }
        public int HAISHA_SyuEigCdSeq  { get; set; }
        public int EIGYOSHO_EigyoCd  { get; set; }
        public string EIGYOSHO_EigyoNm { get; set; }
        public string HAISHA_SyuPaTime { get; set; }
        public int HAISHA_JyoSyaJin  { get; set; }
        public int HAISHA_PlusJin { get; set; }
        public string HAISHA_GoSya { get; set; }
        public string HAISHA_HaiSKouKNm  { get; set; }
        public string HAISHA_HaiSBinNm  { get; set; }
        public string HAISHA_TouSKouKNm { get; set; }
        public string HAISHA_TouSBinNm { get; set; }
        public string HAISHA_TouSetTime { get; set; }
        public string HAISHA_TouNm { get; set; }
        public string HAISHA_TouJyusyo1 { get; set; }
        public string HAISHA_TouJyusyo2 { get; set; }
        public int YYKSYU_SyaSyuDai  { get; set; }
        public string SYASYU_SYARYO_SyaSyuNm  { get; set; }
        public int SYARYO_SyaRyoCd { get; set; }
        public string SYARYO_SyaRyoNm { get; set; }
        public string HENSYA_TenkoNo  { get; set; }
        public string SYARYO_KariSyaRyoNm  { get; set; }
        public string TOKISK_TokuiNm  { get; set; }
        public string TOKIST_SitenNm { get; set; }
        public int YYKSYU_SyaSyuDaisum { get; set; }
        public string HAISHA_BikoNm { get; set; }
        public string DriverList { get; set; }
        public string GuiderList { get; set; }
    }
        public class ReportOutputListData
    {
        public static List<ReportOutputData> ReportOutputlst = new List<ReportOutputData>
        {
            new ReportOutputData { IdValue = 0, StringValue = "すべて", },
            new ReportOutputData { IdValue = 1, StringValue = "運行指示書"},
            new ReportOutputData { IdValue = 2, StringValue = "乗務記録簿"},
        };
    }
}