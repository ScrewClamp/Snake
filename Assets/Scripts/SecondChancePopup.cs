using DG.Tweening;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SecondChancePopup : MonoBehaviour
{






	public Image buttonImage;

	public float duration;

	private Tweener _tweener;

	public UIElementGroup uiElementGroup;

	public event Action WatchVideoClickedEvent;

	public event Action BackClickedEvent;

	public event Action HideFinishedEvent;

	public void WatchVideoClicked()
	{
		this._tweener.Kill(false);
		if (this.WatchVideoClickedEvent != null)
		{
			this.WatchVideoClickedEvent();
		}
	}

	public void BackClicked()
	{
		this._tweener.Kill(false);
		if (this.BackClickedEvent != null)
		{
			this.BackClickedEvent();
		}
	}

	public void Show()
	{
		this.buttonImage.fillAmount = 1f;
		this._tweener = this.buttonImage.DOFillAmount(0f, this.duration).OnComplete(new TweenCallback(this.OnTimerComplete)).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
		base.gameObject.SetActive(true);
		this.uiElementGroup.Show();
		BackButton.listeners.Add(new Action(this.BackClicked));
	}

	private void OnTimerComplete()
	{
		this.BackClicked();
	}

	public void Hide()
	{
		base.gameObject.SetActive(true);
		this.uiElementGroup.Hide();
		BackButton.RemoveLast();
		this.uiElementGroup.HideFinishEvent += new Action(this.OnHideFinish);
	}

	private void OnHideFinish()
	{
		this.uiElementGroup.HideFinishEvent -= new Action(this.OnHideFinish);
		base.gameObject.SetActive(false);
		if (this.HideFinishedEvent != null)
		{
			this.HideFinishedEvent();
		}
	}
}
