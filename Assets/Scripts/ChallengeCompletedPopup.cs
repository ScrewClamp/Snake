using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeCompletedPopup : MonoBehaviour
{
	[SerializeField]
	private GameObject _challengeIconImage;

	[SerializeField]
	private GameObject _challengeDescription;

	[SerializeField]
	private SkinsPopup _skinsPopup;

	[SerializeField]
	private UIElementGroup _uiElementGroup;

	public void InitializePopup(ChallengeItem item)
	{
		Image component = this._challengeIconImage.GetComponent<Image>();
		component.sprite = item.challengeIcon;
		Text component2 = this._challengeDescription.GetComponent<Text>();
		component2.text = item.itemDescription;
	}

	public void Show()
	{
		BackButton.listeners.Add(new Action(this.OnOkButtonClick));
		base.gameObject.SetActive(true);
		this._uiElementGroup.ShowFinishEvent += new Action(this.OnShowFinish);
		this._uiElementGroup.Show();
	}

	private void OnShowFinish()
	{
		this._uiElementGroup.ShowFinishEvent -= new Action(this.OnShowFinish);
		Time.timeScale = 0f;
	}

	public void OnOkButtonClick()
	{
		this._uiElementGroup.HideFinishEvent += new Action(this.OnHideFinish);
		this._uiElementGroup.Hide();
		BackButton.RemoveLast();
	}

	private void OnHideFinish()
	{
		this._uiElementGroup.HideFinishEvent -= new Action(this.OnHideFinish);
		base.gameObject.SetActive(false);
		this._skinsPopup.ShowAvailableNewSkin();
	}
}
