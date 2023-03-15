using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinsPopup : MonoBehaviour
{
	[SerializeField]
	private UIElementGroup _uiElementGroup;

	[SerializeField]
	private MenuConfig _menuConfig;

	[SerializeField]
	private ChallengeCompletedPopup _challengeCompletedPopup;

	[SerializeField]
	private SkinSelectorAnimation _skinSelector;

	private bool disabledAfterStart;

	private Queue<ChallengeItem> _unlockedSkins = new Queue<ChallengeItem>();

	public void OnShowPopup()
	{
		this._skinSelector.Animate();
		this._unlockedSkins.Clear();
		base.gameObject.SetActive(true);
		this._uiElementGroup.ShowFinishEvent += new Action(this.OnShowFinish);
		this._uiElementGroup.Show();
		BackButton.listeners.Add(new Action(this.OnHidePopup));
	}

	private void OnShowFinish()
	{
		this._uiElementGroup.ShowFinishEvent -= new Action(this.OnShowFinish);
		Time.timeScale = 0f;
	}

	private void Update()
	{
		if (!this.disabledAfterStart)
		{
			this.InitializeSkins();
		}
	}

	private void InitializeSkins()
	{
		base.gameObject.SetActive(false);
		this.disabledAfterStart = true;
		AbstractChallengeProgress.OnItemUnlocked = (Action<ChallengeItem>)Delegate.Combine(AbstractChallengeProgress.OnItemUnlocked, new Action<ChallengeItem>(this.AddToSkinsPopupQueue));
	}

	public void OnHidePopup()
	{
		this._uiElementGroup.HideFinishEvent += new Action(this.OnHideFinish);
		this._uiElementGroup.Hide();
		Time.timeScale = 1f;
		BackButton.RemoveLast();
	}

	private void OnHideFinish()
	{
		this._uiElementGroup.HideFinishEvent -= new Action(this.OnHideFinish);
		base.gameObject.SetActive(false);
		this._menuConfig.CheckNewSkinAvailable();
		this._skinSelector.StopAnimating();
	}

	private void AddToSkinsPopupQueue(ChallengeItem item)
	{
		if (!item.isUnlockedByDefault)
		{
			this._unlockedSkins.Enqueue(item);
		}
	}

	public void ShowAvailableNewSkin()
	{
		if (this._unlockedSkins.Count > 0)
		{
			ChallengeItem item = this._unlockedSkins.Peek();
			this._unlockedSkins.Dequeue();
			this._challengeCompletedPopup.InitializePopup(item);
			this._challengeCompletedPopup.Show();
		}
		else
		{
			Time.timeScale = 1f;
		}
	}
}
