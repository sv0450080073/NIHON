using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.UI.Models
{
    public class HeaderTemplate
    {
        public List<RowHeaderTemplate> Rows { get; set; }
        public int StickyCount { get; set; }
    }

    public class GridTemplate
    {
        public HeaderTemplate Header { get; set; }
        public BodyTemplate Body { get; set; }
    }

    public enum SelectedModeEnum
    {
        Single,
        Multiple
    }

    public class ShowCheckboxArgs<T>
    {
        public Func<T, T, bool> RowIdentifier { get; set; }
        public Func<T, bool> Disable { get; set; }
    }

    public class BodyTemplate
    {
        [JsonIgnore]
        public Func<dynamic, string> CustomCssDelegate { get; set; }
        public List<RowBodyTemplate> Rows { get; set; }
    }
    public class RowClickEventArgs<T>
    {
        public T SelectedItem { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDbClick { get; set; }
        public MouseEventArgs Event { get; set; }
    }

    public class CheckedChangeEventArgs<T>
    {
        public bool IsChecked { get; set; }
        public List<T> CheckedItems{ get; set; }
        public T CheckedItem { get; set; }
        public bool IsCheckAll { get; set; }
    }

    public class RowBodyTemplate : RowTemplate<ColumnBodyTemplate>
    {
        public bool IsRemoveChildBorder { get; set; }
    }
    public class RowTemplate<T>
    {
        public List<T> Columns { get; set; }
    }

    public class ColumnBodyTemplate : ColumnTemplate
    {
        [JsonIgnore]
        public Func<object, string> CustomTextFormatDelegate { get; set; }
        public ControlBase Control { get; set; }
        public string DisplayFieldName { get; set; }
        public AlignColEnum AlignCol { get; set; }
        public string CodeName { get; set; }
        public string CustomCss { get; set; }
    }

    public class ColumnTemplate
    {
        private int _colSpan { get; set; }
        private int _rowSpan { get; set; }
        public int RowSpan
        {
            get
            {
                return _rowSpan == 0 ? 1 : _rowSpan;
            }
            set { _rowSpan = value; }
        }
        public int ColSpan
        {
            get
            {
                return _colSpan == 0 ? 1 : _colSpan;
            }
            set { _colSpan = value; }
        }
    }

    public class ColumnLocation
    {
        public int RelativeIndex { get; set; }
        public int ActualIndex { get; set; }
    }


    public enum AlignColEnum
    {
        Left,
        Right,
        Center
    }

    public enum DisplayTextHeader
    {
        Horizontal,
        Vertical
    }

    public class RowHeaderTemplate : RowTemplate<ColumnHeaderTemplate>
    {
    }

    public class ColumnHeaderTemplate : ColumnTemplate
    {
        public string CssClass { get; set; }
        private int _width { get; set; }
        [Required]
        public string ColName { get; set; }
        public int Width { get => _width == 0 ? 100 : _width; set { _width = value; } }
        public string CodeName { get; set; }
        public string ParentCodeName { get; set; }
        public DisplayTextHeader TextDisplay { get; set; } = DisplayTextHeader.Horizontal;
    }

    public class CellColorControl<T>: ControlBase
    {
        public Func<T, string> ColorDelegate { get; set; }
    }

    /// <summary>
    /// In case you want to display icon in 1 cell of table
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IconControl<T> : ControlBase
    {
        public string IconClass { get; set; }
        public Action<T> CallBack { get; set; }
    }

    /// <summary>
    /// In case you want to display multiple lines in 1 cell of table
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiLineControl<T> : ControlBase
    {
        public Func<T, IEnumerable<string>> MutilineText { get; set; }

    }

    public class TableControl<TValue> : ControlBase
    {
        public Func<TValue, IReadOnlyList<IReadOnlyList<string>>> TableValue { get; set; }
    } 

    public class ControlBase
    {

    }

    public class TempRow
    {
        public int Index { get; set; }
        public int ColSpan { get; set; }
        public int RowSpan { get; set; }
    }

    public class SwapCol
    {
        public int RowIndex { get; set; }
        public int SrcIndex { get; set; }
        public int DestIndex { get; set; }
    }
}
