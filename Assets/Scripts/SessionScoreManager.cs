using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class SessionScoreManager : MonoBehaviour
{
	private const string sessionBestScoreKey = "SessionBestScore";



	[SerializeField]
	private StringWave _blockStringWave;

	[SerializeField]
	private PerfectWave _blockPerfectWave;

	[SerializeField]
	private LevelManager _levelManager;

	[SerializeField]
	private GameState _gameState;

	private int _SessionScore_k__BackingField;

	public event Action ScoreUpdatedEvent;

	public bool IsNewBestScore
	{
		get
		{
			return this.SessionScore == this.SessionBestScore && this.SessionScore > 0;
		}
	}

	public int SessionScore
	{
		get;
		private set;
	}

	public int SessionBestScore
	{
		get
		{
			return PlayerPrefs.GetInt("SessionBestScore", 0);
		}
		private set
		{
			PlayerPrefs.SetInt("SessionBestScore", value);
		}
	}

	private void Start()
	{
		StringWave expr_06 = this._blockStringWave;
		expr_06.onBlockDestroy = (Action<GameObject>)Delegate.Combine(expr_06.onBlockDestroy, new Action<GameObject>(this.OnBlockDestroy));
		PerfectWave expr_2D = this._blockPerfectWave;
		expr_2D.onPerfectWave = (Action<Vector3, int>)Delegate.Combine(expr_2D.onPerfectWave, new Action<Vector3, int>(this.OnPerfectWave));
		this._gameState.OnGameStartedEvent.AddListener(new UnityAction(this.OnGameStarted));
	}

	private void OnGameStarted()
	{
		this.SessionScore = 0;
	}

	private void OnBlockDestroy(GameObject block)
	{
		int score = block.GetComponent<ScoreComponent>().GetScore();
		this.OnScoreChanged(score);
	}

	private void OnScoreChanged(int blockScore)
	{
		this.UpdateScore(blockScore);
		if (this.ScoreUpdatedEvent != null)
		{
			this.ScoreUpdatedEvent();
		}
	}

	private void OnPerfectWave(Vector3 pos, int score)
	{
		this.OnScoreChanged(score);
	}

	private void UpdateScore(int blockScore)
	{
		this.SessionScore += blockScore;
		if (this.SessionBestScore < this.SessionScore)
		{
			this.SessionBestScore = this.SessionScore;
		}
	}
}
