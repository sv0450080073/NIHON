using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SharedLibraries.UI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedLibraries.UI.Components
{
    public class KoboInputFileBase : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> UnmatchedParameters { get; set; }
        [Parameter] public EventCallback<IKoboFile[]> OnChange { get; set; }
        [Parameter] public string Id { get; set; }
        [Inject] IJSRuntime _js { get; set; }
        protected ElementReference elRef;
        protected IDisposable thisRef;
        protected KoboFile[] _files;
        protected const int FilePart = 20480;
        protected List<KoboFile> Files = new List<KoboFile>();

        [JSInvokable]
        public async Task HandleFileChange(KoboFile[] files, bool isEnd)
        {
            Files.AddRange(files);
            if (isEnd)
            {
                foreach (var f in files)
                {
                    f.Owner = (KoboInputFile)this;
                }

                await OnChange.InvokeAsync(Files.ToArray());
                Files.Clear();
            }

            _files = files;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                thisRef = DotNetObjectReference.Create(this);
                await _js.InvokeVoidAsync("inputFile.init", Id, elRef, thisRef);
            }
        }

        public async Task<byte[]> GetFileBytes(KoboFile file)
        {
            List<byte> buffer = new List<byte>();
            var count = (file.Size * 1.0 / FilePart) > 0 ? (file.Size / FilePart) + 1 : file.Size / FilePart;

            for (var i = 0; i < count; i++)
            {
                long start = i * FilePart;
                long moveTo = (i + 1) * FilePart > file.Size ? file.Size : (i + 1) * FilePart;
                var b = await _js.InvokeAsync<byte[]>("inputFile.readFileData", elRef, file.Id, start, moveTo);
                buffer.AddRange(b);

                file.Progress = (int)(moveTo * 100 / file.Size);
                file.RaiseOnDataRead();
            }
            return buffer.ToArray();
        }
    }
}
