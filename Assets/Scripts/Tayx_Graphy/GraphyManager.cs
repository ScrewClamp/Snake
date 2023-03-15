using System;
using Tayx.Graphy.Advanced;
using Tayx.Graphy.Audio;
using Tayx.Graphy.Fps;
using Tayx.Graphy.Ram;
using Tayx.Graphy.Utils;
using UnityEngine;

namespace Tayx.Graphy
{
	public class GraphyManager : Singleton<GraphyManager>
	{
		public enum Mode
		{
			FULL,
			LIGHT
		}

		public enum ModuleType
		{
			FPS,
			RAM,
			AUDIO,
			ADVANCED
		}

		public enum ModuleState
		{
			FULL,
			TEXT,
			BASIC,
			BACKGROUND,
			OFF
		}

		public enum ModulePosition
		{
			TOP_RIGHT,
			TOP_LEFT,
			BOTTOM_RIGHT,
			BOTTOM_LEFT
		}

		public enum LookForAudioListener
		{
			ALWAYS,
			ON_SCENE_LOAD,
			NEVER
		}

		private enum ModuleToggleState
		{
			FPS_BASIC,
			FPS_TEXT,
			FPS_FULL,
			FPS_TEXT_RAM_TEXT,
			FPS_FULL_RAM_TEXT,
			FPS_FULL_RAM_FULL,
			FPS_TEXT_RAM_TEXT_AUDIO_TEXT,
			FPS_FULL_RAM_TEXT_AUDIO_TEXT,
			FPS_FULL_RAM_FULL_AUDIO_TEXT,
			FPS_FULL_RAM_FULL_AUDIO_FULL,
			FPS_FULL_RAM_FULL_AUDIO_FULL_ADVANCED_FULL,
			FPS_BASIC_ADVANCED_FULL
		}

		private FpsManager m_fpsManager;

		private RamManager m_ramManager;

		private AudioManager m_audioManager;

		private AdvancedData m_advancedData;

		private FpsMonitor m_fpsMonitor;

		private RamMonitor m_ramMonitor;

		private AudioMonitor m_audioMonitor;

		[SerializeField]
		private GraphyManager.Mode m_graphyMode;

		private GraphyManager.ModuleToggleState m_moduleToggleState = GraphyManager.ModuleToggleState.FPS_BASIC_ADVANCED_FULL;

		private bool m_active = true;

		[SerializeField]
		private bool m_keepAlive = true;

		[SerializeField]
		private bool m_background = true;

		[SerializeField]
		private Color m_backgroundColor = new Color(0f, 0f, 0f, 0.3f);

		[SerializeField]
		private KeyCode m_toggleModeKeyCode = KeyCode.G;

		[SerializeField]
		private bool m_toggleModeCtrl = true;

		[SerializeField]
		private bool m_toggleModeAlt;

		[SerializeField]
		private KeyCode m_toggleActiveKeyCode = KeyCode.H;

		[SerializeField]
		private bool m_toggleActiveCtrl = true;

		[SerializeField]
		private bool m_toggleActiveAlt;

		[SerializeField]
		private GraphyManager.ModulePosition m_graphModulePosition;

		[SerializeField]
		private GraphyManager.ModuleState m_fpsModuleState;

		[Range(0f, 200f), SerializeField, Tooltip("Time (in seconds) to reset the minimum and maximum framerates if they don't change in the specified time. Set to 0 if you don't want it to reset.")]
		private int m_timeToResetMinMaxFps = 10;

		[SerializeField]
		private Color m_goodFpsColor = new Color32(118, 212, 58, 255);

		[SerializeField]
		private int m_goodFpsThreshold = 60;

		[SerializeField]
		private Color m_cautionFpsColor = new Color32(243, 232, 0, 255);

		[SerializeField]
		private int m_cautionFpsThreshold = 30;

		[SerializeField]
		private Color m_criticalFpsColor = new Color32(220, 41, 30, 255);

		[Range(10f, 300f), SerializeField]
		private int m_fpsGraphResolution = 150;

		[Range(1f, 200f), SerializeField]
		private int m_fpsTextUpdateRate = 3;

		[SerializeField]
		private GraphyManager.ModuleState m_ramModuleState;

		[SerializeField]
		private Color m_allocatedRamColor = new Color32(255, 190, 60, 255);

		[SerializeField]
		private Color m_reservedRamColor = new Color32(205, 84, 229, 255);

		[SerializeField]
		private Color m_monoRamColor = new Color(0.3f, 0.65f, 1f, 1f);

		[Range(10f, 300f), SerializeField]
		private int m_ramGraphResolution = 150;

		[Range(1f, 200f), SerializeField]
		private int m_ramTextUpdateRate = 3;

		[SerializeField]
		private GraphyManager.ModuleState m_audioModuleState;

		[SerializeField]
		private GraphyManager.LookForAudioListener m_findAudioListenerInCameraIfNull = GraphyManager.LookForAudioListener.ON_SCENE_LOAD;

		[SerializeField]
		private AudioListener m_audioListener;

		[SerializeField]
		private Color m_audioGraphColor = Color.white;

		[Range(10f, 300f), SerializeField]
		private int m_audioGraphResolution = 81;

		[Range(1f, 200f), SerializeField]
		private int m_audioTextUpdateRate = 3;

		[SerializeField]
		private FFTWindow m_FFTWindow = FFTWindow.Blackman;

		[SerializeField, Tooltip("Must be a power of 2 and between 64-8192")]
		private int m_spectrumSize = 512;

		[SerializeField]
		private GraphyManager.ModulePosition m_advancedModulePosition = GraphyManager.ModulePosition.BOTTOM_LEFT;

		[SerializeField]
		private GraphyManager.ModuleState m_advancedModuleState;

		public GraphyManager.Mode GraphyMode
		{
			get
			{
				return this.m_graphyMode;
			}
			set
			{
				this.m_graphyMode = value;
				this.UpdateAllParameters();
			}
		}

		public bool KeepAlive
		{
			get
			{
				return this.m_keepAlive;
			}
		}

		public bool Background
		{
			get
			{
				return this.m_background;
			}
			set
			{
				this.m_background = value;
				this.UpdateAllParameters();
			}
		}

		public Color BackgroundColor
		{
			get
			{
				return this.m_backgroundColor;
			}
			set
			{
				this.m_backgroundColor = value;
				this.UpdateAllParameters();
			}
		}

		public GraphyManager.ModulePosition GraphModulePosition
		{
			get
			{
				return this.m_graphModulePosition;
			}
			set
			{
				this.m_graphModulePosition = value;
				this.m_fpsManager.SetPosition(this.m_graphModulePosition);
				this.m_ramManager.SetPosition(this.m_graphModulePosition);
				this.m_audioManager.SetPosition(this.m_graphModulePosition);
			}
		}

		public GraphyManager.ModuleState FpsModuleState
		{
			get
			{
				return this.m_fpsModuleState;
			}
			set
			{
				this.m_fpsModuleState = value;
				this.m_fpsManager.SetState(this.m_fpsModuleState);
			}
		}

		public int TimeToResetMinMaxFps
		{
			get
			{
				return this.m_timeToResetMinMaxFps;
			}
			set
			{
				this.m_timeToResetMinMaxFps = value;
				this.m_fpsManager.UpdateParameters();
			}
		}

		public Color GoodFPSColor
		{
			get
			{
				return this.m_goodFpsColor;
			}
			set
			{
				this.m_goodFpsColor = value;
				this.m_fpsManager.UpdateParameters();
			}
		}

		public Color CautionFPSColor
		{
			get
			{
				return this.m_cautionFpsColor;
			}
			set
			{
				this.m_cautionFpsColor = value;
				this.m_fpsManager.UpdateParameters();
			}
		}

		public Color CriticalFPSColor
		{
			get
			{
				return this.m_criticalFpsColor;
			}
			set
			{
				this.m_criticalFpsColor = value;
				this.m_fpsManager.UpdateParameters();
			}
		}

		public int GoodFPSThreshold
		{
			get
			{
				return this.m_goodFpsThreshold;
			}
			set
			{
				this.m_goodFpsThreshold = value;
				this.m_fpsManager.UpdateParameters();
			}
		}

		public int CautionFPSThreshold
		{
			get
			{
				return this.m_cautionFpsThreshold;
			}
			set
			{
				this.m_cautionFpsThreshold = value;
				this.m_fpsManager.UpdateParameters();
			}
		}

		public int FpsGraphResolution
		{
			get
			{
				return this.m_fpsGraphResolution;
			}
			set
			{
				this.m_fpsGraphResolution = value;
				this.m_fpsManager.UpdateParameters();
			}
		}

		public int FpsTextUpdateRate
		{
			get
			{
				return this.m_fpsTextUpdateRate;
			}
			set
			{
				this.m_fpsTextUpdateRate = value;
				this.m_fpsManager.UpdateParameters();
			}
		}

		public float CurrentFPS
		{
			get
			{
				return this.m_fpsMonitor.CurrentFPS;
			}
		}

		public float AverageFPS
		{
			get
			{
				return this.m_fpsMonitor.AverageFPS;
			}
		}

		public float MinFPS
		{
			get
			{
				return this.m_fpsMonitor.MinFPS;
			}
		}

		public float MaxFPS
		{
			get
			{
				return this.m_fpsMonitor.MaxFPS;
			}
		}

		public GraphyManager.ModuleState RamModuleState
		{
			get
			{
				return this.m_ramModuleState;
			}
			set
			{
				this.m_ramModuleState = value;
				this.m_ramManager.SetState(this.m_ramModuleState);
			}
		}

		public Color AllocatedRamColor
		{
			get
			{
				return this.m_allocatedRamColor;
			}
			set
			{
				this.m_allocatedRamColor = value;
				this.m_ramManager.UpdateParameters();
			}
		}

		public Color ReservedRamColor
		{
			get
			{
				return this.m_reservedRamColor;
			}
			set
			{
				this.m_reservedRamColor = value;
				this.m_ramManager.UpdateParameters();
			}
		}

		public Color MonoRamColor
		{
			get
			{
				return this.m_monoRamColor;
			}
			set
			{
				this.m_monoRamColor = value;
				this.m_ramManager.UpdateParameters();
			}
		}

		public int RamGraphResolution
		{
			get
			{
				return this.m_ramGraphResolution;
			}
			set
			{
				this.m_ramGraphResolution = value;
				this.m_ramManager.UpdateParameters();
			}
		}

		public int RamTextUpdateRate
		{
			get
			{
				return this.m_ramTextUpdateRate;
			}
			set
			{
				this.m_ramTextUpdateRate = value;
				this.m_ramManager.UpdateParameters();
			}
		}

		public float AllocatedRam
		{
			get
			{
				return this.m_ramMonitor.AllocatedRam;
			}
		}

		public float ReservedRam
		{
			get
			{
				return this.m_ramMonitor.ReservedRam;
			}
		}

		public float MonoRam
		{
			get
			{
				return this.m_ramMonitor.MonoRam;
			}
		}

		public GraphyManager.ModuleState AudioModuleState
		{
			get
			{
				return this.m_audioModuleState;
			}
			set
			{
				this.m_audioModuleState = value;
				this.m_audioManager.SetState(this.m_audioModuleState);
			}
		}

		public AudioListener AudioListener
		{
			get
			{
				return this.m_audioListener;
			}
			set
			{
				this.m_audioListener = value;
				this.m_audioManager.UpdateParameters();
			}
		}

		public GraphyManager.LookForAudioListener FindAudioListenerInCameraIfNull
		{
			get
			{
				return this.m_findAudioListenerInCameraIfNull;
			}
			set
			{
				this.m_findAudioListenerInCameraIfNull = value;
				this.m_audioManager.UpdateParameters();
			}
		}

		public Color AudioGraphColor
		{
			get
			{
				return this.m_audioGraphColor;
			}
			set
			{
				this.m_audioGraphColor = value;
				this.m_audioManager.UpdateParameters();
			}
		}

		public int AudioGraphResolution
		{
			get
			{
				return this.m_audioGraphResolution;
			}
			set
			{
				this.m_audioGraphResolution = value;
				this.m_audioManager.UpdateParameters();
			}
		}

		public int AudioTextUpdateRate
		{
			get
			{
				return this.m_audioTextUpdateRate;
			}
			set
			{
				this.m_audioTextUpdateRate = value;
				this.m_audioManager.UpdateParameters();
			}
		}

		public FFTWindow FftWindow
		{
			get
			{
				return this.m_FFTWindow;
			}
			set
			{
				this.m_FFTWindow = value;
				this.m_audioManager.UpdateParameters();
			}
		}

		public int SpectrumSize
		{
			get
			{
				return this.m_spectrumSize;
			}
			set
			{
				this.m_spectrumSize = value;
				this.m_audioManager.UpdateParameters();
			}
		}

		public float[] Spectrum
		{
			get
			{
				return this.m_audioMonitor.Spectrum;
			}
		}

		public float MaxDB
		{
			get
			{
				return this.m_audioMonitor.MaxDB;
			}
		}

		public GraphyManager.ModuleState AdvancedModuleState
		{
			get
			{
				return this.m_advancedModuleState;
			}
			set
			{
				this.m_advancedModuleState = value;
				this.m_advancedData.SetState(this.m_advancedModuleState);
			}
		}

		public GraphyManager.ModulePosition AdvancedModulePosition
		{
			get
			{
				return this.m_advancedModulePosition;
			}
			set
			{
				this.m_advancedModulePosition = value;
				this.m_advancedData.SetPosition(this.m_advancedModulePosition);
			}
		}

		protected GraphyManager()
		{
		}

		private void Start()
		{
			this.Init();
		}

		private void Update()
		{
			this.CheckForHotkeyPresses();
		}

		public void SetModulePosition(GraphyManager.ModuleType moduleType, GraphyManager.ModulePosition modulePosition)
		{
			switch (moduleType)
			{
			case GraphyManager.ModuleType.FPS:
			case GraphyManager.ModuleType.RAM:
			case GraphyManager.ModuleType.AUDIO:
				this.m_graphModulePosition = modulePosition;
				this.m_ramManager.SetPosition(modulePosition);
				this.m_fpsManager.SetPosition(modulePosition);
				this.m_audioManager.SetPosition(modulePosition);
				break;
			case GraphyManager.ModuleType.ADVANCED:
				this.m_advancedData.SetPosition(modulePosition);
				break;
			}
		}

		public void SetModuleMode(GraphyManager.ModuleType moduleType, GraphyManager.ModuleState moduleState)
		{
			switch (moduleType)
			{
			case GraphyManager.ModuleType.FPS:
				this.m_fpsManager.SetState(moduleState);
				break;
			case GraphyManager.ModuleType.RAM:
				this.m_ramManager.SetState(moduleState);
				break;
			case GraphyManager.ModuleType.AUDIO:
				this.m_audioManager.SetState(moduleState);
				break;
			case GraphyManager.ModuleType.ADVANCED:
				this.m_advancedData.SetState(moduleState);
				break;
			}
		}

		private void Init()
		{
			if (this.m_keepAlive)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.transform.root.gameObject);
			}
			this.m_fpsMonitor = (base.GetComponentInChildren(typeof(FpsMonitor), true) as FpsMonitor);
			this.m_ramMonitor = (base.GetComponentInChildren(typeof(RamMonitor), true) as RamMonitor);
			this.m_audioMonitor = (base.GetComponentInChildren(typeof(AudioMonitor), true) as AudioMonitor);
			this.m_fpsManager = (base.GetComponentInChildren(typeof(FpsManager), true) as FpsManager);
			this.m_ramManager = (base.GetComponentInChildren(typeof(RamManager), true) as RamManager);
			this.m_audioManager = (base.GetComponentInChildren(typeof(AudioManager), true) as AudioManager);
			this.m_advancedData = (base.GetComponentInChildren(typeof(AdvancedData), true) as AdvancedData);
			this.m_fpsManager.SetPosition(this.m_graphModulePosition);
			this.m_ramManager.SetPosition(this.m_graphModulePosition);
			this.m_audioManager.SetPosition(this.m_graphModulePosition);
			this.m_advancedData.SetPosition(this.m_advancedModulePosition);
			this.m_fpsManager.SetState(this.m_fpsModuleState);
			this.m_ramManager.SetState(this.m_ramModuleState);
			this.m_audioManager.SetState(this.m_audioModuleState);
			this.m_advancedData.SetState(this.m_advancedModuleState);
		}

		private void CheckForHotkeyPresses()
		{
			if (this.m_toggleModeCtrl && this.m_toggleModeAlt)
			{
				if (this.CheckFor3KeyPress(this.m_toggleModeKeyCode, KeyCode.LeftControl, KeyCode.LeftAlt) || this.CheckFor3KeyPress(this.m_toggleModeKeyCode, KeyCode.RightControl, KeyCode.LeftAlt) || this.CheckFor3KeyPress(this.m_toggleModeKeyCode, KeyCode.RightControl, KeyCode.RightAlt) || this.CheckFor3KeyPress(this.m_toggleModeKeyCode, KeyCode.LeftControl, KeyCode.RightAlt))
				{
					this.ToggleModes();
				}
			}
			else if (this.m_toggleModeCtrl)
			{
				if (this.CheckFor2KeyPress(this.m_toggleModeKeyCode, KeyCode.LeftControl) || this.CheckFor2KeyPress(this.m_toggleModeKeyCode, KeyCode.RightControl))
				{
					this.ToggleModes();
				}
			}
			else if (this.m_toggleModeAlt)
			{
				if (this.CheckFor2KeyPress(this.m_toggleModeKeyCode, KeyCode.LeftAlt) || this.CheckFor2KeyPress(this.m_toggleModeKeyCode, KeyCode.RightAlt))
				{
					this.ToggleModes();
				}
			}
			else if (this.CheckFor1KeyPress(this.m_toggleModeKeyCode))
			{
				this.ToggleModes();
			}
			if (this.m_toggleActiveCtrl && this.m_toggleActiveAlt)
			{
				if (this.CheckFor3KeyPress(this.m_toggleActiveKeyCode, KeyCode.LeftControl, KeyCode.LeftAlt) || this.CheckFor3KeyPress(this.m_toggleActiveKeyCode, KeyCode.RightControl, KeyCode.LeftAlt) || this.CheckFor3KeyPress(this.m_toggleActiveKeyCode, KeyCode.RightControl, KeyCode.RightAlt) || this.CheckFor3KeyPress(this.m_toggleActiveKeyCode, KeyCode.LeftControl, KeyCode.RightAlt))
				{
					this.ToggleActive();
				}
			}
			else if (this.m_toggleActiveCtrl)
			{
				if (this.CheckFor2KeyPress(this.m_toggleActiveKeyCode, KeyCode.LeftControl) || this.CheckFor2KeyPress(this.m_toggleActiveKeyCode, KeyCode.RightControl))
				{
					this.ToggleActive();
				}
			}
			else if (this.m_toggleActiveAlt)
			{
				if (this.CheckFor2KeyPress(this.m_toggleActiveKeyCode, KeyCode.LeftAlt) || this.CheckFor2KeyPress(this.m_toggleActiveKeyCode, KeyCode.RightAlt))
				{
					this.ToggleActive();
				}
			}
			else if (this.CheckFor1KeyPress(this.m_toggleActiveKeyCode))
			{
				this.ToggleActive();
			}
		}

		private void ToggleModes()
		{
			if (this.m_moduleToggleState >= (GraphyManager.ModuleToggleState)Enum.GetNames(typeof(GraphyManager.ModuleToggleState)).Length - 1)
			{
				this.m_moduleToggleState = GraphyManager.ModuleToggleState.FPS_BASIC;
			}
			else
			{
				this.m_moduleToggleState++;
			}
			switch (this.m_moduleToggleState)
			{
			case GraphyManager.ModuleToggleState.FPS_BASIC:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.BASIC);
				this.m_ramManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_audioManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_TEXT:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_ramManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_audioManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_FULL:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_ramManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_audioManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_TEXT_RAM_TEXT:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_ramManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_audioManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_FULL_RAM_TEXT:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_ramManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_audioManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_FULL_RAM_FULL:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_ramManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_audioManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_TEXT_RAM_TEXT_AUDIO_TEXT:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_ramManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_audioManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_FULL_RAM_TEXT_AUDIO_TEXT:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_ramManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_audioManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_FULL_RAM_FULL_AUDIO_TEXT:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_ramManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_audioManager.SetState(GraphyManager.ModuleState.TEXT);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_FULL_RAM_FULL_AUDIO_FULL:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_ramManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_audioManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
				break;
			case GraphyManager.ModuleToggleState.FPS_FULL_RAM_FULL_AUDIO_FULL_ADVANCED_FULL:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_ramManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_audioManager.SetState(GraphyManager.ModuleState.FULL);
				this.m_advancedData.SetState(GraphyManager.ModuleState.FULL);
				break;
			case GraphyManager.ModuleToggleState.FPS_BASIC_ADVANCED_FULL:
				this.m_fpsManager.SetState(GraphyManager.ModuleState.BASIC);
				this.m_ramManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_audioManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_advancedData.SetState(GraphyManager.ModuleState.FULL);
				break;
			}
		}

		private void ToggleActive()
		{
			this.m_active = !this.m_active;
			if (this.m_active)
			{
				this.m_fpsManager.RestorePreviousState();
				this.m_ramManager.RestorePreviousState();
				this.m_audioManager.RestorePreviousState();
				this.m_advancedData.RestorePreviousState();
			}
			else
			{
				this.m_fpsManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_ramManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_audioManager.SetState(GraphyManager.ModuleState.OFF);
				this.m_advancedData.SetState(GraphyManager.ModuleState.OFF);
			}
		}

		private bool CheckFor1KeyPress(KeyCode key)
		{
			return UnityEngine.Input.GetKeyDown(key);
		}

		private bool CheckFor2KeyPress(KeyCode key1, KeyCode key2)
		{
			return (UnityEngine.Input.GetKeyDown(key1) && UnityEngine.Input.GetKey(key2)) || (UnityEngine.Input.GetKeyDown(key2) && UnityEngine.Input.GetKey(key1));
		}

		private bool CheckFor3KeyPress(KeyCode key1, KeyCode key2, KeyCode key3)
		{
			return (UnityEngine.Input.GetKeyDown(key1) && UnityEngine.Input.GetKey(key2) && UnityEngine.Input.GetKey(key3)) || (UnityEngine.Input.GetKeyDown(key2) && UnityEngine.Input.GetKey(key1) && UnityEngine.Input.GetKey(key3)) || (UnityEngine.Input.GetKeyDown(key3) && UnityEngine.Input.GetKey(key1) && UnityEngine.Input.GetKey(key2));
		}

		private void UpdateAllParameters()
		{
			this.m_fpsManager.UpdateParameters();
			this.m_ramManager.UpdateParameters();
			this.m_audioManager.UpdateParameters();
			this.m_advancedData.UpdateParameters();
		}
	}
}
