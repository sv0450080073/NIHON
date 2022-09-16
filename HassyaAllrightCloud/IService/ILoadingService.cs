using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ILoadingService
    {
        public Task ShowAsync();
        public void AddChangeEvent(Func<Task> change);
        public bool IsShow();
        public Task HideAsync();
    }

    public class LoadingService : ILoadingService, IDisposable
    {
        private event Func<Task> OnChange;
        public Guid InstanceId = Guid.NewGuid();
        public bool IsLoading { get; set; }
        private async Task NotifyStateChanged()
        {
            if (OnChange != null)
                await OnChange.Invoke();
        }
        public void AddChangeEvent(Func<Task> change) => OnChange = change;

        /// <summary>
        /// Hide loading component
        /// </summary>z
        public async Task HideAsync()
        {
            if (IsLoading)
            {
                await Task.Delay(100);
                IsLoading = false;
                await NotifyStateChanged();
            }
        }

        /// <summary>
        /// Show loading component
        /// </summary>
        public async Task ShowAsync()
        {
            if (!IsLoading)
            {
                await Task.Delay(50);
                IsLoading = true;
                await NotifyStateChanged();
            }
        }

        bool ILoadingService.IsShow() => IsLoading;

        public void Dispose()
        {
            OnChange = null;
        }
    }
}
