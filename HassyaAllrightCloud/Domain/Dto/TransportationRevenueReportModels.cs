using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    #region Monthly Transportation Revenue Models
    public class MonthlyRevenueSearchModel
    {
        public EigyoListItem Eigyo { get; set; }
        public string UriYm { get; set; }
        public string TesukomiKbn { get; set; }
        public TransportationRevenueSearchModel RevenueSearchModel { get; set; }
    }
    public class MonthlyRevenueItem
    {
        public int No { get; set; }
        public int EigyoCd { get; set; }
        public int MesaiKbn { get; set; }
        public string UriYmd { get; set; }
        public long KeiKin { get; set; }
        public long JisSyaRyoSum { get; set; }
        public int GaiSyaRyoSum { get; set; }
        public int EtcSyaRyoSum { get; set; }
        public int CanSum { get; set; }
        public long JisSyaRyoUnc { get; set; }
        public int JisSyaSyuDai { get; set; }
        public long JisSyaRyoSyo { get; set; }
        public long JisSyaRyoTes { get; set; }
        public int GaiUriGakKin { get; set; }
        public int GaiSyaRyoSyo { get; set; }
        public int GaiSyaRyoTes { get; set; }
        public int EtcUriGakKin { get; set; }
        public int EtcSyaRyoSyo { get; set; }
        public int EtcSyaRyoTes { get; set; }
        public int CanUnc { get; set; }
        public int CanSyoG { get; set; }
        public long JyuSyaRyoRui { get; set; }
        public long UntSoneki { get; set; }

        public int Jippi { get; set; }
        public int HighwayUriGakKin { get; set; }
        public int HighwaySyaRyoSyo { get; set; }
        public int HighwaySyaRyoTes { get; set; }
        public int HighwaySyaRyoSum { get; set; }
        public int HotelUriGakKin { get; set; }
        public int HotelSyaRyoSyo { get; set; }
        public int HotelSyaRyoTes { get; set; }
        public int HotelSyaRyoSum { get; set; }
        public int ParkingUriGakKin { get; set; }
        public int ParkingSyaRyoSyo { get; set; }
        public int ParkingSyaRyoTes { get; set; }
        public int ParkingSyaRyoSum { get; set; }
        public int OtherUriGakKin { get; set; }
        public int OtherSyaRyoSyo { get; set; }
        public int OtherSyaRyoTes { get; set; }
        public int OtherSyaRyoSum { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyak { get; set; }


        public long SJyuSyaRyoUnc { get; set; }
        public long SJyuSyaRyoSyo { get; set; }
        public long SJyuSyaRyoTes { get; set; }
        public long SJyuSyaRyoRui { get; set; }
        public long SJisSyaRyoUnc { get; set; }
        public long SJisSyaRyoSyo { get; set; }
        public long SJisSyaRyoTes { get; set; }
        public int SFutUriGakKin { get; set; }
        public int SFutSyaRyoSyo { get; set; }
        public int SFutSyaRyoTes { get; set; }
        public int SYoushaUnc { get; set; }
        public int SYoushaSyo { get; set; }
        public int SYoushaTes { get; set; }
        public int SYfuUriGakKin { get; set; }
        public int SYfuSyaRyoSyo { get; set; }
        public int SYfuSyaRyoTes { get; set; }
        public long SSoneki { get; set; }
       
        public IEnumerable<MonthlyRevenueDetailItem> DetailItems { get; set; }
    }
    public class MonthlyRevenueDetailItem
    {
        public int EigyoCd { get; set; }
        public string UriYmd { get; set; }
        public int MesaiKbn { get; set; }
        public string YouRyakuNm { get; set; }
        public string YouSitRyakuNm { get; set; }
        public int YouSyaSyuDai { get; set; }
        public int YouG { get; set; }
        public int YouFutG { get; set; }
        public int YouSyaRyoUnc { get; set; }
        public int YouSyaRyoSyo { get; set; }
        public int YouSyaRyoTes { get; set; }
        public int YfuUriGakKin { get; set; }
        public int YfuSyaRyoSyo { get; set; }
        public int YfuSyaRyoTes { get; set; }

        public int YouGyosyaCd { get; set; }
        public int YouCd { get; set; }
        public int YouSitCd { get; set; }
        public string YouGyosyaNm { get; set; }
        public string YouNm { get; set; }
        public string YouSitenNm { get; set; }
    }
    public class MonthlyRevenueCSVReportData
    {
        public string No { get; set; }
        public string UriYmd { get; set; }
        public string EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string EigyoRyak { get; set; }
        public long KeiKin { get; set; }
        public string YouGyosyaCd { get; set; }
        public string YouCd { get; set; }
        public string YouSitCd { get; set; }
        public string YouGyosyaNm { get; set; }
        public string YouNm { get; set; }
        public string YouSitenNm { get; set; }
        public string YouRyakuNm { get; set; }
        public string YouSitRyakuNm { get; set; }
        public string YouSyaRyoUnc { get; set; }
        public string YouSyaSyuDai { get; set; }
        public string YouSyaRyoSyo { get; set; }
        public string YouSyaRyoTes { get; set; }
        public string YouG { get; set; }
        public string YfuUriGakKin { get; set; }
        public string YfuSyaRyoSyo { get; set; }
        public string YfuSyaRyoTes { get; set; }
        public string YouFutG { get; set; }
        public long JisSyaRyoUnc { get; set; }
        public int JisSyaSyuDai { get; set; }
        public long JisSyaRyoSyo { get; set; }
        public long JisSyaRyoTes { get; set; }
        public long JisSyaRyoSum { get; set; }
        public int GaiUriGakKin { get; set; }
        public int GaiSyaRyoSyo { get; set; }
        public int GaiSyaRyoTes { get; set; }
        public int GaiSyaRyoSum { get; set; }
        public int Jippi { get; set; }
        public int HighwayUriGakKin { get; set; }
        public int HighwaySyaRyoSyo { get; set; }
        public int HighwaySyaRyoTes { get; set; }
        public int HighwaySyaRyoSum { get; set; }
        public int HotelUriGakKin { get; set; }
        public int HotelSyaRyoSyo { get; set; }
        public int HotelSyaRyoTes { get; set; }
        public int HotelSyaRyoSum { get; set; }
        public int ParkingUriGakKin { get; set; }
        public int ParkingSyaRyoSyo { get; set; }
        public int ParkingSyaRyoTes { get; set; }
        public int ParkingSyaRyoSum { get; set; }

        public int UriGakKin { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public int SyaRyoSum { get; set; }

        public int CanUnc { get; set; }
        public int CanSyoG { get; set; }
        public int CanSum { get; set; }
        public long UntSoneki { get; set; }
    }
    public class MonthlyRevenueReportItem
    {
        public string No { get; set; }
        public string UriYmd { get; set; }
        public string KeiKin { get; set; }
        public string JisSyaSyuDai { get; set; }
        public string JisSyaRyoUnc { get; set; }
        public string JisSyaRyoSyo { get; set; }
        public string JisSyaRyoTes { get; set; }
        public string JisSyaRyoSum { get; set; }
        public string GaiUriGakKin { get; set; }
        public string GaiSyaRyoSyo { get; set; }
        public string GaiSyaRyoTes { get; set; }
        public string GaiSyaRyoSum { get; set; }
        public string EtcUriGakKin { get; set; }
        public string EtcSyaRyoSyo { get; set; }
        public string EtcSyaRyoTes { get; set; }
        public string EtcSyaRyoSum { get; set; }
        public string CanUnc { get; set; }
        public string CanSyoG { get; set; }
        public string CanSum { get; set; }
        public string YouRyakuNm { get; set; }
        public string YouSitRyakuNm { get; set; }
        public string YouSyaSyuDai { get; set; }
        public string YouSyaRyoUnc { get; set; }
        public string YouSyaRyoSyo { get; set; }
        public string YouSyaRyoTes { get; set; }
        public string YouG { get; set; }
        public string YfuUriGakKin { get; set; }
        public string YfuSyaRyoSyo { get; set; }
        public string YfuSyaRyoTes { get; set; }
        public string YouFutG { get; set; }
        public string UntSoneki { get; set; }
    }
    public class MonthlyRevenueReportData
    {
        public IEnumerable<MonthlyRevenueReportItem> MonthlyRevenueItems { get; set; }
        public List<MonthlyRevenueSummary> Summaries { get; set; }
        public MonthlyRevenueCommonData CommonData { get; set; }
    }
    public class MonthlyRevenueCommonData
    {
        public string EigyoKbnNm { get; set; }
        public string UriYm { get; set; }
        public string EigyoKbn { get; set; }
        public string TesukomiKbnNm { get; set; }
        public string ProcessingDate { get; set; }
        public string Syain { get; set; }
        public string Eigyo { get; set; }
    }
    public class MonthlyRevenueSummary
    {
        public string Name { get; set; }
        public string ZeiKomi { get; set; }
        public string JisSyaSyuDai { get; set; }
        public string JisSyaRyoUnc { get; set; }
        public string JisSyaRyoSyo { get; set; }
        public string JisSyaRyoTes { get; set; }
        public string JisG { get; set; }
        public string GuiUri { get; set; }
        public string GuitZei { get; set; }
        public string GuiTes { get; set; }
        public string GuiG { get; set; }
        public string OtherFutUri { get; set; }
        public string OtherFuttZei { get; set; }
        public string OtherFutTes { get; set; }
        public string OtherFutG { get; set; }
        public string CanKin { get; set; }
        public string CanZei { get; set; }
        public string CanG { get; set; }
        public string YouNm { get; set; }
        public string YouDai { get; set; }
        public string YouUnt { get; set; }
        public string YouZei { get; set; }
        public string YouTes { get; set; }
        public string YouG { get; set; }
        public string YouFutHas { get; set; }
        public string YouFutZei { get; set; }
        public string YouFutTes { get; set; }
        public string YouFutG { get; set; }
        public string DailySoneki { get; set; }
    }
    public class MonthlyRevenueData
    {
        public List<MonthlyRevenueItem> MonthlyRevenueItems { get; set; }
        public List<SummaryResult> SummaryResult { get; set; }
        public IEnumerable<MonthlyRevenueDetailItem> DetailItems { get; set; }
    }
    #endregion

    #region Daily Transportation Revenue Models
    public class DailyRevenueSearchModel
    {
        public EigyoListItem Eigyo { get; set; }
        public string UriYmd { get; set; }
        public string TesukomiKbn { get; set; }
        public string EigyoKbnNm { get; set; }
        public TransportationRevenueSearchModel RevenueSearchModel { get; set; }
    }
    public class DailyRevenueCommonData
    {
        public string UriYmd { get; set; }
        public string EigyoKbnNm { get; set; }
        public string TesukomiKbnNm { get; set; }
        public string EigyoKbn { get; set; }
        public string Syain { get; set; }
        public string EigyoFrom { get; internal set; }
        public string EigyoTo { get; internal set; }
        public string UriYmdFrom { get; internal set; }
        public string UriYmdTo { get; internal set; }
        public string ProcessingDate { get; set; }
    }
    public class DailyRevenueData
    {
        public List<DailyRevenueItem> DailyRevenueItems { get; set; }
        public List<SummaryResult> SummaryResult { get; set; }
        public IEnumerable<DailyRevenueDetailItem> DetailItems { get; set; }
    }
    public class DailyRevenueSummary
    {
        public string Name { get; set; }
        public string ZeiKomi { get; set; }
        public string JisSyaSyuDai { get; set; }
        public string JisSyaRyoUnc { get; set; }
        public string JisSyaRyoSyo { get; set; }
        public string JisSyaRyoTes { get; set; }
        public string JisG { get; set; }
        public string GuiUri { get; set; }
        public string GuitZei { get; set; }
        public string GuiTes { get; set; }
        public string GuiG { get; set; }
        public string OtherFutUri { get; set; }
        public string OtherFuttZei { get; set; }
        public string OtherFutTes { get; set; }
        public string OtherFutG { get; set; }
        public string CanKin { get; set; }
        public string CanZei { get; set; }
        public string CanG { get; set; }
        public string YouNm { get; set; }
        public string YouDai { get; set; }
        public string YouUnt { get; set; }
        public string YouZei { get; set; }
        public string YouTes { get; set; }
        public string YouG { get; set; }
        public string YouFutHas { get; set; }
        public string YouFutZei { get; set; }
        public string YouFutTes { get; set; }
        public string YouFutG { get; set; }
        public string DailySoneki { get; set; }

    }
    public class DailyRevenueReportItem
    {
        public string No { get; set; }
        public string TokRyakuNm { get; set; }
        public string SitRyakuNm { get; set; }
        public string UkeNo { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public string KeiKin { get; set; }
        public string JisSyaSyuDai { get; set; }
        public string JisSyaRyoUnc { get; set; }
        public string JisSyaRyoSyo { get; set; }
        public string JisSyaRyoTes { get; set; }
        public string JisSyaRyoSum { get; set; }
        public string GaiUriGakKin { get; set; }
        public string GaiSyaRyoSyo { get; set; }
        public string GaiSyaRyoTes { get; set; }
        public string GaiSyaRyoSum { get; set; }
        public string EtcUriGakKin { get; set; }
        public string EtcSyaRyoSyo { get; set; }
        public string EtcSyaRyoTes { get; set; }
        public string EtcSyaRyoSum { get; set; }
        public string CanUnc { get; set; }
        public string CanSyoG { get; set; }
        public string CanSum { get; set; }
        public string YouRyakuNm { get; set; }
        public string YouSitRyakuNm { get; set; }
        public string YouSyaSyuDai { get; set; }
        public string YouSyaRyoUnc { get; set; }
        public string YouSyaRyoSyo { get; set; }
        public string YouSyaRyoTes { get; set; }
        public string YouG { get; set; }
        public string YfuUriGakKin { get; set; }
        public string YfuSyaRyoSyo { get; set; }
        public string YfuSyaRyoTes { get; set; }
        public string YouFutG { get; set; }
        public string UntSoneki { get; set; }
    }
    public class DailyRevenueReportData
    {
        public IEnumerable<DailyRevenueReportItem> DailyRevenueItems { get; set; }
        public IEnumerable<DailyRevenueSummary> Summaries { get; set; }
        public DailyRevenueCommonData CommonData { get; set; }
    }
    public class DailyRevenueItem
    {
        public int No { get; set; }
        public int MesaiKbn { get; set; }
        public string UriYmd { get; set; }
        public int SeiEigyoCd { get; set; }
        public int UkeEigyoCd { get; set; }
        public string SeiEigyoNm { get; set; }
        public string UkeEigyoNm { get; set; }
        public string SeiEigyoRyak { get; set; }
        public int GyosyaCd { get; set; }
        public int TokuiCd { get; set; }
        public int SitenCd { get; set; }
        public string GyosyaNm { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public int SirGyosyaCd { get; set; }
        public int SirCd { get; set; }
        public int SirSitenCd { get; set; }
        public string SirGyosyaNm { get; set; }
        public string SirNm { get; set; }
        public string SirSitenNm { get; set; }

        public int UnkRen { get; set; }
        public string UkeNo { get; set; }
        public string UkeRyakuNm { get; set; }
        public string TokRyakuNm { get; set; }
        public string SitRyakuNm { get; set; }
        public string SirRyakuNm { get; set; }
        public string SirSitRyakuNm { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public int Nissu { get; set; }
        public long KeiKin { get; set; }
        public long JisSyaRyoSum { get; set; }
        public int GaiSyaRyoSum { get; set; }
        public int EtcSyaRyoSum { get; set; }
        public int CanSum { get; set; }
        public long UntSoneki { get; set; }
        public long JisSyaRyoUnc { get; set; }
        public int JisSyaSyuDai { get; set; }
        public int JisZeiKbn { get; set; }
        public string JisZeiKbnNm { get; set; }
        public decimal JisSyaRyoSyoRit { get; set; }
        public long JisSyaRyoSyo { get; set; }
        public decimal JisSyaRyoTesRit { get; set; }
        public long JisSyaRyoTes { get; set; }
        public int GaiUriGakKin { get; set; }
        public int GaiSyaRyoSyo { get; set; }
        public int GaiSyaRyoTes { get; set; }
        public int EtcUriGakKin { get; set; }
        public int EtcSyaRyoSyo { get; set; }
        public int EtcSyaRyoTes { get; set; }
        public int CanUnc { get; set; }
        public int CanZKbn { get; set; }
        public string CanZKbnNm { get; set; }
        public decimal CanSyoR { get; set; }
        public int CanSyoG { get; set; }

        public int OtherUriGakKin { get; set; }
        public int OtherSyaRyoSyo { get; set; }
        public int OtherSyaRyoTes { get; set; }
        public int OtherSyaRyoSum { get; set; }
        public int Jippi { get; set; }
        public int HighwayUriGakKin { get; set; }
        public int HighwaySyaRyoSyo { get; set; }
        public int HighwaySyaRyoTes { get; set; }
        public int HighwaySyaRyoSum { get; set; }
        public int HotelUriGakKin { get; set; }
        public int HotelSyaRyoSyo { get; set; }
        public int HotelSyaRyoTes { get; set; }
        public int HotelSyaRyoSum { get; set; }
        public int ParkingUriGakKin { get; set; }
        public int ParkingSyaRyoSyo { get; set; }
        public int ParkingSyaRyoTes { get; set; }
        public int ParkingSyaRyoSum { get; set; }

        public IEnumerable<DailyRevenueDetailItem> DetailItems { get; set; }
    }
    public class DailyRevenueDetailItem
    {
        public int MesaiKbn { get; set; }
        public int YouGyosyaCd { get; set; }
        public string UkeNo { get; set; }
        public int UnkRen { get; set; }
        public string YouRyakuNm { get; set; }
        public string YouSitRyakuNm { get; set; }
        public int YouSyaRyoUnc { get; set; }
        public decimal YouZeiritsu { get; set; }
        public int YouSyaRyoSyo { get; set; }
        public decimal YouTesuRitu { get; set; }
        public int YouSyaRyoTes { get; set; }
        public int YouG { get; set; }
        public int YouSyaSyuDai { get; set; }
        public int YfuUriGakKin { get; set; }
        public int YfuSyaRyoSyo { get; set; }
        public int YfuSyaRyoTes { get; set; }
        public int YouFutG { get; set; }
        public int YouCd { get; set; }
        public int YouSitCd { get; set; }
        public string YouGyosyaNm { get; set; }
        public string YouNm { get; set; }
        public string YouSitenNm { get; set; }
        public int YouZeiKbn { get; set; }
        public string YouZKbnNm { get; set; }
    }
    public class DailyRevenueCSVReportData
    {
        public string No { get; set; }
        public string UriYmd { get; set; }
        public int EigyoCd { get; set; }
        public string EigyoNm { get; set; }
        public string RyakuNm { get; set; }
        public string TesuInKbnCd { get; set; }
        public string TesuInKbnNm { get; set; }
        public string UkeNo { get; set; }
        public int UkeEigyoCd { get; set; }
        public string UkeEigyoNm { get; set; }
        public string UkeRyakuNm { get; set; }
        public int GyosyaCd { get; set; }
        public int TokuiCd { get; set; }
        public int SitenCd { get; set; }
        public string GyosyaNm { get; set; }
        public string TokuiNm { get; set; }
        public string SitenNm { get; set; }
        public string TokRyakuNm { get; set; }
        public string SitRyakuNm { get; set; }
        public int SirGyosyaCd { get; set; }
        public int SirCd { get; set; }
        public int SirSitenCd { get; set; }
        public string SirGyosyaNm { get; set; }
        public string SirNm { get; set; }
        public string SirSitenNm { get; set; }
        public string SirRyakuNm { get; set; }
        public string SirSitRyakuNm { get; set; }
        public string DanTaNm { get; set; }
        public string IkNm { get; set; }
        public int Nissu { get; set; }
        public long KeiKin { get; set; }
        public string YouGyosyaCd { get; set; }
        public int YouCd { get; set; }
        public int YouSitCd { get; set; }
        public string YouGyosyaNm { get; set; }
        public string YouNm { get; set; }
        public string YouSitenNm { get; set; }
        public string YouRyakuNm { get; set; }
        public string YouSitRyakuNm { get; set; }
        public int YouSyaRyoUnc { get; set; }
        public int YouSyaSyuDai { get; set; }
        public int YouZeiKbn { get; set; }
        public string YouZKbnNm { get; set; }
        public decimal YouZeiritsu { get; set; }
        public int YouSyaRyoSyo { get; set; }
        public decimal YouTesuRitu { get; set; }
        public int YouSyaRyoTes { get; set; }
        public string YouG { get; set; }
        public int YfuUriGakKin { get; set; }
        public int YfuSyaRyoSyo { get; set; }
        public int YfuSyaRyoTes { get; set; }
        public int YouFutG { get; set; }
        public long JisSyaRyoUnc { get; set; }
        public int JisSyaSyuDai { get; set; }
        public int JisZeiKbn { get; set; }
        public string JisZeiKbnNm { get; set; }
        public decimal JisSyaRyoSyoRit { get; set; }
        public long JisSyaRyoSyo { get; set; }
        public decimal JisSyaRyoTesRit { get; set; }
        public long JisSyaRyoTes { get; set; }
        public long JisSyaRyoSum { get; set; }
        public int GaiUriGakKin { get; set; }
        public int GaiSyaRyoSyo { get; set; }
        public int GaiSyaRyoTes { get; set; }
        public int GaiSyaRyoSum { get; set; }
        public int Jippi { get; set; }
        public int HighwayUriGakKin { get; set; }
        public int HighwaySyaRyoSyo { get; set; }
        public int HighwaySyaRyoTes { get; set; }
        public int HighwaySyaRyoSum { get; set; }
        public int HotelUriGakKin { get; set; }
        public int HotelSyaRyoSyo { get; set; }
        public int HotelSyaRyoTes { get; set; }
        public int HotelSyaRyoSum { get; set; }
        public int ParkingUriGakKin { get; set; }
        public int ParkingSyaRyoSyo { get; set; }
        public int ParkingSyaRyoTes { get; set; }
        public int ParkingSyaRyoSum { get; set; }
        public int UriGakKin { get; set; }
        public int SyaRyoSyo { get; set; }
        public int SyaRyoTes { get; set; }
        public int SyaRyoSum { get; set; }
        public int CanUnc { get; set; }
        public int CanZKbn { get; set; }
        public string CanZKbnNm { get; set; }
        public decimal CanSyoR { get; set; }
        public int CanSyoG { get; set; }
        public int CanSum { get; set; }
        public long UntSoneki { get; set; }
    }
    #endregion

    #region Common Models
    public class CustomKiDto
    {
        public string Kinou01 { get; set; }
        public string Kinou02 { get; set; }
    }
    public class Kinou
    {
        /// <summary>
        /// Kinou02
        /// </summary>
        public int JippiFlg { get; set; }
        /// <summary>
        /// Kinou01
        /// </summary>
        public int FutaiMeiFlg { get; set; }
    }
    public class CustomKiSearchModel
    {
        public int SyainCdSeq { get; set; }
        public string KinouId { get; set; }
    }
    public class SummaryResult
    {
        public string Type { get; set; }
        public int MesaiKbn { get; set; }
        public long SJyuSyaRyoUnc { get; set; }
        public long SJyuSyaRyoSyo { get; set; }
        public long SJyuSyaRyoTes { get; set; }
        public long SJyuSyaRyoRui { get; set; }
        public long SJisSyaRyoUnc { get; set; }
        public long SJisSyaRyoSyo { get; set; }
        public long SJisSyaRyoTes { get; set; }
        public int SFutUriGakKin { get; set; }
        public int SFutSyaRyoSyo { get; set; }
        public int SFutSyaRyoTes { get; set; }
        public int SYoushaUnc { get; set; }
        public int SYoushaSyo { get; set; }
        public int SYoushaTes { get; set; }
        public int SYfuUriGakKin { get; set; }
        public int SYfuSyaRyoSyo { get; set; }
        public int SYfuSyaRyoTes { get; set; }
        public long SSoneki { get; set; }
        public int JisSyaSyuDai { get; set; }
        public int GaiUriGakKin { get; set; }
        public int GaiSyaRyoSyo { get; set; }
        public int GaiSyaRyoTes { get; set; }
        public int GaiSyaRyoSum { get; set; }
        public int CanUnc { get; set; }
        public int CanSyoG { get; set; }
        public int CanSum { get; set; }

        public int EtcUriGakKin { get; set; }
        public int EtcSyaRyoSyo { get; set; }
        public int EtcSyaRyoTes { get; set; }
        public int EtcSyaRyoSum { get; set; }
    }
    public class FormSearchModel
    {
        public DateTime UriYmdFrom { get; set; }
        public DateTime UriYmdTo { get; set; }
        public ComboboxBaseItem TesuInKbn { get; set; }
        public ComboboxBaseItem PageSize { get; set; }
        public ReservationClassComponentData YoyaKbnFrom { get; set; }
        public ReservationClassComponentData YoyaKbnTo { get; set; }
        public CompanyListItem Company { get; set; }
        public EigyoListItem EigyoFrom { get; set; }
        public EigyoListItem EigyoTo { get; set; }
        public string UkeNoFrom { get; set; }
        public string UkeNoTo { get; set; }
        public ComboboxBaseItem OutputWithHeader { get; set; }
        public ComboboxBaseItem KukuriKbn { get; set; }
        public ComboboxBaseItem KugiriCharType { get; set; }
        public EigyoKbnEnum EigyoKbn { get; set; }
        public OutputType OutputType { get; set; }
        public bool DailyTable { get; set; } = true;
        public bool TotalTable { get; set; }
    }
    public class ComboboxBaseItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }
    public class DisplayColumn
    {
        public string PropName { get; set; }
        public string ColumnName { get; set; }
    }
    public class TransportationRevenueSearchModel
    {
        public string UriYmdFrom { get; set; }
        public string UriYmdTo { get; set; }
        public TesuInKbnEnum TesuInKbn { get; set; }
        public int YoyaKbnFrom { get; set; }
        public int YoyaKbnTo { get; set; }
        public int Company { get; set; }
        public int EigyoFrom { get; set; }
        public int EigyoTo { get; set; }
        public string EigyoNmFrom { get; set; }
        public string EigyoNmTo { get; set; }
        public string UkeNoFrom { get; set; }
        public string UkeNoTo { get; set; }
        public ShowHeaderEnum OutputWithHeader { get; set; }
        public WrapContentEnum KukuriKbn { get; set; }
        public SeperatorEnum KugiriCharType { get; set; }
        public int TenantCdSeq { get; set; }
        public EigyoKbnEnum EigyoKbn { get; set; }
        public PageSize PageSize { get; set; }
        public bool IsDailyReport { get; set; }
        public Guid ReportId { get; set; }
    }
    public class YoyaKbnDto
    {
        public int YoyaKbnSeq { get; set; }
        public int YoyaKbn { get; set; }
        public string YoyaKbnNm { get; set; }
        public string PriorityNum { get; set; }
        public string DisplayName
        {
            get
            {
                return YoyaKbnNm;
            }
        }
    }
    #endregion

    #region Enums
    public enum OutputType
    {
        ExportPdf,
        Preview,
        Print,
        CSV,
        ExportExcel
    }

    public enum EigyoKbnEnum
    {
        BillingOffice = 1,
        ReceptionOffice = 2
    }

    public enum TesuInKbnEnum
    {
        IncludingFee = 0,
        WithoutCommission = 1,
        MasterSetting = 2
    }

    public enum SeperatorEnum
    {
        ByTab = 1,
        BySemicolon = 2,
        ByComma = 3
    }

    public enum PageSize
    {
        A4 = 1,
        A3 = 2,
        B4 = 3
    }

    public enum ShowHeaderEnum
    {
        ShowHeader = 1,
        DoNotShowHeader = 2
    }

    public enum WrapContentEnum
    {
        PutInDoubleQuater = 1,
        DoNotPutInDoubleQuater = 2
    }
    #endregion
}
