using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HassyaAllrightCloud.Commons.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Using foreach with index
        /// </summary>
        /// <example>
        /// foreach (var (item, index) in collection.WithIndex())
        /// {
        ///    DoSomething(item, index);
        /// }
        /// </example>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="source">Source with index</param>
        /// <returns>Linq and tuples</returns>
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        /// <summary>
        /// Convert collection list to <see cref="DataTable"/>
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="source">Source</param>
        /// <returns>Collection as <see cref="DataTable"/> type</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in source)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// Convert <see cref="IEnumerable{T}"/> value to serial text as string. Per item delimiter by a newline
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="data">Object will be convert</param>
        /// <returns>Serial text of source as string</returns>
        public static StringBuilder ToSerialText<T>(this IEnumerable<T> source)
        {
            if(source is null)
            {
                throw new ArgumentNullException(nameof(source), "Data source should be not null");
            }

            StringBuilder result = new StringBuilder();
            Type type = typeof(T);
            List<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

            foreach(T item in source)
            {
                string serialText = string.Empty;
                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(item, null);

                    serialText += propValue?.ToString() ?? string.Empty;
                }
                result.AppendLine(serialText);
            }
            return result;
        }

        /// <summary>
        /// Convert <see cref="IEnumerable{T}"/> value to serial text as string. Per item delimiter by a newline
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="source">Data source will be converted</param>
        /// <param name="delimiter">Symbol delimiter between field value</param>
        /// <param name="groupSymbol">Symbol cover field value</param>
        /// <returns>Serial text of source after converted as string</returns>
        public static StringBuilder ToDelimiterText<T>(this IEnumerable<T> source, string delimiter = "", string groupSymbol = "")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source), "Data source should be not null");
            }

            StringBuilder result = new StringBuilder();
            Type type = typeof(T);
            List<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

            foreach (T item in source)
            {
                // this line will be ignore any fields is boolean type
                var objValues = props.Where(_=>_.PropertyType != typeof(bool)).Select(_ => new string(string.Concat(groupSymbol, _?.GetValue(item, null)?.ToString()?? string.Empty, groupSymbol).ToArray()));
                result.AppendLine(string.Join(delimiter, objValues));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="selectKeyLeft"></param>
        /// <param name="selectKeyRight"></param>
        /// <param name="projection"></param>
        /// <param name="defaultA"></param>
        /// <param name="defaultB"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IList<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> selectKeyLeft,
            Func<TRight, TKey> selectKeyRight,
            Func<TLeft, TRight, TKey, TResult> projection,
            TLeft defaultA = default(TLeft),
            TRight defaultB = default(TRight),
            IEqualityComparer<TKey> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var alookup = left.ToLookup(selectKeyLeft, comparer);
            var blookup = right.ToLookup(selectKeyRight, comparer);

            var keys = new HashSet<TKey>(alookup.Select(p => p.Key), comparer);
            keys.UnionWith(blookup.Select(p => p.Key));

            var join = from key in keys
                       from xa in alookup[key].DefaultIfEmpty(defaultA)
                       from xb in blookup[key].DefaultIfEmpty(defaultB)
                       select projection(xa, xb, key);

            return join.ToList();
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the default equality comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return _(); IEnumerable<TSource> _()
            {
                var knownKeys = new HashSet<TKey>(comparer);
                foreach (var element in source)
                {
                    if (knownKeys.Add(keySelector(element)))
                        yield return element;
                }
            }
        }

        /// <summary>
        /// Convert multiple to single dictionary value with same key. The key will be refactor.
        /// <example>
        ///  (Key : Values) A-0 : 11, A-1 : 33, A-2 : 44, A-3 : 55
        ///  Output: nguyenne : 11-33-44-55
        /// </example>
        /// </summary>
        /// <param name="values">Input values</param>
        /// <returns>New dictionary with distinct key format</returns>
        public static Dictionary<string, string> ConvertMultipleToSingleValues(this Dictionary<string, string> values)
        {
            Dictionary<string, string> newValues = new Dictionary<string, string>();

            var keys = values.Select(_ => _.Key.Split("-").FirstOrDefault()).Distinct();

            foreach (string key in keys)
            {
                var vals = values.Where(_ => _.Key.Split('-').FirstOrDefault()?.Equals(key) ?? false).Select(_ => _.Value);

                newValues.Add(key, string.Join('-', vals));
            }

            return newValues;
        }

        /// <summary>
        /// Convert single to multiple dictionary value with same key. The key will be generate follow new format with old key.
        /// <para>Key format: [OldKey]-[Times][IndexIncreate]</para>
        /// <example>
        ///  Key: nguyenne
        ///  Values: 11-33-44-55
        ///  Output => A-0 : 11, A-1 : 33, A-2 : 44, A-3 : 55
        /// </example>
        /// </summary>
        /// <param name="values">Input values</param>
        /// <param name="times">The text to distinguish input same value key</param>
        /// <returns>New dictionary with new format</returns>
        public static Dictionary<string, string> ConvertSingleToMultipleValues(this Dictionary<string, string> values, string times = null)
        {
            Dictionary<string, string> newValues = new Dictionary<string, string>();
            foreach (var item in values)
            {
                var vals = item.Value.Split("-");

                foreach (var (val, index) in vals.WithIndex())
                {
                    newValues.Add(item.Key + "-" + times + index, val);
                }
            }

            return newValues;
        }
    }
}
