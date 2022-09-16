using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TKD_FavoriteSiteData
    {
        public int FavoriteSite_FavoriteSiteCdSeq { get; set; }
        public string FavoriteSite_SiteTitle { get; set; }
        public string FavoriteSite_SiteUrl { get; set; }
        public short FavoriteSite_DisplayOrder { get; set; }
    }
}
