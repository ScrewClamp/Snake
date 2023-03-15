using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VibrationManager : MonoBehaviour
{
	public Toggle vibrationToggleButton;

	private void Start()
	{
		bool isOn = PlayerPrefs.GetInt("IsVibrationOn", 1) == 1;
		this.vibrationToggleButton.isOn = isOn;
		this.vibrationToggleButton.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleChanged));
	}

	private void OnToggleChanged(bool isToggleOn)
	{
		PlayerPrefs.SetInt("IsVibrationOn", (!isToggleOn) ? 0 : 1);
		this.VibrateOnClick();
	}

	private void Vibrate(long miliseconds)
	{
		if (this.vibrationToggleButton.isOn)
		{
			//Vibration.Vibrate(miliseconds);
		}
	}

	public void VibrateOnClick()
	{
		this.Vibrate(200L);
	}

	public void VibrateOnBlockHit()
	{
		this.Vibrate(20L);
	}

	public void VibrateOnGameOver()
	{
		this.Vibrate(300L);
	}
}
