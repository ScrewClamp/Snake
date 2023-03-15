/*
using System;
using UnityEngine;

public static class Vibration
{
	public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

	public static AndroidJavaObject currentActivity = Vibration.unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

	public static AndroidJavaObject vibrator = Vibration.currentActivity.Call<AndroidJavaObject>("getSystemService", new object[]
	{
		"vibrator"
	});

	public static void Vibrate()
	{
		if (Vibration.isAndroid())
		{
			Vibration.vibrator.Call("vibrate", Array.Empty<object>());
		}
		else
		{
			Handheld.Vibrate();
		}
	}

	public static void Vibrate(long milliseconds)
	{
		if (Vibration.isAndroid())
		{
			Vibration.vibrator.Call("vibrate", new object[]
			{
				milliseconds
			});
		}
		else
		{
			Handheld.Vibrate();
		}
	}

	public static void Vibrate(long[] pattern, int repeat)
	{
		if (Vibration.isAndroid())
		{
			Vibration.vibrator.Call("vibrate", new object[]
			{
				pattern,
				repeat
			});
		}
		else
		{
			Handheld.Vibrate();
		}
	}

	public static bool HasVibrator()
	{
		return Vibration.isAndroid();
	}

	public static void Cancel()
	{
		if (Vibration.isAndroid())
		{
			Vibration.vibrator.Call("cancel", Array.Empty<object>());
		}
	}

	private static bool isAndroid()
	{
		return true;
	}
}
*/