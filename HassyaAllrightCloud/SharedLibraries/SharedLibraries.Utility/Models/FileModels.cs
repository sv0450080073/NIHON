using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Utility.Models
{

    public class S3File
    {
        public string EncryptedId { get; set; }
        public string EncryptedFolderId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Size { get; set; }
        public int TotalDownload { get; set; }
        public int SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdSyainNm { get; set; }
        public string UpdPrgID { get; set; }
        public string FileType { get; set; }
    }

    public class S3Folder
    {
        public string EncryptedId { get; set; }
        public int TenantCdSeq { get; set; }
        public string EncryptedParentId { get; set; }
        public string Name { get; set; }
        public byte SiyoKbn { get; set; }
        public string UpdYmd { get; set; }
        public string UpdTime { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdSyainNm { get; set; }
        public string UpdPrgID { get; set; }
        public bool HasChildren { get; set; }
    }

    public class TransferFile
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class FileSendData
    {
        public byte[] File { get; set; }
        public int UpdSyainCd { get; set; }
        public int TenantId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Password { get; set; }
        public int FileSize { get; set; }
        public string UpdPrgID { get; set; }
    }

    public class DownloadedInfo
    {
        public int No { get; set; }
        public string DownloadedDate { get; set; }
        public string DowloadedIP { get; set; }
    }
    public class UpdateFileStatusModel
    {
        public string FileId { get; set; }
        public int UpdSyainCd { get; set; }
        public string UpdPrgID { get; set; }
    }

    public class RenameFileModel
    {
        [Required]
        public string FileId { get; set; }
        [Required]
        public string NewName { get; set; }
        public string UpdPrgID { get; set; }
        public int UpdSyainCd { get; set; }
    }

    public class RenameFolderModel
    {
        [Required]
        public string FolderId { get; set; }
        [Required]
        public string NewName { get; set; }
        public string UpdPrgID { get; set; }
        public int UpdSyainCd { get; set; }
    }
    public class MoveFileModel
    {
        [Required]
        public string FromId { get; set; }
        [Required]
        public string ToId { get; set; }
        public string UpdPrgID { get; set; }
        public int UpdSyainCd { get; set; }
    }

    public class MoveFolderModel
    {
        [Required]
        public string FromId { get; set; }
        [Required]
        public string ToId { get; set; }
        public string UpdPrgID { get; set; }
        public int UpdSyainCd { get; set; }
    }
}
