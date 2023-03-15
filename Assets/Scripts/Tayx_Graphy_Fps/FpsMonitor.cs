using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tayx.Graphy.Fps
{
	public class FpsMonitor : MonoBehaviour
	{
		private GraphyManager m_graphyManager;

		private float m_currentFps = -1f;

		private float m_avgFps = -1f;

		private float m_minFps = -1f;

		private float m_maxFps = -1f;

		[SerializeField]
		private int m_averageSamples = 200;

		private List<float> m_averageFpsSamples;

		private int m_timeToResetMinMaxFps = 10;

		private float m_timeToResetMinFpsPassed;

		private float m_timeToResetMaxFpsPassed;

		private float unscaledDeltaTime;

		public float CurrentFPS
		{
			get
			{
				return this.m_currentFps;
			}
		}

		public float AverageFPS
		{
			get
			{
				return this.m_avgFps;
			}
		}

		public float MinFPS
		{
			get
			{
				return this.m_minFps;
			}
		}

		public float MaxFPS
		{
			get
			{
				return this.m_maxFps;
			}
		}

		private void Awake()
		{
			this.Init();
		}

		private void Update()
		{
			this.unscaledDeltaTime = Time.unscaledDeltaTime;
			this.m_timeToResetMinFpsPassed += this.unscaledDeltaTime;
			this.m_timeToResetMaxFpsPassed += this.unscaledDeltaTime;
			this.m_currentFps = 1f / Time.deltaTime;
			this.m_avgFps = 0f;
			if (this.m_averageFpsSamples.Count >= this.m_averageSamples)
			{
				this.m_averageFpsSamples.Add(this.m_currentFps);
				this.m_averageFpsSamples.RemoveAt(0);
			}
			else
			{
				this.m_averageFpsSamples.Add(this.m_currentFps);
			}
			for (int i = 0; i < this.m_averageFpsSamples.Count; i++)
			{
				this.m_avgFps += this.m_averageFpsSamples[i];
			}
			this.m_avgFps /= (float)this.m_averageSamples;
			if (this.m_timeToResetMinMaxFps > 0 && this.m_timeToResetMinFpsPassed > (float)this.m_timeToResetMinMaxFps)
			{
				this.m_minFps = -1f;
				this.m_timeToResetMinFpsPassed = 0f;
			}
			if (this.m_timeToResetMinMaxFps > 0 && this.m_timeToResetMaxFpsPassed > (float)this.m_timeToResetMinMaxFps)
			{
				this.m_maxFps = -1f;
				this.m_timeToResetMaxFpsPassed = 0f;
			}
			if (this.m_currentFps < this.m_minFps || this.m_minFps < 0f)
			{
				this.m_minFps = this.m_currentFps;
				this.m_timeToResetMinFpsPassed = 0f;
			}
			if (this.m_currentFps > this.m_maxFps || this.m_maxFps < 0f)
			{
				this.m_maxFps = this.m_currentFps;
				this.m_timeToResetMaxFpsPassed = 0f;
			}
		}

		public void UpdateParameters()
		{
			this.m_timeToResetMinMaxFps = this.m_graphyManager.TimeToResetMinMaxFps;
		}

		private void Init()
		{
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_averageFpsSamples = new List<float>();
			this.UpdateParameters();
		}
	}
}
