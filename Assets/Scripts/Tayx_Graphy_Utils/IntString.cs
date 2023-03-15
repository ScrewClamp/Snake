using System;
using UnityEngine;

namespace Tayx.Graphy.Utils
{
	public static class IntString
	{
		public static string[] positiveBuffer = new string[0];

		public static string[] negativeBuffer = new string[0];

		public static float maxValue
		{
			get
			{
				return (float)IntString.positiveBuffer.Length;
			}
		}

		public static float minValue
		{
			get
			{
				return (float)IntString.negativeBuffer.Length;
			}
		}

		public static bool Inited
		{
			get
			{
				return IntString.positiveBuffer.Length > 0 || IntString.negativeBuffer.Length > 0;
			}
		}

		public static void Init(int minNegativeValue, int maxPositiveValue)
		{
			if (maxPositiveValue >= 0)
			{
				IntString.positiveBuffer = new string[maxPositiveValue];
				for (int i = 0; i < maxPositiveValue; i++)
				{
					IntString.positiveBuffer[i] = i.ToString();
				}
			}
			if (minNegativeValue <= 0)
			{
				int num = Mathf.Abs(minNegativeValue);
				IntString.negativeBuffer = new string[num];
				for (int j = 0; j < num; j++)
				{
					IntString.negativeBuffer[j] = (-j).ToString();
				}
			}
		}

		public static string ToStringNonAlloc(this int value)
		{
			if (value >= 0 && value < IntString.positiveBuffer.Length)
			{
				return IntString.positiveBuffer[value];
			}
			if (value < 0 && -value < IntString.negativeBuffer.Length)
			{
				return IntString.negativeBuffer[-value];
			}
			return value.ToString();
		}
	}
}
