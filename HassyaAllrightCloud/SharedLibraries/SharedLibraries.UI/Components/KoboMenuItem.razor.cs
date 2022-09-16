using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SharedLibraries.UI.Models;

namespace SharedLibraries.UI.Components
{
    public class KoboMenuItemBase : ComponentBase
    {
        [Parameter] public byte Depth { get; set; }
        [Parameter] public KoboMenuItemModel MenuItem { get; set; }
        [Parameter] public EventCallback<Guid> OnExpand { get; set; }
        [Parameter] public EventCallback<Guid> OnActive { get; set; }
        [Parameter] public Dictionary<int, string> ServiceEndpoints { get; set; }
        [Parameter] public bool IsMinimize { get; set; }
        [Inject] NavigationManager _nav { get; set; }
        [Inject] IJSRuntime _js { get; set; }
        protected string cssClass = "";
        public bool showClass = false;
        byte _count = 0;
        protected override void OnInitialized()
        {
            cssClass = MenuItem.IsExpand ? "kobo-display-block" : "kobo-display-none";

        }
        protected Dictionary<Guid, KoboMenuItem> items = new Dictionary<Guid, KoboMenuItem>();
        protected override void OnAfterRender(bool firstRender)
        {
            if (_count == 1)
            {
                // Only need this attribute before first click
                cssClass = string.Empty;
                _count++;
                StateHasChanged();
            }
        }

        protected async Task ItemClicked(KoboMenuItemModel item, MouseEventArgs e)
        {
            if (IsMinimize && Depth == 1)
            {
                return;
            }
            else
            {
                if (item.Children != null && item.Children.Any())
                {
                    item.IsExpand = !item.IsExpand;
                    if (_count <= 1)
                        _count++;
                    await _js.InvokeVoidAsync("toggle", MenuItem.Id.ToString(), MenuItem.IsExpand);
                    if (item.IsExpand && !IsMinimize)
                    {
                        await OnExpand.InvokeAsync(MenuItem.Id);
                    }
                }
                else if (!string.IsNullOrEmpty(item.KinouUrl))
                {
                    MenuItem.IsActive = true;
                    await OnActive.InvokeAsync(MenuItem.Id);
                    if (ServiceEndpoints.ContainsKey(item.ServiceCdSeq))
                    {
                        _nav.NavigateTo($"{ServiceEndpoints[item.ServiceCdSeq]}/{item.KinouUrl}");
                    }
                    else
                    {
                        // Hard code for only "bookinginput" to force load 
                        if (item.KinouUrl == "bookinginput" && item.ServiceCdSeq == 1)
                        {
                            var param = _nav.Uri.Split('?').Count();
                            if (param == 1)
                            {
                                _nav.NavigateTo(item.KinouUrl);
                            }
                            else
                            {
                                _nav.NavigateTo(item.KinouUrl, true);
                            }
                        }
                        else
                            _nav.NavigateTo(item.KinouUrl);
                    }
                }
            }
        }

        protected async Task OnChildrenExpand(Guid id)
        {
            foreach (var item in items)
            {
                item.Value.Toggle(id);
            }
            await OnExpand.InvokeAsync(MenuItem.Id);
        }

        public void Toggle(Guid id)
        {
            if (MenuItem.Id != id)
            {
                _js.InvokeVoidAsync("toggle", MenuItem.Id.ToString(), false);
                if (_count <= 1)
                    _count++;
                MenuItem.IsExpand = false;
            }
        }

        public void DeActive(Guid id)
        {
            if (MenuItem.Id != id)
            {
                MenuItem.IsActive = false;
            }

            foreach (var item in items)
            {
                item.Value.DeActive(id);
            }
            StateHasChanged();
        }

        protected Task OnActiveCallBack(Guid id)
        {
            return OnActive.InvokeAsync(id);
        }
    }
}
