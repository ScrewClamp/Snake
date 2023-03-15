using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Tapdaq
{
	public class TDCallbacks
	{
		private static TDCallbacks reference;



























		public static event Action<TDAdEvent> AdAvailable;

		public static event Action<TDAdEvent> AdNotAvailable;

		public static event Action<TDAdEvent> AdWillDisplay;

		public static event Action<TDAdEvent> AdDidDisplay;

		public static event Action<TDAdEvent> AdClicked;

		public static event Action<TDAdEvent> AdClosed;

		public static event Action<TDAdEvent> AdError;

		public static event Action TapdaqConfigLoaded;

		public static event Action<TDAdError> TapdaqConfigFailedToLoad;

		public static event Action<TDVideoReward> RewardVideoValidated;

		public static event Action<Dictionary<string, object>> CustomEvent;

		[Obsolete("Use events 'AdWillDisplay' and 'AdDidDisplay' instead.")]
		public static event Action<TDAdEvent> AdStarted;

		[Obsolete("Use event 'AdClosed' instead.")]
		public static event Action<TDAdEvent> AdFinished;

		public static TDCallbacks instance
		{
			get
			{
				if (TDCallbacks.reference == null)
				{
					TDCallbacks.reference = new TDCallbacks();
				}
				return TDCallbacks.reference;
			}
		}

		internal TDCallbacks()
		{
		}

		private static void Invoke<T>(Action<T> action, T value)
		{
			if (action != null)
			{
				action(value);
			}
		}

		private static void Invoke(Action action)
		{
			if (action != null)
			{
				action();
			}
		}

		public void OnAdAvailable(TDAdEvent adEvent)
		{
			TDCallbacks.Invoke<TDAdEvent>(TDCallbacks.AdAvailable, adEvent);
		}

		public void OnAdClicked(TDAdEvent adEvent)
		{
			TDCallbacks.Invoke<TDAdEvent>(TDCallbacks.AdClicked, adEvent);
		}

		public void OnAdError(TDAdEvent adEvent)
		{
			TDCallbacks.Invoke<TDAdEvent>(TDCallbacks.AdError, adEvent);
		}

		public void OnAdClosed(TDAdEvent adEvent)
		{
			TDCallbacks.Invoke<TDAdEvent>(TDCallbacks.AdClosed, adEvent);
		}

		public void OnAdNotAvailable(TDAdEvent adEvent)
		{
			TDCallbacks.Invoke<TDAdEvent>(TDCallbacks.AdNotAvailable, adEvent);
		}

		public void OnAdDidDisplay(TDAdEvent adEvent)
		{
			TDCallbacks.Invoke<TDAdEvent>(TDCallbacks.AdDidDisplay, adEvent);
		}

		public void OnAdWillDisplay(TDAdEvent adEvent)
		{
			TDCallbacks.Invoke<TDAdEvent>(TDCallbacks.AdWillDisplay, adEvent);
		}

		public void OnTapdaqConfigLoaded()
		{
			TDCallbacks.Invoke(TDCallbacks.TapdaqConfigLoaded);
		}

		public void OnTapdaqConfigFailedToLoad(TDAdError error)
		{
			TDCallbacks.Invoke<TDAdError>(TDCallbacks.TapdaqConfigFailedToLoad, error);
		}

		public void OnRewardedVideoValidated(TDVideoReward reward)
		{
			TDCallbacks.Invoke<TDVideoReward>(TDCallbacks.RewardVideoValidated, reward);
		}

		public void OnCustomEvent(Dictionary<string, object> dictionary)
		{
			TDCallbacks.Invoke<Dictionary<string, object>>(TDCallbacks.CustomEvent, dictionary);
		}
	}
}
