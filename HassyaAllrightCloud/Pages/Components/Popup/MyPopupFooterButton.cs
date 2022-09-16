using DevExpress.Blazor;
using System;

namespace HassyaAllrightCloud.Pages.Components.Popup
{
    public class MyPopupFooterButton
    {
        public string Text { get; set; }
        public Action OnClick { get; set; }
        public ButtonRenderStyle ButtonStyle { get; set; }

        public MyPopupFooterButton(string text, ButtonRenderStyle buttonStyle, Action onClick)
        {
            Text = text;
            OnClick = onClick;
            ButtonStyle = buttonStyle;
        }

        public MyPopupFooterButton(string text, Action onClick) : this(text, ButtonRenderStyle.Primary, onClick)
        {
        }
    }
}
