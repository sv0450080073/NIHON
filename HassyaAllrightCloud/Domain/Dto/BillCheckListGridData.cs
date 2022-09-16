using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class BillCheckListGridData
    {
        public string UkeNo { get; set; }
        public short MisyuRen { get; set; }
        public byte SeiFutSyu { get; set; }
        public short FutTumRen { get; set; }
        public short FutuUnkRen { get; set; }

        // 請求年月日
        public DateTime? BillDate { get; set; }
        // 受付営業所
        public string Office { get; set; }
        // 請求先名
        public string BillAddress { get; set; }
        // 団体名
        public string GroupName { get; set; }
        // 行き名
        public string DestinationName { get; set; }
        // 配車年月日
        public DateTime? DispatchDate { get; set; }
        // 到着年月日
        public DateTime? ArrivalDate { get; set; }
        // 請求付帯種別名
        public string BillIncidentTypeName { get; set; }
        // 付帯積込品名
        public string IncidentLoadingGoodsName { get; set; }
        // 精算名
        public string PaymentName { get; set; }
        // 台数／数量
        public string Quantity { get; set; }
        // 車種（型）
        public string BusType { get; set; }
        // 単価
        public decimal Price { get; set; }
        // 請求額
        public int BillAmount { get; set; }
        // 入金年月日
        public DateTime? DepositDate { get; set; }
        // 入金合計
        public decimal DepositAmount { get; set; }
        // 未収額
        public decimal UnpaidAmount { get; set; }
        // 売上額
        public int SalesAmount { get; set; }
        // 消費税額
        public int TaxAmount { get; set; }
        // 手数料率
        public decimal CommissionRate { get; set; }
        // 手数料額
        public int CommissionAmount { get; set; }
        // 発生年月日
        public DateTime? OccurrenceDate { get; set; }
        // 発行年月日
        public DateTime? IssuedDate { get; set; }
        // 受付番号
        public string ReceiptNumber { get; set; }
        // 行のテキストの色の管理のためのプロパティ
        public byte NyuKinKbn { get; set; }
        public byte NCouKbn { get; set; }
        // 台数
        public int UnitNumber { get; set; }
        // 数量
        public int QuantityNumber { get; set; }
        // 請求先 code
        public string BillAddressCode { get; set; }
        public int No { get; set; }
        public BillCheckListGridData()
        {

        }
        public BillCheckListGridData(BillCheckListGridData data)
        {
            UkeNo = data.UkeNo;
            MisyuRen = data.MisyuRen;
            SeiFutSyu = data.SeiFutSyu;
            BillDate = data.BillDate;
            Office = data.Office;
            BillAddress = data.BillAddress;
            GroupName = data.GroupName;
            DestinationName = data.DestinationName;
            DispatchDate = data.DispatchDate;
            ArrivalDate = data.ArrivalDate;
            BillIncidentTypeName = data.BillIncidentTypeName;
            IncidentLoadingGoodsName = data.IncidentLoadingGoodsName;
            PaymentName = data.PaymentName;
            Quantity = data.Quantity;
            BusType = data.BusType;
            Price = data.Price;
            BillAmount = data.BillAmount;
            DepositDate = data.DepositDate;
            DepositAmount = data.DepositAmount;
            UnpaidAmount = data.UnpaidAmount;
            SalesAmount = data.SalesAmount;
            TaxAmount = data.TaxAmount;
            CommissionRate = data.CommissionRate;
            CommissionAmount = data.CommissionAmount;
            OccurrenceDate = data.OccurrenceDate;
            IssuedDate = data.IssuedDate;
            ReceiptNumber = data.ReceiptNumber;
            // color
            NyuKinKbn = data.NyuKinKbn;
            NCouKbn = data.NCouKbn;
            UnitNumber = data.UnitNumber;
            QuantityNumber = data.QuantityNumber;
            BillAddressCode = data.BillAddressCode;
            No = data.No;
        }
    }
    public class BillCheckListTotalData
    {
        // 1:total select, 2:Total one page, 3: Total all page 
        public int Type { get; set; }
        public string Text { get; set; }
        public byte SeiFutSyu { get; set; }
        // 請求額
        public long? BillAmountTotal { get; set; }
        // 入金合計
        public decimal? DepositAmountTotal { get; set; }
        // 未収額
        public decimal? UnpaidAmountTotal { get; set; }
        // 売上額
        public long? SalesAmountTotal { get; set; }
        // 消費税額
        public long? TaxAmountTotal { get; set; }
        // 手数料額
        public long? CommissionAmount { get; set; }
        // 台数
        public long? UnitNumberTotal { get; set; }
        // 数量
        public long? QuantityNumberTotal { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public BillCheckListTotalData()
        {

        }
        public BillCheckListTotalData(BillCheckListTotalData data)
        {
            Type = data.Type;
            Text = data.Text;
            SeiFutSyu = data.SeiFutSyu;
            SeiFutSyu = data.SeiFutSyu;
            BillAmountTotal = data.BillAmountTotal;
            DepositAmountTotal = data.DepositAmountTotal;
            UnpaidAmountTotal = data.UnpaidAmountTotal;
            SalesAmountTotal = data.SalesAmountTotal;
            TaxAmountTotal = data.TaxAmountTotal;
            CommissionAmount = data.CommissionAmount;
            UnitNumberTotal = data.UnitNumberTotal;
            QuantityNumberTotal = data.QuantityNumberTotal;
            UserCode = data.UserCode;
            UserName = data.UserName;
        }

    }
    public class BillCheckListReportPDF
    {
        public List<BillCheckListGridData> ListData { get; set; }
        public List<BillCheckListTotalData> ListTotal { get; set; }
        public int PageNumber { get; set; }
        public string CurrentDate { get; set; }
        public string OutputType { get; set; }
        public string BillPeriodFrom { get; set; }
        public string BillPeriodTo { get; set; }
        public string BillOffice { get; set; }
        public string BillOfficeCode { get; set; }
        public string StartBillAddress { get; set; }
        public string EndBillAddress { get; set; }
        public string StartBillAddressCode { get; set; }
        public string EndBillAddressCode { get; set; }
        public string BillAddress { get; set; }
        public string BillAddressCode { get; set; }
        public string BillIssuedClassification { get; set; }

    }

    public class BillCheckListModelCsvData
    {
        // 請求営業所コード
        public int BillOfficeCode { get; set; }
        // 請求営業所名
        public string BillOffice { get; set; }
        // 請求営業所略名
        public string BillOfficeAbbreviation { get; set; }
        // 請求先業者コード
        public int BillCompanyCode { get; set; }
        // 請求先コード
        public int BillAddressCode { get; set; }
        // 請求先支店コード
        public int BillBranchCode { get; set; }
        // 請求先業者コード名
        public string BillCompanyName { get; set; }
        // 請求先名
        public string BillAddress { get; set; }
        // 請求先支店名
        public string BillBranchName { get; set; }
        // 請求先略名
        public string BillAbbreviation { get; set; }
        // 請求先支店略名
        public string BillBranchShortName { get; set; }
        // 請求年月日
        public string BillDate { get; set; }
        // 受付番号
        public string ReceiptNumber { get; set; }
        // 受付営業所コード
        public int ReceiptOfficeCode { get; set; }
        // 受付営業所名
        public string ReceiptOfficeName { get; set; }
        // 受付営業所略名
        public string ReceiptOfficeAbbreviationName { get; set; }
        // 団体名
        public string GroupName { get; set; }
        // 行き名
        public string DestinationName { get; set; }
        // 配車年月日
        public string DispatchDate { get; set; }
        // 到着年月日
        public string ArrivalDate { get; set; }
        // 請求付帯種別
        public byte BillIncidentType { get; set; }
        // 請求付帯種別名
        public string BillIncidentTypeName { get; set; }
        // 付帯積込品名
        public string IncidentLoadingGoodsName { get; set; }
        // 精算コード
        public string PaymentCode { get; set; }
        // 精算名
        public string PaymentName { get; set; }
        // 数量
        public int UnitNumber { get; set; }
        // 単価
        public string Price { get; set; }
        // 請求額
        public int BillAmount { get; set; }
        // 入金年月日
        public string DepositDate { get; set; }
        // 入金合計
        public decimal DepositAmount { get; set; }
        // 未収額
        public decimal UnpaidAmount { get; set; }
        // 売上額
        public int SalesAmount { get; set; }
        // 消費税額
        public int TaxAmount { get; set; }
        // 手数料率
        public decimal CommissionRate { get; set; }
        // 手数料額
        public int CommissionAmount { get; set; }
        // 発生年月日
        public string OccurrenceDate { get; set; }
        // 発行年月日
        public string IssuedDate { get; set; }
        // 得意先コード使用開始年月日
        public string TSiyoStaYmd { get; set; }
        // 得意先コード使用終了年月日
        public string TSiyoEndYmd { get; set; }
        // 得意先支店コード使用開始年月日
        public string SSiyoStaYmd { get; set; }
        // 得意先支店コード使用終了年月日
        public string SSiyoEndYmd { get; set; }
        // 台数
        public string QuantityNumber { get; set; }
        // 車種単価
        public string Sum_SyaSyuTan { get; set; }
        public string UkeNo { get; set; }
        public short MisyuRen { get; set; }
        public byte SeiFutSyu { get; set; }
        public short FutTumRen { get; set; }
        public short FutuUnkRen { get; set; }
        public int No { get; set; }

        public BillCheckListModelCsvData()
        {

        }
        public BillCheckListModelCsvData(BillCheckListModelCsvData data)
        {
            BillOfficeCode = data.BillOfficeCode;
            BillOffice = data.BillOffice;
            BillOfficeAbbreviation = data.BillOfficeAbbreviation;
            BillCompanyCode = data.BillCompanyCode;
            BillAddressCode = data.BillAddressCode;
            BillBranchCode = data.BillBranchCode;
            BillCompanyName = data.BillCompanyName;
            BillAddress = data.BillAddress;
            BillBranchName = data.BillBranchName;
            BillAbbreviation = data.BillAbbreviation;
            BillBranchShortName = data.BillBranchShortName;
            BillDate = data.BillDate;
            ReceiptNumber = data.ReceiptNumber;
            ReceiptOfficeCode = data.ReceiptOfficeCode;
            ReceiptOfficeName = data.ReceiptOfficeName;
            ReceiptOfficeAbbreviationName = data.ReceiptOfficeAbbreviationName;
            GroupName = data.GroupName;
            DestinationName = data.DestinationName;
            DispatchDate = data.DispatchDate;
            ArrivalDate = data.ArrivalDate;
            BillIncidentType = data.BillIncidentType;
            BillIncidentTypeName = data.BillIncidentTypeName;
            IncidentLoadingGoodsName = data.IncidentLoadingGoodsName;
            PaymentCode = data.PaymentCode;
            PaymentName = data.PaymentName;
            QuantityNumber = data.QuantityNumber;
            Price = data.Price;
            BillAmount = data.BillAmount;
            DepositDate = data.DepositDate;
            DepositAmount = data.DepositAmount;
            UnpaidAmount = data.UnpaidAmount;
            SalesAmount = data.SalesAmount;
            TaxAmount = data.TaxAmount;
            CommissionRate = data.CommissionRate;
            CommissionAmount = data.CommissionAmount;
            OccurrenceDate = data.OccurrenceDate;
            IssuedDate = data.IssuedDate;
            TSiyoStaYmd = data.TSiyoStaYmd;
            TSiyoEndYmd = data.TSiyoEndYmd;
            SSiyoStaYmd = data.SSiyoStaYmd;
            SSiyoEndYmd = data.SSiyoEndYmd;
            UnitNumber = data.UnitNumber;
            Sum_SyaSyuTan = data.Sum_SyaSyuTan;
        }
    }
}
