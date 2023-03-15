using System;
using Tayx.Graphy.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Fps
{
	public class FpsGraph : Tayx.Graphy.Graph.Graph
    {
		private GraphyManager m_graphyManager;

		private FpsMonitor m_fpsMonitor;

		[SerializeField]
		private Image m_imageGraph;

		private int m_resolution = 150;

		private ShaderGraph m_shaderGraph;

		public Shader ShaderFull;

		public Shader ShaderLight;

		private int[] m_fpsArray;

		private int m_highestFps;

		private void Awake()
		{
			this.Init();
		}

		private void Update()
		{
			this.UpdateGraph();
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
			this.m_resolution = this.m_graphyManager.FpsGraphResolution;
			this.CreatePoints();
		}

		protected override void UpdateGraph()
		{
			int num = (int)(1f / Time.unscaledDeltaTime);
			int num2 = 0;
			for (int i = 0; i <= this.m_resolution - 1; i++)
			{
				if (i >= this.m_resolution - 1)
				{
					this.m_fpsArray[i] = num;
				}
				else
				{
					this.m_fpsArray[i] = this.m_fpsArray[i + 1];
				}
				if (num2 < this.m_fpsArray[i])
				{
					num2 = this.m_fpsArray[i];
				}
			}
			this.m_highestFps = ((this.m_highestFps >= 1 && this.m_highestFps > num2) ? (this.m_highestFps - 1) : num2);
			for (int j = 0; j <= this.m_resolution - 1; j++)
			{
				this.m_shaderGraph.Array[j] = (float)this.m_fpsArray[j] / (float)this.m_highestFps;
			}
			this.m_shaderGraph.UpdatePoints();
			this.m_shaderGraph.Average = this.m_fpsMonitor.AverageFPS / (float)this.m_highestFps;
			this.m_shaderGraph.UpdateAverage();
			this.m_shaderGraph.GoodThreshold = (float)this.m_graphyManager.GoodFPSThreshold / (float)this.m_highestFps;
			this.m_shaderGraph.CautionThreshold = (float)this.m_graphyManager.CautionFPSThreshold / (float)this.m_highestFps;
			this.m_shaderGraph.UpdateThresholds();
		}

		protected override void CreatePoints()
		{
			this.m_shaderGraph.Array = new float[this.m_resolution];
			this.m_fpsArray = new int[this.m_resolution];
			for (int i = 0; i < this.m_resolution; i++)
			{
				this.m_shaderGraph.Array[i] = 0f;
			}
			this.m_shaderGraph.GoodColor = this.m_graphyManager.GoodFPSColor;
			this.m_shaderGraph.CautionColor = this.m_graphyManager.CautionFPSColor;
			this.m_shaderGraph.CriticalColor = this.m_graphyManager.CriticalFPSColor;
			this.m_shaderGraph.UpdateColors();
			this.m_shaderGraph.UpdateArray();
		}

		private void Init()
		{
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_fpsMonitor = base.GetComponent<FpsMonitor>();
			this.m_shaderGraph = new ShaderGraph();
			this.m_shaderGraph.Image = this.m_imageGraph;
			this.UpdateParameters();
		}
	}
}
