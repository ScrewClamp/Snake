using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tapdaq
{
	[Serializable]
	public class TDNativeAd
	{
		private delegate void LoadListener(Action<TDNativeAd> callback);

		private sealed class _LoadTexture_c__AnonStorey0
		{
			internal TDNativeAd.LoadListener loadListener;

			internal Action<TDNativeAd> onLoadCallback;

			internal TDNativeAd _this;

			internal void __m__0(Texture2D intTexture)
			{
				this._this.texture = intTexture;
				this.loadListener(this.onLoadCallback);
			}

			internal void __m__1(Texture2D intTexture)
			{
				this._this.iconTexture = intTexture;
				this.loadListener(this.onLoadCallback);
			}
		}

		public string applicationId;

		public string targetingId;

		public string subscriptionId;

		public string appName;

		public string description;

		public string buttonText;

		public string developerName;

		public string ageRating;

		public string appSize;

		public string averageReview;

		public string totalReviews;

		public string category;

		public string appVersion;

		public string price;

		public string currency;

		public string imageUrl;

		public string title;

		public string iconUrl;

		public string iconPath;

		public string uniqueId;

		public string creativeIdentifier;

		private Texture2D _texture_k__BackingField;

		private Texture2D _iconTexture_k__BackingField;

		public Texture2D texture
		{
			get;
			private set;
		}

		public Texture2D iconTexture
		{
			get;
			private set;
		}

		public void LoadTexture(Action<TDNativeAd> onLoadCallback)
		{
			this.texture = null;
			this.iconTexture = null;
			TDDebugLogger.Log("image url : " + this.imageUrl + ", iconUrl : " + this.iconUrl);
			if (string.IsNullOrEmpty(this.imageUrl) && string.IsNullOrEmpty(this.iconUrl))
			{
				onLoadCallback(null);
				return;
			}
			TDNativeAd.LoadListener loadListener = (string.IsNullOrEmpty(this.imageUrl) || string.IsNullOrEmpty(this.iconUrl)) ? new TDNativeAd.LoadListener(this.LoadTextureListener) : new TDNativeAd.LoadListener(this.LoadBothTexturesListener);
			SpriteLoader.Instance.LoadTextureAsync(this.imageUrl, delegate(Texture2D intTexture)
			{
				this.texture = intTexture;
				loadListener(onLoadCallback);
			});
			if (!string.IsNullOrEmpty(this.iconUrl))
			{
				SpriteLoader.Instance.LoadTextureAsync(this.iconUrl, delegate(Texture2D intTexture)
				{
					this.iconTexture = intTexture;
					loadListener(onLoadCallback);
				});
			}
		}

		private void LoadTextureListener(Action<TDNativeAd> callback)
		{
			TDDebugLogger.Log("LoadTextureListener called ");
			if (callback != null)
			{
				callback(this);
			}
		}

		private void LoadBothTexturesListener(Action<TDNativeAd> callback)
		{
			TDDebugLogger.Log("LoadBothTexturesListener called ");
			if (this.texture == null || this.iconTexture == null)
			{
				TDDebugLogger.Log("One of texture is missed!!! ");
				return;
			}
			this.LoadTextureListener(callback);
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static TDNativeAd CreateNativeAd(string jsonString)
		{
			return JsonConvert.DeserializeObject<TDNativeAd>(jsonString);
		}
	}
}
