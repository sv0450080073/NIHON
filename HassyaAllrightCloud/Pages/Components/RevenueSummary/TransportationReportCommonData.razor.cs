using HassyaAllrightCloud.Domain.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using HassyaAllrightCloud.Commons.Helpers;
using SharedLibraries.UI.Models;
using System.Linq;

namespace HassyaAllrightCloud.Pages.Components.RevenueSummary
{
    public class TransportationReportCommonDataBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<TransportationReportCommonData> _lang { get; set; }
        [Parameter] public List<SummaryResult> SummaryResult { get; set; } = new List<SummaryResult>();
        [Parameter] public bool IsDailyReport { get; set; }
        [Parameter] public int GridSize { get; set; }
        protected HeaderTemplate Header;
        protected BodyTemplate Body;
        protected override void OnInitialized()
        {
            InitTable();
        }

        private IEnumerable<string> GetType(SummaryResult item)
        {
            if (item.MesaiKbn == 1)
                yield return IsDailyReport ? _lang["TotalByDay"] : _lang["TotalPage"];
            else
                yield return IsDailyReport ? _lang["Accumulation"] : _lang["TotalByMonth"];
        }

        private void InitTable()
        {
            Header = new HeaderTemplate()
            {
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate(){ ColName = _lang["total_col"], RowSpan = 2, Width = 60 },
                            new ColumnHeaderTemplate(){ ColName = _lang["orders_col"], ColSpan = 4, Width = 430 },
                            new ColumnHeaderTemplate(){ ColName = _lang["in_house_col"], ColSpan = 6, Width = 560 },
                            new ColumnHeaderTemplate(){ ColName = _lang["mercenary_col"], ColSpan = 6, Width = 470 },
                            new ColumnHeaderTemplate(){ ColName = _lang["profit_and_loss_col"], RowSpan = 2, Width = 100 }
                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate(){ ColName = _lang["fare_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["cumulative_col"] },

                            new ColumnHeaderTemplate(){ ColName = _lang["fare_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["incidental_col"] },

                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fare_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },

                            new ColumnHeaderTemplate(){ ColName = _lang["fee_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["incidental_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["consumption_tax_col"] },
                            new ColumnHeaderTemplate(){ ColName = _lang["fee_col"] },
                        }
                    }
                }
            };

            Body = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns  = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Center, DisplayFieldName = nameof(Domain.Dto.SummaryResult.Type), Control = new MultiLineControl<SummaryResult>(){ MutilineText = GetType } },
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SJyuSyaRyoUnc), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SJyuSyaRyoSyo), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SJyuSyaRyoTes), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SJyuSyaRyoRui), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SJisSyaRyoUnc), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SJisSyaRyoSyo), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SJisSyaRyoTes), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SFutUriGakKin), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SFutSyaRyoSyo), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SFutSyaRyoTes), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SYoushaUnc), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SYoushaSyo), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SYoushaTes), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SYfuUriGakKin), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SYfuSyaRyoSyo), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SYfuSyaRyoTes), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                            new ColumnBodyTemplate() { AlignCol = AlignColEnum.Right, DisplayFieldName = nameof(Domain.Dto.SummaryResult.SSoneki), CustomTextFormatDelegate = e => KoboGridHelper.AddCommasForNumber(e)},
                        }
                    }
                }
            };
        }
    }
}
