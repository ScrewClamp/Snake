using System;
using System.Collections;
using System.Collections.Generic;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Ram
{
	public class RamManager : MonoBehaviour, IMovable, IModifiableState
	{
		private GraphyManager m_graphyManager;

		private RamGraph m_ramGraph;

		private RamText m_ramText;

		private RectTransform m_rectTransform;

		[SerializeField]
		private GameObject m_ramGraphGameObject;

		private List<GameObject> m_childrenGameObjects = new List<GameObject>();

		[SerializeField]
		private List<Image> m_backgroundImages = new List<Image>();

		private GraphyManager.ModuleState m_previousModuleState;

		private GraphyManager.ModuleState m_currentModuleState;

		private void Awake()
		{
			this.Init();
		}

		private void Start()
		{
			this.UpdateParameters();
		}

		public void SetPosition(GraphyManager.ModulePosition newModulePosition)
		{
			float num = Mathf.Abs(this.m_rectTransform.anchoredPosition.x);
			float num2 = Mathf.Abs(this.m_rectTransform.anchoredPosition.y);
			switch (newModulePosition)
			{
			case GraphyManager.ModulePosition.TOP_RIGHT:
				this.m_rectTransform.anchorMax = Vector2.one;
				this.m_rectTransform.anchorMin = Vector2.one;
				this.m_rectTransform.anchoredPosition = new Vector2(-num, -num2);
				break;
			case GraphyManager.ModulePosition.TOP_LEFT:
				this.m_rectTransform.anchorMax = Vector2.up;
				this.m_rectTransform.anchorMin = Vector2.up;
				this.m_rectTransform.anchoredPosition = new Vector2(num, -num2);
				break;
			case GraphyManager.ModulePosition.BOTTOM_RIGHT:
				this.m_rectTransform.anchorMax = Vector2.right;
				this.m_rectTransform.anchorMin = Vector2.right;
				this.m_rectTransform.anchoredPosition = new Vector2(-num, num2);
				break;
			case GraphyManager.ModulePosition.BOTTOM_LEFT:
				this.m_rectTransform.anchorMax = Vector2.zero;
				this.m_rectTransform.anchorMin = Vector2.zero;
				this.m_rectTransform.anchoredPosition = new Vector2(num, num2);
				break;
			}
		}

		public void SetState(GraphyManager.ModuleState state)
		{
			this.m_previousModuleState = this.m_currentModuleState;
			this.m_currentModuleState = state;
			switch (state)
			{
			case GraphyManager.ModuleState.FULL:
				base.gameObject.SetActive(true);
				this.m_childrenGameObjects.SetAllActive(true);
				this.SetGraphActive(true);
				if (this.m_graphyManager.Background)
				{
					this.m_backgroundImages.SetOneActive(0);
				}
				else
				{
					this.m_backgroundImages.SetAllActive(false);
				}
				break;
			case GraphyManager.ModuleState.TEXT:
			case GraphyManager.ModuleState.BASIC:
				base.gameObject.SetActive(true);
				this.m_childrenGameObjects.SetAllActive(true);
				this.SetGraphActive(false);
				if (this.m_graphyManager.Background)
				{
					this.m_backgroundImages.SetOneActive(1);
				}
				else
				{
					this.m_backgroundImages.SetAllActive(false);
				}
				break;
			case GraphyManager.ModuleState.BACKGROUND:
				base.gameObject.SetActive(true);
				this.SetGraphActive(false);
				this.m_childrenGameObjects.SetAllActive(false);
				this.m_backgroundImages.SetAllActive(false);
				break;
			case GraphyManager.ModuleState.OFF:
				base.gameObject.SetActive(false);
				break;
			}
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
			this.m_ramGraph.UpdateParameters();
			this.m_ramText.UpdateParameters();
			this.SetState(this.m_graphyManager.RamModuleState);
		}

		private void Init()
		{
			this.m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
			this.m_ramGraph = base.GetComponent<RamGraph>();
			this.m_ramText = base.GetComponent<RamText>();
			this.m_rectTransform = base.GetComponent<RectTransform>();
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					if (transform.parent == base.transform)
					{
						this.m_childrenGameObjects.Add(transform.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		private void SetGraphActive(bool active)
		{
			this.m_ramGraph.enabled = active;
			this.m_ramGraphGameObject.SetActive(active);
		}
	}
}
