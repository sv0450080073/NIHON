using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class IncidentalBooking
    {
        public IncidentalBooking()
        {
            VehicleInformationList = new List<VehicleInformation>();
            SettingQuantityList = new List<SettingQuantity>();
            LoadFuttumList = new List<LoadFuttum>();
        }

        // if(Futtum.Count > 0) => true [include deleted]
        // else => false
        public bool IsEditMode { get; set; }
        // soft delete => track this to create new Futtum
        public short FuttumRenMax { get; set; }
        public short UnkRen { get; set; }
        public bool IsPreviousDay { get; set; }
        public bool IsAfterDay { get; set; }
        public string YoyaKbn { get; set; }
        public DateTime HaiSYmd { get; set; }
        public DateTime TouYmd { get; set; }
        public DateTime UkeYmd { get; set; }
        public int UkeCd { get; set; }
        public string DanTaNm { get; set; }
        public string Tokui { get; set; }
        public string TokuSiten { get; set; }
        public byte TesKbnFut { get; set; }
        public string TokuiTanNm { get; set; }
        public string TokuiTel { get; set; }
        public string TokuiFax { get; set; }
        public string IkNm { get; set; }
        public string HaiSNm { get; set; }
        public BookingInputHelper.MyTime HaiSTime { get; set; }
        public BookingInputHelper.MyTime SyuPaTime { get; set; }
        public string TouNm { get; set; }
        public BookingInputHelper.MyTime TouChTime { get; set; }
        public short JyoSyaJin { get; set; }
        public short PlusJin { get; set; }
        public short DrvJin { get; set; }
        public short GuiSu { get; set; }
        public string OthJinKbn1 { get; set; }
        public short OthJin1 { get; set; }
        public string OthJinKbn2 { get; set; }
        public short OthJin2 { get; set; }
        public byte DefaultLoadedItemTaxType { get; set; }
        public decimal DefaultFutaiChargeRate { get; set; }
        public RoundTaxAmountType RoundType { get; set; }
        public IncidentalViewMode FuttumKbnMode { get; set; }
        public List<VehicleInformation> VehicleInformationList { get; set; }
        public List<SettingQuantity> SettingQuantityList { get; set; }

        public List<LoadFuttum> LoadFuttumList { get; set; }
    }

    public class VehicleInformation
    {
        public int No { get; set; }
        public DateTime HaiSYmd { get; set; }
        public int SyaRyoCd { get; set; }
        public string SyaRyoNm { get; set; }
        public string WorkingBranch { get; set; }
    }

    public class LoadFuttum
    {
        public LoadFuttum()
        {
            SettingQuantityList = new List<SettingQuantity>();
            CanDelete = true;
            RoundType = RoundTaxAmountType.Rounding;
            FirstLoad = true;
            EditState = FormEditState.None;
        }

        public int Index { get; set; }
        public bool CanDelete { get; set; }
        public short FutTumRen { get; set; }

        public IncidentalViewMode FuttumKbnMode { get; set; }
        public ScheduleSelectorModel ScheduleDate { get; set; }

        public string SeisanNm { get; set; }
        public LoadSeisanCd SelectedLoadSeisanCd { get; set; }

        // refactor this
        public LoadSeisanKbn SelectedLoadSeisanKbn { get; set; }

        public string FutTumNm { get; set; }
        public LoadFutai SelectedLoadFutai { get; set; } // futtumkbn = 1
        public LoadTsumi SelectedLoadTsumi { get; set; } // futtumkbn = 2

        public string NoteInput { get; set; }
        public LoadDouro SelectedLoadNyuDouro { get; set; }
        public LoadDouro SelectedLoadShuDouro { get; set; }

        public string IriRyoNm { get; set; }
        public LoadNyuRyokinName SelectedLoadNyuRyokinName { get; set; }

        public string DeRyoNm { get; set; }
        public LoadNyuRyokinName SelectedLoadShuRyokin { get; set; }

        #region Earnings

        // Quantity
        private short _suryo;
        public string Suryo
        {
            get
            {
                return _suryo.ToString();
            }
            set
            {
                if (short.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out short newValue) && newValue >= 0 && newValue <= 999)
                {
                    _suryo = newValue;
                }
            }
        }

        // Unit price
        private int _tanka;
        public string Tanka
        {
            get
            {
                return _tanka.ToString();
            }
            set
            {
                if (int.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out int newValue) && newValue >= 0 && newValue <= 999999)
                {
                    _tanka = newValue;
                }
            }
        }

        // Total amount
        public int UriGakKin
        {
            get
            {
                return _suryo * _tanka;
            }
        }
        public string UriGakKinText => UriGakKin.ToString();
        public int TotalAmountWithoutTax
        {
            get
            {
                if(TaxType?.IdValue == Constants.InTax.IdValue)
                {
                    return UriGakKin - SyaRyoSyo;
                }
                else
                {
                    return UriGakKin;
                }
            }
        }

        // Tax included Total
        public int ZeikomiKin
        {
            get
            {
                if (TaxType?.IdValue == Constants.NoTax.IdValue || TaxType?.IdValue == Constants.InTax.IdValue)
                {
                    return UriGakKin;
                }
                else if (TaxType?.IdValue == Constants.ForeignTax.IdValue)
                {
                    return UriGakKin + SyaRyoSyo;
                }
                else
                {
                    return 0;
                }
            }
        }

        private TaxTypeList _taxType;
        public TaxTypeList TaxType
        {
            get
            {
                return _taxType;
            }
            set
            {
                _taxType = value;
                if(!FirstLoad) Zeiritsu = DefaultTaxRate.ToString();
            }
        }

        public RoundTaxAmountType RoundType { get; set; }

        // Tax Rate
        public decimal DefaultTaxRate { get; set; }
        private decimal _zeiritsu;
        public string Zeiritsu
        {
            get
            {
                if(_taxType.IdValue == Constants.NoTax.IdValue)
                {
                    _zeiritsu = 0;
                }
                return _zeiritsu.ToString();
            }
            set
            {
                if (decimal.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out decimal newValue) && newValue >= 0 && newValue < 100)
                {
                    _zeiritsu = newValue;
                }
            }
        }

        // Tax amount
        public int SyaRyoSyo
        {
            get
            {
                if (TaxType?.IdValue == Constants.ForeignTax.IdValue)
                {
                    var result = UriGakKin * _zeiritsu / 100;
                    return (int)BookingInputHelper.RoundTaxAmountHelper[RoundType].Invoke(result);
                }
                else if (TaxType?.IdValue == Constants.InTax.IdValue)
                {
                    var result = UriGakKin * _zeiritsu / (100 + _zeiritsu);
                    return (int)BookingInputHelper.RoundTaxAmountHelper[RoundType].Invoke(result);
                }
                else
                {
                    return 0;
                }
            }
        }

        // Charge rate
        private decimal _tesuRitu;
        public string TesuRitu
        {
            get
            {
                return _tesuRitu.ToString();
            }
            set
            {
                if (decimal.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out decimal newValue) && newValue >= 0 && newValue < 100)
                {
                    _tesuRitu = newValue;
                }
            }
        }

        // Charge amount
        public int SyaRyoTes
        {
            get
            {
                return (int)Math.Ceiling(ZeikomiKin * (_tesuRitu / 100));
            }
        }

        #endregion

        #region Purchasing

        // Quantity
        private short _sirSuryo;
        public string SirSuryo
        {
            get
            {
                return _sirSuryo.ToString();
            }
            set
            {
                if (short.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out short newValue) && newValue >= 0 && newValue <= 999)
                {
                    _sirSuryo = newValue;
                }
            }
        }

        // Unit price
        private int _sirTanka;
        public string SirTanka
        {
            get
            {
                return _sirTanka.ToString();
            }
            set
            {
                if (int.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out int newValue) && newValue >= 0 && newValue <= 999999)
                {
                    _sirTanka = newValue;
                }
            }
        }

        // Total amount
        public int SirGakKin
        {
            get
            {
                return _sirSuryo * _sirTanka;
            }
        }
        public string SirGakKinText => SirGakKin.ToString();
        public int SirGakKinWithoutTax
        {
            get
            {
                if (SirTaxType?.IdValue == Constants.InTax.IdValue)
                {
                    return SirGakKin - SirSyaRyoSyo;
                }
                else
                {
                    return SirGakKin;
                }
            }
        }

        // Tax included Total
        public decimal SirZeikomiKin
        {
            get
            {
                if (SirTaxType?.IdValue == Constants.NoTax.IdValue || SirTaxType?.IdValue == Constants.InTax.IdValue)
                {
                    return SirGakKin;
                }
                else if (SirTaxType?.IdValue == Constants.ForeignTax.IdValue)
                {
                    return SirGakKin + SirSyaRyoSyo;
                }
                else
                {
                    return 0;
                }
            }
        }

        private TaxTypeList _sirTaxType;
        public TaxTypeList SirTaxType
        {
            get
            {
                return _sirTaxType;
            }
            set
            {
                _sirTaxType = value;
                if(!FirstLoad) SirZeiritsu = DefaultTaxRate.ToString();
            }
        }

        // Tax rate
        private decimal _sirZeiritsu;
        public string SirZeiritsu
        {
            get
            {
                if(_sirTaxType.IdValue == Constants.NoTax.IdValue)
                {
                    _sirZeiritsu = 0;
                }
                return _sirZeiritsu.ToString();
            }
            set
            {
                if (decimal.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out decimal newValue) && newValue >= 0 && newValue < 100)
                {
                    _sirZeiritsu = newValue;
                }
            }
        }

        // Tax amount
        public int SirSyaRyoSyo
        {
            get
            {
                if (SirTaxType?.IdValue == Constants.ForeignTax.IdValue)
                {
                    var result = SirGakKin * _sirZeiritsu / 100;
                    return (int)BookingInputHelper.RoundTaxAmountHelper[RoundType].Invoke(result);
                }
                else if (SirTaxType?.IdValue == Constants.InTax.IdValue)
                {
                    var result = SirGakKin * _sirZeiritsu / (100 + _sirZeiritsu);
                    return (int)BookingInputHelper.RoundTaxAmountHelper[RoundType].Invoke(result);
                }
                else
                {
                    return 0;
                }
            }
        }

        public LoadCustomerList SelectedCustomer { get; set; }
        public List<SettingQuantity> SettingQuantityList { get; set; }

        #endregion

        public bool Editing { get; set; }
        public FormEditState EditState { get; set; }
        // use to enable auto calculate value
        public bool FirstLoad { get; set; }
    }

    public class SettingQuantity
    {
        public short TeiDanNo { get; set; }
        public short UnkRen { get; set; }
        public short BunkRen { get; set; }

        public DateTime GarageLeaveDate { get; set; }
        public DateTime GarageReturnDate { get; set; }
        public short GoSyaJyn { get; set; }
        public short BunKSyuJyn { get; set; }

        public string GoSya { get; set; }
        public string GosyaDisplay
        {
            get
            {
                if (BunKSyuJyn != 0)
                {
                    return string.Format("{0} 号車 - {1:D3}", GoSya.Trim(), BunkRen);
                }
                return string.Format("{0} 号車", GoSya.Trim());
            }
        }
        public string SyaRyoNm { get; set; }

        private short _suryo;
        public string Suryo
        {
            get
            {
                return _suryo.ToString();
            }
            set
            {
                if (short.TryParse(value.Normalize(System.Text.NormalizationForm.FormKC), out short newValue) && newValue >= 0 && newValue <= 999)
                {
                    _suryo = newValue;
                }
            }
        }
        public string YouSha { get; set; }
    }

    public class LoadFutaiType
    {
        /// <summary>
        /// Mark this object is selected all item. Default values is <c>false</c>
        /// </summary>
        public bool IsSelectedAll { get; set; } = false;

        public int FutaiCdSeq { get; set; }
        public int FutaiCd { get; set; }
        public int FutGuiKbn { get; set; }
        
        public string RyakuNm { get; set; }
        public int CodeKbnSeq { get; set; }
        public string CodeKbnNm { get; set; }

        public string FutaiCodeKbn => (IsSelectedAll) ? Constants.SelectedAll : CodeKbnNm;
    }

    public class LoadFutai
    {
        /// <summary>
        /// Mark this object is selected all item. Default values is <c>false</c>
        /// </summary>
        public bool IsSelectedAll { get; set; } = false;

        public short Futaicd { get; set; }
        public string FutaiNm { get; set; }
        public string RyakuNm { get; set; }
        public int FutaiCdSeq { get; set; }
        public byte ZeiHyoKbn { get; set; }
        public string Text
        {
            get
            {
                return string.Format("{0:D5} : {1}", Futaicd, FutaiNm);
            }
        }

        /// <summary>
        /// Get Futai text as format: [FutaiCd]:[RyakuNm] or SelectedAll text
        /// </summary>
        public string FutaiText
        {
            get
            {
                if (IsSelectedAll || FutaiCdSeq == 0)
                    return Constants.SelectedAll;
                return string.Format("{0:D5} : {1}", Futaicd, RyakuNm);
            }
        }
    }

    public class LoadTsumi
    {
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }
        public string RyakuNm { get; set; }
        public int CodeKbnSeq { get; set; }
        public string Text
        {
            get
            {
                return string.Format("{0} : {1}", CodeKbn, CodeKbnNm);
            }
        }
    }

    public class LoadSeisanCd
    {
        public short SeisanCd { get; set; }
        public string RyakuNm { get; set; }
        public byte SeisanKbn { get; set; }
        public int SeisanCdSeq { get; set; }
        public string Text
        {
            get
            {
                return string.Format("{0} : {1}", SeisanCd, RyakuNm);
            }
        }
    }

    public class LoadSeisanKbn
    {
        public string RyakuName { get; set; }
        public string CodeKbn { get; set; }
        public string Text
        {
            get
            {
                return RyakuName;
            }
        }
    }

    public class LoadNyuRyokinName
    {
        public string RyokinTikuCd { get; set; }
        public string RyokinCd { get; set; }
        public string RyakuNm { get; set; }
        public int DouroCdSeq { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0}-{1} : {2}", RyokinTikuCd, RyokinCd, RyakuNm);
            }
        }
    }

    public class LoadDouro
    {
        public int CodeKbnSeq { get; set; }
        public string CodeKbn { get; set; }
        public string CodeKbnName { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0} : {1}", CodeKbn, CodeKbnName);
            }
        }
    }

    public class SettingTaxRate
    {
        public decimal Zeiritsu1 { get; set; }
        public DateTime Zei1StartDate { get; set; }
        public DateTime Zei1EndDate { get; set; }

        public decimal Zeiritsu2 { get; set; }
        public DateTime Zei2StartDate { get; set; }
        public DateTime Zei2EndDate { get; set; }

        public decimal Zeiritsu3 { get; set; }
        public DateTime Zei3StartDate { get; set; }
        public DateTime Zei3EndDate { get; set; }

        public decimal GetTaxRate(DateTime startDate)
        {
            if (startDate.IsInRange(Zei1StartDate, Zei1EndDate))
            {
                return Zeiritsu1;
            }
            else if (startDate.IsInRange(Zei2StartDate, Zei2EndDate))
            {
                return Zeiritsu2;
            }
            else if (startDate.IsInRange(Zei3StartDate, Zei3EndDate))
            {
                return Zeiritsu3;
            }
            return 10;
        }
    }

    public class MaxUpdYmdTime
    {
        public long FutTumMaxUpdYmdTime { get; set; }
        public long MishumMaxUpdYmdTime { get; set; }
    }
}
