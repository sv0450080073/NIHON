using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TKD_FavoriteMenuData
    {
        public int FavoriteMenu_FavoriteMenuCdSeq { get; set; }
        public string FavoriteMenu_MenuTitle { get; set; }
        public string FavoriteMenu_MenuUrl { get; set; }
        public short FavoriteMenu_DisplayOrder { get; set; }
    }
}
