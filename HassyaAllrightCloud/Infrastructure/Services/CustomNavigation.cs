using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Infrastructure.Services
{
    public class CustomNavigation
    {
        private readonly NavigationManager _navMagager;
        public CustomNavigation(NavigationManager navManager)
        {
            _navMagager = navManager;
        }

        public void NavigateTo(string uri, bool forceLoad = false)
        {
            string newUri = uri.TrimStart('/');
            //await Task.Run(() => { _navMagager.NavigateTo(newUri, forceLoad); });
            _navMagager.NavigateTo(newUri, forceLoad);
        }
        public string BaseUri { get => _navMagager.BaseUri;}
        public string Uri { get => _navMagager.Uri;}
        public Uri ToAbsoluteUri(string relativeUri)
        {
            return _navMagager.ToAbsoluteUri(relativeUri);
        }
        public string ToBaseRelativePath(string uri)
        {
            return _navMagager.ToBaseRelativePath(uri);
        }
    }
}
