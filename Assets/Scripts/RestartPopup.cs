using System;
using UnityEngine;
using UnityEngine.UI;

public class RestartPopup : MonoBehaviour
{
	public SessionScoreManager sessionScoreManager;

	public Text bestScore;

	public GameObject newBestScoreIcon;

	public GameObject regularBestScoreIcon;

	public UIElementGroup uiElementGroup;

	public UIElementGroup menuButtonsGroup;

	private void Start()
	{
		this.Show();
	}

	public void Show()
	{
		this.bestScore.text = this.sessionScoreManager.SessionBestScore.ToString();
		bool isNewBestScore = this.sessionScoreManager.IsNewBestScore;
		this.newBestScoreIcon.SetActive(isNewBestScore);
		this.regularBestScoreIcon.SetActive(!isNewBestScore);
		this.uiElementGroup.Show();
		this.menuButtonsGroup.Show();
	}

	public void Hide()
	{
		this.uiElementGroup.Hide();
		this.menuButtonsGroup.Hide();
	}
}
