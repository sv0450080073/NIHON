#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1c36ffecad8ccd06cd9d35703bb2d43cbe5db539"
// <auto-generated/>
#pragma warning disable 1591
namespace HassyaAllrightCloud.Pages.Components.Home
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Infrastructure.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Domain.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Domain.Dto;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Commons.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Commons.Constants;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Commons.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using BlazorContextMenu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Application.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.Extensions.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.IService;

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 29 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot;

#line default
#line hidden
#nullable disable
#nullable restore
#line 30 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor.Reporting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Pages.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using SharedLibraries.UI.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 33 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Blazored.Modal;

#line default
#line hidden
#nullable disable
#nullable restore
#line 34 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Blazored.Modal.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using SharedLibraries.UI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Radzen;

#line default
#line hidden
#nullable disable
#nullable restore
#line 38 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Radzen.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Pages.Components.CommonComponents;

#line default
#line hidden
#nullable disable
    public partial class MenuPopup : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Forms.EditForm>(0);
            __builder.AddAttribute(1, "EditContext", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.Forms.EditContext>(
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                        editFormContext

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Forms.EditContext>)((formContext) => (__builder2) => {
                __builder2.AddMarkupContent(3, "\r\n    ");
                __builder2.OpenComponent<HassyaAllrightCloud.Application.Validation.FluentValidator<MenuPopupValidator>>(4);
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(5, "\r\n    ");
                __builder2.OpenElement(6, "div");
                __builder2.AddAttribute(7, "class", "form-group d-flex flex-nowrap");
                __builder2.AddMarkupContent(8, "\r\n        ");
                __builder2.OpenElement(9, "label");
                __builder2.AddAttribute(10, "class", "col-form-label-sm mr-3 width--90 pl-2");
                __builder2.AddContent(11, 
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                                              Lang["Name"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(12, "\r\n        ");
                __builder2.OpenElement(13, "div");
                __builder2.AddAttribute(14, "class", "has-tooltip-error");
                __builder2.AddAttribute(15, "style", "width: 400px");
                __builder2.AddMarkupContent(16, "\r\n            ");
                __builder2.OpenComponent<HassyaAllrightCloud.Pages.Components.Tooltip>(17);
                __builder2.AddAttribute(18, "ValueExpressions", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.Object>>>(
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                         () => createMenu.FavoriteMenu_MenuTitle

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(19, "Lang", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Collections.Generic.Dictionary<System.String, System.String>>(
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                            LangDic

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(20, "Text", "");
                __builder2.AddAttribute(21, "Position", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<HassyaAllrightCloud.Commons.Constants.PositionTooltip>(
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                                       PositionTooltip.top

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(22, "\r\n            ");
                __builder2.OpenComponent<DevExpress.Blazor.DxTextBox>(23);
                __builder2.AddAttribute(24, "CssClass", "flex-grow-1 focus");
                __builder2.AddAttribute(25, "ClearButtonDisplayMode", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DevExpress.Blazor.DataEditorClearButtonDisplayMode>(
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                               DataEditorClearButtonDisplayMode.Auto

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(26, "TextChanged", new System.Action<System.String>(
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                      (newValue) => UpdateMenuTitle(newValue)

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(27, "Text", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                              createMenu.FavoriteMenu_MenuTitle

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(28, "TextExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                         () => createMenu.FavoriteMenu_MenuTitle

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(29, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(30, "\r\n    ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(31, "\r\n    ");
                __builder2.OpenElement(32, "div");
                __builder2.AddAttribute(33, "class", "form-group d-flex flex-nowrap");
                __builder2.AddMarkupContent(34, "\r\n        ");
                __builder2.OpenElement(35, "label");
                __builder2.AddAttribute(36, "class", "col-form-label-sm mr-3 width--90 pl-2");
                __builder2.AddContent(37, 
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                                              Lang["Link"]

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(38, "\r\n        ");
                __builder2.OpenElement(39, "div");
                __builder2.AddAttribute(40, "class", "has-tooltip-error");
                __builder2.AddAttribute(41, "style", "width: 400px");
                __builder2.AddMarkupContent(42, "\r\n            ");
                __builder2.OpenComponent<HassyaAllrightCloud.Pages.Components.Tooltip>(43);
                __builder2.AddAttribute(44, "ValueExpressions", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.Object>>>(
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                         () => createMenu.FavoriteMenu_MenuUrl

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(45, "Lang", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Collections.Generic.Dictionary<System.String, System.String>>(
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                            LangDic

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(46, "Text", "");
                __builder2.AddAttribute(47, "Position", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<HassyaAllrightCloud.Commons.Constants.PositionTooltip>(
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                                       PositionTooltip.top

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(48, "\r\n            ");
                __builder2.OpenComponent<DevExpress.Blazor.DxTextBox>(49);
                __builder2.AddAttribute(50, "CssClass", "flex-grow-1 focus");
                __builder2.AddAttribute(51, "ClearButtonDisplayMode", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DevExpress.Blazor.DataEditorClearButtonDisplayMode>(
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                               DataEditorClearButtonDisplayMode.Auto

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(52, "TextChanged", new System.Action<System.String>(
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                      (newValue) => UpdateMenuLink(newValue)

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(53, "Text", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 28 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                              createMenu.FavoriteMenu_MenuUrl

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(54, "TextExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<System.String>>>(
#nullable restore
#line 29 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
                                         () => createMenu.FavoriteMenu_MenuUrl

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(55, "\r\n        ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(56, "\r\n    ");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(57, "\r\n");
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\Pages\Components\Home\MenuPopup.razor"
       
    [Parameter] public int MenuId { get; set; }

    string content;
    public TKD_FavoriteMenuData createMenu;
    public EditContext editFormContext { get; set; }
    public Dictionary<string, string> LangDic = new Dictionary<string, string>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            LoadData();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    private async Task LoadData()
    {
        try
        {
            if (MenuId != 0)
            {
                createMenu = await TKD_FavoriteMenuService.GetFavoriteMenuById(Convert.ToInt32(MenuId));
                StateHasChanged();
            }
            else
            {
                createMenu = new TKD_FavoriteMenuData()
                {
                    FavoriteMenu_MenuTitle = "",
                    FavoriteMenu_MenuUrl = ""
                };
            }
            editFormContext = new EditContext(createMenu);
            var dataLang = Lang.GetAllStrings();
            LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    public async Task<string> Save()
    {
        try
        {
            if (editFormContext.Validate())
            {
                if (MenuId != 0)
                {
                    return await TKD_FavoriteMenuService.UpdateFavoriteMenu(createMenu);
                }
                else
                {
                    return await TKD_FavoriteMenuService.CreateFavoriteMenu(createMenu);
                }
            }
            else
            {
                return "";
            }
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            return "";
        }
    }

    public void UpdateMenuTitle(string title)
    {
        try
        {
            createMenu.FavoriteMenu_MenuTitle = title;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }

    public void UpdateMenuLink(string link)
    {
        try
        {
            createMenu.FavoriteMenu_MenuUrl = link;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
        }
    }



#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IErrorHandlerService errorModalService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITKD_FavoriteMenuService TKD_FavoriteMenuService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<MenuPopup> Lang { get; set; }
    }
}
#pragma warning restore 1591