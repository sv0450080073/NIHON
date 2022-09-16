using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class SelectedSaleBranchModel
    {
        public int Id { get; set; }
        public int CEigyoCd { get; set; } = -1;
        public int CEigyoCdSeq { get; set; }
        public string CEiComRyak { get; set; }
        public int CEiComCd { get; set; }
        public string CEigyoNm { get; set; }
        public string CEigyoRyak { get; set; }
        public string Text
        {
            get
            {
                return CEigyoCd == -1 ? string.Empty : string.Format("{0} : {1}",CEigyoCd.ToString("D5"), CEigyoRyak );
            }
        }
    }
}
