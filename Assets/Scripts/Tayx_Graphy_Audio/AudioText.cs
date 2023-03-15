using System;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Audio
{
	public class AudioText : MonoBehaviour
	{
		private GraphyManager m_graphyManager;

		private AudioMonitor m_audioMonitor;

		[SerializeField]
		private Text m_DBText;

		private int m_updateRate = 4;

		private float m_deltaTimeOffset;

		private void Awake()
		{
			this.Init();
		}

		private void Update()
		{
			if (this.m_audioMonitor.SpectrumDataAvailable)
			{
				if (this.m_deltaTimeOffset > 1f / (float)this.m_updateRate)
				{
					this.m_deltaTimeOffset = 0f;
					this.m_DBText.text = Mathf.Clamp(this.m_audioMonitor.MaxDB, -80f, 0f).ToStringNonAlloc();
				}
				else
				{
					this.m_deltaTimeOffset += Time.deltaTime;
				}
			}
		}

		public void UpdateParameters()
		{
			this.m_updateRate = this.m_graphyManager.AudioTextUpdateRate;
		}

		private void Init()
		{
			if (!FloatString.Inited || FloatString.minValue > -1000f || FloatString.maxValue < 16384f)
			{
				FloatString.Init(-1001f, 16386f, 1);
			}
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_audioMonitor = base.GetComponent<AudioMonitor>();
			this.UpdateParameters();
		}
	}
}
