using Newtonsoft.Json;
using System;

namespace Tapdaq
{
	[Serializable]
	public class TDAdEvent
	{
		public string adType;

		public string message;

		public string tag;

		public TDAdError error;

		public TDAdEvent()
		{
		}

		public TDAdEvent(string adType, string message, string tag = null)
		{
			this.adType = adType;
			this.message = message;
			this.tag = tag;
		}

		public TDAdType GetTypeOfEvent()
		{
			if (this.adType == "INTERSTITIAL")
			{
				return TDAdType.TDAdTypeInterstitial;
			}
			if (this.adType == "BANNER")
			{
				return TDAdType.TDAdTypeBanner;
			}
			if (this.adType == "VIDEO")
			{
				return TDAdType.TDAdTypeVideo;
			}
			if (this.adType == "REWARD_AD")
			{
				return TDAdType.TDAdTypeRewardedVideo;
			}
			if (this.adType == "OFFERWALL")
			{
				return TDAdType.TDAdTypeOfferwall;
			}
			if (this.IsNativeAdEvent())
			{
				TDNativeAdType nativeEventType = this.GetNativeEventType();
				return TDEnumHelper.GetAdTypeFromNativeType(nativeEventType);
			}
			if (this.IsMoreAppsEvent())
			{
				return TDAdType.TDAdTypeNone;
			}
			return TDAdType.TDAdTypeNone;
		}

		public bool IsInterstitialEvent()
		{
			return this.GetTypeOfEvent() == TDAdType.TDAdTypeInterstitial;
		}

		public bool IsVideoEvent()
		{
			return this.GetTypeOfEvent() == TDAdType.TDAdTypeVideo;
		}

		public bool IsRewardedVideoEvent()
		{
			return this.GetTypeOfEvent() == TDAdType.TDAdTypeRewardedVideo;
		}

		public bool IsBannerEvent()
		{
			return this.GetTypeOfEvent() == TDAdType.TDAdTypeBanner;
		}

		public bool IsNativeAdEvent()
		{
			return this.adType == "NATIVE_AD";
		}

		public bool IsMoreAppsEvent()
		{
			return this.adType == "MORE_APPS";
		}

		public bool IsOfferwallEvent()
		{
			return this.adType == "OFFERWALL";
		}

		public TDNativeAdType GetNativeEventType()
		{
			if (this.IsNativeAdEvent() && this.message != null)
			{
				TDNativeAdMessage tDNativeAdMessage = JsonConvert.DeserializeObject<TDNativeAdMessage>(this.message);
				if (tDNativeAdMessage != null)
				{
					return TDEnumHelper.GetEnumFromString<TDNativeAdType>(tDNativeAdMessage.nativeType, TDNativeAdType.TDNativeAdType1x1Large);
				}
			}
			return TDNativeAdType.TDNativeAdTypeNone;
		}

		public string GetNativeEventMessage()
		{
			if (this.IsNativeAdEvent() && this.message != null)
			{
				TDNativeAdMessage tDNativeAdMessage = JsonConvert.DeserializeObject<TDNativeAdMessage>(this.message);
				if (tDNativeAdMessage != null)
				{
					return tDNativeAdMessage.messageText;
				}
			}
			return null;
		}
	}
}
