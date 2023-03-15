using System;
using UnityEngine;

public class AdsTester : MonoBehaviour
{
	private AdmobManager _admobManager;

	private AdsManager _adsManager;

	private UnityAdsManager _unityAdsManager;

	private string outputMessage = "Barev";

	private void Awake()
	{
		this._admobManager = base.GetComponent<AdmobManager>();
		this._unityAdsManager = base.GetComponent<UnityAdsManager>();
		this._adsManager = base.GetComponent<AdsManager>();
		this._admobManager.RewardedVideoFinishedEvent += new Action<bool>(this.AdmobManagerOnRewardedVideoFinishedEvent);
		this._unityAdsManager.RewardedVideoFinishedEvent += new Action<bool>(this.UnityAdsManagerOnRewardedVideoFinishedEvent);
	}

	private void UnityAdsManagerOnRewardedVideoFinishedEvent(bool obj)
	{
		this.outputMessage = "Unity Rewarded Video Finished " + obj;
	}

	private void AdmobManagerOnRewardedVideoFinishedEvent(bool obj)
	{
		this.outputMessage = "Admob Rewarded Video Finished " + obj;
	}

	public void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle();
		GUI.skin.button.fontSize = (int)(0.035f * (float)Screen.width);
		float width = 0.35f * (float)Screen.width;
		float height = 0.15f * (float)Screen.height;
		float x = 0.1f * (float)Screen.width;
		float x2 = 0.55f * (float)Screen.width;
		Rect position = new Rect(x, 0.05f * (float)Screen.height, width, height);
		if (GUI.Button(position, "Request\nBanner"))
		{
			this._admobManager.RequestBanner();
		}
		Rect position2 = new Rect(x, 0.225f * (float)Screen.height, width, height);
		if (GUI.Button(position2, "Show\nBanner"))
		{
			this._admobManager.ShowBanner();
		}
		Rect position3 = new Rect(x, 0.4f * (float)Screen.height, width, height);
		if (GUI.Button(position3, "Request\nInterstitial"))
		{
			this._admobManager.RequestInterstitial();
		}
		Rect position4 = new Rect(x, 0.575f * (float)Screen.height, width, height);
		if (GUI.Button(position4, "Show\nInterstitial"))
		{
			this._admobManager.ShowInterstitial();
		}
		Rect position5 = new Rect(x2, 0.4f * (float)Screen.height, width, height);
		if (GUI.Button(position5, "Admanager \nShow Video"))
		{
			this._adsManager.ShowRewardedVideo();
		}
		Rect position6 = new Rect(x2, 0.575f * (float)Screen.height, width, height);
		if (GUI.Button(position6, "Admanager Show\nInterstitial"))
		{
			this._adsManager.ShowInterstitial();
		}
		Rect position7 = new Rect(x, 0.75f * (float)Screen.height, width, height);
		if (GUI.Button(position7, "SHow Unity \n Video"))
		{
			this._unityAdsManager.ShowRewardedAd();
		}
		Rect position8 = new Rect(x2, 0.75f * (float)Screen.height, width, height);
		if (GUI.Button(position8, "SHow Unity \n Inters"))
		{
			this._unityAdsManager.ShowInterstitial();
		}
		Rect position9 = new Rect(x2, 0.05f * (float)Screen.height, width, height);
		if (GUI.Button(position9, "Request\nRewarded Video"))
		{
		}
		Rect position10 = new Rect(x2, 0.225f * (float)Screen.height, width, height);
		if (GUI.Button(position10, "Show\nRewarded Video"))
		{
			this._admobManager.ShowRewardedAd();
		}
		Rect position11 = new Rect(x2, 0.925f * (float)Screen.height, width, 0.05f * (float)Screen.height);
		GUI.Label(position11, this.outputMessage);
	}
}
