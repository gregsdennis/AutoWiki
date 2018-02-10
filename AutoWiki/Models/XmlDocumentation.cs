using System.Collections.Generic;
using System.Xml.Linq;

namespace AutoWiki.Models
{
	internal class XmlDocumentation
	{
		public string AssemblyName { get; set; }
		public string MemberName { get; set; }
		public MemberType MemberType { get; set; }
		public List<XElement> Data { get; set; }
	}
}