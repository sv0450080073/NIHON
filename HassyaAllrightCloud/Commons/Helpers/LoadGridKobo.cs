using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using SharedLibraries.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class LoadGridKobo
    {
        public static (HeaderTemplate, BodyTemplate) Load(HeaderTemplate DefaultHeader, BodyTemplate DefaultBody, List<TkdGridLy> gridlayouts, byte stickyCount)
        {
            if (gridlayouts.Count > 0)
            {
                Dictionary<(string, int), ColumnHeaderTemplate> dictHeader = new Dictionary<(string, int), ColumnHeaderTemplate>();
                Dictionary<(string, int), ColumnBodyTemplate> dictBody = new Dictionary<(string, int), ColumnBodyTemplate>();

                var data = SetUp(DefaultHeader, DefaultBody, dictHeader, dictBody, stickyCount);

                var dictLevel1 = dictHeader.Where(_ => _.Key.Item2 == 0);
                foreach (var grid in gridlayouts)
                {
                    foreach (var item in dictLevel1)
                    {
                        if (item.Key.Item1 == grid.ItemNm)
                        {
                            AddHeader(data.Item1, data.Item2, item, dictHeader, dictBody, item.Key.Item1, gridlayouts);
                            break;
                        }
                    }
                }

                return (data.Item1, data.Item2);
            }
            
            return (DefaultHeader, DefaultBody);
        }

        private static (HeaderTemplate, BodyTemplate) SetUp(HeaderTemplate DefaultHeader, BodyTemplate DefaultBody,
            Dictionary<(string, int), ColumnHeaderTemplate> dictHeader, Dictionary<(string, int), ColumnBodyTemplate> dictBody, byte stickyCount)
        {
            var maxHeader = DefaultHeader.Rows.Count;
            var maxBody = DefaultBody.Rows.Count;

            HeaderTemplate TempHeader = new HeaderTemplate()
            {
                StickyCount = stickyCount,
                Rows = new List<RowHeaderTemplate>()
                {

                }
            };

            while (TempHeader.Rows.Count < maxHeader)
            {
                TempHeader.Rows.Add(new RowHeaderTemplate()
                {
                    Columns = new List<ColumnHeaderTemplate>()
                });
            }

            BodyTemplate TempBody = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {

                }
            };

            while (TempBody.Rows.Count < maxBody)
            {
                TempBody.Rows.Add(new RowBodyTemplate()
                {
                    Columns = new List<ColumnBodyTemplate>()
                });
            }

            for (int i = 0; i < DefaultHeader.Rows.Count; i++)
            {
                foreach (var column in DefaultHeader.Rows[i].Columns)
                {
                    dictHeader.Add((column.CodeName, i), column);
                }
            }

            for (int i = 0; i < DefaultBody.Rows.Count; i++)
            {
                foreach (var column in DefaultBody.Rows[i].Columns)
                {
                    dictBody.Add((column.CodeName, i), column);
                }
            }

            return (TempHeader, TempBody);
        }

        private static void AddHeader(HeaderTemplate TempHeader, BodyTemplate TempBody, KeyValuePair<(string, int), ColumnHeaderTemplate> item,
            Dictionary<(string, int), ColumnHeaderTemplate> dictHeader, Dictionary<(string, int), ColumnBodyTemplate> dictBody, string key, List<TkdGridLy> gridlayouts)
        {
            item.Value.Width = gridlayouts.FirstOrDefault(_ => _.ItemNm == key)?.Width ?? item.Value.Width;
            TempHeader.Rows[item.Key.Item2].Columns.Add(item.Value);
            var listBody = dictBody.Where(_ => _.Key.Item1 == key).ToList();
            foreach (var bodyItem in listBody)
            {
                AddBody(TempBody, bodyItem);
            }
            var listHeader = dictHeader.Where(_ => _.Value.ParentCodeName == item.Value.CodeName);
            foreach(var listItem in listHeader)
            {
                AddHeader(TempHeader, TempBody, listItem, dictHeader, dictBody, item.Key.Item1, gridlayouts);
            }
        }

        private static void AddBody(BodyTemplate TempBody, KeyValuePair<(string, int), ColumnBodyTemplate> listItem)
        {
            TempBody.Rows[listItem.Key.Item2].Columns.Add(listItem.Value);
        }
    }
}
