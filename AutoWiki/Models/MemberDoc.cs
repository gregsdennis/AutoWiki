using System.Reflection;

namespace AutoWiki.Models
{
	internal class MemberDoc : Comment
	{
		public MemberInfo AssociatedMember { get; set; }
		public MemberType MemberType { get; set; }
	}
}