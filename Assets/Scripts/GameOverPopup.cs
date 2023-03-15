using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{


	public ScoreSystem scoreSystem;

	[SerializeField]
	private SessionScoreManager _sessionScoreManager;

	[SerializeField]
	private SoundManager _soundManager;

	[SerializeField]
	private VibrationManager _vibrationManager;

	[SerializeField]
	private Text _completedPercente;

	[SerializeField]
	private Text _currentScore;

	[SerializeField]
	private Text _bestScore;

	[SerializeField]
	private GameObject _regularBestScoreIcon;

	[SerializeField]
	private GameObject _newBestScoreIcon;

	[SerializeField]
	private UIElementGroup _uiElementGroup;

	[SerializeField]
	private UIElementGroup menuButtons;

	public event Action OnClickEvent;

	private void Awake()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	private void OnClick()
	{
		if (this.OnClickEvent != null)
		{
			this.OnClickEvent();
		}
	}

	private void OnEnable()
	{
		this._soundManager.PlayGameOver();
		this._vibrationManager.VibrateOnGameOver();
		this._currentScore.text = this._sessionScoreManager.SessionScore.ToString();
		this._bestScore.text = this._sessionScoreManager.SessionBestScore.ToString();
		this._completedPercente.text = string.Format("{0}% COMPLETED", this.scoreSystem.GetCompletedPercente());
		this.UpdateIcon();
	}

	private void UpdateIcon()
	{
		bool isNewBestScore = this._sessionScoreManager.IsNewBestScore;
		this._regularBestScoreIcon.SetActive(!isNewBestScore);
		this._newBestScoreIcon.SetActive(isNewBestScore);
	}

	public void Show()
	{
		this._uiElementGroup.Show();
		this.menuButtons.Show();
	}

	public void Hide()
	{
		this._uiElementGroup.Hide();
		this.menuButtons.Hide();
	}
}
