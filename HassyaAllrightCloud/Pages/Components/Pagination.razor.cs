using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace HassyaAllrightCloud.Pages.Components
{
    public class PaginationBase : ComponentBase
    {
        [Inject] public IStringLocalizer<Pagination> _lang { get; set; }

        //[Parameter]
        //public int TotalPage { get; set; }
        [Parameter]
        public int TotalCount { get; set; }
        [Parameter] 
        public EventCallback<int> OnChangePage { get; set; }
        [Parameter]
        public byte ItemPerPage { get; set; }
        [Parameter]
        public EventCallback<byte> OnChangeItemPerPage { get; set; }

        public int currentPage { get; set; }
        public List<byte> listItemPerPage { get; set; } = new List<byte>() { 10, 25, 50, 100 };
        public int totalPage { get; set; }

        protected override void OnParametersSet()
        {
            totalPage = (int)Math.Ceiling(TotalCount * 1.0 / ItemPerPage);
            StateHasChanged();
        }

        protected void OnPageClick(int page)
        {
            if(page >= 0 && page <= totalPage - 1)
            {
                currentPage = page;
                OnChangePage.InvokeAsync(page);
                StateHasChanged();
            }
        }

        protected void ChangeItemPerPage(byte itemPerPage)
        {
            ItemPerPage = itemPerPage;
            OnChangeItemPerPage.InvokeAsync(ItemPerPage);
            totalPage = (int)Math.Ceiling(TotalCount * 1.0 / ItemPerPage);
            if (currentPage >= totalPage)
            {
                currentPage = totalPage - 1;
            }
            OnChangePage.InvokeAsync(currentPage);
            StateHasChanged();
        }

        protected int GetToValue()
        {
            int data = 0;
            if (TotalCount == 0) data = 0;
            else if (TotalCount % ItemPerPage != 0 && currentPage == totalPage - 1 && currentPage != 0) data = currentPage * ItemPerPage + TotalCount % ItemPerPage;
            else data = currentPage * ItemPerPage + ItemPerPage;

            if (data > TotalCount) return TotalCount;
            return data;
        }

        public void Reset()
        {
            TotalCount = 0;
            currentPage = 0;
            StateHasChanged();
        }
    }
}