using System;

public class CompletedLevelsChallengeProgress : AbstractChallengeProgress
{
	protected sealed override string GetChallengeName()
	{
		return "completedLevelsChallenge";
	}

	protected sealed override void InitializeChallengeProgress()
	{
		int completedLevels = this._challengeManager.GetCompletedLevels();
		this.challengeItem.currentProgress = completedLevels;
	}

	protected sealed override void RegisterListeners()
	{
		ChallengeManager expr_06 = this._challengeManager;
		expr_06.OnCompletedLevel = (Action<int>)Delegate.Combine(expr_06.OnCompletedLevel, new Action<int>(base.UpdateChallengeProgress));
	}
}
