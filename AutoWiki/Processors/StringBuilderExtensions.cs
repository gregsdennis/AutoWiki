using System.Text;

namespace AutoWiki.Processors
{
	internal static class StringBuilderExtensions
	{
		public static void Header(this StringBuilder sb, int level, string content)
		{
			sb.AppendLine($"{new string('#', level)} {content}");
		}
	}
}