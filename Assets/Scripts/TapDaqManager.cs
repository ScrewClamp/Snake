using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using Tapdaq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TapDaqManager : MonoBehaviour
{
	private sealed class _ShowNativeAd_c__AnonStorey0
	{
		internal TDNativeAdType adType;

		internal TapDaqManager _this;

		internal void __m__0(TDNativeAd obj)
		{
			this._this.DisplayNativeAd(obj, this._this.largeCrossPromoObject, this._this.largeCrossPromoImage, this.adType);
			AdManager.SendNativeImpression(this._this.largeNativeAd);
		}
	}

	private static readonly TDNativeAdType largePromoAdType = TDNativeAdType.TDNativeAdType2x3Large;

	[SerializeField]
	private GameState _gameState;

	[SerializeField]
	private string placementTag;

	private TDNativeAd largeNativeAd;

	[SerializeField]
	private RectTransform largeCrossPromoObject;

	[SerializeField]
	private Image largeCrossPromoImage;

	private bool isTapDaqReady;

	private bool isTapdaqAdLoaded;

	private void Awake()
	{
		AdManager.Init();
		this._gameState.OnGameStartedEvent.AddListener(new UnityAction(this.OnGameStarted));
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
	}

	private void Start()
	{
		this.HideCrossPromoObjects(false);
	}

	private void HideCrossPromoObjects(bool animate = false)
	{
		UnityEngine.Debug.Log("HideCrossPromoObjects");
		Vector2 vector = new Vector2(-this.largeCrossPromoObject.rect.width * this.largeCrossPromoObject.localScale.x, this.largeCrossPromoObject.anchoredPosition.y);
		if (animate)
		{
			this.largeCrossPromoObject.DOAnchorPos(vector, 0.5f, false).SetEase(Ease.OutExpo).OnComplete(delegate
			{
				this.largeCrossPromoObject.gameObject.SetActive(false);
			});
			return;
		}
		this.largeCrossPromoObject.gameObject.SetActive(false);
		this.largeCrossPromoObject.anchoredPosition = vector;
	}

	private void OnGameStarted()
	{
		this.HideCrossPromoObjects(false);
		this.LoadTapDaqAds(new bool?(true));
	}

	private void OnGameOver()
	{
		this.ShowCrossPromoObjects();
	}

	private void ShowCrossPromoObjects()
	{
		UnityEngine.Debug.Log("ShowCrossPromoObjects");
		if (!this.isTapdaqAdLoaded)
		{
			return;
		}
		this.largeCrossPromoObject.gameObject.SetActive(true);
		this.largeCrossPromoObject.DOKill(true);
		this.largeCrossPromoObject.DOAnchorPosX(0f, 0.5f, false).SetEase(Ease.OutExpo);
	}

	private void OnEnable()
	{
		TDCallbacks.TapdaqConfigLoaded += new Action(this.OnTapdaqConfigLoaded);
		TDCallbacks.TapdaqConfigFailedToLoad += new Action<TDAdError>(this.OnTapdaqConfigFailToLoad);
		TDCallbacks.AdAvailable += new Action<TDAdEvent>(this.OnAdAvailable);
	}

	private void OnDisable()
	{
		TDCallbacks.TapdaqConfigLoaded -= new Action(this.OnTapdaqConfigLoaded);
		TDCallbacks.TapdaqConfigFailedToLoad -= new Action<TDAdError>(this.OnTapdaqConfigFailToLoad);
		TDCallbacks.AdAvailable -= new Action<TDAdEvent>(this.OnAdAvailable);
	}

	private void OnTapdaqConfigLoaded()
	{
		this.LoadTapDaqAds(new bool?(true));
		this.isTapDaqReady = true;
		UnityEngine.Debug.Log("OnTapdaqConfigLoaded");
	}

	private void OnTapdaqConfigFailToLoad(TDAdError error)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"OnTapdaqConfigFailToLoad ",
			error.code,
			"  ",
			error.message
		}));
	}

	private void LoadTapDaqAds(bool? large)
	{
		AdManager.LoadNativeAdvertForTag(this.placementTag, TapDaqManager.largePromoAdType);
		UnityEngine.Debug.Log("LoadTapDaqAds");
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		AdManager.OnApplicationPause(pauseStatus);
	}

	private void OnAdAvailable(TDAdEvent e)
	{
		UnityEngine.Debug.Log("OnAdAvailable Tapdaq " + this.placementTag);
		if (e.adType == "NATIVE_AD" && e.tag == this.placementTag)
		{
			this.largeNativeAd = AdManager.GetNativeAd(TapDaqManager.largePromoAdType, this.placementTag);
			this.ShowNativeAd(TapDaqManager.largePromoAdType);
		}
	}

	private void DisplayNativeAd(TDNativeAd nativeAd, RectTransform imageWrapper, Image image, TDNativeAdType nativeAdType)
	{
		Texture2D texture = nativeAd.texture;
		if (texture == null)
		{
			UnityEngine.Debug.LogError("Texture not loaded");
			return;
		}
		image.rectTransform.sizeDelta = nativeAdType.ToVector2();
		image.material.mainTexture = texture;
		this.isTapdaqAdLoaded = true;
	}

	public void ShowNativeAd(TDNativeAdType adType)
	{
		Resources.UnloadUnusedAssets();
		this.largeNativeAd.LoadTexture(delegate(TDNativeAd obj)
		{
			this.DisplayNativeAd(obj, this.largeCrossPromoObject, this.largeCrossPromoImage, adType);
			AdManager.SendNativeImpression(this.largeNativeAd);
		});
	}

	public void OnPromoClick()
	{
		AdManager.SendNativeClick(this.largeNativeAd);
	}
}
