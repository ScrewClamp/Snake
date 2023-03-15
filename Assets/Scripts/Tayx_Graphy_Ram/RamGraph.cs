using System;
using Tayx.Graphy.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Ram
{
	public class RamGraph : Tayx.Graphy.Graph.Graph
    {
		private GraphyManager m_graphyManager;

		private RamMonitor m_ramMonitor;

		[SerializeField]
		private Image m_imageAllocated;

		[SerializeField]
		private Image m_imageReserved;

		[SerializeField]
		private Image m_imageMono;

		private int m_resolution = 150;

		private ShaderGraph m_shaderGraphAllocated;

		private ShaderGraph m_shaderGraphReserved;

		private ShaderGraph m_shaderGraphMono;

		public Shader ShaderFull;

		public Shader ShaderLight;

		private float[] m_allocatedArray;

		private float[] m_reservedArray;

		private float[] m_monoArray;

		private float m_highestMemory;

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
					this.m_shaderGraphAllocated.ArrayMaxSize = 128;
					this.m_shaderGraphReserved.ArrayMaxSize = 128;
					this.m_shaderGraphMono.ArrayMaxSize = 128;
					this.m_shaderGraphAllocated.Image.material = new Material(this.ShaderLight);
					this.m_shaderGraphReserved.Image.material = new Material(this.ShaderLight);
					this.m_shaderGraphMono.Image.material = new Material(this.ShaderLight);
				}
			}
			else
			{
				this.m_shaderGraphAllocated.ArrayMaxSize = 512;
				this.m_shaderGraphReserved.ArrayMaxSize = 512;
				this.m_shaderGraphMono.ArrayMaxSize = 512;
				this.m_shaderGraphAllocated.Image.material = new Material(this.ShaderFull);
				this.m_shaderGraphReserved.Image.material = new Material(this.ShaderFull);
				this.m_shaderGraphMono.Image.material = new Material(this.ShaderFull);
			}
			this.m_shaderGraphAllocated.InitializeShader();
			this.m_shaderGraphReserved.InitializeShader();
			this.m_shaderGraphMono.InitializeShader();
			this.m_resolution = this.m_graphyManager.RamGraphResolution;
			this.CreatePoints();
		}

		protected override void UpdateGraph()
		{
			float allocatedRam = this.m_ramMonitor.AllocatedRam;
			float reservedRam = this.m_ramMonitor.ReservedRam;
			float monoRam = this.m_ramMonitor.MonoRam;
			this.m_highestMemory = 0f;
			for (int i = 0; i <= this.m_resolution - 1; i++)
			{
				if (i >= this.m_resolution - 1)
				{
					this.m_allocatedArray[i] = allocatedRam;
					this.m_reservedArray[i] = reservedRam;
					this.m_monoArray[i] = monoRam;
				}
				else
				{
					this.m_allocatedArray[i] = this.m_allocatedArray[i + 1];
					this.m_reservedArray[i] = this.m_reservedArray[i + 1];
					this.m_monoArray[i] = this.m_monoArray[i + 1];
				}
				if (this.m_highestMemory < this.m_reservedArray[i])
				{
					this.m_highestMemory = this.m_reservedArray[i];
				}
			}
			for (int j = 0; j <= this.m_resolution - 1; j++)
			{
				this.m_shaderGraphAllocated.Array[j] = this.m_allocatedArray[j] / this.m_highestMemory;
				this.m_shaderGraphReserved.Array[j] = this.m_reservedArray[j] / this.m_highestMemory;
				this.m_shaderGraphMono.Array[j] = this.m_monoArray[j] / this.m_highestMemory;
			}
			this.m_shaderGraphAllocated.UpdatePoints();
			this.m_shaderGraphReserved.UpdatePoints();
			this.m_shaderGraphMono.UpdatePoints();
		}

		protected override void CreatePoints()
		{
			this.m_shaderGraphAllocated.Array = new float[this.m_resolution];
			this.m_shaderGraphReserved.Array = new float[this.m_resolution];
			this.m_shaderGraphMono.Array = new float[this.m_resolution];
			this.m_allocatedArray = new float[this.m_resolution];
			this.m_reservedArray = new float[this.m_resolution];
			this.m_monoArray = new float[this.m_resolution];
			for (int i = 0; i < this.m_resolution; i++)
			{
				this.m_shaderGraphAllocated.Array[i] = 0f;
				this.m_shaderGraphReserved.Array[i] = 0f;
				this.m_shaderGraphMono.Array[i] = 0f;
			}
			this.m_shaderGraphAllocated.GoodColor = this.m_graphyManager.AllocatedRamColor;
			this.m_shaderGraphAllocated.CautionColor = this.m_graphyManager.AllocatedRamColor;
			this.m_shaderGraphAllocated.CriticalColor = this.m_graphyManager.AllocatedRamColor;
			this.m_shaderGraphAllocated.UpdateColors();
			this.m_shaderGraphReserved.GoodColor = this.m_graphyManager.ReservedRamColor;
			this.m_shaderGraphReserved.CautionColor = this.m_graphyManager.ReservedRamColor;
			this.m_shaderGraphReserved.CriticalColor = this.m_graphyManager.ReservedRamColor;
			this.m_shaderGraphReserved.UpdateColors();
			this.m_shaderGraphMono.GoodColor = this.m_graphyManager.MonoRamColor;
			this.m_shaderGraphMono.CautionColor = this.m_graphyManager.MonoRamColor;
			this.m_shaderGraphMono.CriticalColor = this.m_graphyManager.MonoRamColor;
			this.m_shaderGraphMono.UpdateColors();
			this.m_shaderGraphAllocated.GoodThreshold = 0f;
			this.m_shaderGraphAllocated.CautionThreshold = 0f;
			this.m_shaderGraphAllocated.UpdateThresholds();
			this.m_shaderGraphReserved.GoodThreshold = 0f;
			this.m_shaderGraphReserved.CautionThreshold = 0f;
			this.m_shaderGraphReserved.UpdateThresholds();
			this.m_shaderGraphMono.GoodThreshold = 0f;
			this.m_shaderGraphMono.CautionThreshold = 0f;
			this.m_shaderGraphMono.UpdateThresholds();
			this.m_shaderGraphAllocated.UpdateArray();
			this.m_shaderGraphReserved.UpdateArray();
			this.m_shaderGraphMono.UpdateArray();
			this.m_shaderGraphAllocated.Average = 0f;
			this.m_shaderGraphReserved.Average = 0f;
			this.m_shaderGraphMono.Average = 0f;
			this.m_shaderGraphAllocated.UpdateAverage();
			this.m_shaderGraphReserved.UpdateAverage();
			this.m_shaderGraphMono.UpdateAverage();
		}

		private void Init()
		{
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_ramMonitor = base.GetComponent<RamMonitor>();
			this.m_shaderGraphAllocated = new ShaderGraph();
			this.m_shaderGraphReserved = new ShaderGraph();
			this.m_shaderGraphMono = new ShaderGraph();
			this.m_shaderGraphAllocated.Image = this.m_imageAllocated;
			this.m_shaderGraphReserved.Image = this.m_imageReserved;
			this.m_shaderGraphMono.Image = this.m_imageMono;
			this.UpdateParameters();
		}
	}
}
