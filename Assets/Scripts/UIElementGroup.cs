using DG.Tweening;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIElementGroup : MonoBehaviour
{




	[SerializeField]
	private CanvasGroup _canvasGroup;

	public Vector3 hiddenScale = Vector3.one;

	public Vector3 shownScale = Vector3.one;

	public float showDuration;

	public float hideDuration;

	public event Action ShowFinishEvent;

	public event Action HideFinishEvent;

	public void Show()
	{
		base.transform.localScale = this.hiddenScale;
		this._canvasGroup.alpha = 0f;
		base.gameObject.SetActive(true);
		bool isIndependentUpdate = true;
		this._canvasGroup.DOFade(1f, this.showDuration).SetUpdate(UpdateType.Normal, isIndependentUpdate);
		base.transform.DOScale(this.shownScale, this.showDuration).OnComplete(new TweenCallback(this.OnShowFinish)).SetUpdate(UpdateType.Normal, isIndependentUpdate);
	}

	private void OnShowFinish()
	{
		if (this.ShowFinishEvent != null)
		{
			this.ShowFinishEvent();
		}
	}

	public void Hide()
	{
		base.transform.localScale = this.shownScale;
		this._canvasGroup.alpha = 1f;
		bool isIndependentUpdate = true;
		this._canvasGroup.DOFade(0f, this.hideDuration).SetUpdate(UpdateType.Normal, isIndependentUpdate);
		base.transform.DOScale(this.hiddenScale, this.hideDuration).OnComplete(new TweenCallback(this.OnHideFinish)).SetUpdate(UpdateType.Normal, isIndependentUpdate);
	}

	private void OnHideFinish()
	{
		base.gameObject.SetActive(false);
		if (this.HideFinishEvent != null)
		{
			this.HideFinishEvent();
		}
	}
}
