using HassyaAllrightCloud.Commons.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HassyaAllrightCloud.Commons.Constants.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class ComboboxFixField
    {
        public int IdValue { get; set; }
        public string StringValue { get; set; }
    }
    public class BillIssuedClassificationListData
    {
        public static List<ComboboxFixField> BillIssuedClassificationList = new List<ComboboxFixField>
        {
            BillIssuedClassificationCheckListConstants.All,
            BillIssuedClassificationCheckListConstants.Paid,
            BillIssuedClassificationCheckListConstants.Unpaid
        };

    }

    public class ReceivableUnpaidList
    {
        public static List<ComboboxFixField> ReceivableUnpaids = new List<ComboboxFixField>
        {
            ReceivableUnpaidConstants.Field0,
            ReceivableUnpaidConstants.Field1,
            ReceivableUnpaidConstants.Field2,
            ReceivableUnpaidConstants.Field3,
        };
    }

    public class BillTypeSortGridList
    {
        public static List<ComboboxFixField> BillTypeSortGridData = new List<ComboboxFixField>
        {
            BillTypeSortConstants.BillOrder,
            BillTypeSortConstants.OrderByDay
        };

    }
    public class BillTypePagePrintList
    {
        public static  List<ComboboxFixField> BillTypePagePrintData = new List<ComboboxFixField>
        {
            BillTypePageConstants.A4,
            BillTypePageConstants.A3,
            BillTypePageConstants.B4
        };

    }
    public class ShowHeaderOptions
    {
        public static List<ComboboxFixField> ShowHeaderOptionData = new List<ComboboxFixField>
        {
            ShowHeaderOption.OutWHeader,
            ShowHeaderOption.OutNotHeader
        };
    }
    public class GroupTypes
    {
        public static List<ComboboxFixField> GroupTypeData = new List<ComboboxFixField>
        {
            GroupTypeOption.Active,
            GroupTypeOption.No
        };
    }
    public class DelimiterTypes
    {
        public static List<ComboboxFixField> DelimiterTypeData = new List<ComboboxFixField>
        {
            DelimiterTypeOption.Tab,
            DelimiterTypeOption.Semicolon,
            DelimiterTypeOption.Comma,
            DelimiterTypeOption.Space,
            DelimiterTypeOption.FixedLength,
            DelimiterTypeOption.OtherCharacters
        };
    }
}
