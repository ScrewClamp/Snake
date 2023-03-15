using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy
{
	public class ShaderGraph
	{
		public const int ArrayMaxSizeFull = 512;

		public const int ArrayMaxSizeLight = 128;

		public int ArrayMaxSize = 128;

		public Image Image;

		private string Name = "GraphValues";

		private string Name_Length = "GraphValues_Length";

		public float[] Array;

		public float Average;

		private int averagePropertyId;

		public float GoodThreshold;

		public float CautionThreshold;

		private int goodThresholdPropertyId;

		private int cautionThresholdPropertyId;

		public Color GoodColor;

		public Color CautionColor;

		public Color CriticalColor;

		private int goodColorPropertyId;

		private int cautionColorPropertyId;

		private int criticalColorPropertyId;

		public void InitializeShader()
		{
			this.Image.material.SetFloatArray(this.Name, new float[this.ArrayMaxSize]);
			this.averagePropertyId = Shader.PropertyToID("Average");
			this.goodThresholdPropertyId = Shader.PropertyToID("_GoodThreshold");
			this.cautionThresholdPropertyId = Shader.PropertyToID("_CautionThreshold");
			this.goodColorPropertyId = Shader.PropertyToID("_GoodColor");
			this.cautionColorPropertyId = Shader.PropertyToID("_CautionColor");
			this.criticalColorPropertyId = Shader.PropertyToID("_CriticalColor");
		}

		public void UpdateArray()
		{
			this.Image.material.SetInt(this.Name_Length, this.Array.Length);
		}

		public void UpdateAverage()
		{
			this.Image.material.SetFloat(this.averagePropertyId, this.Average);
		}

		public void UpdateThresholds()
		{
			this.Image.material.SetFloat(this.goodThresholdPropertyId, this.GoodThreshold);
			this.Image.material.SetFloat(this.cautionThresholdPropertyId, this.CautionThreshold);
		}

		public void UpdateColors()
		{
			this.Image.material.SetColor(this.goodColorPropertyId, this.GoodColor);
			this.Image.material.SetColor(this.cautionColorPropertyId, this.CautionColor);
			this.Image.material.SetColor(this.criticalColorPropertyId, this.CriticalColor);
		}

		public void UpdatePoints()
		{
			this.Image.material.SetFloatArray(this.Name, this.Array);
		}
	}
}
