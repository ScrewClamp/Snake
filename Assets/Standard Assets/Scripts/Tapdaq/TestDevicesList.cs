using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tapdaq
{
	[Serializable]
	public class TestDevicesList
	{
		[SerializeField]
		public List<string> adMobDevices = new List<string>();

		[SerializeField]
		public List<string> facebookDevices = new List<string>();

		public TestDevicesList(List<TestDevice> devices, TestDeviceType deviceType)
		{
			foreach (TestDevice current in devices)
			{
				if (current.type == deviceType)
				{
					if (!string.IsNullOrEmpty(current.adMobId))
					{
						this.adMobDevices.Add(current.adMobId);
					}
					if (!string.IsNullOrEmpty(current.facebookId))
					{
						this.facebookDevices.Add(current.facebookId);
					}
				}
			}
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}

		public string GetAdMobListJson()
		{
			return JsonConvert.SerializeObject(this.adMobDevices);
		}

		public string GetFacebookListJson()
		{
			return JsonConvert.SerializeObject(this.facebookDevices);
		}
	}
}
