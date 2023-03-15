using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tapdaq
{
	[Serializable]
	public class AdTags
	{
		public static string DefaultTag = "default";

		[SerializeField]
		private string[] tags = new string[Enum.GetValues(typeof(TDAdType)).Length];

		private static Func<KeyValuePair<TDAdType, string>, TagsList> __f__am_cache0;

		public string this[int index]
		{
			get
			{
				if (index >= this.tags.Count<string>())
				{
					this.UpdateTagsLength();
				}
				return this.tags[index];
			}
			set
			{
				if (index >= this.tags.Count<string>())
				{
					this.UpdateTagsLength();
				}
				this.tags[index] = value;
			}
		}

		public int Length
		{
			get
			{
				return this.tags.Length;
			}
		}

		private void UpdateTagsLength()
		{
			if (this.tags.Count<string>() < Enum.GetValues(typeof(TDAdType)).Length)
			{
				string[] array = new string[Enum.GetValues(typeof(TDAdType)).Length];
				for (int i = 0; i < this.tags.Count<string>(); i++)
				{
					array[i] = this.tags[i];
				}
				this.tags = array;
			}
		}

		public Dictionary<TDAdType, string> GetTags()
		{
			Dictionary<TDAdType, string> dictionary = new Dictionary<TDAdType, string>();
			for (int i = 1; i < this.tags.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.tags[i]))
				{
					dictionary.Add((TDAdType)i, this.tags[i]);
				}
			}
			return dictionary;
		}

		public string GetTagsJson()
		{
			TagsList[] value = (from pair in this.GetTags()
			select new TagsList(pair.Key, pair.Value)).ToArray<TagsList>();
			return JsonConvert.SerializeObject(value);
		}
	}
}
