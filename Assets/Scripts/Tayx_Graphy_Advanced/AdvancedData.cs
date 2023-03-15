using System;
using System.Collections.Generic;
using System.Text;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Advanced
{
	public class AdvancedData : MonoBehaviour, IMovable, IModifiableState
	{
		private GraphyManager m_graphyManager;

		private RectTransform m_rectTransform;

		[SerializeField]
		private List<Image> m_backgroundImages = new List<Image>();

		[SerializeField]
		private Text m_graphicsDeviceVersionText;

		[SerializeField]
		private Text m_processorTypeText;

		[SerializeField]
		private Text m_operatingSystemText;

		[SerializeField]
		private Text m_systemMemoryText;

		[SerializeField]
		private Text m_graphicsDeviceNameText;

		[SerializeField]
		private Text m_graphicsMemorySizeText;

		[SerializeField]
		private Text m_screenResolutionText;

		[SerializeField]
		private Text m_gameWindowResolutionText;

		[Range(1f, 60f), SerializeField]
		private float m_updateRate = 1f;

		private float m_deltaTime;

		private StringBuilder m_sb;

		private GraphyManager.ModuleState m_previousModuleState;

		private GraphyManager.ModuleState m_currentModuleState;

		private readonly string[] m_windowStrings = new string[]
		{
			"Window: ",
			"x",
			"@",
			"Hz",
			"[",
			"dpi]"
		};

		private void Awake()
		{
			this.Init();
		}

		private void Update()
		{
			this.m_deltaTime += Time.unscaledDeltaTime;
			if ((double)this.m_deltaTime > 1.0 / (double)this.m_updateRate)
			{
				this.m_sb.Length = 0;
				this.m_sb.Append(this.m_windowStrings[0]).Append(Screen.width.ToStringNonAlloc()).Append(this.m_windowStrings[1]).Append(Screen.height.ToStringNonAlloc()).Append(this.m_windowStrings[2]).Append(Screen.currentResolution.refreshRate.ToStringNonAlloc()).Append(this.m_windowStrings[3]).Append(this.m_windowStrings[4]).Append(Screen.dpi.ToStringNonAlloc()).Append(this.m_windowStrings[5]);
				this.m_gameWindowResolutionText.text = this.m_sb.ToString();
				this.m_deltaTime = 0f;
			}
		}

		public void SetPosition(GraphyManager.ModulePosition newModulePosition)
		{
			float num = Mathf.Abs(this.m_backgroundImages[0].rectTransform.anchoredPosition.x);
			float num2 = Mathf.Abs(this.m_rectTransform.anchoredPosition.y);
			switch (newModulePosition)
			{
			case GraphyManager.ModulePosition.TOP_RIGHT:
				this.m_rectTransform.anchorMax = Vector2.one;
				this.m_rectTransform.anchorMin = Vector2.up;
				this.m_rectTransform.anchoredPosition = new Vector2(0f, -num2);
				this.m_backgroundImages[0].rectTransform.anchorMax = Vector2.one;
				this.m_backgroundImages[0].rectTransform.anchorMin = Vector2.right;
				this.m_backgroundImages[0].rectTransform.anchoredPosition = new Vector2(-num, 0f);
				break;
			case GraphyManager.ModulePosition.TOP_LEFT:
				this.m_rectTransform.anchorMax = Vector2.one;
				this.m_rectTransform.anchorMin = Vector2.up;
				this.m_rectTransform.anchoredPosition = new Vector2(0f, -num2);
				this.m_backgroundImages[0].rectTransform.anchorMax = Vector2.up;
				this.m_backgroundImages[0].rectTransform.anchorMin = Vector2.zero;
				this.m_backgroundImages[0].rectTransform.anchoredPosition = new Vector2(num, 0f);
				break;
			case GraphyManager.ModulePosition.BOTTOM_RIGHT:
				this.m_rectTransform.anchorMax = Vector2.right;
				this.m_rectTransform.anchorMin = Vector2.zero;
				this.m_rectTransform.anchoredPosition = new Vector2(0f, num2);
				this.m_backgroundImages[0].rectTransform.anchorMax = Vector2.one;
				this.m_backgroundImages[0].rectTransform.anchorMin = Vector2.right;
				this.m_backgroundImages[0].rectTransform.anchoredPosition = new Vector2(-num, 0f);
				break;
			case GraphyManager.ModulePosition.BOTTOM_LEFT:
				this.m_rectTransform.anchorMax = Vector2.right;
				this.m_rectTransform.anchorMin = Vector2.zero;
				this.m_rectTransform.anchoredPosition = new Vector2(0f, num2);
				this.m_backgroundImages[0].rectTransform.anchorMax = Vector2.up;
				this.m_backgroundImages[0].rectTransform.anchorMin = Vector2.zero;
				this.m_backgroundImages[0].rectTransform.anchoredPosition = new Vector2(num, 0f);
				break;
			}
			switch (newModulePosition)
			{
			case GraphyManager.ModulePosition.TOP_RIGHT:
			case GraphyManager.ModulePosition.BOTTOM_RIGHT:
				this.m_processorTypeText.alignment = TextAnchor.UpperRight;
				this.m_systemMemoryText.alignment = TextAnchor.UpperRight;
				this.m_graphicsDeviceNameText.alignment = TextAnchor.UpperRight;
				this.m_graphicsDeviceVersionText.alignment = TextAnchor.UpperRight;
				this.m_graphicsMemorySizeText.alignment = TextAnchor.UpperRight;
				this.m_screenResolutionText.alignment = TextAnchor.UpperRight;
				this.m_gameWindowResolutionText.alignment = TextAnchor.UpperRight;
				this.m_operatingSystemText.alignment = TextAnchor.UpperRight;
				break;
			case GraphyManager.ModulePosition.TOP_LEFT:
			case GraphyManager.ModulePosition.BOTTOM_LEFT:
				this.m_processorTypeText.alignment = TextAnchor.UpperLeft;
				this.m_systemMemoryText.alignment = TextAnchor.UpperLeft;
				this.m_graphicsDeviceNameText.alignment = TextAnchor.UpperLeft;
				this.m_graphicsDeviceVersionText.alignment = TextAnchor.UpperLeft;
				this.m_graphicsMemorySizeText.alignment = TextAnchor.UpperLeft;
				this.m_screenResolutionText.alignment = TextAnchor.UpperLeft;
				this.m_gameWindowResolutionText.alignment = TextAnchor.UpperLeft;
				this.m_operatingSystemText.alignment = TextAnchor.UpperLeft;
				break;
			}
		}

		public void SetState(GraphyManager.ModuleState state)
		{
			this.m_previousModuleState = this.m_currentModuleState;
			this.m_currentModuleState = state;
			bool flag = state == GraphyManager.ModuleState.FULL || state == GraphyManager.ModuleState.TEXT || state == GraphyManager.ModuleState.BASIC;
			base.gameObject.SetActive(flag);
			this.m_backgroundImages.SetAllActive(flag && this.m_graphyManager.Background);
		}

		public void RestorePreviousState()
		{
			this.SetState(this.m_previousModuleState);
		}

		public void UpdateParameters()
		{
			foreach (Image current in this.m_backgroundImages)
			{
				current.color = this.m_graphyManager.BackgroundColor;
			}
			this.SetPosition(this.m_graphyManager.AdvancedModulePosition);
			this.SetState(this.m_graphyManager.AdvancedModuleState);
		}

		private void Init()
		{
			if (!FloatString.Inited || FloatString.minValue > -1000f || FloatString.maxValue < 16384f)
			{
				FloatString.Init(-1001f, 16386f, 1);
			}
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_sb = new StringBuilder();
			this.m_rectTransform = base.GetComponent<RectTransform>();
			this.m_processorTypeText.text = string.Concat(new object[]
			{
				"CPU: ",
				SystemInfo.processorType,
				" [",
				SystemInfo.processorCount,
				" cores]"
			});
			this.m_systemMemoryText.text = "RAM: " + SystemInfo.systemMemorySize + " MB";
			this.m_graphicsDeviceVersionText.text = "Graphics API: " + SystemInfo.graphicsDeviceVersion;
			this.m_graphicsDeviceNameText.text = "GPU: " + SystemInfo.graphicsDeviceName;
			this.m_graphicsMemorySizeText.text = string.Concat(new object[]
			{
				"VRAM: ",
				SystemInfo.graphicsMemorySize,
				"MB. Max texture size: ",
				SystemInfo.maxTextureSize,
				"px. Shader level: ",
				SystemInfo.graphicsShaderLevel
			});
			Resolution currentResolution = Screen.currentResolution;
			this.m_screenResolutionText.text = string.Concat(new object[]
			{
				"Screen: ",
				currentResolution.width,
				"x",
				currentResolution.height,
				"@",
				currentResolution.refreshRate,
				"Hz"
			});
			this.m_operatingSystemText.text = string.Concat(new object[]
			{
				"OS: ",
				SystemInfo.operatingSystem,
				" [",
				SystemInfo.deviceType,
				"]"
			});
			float num = 0f;
			List<Text> list = new List<Text>
			{
				this.m_graphicsDeviceVersionText,
				this.m_processorTypeText,
				this.m_systemMemoryText,
				this.m_graphicsDeviceNameText,
				this.m_graphicsMemorySizeText,
				this.m_screenResolutionText,
				this.m_gameWindowResolutionText,
				this.m_operatingSystemText
			};
			foreach (Text current in list)
			{
				if (current.preferredWidth > num)
				{
					num = current.preferredWidth;
				}
			}
			this.m_backgroundImages[0].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num + 10f);
			this.m_backgroundImages[0].rectTransform.anchoredPosition = new Vector2((num + 15f) / 2f * Mathf.Sign(this.m_backgroundImages[0].rectTransform.anchoredPosition.x), this.m_backgroundImages[0].rectTransform.anchoredPosition.y);
			this.UpdateParameters();
		}
	}
}
