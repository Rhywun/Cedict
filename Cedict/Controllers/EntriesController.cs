using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cedict.Models;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cedict.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EntriesController : ControllerBase
	{
		private readonly CedictContext _context;

		public EntriesController(CedictContext context) => _context = context;

		// GET: api/entries?query=zhong1
		// GET: api/entries?query=zhong1&type=s
		// GET: api/entries?query=zhong1&type=s&skip=0&take=10
		// GET: api/entries?query=zhong1guo2ren2
		// GET: api/entries?query=中国人&type=c
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK,
		                      Type = typeof(IEnumerable<Entry>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResult<Entry>>> GetEntries(
			string query     = "zhong1",
			string type      = "s",
			int    pageIndex = 0,
			int    pageSize  = 10)
		{
			if (type != "s" && type != "c")
			{
				return BadRequest();
			}

			if (type == "s")
			{
				// Standard search
				return await ApiResult<Entry>.CreateAsync(
					_context.Entries.Where(GetStandardPredicate(query))
					        .AsQueryable(),
					pageIndex,
					pageSize);
			}
			else // type == "c"
			{
				// Multi-character search
				return await ApiResult<Entry>.CreateAsync(
					_context.Entries.Where(GetCharacterPredicate(query))
					        .AsQueryable(),
					pageIndex,
					pageSize);
			}
		}

		private static Expression<Func<Entry, bool>> GetStandardPredicate(string query)
		{
			query = query.ToUpper();
			var pred = PredicateBuilder.New<Entry>(true);

			if (query.StartsWith('*') && query.EndsWith('*'))
			{
				query = query.Trim('*');
				pred  = pred.Or(e => e.Traditional.Contains(query));
				pred  = pred.Or(e => e.Simplified.Contains(query));
				pred = pred.Or(e => e.Pinyin.ToUpper()
				                     .Replace(" ", "")
				                     .Contains(query));
				pred = pred.Or(e => e.English.ToUpper().Contains(query));
			}
			else if (query.StartsWith('*'))
			{
				query = query.Trim('*');
				pred  = pred.Or(e => e.Traditional.EndsWith(query));
				pred  = pred.Or(e => e.Simplified.EndsWith(query));
				pred = pred.Or(e => e.Pinyin.ToUpper()
				                     .Replace(" ", "")
				                     .EndsWith(query));
				pred = pred.Or(e => e.English.ToUpper().EndsWith(query));
			}
			else if (query.EndsWith('*'))
			{
				query = query.Trim('*');
				pred  = pred.Or(e => e.Traditional.StartsWith(query));
				pred  = pred.Or(e => e.Simplified.StartsWith(query));
				pred = pred.Or(e => e.Pinyin.ToUpper()
				                     .Replace(" ", "")
				                     .StartsWith(query));
				pred = pred.Or(e => e.English.ToUpper().StartsWith(query));
			}
			else
			{
				pred = pred.Or(e => e.Traditional == query);
				pred = pred.Or(e => e.Simplified == query);
				pred = pred.Or(
					e => e.Pinyin.ToUpper().Replace(" ", "") == query);
				pred = pred.Or(e => e.English.ToUpper() == query);
			}

			return pred;
		}

		private static Expression<Func<Entry, bool>> GetCharacterPredicate(
			string query)
		{
			var predicate = PredicateBuilder.New<Entry>(true);
			foreach (char c in query)
			{
				predicate = predicate.Or(e => e.Traditional == c.ToString());
				predicate = predicate.Or(e => e.Simplified == c.ToString());
			}

			return predicate;
		}
	}
}
