using System;
using UnityEngine;
using UnityEngine.UI;

public class UnlockSkinByAd : MonoBehaviour
{
	private string adLeft = "X  ";

	public Text watchAdText;

	public Button watchAdButton;

	public AdsManager adsManager;

	private AbstractChallengeProgress _challengeProgress;

	public void SetCurrentChallengeProgress(AbstractChallengeProgress currentChallengeProgress)
	{
		this._challengeProgress = currentChallengeProgress;
		int remainingAdToWatchCount = this._challengeProgress.GetRemainingAdToWatchCount();
		if (remainingAdToWatchCount > 0 && this.adsManager.IsAnyRewardedVideoAvailable())
		{
			this.watchAdButton.gameObject.SetActive(true);
			this.watchAdText.text = this.adLeft + remainingAdToWatchCount;
		}
		else
		{
			this.watchAdButton.gameObject.SetActive(false);
		}
	}

	public void OnWatchAdButtonClick()
	{
		this.adsManager.RewardedVideoFinishedEvent += new Action<bool>(this.OnRewardedVideoFinished);
		this.adsManager.ShowRewardedVideo();
	}

	private void OnRewardedVideoFinished(bool isSuccess)
	{
		this.adsManager.RewardedVideoFinishedEvent -= new Action<bool>(this.OnRewardedVideoFinished);
		if (isSuccess)
		{
			this.OnPlayerWatchedSingleAd();
		}
	}

	private void OnPlayerWatchedSingleAd()
	{
		this._challengeProgress.WatchSingleAd();
		int remainingAdToWatchCount = this._challengeProgress.GetRemainingAdToWatchCount();
		this.watchAdText.text = this.adLeft + remainingAdToWatchCount;
		if (remainingAdToWatchCount <= 0)
		{
			this.watchAdButton.gameObject.SetActive(false);
			base.GetComponent<ChallengeProgressPopup>().OnCloseChallengeProgressPopup();
		}
	}
}
