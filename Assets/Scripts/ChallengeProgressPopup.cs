using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeProgressPopup : MonoBehaviour
{
	[SerializeField]
	private UIElementGroup _uiElementGroup;

	[SerializeField]
	private GameObject _challengeIconImage;

	[SerializeField]
	private GameObject _challengeDescription;

	[SerializeField]
	private GameObject _challengeProgress;

	[SerializeField]
	private GameObject _challengeProgressText;

	[SerializeField]
	private GameObject _skinsPopup;

	public void InitializePopup(ChallengeItem item)
	{
		Image component = this._challengeIconImage.GetComponent<Image>();
		component.sprite = item.challengeIcon;
		Text component2 = this._challengeDescription.GetComponent<Text>();
		component2.text = item.itemDescription;
		Slider component3 = this._challengeProgress.GetComponent<Slider>();
		component3.value = (float)item.currentProgress / (float)item.maxValue;
		Text component4 = this._challengeProgressText.GetComponent<Text>();
		component4.text = string.Format("{0}/{1}", item.currentProgress, item.maxValue);
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
		this._uiElementGroup.Show();
		BackButton.listeners.Add(new Action(this.OnCloseChallengeProgressPopup));
	}

	public void OnCloseChallengeProgressPopup()
	{
		this._uiElementGroup.HideFinishEvent += new Action(this.OnHideFinish);
		this._uiElementGroup.Hide();
		BackButton.RemoveLast();
	}

	private void OnHideFinish()
	{
		base.gameObject.SetActive(false);
	}
}
