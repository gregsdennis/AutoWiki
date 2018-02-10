using System.Collections.Generic;

namespace AutoWiki.Models
{
	internal class Page
	{
		public string FileName { get; set; }
		public List<Type> Types { get; } = new List<Type>();
	}

	internal class Type
	{
		public System.Type AssociatedType { get; set; }

		public string Assembly { get; set; }
		public string Namespace { get; set; }
		public string Name { get; set; }
		public List<Property> Properties { get; } = new List<Property>();
	}

	internal class Property
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public Dictionary<string, string> Sections { get; } = new Dictionary<string, string>();
	}
}

