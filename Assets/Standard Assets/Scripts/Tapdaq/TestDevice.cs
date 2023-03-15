using System;

namespace Tapdaq
{
	[Serializable]
	public class TestDevice
	{
		public string name;

		public TestDeviceType type;

		public string adMobId;

		public string facebookId;

		public TestDevice(string deviceName, TestDeviceType deviceType)
		{
			this.name = deviceName;
			this.type = deviceType;
		}
	}
}
