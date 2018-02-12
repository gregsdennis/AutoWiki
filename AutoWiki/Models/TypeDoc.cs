using System;
using System.Collections.Generic;

namespace AutoWiki.Models
{
	internal class TypeDoc : Comment
	{
		public Type AssociatedType { get; set; }
		public List<MemberDoc> Members { get; } = new List<MemberDoc>();
	}
}