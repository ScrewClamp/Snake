using System;
using UnityEngine;

public class SessionScoreBasedChallenge : AbstractChallengeProgress
{
	[SerializeField]
	private SessionScoreManager _sessionScoreManager;

	protected override void InitializeChallengeProgress()
	{
		int sessionScore = this._sessionScoreManager.SessionScore;
		this.challengeItem.currentProgress = sessionScore;
	}

	protected override void RegisterListeners()
	{
		this._sessionScoreManager.ScoreUpdatedEvent += new Action(this.OnScoreUpdated);
	}

	private void OnScoreUpdated()
	{
		int sessionScore = this._sessionScoreManager.SessionScore;
		base.UpdateChallengeProgress(sessionScore);
	}

	protected override string GetChallengeName()
	{
		return "completedSessionChallenge";
	}
}
