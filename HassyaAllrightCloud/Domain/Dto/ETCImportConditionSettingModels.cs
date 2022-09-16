using HassyaAllrightCloud.Pages.Components.InputFileComponent;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class FileItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IFileListEntry File { get; set; }
        public List<ContentTableModel> Content { get; set; }
    }

    public class ContentTableModel
    {
        public string RawText { get; set; }
        public string Error { get; set; }
    }

    public class Model
    {
        public ConnectEtcType ConnectEtcType { get; set; }
        public Guid SelectedFile { get; set; }

    }

    public enum ConnectEtcType
    {
        Start,
        DoNotStart
    }
    public class ETCSettingModel
    {
        public int SyaRyoCdCol { get; set; }
        public int CardNoCol { get; set; }
        public int UnkYmdCol { get; set; }
        public int UnkTimeCol { get; set; }
        public int TankaCol { get; set; }
        public int IriRyoKinCol { get; set; }
        public int DeRyoKinCol { get; set; }
        public int JigyosIriCol { get; set; }
        public int JigyosDeCol { get; set; }
        public int KinEtcVal { get; set; }
        public int FutEtcVal { get; set; }
        /// <summary>
        /// 立替金通知書用　有料道路立替明細出力区分(左側)　1:付帯料金区分(FutGuiKbn)　2:付帯料金CdSeq
        /// </summary>
        public int TateOutPutFlagVal { get; set; }
        /// <summary>
        /// 立替金通知書用　有料道路立替明細出力内容　↑が1なら付帯料金区分、2なら付帯料金CdSeqを入れる。複数ある場合はカンマ区切り
        /// </summary>
        public int TateOutPutChoiceVal { get; set; }
    }

    public class ETCImportSearchModel
    {
        public int TenantCdSeq { get; set; }
        public string CardNo { get; set; }
        public string UnkYmd { get; set; }
        public string UnkTime { get; set; }
        public int SyaRyoCd { get; set; }
    }

    public class RyokinSearchModel
    {
        public string RyokinTikuCd { get; set; }
        public string RyokinCd { get; set; }
    }

    public class RyokinSplitSearchModel
    {
        public string IriRyoChiCd { get; set; }
        public string IriRyoCd { get; set; }
        public string DeRyoChiCd { get; set; }
        public string DeRyoCd { get; set; }
    }

    public class RyokinSplitModel
    {
        public string TargetEntranceRyokinTikuCd { get; set; }
        public string TargetEntranceRyokinCd { get; set; }
        public string TargetExitRyokinTikuCd { get; set; }
        public string TargetExitRyokinCd { get; set; }
        public int DividedNumber { get; set; }
        public string EntranceRyokinTikuCd { get; set; }
        public string EntranceRyokinCd { get; set; }
        public string ExitRyokinTikuCd { get; set; }
        public string ExitRyokinCd { get; set; }
        public int Fee { get; set; }
        public int Fee2 { get; set; }
        public int Fee3 { get; set; }
        public int Fee4 { get; set; }
        public int Fee5 { get; set; }
        public string SiyoStaYmd { get; set; }
        public string SiyoEndYmd { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgId { get; set; }
    }

    public class CommonModel
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class CurrentDateModel
    {
        public string Syshiduke { get; set; }
        public string Sysjikan { get; set; }
    }
}
