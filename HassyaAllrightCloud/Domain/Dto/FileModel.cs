namespace HassyaAllrightCloud.Domain.Dto
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] Base64 { get; set; }
        public string FolderName { get; set; }
    }
}
