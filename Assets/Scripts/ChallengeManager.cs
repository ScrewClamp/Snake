using System;
using UnityEngine;
using UnityEngine.Events;

public class ChallengeManager : MonoBehaviour
{
	public Action<int> OnPlayedGamesChanged;

	public Action<int> OnCompletedLevel;

	private int _playedGames;

	private int _completedLevels;

	private int _scoreInLevel;

	private int _perfectTransitionCount;

	private int _playedDaysInARow;

	[SerializeField]
	private GameState _gameState;

	[SerializeField]
	private LevelManager _levelManager;

	private void Awake()
	{
		this.ConfigurePlayedGames();
		this.ConfigureCompletedLevels();
		this._scoreInLevel = ((!PlayerPrefs.HasKey("scoreInLevel")) ? 0 : PlayerPrefs.GetInt("scoreInLevel"));
		this._perfectTransitionCount = ((!PlayerPrefs.HasKey("perfectTransitionCount")) ? 0 : PlayerPrefs.GetInt("perfectTransitionCount"));
		this._playedDaysInARow = ((!PlayerPrefs.HasKey("playedDaysInARow")) ? 0 : PlayerPrefs.GetInt("playedDaysInARow"));
	}

	private void ConfigurePlayedGames()
	{
		this._playedGames = PlayerPrefs.GetInt("playedGames", 0);
		this._gameState.OnGameStartedEvent.AddListener(new UnityAction(this.IncrementPlayedGames));
		this._levelManager.LevelUpEvent += new Action<int>(this.IncrementPlayedGamesOnLevelUp);
	}

	private void ConfigureCompletedLevels()
	{
		this._completedLevels = this._levelManager.CurrentLevelIndex;
		this._levelManager.LevelUpEvent += new Action<int>(this.UpdateCompletedLevels);
	}

	private void IncrementPlayedGamesOnLevelUp(int newLevel)
	{
		this.IncrementPlayedGames();
	}

	private void IncrementPlayedGames()
	{
		this._playedGames++;
		this.DispatchPlayedGames();
	}

	private void DispatchPlayedGames()
	{
		if (this.OnPlayedGamesChanged != null)
		{
			this.OnPlayedGamesChanged(this._playedGames);
		}
	}

	private void UpdateCompletedLevels(int newLevel)
	{
		this._completedLevels = newLevel;
		this.DispatchCompletedLevel(newLevel);
	}

	private void DispatchCompletedLevel(int newLevel)
	{
		if (this.OnCompletedLevel != null)
		{
			this.OnCompletedLevel(newLevel);
		}
	}

	public int GetPlayedGames()
	{
		return this._playedGames;
	}

	public int GetCompletedLevels()
	{
		return this._completedLevels;
	}

	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("playedGames", this._playedGames);
		PlayerPrefs.SetInt("scoreInLevel", this._scoreInLevel);
		PlayerPrefs.SetInt("perfectTransitionCount", this._perfectTransitionCount);
		PlayerPrefs.SetInt("playedDaysInARow", this._playedDaysInARow);
	}

	private void OnApplicationPause()
	{
		PlayerPrefs.SetInt("playedGames", this._playedGames);
		PlayerPrefs.SetInt("scoreInLevel", this._scoreInLevel);
		PlayerPrefs.SetInt("perfectTransitionCount", this._perfectTransitionCount);
		PlayerPrefs.SetInt("playedDaysInARow", this._playedDaysInARow);
	}
}
