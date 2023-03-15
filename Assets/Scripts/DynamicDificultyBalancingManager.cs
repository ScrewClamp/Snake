using System;
using UnityEngine;
using UnityEngine.Events;

public class DynamicDificultyBalancingManager : MonoBehaviour
{
	public GameState gameState;

	public LevelDataManager levelDataManager;

	public LevelManager levelManager;

	public BlockGenerator blockGenerator;

	public CollectibleGenerator collectibleGenerator;

	public int gameOverCountThresholdToMakeLevelEasier = 4;

	public int maxCountToMakeEasier;

	public float easierRatio;

	private int _countToMakeEasier;

	private int _gameOverCountOnLevel;

	private float _maxCollectiblePeriodInSeconds;

	private float _maxEasyBlockProbability;

	private void Start()
	{
		this.gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
		this.levelManager.LevelUpEvent += new Action<int>(this.OnLevelUp);
		this.levelDataManager.ConfigurationFinishedEvent += new Action(this.OnConfigurationFinished);
		this.OnConfigurationFinished();
	}

	private void OnConfigurationFinished()
	{
		this._maxCollectiblePeriodInSeconds = this.collectibleGenerator.PeriodInSeconds * 0.7f;
		this._maxEasyBlockProbability = 95f;
	}

	private void OnLevelUp(int levelIndex)
	{
		this._gameOverCountOnLevel = 0;
		this._countToMakeEasier = 0;
	}

	private void OnGameOver()
	{
		this._gameOverCountOnLevel++;
		bool flag = this._gameOverCountOnLevel % this.gameOverCountThresholdToMakeLevelEasier == 0;
		bool flag2 = this._countToMakeEasier <= this.maxCountToMakeEasier && flag;
		if (flag2)
		{
			this._countToMakeEasier++;
			this.MakeLevelEasier();
		}
	}

	private void MakeLevelEasier()
	{
		this.collectibleGenerator.PeriodInSeconds = Mathf.Max(this._maxCollectiblePeriodInSeconds, this.collectibleGenerator.PeriodInSeconds * (1f - this.easierRatio));
		this.blockGenerator.EasyBlockProbability = Mathf.Min(this._maxEasyBlockProbability, this.blockGenerator.EasyBlockProbability + 100f * this.easierRatio);
	}
}
