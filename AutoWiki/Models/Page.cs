using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoWiki.Models
{
	internal class Page
	{
		public string FileName { get; set; }
		public List<TypeDoc> Types { get; } = new List<TypeDoc>();
	}

	internal abstract class Comment
	{
		public List<Tag> Tags { get; } = new List<Tag>();
	}

	internal class TypeDoc : Comment
	{
		public Type AssociatedType { get; set; }
		public List<MemberDoc> Members { get; } = new List<MemberDoc>();
	}

	internal class MemberDoc : Comment
	{
		public MemberInfo AssociatedMember { get; set; }
		public MemberType MemberType { get; set; }
	}

	internal class Tag
	{
		public string Name { get; set; }
		public string Text { get; set; }
	}

	internal class TypeParamTag : Tag
	{
		public string ParamName { get; set; }
	}

	internal class ParamTag : Tag
	{
		public string ParamName { get; set; }
	}

	internal class ExceptionTag : Tag
	{
		public string ExceptionType { get; set; }
	}
}