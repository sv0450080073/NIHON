using DevExpress.Blazor;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.IService;
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
    public class DateInputComponentBase : ComponentBase
    {
        [Parameter] public Expression<Func<Object>> DateInputExpression { get; set; }
        [Parameter] public Dictionary<string, string> LangDic { get; set; }
        [Parameter] public DropDownDirection DropDownDirection { get; set; } = DropDownDirection.Down;
        [Parameter] public DataEditorClearButtonDisplayMode ClearButtonDisplayMode { get; set; } = DataEditorClearButtonDisplayMode.Never;
        [Parameter] public DateTime? SelectedDate { get; set; }
        [Parameter] public EventCallback<DateTime?> SelectedDateChanged { get; set; } = new EventCallback<DateTime?>();

        [Parameter] public string CssClass { get; set; } = "w-100";
        [Parameter] public string Format { get; set; } = "yyyy/MM/dd";
        [Parameter] public string DisplayFormat { get; set; } = "yyyy/MM/dd";
        [Parameter] public bool ReadOnly { get; set; } = false;

        [Inject] protected IStringLocalizer<Commons.CommonComponents> Lang { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }

        public string dateInputString { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await SelectedDateChanged.InvokeAsync(SelectedDate);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task OnDateChanged(object date)
        {
            try
            {
                if(ClearButtonDisplayMode == DataEditorClearButtonDisplayMode.Never)
                {
                    if (date == null)
                        return;
                    SelectedDate = (DateTime)date;
                } else
                    SelectedDate = (DateTime?)date;
                await SelectedDateChanged.InvokeAsync(SelectedDate);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task Enter(KeyboardEventArgs e)
        {
            try
            {
                if (e.Code != "Enter")
                    return;

                var date = CommonHelper.ConvertToDateTime(dateInputString);
                if (date == null)
                    return;
                await OnDateChanged((DateTime)date);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}
