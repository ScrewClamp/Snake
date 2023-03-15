using System;
using UnityEngine;
using UnityEngine.Events;

public class SecondChanceManager : MonoBehaviour
{
	public GameState gameState;

	public SecondChancePopup secondChancePopup;

	public ScoreComponent playerScoreComponent;

	public PlayerController playerController;

	public PlayerLiveCalculator playerLiveCalculator;

	public ResetPlayer resetPlayer;

	public AdsManager _adsManager;

	public ScoreSystem scoreSystem;

	public LevelManager levelManager;

	public int thresholdPercentage = 60;

	public bool isLevelUp;

	private bool _isSecondChanceUsed;

	private void Start()
	{
		this.gameState.OnRequestGameOverEvent.AddListener(new UnityAction(this.OnRequestGameOver));
		this.levelManager.LevelUpEvent += new Action<int>(this.OnLevelUp);
	}

	private void OnLevelUp(int obj)
	{
		this.isLevelUp = true;
	}

	private void OnRequestGameOver()
	{
		bool flag = this.IsVideoAdAvailable();
		bool flag2 = this.scoreSystem.GetCompletedPercente() > this.thresholdPercentage || this.isLevelUp;
		if (!this._isSecondChanceUsed && flag && flag2)
		{
			this.ShowSecondChancePopup();
		}
		else
		{
			this._isSecondChanceUsed = false;
			this.GameOver();
		}
	}

	private void GameOver()
	{
		this.isLevelUp = false;
		this.gameState.currentState = GameState.State.GameOver;
	}

	private void ShowSecondChancePopup()
	{
		Time.timeScale = 0f;
		this.playerController.isTouchOn = false;
		this.secondChancePopup.Show();
		this.secondChancePopup.WatchVideoClickedEvent += new Action(this.OnWatchVideoClicked);
		this.secondChancePopup.BackClickedEvent += new Action(this.OnBackClicked);
	}

	private void OnBackClicked()
	{
		this.secondChancePopup.WatchVideoClickedEvent -= new Action(this.OnWatchVideoClicked);
		this.secondChancePopup.BackClickedEvent -= new Action(this.OnBackClicked);
		this.secondChancePopup.Hide();
		this.secondChancePopup.HideFinishedEvent += new Action(this.OnHideFinished);
	}

	private void OnHideFinished()
	{
		this.secondChancePopup.HideFinishedEvent -= new Action(this.OnHideFinished);
		Time.timeScale = 1f;
		this.playerController.isTouchOn = true;
		this.GameOver();
	}

	private void OnWatchVideoClicked()
	{
		this.secondChancePopup.WatchVideoClickedEvent -= new Action(this.OnWatchVideoClicked);
		this.secondChancePopup.BackClickedEvent -= new Action(this.OnBackClicked);
		this._adsManager.RewardedVideoFinishedEvent += new Action<bool>(this.OnRewardedVideoFinished);
		this._adsManager.ShowRewardedVideo();
	}

	private void OnRewardedVideoFinished(bool isSiccess)
	{
		this._adsManager.RewardedVideoFinishedEvent -= new Action<bool>(this.OnRewardedVideoFinished);
		this.secondChancePopup.WatchVideoClickedEvent -= new Action(this.OnWatchVideoClicked);
		this.secondChancePopup.BackClickedEvent -= new Action(this.OnBackClicked);
		this.secondChancePopup.Hide();
		Time.timeScale = 1f;
		this.playerController.isTouchOn = true;
		this.playerController.ResetTapCount();
		this._isSecondChanceUsed = true;
		if (isSiccess)
		{
			int num = this.playerScoreComponent.GetScore() + 4;
			this.playerScoreComponent.SetScore(num);
			this.playerLiveCalculator.onPlayerLiveCountChanged.Invoke(num);
			this.resetPlayer.ResetPlayerPositionOnSecondChance();
		}
		else
		{
			this.GameOver();
		}
	}

	private bool IsVideoAdAvailable()
	{
		return this._adsManager.IsAnyRewardedVideoAvailable();
	}
}
