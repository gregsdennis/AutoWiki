using System.Collections.Generic;
using System.Xml.Linq;

namespace AutoWiki.Models
{
	internal class Page
	{
		public string FileName { get; set; }
		public List<Type> Types { get; set; }
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

	internal class XmlDocumentation
	{
		public string AssemblyName { get; set; }
		public string MemberName { get; set; }
		public MemberType MemberType { get; set; }
		public List<XElement> Data { get; set; }
	}

	internal enum MemberType
	{
		Type,
		Field,
		Property,
		Method,
		Event,
		Constructor
	}
}

