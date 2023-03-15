using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
	private sealed class _ShowInterstitial_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal AdsManager _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _ShowInterstitial_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(0.5f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._this._isAdmobInterstitialTurnToShow)
				{
					this._this.TryToShowAdmobInterstitial();
				}
				else
				{
					this._this.TryToShowUnityAdsInterstitial();
				}
				this._this._isAdmobInterstitialTurnToShow = !this._this._isAdmobInterstitialTurnToShow;
				if (this._this._isAdmobInterstitialTurnToShow)
				{
					this._this._admobManager.RequestInterstitial();
				}
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}



	public GameState _gameState;

	public int sessionCountToShowInterstitial;

	public int currentSessionCount;

	public Button noAdsButton;

	public Text noAdsButtonText;

	public Purchaser _purchaser;

	private AdmobManager _admobManager;

	private UnityAdsManager _unityAdsManager;

	private bool _isAdmobVideoAdTurnToShow;

	private bool _isAdmobInterstitialTurnToShow;

	private bool _IsNoAds_k__BackingField;

	public event Action<bool> RewardedVideoFinishedEvent;

	public bool IsNoAds
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.IsNoAds = (PlayerPrefs.GetInt("NoAds", 0) == 1);
		this.noAdsButton.gameObject.SetActive(!this.IsNoAds);
		this._unityAdsManager = base.GetComponent<UnityAdsManager>();
		this._unityAdsManager.Init();
		this._unityAdsManager.RewardedVideoFinishedEvent += new Action<bool>(this.OnRewardedVideoFinished);
		if (!this.IsNoAds)
		{
			this._admobManager = base.GetComponent<AdmobManager>();
			this._admobManager.Init();
			this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
			this._gameState.OnGameStartedEvent.AddListener(new UnityAction(this.OnGameStarted));
			this._purchaser.InitializedEvent += new Action(this.PurchaserOnInitializedEvent);
		}
	}

	private void PurchaserOnInitializedEvent()
	{
		this._purchaser.InitializedEvent -= new Action(this.PurchaserOnInitializedEvent);
		if (this._purchaser.IsInitialized())
		{
			string productPrice = this._purchaser.GetProductPrice();
			this.noAdsButtonText.text = ((!(productPrice == string.Empty)) ? productPrice : "BUY");
		}
	}

	private void OnGameStarted()
	{
		this._gameState.OnGameStartedEvent.RemoveListener(new UnityAction(this.OnGameStarted));
		this.ShowBannerAd();
	}

	public void OnNoAdsPurchesed()
	{
		PlayerPrefs.SetInt("NoAds", 1);
		this.IsNoAds = true;
		this.noAdsButton.gameObject.SetActive(!this.IsNoAds);
		this._admobManager.RemoveAds();
	}

	private void OnGameOver()
	{
		if (this.IsNoAds)
		{
			return;
		}
		this.currentSessionCount++;
		if (this.currentSessionCount % this.sessionCountToShowInterstitial == 0)
		{
			this.currentSessionCount = 0;
			if (!this.IsNoAds)
			{
				base.StartCoroutine(this.ShowInterstitial());
			}
		}
	}

	private void OnRewardedVideoFinished(bool isSuccess)
	{
		if (isSuccess)
		{
			this.currentSessionCount = 0;
		}
		if (this.RewardedVideoFinishedEvent != null)
		{
			this.RewardedVideoFinishedEvent(isSuccess);
		}
	}

	public void ShowRewardedVideo()
	{
		if (this._unityAdsManager.IsRewardedVideoLoaded())
		{
			this._unityAdsManager.ShowRewardedAd();
		}
	}

	public IEnumerator ShowInterstitial()
	{
		AdsManager._ShowInterstitial_c__Iterator0 _ShowInterstitial_c__Iterator = new AdsManager._ShowInterstitial_c__Iterator0();
		_ShowInterstitial_c__Iterator._this = this;
		return _ShowInterstitial_c__Iterator;
	}

	private void TryToShowUnityAdsInterstitial()
	{
		if (this._unityAdsManager.IsInterstitialLoaded())
		{
			this._unityAdsManager.ShowInterstitial();
		}
		else
		{
			this._admobManager.ShowInterstitial();
		}
	}

	private void TryToShowAdmobInterstitial()
	{
		if (this._admobManager.IsInterstitialLoaded())
		{
			this._admobManager.ShowInterstitial();
		}
		else
		{
			this._unityAdsManager.ShowInterstitial();
		}
	}

	public void ShowBannerAd()
	{
		if (this.IsNoAds)
		{
			return;
		}
		this._admobManager.RequestBanner();
	}

	public bool IsAnyRewardedVideoAvailable()
	{
		return this._unityAdsManager.IsRewardedVideoLoaded();
	}
}
