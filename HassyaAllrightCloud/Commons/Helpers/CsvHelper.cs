using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public static class CsvHelper
    {
        public static byte[] ExportDatatableToCsv(DataTable dataTable, string path, bool isHaveNo = false,
            bool isWithHeader = false, bool isEnclose = false, string space = "\t")
        {
            StringBuilder sb = new StringBuilder();

            if (isWithHeader)
            {
                List<string> columnNames;
                if (isEnclose)
                {
                    columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => string.Concat("\"", column.ColumnName, "\"")).ToList();
                }
                else
                {
                    columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).ToList();
                }

                if (isHaveNo)
                {
                    if(isEnclose)
                        columnNames.Insert(0, string.Concat("\"", "No", "\""));
                    else
                        columnNames.Insert(0, "No");
                }

                sb.AppendLine(string.Join($"{space}", columnNames));
            }

            int rowCount = 1;
            foreach (DataRow row in dataTable.Rows)
            {
                List<string> fields;
                if (isEnclose)
                {
                    fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString(), "\"")).ToList();
                }
                else
                {
                    fields = row.ItemArray.Select(field => field.ToString()).ToList();
                }

                if (isHaveNo)
                {
                    if (isEnclose)
                    {
                        fields.Insert(0, string.Concat("\"", (rowCount++).ToString(), "\""));
                    }
                    else
                    {
                        fields.Insert(0, (rowCount++).ToString());
                    }
                }

                sb.AppendLine(string.Join($"{space}", fields));
            }

            using(var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(sb.ToString());
            }
            byte[] result = File.ReadAllBytes(path);
            File.Delete(path);
            return result;
        }

        /// <summary>
        /// Export <see cref="IEnumerable{T}"/> collection to CSV file
        /// </summary>
        /// <typeparam name="T">Type of source</typeparam>
        /// <param name="source">Data source want to export</param>
        /// <param name="csvConfig">Config option to export</param>
        /// <param name="header">Header of the CSV file. Default values is <c>null</c></param>
        /// <returns>Stream byte of the CSV file</returns>
        public static byte[] ExportCollectionToCsv<T>(IEnumerable<T> source, CsvConfigOption csvConfig, IEnumerable<string> header = null) where T : class
        {
            source = source ?? throw new ArgumentNullException(nameof(source), "Data source should be not null");
            csvConfig = csvConfig ?? throw new ArgumentNullException(nameof(csvConfig), "Configuration should be not null");

            if (!Directory.Exists(csvConfig.FilePath))
                throw new DirectoryNotFoundException($"{nameof(csvConfig.FilePath)} is invalid");

            StringBuilder resultCsv = new StringBuilder();

            if(csvConfig.Delimiter.Option == CSV_Delimiter.FixedLength) // fixed length => disbale group and header
            {
                resultCsv = source.ToSerialText();
            }
            else
            {
                string groupSymbol = string.Empty;
                string delimiter = string.Empty;

                switch (csvConfig.Delimiter.Option)
                {
                    case CSV_Delimiter.Comma:
                        delimiter = ",";
                        break;
                    case CSV_Delimiter.Semicolon:
                        delimiter = ";";
                        break;
                    case CSV_Delimiter.Space:
                        delimiter = "\t";
                        break;
                    case CSV_Delimiter.Other:
                        delimiter = csvConfig.DelimiterSymbol;
                        break;
                    default:
                        break;
                }
                switch (csvConfig.GroupSymbol.Option)
                {
                    case CSV_Group.QuotationMarks:
                        groupSymbol = "\"";
                        break;
                    case CSV_Group.WithoutQuotes:
                        break;
                    case CSV_Group.Empty:
                        break;
                    default:
                        break;
                }

                if (csvConfig.Header.Option == CSV_Header.IncludeHeader)
                {
                    if (header is null)
                    {
                        throw new ArgumentNullException(nameof(header), "The header is required but header data is null");
                    }
                    else
                    {
                        //Write header
                        resultCsv.AppendLine(string.Join(delimiter, 
                            header.Select(_=> string.Concat(groupSymbol, _.ToString(), groupSymbol))
                                  .ToList()
                                  ));
                    }
                }
                resultCsv.AppendLine(source.ToDelimiterText(delimiter, groupSymbol).ToString());
            }

            return Encoding.UTF8.GetBytes(resultCsv.ToString());
        }
    }
}
