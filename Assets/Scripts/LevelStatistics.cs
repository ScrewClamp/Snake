using System;

[Serializable]
public class LevelStatistics
{
	public int levelIndex;

	public int tapCount;

	public float levelUpDurationSeconds;

	public float levelOverallDurationSeconds;

	public int gameOverCountBeforeLevelUp;

	public float TapFrequencyInSeconds
	{
		get
		{
			return this.levelOverallDurationSeconds / (float)this.tapCount;
		}
	}

	public float AverageSessionDuration
	{
		get
		{
			return this.levelOverallDurationSeconds / (float)(this.gameOverCountBeforeLevelUp + 1);
		}
	}

	public override string ToString()
	{
		return string.Format("\t Level : {0} \n\t Tap Count: {1} \n\t Tap Frequency  (Sec): {2}\n\t Average Session Duration (Sec): {3}\n\t Level Up Duration (Sec): {4}\n\t Level Overall Duration (Sec): {5}\n\t GameOver Count Before Level Up: {6}", new object[]
		{
			this.levelIndex + 1,
			this.tapCount,
			this.TapFrequencyInSeconds,
			this.AverageSessionDuration,
			this.levelUpDurationSeconds,
			this.levelOverallDurationSeconds,
			this.gameOverCountBeforeLevelUp
		});
	}
}
