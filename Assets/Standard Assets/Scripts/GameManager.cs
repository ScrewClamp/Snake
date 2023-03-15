using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using UDPEditor;
using UnityEngine;
using UnityEngine.UDP;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public class InitListener : IInitListener
	{
		public void OnInitialized(UserInfo userInfo)
		{
			UnityEngine.Debug.Log("[Game]On Initialized suceeded");
			GameManager.Show("Initialize succeeded", false);
			GameManager._initialized = true;
		}

		public void OnInitializeFailed(string message)
		{
			UnityEngine.Debug.Log("[Game]OnInitializeFailed: " + message);
			GameManager.Show("Initialize Failed: " + message, false);
		}
	}

	public class PurchaseListener : IPurchaseListener
	{
		public void OnPurchase(PurchaseInfo purchaseInfo)
		{
			string message = string.Format("[Game] Purchase Succeeded, productId: {0}, cpOrderId: {1}, developerPayload: {2}, storeJson: {3}", new object[]
			{
				purchaseInfo.ProductId,
				purchaseInfo.GameOrderId,
				purchaseInfo.DeveloperPayload,
				purchaseInfo.StorePurchaseJsonString
			});
			UnityEngine.Debug.Log(message);
			GameManager.Show(message, false);
			if (GameManager.m_consumeOnPurchase)
			{
				UnityEngine.Debug.Log("Consuming");
				StoreService.ConsumePurchase(purchaseInfo, this);
			}
		}

		public void OnPurchaseFailed(string message, PurchaseInfo purchaseInfo)
		{
			UnityEngine.Debug.Log("Purchase Failed: " + message);
			GameManager.Show("Purchase Failed: " + message, false);
		}

		public void OnPurchaseRepeated(string productCode)
		{
			throw new NotImplementedException();
		}

		public void OnPurchaseConsume(PurchaseInfo purchaseInfo)
		{
			GameManager.Show("Consume success for " + purchaseInfo.ProductId, true);
			UnityEngine.Debug.Log("Consume success: " + purchaseInfo.ProductId);
		}

		public void OnMultiPurchaseConsume(List<bool> successful, List<PurchaseInfo> purchaseInfos, List<string> messages)
		{
			int count = successful.Count;
			for (int i = 0; i < count; i++)
			{
				if (successful[i])
				{
					string message = string.Format("Consuming succeeded for {0}\n", purchaseInfos[i].ProductId);
					GameManager.Show(message, true);
					UnityEngine.Debug.Log(message);
				}
				else
				{
					string message = string.Format("Consuming failed for {0}, reason: {1}", purchaseInfos[i].ProductId, messages[i]);
					GameManager.Show(message, true);
					UnityEngine.Debug.Log(message);
				}
			}
		}

		public void OnPurchaseConsumeFailed(string message, PurchaseInfo purchaseInfo)
		{
			UnityEngine.Debug.Log("Consume Failed: " + message);
			GameManager.Show("Consume Failed: " + message, false);
		}

		public void OnQueryInventory(Inventory inventory)
		{
			UnityEngine.Debug.Log("OnQueryInventory");
			UnityEngine.Debug.Log("[Game] Product List: ");
			string text = "Product List: \n";
			foreach (KeyValuePair<string, ProductInfo> current in inventory.GetProductDictionary())
			{
				UnityEngine.Debug.Log("[Game] Returned product: " + current.Key + " " + current.Value.ProductId);
				text += string.Format("{0}:\n\tTitle: {1}\n\tDescription: {2}\n\tConsumable: {3}\n\tPrice: {4}\n\tCurrency: {5}\n\tPriceAmountMicros: {6}\n\tItemType: {7}\n", new object[]
				{
					current.Key,
					current.Value.Title,
					current.Value.Description,
					current.Value.Consumable,
					current.Value.Price,
					current.Value.Currency,
					current.Value.PriceAmountMicros,
					current.Value.ItemType
				});
			}
			text += "\nPurchase List: \n";
			foreach (KeyValuePair<string, PurchaseInfo> current2 in inventory.GetPurchaseDictionary())
			{
				UnityEngine.Debug.Log("[Game] Returned purchase: " + current2.Key);
				text += string.Format("{0}\n", current2.Value.ProductId);
			}
			GameManager.Show(text, false);
			if (GameManager._consumeOnQuery)
			{
				StoreService.ConsumePurchase(inventory.GetPurchaseList(), this);
			}
		}

		public void OnQueryInventoryFailed(string message)
		{
			UnityEngine.Debug.Log("OnQueryInventory Failed: " + message);
			GameManager.Show("QueryInventory Failed: " + message, false);
		}
	}

	private sealed class _InitUI_c__AnonStorey0
	{
		internal List<string> productIds;

		internal GameManager _this;

		internal void __m__0()
		{
			GameManager._initialized = false;
			UnityEngine.Debug.Log("Init button is clicked.");
			GameManager.Show("Initializing", false);
			StoreService.Initialize(this._this.initListener, null);
		}

		internal void __m__1()
		{
			if (!GameManager._initialized)
			{
				GameManager.Show("Please Initialize first", false);
				return;
			}
			string text = this._this._dropdown.options[this._this._dropdown.value].text;
			UnityEngine.Debug.Log("Buy button is clicked.");
			GameManager.Show("Buying Product: " + text, false);
			GameManager.m_consumeOnPurchase = false;
			UnityEngine.Debug.Log(this._this._dropdown.options[this._this._dropdown.value].text + " will be bought");
			StoreService.Purchase(text, null, "{\"AnyKeyYouWant:\" : \"AnyValueYouWant\"}", this._this.purchaseListener);
		}

		internal void __m__2()
		{
			if (!GameManager._initialized)
			{
				GameManager.Show("Please Initialize first", false);
				return;
			}
			string text = this._this._dropdown.options[this._this._dropdown.value].text;
			GameManager.Show("Buying Product: " + text, false);
			UnityEngine.Debug.Log("Buy&Consume button is clicked.");
			GameManager.m_consumeOnPurchase = true;
			StoreService.Purchase(text, null, "buy and consume developer payload", this._this.purchaseListener);
		}

		internal void __m__3()
		{
			if (!GameManager._initialized)
			{
				GameManager.Show("Please Initialize first", false);
				return;
			}
			GameManager._consumeOnQuery = false;
			UnityEngine.Debug.Log("Query button is clicked.");
			GameManager.Show("Querying Inventory", false);
			StoreService.QueryInventory(this.productIds, this._this.purchaseListener);
		}

		internal void __m__4()
		{
			if (!GameManager._initialized)
			{
				GameManager.Show("Please Initialize first", false);
				return;
			}
			GameManager._consumeOnQuery = true;
			GameManager.Show("Querying Inventory", false);
			UnityEngine.Debug.Log("QueryConsume button is clicked.");
			StoreService.QueryInventory(this.productIds, this._this.purchaseListener);
		}
	}

	public string Product1;

	public string Product2;

	private static bool m_consumeOnPurchase;

	private static bool _consumeOnQuery;

	private Dropdown _dropdown;

	private List<Dropdown.OptionData> options;

	private static Text _textField;

	private static bool _initialized;

	private GameManager.PurchaseListener purchaseListener;

	private GameManager.InitListener initListener;

	private AppInfo appInfo;

	private void Start()
	{
		this.purchaseListener = new GameManager.PurchaseListener();
		this.initListener = new GameManager.InitListener();
		this.appInfo = new AppInfo();
		AppStoreSettings appStoreSettings = Resources.Load<AppStoreSettings>("GameSettings");
		this.appInfo.AppSlug = appStoreSettings.AppSlug;
		this.appInfo.ClientId = appStoreSettings.UnityClientID;
		this.appInfo.ClientKey = appStoreSettings.UnityClientKey;
		this.appInfo.RSAPublicKey = appStoreSettings.UnityClientRSAPublicKey;
		UnityEngine.Debug.Log("App Name: " + appStoreSettings.AppName);
		GameObject gameObject = GameObject.Find("Information");
		GameManager._textField = gameObject.GetComponent<Text>();
		GameManager._textField.text = "Please Click Init to Start";
		gameObject = GameObject.Find("Dropdown");
		this._dropdown = gameObject.GetComponent<Dropdown>();
		this._dropdown.ClearOptions();
		this._dropdown.options.Add(new Dropdown.OptionData(this.Product1));
		this._dropdown.options.Add(new Dropdown.OptionData(this.Product2));
		this._dropdown.RefreshShownValue();
		this.InitUI();
	}

	private static void Show(string message, bool append = false)
	{
		GameManager._textField.text = ((!append) ? message : string.Format("{0}\n{1}", GameManager._textField.text, message));
	}

	private void InitUI()
	{
		this.GetButton("InitButton").onClick.AddListener(delegate
		{
			GameManager._initialized = false;
			UnityEngine.Debug.Log("Init button is clicked.");
			GameManager.Show("Initializing", false);
			StoreService.Initialize(this.initListener, null);
		});
		this.GetButton("BuyButton").onClick.AddListener(delegate
		{
			if (!GameManager._initialized)
			{
				GameManager.Show("Please Initialize first", false);
				return;
			}
			string text = this._dropdown.options[this._dropdown.value].text;
			UnityEngine.Debug.Log("Buy button is clicked.");
			GameManager.Show("Buying Product: " + text, false);
			GameManager.m_consumeOnPurchase = false;
			UnityEngine.Debug.Log(this._dropdown.options[this._dropdown.value].text + " will be bought");
			StoreService.Purchase(text, null, "{\"AnyKeyYouWant:\" : \"AnyValueYouWant\"}", this.purchaseListener);
		});
		this.GetButton("BuyConsumeButton").onClick.AddListener(delegate
		{
			if (!GameManager._initialized)
			{
				GameManager.Show("Please Initialize first", false);
				return;
			}
			string text = this._dropdown.options[this._dropdown.value].text;
			GameManager.Show("Buying Product: " + text, false);
			UnityEngine.Debug.Log("Buy&Consume button is clicked.");
			GameManager.m_consumeOnPurchase = true;
			StoreService.Purchase(text, null, "buy and consume developer payload", this.purchaseListener);
		});
		List<string> productIds = new List<string>
		{
			this.Product1,
			this.Product2
		};
		this.GetButton("QueryButton").onClick.AddListener(delegate
		{
			if (!GameManager._initialized)
			{
				GameManager.Show("Please Initialize first", false);
				return;
			}
			GameManager._consumeOnQuery = false;
			UnityEngine.Debug.Log("Query button is clicked.");
			GameManager.Show("Querying Inventory", false);
			StoreService.QueryInventory(productIds, this.purchaseListener);
		});
		this.GetButton("QueryConsumeButton").onClick.AddListener(delegate
		{
			if (!GameManager._initialized)
			{
				GameManager.Show("Please Initialize first", false);
				return;
			}
			GameManager._consumeOnQuery = true;
			GameManager.Show("Querying Inventory", false);
			UnityEngine.Debug.Log("QueryConsume button is clicked.");
			StoreService.QueryInventory(productIds, this.purchaseListener);
		});
	}

	private Button GetButton(string buttonName)
	{
		GameObject gameObject = GameObject.Find(buttonName);
		if (gameObject != null)
		{
			return gameObject.GetComponent<Button>();
		}
		return null;
	}
}
