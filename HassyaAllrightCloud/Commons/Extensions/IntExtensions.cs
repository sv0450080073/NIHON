

namespace HassyaAllrightCloud.Commons.Extensions
{
    public static class IntExtensions
    {

        public static string AddPaddingLeft(this int val, int numOfLoop, char charLeft = '0') => val.ToString().PadLeft(numOfLoop, charLeft);
    }
}
