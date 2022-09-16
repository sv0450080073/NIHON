using System;
using System.Collections.Generic;
using System.Linq;

namespace HassyaAllrightCloud.Domain.Dto
{
    /// <summary>
    /// Pagging collection
    /// </summary>
    /// <typeparam name="T">Source type</typeparam>
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        /// <summary>
        /// Records per page. Default values is 10
        /// </summary>
        public int PageSize { get; private set; } = 10;
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> source, int count, int pageNumber, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException($"{nameof(pageNumber)} should be larger than zero.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException($"{nameof(pageSize)} should be larger than zero.");
            if (count < 0)
                throw new ArgumentOutOfRangeException($"{nameof(count)} should be larger or equals zero.");

            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(source);
        }

        /// <summary>
        /// Input list is a single page
        /// </summary>
        /// <param name="source"></param>
        public PagedList(List<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            int count = source.Count;
            TotalCount = count;
            PageSize = count;
            CurrentPage = 1;
            TotalPages = 1;

            AddRange(source);
        }

        /// <summary>
        /// Convert <see cref="IQueryable{T}"/> to <see cref="PagedList{T}"/>
        /// </summary>
        /// <param name="source">Source wanna converted</param>
        /// <param name="pageNumber">Page want to get</param>
        /// <param name="pageSize">Records per page</param>
        /// <returns><see cref="PagedList{T}"/> item had paged</returns>
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException($"{nameof(pageNumber)} should be larger than zero.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException($"{nameof(pageSize)} should be larger than zero.");

            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Get list paged
        /// </summary>
        /// <param name="source">Source wanna get</param>
        /// <param name="pageSize">Records per page</param>
        /// <returns>List paged data</returns>
        public static List<PagedList<T>> GetAllPageds(IQueryable<T> source, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException($"{nameof(pageSize)} should be larger than zero.");

            List<PagedList<T>> listPageds = new List<PagedList<T>>();
            int i = 1;
            while (true)
            {
                var page = PagedList<T>.ToPagedList(source, i++, pageSize);

                if (!page.Any())
                    break;

                listPageds.Add(page);
            }

            return listPageds;
        }
    }
}
