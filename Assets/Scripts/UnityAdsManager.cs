using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour
{
	public string UnidtAdAppID = "2745559";

	public event Action<bool> RewardedVideoFinishedEvent;

	public void Init()
	{
		Advertisement.Initialize(UnidtAdAppID);
	}

	public bool IsRewardedVideoLoaded()
	{
		return Advertisement.IsReady("rewardedVideo");
	}

	public void ShowRewardedAd()
	{
		if (this.IsRewardedVideoLoaded())
		{
			ShowOptions showOptions = new ShowOptions
			{
				resultCallback = new Action<ShowResult>(this.HandleShowResult)
			};
			Advertisement.Show("rewardedVideo", showOptions);
		}
	}

	public bool IsInterstitialLoaded()
	{
		return Advertisement.IsReady("video");
	}

	public void ShowInterstitial()
	{
		if (this.IsInterstitialLoaded())
		{
			Advertisement.Show("video");
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		if (result != ShowResult.Finished)
		{
			if (result != ShowResult.Skipped)
			{
				if (result == ShowResult.Failed)
				{
					UnityEngine.Debug.LogError("The ad failed to be shown.");
				}
			}
			else
			{
				UnityEngine.Debug.Log("The ad was skipped before reaching the end.");
			}
		}
		else
		{
			UnityEngine.Debug.Log("The ad was successfully shown.");
		}
		bool obj = result == ShowResult.Finished;
		if (this.RewardedVideoFinishedEvent != null)
		{
			this.RewardedVideoFinishedEvent(obj);
		}
	}
}
