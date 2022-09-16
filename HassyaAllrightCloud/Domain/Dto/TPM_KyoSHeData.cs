using System.Collections.Generic;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class KyoSHeDatabyDate
    {
        public DateTime date { get; set; }
        public List<TPM_KyoSHeData> KyoSHelst { get; set; }
        public List<TKD_KikyujData> Kikyujdriverlst { get; set; }
        public List<TKD_KikyujData> Kikyujguiderlst { get; set; }
        public List<TKD_KobanData> Kobandriverlst { get; set; }
        public List<HaishadrgdData> haishadrgdData { get; set; }

    }
    public class HaishadrgdData
    {
        public DateTime date { get; set; }
        public int allDrivers { get; set; }
        public int allGuides { get; set; }
        public int largeDriver { get; set; }
        public int MediumDriver { get; set; }
        public int SmallDriver { get; set; }
        public int EigyoCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public int KSKbn { get; set; }
    }
    public class TPM_KyoSHeData
    {
        public int SyainCdSeq { get; set; }
        public int SyokumuCdSeq { get; set; }
        public int EigyoCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public string TenkoNo { get; set; }
        public string StaYmd { get; set; }
        public string EndYmd { get; set; }
        public int SyokumuKbn { get; set; }
        public byte BigTypeDrivingFlg { get; set; }
        public byte MediumTypeDrivingFlg { get; set; }
        public byte SmallTypeDrivingFlg { get; set; }
    }
    public class TKD_KikyujData
    {
        public int KinKyuTblCdSeq { get; set; }
        public int SyainCdSeq { get; set; }
        public string KinKyuSYmd { get; set; }
        public string KinKyuSTime { get; set; }
        public string KinKyuEYmd { get; set; }
        public string KinKyuETime { get; set; }
        public int KinKyuCdSeq { get; set; }
        public string BikoNm { get; set; }

    }
    public class TKD_KobanData 
    {
        public string UnkYmd { get; set; }
        public int SyainCdSeq { get; set; }
        public int KouBnRen { get; set; }
        public int KinKyuTblCdSeq { get; set; }
        public byte KinKyuKbn { get; set; }
        public byte KyusyutsuKbn { get; set; }
        public byte BigTypeDrivingFlg { get; set; }
        public byte MediumTypeDrivingFlg { get; set; }
        public byte SmallTypeDrivingFlg { get; set; }
    }
}
