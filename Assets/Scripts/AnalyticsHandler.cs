using System;
using UnityEngine;
using UnityEngine.Events;

public class AnalyticsHandler : MonoBehaviour
{
	public GameState gameState;

	public LevelManager levelManager;

	private string _progressionString = string.Empty;

	private void Awake()
	{
		this.gameState.OnGameStartedEvent.AddListener(new UnityAction(this.OnGameStarted));
		this.gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
		this.levelManager.LevelUpEvent += new Action<int>(this.OnLevelUp);
		this.InitializeProgressionString();
	}

	private void InitializeProgressionString()
	{
		this._progressionString = "Level_" + (this.levelManager.CurrentLevelIndex + 1);
	}

	private void OnGameStarted()
	{
	}

	private void OnLevelUp(int nextLevelIndex)
	{
		this.InitializeProgressionString();
		this.OnGameStarted();
	}

	private void OnGameOver()
	{
	}
}
