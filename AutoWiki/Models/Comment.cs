using System.Collections.Generic;

namespace AutoWiki.Models
{
	internal abstract class Comment
	{
		public List<Tag> Tags { get; } = new List<Tag>();
	}
}