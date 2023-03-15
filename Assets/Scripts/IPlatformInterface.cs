using System;

public interface IPlatformInterface
{
	void ShowRateUsPopup(string title, string message);

	void OpenAppRatingPage();

	void SetNativeDelegateResponseCallback(NativeDelegateResponse callback);
}
