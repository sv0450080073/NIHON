using DevExpress.XtraRichEdit.Unicode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class JourneysData
    {
        public string Haisha_UkeNo { get; set; } = "0";
        public int Yyksho_UkeCd { get; set; }
        public short Haisha_UnkRen { get; set; }
        public short Haisha_SyaSyuRen { get; set; }
        public short Haisha_TeiDanNo { get; set; }
        public short Haisha_BunkRen { get; set; }
        public string Haisha_GoSya { get; set; }
        public short Haisha_BunKSyuJyn { get; set; }
        public string Haisha_HaiSYmd { get; set; } = "";
        public string Haisha_HaiSTime { get; set; } = "";
        public string Haisha_TouYmd { get; set; } = "";
        public string Haisha_SyuPaTime { get; set; } = "";
        public string Haisha_TouChTime { get; set; } = "";
        public string Haisha_SyukoTime { get; set; } = "";
        public string Haisha_KikTime { get; set; } = "";
        public string Unkobi_TouChTime { get; set; } = "";
        public string Unkobi_SyuPaTime { get; set; } = "";
        public int Unkobi_ZenHaFlg { get; set; }
        public int Unkobi_KhakFlg { get; set; }
        public string Unkobi_HaiSYmd { get; set; } = "";
        public string Unkobi_SyukoYmd { get; set; } = "";
        public string Unkobi_HaiSTime { get; set; } = "";
        public string Unkobi_SyukoTime { get; set; } = "";
        public string Unkobi_KikTime { get; set; } = "";
        public string Unkobi_TouYmd { get; set; } = "";      
        public string Unkobi_KikYmd { get; set; } = "";      
        public string Textbun => Haisha_BunKSyuJyn != 0 ? $"-{Haisha_BunKSyuJyn.ToString("D3")}" : "";
        public string Text => Haisha_UkeNo != "0" ? $"{Haisha_GoSya}：" + "号車" + Textbun : "共通";

    }
}
