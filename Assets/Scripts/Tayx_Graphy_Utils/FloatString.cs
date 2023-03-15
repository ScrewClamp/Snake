using System;
using UnityEngine;

namespace Tayx.Graphy.Utils
{
	public static class FloatString
	{
		private static float decimalMultiplayer = 1f;

		public static string[] positiveBuffer = new string[0];

		public static string[] negativeBuffer = new string[0];

		public static bool Inited
		{
			get
			{
				return FloatString.positiveBuffer.Length > 0 || FloatString.negativeBuffer.Length > 0;
			}
		}

		public static float maxValue
		{
			get
			{
				return (FloatString.positiveBuffer.Length - 1).FromIndex();
			}
		}

		public static float minValue
		{
			get
			{
				return -(FloatString.negativeBuffer.Length - 1).FromIndex();
			}
		}

		public static void Init(float minNegativeValue, float maxPositiveValue, int deciminals = 1)
		{
			FloatString.decimalMultiplayer = (float)FloatString.Pow(10, Mathf.Clamp(deciminals, 1, 5));
			int num = minNegativeValue.ToIndex();
			int num2 = maxPositiveValue.ToIndex();
			if (num2 >= 0)
			{
				FloatString.positiveBuffer = new string[num2];
				for (int i = 0; i < num2; i++)
				{
					FloatString.positiveBuffer[i] = i.FromIndex().ToString("0.0");
				}
			}
			if (num >= 0)
			{
				FloatString.negativeBuffer = new string[num];
				for (int j = 0; j < num; j++)
				{
					FloatString.negativeBuffer[j] = (-j).FromIndex().ToString("0.0");
				}
			}
		}

		public static string ToStringNonAlloc(this float value)
		{
			int num = value.ToIndex();
			if (value >= 0f && num < FloatString.positiveBuffer.Length)
			{
				return FloatString.positiveBuffer[num];
			}
			if (value < 0f && num < FloatString.negativeBuffer.Length)
			{
				return FloatString.negativeBuffer[num];
			}
			return value.ToString();
		}

		public static string ToStringNonAlloc(this float value, string format)
		{
			int num = value.ToIndex();
			if (value >= 0f && num < FloatString.positiveBuffer.Length)
			{
				return FloatString.positiveBuffer[num];
			}
			if (value < 0f && num < FloatString.negativeBuffer.Length)
			{
				return FloatString.negativeBuffer[num];
			}
			return value.ToString(format);
		}

		private static int Pow(int f, int p)
		{
			for (int i = 1; i < p; i++)
			{
				f *= f;
			}
			return f;
		}

		private static int ToIndex(this float f)
		{
			return Mathf.Abs((f * FloatString.decimalMultiplayer).ToInt());
		}

		private static float FromIndex(this int i)
		{
			return i.ToFloat() / FloatString.decimalMultiplayer;
		}

		public static int ToInt(this float f)
		{
			return (int)f;
		}

		public static float ToFloat(this int i)
		{
			return (float)i;
		}
	}
}
