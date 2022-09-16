using DevExpress.DataAccess;
using DevExpress.DataAccess.ObjectBinding;
using HassyaAllrightCloud.Application.BillPrint.Queries;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.AspNetCore.Components;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.BillPrint
{
    public class PaymentRequestReport
    {
        public MainInfoReport MainInfoReport { get; set; }
        public List<TableReport> TableReports { get; set; }
        public SeiComInfo seiComInfo { get; set; }
    }

    public class PaymentRequestGroup
    {
        public int SeiOutSeq { get; set; }
        public short SeiRen { get; set; }
        public string SeiFileId { get; set; }
    }

    public class PaymentRequestTenantInfo
    {
        public int TenantCdSeq { get; set; }
        public string TenantCompanyName { get; set; }
    }

    public class MainInfoReport
    {
        public int SeiOutSeq { get; set; }
        public string SeiOutSeqNm { get; set; }
        public short SeiRen { get; set; }
        public string SeiRenNm { get; set; }
        public int TokuiSeq { get; set; }
        public int SitenCdSeq { get; set; }
        public string SiyoEndYmd { get; set; }
        public string SeikYm { get; set; }
        public DateTime? SeikYmDateTime { get; set; }
        public int ZenKurG { get; set; }
        public int KonUriG { get; set; }
        public int KonSyoG { get; set; }
        public int KonTesG { get; set; }
        public int KonNyuG { get; set; }
        public int KonSeiG { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgID { get; set; }
        public string ZipCd { get; set; }
        public string Jyus1 { get; set; }
        public string Jyus2 { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string SeiEigZipCd { get; set; }
        public string SeiEigJyus1 { get; set; }
        public string SeiEigJyus2 { get; set; }
        public string SeiEigEigyoNm { get; set; }
        public string TokuiTanNm { get; set; }
        public string SeiEigTelNo { get; set; }
        public string SeiEigFaxNo { get; set; }
        public string BankNm1 { get; set; }
        public string BankSitNm1 { get; set; }
        public string YokinSyuNm1 { get; set; }
        public string KouzaNo1 { get; set; }
        public string BankNm2 { get; set; }
        public string BankSitNm2 { get; set; }
        public string YokinSyuNm2 { get; set; }
        public string KouzaNo2 { get; set; }
        public string BankNm3 { get; set; }
        public string BankSitNm3 { get; set; }
        public string YokinSyuNm3 { get; set; }
        public string KouzaNo3 { get; set; }
        public string KouzaMeigi { get; set; }
        public string MeisaiKensu { get; set; }
        public string SeiHatYmd { get; set; }
        public DateTime SeiHatYmdDateTime { get; set; }
        public string SeiEigCompanyNm { get; set; }
        public string Reissue { get; set; }
        public string TekaSeikyuTouNo { get; set; }
        public string ComSealImgFileId { get; set; }
        public string SubTitle { get; set; }
        public string SeiTokuiNm { get; set; }
    }

    public class TableReport
    {
        public int SeiOutSeq { get; set; }
        public short SeiRen { get; set; }
        public short SeiMeiRen { get; set; }
        public short SeiUchRen { get; set; }
        public string HasYmd { get; set; }
        public string YoyaNm { get; set; }
        public string FutTumNm { get; set; }
        public string HaiSymd { get; set; }
        public string TouYmd { get; set; }
        public short Suryo { get; set; }
        public int TanKa { get; set; }
        public string SyaSyuNm { get; set; }
        public string SyaSyuNmDisplay { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgID { get; set; }
        public int UriGakKin { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public int SeiKin { get; set; }
        public decimal NyuKinRui { get; set; }
        public string BikoNm { get; set; }
        public byte SeiSaHKbn { get; set; }
        public string UkeNo { get; set; }
        public string IriRyoNm { get; set; }
        public string DeRyoNm { get; set; }
        public byte SeiFutSyu { get; set; }
        public short FutaiCd { get; set; }
        public decimal Zeiritsu { get; set; }
        public string MeisaiUchiwake { get; set; }
        public string ReissueKbn { get; set; }
    }

    public class SeiComInfo
    {
        public byte SeiGenFlg { get; set; }
        public byte SeiKrksKbn { get; set; }
        public string SeiCom1 { get; set; }
        public string SeiCom2 { get; set; }
        public string SeiCom3 { get; set; }
        public string SeiCom4 { get; set; }
        public string SeiCom5 { get; set; }
        public string SeiCom6 { get; set; }

    }
    public class CompanyDto
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public int CompanyCd { get; set; }
        public string CompanyNm { get; set; }
        public string RyakuNm { get; set; }
        public string ComSealImgFileId { get; set; }
    }
}