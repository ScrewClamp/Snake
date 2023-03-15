using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tayx.Graphy.Audio
{
	public class AudioMonitor : MonoBehaviour
	{
		private const float m_refValue = 1f;

		private GraphyManager m_graphyManager;

		private AudioListener m_audioListener;

		private GraphyManager.LookForAudioListener m_findAudioListenerInCameraIfNull = GraphyManager.LookForAudioListener.ON_SCENE_LOAD;

		private FFTWindow m_FFTWindow = FFTWindow.Blackman;

		private int m_spectrumSize = 512;

		private float[] m_spectrum;

		private float m_maxDB;

		public float[] Spectrum
		{
			get
			{
				return this.m_spectrum;
			}
		}

		public float MaxDB
		{
			get
			{
				return this.m_maxDB;
			}
		}

		public bool SpectrumDataAvailable
		{
			get
			{
				return this.m_audioListener != null;
			}
		}

		private void Awake()
		{
			this.Init();
		}

		private void Update()
		{
			if (this.m_audioListener != null)
			{
				AudioListener.GetOutputData(this.m_spectrum, 0);
				float num = 0f;
				for (int i = 0; i < this.m_spectrum.Length; i++)
				{
					num += this.m_spectrum[i] * this.m_spectrum[i];
				}
				float num2 = Mathf.Sqrt(num / (float)this.m_spectrum.Length);
				this.m_maxDB = 20f * Mathf.Log10(num2 / 1f);
				if (this.m_maxDB < -80f)
				{
					this.m_maxDB = -80f;
				}
				AudioListener.GetSpectrumData(this.m_spectrum, 0, this.m_FFTWindow);
			}
			else if (this.m_audioListener == null && this.m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ALWAYS)
			{
				this.FindAudioListener();
			}
		}

		public void UpdateParameters()
		{
			this.m_findAudioListenerInCameraIfNull = this.m_graphyManager.FindAudioListenerInCameraIfNull;
			this.m_audioListener = this.m_graphyManager.AudioListener;
			this.m_FFTWindow = this.m_graphyManager.FftWindow;
			this.m_spectrumSize = this.m_graphyManager.SpectrumSize;
			if (this.m_audioListener == null && this.m_findAudioListenerInCameraIfNull != GraphyManager.LookForAudioListener.NEVER)
			{
				this.FindAudioListener();
			}
			this.m_spectrum = new float[this.m_spectrumSize];
		}

		public float lin2dB(float linear)
		{
			return Mathf.Clamp(Mathf.Log10(linear) * 20f, -160f, 0f);
		}

		public float dBNormalized(float db)
		{
			return (db + 160f) / 160f;
		}

		private void FindAudioListener()
		{
			this.m_audioListener = Camera.main.GetComponent<AudioListener>();
		}

		private void Init()
		{
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.UpdateParameters();
			SceneManager.sceneLoaded += delegate(Scene scene, LoadSceneMode loadMode)
			{
				if (this.m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ON_SCENE_LOAD)
				{
					this.FindAudioListener();
				}
			};
		}
	}
}
