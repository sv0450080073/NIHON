using SharedLibraries.UI.Components;
using System;
using System.Threading.Tasks;

namespace SharedLibraries.UI.Models
{
    public interface IKoboFile
    {
        DateTime LastModified { get; }

        string Name { get; }

        long Size { get; }

        string Type { get; }

        public string RelativePath { get; set; }

        Task<byte[]> Data { get; }

        public int Progress { get; set; }

        event EventHandler OnDataRead;
    }

    public class KoboFile : IKoboFile
    {
        internal KoboInputFile Owner { get; set; }

        public event EventHandler OnDataRead;

        public int Id { get; set; }

        public DateTime LastModified { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string Type { get; set; }

        public string RelativePath { get; set; }

        public Task<byte[]> Data
        {
            get
            {
                return Owner.GetFileBytes(this);
            }
        }

        public int Progress { get; set; }

        internal void RaiseOnDataRead()
        {
            OnDataRead?.Invoke(this, null);
        }
    }
}
