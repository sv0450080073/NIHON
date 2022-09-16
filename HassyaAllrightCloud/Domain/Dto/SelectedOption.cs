namespace HassyaAllrightCloud.Domain.Dto
{
    /// <summary>
    /// Custom list options
    /// </summary>
    /// <typeparam name="TOption">Type of the option</typeparam>
    public class SelectedOption<TOption> where TOption : struct, System.Enum
    {
        /// <summary>
        ///  Option was selected
        /// </summary>
        public TOption Option { get; set; }
        public string DisplayName { get; set; }
    }
}
