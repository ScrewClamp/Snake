using System;
using UnityEngine;

[Serializable]
public struct LevelData
{
	public int levelIndex;

	[Space(10f)]
	public int targetHpForLevelUp;

	public float levelMinDurationInSeconds;

	[Range(1f, 13f), Space(10f)]
	public float cameraKillTime;

	[Range(1f, 25f)]
	public float cameraMaxSpeed;

	[Range(2f, 20f)]
	public float playerInitialSpeed;

	[Range(2f, 20f)]
	public float playerMaxSpeed;

	[Range(0f, 5f)]
	public float playerDefaultAcceleration;

	[Range(0f, 10f)]
	public float playerTimeWithInertiaInSeconds;

	[Range(0f, 5f)]
	public float playerDecelerationDurationInSeconds;

	[Range(0f, 100f), Space(10f)]
	public float easyBlockProbability;

	[Range(0f, 1f)]
	public float easyBlockAndPlayerLiveCountRatio;

	[Range(0f, 100f)]
	public float blockNoiseProbability;

	[Range(1f, 100f)]
	public float blockNoiseRangePercent;

	[Range(0f, 100f)]
	public float blockNoiseLowerBoundProbability;

	[Range(1f, 50f)]
	public int blockHpRelativeRange;

	[Range(0f, 100f)]
	public float blockHpDifficulty;

	[Range(0f, 100f), Space(10f)]
	public float obstacleMinPeriodInSeconds;

	[Range(0f, 100f)]
	public float obstacleMaxPeriodInSeconds;

	[Range(0f, 1f)]
	public float obstacleLaneProbabilityForPlayer;

	[Range(0f, 10f)]
	public float obstacleTimeoutToShowWarning;

	[Range(0f, 10f)]
	public float obstacleWarningDuration;

	[Range(0f, 10f), Space(10f)]
	public float collectiblePeriodInSeconds;

	[Range(-1f, 0f)]
	public float collectibleMinDeltaPeriodInSeconds;

	[Range(0f, 1f)]
	public float collectibleMaxDeltaPeriodInSeconds;

	[Range(0f, 5f)]
	public float collectibleHpIncrement;

	[Range(0f, 100f)]
	public float collectibleNoiseProbability;

	[Range(1f, 100f)]
	public float collectibleNoiseRangePercent;

	[Range(0f, 100f)]
	public float collectibleNoiseLowerBoundProbability;

	[Range(0f, 20f), Space(10f)]
	public float powerUpPeriodInSeconds;

	[Range(-3f, 0f)]
	public float powerUpMinDeltaPeriodInSeconds;

	[Range(0f, 3f)]
	public float powerUpMaxDeltaPeriodInSeconds;
}
