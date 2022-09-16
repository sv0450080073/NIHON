using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using SharedLibraries.UI.Models;
using Microsoft.Extensions.Localization;

namespace SharedLibraries.UI.Components
{
    public class KoboSimpleGridBase<T> : ComponentBase, IDisposable
    {
        [Parameter] public ShowCheckboxArgs<T> ShowCheckbox { get; set; }
        [Parameter] public HeaderTemplate Header { get; set; }
        [Parameter] public BodyTemplate Body { get; set; }
        [Parameter] public List<T> DataItems { get; set; }
        [Parameter] public string CssClass { get; set; }
        [Parameter] public bool EnableSaveBtn { get; set; }
        [Parameter] public SelectedModeEnum SelectedMode { get; set; }
        [Parameter] public Action<RowClickEventArgs<T>> OnRowClick { get; set; }
        [Parameter] public Action<bool> OnSave { get; set; }
        [Parameter] public Action<T> OnRowDbClick { get; set; }
        [Parameter] public Action<CheckedChangeEventArgs<T>> CheckedItemsChanged { get; set; }
        [Parameter] public List<T> CheckedItems { get; set; } = new List<T>();
        [Parameter] public bool IsSelectable { get; set; } = true;
        [Parameter] public List<T> SelectedItems { get; set; } = new List<T>();
        [Parameter] public bool ShowEmptyRow { get; set; } = true;
        [Inject] private IJSRuntime _jsRuntime { get; set; }
        [Inject] protected IStringLocalizer<CommonResource> _lang { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Save actual index and relative index of cols
        /// </summary>
        private List<Dictionary<int, int>> RowLocations = new List<Dictionary<int, int>>();
        protected T SelectedItem { get; set; }
        protected bool isCheckAll;
        IDisposable thisReference;

        [JSInvokable]
        public void UpdateColOrder(int startIndex, int endIndex)
        {
            if (ShowCheckbox != null)
            {
                startIndex--;
                endIndex--;
            }

            int srcColSpan, destColSpan;
            (srcColSpan, destColSpan) = SwapCol<RowHeaderTemplate, ColumnHeaderTemplate>(startIndex, endIndex, Header.Rows);
            SwapCol<RowBodyTemplate, ColumnBodyTemplate>(startIndex, endIndex, Body.Rows, srcColSpan, destColSpan);
            InitTablePositions();
            StateHasChanged();
        }

        [JSInvokable]
        public void UpdateColWidth(int rowIndex, int colIndex, int width)
        {
            if (ShowCheckbox != null) colIndex -= 1;
            if (Header.Rows != null && rowIndex <= Header.Rows.Count && colIndex <= Header.Rows[rowIndex].Columns.Count)
            {
                var balance = width - Header.Rows[rowIndex].Columns[colIndex].Width;
                Header.Rows[rowIndex].Columns[colIndex].Width = width;
                UpdateParentWidth(rowIndex, colIndex, balance);
                UpdateChildrenWidth(rowIndex, colIndex, balance);
            }

            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            // Todo: Add Validations to throw meaningful error for developer
            var context = new ValidationContext(Header, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(Header, context, results, true))
                throw new Exception($"{nameof(KoboSimpleGrid<T>)} throw an error: {results.First().ErrorMessage}");
            context = new ValidationContext(Body, serviceProvider: null, items: null);
            if (!Validator.TryValidateObject(Body, context, results, true))
                throw new Exception($"{nameof(KoboSimpleGrid<T>)} throw an error: {results.First().ErrorMessage}");
            InitTablePositions();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var colHeight = 2;
            var borderWidth = 2;
            thisReference = DotNetObjectReference.Create(this);
            if (firstRender)
                await _jsRuntime.InvokeVoidAsync("KoboGrid.initColEvents", Header.StickyCount, borderWidth, Id, thisReference);

            await _jsRuntime.InvokeVoidAsync("KoboGrid.updateGridSpliter", Id);
            await _jsRuntime.InvokeVoidAsync("KoboGrid.initLeftAndTop", colHeight, borderWidth, Header.StickyCount, Id);
        }

        
        private void InitTablePositions()
        {
            RowLocations.Clear();
            var tempTable = new List<TempRow>();
            for (var i = 0; i < Header.Rows.Count; i++)
            {
                var cols = Header.Rows[i].Columns;
                var colLocations = new Dictionary<int, int>();

                if (tempTable.Any())
                {
                    tempTable.ForEach(e => e.RowSpan--);
                    tempTable = tempTable.Where(e => e.RowSpan > 0).ToList();
                }

                var colCount = 0;
                for (var j = 0; j < cols.Count; j++)
                {
                    var col = cols[j];
                    colCount++;
                    while (true)
                    {
                        var temp = tempTable.Find(r => r.Index == colCount);

                        if (temp != null)
                        {
                            AddCol2Dic(-1, colCount, temp.ColSpan, colLocations);
                            colCount += temp.ColSpan;
                        }
                        else break;
                    }

                    if (col.RowSpan > 1)
                    {
                        tempTable.Add(new TempRow() { ColSpan = col.ColSpan, Index = colCount, RowSpan = col.RowSpan });
                    }

                    AddCol2Dic(j, colCount, col.ColSpan, colLocations);
                    colCount += col.ColSpan > 1 ? col.ColSpan - 1 : 0;
                }
                RowLocations.Add(colLocations);
            }
        }

        private void AddCol2Dic(int relativeIndex, int colCount, int colSpan, Dictionary<int, int> dics)
        {
            --colCount;
            for (var i = 0; i < colSpan; i++)
            {
                // actual index, relative index
                dics.Add(colCount + i, relativeIndex);
            }
        }

        private void UpdateParentWidth(int rowIndex, int colIndex, int balance)
        {
            
            if (rowIndex > 0)
            {
                var actualIndex = RowLocations[rowIndex].FirstOrDefault(c => c.Value == colIndex).Key;
                for (var i = 0; i < rowIndex; i++)
                {
                    var relativeParentIndex = RowLocations[i].FirstOrDefault(c => c.Key == actualIndex).Value;
                    Header.Rows[i].Columns[relativeParentIndex].Width += balance;
                }
            }
        }


        private void UpdateChildrenWidth(int rowIndex, int colIndex, int balance)
        {
            
            if (rowIndex < RowLocations.Count)
            {
                var listActualIndex = RowLocations[rowIndex]?.Where(c => c.Value == colIndex)?.Select(c => c.Key);
                if(listActualIndex != null && listActualIndex.Any())
                {
                    for (var i = rowIndex + 1; i < RowLocations.Count; i++)
                    {
                        var listRelativeIndex = RowLocations[i].Where(c => listActualIndex.Contains(c.Key) && c.Value != -1).Select(c => c.Value).Distinct().ToList();
                        if (listRelativeIndex.Any())
                        {
                            var remainder = balance % listRelativeIndex.Count;
                            var childWidth = balance / listRelativeIndex.Count;
                            var temp = 0;
                            foreach (var index in listRelativeIndex)
                            {
                                temp++;
                                if (temp == listRelativeIndex.Count)
                                    Header.Rows[i].Columns[index].Width += childWidth + remainder;
                                else
                                    Header.Rows[i].Columns[index].Width += childWidth;
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            Header = null;
            Body = null;
            DataItems = null;
            SelectedItems = null;
            CheckedItemsChanged = null;
        }

        protected string GetTextAlignClass(ColumnBodyTemplate col)
        {
            switch (col.AlignCol)
            {
                case AlignColEnum.Center:
                    return "text-center";
                case AlignColEnum.Left:
                    return "text-left";
                case AlignColEnum.Right:
                    return "text-right";
                default:
                    return string.Empty;
            }
        }

        private (int, int) SwapCol<R, C>(int srcIndex, int destIndex, List<R> rows, int srcColSpan = -1, int destColSpan = -1) where R : RowTemplate<C> where C : ColumnTemplate
        {
            // Save columns has rowspan > 1
            var tempTable = new List<TempRow>();
            // Temp list to save deleted columns
            var removeList = new List<TempRow>();
            // Contains Rows > Columns will be swap
            var swapCols = new List<SwapCol>();
            //int srcColSpan = 0, destColSpan = 0;
            if (rows.Any())
            {
                // Loop earch row
                for (var i = 0; i < rows.Count; i++)
                {
                    var tr = rows[i];
                    var cols = tr.Columns;
                    var colCount = 0;
                    var reSrcIndex = 0;
                    var reDestIndex = 0;
                    var foundSrc = false;
                    var foundDest = false;
                    int a = -1, b = -1;

                    // Each time move to new row -> remove columns does not have rowspan reach current row in tempTable
                    if (tempTable != null && tempTable.Count > 0)
                    {
                        foreach (var item in tempTable)
                        {
                            item.RowSpan--;
                            if (item.RowSpan < 1)
                                removeList.Add(item);
                        }

                        if (removeList != null && removeList.Count > 0)
                        {
                            foreach (var item in removeList)
                                tempTable.Remove(item);
                        }
                    }

                    // Loop each column
                    for (var j = 0; j < cols.Count; j++)
                    {

                        if (foundDest && foundSrc)
                            break;
                        else
                        {
                            var col = cols[j];
                            var colRowSpan = col.RowSpan;
                            var colColSpan = col.ColSpan;

                            // This variable will count the actual col index of current row.
                            colCount++;

                            while (true)
                            {
                                // Loop in tempTable in case the first col has rowspan > 1
                                var temp = tempTable.Find(r => r.Index == colCount);
                                if (temp != null)
                                {
                                    colCount += temp.ColSpan;
                                    if (colCount > srcIndex && a == -1)
                                        a = j;
                                    if (colCount > destIndex && b == -1)
                                        b = j;
                                }
                                else break;
                            }

                            if (colRowSpan > 1)
                                tempTable.Add(new TempRow() { Index = colCount, ColSpan = colColSpan, RowSpan = colRowSpan });

                            if (!foundSrc && colCount == srcIndex)
                            { foundSrc = true; reSrcIndex = j; }

                            if (!foundDest && colCount == destIndex)
                            { foundDest = true; reDestIndex = j; }

                            colCount += colColSpan > 1 ? colColSpan - 1 : 0;
                        }
                    }

                    var isSpecialCase = false;
                    if (srcIndex > destIndex)
                    {
                        if (foundSrc && !foundDest)
                        {
                            reDestIndex = b;
                            foundDest = true;
                        }
                    }
                    else
                    {
                        if (foundSrc && !foundDest)
                        {
                            if (colCount < destIndex)
                            {
                                reDestIndex = cols.Count - 1;
                                foundDest = true;
                                isSpecialCase = true;
                            }
                            else
                            {
                                reDestIndex = b - 1;
                                foundDest = true;
                            }
                        }
                    }

                    if (i == 0 && srcColSpan == -1 && destColSpan == -1)
                    {
                        srcColSpan = rows.First().Columns[reSrcIndex].ColSpan;
                        destColSpan = rows.First().Columns[reDestIndex].ColSpan;
                    }

                    if (foundDest && foundSrc)
                    {
                        if (srcIndex > destIndex)
                        {
                            var src = cols[reSrcIndex];
                            var tempSrcColSpan = src.ColSpan;
                            if (srcColSpan > 1 && tempSrcColSpan < srcColSpan)
                            {
                                var tempCount = 0;
                                // find the latest one of src
                                while (tempSrcColSpan < srcColSpan)
                                {
                                    tempCount++;
                                    var tempCol = cols[reSrcIndex + tempCount];
                                    tempSrcColSpan += tempCol.ColSpan;
                                }

                                for (var k = 0; k <= tempCount; k++)
                                {
                                    swapCols.Add(new SwapCol() { RowIndex = i, SrcIndex = reSrcIndex + tempCount, DestIndex = reDestIndex });
                                }
                            }
                            else
                            {
                                swapCols.Add(new SwapCol() { RowIndex = i, SrcIndex = reSrcIndex, DestIndex = reDestIndex });
                            }
                        }
                        else if (srcIndex < destIndex)
                        {
                            // fint the latest one of dest
                            var dest = cols[reDestIndex];
                            var tempDestColSpan = dest.ColSpan;
                            var tempDestIndex = reDestIndex;
                            if (destColSpan > 1 && tempDestColSpan < destColSpan && !isSpecialCase)
                            {
                                var tempCount = 0;
                                while (tempDestColSpan < destColSpan)
                                {
                                    tempCount++;
                                    var tempCol = cols[reDestIndex + tempCount];
                                    tempDestColSpan += tempCol.ColSpan;
                                }

                                tempDestIndex += tempCount;
                            }
                            var src = cols[reSrcIndex];
                            var tempSrcColSpan = src.ColSpan;
                            if (srcColSpan > 1 && tempSrcColSpan < srcColSpan)
                            {
                                var tempCount = 0;
                                // find the latest one of src
                                while (tempSrcColSpan < srcColSpan)
                                {
                                    tempCount++;
                                    var tempCol = cols[reSrcIndex + tempCount];
                                    tempSrcColSpan += tempCol.ColSpan;
                                }

                                for (var k = 0; k <= tempCount; k++)
                                {
                                    swapCols.Add(new SwapCol() { RowIndex = i, SrcIndex = reSrcIndex, DestIndex = tempDestIndex });
                                }
                            }
                            else
                            {
                                swapCols.Add(new SwapCol() { RowIndex = i, SrcIndex = reSrcIndex, DestIndex = tempDestIndex });
                            }
                        }
                    }
                }
                Swap<R, C>(swapCols, rows);
            }
            return (srcColSpan, destColSpan);
        }

        private void Swap<R, C>(List<SwapCol> swapCols, List<R> rows) where R : RowTemplate<C> where C : ColumnTemplate
        {
            if (swapCols != null && swapCols.Any())
            {
                for (var i = 0; i < swapCols.Count; i++)
                {
                    var item = swapCols[i];
                    var tr = rows[item.RowIndex];
                    var src = tr.Columns[item.SrcIndex];

                    if (item.SrcIndex > item.DestIndex)
                    {
                        var temp = src;

                        tr.Columns.RemoveAt(item.SrcIndex);
                        tr.Columns.Insert(item.DestIndex, temp);
                    }
                    else
                    {
                        tr.Columns.Insert(item.DestIndex + 1, src);
                        tr.Columns.RemoveAt(item.SrcIndex);
                    }
                }
            }
        }

        protected void RowOnClick(T dataItem, MouseEventArgs e)
        {
            if (IsSelectable)
            {
                bool isSelected = false;
                if (SelectedMode == SelectedModeEnum.Multiple)
                {
                    if (SelectedItems.Contains(dataItem))
                        SelectedItems.Remove(dataItem);
                    else
                    {
                        SelectedItems.Add(dataItem);
                        SelectedItem = dataItem;
                        isSelected = true;
                    }
                }
                else
                {
                    if (!SelectedItems.Contains(dataItem))
                    {
                        SelectedItems.Clear();
                        SelectedItems.Add(dataItem);
                        SelectedItem = dataItem;
                        isSelected = true;
                    }
                    else
                        SelectedItems.Clear();
                }

                OnRowClick?.Invoke(new RowClickEventArgs<T>() { SelectedItem = SelectedItem, Event = e, IsSelected = isSelected });
            }
        }

        protected string CustomCss(T item) => Body.CustomCssDelegate != null ? Body.CustomCssDelegate(item) : string.Empty;

        protected string SelectedRow(T item) => SelectedItems.Contains(item) ? "selected-row" : string.Empty;

        protected void RowDbClick(T dataItem)
        {
            if (IsSelectable)
            {
                OnRowDbClick?.Invoke(dataItem);
            }
        }

        protected void CheckAllChecked(Func<T, T, bool> checkedBy)
        {
            isCheckAll = !isCheckAll;
            foreach (var item in GetCheckedItems(checkedBy))
            {
                var temp = CheckedItems.FirstOrDefault(e => checkedBy(e, item));
                CheckedItems.Remove(temp);
            }

            if (isCheckAll)
                CheckedItems.AddRange(DataItems.Where(e => (ShowCheckbox == null || ShowCheckbox.Disable == null) ? true : !ShowCheckbox.Disable(e)));
            CheckedItemsChanged?.Invoke(new CheckedChangeEventArgs<T>()
            {
                CheckedItem = default,
                IsCheckAll = true,
                CheckedItems = CheckedItems
            });
            StateHasChanged();
        }

        protected bool IsChecked(T item, Func<T, T, bool> checkedBy) => CheckedItems.Any(e => checkedBy(e, item));

        protected object GetVal(ColumnBodyTemplate col, T item) => GetProperty(col, item).GetValue(item);

        protected void CheckedChange(T dataItem, Func<T, T, bool> checkedBy)
        {
            var temp = CheckedItems.FirstOrDefault(e => checkedBy(e, dataItem));
            if (temp != null)
                CheckedItems.Remove(temp);
            else
                CheckedItems.Add(dataItem);
            StateHasChanged();
            CheckedItemsChanged?.Invoke(new CheckedChangeEventArgs<T>()
            {
                CheckedItem = dataItem,
                IsChecked = temp == null,
                CheckedItems = CheckedItems
            });
        }

        protected IEnumerable<T> GetCheckedItems(Func<T, T, bool> checkedBy) => DataItems.Where(e => CheckedItems.Any(f => checkedBy(e, f)));

        protected bool IsCheckedAll(Func<T, T, bool> checkedBy) => isCheckAll = GetCheckedItems(checkedBy).Count() == DataItems.Count;

        private PropertyInfo GetProperty(ColumnBodyTemplate col, T item)
        {
            var prop = item.GetType().GetProperty(col.DisplayFieldName);
            if (prop == null)
                throw new Exception($"Column {col.DisplayFieldName} doesn't exist");
            else
                return prop;
        }

        /// <summary>
        /// Get css class for remove child rows border top and bottom
        /// </summary>
        /// <param name="row">Current RowBodyTemplate</param>
        /// <param name="rowIndex">Current row index</param>
        /// <returns>class for remove border</returns>
        protected string GetBorderClass(RowBodyTemplate row, int rowIndex)
        {
            if (row.IsRemoveChildBorder)
            {
                if (rowIndex == 0)
                {
                    return "border-bottom-0";
                }
                return "border-bottom-0 border-top-0";
            }
            return string.Empty;
        }
    }
}
