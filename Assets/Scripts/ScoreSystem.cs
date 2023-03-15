using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
	private const string bestCollectedHpInLevelKey = "BestCollectedHpInLevel";

	[SerializeField]
	private LevelProgress _levelProgress;

	[SerializeField]
	private LevelManager _levelManager;

	private int _currentLevelScore;

	public int BestCollectedHpInLevel
	{
		get
		{
			return PlayerPrefs.GetInt("BestCollectedHpInLevel", 0);
		}
		set
		{
			PlayerPrefs.SetInt("BestCollectedHpInLevel", value);
		}
	}

	private void Start()
	{
		this._currentLevelScore = 0;
		this._levelProgress.ResetScore();
		int bestCollectedHpInLevel = this.BestCollectedHpInLevel;
		this._levelProgress.DrawFlagInProgress((float)bestCollectedHpInLevel / (float)this._levelManager.TargetHpForLevelUp);
		if (bestCollectedHpInLevel == 0)
		{
			this._levelProgress.HideFlag();
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("CurrentLevelText");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("NextLevelText");
		gameObject.GetComponent<Text>().text = this._levelProgress.GetLevelIndexToShow().ToString();
		gameObject2.GetComponent<Text>().text = (this._levelProgress.GetLevelIndexToShow() + 1).ToString();
		LevelProgress expr_AB = this._levelProgress;
		expr_AB.onNextLevel = (Action)Delegate.Combine(expr_AB.onNextLevel, new Action(this.OnLevelUp));
		this._levelManager.JustBeforeLevelUpEvent += new Action(this.OnJustBeforeLevelUp);
	}

	public int GetScore()
	{
		return this._currentLevelScore;
	}

	public void UpdateScore(int addedScore)
	{
		this._currentLevelScore += addedScore;
		this.UpdateBestScoreIfReached();
	}

	public void Reset()
	{
		this._currentLevelScore = 0;
		GameObject gameObject = GameObject.FindGameObjectWithTag("CurrentLevelText");
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("NextLevelText");
		gameObject.GetComponent<Text>().text = this._levelProgress.GetLevelIndexToShow().ToString();
		gameObject2.GetComponent<Text>().text = (this._levelProgress.GetLevelIndexToShow() + 1).ToString();
	}

	private void UpdateBestScoreIfReached()
	{
		int @int = PlayerPrefs.GetInt("BestCollectedHpInLevel", 0);
		int num = Mathf.Max(@int, this._currentLevelScore);
		PlayerPrefs.SetInt("BestCollectedHpInLevel", num);
		this._levelProgress.DrawFlagInProgress((float)num / (float)this._levelManager.TargetHpForLevelUp);
	}

	public int GetCompletedPercente()
	{
		return Math.Min((int)((float)this._currentLevelScore * 100f / (float)this._levelManager.TargetHpForLevelUp), 100);
	}

	private void OnJustBeforeLevelUp()
	{
		this.BestCollectedHpInLevel = 0;
		this._levelProgress.DrawFlagInProgress(0f / (float)this._levelManager.TargetHpForLevelUp);
	}

	private void OnLevelUp()
	{
		this._levelProgress.HideFlag();
		this.Reset();
	}
}
