using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IReportLoadingService : IDisposable
    {
        Task UpdateProgress(int progress, Guid key);
        bool IsCanceled(Guid key);
        void Initialize(Guid key, Func<int, Guid, Task> change);
        void AbortReport(Guid key);
        void Start(Guid reportId);
        void Stop(Guid reportId);
        Task UpdateProgress(int currentIndex, int total, Guid key);
    }

    public class ReportLoadingService : IReportLoadingService
    {
        private Dictionary<Guid, Func<int, Guid, Task>> Update = new Dictionary<Guid, Func<int, Guid, Task>>();
        Dictionary<Guid, bool> sources = new Dictionary<Guid, bool>();
        private async Task NotifyStateChanged(int progress, Guid key) => await Update[key]?.Invoke(progress, key);
        public async Task UpdateProgress(int progress, Guid key)
        {
            await NotifyStateChanged(progress, key);
        }
        public async Task UpdateProgress(int currentIndex, int total, Guid key)
        {
            await NotifyStateChanged(GetPercent(currentIndex, total), key);
        }

        private int GetPercent(int currentIndex, int total)
        {
            return currentIndex * 100 / total;
        }
        public void Dispose()
        {
            Update = null;
            sources = null;
        }

        public bool IsCanceled(Guid key) => !sources.ContainsKey(key) || !sources[key];

        public void Initialize(Guid key, Func<int, Guid, Task> change)
        {
            Update[key] = change;
            sources[key] = true;
        }

        public void AbortReport(Guid key)
        {
            sources.Remove(key);
            Update.Remove(key);
        }

        public async void Start(Guid reportId)
        {
            int progress = 0;
            while (true)
            {
                if (IsCanceled(reportId))
                {
                    await Update[reportId]?.Invoke(100, reportId);
                    break;
                }
                else
                {
                    if (progress < 99)
                        progress++;
                    await Update[reportId]?.Invoke(progress, reportId);
                }

                await Task.Delay(300);
            }
        }
        public void Stop(Guid reportId)
        {
            sources[reportId] = false;
        }
    }
}
