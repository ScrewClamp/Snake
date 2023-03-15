using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tapdaq
{
	public static class TDExtensionMethods
	{
		public static int ParseInt(this string str, int defaultValue)
		{
			int result;
			if (int.TryParse(str, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static float ParseFloat(this string str, float defaultValue)
		{
			float result;
			if (float.TryParse(str, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static TV GetValue<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV defaultValue)
		{
			if (!dict.ContainsKey(key))
			{
				return defaultValue;
			}
			return dict[key];
		}

		public static T GetValueOrDefault<T>(this List<T> list, int index, T def)
		{
			if (index >= list.Count)
			{
				return def;
			}
			return list[index];
		}

		public static Vector2 ToVector2(this TDNativeAdType adType)
		{
			switch (adType)
			{
			case TDNativeAdType.TDNativeAdType1x1Large:
				return new Vector2(750f, 750f);
			case TDNativeAdType.TDNativeAdType1x1Medium:
				return new Vector2(375f, 375f);
			case TDNativeAdType.TDNativeAdType1x1Small:
				return new Vector2(150f, 150f);
			case TDNativeAdType.TDNativeAdType1x2Large:
				return new Vector2(900f, 1800f);
			case TDNativeAdType.TDNativeAdType1x2Medium:
				return new Vector2(450f, 900f);
			case TDNativeAdType.TDNativeAdType1x2Small:
				return new Vector2(180f, 360f);
			case TDNativeAdType.TDNativeAdType2x1Large:
				return new Vector2(1800f, 900f);
			case TDNativeAdType.TDNativeAdType2x1Medium:
				return new Vector2(900f, 450f);
			case TDNativeAdType.TDNativeAdType2x1Small:
				return new Vector2(360f, 180f);
			case TDNativeAdType.TDNativeAdType2x3Large:
				return new Vector2(960f, 1440f);
			case TDNativeAdType.TDNativeAdType2x3Medium:
				return new Vector2(480f, 720f);
			case TDNativeAdType.TDNativeAdType2x3Small:
				return new Vector2(192f, 288f);
			case TDNativeAdType.TDNativeAdType3x2Large:
				return new Vector2(1440f, 960f);
			case TDNativeAdType.TDNativeAdType3x2Medium:
				return new Vector2(720f, 480f);
			case TDNativeAdType.TDNativeAdType3x2Small:
				return new Vector2(288f, 192f);
			case TDNativeAdType.TDNativeAdType1x5Large:
				return new Vector2(360f, 1800f);
			case TDNativeAdType.TDNativeAdType1x5Medium:
				return new Vector2(180f, 900f);
			case TDNativeAdType.TDNativeAdType1x5Small:
				return new Vector2(72f, 360f);
			case TDNativeAdType.TDNativeAdType5x1Large:
				return new Vector2(1800f, 360f);
			case TDNativeAdType.TDNativeAdType5x1Medium:
				return new Vector2(900f, 180f);
			case TDNativeAdType.TDNativeAdType5x1Small:
				return new Vector2(360f, 72f);
			default:
				return Vector2.zero;
			}
		}
	}
}
