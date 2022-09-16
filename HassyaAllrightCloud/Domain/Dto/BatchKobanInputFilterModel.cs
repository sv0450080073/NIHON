using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Helpers.BookingInputHelper;


namespace HassyaAllrightCloud.Domain.Dto
{
    public class BatchKobanInputFilterModel
    {
        public DateTime KinmuYmd { get; set; }
        public MyTime TimeStart { get; set; }
        public MyTime TimeEnd { get; set; }
        public CompanyData Company { get; set; }
        public LoadSaleBranchList EigyoStart { get; set; }
        public LoadSaleBranchList EigyoEnd { get; set; }
        public Staffs SyainStart { get; set; }
        public Staffs SyainEnd { get; set; }
        public TaskModel SyokumuStart { get; set; }
        public TaskModel SyokumuEnd { get; set; }
        public ComboboxFixField SyuJun { get; set; }
        public ComboboxFixField DisplayKbn { get;set;}
    }
}
