using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoWiki
{
	internal class MemberInfoComparer : IEqualityComparer<MemberInfo>
	{
		public static MemberInfoComparer Instance { get; } = new MemberInfoComparer();

		private MemberInfoComparer() { }

		public bool Equals(MemberInfo x, MemberInfo y)
		{
			return x.GetType() == y.GetType() &&
			       x.Name == y.Name &&
			       _AreEqualMethods(x as MethodBase, y as MethodBase) &&
			       _AreEqualMethods(x as PropertyInfo, y as PropertyInfo);
		}
		public int GetHashCode(MemberInfo obj)
		{
			return 0;
		}

		private static bool _AreEqualMethods(MethodBase x, MethodBase y)
		{
			if (x == null) return true;

			return x.GetParameters().SequenceEqual(y.GetParameters());
		}

		private static bool _AreEqualMethods(PropertyInfo x, PropertyInfo y)
		{
			if (x == null) return true;

			var xParameters = x.GetIndexParameters();
			var yParameters = y.GetIndexParameters();

			return (xParameters == null && yParameters == null) || (xParameters?.SequenceEqual(y.GetIndexParameters()) ?? false);
		}
	}
}