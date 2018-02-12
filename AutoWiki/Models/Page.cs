using System.Collections.Generic;

namespace AutoWiki.Models
{
	internal class Page
	{
		public string FileName { get; set; }
		public List<TypeDoc> Types { get; } = new List<TypeDoc>();
	}
}