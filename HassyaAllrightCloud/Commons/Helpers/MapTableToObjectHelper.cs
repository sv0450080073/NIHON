using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class MapTableToObjectHelper
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static async Task<List<T>> ConvertToListObject<T>(SqlDataReader rd, int recordTakes = -1) where T : class, new()
        {
            List<T> res = new List<T>();
            int index = 0;

            while (await rd.ReadAsync())
            {
                if (recordTakes > -1 && index == recordTakes)
                    break;

                T t = new T();
                
                for (int inc = 0; inc < rd.FieldCount; inc++)
                {
                    Type type = t.GetType();
                    PropertyInfo prop = type.GetProperty(rd.GetName(inc));
                    if (prop == null)
                        continue;
                    prop.SetValue(t, Convert.ChangeType(rd.GetValue(inc), prop.PropertyType), null);
                }

                res.Add(t);
                index++;
            }

            return res;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (PropertyInfo pro in temp.GetProperties().Where(_ => _.CanWrite))
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    if (pro.Name == column.ColumnName && !string.IsNullOrEmpty(dr[column.ColumnName].ToString()))
                    {
                        if (pro.PropertyType == typeof(string))
                        {
                            pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                        }
                        else if(pro.PropertyType == typeof(DateTime?) || pro.PropertyType == typeof(DateTime))
                        {
                            var value = DateTime.ParseExact(dr[column.ColumnName].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                            pro.SetValue(obj, value, null);
                        }
                        else
                        {
                            try
                            {
                                pro.SetValue(obj, dr[column.ColumnName] , null);
                            }
                            catch(Exception ex)
                            {
                                throw new Exception($"Invalid type of property: {pro.Name}", ex);
                            }
                        }
                        break;
                    }
                }
            }
            
            return obj;
        }
    }
}
