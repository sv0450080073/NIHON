using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SharedLibraries.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedLibraries.UI.Components
{
    public class KoboMenuBase : ComponentBase
    {
        [Parameter] public List<KoboMenuItemModel> MenuItems { get; set; }
        [Parameter] public Dictionary<int, string> ServiceEndpoints { get; set; }
        [Parameter] public bool IsMinimize { get; set; }
        [Parameter] public string LogoTitle { get; set; }
        [Parameter] public string LogoUrl { get; set; }
        [Parameter] public string HomeUrl { get; set; }
        [Inject] IJSRuntime _js { get; set; }
        [Inject] protected IStringLocalizer<CommonResource> _lang { get; set; }
        [Inject] protected NavigationManager _nav { get; set; }
        bool firstLoad = true;
        protected Dictionary<Guid, KoboMenuItem> items = new Dictionary<Guid, KoboMenuItem>();
        protected override void OnParametersSet()
        {
            if (firstLoad && MenuItems != null && MenuItems.Any())
            {
                InitMenu();
                firstLoad = false;
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _js.InvokeVoidAsync("initMenu");
            }
        }

        /// <summary>
        /// Set active item
        /// </summary>
        protected void InitMenu()
        {
            var relativePath = _nav.Uri.Replace(_nav.BaseUri, string.Empty);
            var result = SetItemActive(MenuItems, relativePath);
            // There is no active item => Set the second item expand
            if (!result && MenuItems != null && MenuItems.Any())
                MenuItems.First().IsExpand = true;
        }

        protected bool SetItemActive(List<KoboMenuItemModel> items, string relativePath)
        {
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.KinouUrl) &&
                    !string.IsNullOrWhiteSpace(item.KinouUrl) &&
                    relativePath.StartsWith(item.KinouUrl))
                {
                    item.IsActive = true;
                    return true;
                }
                else
                {
                    if (item.Children != null)
                    {
                        var isActive = SetItemActive(item.Children, relativePath);
                        if (isActive)
                        {
                            item.IsExpand = true; 
                            return isActive;
                        }
                    }
                }
            }
            return false;
        }

        protected void OnChildrenExpand(Guid id)
        {
            foreach (var item in items)
            {
                item.Value.Toggle(id);
            }
        }

        protected void OnChildrenActive(Guid id)
        {
            foreach (var item in items)
            {
                item.Value.DeActive(id);
            }
        }
    }
}
