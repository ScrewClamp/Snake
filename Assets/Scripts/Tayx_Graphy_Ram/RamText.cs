using System;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Ram
{
	public class RamText : MonoBehaviour
	{
		private GraphyManager m_graphyManager;

		private RamMonitor m_ramMonitor;

		[SerializeField]
		private Text m_allocatedSystemMemorySizeText;

		[SerializeField]
		private Text m_reservedSystemMemorySizeText;

		[SerializeField]
		private Text m_monoSystemMemorySizeText;

		private float m_updateRate = 4f;

		private float m_deltaTime;

		private readonly string m_memoryStringFormat = "0.0";

		private void Awake()
		{
			this.Init();
		}

		private void Update()
		{
			this.m_deltaTime += Time.unscaledDeltaTime;
			if ((double)this.m_deltaTime > 1.0 / (double)this.m_updateRate)
			{
				this.m_allocatedSystemMemorySizeText.text = this.m_ramMonitor.AllocatedRam.ToStringNonAlloc(this.m_memoryStringFormat);
				this.m_reservedSystemMemorySizeText.text = this.m_ramMonitor.ReservedRam.ToStringNonAlloc(this.m_memoryStringFormat);
				this.m_monoSystemMemorySizeText.text = this.m_ramMonitor.MonoRam.ToStringNonAlloc(this.m_memoryStringFormat);
				this.m_deltaTime = 0f;
			}
		}

		public void UpdateParameters()
		{
			this.m_allocatedSystemMemorySizeText.color = this.m_graphyManager.AllocatedRamColor;
			this.m_reservedSystemMemorySizeText.color = this.m_graphyManager.ReservedRamColor;
			this.m_monoSystemMemorySizeText.color = this.m_graphyManager.MonoRamColor;
			this.m_updateRate = (float)this.m_graphyManager.RamTextUpdateRate;
		}

		private void Init()
		{
			if (!FloatString.Inited || FloatString.minValue > -1000f || FloatString.maxValue < 16384f)
			{
				FloatString.Init(-1001f, 16386f, 1);
			}
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_ramMonitor = base.GetComponent<RamMonitor>();
			this.UpdateParameters();
		}
	}
}
