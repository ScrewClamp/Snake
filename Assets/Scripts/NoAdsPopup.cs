using System;
using UnityEngine;

public class NoAdsPopup : MonoBehaviour
{
	public AdsManager adsManager;

	public UIElementGroup _elementGroup;

	[SerializeField]
	private Purchaser _purchaser;

	private void Start()
	{
		this._elementGroup = base.GetComponent<UIElementGroup>();
	}

	private void PurchaserOnPurchaseFinishedEvent()
	{
		this._purchaser.PurchaseFinishedEvent -= new Action(this.PurchaserOnPurchaseFinishedEvent);
		this.adsManager.OnNoAdsPurchesed();
		this.Hide();
	}

	public void Show()
	{
		BackButton.listeners.Add(new Action(this.Hide));
		this._elementGroup.Show();
	}

	public void Hide()
	{
		BackButton.RemoveLast();
		this._elementGroup.Hide();
	}

	public void BuyNoAdsProduct()
	{
		this._purchaser.PurchaseFinishedEvent += new Action(this.PurchaserOnPurchaseFinishedEvent);
		this._purchaser.BuyNonConsumable();
	}

	public void RestoreNoAdsProduct()
	{
		this._purchaser.PurchaseFinishedEvent += new Action(this.OnPurchaseFinished);
		this._purchaser.RestorePurchases();
	}

	private void OnPurchaseFinished()
	{
		this._purchaser.PurchaseFinishedEvent -= new Action(this.OnPurchaseFinished);
		this.adsManager.OnNoAdsPurchesed();
	}
}
