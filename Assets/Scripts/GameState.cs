using System;
using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
	public enum State
	{
		InGame,
		Idle,
		GameOver
	}

	public UnityEvent OnGameStartedEvent;

	public UnityEvent OnGameIdleEvent;

	public UnityEvent OnGameOverEvent;

	public UnityEvent OnRequestGameOverEvent;

	private GameState.State _currentStateInternal;

	public GameState.State currentState
	{
		get
		{
			return this._currentStateInternal;
		}
		set
		{
			this._currentStateInternal = value;
			if (value != GameState.State.InGame)
			{
				if (value != GameState.State.Idle)
				{
					if (value == GameState.State.GameOver)
					{
						this.OnGameOverEvent.Invoke();
					}
				}
				else
				{
					this.OnGameIdleEvent.Invoke();
				}
			}
			else
			{
				this.OnGameStartedEvent.Invoke();
			}
		}
	}

	private void Awake()
	{
		this.currentState = GameState.State.Idle;
	}

	public bool IsInGame()
	{
		return this.currentState == GameState.State.InGame;
	}

	public bool IsGameIdle()
	{
		return this.currentState == GameState.State.Idle;
	}

	public bool IsGameOver()
	{
		return this.currentState == GameState.State.GameOver;
	}

	public void RequestGameOver()
	{
		if (this.OnRequestGameOverEvent != null)
		{
			this.OnRequestGameOverEvent.Invoke();
		}
	}
}
