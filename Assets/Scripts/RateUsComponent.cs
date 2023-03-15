using System;
using UnityEngine;

public class RateUsComponent : MonoBehaviour
{
	[Tooltip("For example: com.yourcompany.yourapp")]
	public string GoogleAppId = "com.test.t";

	[Tooltip("For example: 12345678")]
	public string AppleAppId = "0";

	public string title;

	public string message;

	private IPlatformInterface m_currentPlatform;

	private void Start()
	{
		this.m_currentPlatform = new AndroidPlatform(this.GoogleAppId);
	}

	public void SetNativeDelegateResponseCallback(NativeDelegateResponse callback)
	{
		if (this.m_currentPlatform != null)
		{
			this.m_currentPlatform.SetNativeDelegateResponseCallback(callback);
		}
	}

	public void OpenAppRatingPage()
	{
		this.m_currentPlatform.OpenAppRatingPage();
	}

	public void ShowRateUs()
	{
		if (this.m_currentPlatform != null)
		{
			this.m_currentPlatform.ShowRateUsPopup(this.title, this.message);
		}
		else
		{
			UnityEngine.Debug.Log("'Rate us' popup only works on Android/IOS platforms");
		}
	}
}
