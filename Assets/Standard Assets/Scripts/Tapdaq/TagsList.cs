using System;
using System.Collections.Generic;
using System.Linq;

namespace Tapdaq
{
	[Serializable]
	public class TagsList
	{
		public string ad_type;

		public List<string> placement_tags;

		public TagsList(TDAdType adType, string tags)
		{
			this.ad_type = adType.ToString();
			this.placement_tags = tags.Split(new string[]
			{
				","
			}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
		}
	}
}
