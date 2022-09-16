using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.DailyBatchCopy
{
    public class ListDataBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<Pages.DailyBatchCopy> _lang { get; set; }

        [Parameter]
        public List<DailyBatchCopyData> listData { get; set; }
        [Parameter]
        public EventCallback<DailyBatchCopyData> RemoveItem { get; set; }

        public List<DailyBatchCopyData> listDataDisplay { get; set; } = new List<DailyBatchCopyData>();

        public byte itemPerPage { get; set; } = 25;
        public int currentPage { get; set; } = 0;
        protected Pagination paging = new Pagination();

        protected override void OnInitialized()
        {
            currentPage = 0;
            itemPerPage = 25;
            paging.currentPage = 0;
            listDataDisplay = listData.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
        }

        protected override void OnParametersSet()
        {
            listDataDisplay = listData.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
            StateHasChanged();
        }

        protected async Task OnRemoveItem(DailyBatchCopyData item)
        {
            await RemoveItem.InvokeAsync(item);
        }

        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            itemPerPage = _itemPerPage;
            StateHasChanged();
        }

        protected void OnChangePage(int page)
        {
            currentPage = page;
            listDataDisplay = listData.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
            StateHasChanged();
        }
    }
}
