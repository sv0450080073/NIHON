using System;
using System.Collections.Generic;
using System.Reflection;

namespace HassyaAllrightCloud.Commons.Extensions
{
    public static class ClonePropertiesClassExtension
    {
        /// <summary>
        /// Clone data of properties simple way. Use only for entity class model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dataDest">Destination data clone to</param>
        /// <param name="dataSource">Source data to clone</param>
        public static TModel SimpleCloneProperties<TModel>(this TModel dataDest, TModel dataSource) where TModel : class
        {
            if (dataDest is null || dataSource is null)
            {
                return null;
            }

            foreach (var property in typeof(TModel).GetProperties())
            {
                if (property.CanWrite && property.CanRead)
                {
                    property.SetValue(dataDest, property.GetValue(dataSource));
                }
            }

            return dataDest;
        }

        public static void SetPropertiesData<TModel>(this TModel dataDest, TModel dataSource) where TModel : class, new()
        {
            if (dataSource is null || dataDest is null)
            {
                return;
            }

            foreach (var property in dataDest.GetType().GetProperties())
            {
                if (property.CanWrite && property.CanRead)
                {
                    if (property.PropertyType.IsClass && !property.PropertyType.Namespace.StartsWith("System"))
                    {
                        var destChildValue = property.GetValue(dataDest);
                        var sourceChildValue = property.GetValue(dataSource);

                        if (sourceChildValue is null)
                        {
                            property.SetValue(dataDest, null);
                        }
                        else
                        {
                            if (destChildValue is null)
                            {
                                string typeName = property.PropertyType.FullName;

                                Assembly execAsm = Assembly.GetExecutingAssembly();
                                var newDestChildValue = AppDomain
                                   .CurrentDomain
                                   .CreateInstanceAndUnwrap(execAsm.FullName, typeName);

                                property.SetValue(dataDest, newDestChildValue);
                                destChildValue = property.GetValue(dataDest);
                            }
                            destChildValue.SetPropertiesData(sourceChildValue);
                        }
                    }
                    else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {

                    }
                    else
                    {
                        property.SetValue(dataDest, property.GetValue(dataSource));
                    }
                }
            }
        }
    }
}
