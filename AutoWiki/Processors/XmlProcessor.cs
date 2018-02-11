using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AutoWiki.Models;

namespace AutoWiki.Processors
{
	internal static class XmlProcessor
	{
		public static List<XmlDocumentation> Load(string fileName)
		{
			XDocument xml;
			using (var reader = new StreamReader(fileName))
			{
				xml = XDocument.Load(reader);
			}

			var assemblyName = xml.Root.Element("assembly").Element("name").Value;

			return xml.Root.Element("members").Elements().Select(e => _TranslateAssembly(e, assemblyName)).ToList();
		}

		private static XmlDocumentation _TranslateAssembly(XElement memberNode, string assemblyName)
		{
			var member = new XmlDocumentation
				{
					AssemblyName = assemblyName,
					Data = memberNode.Elements().ToList()
				};

			return memberNode.GetMemberDetails("name", member);
		}

		public static XmlDocumentation GetMemberDetails(this XElement memberNode, string attributeName, XmlDocumentation member)
		{
			var nameValue = memberNode.Attribute(attributeName).Value;
			var parts = nameValue.Split(":");

			switch (parts[0])
			{
				case "T":
					member.MemberType = MemberType.Type;
					member.MemberName = parts[1];
					break;
				case "F":
					member.MemberType = MemberType.Field;
					member.MemberName = parts[1];
					break;
				case "P":
					member.MemberType = MemberType.Property;
					member.MemberName = parts[1];
					break;
				case "M":
					parts = parts[1].Split(".#ctor");
					member.MemberType = parts.Length == 1
						                    ? MemberType.Method
						                    : MemberType.Constructor;
					member.MemberName = parts[0];
					break;
				case "E":
					member.MemberType = MemberType.Event;
					member.MemberName = parts[1];
					break;
			}

			return member;
		}
	}
}
