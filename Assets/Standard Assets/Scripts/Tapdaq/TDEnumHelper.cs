using System;
using System.Collections;

namespace Tapdaq
{
	public static class TDEnumHelper
	{
		public static string FixAndroidAdapterName(string adapterName)
		{
			if (adapterName == Enum.GetName(typeof(TapdaqAdapter), TapdaqAdapter.FANAdapter))
			{
				return "FacebookAdapter";
			}
			return adapterName;
		}

		public static TDAdType GetAdTypeFromNativeType(TDNativeAdType nativeType)
		{
			string text = nativeType.ToString();
			string enumString = text.Replace("TDNativeAdType", "TDAdType");
			return TDEnumHelper.GetEnumFromString<TDAdType>(enumString, TDAdType.TDAdTypeNone);
		}

		public static TDNativeAdType GetNativeAdTypeFromAdType(TDAdType adType)
		{
			string text = adType.ToString();
			string enumString = text.Replace("TDAdType", "TDNativeAdType");
			return TDEnumHelper.GetEnumFromString<TDNativeAdType>(enumString, TDNativeAdType.TDNativeAdTypeNone);
		}

		public static T GetEnumFromString<T>(string enumString, T defaultValue)
		{
			Array array = null;
			try
			{
				array = Enum.GetValues(typeof(T));
			}
			catch (Exception var_1_17)
			{
				TDDebugLogger.LogError("Can't GetEnumFromString: " + enumString);
				T result = defaultValue;
				return result;
			}
			if (array == null)
			{
				return defaultValue;
			}
			IEnumerator enumerator = array.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (current.ToString().ToLower() == enumString.ToLower())
					{
						T result = (T)((object)current);
						return result;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return defaultValue;
		}
	}
}
