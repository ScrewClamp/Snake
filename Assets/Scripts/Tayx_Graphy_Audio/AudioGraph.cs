using System;
using Tayx.Graphy.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Audio
{
	public class AudioGraph : Tayx.Graphy.Graph.Graph
    {
		private GraphyManager m_graphyManager;

		private AudioMonitor m_audioMonitor;

		[SerializeField]
		private Image m_imageGraph;

		private int m_resolution = 40;

		private ShaderGraph m_shaderGraph;

		public Shader ShaderFull;

		public Shader ShaderLight;

		private float[] m_graphArray;

		private void Awake()
		{
			this.Init();
		}

		private void Update()
		{
			if (this.m_audioMonitor.SpectrumDataAvailable)
			{
				this.UpdateGraph();
			}
		}

		public void UpdateParameters()
		{
			GraphyManager.Mode graphyMode = this.m_graphyManager.GraphyMode;
			if (graphyMode != GraphyManager.Mode.FULL)
			{
				if (graphyMode == GraphyManager.Mode.LIGHT)
				{
					this.m_shaderGraph.ArrayMaxSize = 128;
					this.m_shaderGraph.Image.material = new Material(this.ShaderLight);
				}
			}
			else
			{
				this.m_shaderGraph.ArrayMaxSize = 512;
				this.m_shaderGraph.Image.material = new Material(this.ShaderFull);
			}
			this.m_shaderGraph.InitializeShader();
			this.m_resolution = this.m_graphyManager.AudioGraphResolution;
			this.CreatePoints();
		}

		protected override void UpdateGraph()
		{
			int num = Mathf.FloorToInt((float)this.m_audioMonitor.Spectrum.Length / (float)this.m_resolution);
			for (int i = 0; i <= this.m_resolution - 1; i++)
			{
				float num2 = 0f;
				for (int j = 0; j < num; j++)
				{
					num2 += this.m_audioMonitor.Spectrum[i * num + j];
				}
				if ((i + 1) % 3 == 0 && i > 1)
				{
					float num3 = (this.m_audioMonitor.dBNormalized(this.m_audioMonitor.lin2dB(num2 / (float)num)) + this.m_graphArray[i - 1] + this.m_graphArray[i - 2]) / 3f;
					this.m_graphArray[i] = num3;
					this.m_graphArray[i - 1] = num3;
					this.m_graphArray[i - 2] = -1f;
				}
				else
				{
					this.m_graphArray[i] = this.m_audioMonitor.dBNormalized(this.m_audioMonitor.lin2dB(num2 / (float)num));
				}
			}
			for (int k = 0; k <= this.m_resolution - 1; k++)
			{
				this.m_shaderGraph.Array[k] = this.m_graphArray[k];
			}
			this.m_shaderGraph.UpdatePoints();
		}

		protected override void CreatePoints()
		{
			this.m_shaderGraph.Array = new float[this.m_resolution];
			this.m_graphArray = new float[this.m_resolution];
			for (int i = 0; i < this.m_resolution; i++)
			{
				this.m_shaderGraph.Array[i] = 0f;
			}
			this.m_shaderGraph.GoodColor = this.m_graphyManager.AudioGraphColor;
			this.m_shaderGraph.CautionColor = this.m_graphyManager.AudioGraphColor;
			this.m_shaderGraph.CriticalColor = this.m_graphyManager.AudioGraphColor;
			this.m_shaderGraph.UpdateColors();
			this.m_shaderGraph.GoodThreshold = 0f;
			this.m_shaderGraph.CautionThreshold = 0f;
			this.m_shaderGraph.UpdateThresholds();
			this.m_shaderGraph.UpdateArray();
			this.m_shaderGraph.Average = 0f;
			this.m_shaderGraph.UpdateAverage();
		}

		private void Init()
		{
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_audioMonitor = base.GetComponent<AudioMonitor>();
			this.m_shaderGraph = new ShaderGraph();
			this.m_shaderGraph.Image = this.m_imageGraph;
			this.UpdateParameters();
		}
	}
}
