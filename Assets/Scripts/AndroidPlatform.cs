using System;

public class AndroidPlatform : IPlatformInterface
{
	private string m_appLink;

	private AndroidRateUsPopUp m_dialog;

	private NativeDelegateResponse _callback;

	public AndroidPlatform(string appId)
	{
		this.m_appLink = string.Format("market://details?id={0}", appId);
	}

	public void ShowRateUsPopup(string title, string message)
	{
		this.m_dialog = AndroidRateUsPopUp.Create(title, message);
		this.m_dialog.onRateUSPopupComplete += new AndroidRateUsPopUp.OnRateUSPopupComplete(this.OnShowRateUsComplete);
	}

	private void OnShowRateUsComplete(RateInfo state)
	{
		if (state == RateInfo.RATED)
		{
			this.OpenAppRatingPage();
		}
		if (this._callback != null)
		{
			this._callback(state);
		}
	}

	public void OpenAppRatingPage()
	{
		AndroidNative.RedirectToAppStoreRatingPage(this.m_appLink);
	}

	public void SetNativeDelegateResponseCallback(NativeDelegateResponse callback)
	{
		if (callback != null)
		{
			this._callback = callback;
		}
	}
}
