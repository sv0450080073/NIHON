using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class LoanBookingIncidentalData
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public int YouTblSeq { get; set; }
        public IncidentalViewMode FuttumKbnMode { get; set; }

        public string TokuiSiten { get; set; }
        public byte TesKbnFut { get; set; }
        public short YouFutTumRenMax { get; set; }
        public bool IsPreviousDay { get; set; }
        public bool IsAfterDay { get; set; }
        public DateTime HaiSYmd { get; set; }
        public DateTime TouYmd { get; set; }
        public RoundTaxAmountType RoundType { get; set; }
        public decimal DefaultFutaiChargeRate { get; set; }
        public List<SettingQuantity> SettingQuantityList { get; set; }
        public bool IsSaveMishumFuttum { get; set; }

        public List<LoadYFutTu> LoadYFutTuList { get; set; }
    }

    public class LoadYFutTu
    {
        public LoadYFutTu()
        {
            SettingQuantityList = new List<SettingQuantity>();
            FirstLoad = true;
            RoundType = RoundTaxAmountType.Rounding;
            EditState = FormEditState.None;
        }

        public short YouFutTumRen { get; set; }
        public IncidentalViewMode FuttumKbnMode { get; set; }
        public ScheduleSelectorModel ScheduleDate { get; set; }
        public RoundTaxAmountType RoundType { get; set; }

        public string YFutTuNm { get; set; }
        public LoadYFutai SelectedLoadYFutai { get; set; } // futtumkbn = 1
        public LoadYTsumi SelectedLoadYTsumi { get; set; } // futtumkbn = 2

        public string RyokinNm { get; set; }
        public LoadYRyokin SelectedLoadYRyoKin { get; set; }

        public string ShuRyokinNm { get; set; }
        public LoadYRyokin SelectedLoadYShuRyoKin { get; set; }

        public string SeisanNm { get; set; }
        public LoadYSeisan SelectedLoadYSeisan { get; set; }

        public List<SettingQuantity> SettingQuantityList { get; set; }

        // combobox
        public YouShaSaveType SaveType { get; set; }

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
        public int Goukei
        {
            get
            {
                return _suryo * _tanka;
            }
        }
        public string GoukeiText
        {
            get
            {
                return Goukei.ToString();
            }
        }
        public int GoukeiWithoutTax
        {
            get
            {
                if(TaxType?.IdValue == Constants.InTax.IdValue)
                {
                    return Goukei - SyaRyoSyo;
                }
                return Goukei;
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
                    var result = Goukei * _zeiritsu / 100;
                    return (int)BookingInputHelper.RoundTaxAmountHelper[RoundType].Invoke(result);
                }
                else if (TaxType?.IdValue == Constants.InTax.IdValue)
                {
                    var result = Goukei * _zeiritsu / (100 + _zeiritsu);
                    return (int)BookingInputHelper.RoundTaxAmountHelper[RoundType].Invoke(result);
                }
                else
                {
                    return 0;
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
                    return Goukei;
                }
                else if (TaxType?.IdValue == Constants.ForeignTax.IdValue)
                {
                    return Goukei + SyaRyoSyo;
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

        public bool Editing { get; set; }
        public FormEditState EditState { get; set; }
        // use to enable auto calculate value
        public bool FirstLoad { get; set; }
        public bool IsSetGoukeiFromHaseiKin { get; set; }
    }

    public class LoadYFutai
    {
        public int FutaiCdSeq { get; set; }
        public short FutaiCd { get; set; }
        public string FutaiNm { get; set; }
        public byte FutGuiKbn { get; set; }
        public string CodeKbRyakuNm { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0:D4}:{1}", FutaiCd, FutaiNm);
            }
        }
    }

    public class LoadYTsumi
    {
        public int CodeKbnSeq { get; set; }
        public string CodeKbn { get; set; }
        public string CodeKbnNm { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0}：{1}", CodeKbn, CodeKbnNm);
            }
        }
    }

    public class LoadYRyokin
    {
        public byte RyoKinTikuCd { get; set; }
        public short RyoKinCd { get; set; }
        public string RyoKinNm { get; set; }
        public string SiyoStaYmd { get; set; }
        public string SiyoEndYmd { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0:D2}ー{1:D3}：{2}", RyoKinTikuCd, RyoKinCd, RyoKinNm);
            }
        }
    }

    public class LoadYSeisan
    {
        public short SeisanCd { get; set; }
        public int SeisanCdSeq { get; set; }
        public string SeisanNm { get; set; }
        public byte SeisanKbn { get; set; }
        public string CodeKbRyakuNm { get; set; }

        public string Text
        {
            get
            {
                return string.Format("{0:D4}:{1}", SeisanCd, SeisanNm);
            }
        }
    }

    public class YouShaSaveType
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public string Text
        {
            get
            {
                return Name;
            }
        }
    }
}
