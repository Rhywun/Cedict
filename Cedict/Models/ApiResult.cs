using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Cedict.Models
{
	public class ApiResult<T>
	{
		/// <summary>
		///     Private constructor called by the CreateAsync method.
		/// </summary>
		private ApiResult(List<T> data,
		                  int     count,
		                  int     pageIndex,
		                  int     pageSize,
		                  string  sortColumn,
		                  string  sortOrder)
		{
			Data       = data;
			PageIndex  = pageIndex;
			PageSize   = pageSize;
			TotalCount = count;
			TotalPages = (int) Math.Ceiling(count / (double) pageSize);
			SortColumn = sortColumn;
			SortOrder  = sortOrder;
		}

		#region Methods

		/// <summary>Pages and/or sorts an IQueryable source.</summary>
		/// <param name="source">An IQueryable source of generic type</param>
		/// <param name="pageIndex">Zero-based current page index (0 = first page)</param>
		/// <param name="pageSize">The actual size of each page</param>
		/// <param name="sortColumn">The sorting column name</param>
		/// <param name="sortOrder">The sorting order ("ASC" or "DESC")</param>
		/// <returns>A object containing the paged result and all the relevant paging
		/// navigation info.</returns>
		public static async Task<ApiResult<T>> CreateAsync(IQueryable<T> source,
		                                                   int           pageIndex,
		                                                   int           pageSize,
		                                                   string        sortColumn = null,
		                                                   string        sortOrder  = null)
		{
			// Count all the items
			int count = await source.CountAsync();

			if (!String.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn))
			{
				// If we are here, sortColumn is safe to use
				sortOrder = !String.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC"
					? "ASC"
					: "DESC";
				source = source.OrderBy(String.Format("{0} {1}", sortColumn, sortOrder));
			}

			// Return one page of items
			source = source.Skip(pageIndex * pageSize).Take(pageSize);
			var data = await source.ToListAsync();
			return new ApiResult<T>(data, count, pageIndex, pageSize, sortColumn, sortOrder);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Checks if the given property name exists
		/// to protect against SQL injection attacks
		/// </summary>
		public static bool IsValidProperty(string propertyName,
		                                   bool   throwExceptionIfNotFound = true)
		{
			var prop = typeof(T).GetProperty(propertyName,
			                                 BindingFlags.IgnoreCase |
			                                 BindingFlags.Public |
			                                 BindingFlags.Instance);
			if (prop == null && throwExceptionIfNotFound)
				throw new NotSupportedException(
					String.Format("ERROR: Property '{0}' does not exist.", propertyName));
			return prop != null;
		}

		#endregion

		#region Properties

		/// <summary>The data result.</summary>
		public List<T> Data { get; }

		/// <summary>Zero-based index of current page.</summary>
		public int PageIndex { get; }

		/// <summary>Number of items contained in each page.</summary>
		public int PageSize { get; }

		/// <summary>Total items count.</summary>
		public int TotalCount { get; }

		/// <summary>Total pages count.</summary>
		public int TotalPages { get; }

		/// <summary>TRUE if the current page has a previous page, FALSE otherwise.</summary>
		public bool HasPreviousPage => PageIndex > 0;

		/// <summary>TRUE if the current page has a next page, FALSE otherwise.</summary>
		public bool HasNextPage => PageIndex + 1 < TotalPages;

		/// <summary>Sorting Column name (or null if none set).</summary>
		public string SortColumn { get; set; }

		/// <summary>Sorting Order ("ASC", "DESC" or null if none set).</summary>
		public string SortOrder { get; set; }

		#endregion
	}
}
