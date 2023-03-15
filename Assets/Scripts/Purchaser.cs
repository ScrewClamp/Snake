using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class Purchaser : MonoBehaviour, IStoreListener
{
	public string noAdProductIOS;

	public string noAdProductAndroid;







	private static IStoreController m_StoreController;

	private static IExtensionProvider m_StoreExtensionProvider;

	public static string kProductIDNonConsumable = "nonconsumable";

	public event Action InitializedEvent;

	public event Action PurchaseFinishedEvent;

	public event Action RestoreFinishedEvent;

	private void Start()
	{
		Purchaser.kProductIDNonConsumable = this.GetProductId();
		if (Purchaser.m_StoreController == null)
		{
			this.InitializePurchasing();
		}
	}

	public void InitializePurchasing()
	{
		if (this.IsInitialized())
		{
			return;
		}
		ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), Array.Empty<IPurchasingModule>());
		configurationBuilder.AddProduct(Purchaser.kProductIDNonConsumable, ProductType.NonConsumable);
		UnityPurchasing.Initialize(this, configurationBuilder);
	}

	public bool IsInitialized()
	{
		return Purchaser.m_StoreController != null && Purchaser.m_StoreExtensionProvider != null;
	}

	public void BuyNonConsumable()
	{
		this.BuyProductID(Purchaser.kProductIDNonConsumable);
	}

	public string GetProductPrice()
	{
		Product product = Purchaser.m_StoreController.products.WithID(this.GetProductId());
		return (product == null) ? string.Empty : product.metadata.localizedPriceString;
	}

	private void BuyProductID(string productId)
	{
		if (this.IsInitialized())
		{
			Product product = Purchaser.m_StoreController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				UnityEngine.Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				Purchaser.m_StoreController.InitiatePurchase(product);
			}
			else
			{
				UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		else
		{
			UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void RestorePurchases()
	{
		if (!this.IsInitialized())
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			UnityEngine.Debug.Log("RestorePurchases started ...");
			IAppleExtensions extension = Purchaser.m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			extension.RestoreTransactions(delegate(bool result)
			{
				UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				if (this.RestoreFinishedEvent != null)
				{
					this.RestoreFinishedEvent();
				}
			});
		}
		else
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		UnityEngine.Debug.Log("OnInitialized: PASS");
		Purchaser.m_StoreController = controller;
		Purchaser.m_StoreExtensionProvider = extensions;
		if (this.InitializedEvent != null)
		{
			this.InitializedEvent();
		}
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		if (string.Equals(args.purchasedProduct.definition.id, Purchaser.kProductIDNonConsumable, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			if (this.PurchaseFinishedEvent != null)
			{
				this.PurchaseFinishedEvent();
			}
		}
		else
		{
			UnityEngine.Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		UnityEngine.Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}

	public string GetProductId()
	{
		string empty = string.Empty;
		return this.noAdProductAndroid;
	}
}
