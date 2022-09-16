using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class TkdFutTumComparer : IEqualityComparer<TkdFutTum>
    {
        public bool Equals([AllowNull] TkdFutTum x, [AllowNull] TkdFutTum y)
        {
            return x.UkeNo == y.UkeNo
                && x.UnkRen == y.UnkRen
                && x.FutTumKbn == y.FutTumKbn
                && x.FutTumRen == y.FutTumRen;
        }

        public int GetHashCode([DisallowNull] TkdFutTum obj)
        {
            int result = obj.UkeNo.GetHashCode() ^ obj.UnkRen ^ obj.FutTumKbn ^ obj.FutTumRen;
            return result.GetHashCode();
        }
    }

    public class TkdMFutTuComparer : IEqualityComparer<TkdMfutTu>
    {
        public bool Equals([AllowNull] TkdMfutTu x, [AllowNull] TkdMfutTu y)
        {
            return x.UkeNo == y.UkeNo
                && x.UnkRen == y.UnkRen
                && x.FutTumKbn == y.FutTumKbn
                && x.FutTumRen == y.FutTumRen
                && x.TeiDanNo == y.TeiDanNo
                && x.BunkRen == y.BunkRen;
        }

        public int GetHashCode([DisallowNull] TkdMfutTu obj)
        {
            int result = obj.UkeNo.GetHashCode()
                ^ obj.UnkRen
                ^ obj.FutTumKbn
                ^ obj.FutTumRen
                ^ obj.TeiDanNo
                ^ obj.BunkRen;
            return result.GetHashCode();
        }
    }

    public class TkdYfutTuComparer : IEqualityComparer<TkdYfutTu>
    {
        public bool Equals([AllowNull] TkdYfutTu x, [AllowNull] TkdYfutTu y)
        {
            return x.UkeNo == y.UkeNo
                && x.UnkRen == y.UnkRen
                && x.YouTblSeq == y.YouTblSeq
                && x.FutTumKbn == y.FutTumKbn
                && x.YouFutTumRen == y.YouFutTumRen;
        }

        public int GetHashCode([DisallowNull] TkdYfutTu obj)
        {
            int result = obj.UkeNo.GetHashCode() ^ obj.UnkRen ^ obj.YouTblSeq ^ obj.FutTumKbn ^ obj.YouFutTumRen;
            return result.GetHashCode();
        }
    }

    public class TkdTkdYmfuTuComparer : IEqualityComparer<TkdYmfuTu>
    {
        public bool Equals([AllowNull] TkdYmfuTu x, [AllowNull] TkdYmfuTu y)
        {
            return x.UkeNo == y.UkeNo
                && x.UnkRen == y.UnkRen
                && x.YouTblSeq == y.YouTblSeq
                && x.FutTumKbn == y.FutTumKbn
                && x.YouFutTumRen == y.YouFutTumRen
                && x.TeiDanNo == y.TeiDanNo
                && x.BunkRen == y.BunkRen;
        }

        public int GetHashCode([DisallowNull] TkdYmfuTu obj)
        {
            int result = obj.UkeNo.GetHashCode()
                ^ obj.UnkRen
                ^ obj.YouTblSeq
                ^ obj.FutTumKbn
                ^ obj.YouFutTumRen
                ^ obj.TeiDanNo
                ^ obj.BunkRen;
            return result.GetHashCode();
        }
    }

    public class SettingQuantityComparer : IEqualityComparer<SettingQuantity>
    {
        public bool Equals([AllowNull] SettingQuantity x, [AllowNull] SettingQuantity y)
        {
            return x.TeiDanNo == y.TeiDanNo
                && x.UnkRen == y.UnkRen
                && x.BunkRen == y.BunkRen;
        }

        public int GetHashCode([DisallowNull] SettingQuantity obj)
        {
            var hCode = obj.TeiDanNo ^ obj.UnkRen ^ obj.BunkRen;
            return hCode.GetHashCode();
        }
    }
}
