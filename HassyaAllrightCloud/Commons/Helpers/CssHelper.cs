using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class CssHelper
    {
        public static string GetFormEditStateCss(FormEditState state)
        {
            return state switch
            {
                FormEditState.Added => "mark-added",
                FormEditState.Edited => "mark-modified",
                FormEditState.None => "",
                _ => ""
            };
        }
    }
}
