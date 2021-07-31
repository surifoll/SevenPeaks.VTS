using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExIgniter.ObjectMapper.ObjectMapper;
using Microsoft.EntityFrameworkCore;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;

namespace SevenPeaks.VTS.Application.Helpers
{
    public class PagedResultHelper
    {
        /// <summary>
        /// Creates a paged set of results.
        /// </summary>
        /// <typeparam name="T">The type of the source IQueryable.</typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="uriService"></param>
        /// <param name="queryable">The source IQueryable.</param>
        /// <param name="page">The page number you want to retrieve.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="route"></param>
        /// <param name="otherQueryString"> for other query params/string</param>
        /// <returns>Returns a paged set of results.</returns>
        public static async Task<PagedResults<TReturn>> CreatePagedResults<T, TReturn>(IUriService uriService,
        IQueryable<T> queryable,
        int page,
        int pageSize,
        string route,
        string otherQueryString
        )
        {
            var skipAmount = pageSize * (page - 1);

            var projection = queryable
            .Skip(skipAmount)
            .Take(pageSize);

            var totalNumberOfRecords = await queryable.CountAsync();
            var results = await projection.ToListAsync();

            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = totalNumberOfRecords / pageSize + (mod == 0 ? 0 : 1);

            var nextPageUrl =
            page == totalPageCount
            ? null
            : uriService.GetPageUri(new PaginationFilter(page + 1, pageSize), route);

            var prevPageUrl =
            page < 2
            ? null
            : uriService.GetPageUri(new PaginationFilter(page - 1, pageSize), route);
            //var mappedObject = testCustomer.Map(new CustomerVm(), vm => new[] { nameof(vm.City), nameof(vm.Order) });

            nextPageUrl = nextPageUrl == null
            ? null
            : new Uri($"{nextPageUrl}&{otherQueryString}");
            prevPageUrl = prevPageUrl == null
            ? null
            : new Uri($"{prevPageUrl}&{otherQueryString}");

            return new PagedResults<TReturn>
            {
                Results = results.Map(new List<TReturn>()),
                PageNumber = page,
                PageSize = results.Count,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords,
                NextPageUrl = nextPageUrl,
                PrevPageUrl = prevPageUrl
            };
        }

        public static PagedResults<T1> CreatePagedResults<T, T1>(IUriService uriService, List<T> queryable, int page,
        int pageSize, string route, string otherQueryString)
        {
            var skipAmount = pageSize * (page - 1);

            var projection = queryable
            .Skip(skipAmount)
            .Take(pageSize);

            var totalNumberOfRecords = queryable.Count();
            var results = projection.ToList();

            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = totalNumberOfRecords / pageSize + (mod == 0 ? 0 : 1);

            var nextPageUrl =
            page == totalPageCount
            ? null
            : uriService.GetPageUri(new PaginationFilter(page + 1, pageSize), route);

            var prevPageUrl =
            page < 2
            ? null
            : uriService.GetPageUri(new PaginationFilter(page - 1, pageSize), route);
            //var mappedObject = testCustomer.Map(new CustomerVm(), vm => new[] { nameof(vm.City), nameof(vm.Order) });
            nextPageUrl = nextPageUrl == null
            ? null
            : new Uri($"{nextPageUrl}&{otherQueryString}");
            prevPageUrl = prevPageUrl == null
            ? null
            : new Uri($"{prevPageUrl}&{otherQueryString}");
            return new PagedResults<T1>
            {
                Results = (IEnumerable<T1>)results,
                PageNumber = page,
                PageSize = results.Count,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords,
                NextPageUrl = nextPageUrl,
                PrevPageUrl = prevPageUrl
            };
        }
    }
}
 
