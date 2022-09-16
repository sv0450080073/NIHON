using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ReservationBooking
    {

        //Booking
        public static IEnumerable<string> BookingTypeList = new List<string>() {

    };

        //SaleBranch
        public static IEnumerable<string> SaleBranchList = new List<string>() {
    };

        //Staff
        public static IEnumerable<string> StaffList = new List<string>() {
    };



        //Supplier
        public static List<string> SupplierList = new List<string>() {

    };
        //Customer
        public static List<string> CustomerList = new List<string>() {

    };
        //Table
        public static IEnumerable<string> TypeList = new List<string>() {

    };

        //Tax
        public static IEnumerable<string> TaxTypeList = new List<string>() {
        "外税",
        "内税",
        "非課税"
    };
    }
}
