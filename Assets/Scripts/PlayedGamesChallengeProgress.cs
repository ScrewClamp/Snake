using System;

public class PlayedGamesChallengeProgress : AbstractChallengeProgress
{
	protected sealed override string GetChallengeName()
	{
		return "playedGamesChallenge";
	}

	protected sealed override void InitializeChallengeProgress()
	{
		int playedGames = this._challengeManager.GetPlayedGames();
		this.challengeItem.currentProgress = playedGames;
	}

	protected sealed override void RegisterListeners()
	{
		ChallengeManager expr_06 = this._challengeManager;
		expr_06.OnPlayedGamesChanged = (Action<int>)Delegate.Combine(expr_06.OnPlayedGamesChanged, new Action<int>(base.UpdateChallengeProgress));
	}
}
