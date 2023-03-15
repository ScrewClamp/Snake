using System;

public class IOSPlatform : IPlatformInterface
{
	private string m_appleId;

	private IOSRateUsPopUp m_dialog;

	private NativeDelegateResponse _callback;

	public IOSPlatform(string appleId)
	{
		this.m_appleId = appleId;
	}

	public void ShowRateUsPopup(string title, string message)
	{
		iOSReviewRequest.Request();
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
		IOSNative.RedirectToAppStoreRatingPage(this.m_appleId);
	}

	public void SetNativeDelegateResponseCallback(NativeDelegateResponse callback)
	{
		if (callback != null)
		{
			this._callback = callback;
		}
	}
}
