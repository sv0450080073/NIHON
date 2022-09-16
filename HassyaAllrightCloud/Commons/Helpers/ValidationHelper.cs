using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public static class ValidationHelper
    {
        #region Rate validation

        /// <summary>
        /// Validate for range of the rate.
        /// </summary>
        /// <param name="rate">Rate will be validated</param>
        /// <returns>
        /// <c>true</c> if rate is valid, otherwise <c>false</c>
        /// </returns>
        public static bool ValidateRateRange(string rate, float minRate, float maxRate)
        {
            bool isSuccess = float.TryParse(rate.Normalize(System.Text.NormalizationForm.FormKC), out float rateValue);

            if (isSuccess)
            {
                isSuccess = (rateValue >= minRate) && (rateValue <= maxRate);

                if (isSuccess)
                {
                    var strs = rate.ToString().Split(".");

                    isSuccess = (strs.Length == 1 || strs[1].Length <= 1); //make sure after mid-point just contain one or none character
                }
            }

            return isSuccess;
        }

        #endregion

        #region Time validation

        /// <summary>
        /// Validate for range of the time. Format time : HHmm
        /// <para>Example: 2359 => Hours: 23, Minutes: 59</para>
        /// </summary>
        /// <param name="time">Time as string will be validated</param>
        /// <param name="maxHours">Max hours</param>
        /// <param name="maxMinutes">Max minutes</param>
        /// <returns>
        /// <c>true</c> if time is in range, otherwise <c>false</c>
        /// </returns>
        public static bool ValidationTimeRange(string time, int maxHours, int maxMinutes)
        {
            if (string.IsNullOrEmpty(time))
                return false;

            //make sure time as string just contain number and had length == 4 match with format: HHmm and not contain any white space
            bool isSuccess = int.TryParse(time, out int timeInt) && time.Length == 4 && !time.Contains(" ");

            if (isSuccess)
            {
                int hour = int.Parse($"{time[0]}{time[1]}");
                int minute = int.Parse($"{time[2]}{time[3]}");

                //make sure time in range
                isSuccess = (hour <= maxHours && hour >= 0 && minute <= maxMinutes && minute >= 0);
            }

            return isSuccess;
        }

        /// <summary>
        /// Validation for time value when input.
        /// </summary>
        /// <returns>
        /// <c>true</c> if rate is valid, otherwise <c>false</c>
        /// </returns>
        public static bool ValidationInputTimeText(string time)
        {
            if (time == null)
                return false;

            time = time.Replace(":", "");

            //make sure time lengt in range [0, 4] and does not contain any white space
            return ((time.Length < 5 && time.Length > 0 && CommonUtil.NumberTryParse(time, out int tmp)) || time.Length == 0) && !time.Contains(" ");
        }

        #endregion
        #region CustomItem validation
        public static CustomFieldValidation ValidationCustomItem(CustomFieldConfigs fieldConfigs, string value , bool isEditNormal =true)
        {
            bool isError = false;
            string errorMessage = "";
            if (fieldConfigs.IsRequired && isEditNormal)
            {
                switch (fieldConfigs.CustomFieldType)
                {
                    case FieldType.Text:
                        SetErrorValue(fieldConfigs.Label, value, ref isError, ref errorMessage);
                        break;
                    case FieldType.Number:                       
                        SetErrorValue(fieldConfigs.Label,value, ref isError, ref errorMessage);                       
                        break;
                    case FieldType.Combobox:
                        SetErrorValue(fieldConfigs.Label,value, ref isError, ref errorMessage);
                        break;
                    case FieldType.Checkbox:
                        SetErrorValue(fieldConfigs.Label,value, ref isError, ref errorMessage);
                        break;
                    case FieldType.Date:
                        SetErrorValue(fieldConfigs.Label,value, ref isError, ref errorMessage);
                        break;
                    case FieldType.Time:
                        SetErrorValue(fieldConfigs.Label,value, ref isError, ref errorMessage);
                        break;
                    case FieldType.RadioButton:
                        SetErrorValue(fieldConfigs.Label,value, ref isError, ref errorMessage);
                        break;
                    default:
                        break;
                }
            }
            if(fieldConfigs.CustomFieldType == FieldType.Number)
            {
                int result = 0;
                int numMin = 0;
                int numMax = 0;
                bool checkParse = int.TryParse(value, out result);
                int.TryParse(fieldConfigs.NumMin, out numMin);
                int.TryParse(fieldConfigs.NumMax, out numMax);
                if (checkParse)
                {
                    if (result < numMin || result > numMax)
                    {
                        isError = true;
                        errorMessage = SetErrorMessage(numMin, numMax);
                    }
                }                
            }
            return new CustomFieldValidation { IsError = isError, ErrorMessage = errorMessage};
        }
        private static void SetErrorValue(string fieldName ,string value, ref bool isError ,ref string errorMessage )
        {
            if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
            {
                isError = true;
                errorMessage = fieldName + Constants.Constants.ErrorMessage.FieldEmpty;
            }
        }
        private static string SetErrorMessage(int numMin , int numMax)
        {
            return $"{numMin} ～ {numMax} で入力してください。";
        }
        #endregion
    }
}
