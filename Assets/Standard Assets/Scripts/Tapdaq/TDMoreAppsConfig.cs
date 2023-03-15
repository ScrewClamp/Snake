using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Tapdaq
{
	[Serializable]
	public class TDMoreAppsConfig
	{
		public string placementTagPrefix;

		public int minAdsToDisplay;

		public int maxAdsToDisplay;

		public string headerText;

		public string installedAppButtonText;

		public string headerTextColor;

		public string headerColor;

		public string headerCloseButtonColor;

		public string backgroundColor;

		public string appNameColor;

		public string appButtonColor;

		public string appButtonTextColor;

		public string installedAppButtonColor;

		public string installedAppButtonTextColor;

		private static int ColorComponent(float val)
		{
			return (int)(Mathf.Clamp01(val) * 255f);
		}

		private static string HexConverter(Color c)
		{
			return string.Concat(new string[]
			{
				"#",
				TDMoreAppsConfig.ColorComponent(c.a).ToString("X2"),
				TDMoreAppsConfig.ColorComponent(c.r).ToString("X2"),
				TDMoreAppsConfig.ColorComponent(c.g).ToString("X2"),
				TDMoreAppsConfig.ColorComponent(c.b).ToString("X2")
			});
		}

		public void SetHeaderTextColor(Color color)
		{
			this.headerTextColor = TDMoreAppsConfig.HexConverter(color);
		}

		public void SetHeaderColor(Color color)
		{
			this.headerColor = TDMoreAppsConfig.HexConverter(color);
		}

		public void SetHeaderCloseButtonColor(Color color)
		{
			this.headerCloseButtonColor = TDMoreAppsConfig.HexConverter(color);
		}

		public void SetBackgroundColor(Color color)
		{
			this.backgroundColor = TDMoreAppsConfig.HexConverter(color);
		}

		public void SetAppNameColor(Color color)
		{
			this.appNameColor = TDMoreAppsConfig.HexConverter(color);
		}

		public void SetAppButtonColor(Color color)
		{
			this.appButtonColor = TDMoreAppsConfig.HexConverter(color);
		}

		public void SetAppButtonTextColor(Color color)
		{
			this.appButtonTextColor = TDMoreAppsConfig.HexConverter(color);
		}

		public void SetInstalledAppButtonColor(Color color)
		{
			this.installedAppButtonColor = TDMoreAppsConfig.HexConverter(color);
		}

		public void SetInstalledAppButtonTextColor(Color color)
		{
			this.installedAppButtonTextColor = TDMoreAppsConfig.HexConverter(color);
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, new JsonSerializerSettings
			{
				DefaultValueHandling = DefaultValueHandling.Ignore
			});
		}
	}
}
