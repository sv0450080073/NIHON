namespace HassyaAllrightCloud.Commons.Constants
{
    public class Common
    {
        //using for temporary, because in prototype not have function login
        public static string UpdPrgId = "HO_NEW";
        public static int BusLeaveOrReturnGaraDuration = 60;
        public static int BusCheckBeforeOrAfterRunningDuration = 0;
        public static byte DefaultPageSize = 25;
        /// <summary>
        /// Define maximum of km running in <see cref="Domain.Dto.MinMaxSettingFormData"/>
        /// </summary>
        public static int LimitPage = 30;
    }

    /// <summary>
    /// CQRS command mode
    /// </summary>
    public enum CommandMode
    {
        Create = 0,
        Update = 1,
        Delete = 2,
    }
}
