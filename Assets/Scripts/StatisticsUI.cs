using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatisticsUI : MonoBehaviour
{
	public Button showStatisticsButton;

	public Button showFPS;

	public GameObject fpsPanel;

	public Text buttonText;

	public GameObject textPanel;

	public Text statisticsText;

	private float _currentTimeScale;

	private void Start()
	{
		this.showStatisticsButton.onClick.AddListener(new UnityAction(this.OnToggleButtonClick));
		this.showFPS.onClick.AddListener(new UnityAction(this.OnShowFPSClick));
		this._currentTimeScale = Time.timeScale;
		this.UpdateButtonText();
	}

	private void OnShowFPSClick()
	{
		this.fpsPanel.SetActive(!this.fpsPanel.activeSelf);
	}

	private void OnToggleButtonClick()
	{
		if (this.textPanel.activeSelf)
		{
			this.textPanel.SetActive(false);
			Time.timeScale = this._currentTimeScale;
		}
		else
		{
			this.textPanel.SetActive(true);
			this._currentTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}
		this.UpdateButtonText();
	}

	private void UpdateButtonText()
	{
		this.buttonText.text = ((!this.textPanel.activeSelf) ? "Show Statistics" : "Hide Statistics");
	}

	public void SwitchVisibility()
	{
		base.gameObject.SetActive(!base.gameObject.activeSelf);
	}
}
