using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
	private BannerView bannerView;

	private InterstitialAd interstitial;

	private RewardBasedVideoAd rewardBasedVideo;

	private float deltaTime;

	private static string outputMessage = string.Empty;

	public static string OutputMessage
	{
		set
		{
			GoogleMobileAdsDemoScript.outputMessage = value;
		}
	}

	public void Start()
	{
		string appId = "ca-app-pub-3620204276438890~9271639921";
		MobileAds.Initialize(appId);
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;
		this.rewardBasedVideo.OnAdLoaded += new EventHandler<EventArgs>(this.HandleRewardBasedVideoLoaded);
		this.rewardBasedVideo.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.HandleRewardBasedVideoFailedToLoad);
		this.rewardBasedVideo.OnAdOpening += new EventHandler<EventArgs>(this.HandleRewardBasedVideoOpened);
		this.rewardBasedVideo.OnAdStarted += new EventHandler<EventArgs>(this.HandleRewardBasedVideoStarted);
		this.rewardBasedVideo.OnAdRewarded += new EventHandler<Reward>(this.HandleRewardBasedVideoRewarded);
		this.rewardBasedVideo.OnAdClosed += new EventHandler<EventArgs>(this.HandleRewardBasedVideoClosed);
		this.rewardBasedVideo.OnAdLeavingApplication += new EventHandler<EventArgs>(this.HandleRewardBasedVideoLeftApplication);
	}

	public void Update()
	{
		this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
	}

	public void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle();
		Rect position = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		gUIStyle.alignment = TextAnchor.LowerRight;
		gUIStyle.fontSize = (int)((double)Screen.height * 0.06);
		gUIStyle.normal.textColor = new Color(0f, 0f, 0.5f, 1f);
		float num = 1f / this.deltaTime;
		string text = string.Format("{0:0.} fps", num);
		GUI.Label(position, text, gUIStyle);
		GUI.skin.button.fontSize = (int)(0.035f * (float)Screen.width);
		float width = 0.35f * (float)Screen.width;
		float height = 0.15f * (float)Screen.height;
		float x = 0.1f * (float)Screen.width;
		float x2 = 0.55f * (float)Screen.width;
		Rect position2 = new Rect(x, 0.05f * (float)Screen.height, width, height);
		if (GUI.Button(position2, "Request\nBanner"))
		{
			this.RequestBanner();
		}
		Rect position3 = new Rect(x, 0.225f * (float)Screen.height, width, height);
		if (GUI.Button(position3, "Destroy\nBanner"))
		{
			this.bannerView.Destroy();
		}
		Rect position4 = new Rect(x, 0.4f * (float)Screen.height, width, height);
		if (GUI.Button(position4, "Request\nInterstitial"))
		{
			this.RequestInterstitial();
		}
		Rect position5 = new Rect(x, 0.575f * (float)Screen.height, width, height);
		if (GUI.Button(position5, "Show\nInterstitial"))
		{
			this.ShowInterstitial();
		}
		Rect position6 = new Rect(x, 0.75f * (float)Screen.height, width, height);
		if (GUI.Button(position6, "Destroy\nInterstitial"))
		{
			this.interstitial.Destroy();
		}
		Rect position7 = new Rect(x2, 0.05f * (float)Screen.height, width, height);
		if (GUI.Button(position7, "Request\nRewarded Video"))
		{
			this.RequestRewardBasedVideo();
		}
		Rect position8 = new Rect(x2, 0.225f * (float)Screen.height, width, height);
		if (GUI.Button(position8, "Show\nRewarded Video"))
		{
			this.ShowRewardBasedVideo();
		}
		Rect position9 = new Rect(x2, 0.925f * (float)Screen.height, width, 0.05f * (float)Screen.height);
		GUI.Label(position9, GoogleMobileAdsDemoScript.outputMessage);
	}

	private AdRequest CreateAdRequest()
	{
		return new AdRequest.Builder().AddTestDevice("SIMULATOR").AddTestDevice("0123456789ABCDEF0123456789ABCDEF").AddKeyword("game").SetGender(Gender.Male).SetBirthday(new DateTime(1985, 1, 1)).TagForChildDirectedTreatment(false).AddExtra("color_bg", "9B30FF").Build();
	}

	private void RequestBanner()
	{
		string adUnitId = "ca-app-pub-3620204276438890/6453904891";
		if (this.bannerView != null)
		{
			this.bannerView.Destroy();
		}
		this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
		this.bannerView.OnAdLoaded += new EventHandler<EventArgs>(this.HandleAdLoaded);
		this.bannerView.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.HandleAdFailedToLoad);
		this.bannerView.OnAdOpening += new EventHandler<EventArgs>(this.HandleAdOpened);
		this.bannerView.OnAdClosed += new EventHandler<EventArgs>(this.HandleAdClosed);
		this.bannerView.OnAdLeavingApplication += new EventHandler<EventArgs>(this.HandleAdLeftApplication);
		this.bannerView.LoadAd(this.CreateAdRequest());
	}

	private void RequestInterstitial()
	{
		string adUnitId = "ca-app-pub-3620204276438890/7383843183";
		if (this.interstitial != null)
		{
			this.interstitial.Destroy();
		}
		this.interstitial = new InterstitialAd(adUnitId);
		this.interstitial.OnAdLoaded += new EventHandler<EventArgs>(this.HandleInterstitialLoaded);
		this.interstitial.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.HandleInterstitialFailedToLoad);
		this.interstitial.OnAdOpening += new EventHandler<EventArgs>(this.HandleInterstitialOpened);
		this.interstitial.OnAdClosed += new EventHandler<EventArgs>(this.HandleInterstitialClosed);
		this.interstitial.OnAdLeavingApplication += new EventHandler<EventArgs>(this.HandleInterstitialLeftApplication);
		this.interstitial.LoadAd(this.CreateAdRequest());
	}

	private void RequestRewardBasedVideo()
	{
		string adUnitId = "ca-app-pub-3940256099942544/5224354917";
		this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
	}

	private void ShowInterstitial()
	{
		if (this.interstitial.IsLoaded())
		{
			this.interstitial.Show();
		}
		else
		{
			GoogleMobileAdsDemoScript.outputMessage = "Interstitial is not ready yet";
		}
	}

	private void ShowRewardBasedVideo()
	{
		if (this.rewardBasedVideo.IsLoaded())
		{
			this.rewardBasedVideo.Show();
		}
		else
		{
			GoogleMobileAdsDemoScript.outputMessage = "Reward based video ad is not ready yet";
		}
	}

	public void HandleAdLoaded(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleAdLoaded event received";
	}

	public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleFailedToReceiveAd event received with message: " + args.Message;
	}

	public void HandleAdOpened(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleAdOpened event received";
	}

	public void HandleAdClosed(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleAdClosed event received";
	}

	public void HandleAdLeftApplication(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleAdLeftApplication event received";
	}

	public void HandleInterstitialLoaded(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleInterstitialLoaded event received";
	}

	public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleInterstitialFailedToLoad event received with message: " + args.Message;
	}

	public void HandleInterstitialOpened(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleInterstitialOpened event received";
	}

	public void HandleInterstitialClosed(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleInterstitialClosed event received";
	}

	public void HandleInterstitialLeftApplication(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleInterstitialLeftApplication event received";
	}

	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleRewardBasedVideoLoaded event received";
	}

	public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message;
	}

	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleRewardBasedVideoOpened event received";
	}

	public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleRewardBasedVideoStarted event received";
	}

	public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleRewardBasedVideoClosed event received";
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		string type = args.Type;
		GoogleMobileAdsDemoScript.outputMessage = "HandleRewardBasedVideoRewarded event received for " + args.Amount.ToString() + " " + type;
	}

	public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
	{
		GoogleMobileAdsDemoScript.outputMessage = "HandleRewardBasedVideoLeftApplication event received";
	}
}
