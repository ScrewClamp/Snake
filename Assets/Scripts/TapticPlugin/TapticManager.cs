using System;
using UnityEngine;

namespace TapticPlugin
{
	public static class TapticManager
	{
		public static void Notification(NotificationFeedback feedback)
		{
			TapticManager._unityTapticNotification((int)feedback);
		}

		public static void Impact(ImpactFeedback feedback)
		{
			TapticManager._unityTapticImpact((int)feedback);
		}

		public static void Selection()
		{
			TapticManager._unityTapticSelection();
		}

		public static bool IsSupported()
		{
			string text = SystemInfo.deviceModel;
			if (text == null || !text.Contains("iPhone"))
			{
				return false;
			}
			text = text.Substring(6, text.IndexOf(',') - 6);
			int num = 0;
			return int.TryParse(text, out num) && num > 8;
		}

		private static void _unityTapticNotification(int type)
		{
		}

		private static void _unityTapticSelection()
		{
		}

		private static void _unityTapticImpact(int style)
		{
		}
	}
}
