using System.Text.RegularExpressions;

namespace Cedict.Controllers
{
	public static class Extensions
	{
		public static string StripDigits(this string s)
			=> Regex.Replace(s, @"\d", "");

	}
}
