using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleButtonSpriteChanger : MonoBehaviour
{
	public Toggle toggleButton;

	public Image target;

	public Sprite onState;

	public Sprite offState;

	private void Start()
	{
		bool isOn = this.toggleButton.isOn;
		this.toggleButton.onValueChanged.AddListener(new UnityAction<bool>(this.ChangeState));
		this.ChangeState(isOn);
	}

	private void ChangeState(bool isOn)
	{
		this.target.sprite = ((!isOn) ? this.offState : this.onState);
	}
}
