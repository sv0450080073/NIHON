using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Utility.Models
{
    public class FolderModel
    {
        [Required]
        public string FolderName { get; set; }
        public int TenantCdSeq { get; set; }
        public int UpdSyainCd { get; set; }
        public string ParentId { get; set; }
        public string UpdPrgID { get; set; }
    }
}
