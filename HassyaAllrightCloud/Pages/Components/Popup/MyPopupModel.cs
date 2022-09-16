using HassyaAllrightCloud.Commons.Constants;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Pages.Components.Popup
{
    public class MyPopupModel
    {
        public bool IsShow { get; private set; }
        public string Title { get; private set; }
        public string BodyText { get; private set; }
        public MyPopupIconType IconType { get; private set; }
        public List<MyPopupFooterButton> FooterButtons { get; private set; }

        public MyPopupModel()
        {
            FooterButtons = new List<MyPopupFooterButton>();
            IconType = MyPopupIconType.Info;
            IsShow = false;
        }

        public MyPopupModel Build()
        {
            FooterButtons.Clear();
            return this;
        }

        public MyPopupModel WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public MyPopupModel WithBody(string bodyText)
        {
            BodyText = bodyText;
            return this;
        }

        public MyPopupModel WithIcon(MyPopupIconType icon)
        {
            IconType = icon;
            return this;
        }

        public MyPopupModel AddButton(MyPopupFooterButton button)
        {
            FooterButtons.Add(button);
            return this;
        }

        public void Show() => IsShow = true;

        public void Hide() => IsShow = false;
    }
}
