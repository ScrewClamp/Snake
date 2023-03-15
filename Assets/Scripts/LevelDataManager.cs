using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{


	[SerializeField]
	private LevelManager _levelManager;

	[SerializeField]
	private LevelProgress _levelProgress;

	[SerializeField]
	private CameraSpeed _cameraSpeed;

	[SerializeField]
	private PlayerSpeed _playerSpeed;

	[SerializeField]
	private BlockGenerator _blockGenerator;

	[SerializeField]
	private CollectibleGenerator _collectibleGenerator;

	[SerializeField]
	private ObstacleGenerator _obstacleGenerator;

	[SerializeField]
	private PowerUpGenerator _powerUpGenerator;

	[SerializeField]
	private List<LevelData> _levelDataContainerOld;

	[SerializeField]
	private List<LevelData> _levelDataContainer;

	private LevelData _currentLevelData;

	public event Action ConfigurationFinishedEvent;

	private void Awake()
	{
		this._levelManager.LevelUpEvent += new Action<int>(this.OnLevelUp);
	}

	private void OnLevelUp(int levelIndex)
	{
		this.ConfigureGameComponents(levelIndex);
	}

	private void Start()
	{
		this.ConfigureGameComponents(this._levelManager.CurrentLevelIndex);
	}

	private void ConfigureGameComponents(int levelIndex)
	{
		int currentLevel = Mathf.Min(levelIndex, this._levelDataContainer.Count - 1);
		this.SetCurrentLevel(currentLevel);
		this.ConfigureTargetHpForLevelUp();
		this.ConfigurePlayerSpeed();
		this.ConfigureCameraSpeed();
		this.ConfigureBlockGenerator();
		this.ConfigureCollectibleGenerator();
		this.ConfigureObstacleGenerator();
		this.ConfigurePowerUpGenerator();
		if (this.ConfigurationFinishedEvent != null)
		{
			this.ConfigurationFinishedEvent();
		}
	}

	private void ConfigureTargetHpForLevelUp()
	{
		this._levelManager.TargetHpForLevelUp = this._currentLevelData.targetHpForLevelUp;
	}

	private void ConfigureCameraSpeed()
	{
		this._cameraSpeed.cameraMaxSpeed = this._currentLevelData.cameraMaxSpeed;
		this._cameraSpeed.SetCameraKillTime(this._currentLevelData.cameraKillTime);
		float num = this._cameraSpeed.CameraHalfHeight();
		float initialSpeed = this._playerSpeed.initialSpeed;
		float num2 = num / this._currentLevelData.cameraKillTime;
		this._cameraSpeed.InitialSpeed = initialSpeed + num2;
	}

	private void ConfigurePlayerSpeed()
	{
		this._playerSpeed.initialSpeed = this._currentLevelData.playerInitialSpeed;
		this._playerSpeed.maxSpeed = this._currentLevelData.playerMaxSpeed;
		this._playerSpeed.SetDefaultAcceleration(this._currentLevelData.playerDefaultAcceleration);
		this._playerSpeed.timeWithInertia = this._currentLevelData.playerTimeWithInertiaInSeconds;
		this._playerSpeed.decelerationDuration = this._currentLevelData.playerDecelerationDurationInSeconds;
		this._playerSpeed.ResetSpeed();
	}

	private void SetCurrentLevel(int levelIndex)
	{
		this._currentLevelData = this._levelDataContainer[levelIndex];
	}

	private void ConfigureBlockGenerator()
	{
		this._blockGenerator.BlockHpDifficulty = this._currentLevelData.blockHpDifficulty;
		this._blockGenerator.BlockHpRelativeRange = this._currentLevelData.blockHpRelativeRange;
		this._blockGenerator.NoiseProbability = this._currentLevelData.blockNoiseProbability;
		this._blockGenerator.NoiseRangePercent = this._currentLevelData.blockNoiseRangePercent;
		this._blockGenerator.NoiseLowerBoundProbability = this._currentLevelData.blockNoiseLowerBoundProbability;
		this._blockGenerator.EasyBlockProbability = this._currentLevelData.easyBlockProbability;
		this._blockGenerator.EasyBlockAndPlayerLiveCountRatio = this._currentLevelData.easyBlockAndPlayerLiveCountRatio;
	}

	private void ConfigureCollectibleGenerator()
	{
		this._collectibleGenerator.LevelMinDurationInSeconds = this._currentLevelData.levelMinDurationInSeconds;
		this._collectibleGenerator.TargetHpForLevelUp = this._currentLevelData.targetHpForLevelUp;
		this._collectibleGenerator.HpProgressionDifference = this._currentLevelData.collectibleHpIncrement;
		this._collectibleGenerator.PeriodInSeconds = this._currentLevelData.collectiblePeriodInSeconds;
		this._collectibleGenerator.MinDeltaPeriodInSeconds = this._currentLevelData.collectibleMinDeltaPeriodInSeconds;
		this._collectibleGenerator.MaxDeltaPeriodInSeconds = this._currentLevelData.collectibleMaxDeltaPeriodInSeconds;
		this._collectibleGenerator.NoiseProbability = this._currentLevelData.collectibleNoiseProbability;
		this._collectibleGenerator.NoiseRangePercent = this._currentLevelData.collectibleNoiseRangePercent;
		this._collectibleGenerator.NoiseLowerBoundProbability = this._currentLevelData.collectibleNoiseLowerBoundProbability;
		this._collectibleGenerator.OnParseFinish();
	}

	private void ConfigureObstacleGenerator()
	{
		this._obstacleGenerator.MinPeriodInSeconds = this._currentLevelData.obstacleMinPeriodInSeconds;
		this._obstacleGenerator.MaxPeriodInSeconds = this._currentLevelData.obstacleMaxPeriodInSeconds;
		this._obstacleGenerator.LaneProbabilityForPlayer = this._currentLevelData.obstacleLaneProbabilityForPlayer;
		this._obstacleGenerator.TimeoutToShowWarning = this._currentLevelData.obstacleTimeoutToShowWarning;
		this._obstacleGenerator.WarningDuration = this._currentLevelData.obstacleWarningDuration;
	}

	private void ConfigurePowerUpGenerator()
	{
		this._powerUpGenerator.PeriodInSeconds = this._currentLevelData.powerUpPeriodInSeconds;
		this._powerUpGenerator.MinDeltaPeriodInSeconds = this._currentLevelData.powerUpMinDeltaPeriodInSeconds;
		this._powerUpGenerator.MaxDeltaPeriodInSeconds = this._currentLevelData.powerUpMaxDeltaPeriodInSeconds;
		this._powerUpGenerator.OnParseFinish();
	}
}
