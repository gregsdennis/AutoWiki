using System.Text;
using System.Text.RegularExpressions;

namespace AutoWiki
{
	internal static class StringBuilderExtensions
	{
		public static void Header(this StringBuilder sb, int level, string header)
		{
			sb.AppendLine(Regex.Replace($"{new string('#', level)} {header}", @"\s+", " ", RegexOptions.Multiline));
			sb.AppendLine();
		}

		public static void Paragraph(this StringBuilder sb, string content)
		{
			sb.AppendLine(Regex.Replace(content, @"\s+", " ", RegexOptions.Multiline));
			sb.AppendLine();
		}
	}
}