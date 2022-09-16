using DevExpress.Blazor;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.IService.CommonComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.CommonComponents
{
    public class ReservationClassComponentBase : ComponentBase
    {
        [Parameter] public int DefaultValue { get; set; }
        [Parameter] public ReservationClassComponentData SelectedReservationClass { get; set; }
        [Parameter] public EventCallback<ReservationClassComponentData> SelectedReservationClassChanged { get; set; } = new EventCallback<ReservationClassComponentData>();
        [Parameter] public Expression<Func<Object>> ReservationClassExpression { get; set; }
        [Parameter] public bool ReadOnly { get; set; } = false;
        [Parameter] public Dictionary<string, string> LangDic { get; set; }
        [Parameter] public DropDownDirection DropDownDirection { get; set; } = DropDownDirection.Down;
        [Parameter] public DataEditorClearButtonDisplayMode ClearButtonDisplayMode { get; set; } = DataEditorClearButtonDisplayMode.Never;
        [Parameter] public ListRenderMode RenderMode { get; set; } = ListRenderMode.Entire;
        [Parameter] public string isAddNullItem { get; set; } = "0";
        [Parameter] public string CssClass { get; set; } = "w-100";

        [Inject] protected IStringLocalizer<Commons.CommonComponents> Lang { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }
        [Inject] protected IReservationClassComponentService _service { get; set; }

        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();
        public string searchString { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ListReservationClass = await _service.GetListReservationClass();
                // N.T.L.Anh Add str 2021/06/01
                if (isAddNullItem != "0")
                {
                    ListReservationClass.Insert(0, null);
                }
                // N.T.L.Anh Add end 2021/06/01
                if (DefaultValue != 0)
                {
                    SelectedReservationClass = ListReservationClass.Where(_ => _ != null).ToList().FirstOrDefault(_ => _.YoyaKbnSeq == DefaultValue);
                    await SelectedReservationClassChanged.InvokeAsync(SelectedReservationClass);
                }
            }
            catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnReservationClassChanged(object reservationClass)
        {
            try
            {
                SelectedReservationClass = reservationClass as ReservationClassComponentData;
                if (SelectedReservationClass != null)
                    SelectedReservationClass.IsSelect = true;
                SelectedReservationClassChanged.InvokeAsync(SelectedReservationClass);
            }
            catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void Enter(KeyboardEventArgs e)
        {
            try
            {
                if (e.Code != "Enter")
                    return;
                if(string.IsNullOrWhiteSpace(searchString))
                {
                    OnReservationClassChanged(null);
                    return;
                }
                var firstItem = ListReservationClass.FirstOrDefault(x => x != null && x.Text.Contains(searchString));
                if (firstItem == null)
                    return;
                OnReservationClassChanged(firstItem);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
