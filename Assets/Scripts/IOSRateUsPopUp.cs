using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class IOSRateUsPopUp : MonoBehaviour
{
	public delegate void OnRateUSPopupComplete(RateInfo state);



	public string title;

	public string message;

	public string rate;

	public string remind;

	public string declined;

	public event IOSRateUsPopUp.OnRateUSPopupComplete onRateUSPopupComplete;

	private void RaiseOnOnRateUSPopupComplete(RateInfo state)
	{
		if (this.onRateUSPopupComplete != null)
		{
			this.onRateUSPopupComplete(state);
		}
	}

	public static IOSRateUsPopUp Create()
	{
		return IOSRateUsPopUp.Create("Like the Game?", "Rate US");
	}

	public static IOSRateUsPopUp Create(string title, string message)
	{
		return IOSRateUsPopUp.Create(title, message, "Rate!", "Remind me later", "No, thanks");
	}

	public static IOSRateUsPopUp Create(string title, string message, string rate, string remind, string declined)
	{
		IOSRateUsPopUp iOSRateUsPopUp = new GameObject("IOSRateUsPopUp").AddComponent<IOSRateUsPopUp>();
		iOSRateUsPopUp.title = title;
		iOSRateUsPopUp.message = message;
		iOSRateUsPopUp.rate = rate;
		iOSRateUsPopUp.remind = remind;
		iOSRateUsPopUp.declined = declined;
		iOSRateUsPopUp.init();
		return iOSRateUsPopUp;
	}

	public void init()
	{
		IOSNative.showRateUsPopUP(this.title, this.message, this.rate, this.remind, this.declined);
	}

	public void OnRatePopUpCallBack(string buttonIndex)
	{
		int num = (int)Convert.ToInt16(buttonIndex);
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					this.RaiseOnOnRateUSPopupComplete(RateInfo.DECLINED);
				}
			}
			else
			{
				this.RaiseOnOnRateUSPopupComplete(RateInfo.REMIND);
			}
		}
		else
		{
			this.RaiseOnOnRateUSPopupComplete(RateInfo.RATED);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
