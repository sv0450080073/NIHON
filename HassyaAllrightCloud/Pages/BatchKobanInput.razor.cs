using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Commons.Constants;
using Microsoft.AspNetCore.Components.Web;
using static HassyaAllrightCloud.Commons.Constants.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Pages
{
    public class BatchKobanInputBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<BatchKobanInput> Lang { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected IBatchKobanInputService BactchKobanInputService { get; set; }
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        protected bool isLoading { get; set; } = false;
        protected int activeTabIndex = 0;
        protected bool IsValid = true;
        protected string dateFormat = "yyyy/MM/dd";
        protected bool IsReadOnly = true;
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        public List<CompanyData> CompanyDatas = new List<CompanyData>();
        public List<LoadSaleBranchList> SaleBranchs = new List<LoadSaleBranchList>();
        public List<Staffs> Staffs = new List<Staffs>();
        public List<TaskModel> Tasks = new List<TaskModel>();
        public List<ComboboxFixField> OutputOrders = new List<ComboboxFixField>();
        public List<ComboboxFixField> DisplayTypes = new List<ComboboxFixField>();
        public List<KobanDataGridModel> KobanDataGrids = new List<KobanDataGridModel>();
        public List<WorkHolidayTypeDataModel> WorkHolidayTypes = new List<WorkHolidayTypeDataModel>();
        public WorkHolidayTypeDataModel SelectedWorkHolidayType = new WorkHolidayTypeDataModel();
        public int ActiveV { get; set; }
        public int EmployeeDisplayOption { get; set; }
        public List<string> Dates = new List<string>();
        public List<string> DateFulls = new List<string>();
        public List<string> DayOfWeeks = new List<string>();
        public List<CheckModel> CheckedAllDay = new List<CheckModel>();
        public List<CellModel> SelectedCell = new List<CellModel>();
        public List<string> Errors = new List<string>();
        public int TableId = 0;
        public int PreviousTableId;
        public string FormName = FormFilterName.BatchKobanInput;
        public string DisPlayHeader;
        Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        public bool Cansave = false;
        public bool CanClick = true;
        public bool IsOpenFilterForm = true;
        public double ToDate = 31;
        public int ActiveTabIndex
        {
            get => activeTabIndex;
            set
            {
                activeTabIndex = value;
                AdjustHeightWhenTabChanged();
            }
        }

        public BatchKobanInputFilterModel BatchKobanInputFilterModel = new BatchKobanInputFilterModel();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("setEventforTimeInput");
                if (firstRender)
                {
                    await JSRuntime.InvokeVoidAsync("loadPageScript", "BatchKobanInput");
                    await JSRuntime.InvokeVoidAsync("AdjustKobanTableHeight");
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var dataLang = Lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                GenerateCheckedAllDay();
                ActiveV = (int)ViewMode.Medium;
                DisPlayHeader = Lang["No"];
                EmployeeDisplayOption = 1;
                BatchKobanInputFilterModel.KinmuYmd = DateTime.Now;
                CompanyDatas = BactchKobanInputService.GetCompanyData(new ClaimModel().TenantID).Result;
                CompanyDatas = CompanyDatas.Where(x => x.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).Distinct().ToList();
                BatchKobanInputFilterModel.Company = CompanyDatas.Where(x => x.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).FirstOrDefault();
                SaleBranchs = BactchKobanInputService.GetSaleBranchData(new ClaimModel().TenantID, BatchKobanInputFilterModel.Company != null ?BatchKobanInputFilterModel.Company.CompanyCdSeq : new ClaimModel().CompanyID).Result;
                SaleBranchs.Insert(0, null);
                Tasks = BactchKobanInputService.GetTasks(new ClaimModel().TenantID).Result;
                WorkHolidayTypes = BactchKobanInputService.GetWorkHolidayTypes(new ClaimModel().TenantID).Result;
                Tasks.Insert(0, null);
                GenerateFixedField();
                BatchKobanInputFilterModel.SyuJun = OutputOrders[0];
                BatchKobanInputFilterModel.DisplayKbn = DisplayTypes[1];
                List<TkdInpCon> tkdInpCons = FilterConditionService.GetFilterCondition(FormName, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                if (tkdInpCons.Any())
                {
                    BatchKobanInputFilterModel = GetFilterModel(tkdInpCons);
                }
                BatchKobanInputFilterModel.TimeStart = new BookingInputHelper.MyTime()
                {
                    myHour = 0,
                    myMinute = 0
                };
                BatchKobanInputFilterModel.TimeEnd = new BookingInputHelper.MyTime()
                {
                    myHour = 23,
                    myMinute = 59
                };
                Staffs = BactchKobanInputService.GetStaffs(new ClaimModel().TenantID, new ClaimModel().CompanyID, BatchKobanInputFilterModel).Result;
                Staffs.Insert(0, null);
                SelectedWorkHolidayType = null;
                
                GenerateDates(BatchKobanInputFilterModel.KinmuYmd);
                GenerateKobanData();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }

        }

        private BatchKobanInputFilterModel GetFilterModel(List<TkdInpCon> tkdInpCons)
        {
            BatchKobanInputFilterModel result = new BatchKobanInputFilterModel();

            foreach(var item in tkdInpCons)
            {
                switch (item.ItemNm)
                {
                    case nameof(BatchKobanInputFilterModel.KinmuYmd):
                        result.KinmuYmd = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                        break;
                    case nameof(BatchKobanInputFilterModel.Company):
                        var savedCompany = CompanyDatas.Where(x => x != null && x.CompanyCdSeq == (string.IsNullOrWhiteSpace(item.JoInput) ? new ClaimModel().CompanyID : Int32.Parse(item.JoInput))).FirstOrDefault();
                        var currentCompany = CompanyDatas.Where(x => x.CompanyCdSeq == new ClaimModel().CompanyID).FirstOrDefault();
                        result.Company = string.IsNullOrWhiteSpace(item.JoInput) ? currentCompany : savedCompany != null ? savedCompany : currentCompany;
                        break;
                    case nameof(BatchKobanInputFilterModel.EigyoStart):
                        result.EigyoStart = string.IsNullOrWhiteSpace(item.JoInput) ? null : SaleBranchs.Where(x => x != null && x.EigyoCdSeq == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(BatchKobanInputFilterModel.EigyoEnd):
                        result.EigyoEnd = string.IsNullOrWhiteSpace(item.JoInput) ? null : SaleBranchs.Where(x => x != null && x.EigyoCdSeq == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(BatchKobanInputFilterModel.SyainStart):
                        result.SyainStart = string.IsNullOrWhiteSpace(item.JoInput) ? null : Staffs.Where(x => x != null && x.SyainCd == item.JoInput).FirstOrDefault();
                        break;
                    case nameof(BatchKobanInputFilterModel.SyainEnd):
                        result.SyainEnd = string.IsNullOrWhiteSpace(item.JoInput) ? null : Staffs.Where(x => x != null && x.SyainCd == item.JoInput).FirstOrDefault();
                        break;
                    case nameof(BatchKobanInputFilterModel.SyokumuStart):
                        result.SyokumuStart = string.IsNullOrWhiteSpace(item.JoInput) ? null : Tasks.Where(x => x != null && x.SyokumuCd == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(BatchKobanInputFilterModel.SyokumuEnd):
                        result.SyokumuEnd = string.IsNullOrWhiteSpace(item.JoInput) ? null : Tasks.Where(x => x != null && x.SyokumuCd == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(BatchKobanInputFilterModel.SyuJun):
                        result.SyuJun = string.IsNullOrWhiteSpace(item.JoInput) ? OutputOrders[0] : OutputOrders.Where(x => x != null && x.IdValue == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                    case nameof(BatchKobanInputFilterModel.DisplayKbn):
                        result.DisplayKbn = string.IsNullOrWhiteSpace(item.JoInput) ? DisplayTypes[1] : DisplayTypes.Where(x => x != null && x.IdValue == Int32.Parse(item.JoInput)).FirstOrDefault();
                        break;
                }
            }

            return result;
        }
        protected async Task ResetForm()
        {
            try
            {
                BatchKobanInputFilterModel.KinmuYmd = DateTime.Now;

                GenerateDates(BatchKobanInputFilterModel.KinmuYmd);

                await Task.Run(() =>
                {
                    isLoading = true;
                    InvokeAsync(StateHasChanged);
                    SelectedCell = new List<CellModel>();
                    SelectedWorkHolidayType = null;
                    IsReadOnly = true;
                    Cansave = false;
                    ActiveV = (int)ViewMode.Medium;
                    EmployeeDisplayOption = 1;
                    CompanyDatas = BactchKobanInputService.GetCompanyData(new ClaimModel().CompanyID).Result;
                    BatchKobanInputFilterModel.Company = CompanyDatas.Where(x => x.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).FirstOrDefault();
                    BatchKobanInputFilterModel.SyuJun = OutputOrders[0];
                    BatchKobanInputFilterModel.DisplayKbn = DisplayTypes[1];
                    WorkHolidayTypes = BactchKobanInputService.GetWorkHolidayTypes(new ClaimModel().TenantID).Result;
                    SelectedWorkHolidayType = null;
                    BatchKobanInputFilterModel.EigyoStart = null;
                    BatchKobanInputFilterModel.EigyoEnd = null;
                    BatchKobanInputFilterModel.SyainStart = null;
                    BatchKobanInputFilterModel.SyainEnd = null;
                    BatchKobanInputFilterModel.SyokumuStart = null;
                    BatchKobanInputFilterModel.SyokumuEnd = null;
                    BatchKobanInputFilterModel.DisplayKbn = new ComboboxFixField()
                    {
                        IdValue = 2,
                        StringValue = BatchKobanInputConstants.FourteenDay
                    };

                    BatchKobanInputFilterModel.TimeStart = new BookingInputHelper.MyTime()
                    {
                        myHour = 0,
                        myMinute = 0
                    };
                    BatchKobanInputFilterModel.TimeEnd = new BookingInputHelper.MyTime()
                    {
                        myHour = 23,
                        myMinute = 59
                    };
                    foreach (var item in CheckedAllDay)
                    {
                        item.IsChecked = false;
                    }
                    GenerateKobanData();
                    if (Errors.Count > 0)
                    {
                        Errors = new List<string>();
                    }
                    isLoading = false;
                    InvokeAsync(StateHasChanged);
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        protected async Task DeleteKobanData()
        {
            try
            {
                await Task.Run(() =>
                {
                    isLoading = true;
                    InvokeAsync(StateHasChanged);
                    List<CellModel> deletedItem = new List<CellModel>();
                    foreach (var item in SelectedCell)
                    {
                        bool kinkyujDeleted = BactchKobanInputService.DeleteKinkyuj(item.Date, item.SyainCdSeq).Result;
                        bool kobanDeleted = BactchKobanInputService.DeleteKoban(item.Date, item.SyainCdSeq).Result;
                        deletedItem.Add(item);
                        if (kinkyujDeleted == false || kobanDeleted == false)
                        {
                            Errors.Add("BI_T008");
                        }
                        else
                        {
                            Errors.Add("BI_T007");
                        }
                    }
                    foreach (var item in deletedItem)
                    {
                        SelectedCell.Remove(item);
                    }
                    IsReadOnly = true;
                    GenerateKobanData();
                    isLoading = false;
                    InvokeAsync(StateHasChanged);
                    foreach (var item in CheckedAllDay)
                    {
                        item.IsChecked = false;
                    }
                    Cansave = false;
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
            
        }
        protected async Task SaveKoban()
        {
            try
            {
                await Task.Run(() =>
                {
                    if (Cansave)
                    {
                        isLoading = true;
                        InvokeAsync(StateHasChanged);
                        if (SelectedWorkHolidayType == null || SelectedCell.Count == 0)
                        {
                            Errors.Add("BI_T009");
                        }
                        else
                        {
                            if (Errors.Contains("BI_T009"))
                            {
                                Errors.Remove("BI_T009");
                            }
                            foreach (var item in SelectedCell)
                            {
                                bool kinkyujDeleted = BactchKobanInputService.DeleteKinkyuj(item.Date, item.SyainCdSeq).Result;
                                bool kobanDeleted = BactchKobanInputService.DeleteKoban(item.Date, item.SyainCdSeq).Result;

                                var kinkyujSaved = BactchKobanInputService.SaveKinkyuj(item, SelectedWorkHolidayType, BatchKobanInputFilterModel.TimeStart, BatchKobanInputFilterModel.TimeEnd).Result;
                                bool kobanSaved = BactchKobanInputService.SaveKoban(item, SelectedWorkHolidayType, BatchKobanInputFilterModel.TimeStart, BatchKobanInputFilterModel.TimeEnd, kinkyujSaved.Item2).Result;

                                if (kinkyujDeleted == false || kobanDeleted == false || kinkyujSaved.Item1 == false || kobanSaved == false)
                                {
                                    Errors.Add("BI_T008");
                                }
                            }
                            if (!Errors.Contains("BI_T008"))
                            {
                                Errors.Add("BI_T007");
                                SelectedCell = new List<CellModel>();
                                IsReadOnly = true;
                                Cansave = false;
                                SelectedWorkHolidayType = null;
                            }
                            GenerateKobanData();
                            foreach (var item in CheckedAllDay)
                            {
                                item.IsChecked = false;
                            }
                            SelectedCell = new List<CellModel>();
                            Cansave = false;
                        }
                        
                        isLoading = false;
                        InvokeAsync(StateHasChanged);
                    }
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        protected void ChangeStartTime(BookingInputHelper.MyTime newValue)
        {
            try
            {
                BatchKobanInputFilterModel.TimeStart = newValue;
                if (BatchKobanInputFilterModel.TimeStart.Str.Replace(":", string.Empty).CompareTo(BatchKobanInputFilterModel.TimeEnd.Str.Replace(":", string.Empty)) > 0)
                {
                    Errors.Add("BI_T005");
                }
                else
                {
                    if (Errors.Contains("BI_T005"))
                    {
                        Errors.Remove("BI_T005");
                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        protected void ChangeEndTime(BookingInputHelper.MyTime newValue)
        {
            try
            {
                BatchKobanInputFilterModel.TimeEnd = newValue;
                if (BatchKobanInputFilterModel.TimeStart.Str.Replace(":", string.Empty).CompareTo(BatchKobanInputFilterModel.TimeEnd.Str.Replace(":", string.Empty)) > 0)
                {
                    Errors.Add("BI_T005");
                }
                else
                {
                    if (Errors.Contains("BI_T005"))
                    {
                        Errors.Remove("BI_T005");
                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        private void GenerateCheckedAllDay()
        {
            try
            {
                for (int i = 0; i < ToDate; i++)
                {
                    CheckedAllDay.Add(new CheckModel()
                    {
                        IsChecked = false
                    });
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        private void GenerateFixedField()
        {
            try
            {
                OutputOrders = new List<ComboboxFixField>();
                OutputOrders.Add(new ComboboxFixField()
                {
                    IdValue = 1,
                    StringValue = BatchKobanInputConstants.OfficeOrder
                });
                OutputOrders.Add(new ComboboxFixField()
                {
                    IdValue = 2,
                    StringValue = BatchKobanInputConstants.EmployeeOrderCode
                });
                OutputOrders.Add(new ComboboxFixField()
                {
                    IdValue = 3,
                    StringValue = BatchKobanInputConstants.EmployeeAttend
                });
                OutputOrders.Add(new ComboboxFixField()
                {
                    IdValue = 4,
                    StringValue = BatchKobanInputConstants.CarAttend
                });
                DisplayTypes = new List<ComboboxFixField>();
                DisplayTypes.Add(new ComboboxFixField()
                {
                    IdValue = 1,
                    StringValue = BatchKobanInputConstants.TwoDay
                });
                DisplayTypes.Add(new ComboboxFixField()
                {
                    IdValue = 2,
                    StringValue = BatchKobanInputConstants.FourteenDay
                });
                DisplayTypes.Add(new ComboboxFixField()
                {
                    IdValue = 3,
                    StringValue = BatchKobanInputConstants.ThirtyDay
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        public void GenerateDates(DateTime startDate)
        {
            try
            {
                ToDate = (startDate.AddMonths(1) - startDate).TotalDays;
                Dates = new List<string>();
                DateFulls = new List<string>();
                DayOfWeeks = new List<string>();
                DateFulls.Add(startDate.ToString().Substring(0, 10).Replace("/", string.Empty));
                Dates.Add(startDate.ToString().Substring(5, 5));
                var culture = new System.Globalization.CultureInfo("ja-JP", false);
                DayOfWeeks.Add(culture.DateTimeFormat.GetDayName(startDate.DayOfWeek).Replace("曜日", string.Empty));
                for (int i = 1; i <= ToDate; i++)
                {
                    var newDate = startDate.AddDays(i);
                    Dates.Add(newDate.ToString().Substring(5, 5));
                    DateFulls.Add(newDate.ToString().Substring(0, 10).Replace("/", string.Empty));
                    DayOfWeeks.Add(culture.DateTimeFormat.GetDayName(newDate.DayOfWeek).Replace("曜日", string.Empty));
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        public async Task OnCellClick(MouseEventArgs e, int row, int cell, int syainCdSeq)
        {
            try
            {
                CheckedAllDay[cell - 1].IsChecked = false;
                if (!e.ShiftKey)
                {
                    CellModel currentSelectedCell = SelectedCell.Where(x => x.Row == row && x.Cell == cell).FirstOrDefault();
                    if (currentSelectedCell != null)
                    {
                        SelectedCell.Remove(currentSelectedCell);
                    }
                    else
                    {
                        SelectedCell.Add(new CellModel()
                        {
                            Row = row,
                            Cell = cell,
                            SyainCdSeq = syainCdSeq,
                            Date = DateFulls[cell - 1]
                        });
                    }
                }
                else
                {
                    if (!SelectedCell.Any(x => x.Row == row))
                    {
                        SelectedCell = new List<CellModel>();
                    }
                    if (SelectedCell.Count == 0)
                    {
                        CellModel cells = new CellModel()
                        {
                            Row = row,
                            Cell = cell,
                            SyainCdSeq = syainCdSeq,
                            Date = DateFulls[cell - 1]
                        };
                        SelectedCell.Add(cells);
                    }
                    else
                    {
                        List<CellModel> cellTemps = new List<CellModel>();
                        CellModel cells = new CellModel()
                        {
                            Row = row,
                            Cell = cell,
                            SyainCdSeq = syainCdSeq,
                            Date = DateFulls[cell - 1]
                        };
                        if (SelectedCell.Any(x => x.Row == row))
                        {
                            SelectedCell.Add(cells);
                            var unSelectedCell = SelectedCell.Where(x => x.Row != row).ToList();
                            foreach (var item in unSelectedCell)
                            {
                                SelectedCell.Remove(item);
                            }
                            for (int i = SelectedCell.OrderBy(x => x.Cell).FirstOrDefault().Cell; i <= SelectedCell.OrderByDescending(x => x.Cell).FirstOrDefault().Cell; i++)
                            {
                                cellTemps.Add(new CellModel()
                                {
                                    Row = row,
                                    Cell = i,
                                    SyainCdSeq = syainCdSeq,
                                    Date = DateFulls[i - 1]
                                });
                            }
                            SelectedCell = cellTemps;
                        }
                    }
                    foreach (var item in SelectedCell)
                    {
                        CheckedAllDay[item.Cell - 1].IsChecked = false;
                    }
                }
                if (SelectedCell.Count > 0)
                {
                    IsReadOnly = false;
                    Cansave = true;
                }
                else
                {
                    Cansave = false;
                }
                if (Errors.Count > 0)
                {
                    Errors = new List<string>();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        public bool GetCellBackGround(int row, int cell)
        {
            if (SelectedCell.Any(x => x.Row == row && x.Cell == cell))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetCellSelectionBackGround(int row, int value, short kinkyuCd)
        {
            if (value != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetEmpolyeeFieldDisplay(KobanDataGridModel kobanDataGrid)
        {
            string result = string.Empty;

            if (EmployeeDisplayOption == 1)
            {
                return kobanDataGrid.SyainNm;
            }
            else if (EmployeeDisplayOption == 2)
            {
                return kobanDataGrid.SyainEigyoNm;
            }
            else
            {
                return kobanDataGrid.SyainCd;
            }
        }
        public async void AdjustHeightWhenTabChanged()
        {
            try
            {
                await Task.Run(() =>
                {
                    InvokeAsync(StateHasChanged).Wait();
                    JSRuntime.InvokeVoidAsync("AdjustHeight");
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        public IEnumerable<string> Store(IEnumerable<string> errorMessage)
        {
            if (errorMessage.Count() > 0)
            {
                IsValid = false;
            }
            else
            {
                IsValid = true;
            }
            return errorMessage;
        }

        protected async Task SelectWorkHolidayType(dynamic value)
        {
            try
            {
                SelectedWorkHolidayType = value;
                if (Errors.Count > 0)
                {
                    Errors = new List<string>();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        protected async Task ChangeValueForm(string ValueName, dynamic value)
        {
            try
            {
                if (value is string && string.IsNullOrEmpty(value))
                {
                    value = null;
                }
                var propertyInfo = BatchKobanInputFilterModel.GetType().GetProperty(ValueName);
                propertyInfo.SetValue(BatchKobanInputFilterModel, value, null);
                isLoading = true;
                if (ValueName.Equals(nameof(BatchKobanInputFilterModel.KinmuYmd)))
                {
                    GenerateDates(BatchKobanInputFilterModel.KinmuYmd);
                    SelectedCell = new List<CellModel>();
                }
                if (ValueName.Equals(nameof(BatchKobanInputFilterModel.DisplayKbn)))
                {
                    SelectedCell = new List<CellModel>();
                }
                if (Errors.Count > 0)
                {
                    Errors = new List<string>();
                }
                if (ValueName.Equals(nameof(BatchKobanInputFilterModel.EigyoStart)) || ValueName.Equals(nameof(BatchKobanInputFilterModel.EigyoEnd)))
                {
                    Staffs = BactchKobanInputService.GetStaffs(new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, BatchKobanInputFilterModel).Result;
                    Staffs.Insert(0, null);
                }
                await Task.Run(() =>
                {
                    isLoading = false;
                    GenerateKobanData();
                    InvokeAsync(StateHasChanged).Wait();
                });
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            

        }

        public async Task CheckAllDay(int cell, bool newValue)
        {
            try
            {
                CheckedAllDay[cell - 1].IsChecked = newValue;
                if (newValue)
                {
                    for (int i = 0; i < KobanDataGrids.Count; i++)
                    {
                        SelectedCell.Add(new CellModel()
                        {
                            Row = i,
                            Cell = cell,
                            Date = DateFulls[cell - 1],
                            SyainCdSeq = KobanDataGrids[i].SyainCdSeq
                        });
                    }
                }
                else
                {
                    var unSelectCells = SelectedCell.Where(x => x.Cell == cell).ToList();
                    foreach (var item in unSelectCells)
                    {
                        SelectedCell.Remove(item);
                    }
                }
                if (SelectedCell.Count > 0)
                {
                    IsReadOnly = false;
                    Cansave = true;
                }
                else
                {
                    Cansave = false;
                }
                if (Errors.Count > 0)
                {
                    Errors = new List<string>();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        public void GenerateKobanData()
        {
            try
            {
                isLoading = true;
                InvokeAsync(StateHasChanged);
                if (IsValid)
                {
                    KobanDataGrids = BactchKobanInputService.GetKobanDataGrids(BatchKobanInputFilterModel, new ClaimModel().TenantID).Result;
                    if (KobanDataGrids.Count == 0)
                    {
                        Errors.Add("BI_T006");
                    }
                    else
                    {
                        if (Errors.Contains("BI_T006"))
                        {
                            Errors.Remove("BI_T006");
                        }
                        keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForBatchKobanInput(BatchKobanInputFilterModel).Result;
                        FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormName, 0, new ClaimModel().SyainCdSeq).Wait();
                    }
                }
                isLoading = false;
                InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        public void clickV(MouseEventArgs e, int number)
        {
            try
            {
                ActiveV = number;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
        }

        public void ChangeEmpolyeeDisplayOption(MouseEventArgs e, int number)
        {
            try
            {
                if (Errors.Count > 0)
                {
                    Errors = new List<string>();
                }
                EmployeeDisplayOption = number;
                DisPlayHeader = @EmployeeDisplayOption == 1 ? @Lang["No"] : @EmployeeDisplayOption == 2 ? @Lang["SaleOffice"] : @Lang["EmployeeCode"];
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }

        public async void MouseDownGrid(int row, int column, int tableId)
        {
            try
            {
                if (!CanClick)
                {
                    return;
                }
                CanClick = false;
                TableId = tableId;
                try
                {
                    await JSRuntime.InvokeAsync<string>("SelectCellByTable", row, column, tableId, DotNetObjectReference.Create(this));
                }
                catch (Exception)
                {
                    CanClick = true;
                }
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
        [JSInvokable]
        public void WhenDragComplete(int fromRow, int toRow, int fromColumn, int toColumn)
        {
            try
            {
                if (TableId != 0 && TableId != 1)
                {
                    fromColumn = fromColumn + TableId;
                    toColumn = toColumn + TableId;
                }

                if (fromRow == toRow && fromColumn == toColumn)
                {
                    CanClick = true;
                    return;
                }
                else
                {
                    for (int i = fromRow; i <= toRow; i++)
                    {
                        for (int j = fromColumn; j <= toColumn; j++)
                        {
                            SelectedCell.Add(new CellModel()
                            {
                                Row = i,
                                Cell = j,
                                Date = DateFulls[j - 1],
                                SyainCdSeq = KobanDataGrids[i].SyainCdSeq
                            });
                        }
                    }
                }
                for (int i = fromColumn; i <= toColumn; i++)
                {
                    CheckedAllDay[i - 1].IsChecked = false;
                }
                if (SelectedCell.Count > 0)
                {
                    IsReadOnly = false;
                    Cansave = true;
                }
                else
                {
                    Cansave = false;
                }
                if (Errors.Count > 0)
                {
                    Errors = new List<string>();
                }
                StateHasChanged();
                CanClick = true;
            }
            catch (Exception ex)
            {
                errorModalService.ShowErrorPopup("Error", ex.GetOriginalException()?.Message);
            }
            
        }
    }
}
