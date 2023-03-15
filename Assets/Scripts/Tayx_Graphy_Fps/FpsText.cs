using System;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Fps
{
	public class FpsText : MonoBehaviour
	{
		private GraphyManager m_graphyManager;

		private FpsMonitor m_fpsMonitor;

		[SerializeField]
		private Text m_fpsText;

		[SerializeField]
		private Text m_msText;

		[SerializeField]
		private Text m_avgFpsText;

		[SerializeField]
		private Text m_minFpsText;

		[SerializeField]
		private Text m_maxFpsText;

		private int m_updateRate = 4;

		private int m_frameCount;

		private float m_deltaTime;

		private float m_fps;

		private const string m_msStringFormat = "0.0";

		private void Awake()
		{
			this.Init();
		}

		private void Update()
		{
			this.m_deltaTime += Time.unscaledDeltaTime;
			this.m_frameCount++;
			if ((double)this.m_deltaTime > 1.0 / (double)this.m_updateRate)
			{
				this.m_fps = (float)this.m_frameCount / this.m_deltaTime;
				this.m_fpsText.text = this.m_fps.ToInt().ToStringNonAlloc();
				this.m_msText.text = (this.m_deltaTime / (float)this.m_frameCount * 1000f).ToStringNonAlloc("0.0");
				this.m_minFpsText.text = this.m_fpsMonitor.MinFPS.ToInt().ToStringNonAlloc();
				this.SetFpsRelatedTextColor(this.m_minFpsText, this.m_fpsMonitor.MinFPS);
				this.m_maxFpsText.text = this.m_fpsMonitor.MaxFPS.ToInt().ToStringNonAlloc();
				this.SetFpsRelatedTextColor(this.m_maxFpsText, this.m_fpsMonitor.MaxFPS);
				this.m_avgFpsText.text = this.m_fpsMonitor.AverageFPS.ToInt().ToStringNonAlloc();
				this.SetFpsRelatedTextColor(this.m_avgFpsText, this.m_fpsMonitor.AverageFPS);
				this.m_deltaTime = 0f;
				this.m_frameCount = 0;
			}
		}

		public void UpdateParameters()
		{
			this.m_updateRate = this.m_graphyManager.FpsTextUpdateRate;
		}

		private void SetFpsRelatedTextColor(Text text, float fps)
		{
			if (fps > (float)this.m_graphyManager.GoodFPSThreshold)
			{
				text.color = this.m_graphyManager.GoodFPSColor;
			}
			else if (fps > (float)this.m_graphyManager.CautionFPSThreshold)
			{
				text.color = this.m_graphyManager.CautionFPSColor;
			}
			else
			{
				text.color = this.m_graphyManager.CriticalFPSColor;
			}
		}

		private void Init()
		{
			if (!FloatString.Inited || FloatString.minValue > -1000f || FloatString.maxValue < 16384f)
			{
				FloatString.Init(-1001f, 16386f, 1);
			}
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_fpsMonitor = base.GetComponent<FpsMonitor>();
			this.UpdateParameters();
		}
	}
}
