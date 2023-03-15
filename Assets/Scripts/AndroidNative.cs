using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AndroidNative
{
	private sealed class _CallStatic_c__AnonStorey1
	{
		internal string methodName;

		internal object[] args;
	}

	private sealed class _CallStatic_c__AnonStorey0
	{
		internal AndroidJavaObject bridge;

		internal AndroidNative._CallStatic_c__AnonStorey1 __f__ref_1;

		internal void __m__0()
		{
			this.bridge.CallStatic(this.__f__ref_1.methodName, this.__f__ref_1.args);
		}
	}

	public static void CallStatic(string methodName, params object[] args)
	{
		try
		{
			string className = "com.noorgames.rateus.librateusplugin.RateUsManager";
			AndroidJavaObject bridge = new AndroidJavaObject(className, Array.Empty<object>());
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			@static.Call("runOnUiThread", new object[]
			{
				new AndroidJavaRunnable(delegate
				{
					bridge.CallStatic(methodName, args);
				})
			});
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning(ex.Message);
		}
	}

	public static void ShowRateUsPopupNative(string title, string message, string rate, string remind, string declined)
	{
		AndroidNative.CallStatic("ShowRatePopup", new object[]
		{
			title,
			message,
			rate,
			remind,
			declined
		});
	}

	public static void RedirectToAppStoreRatingPage(string appLink)
	{
		AndroidNative.CallStatic("OpenAppRatingPage", new object[]
		{
			appLink
		});
	}
}
