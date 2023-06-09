using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tapdaq
{
	public class TDSettings : ScriptableObject
	{
		private static TDSettings instance;

		public const string pluginVersion = "unity_6.3.3";

		public string ios_applicationID = string.Empty;

		public string ios_clientKey = string.Empty;

		public string android_applicationID = string.Empty;

		public string android_clientKey = string.Empty;

		public bool isDebugMode;

		public bool autoReloadAds;

		public AdTags tags = new AdTags();

		[SerializeField]
		public List<TestDevice> testDevices = new List<TestDevice>();

		public static TDSettings getInstance()
		{
			if (TDSettings.instance == null)
			{
				TDSettings.instance = Resources.LoadAll<TDSettings>("Tapdaq")[0];
			}
			return TDSettings.instance;
		}
	}
}
