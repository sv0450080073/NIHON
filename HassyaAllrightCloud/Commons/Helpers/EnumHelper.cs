using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public static class EnumHelper
    {
        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        public static string GetDescription<T>(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Description : value.ToString();
        }

        public static List<KeyValuePair<int, string>> GetEnumValuesAndDescriptions<T>()
        {
            Type enumType = typeof(T);

            if (enumType.GetTypeInfo().BaseType != typeof(Enum))
            {
                throw new ArgumentException("T is not System.Enum");
            }

            List<KeyValuePair<int, string>> enumValList = new List<KeyValuePair<int, string>>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                enumValList.Add(new KeyValuePair<int, string>((int)e, (attributes.Length > 0) ? attributes[0].Description : e.ToString()));
            }

            return enumValList;
        }

        public static List<KeyValuePair<int, string>> GetEnumValueAndName<T>()
        {
            Type enumType = typeof(T);

            if (enumType.GetTypeInfo().BaseType != typeof(Enum))
            {
                throw new ArgumentException("T is not System.Enum");
            }

            List<KeyValuePair<int, string>> enumValList = new List<KeyValuePair<int, string>>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                enumValList.Add(new KeyValuePair<int, string>((int)e, e.ToString()));
            }

            return enumValList;
        }
        public static string GetEnumDescription<T>(int? value)
        {
            return GetEnumDescription((Enum)(object)((T)(object)value));
        }

        public static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo?.GetCustomAttributes(
                typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Description : value.ToString();
        }

        public static IEnumerable<string> GetDescriptions<T>()
        {
            var attributes = typeof(T).GetMembers()
                .SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                .ToList();

            return attributes.Select(x => x.Description);
        }
    }
}
