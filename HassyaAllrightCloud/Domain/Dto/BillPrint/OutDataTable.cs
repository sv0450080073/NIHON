using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto.BillPrint
{
    public class OutDataTable
    {
        public string UkeNo { get; set; }
        public byte SeiFutSyu { get; set; }
        public short FutuUnkRen { get; set; }
        public short FutTumRen { get; set; }
        public int supOutSiji { get; set; }
    }

    public class OutDataTableModel {
        public List<OutDataTable> outDataTables { get; set; } = new List<OutDataTable>();
    }

    public class OutDataTableOutput
    {
        public string EigyoNm { get; set; }
        public string SyoriYm { get; set; }
        public string TokuiZipCd { get; set; }
        public string TokuiJyus1 { get; set; }
        public string TokuiJyus2 { get; set; }
        public string TokuiTokuiNm { get; set; }
        public string TokuiSitenNm { get; set; }
        public string SeikyZipCd { get; set; }
        public string SeikyJyus1 { get; set; }
        public string SeikyJyus2 { get; set; }
        public string SeikyTokuiNm { get; set; }
        public string SeikySitenNm { get; set; }
        public int SeiEigyoCdSeq { get; set; }
        public int SeiEigyoCd { get; set; }
        public string SeiEigyoNm { get; set; }
    }
}
