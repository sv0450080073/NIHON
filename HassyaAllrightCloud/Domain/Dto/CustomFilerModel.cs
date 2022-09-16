using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CustomFilerModel
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string FilterName { get; set; }
        public int EmployeeId { get; set; }
        public int FilterId { get; set; }
    }
}
