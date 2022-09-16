using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.UI.Models
{
    public class KoboMenuItemModel
    {
        public int MenuCdSeq { get; set; }
        public byte Node { get; set; }
        public short Sort { get; set; }
        public int ParentMenuCdSeq { get; set; }
        public string Icon { get; set; }
        public string MenuNm { get; set; }
        public string KinouID { get; set; }
        public int ServiceCdSeq { get; set; }
        public string KinouUrl { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsExpand { get; set; }
        public bool IsActive { get; set; }

        public List<KoboMenuItemModel> Children{ get; set; }
    }
}
