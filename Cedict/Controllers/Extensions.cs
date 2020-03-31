using System.Text.RegularExpressions;

namespace Cedict.Controllers
{
	public static class Extensions
	{
		// TODO: Figure out how to ignore tones - doesn't work in LINQ-to-SQL
		public static string IgnoreTones(this string s)
			=> Regex.Replace(s, @"\d", "");

	}
}
