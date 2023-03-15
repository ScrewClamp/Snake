using System;
using UnityEngine;

public class RateUsManager : MonoBehaviour
{
	public int levelUpCountToShowRate = 7;

	public LevelManager levelManager;

	private RateUsComponent _rateUsComponent;

	private void Start()
	{
		this._rateUsComponent = base.GetComponent<RateUsComponent>();
		this.levelManager.LevelUpEvent += new Action<int>(this.LevelManagerOnLevelUpEvent);
	}

	private void OnRateUsCallback(RateInfo state)
	{
		if (state == RateInfo.DECLINED || state == RateInfo.RATED)
		{
			PlayerPrefs.SetInt("ShowRateUs", 0);
		}
	}

	private void LevelManagerOnLevelUpEvent(int levelIndex)
	{
		bool flag = PlayerPrefs.GetInt("ShowRateUs", 1) == 1;
		bool flag2 = this.levelManager.CurrentLevelIndex == 0;
		bool flag3 = this.levelManager.CurrentLevelIndex % (this.levelUpCountToShowRate - 1) == 0;
		if (!flag2 && flag3 && flag)
		{
			this._rateUsComponent.ShowRateUs();
			this._rateUsComponent.SetNativeDelegateResponseCallback(new NativeDelegateResponse(this.OnRateUsCallback));
		}
	}

	public void ShowRatePopup()
	{
		this._rateUsComponent.OpenAppRatingPage();
	}
}
