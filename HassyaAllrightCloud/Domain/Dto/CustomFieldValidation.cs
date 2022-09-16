using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CustomFieldValidation
    {
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; } = "";
    }
}
