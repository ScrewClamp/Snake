using DG.Tweening;
using System;
using UnityEngine;

public class SkinSelectorAnimation : MonoBehaviour
{
	public Vector3 scale;

	public float duration;

	private Vector3 defaultScale;

	private Sequence sequence;

	private void Start()
	{
		this.defaultScale = base.transform.localScale;
	}

	public void StopAnimating()
	{
		this.sequence.Kill(false);
		this.sequence = null;
	}

	public void Animate()
	{
		Tweener t = base.transform.DOScale(this.scale, this.duration);
		t.SetEase(Ease.InQuad);
		t.SetUpdate(UpdateType.Normal, true);
		Tweener t2 = base.transform.DOScale(this.defaultScale, this.duration).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Normal, true);
		this.sequence = DOTween.Sequence();
		this.sequence.Append(t);
		this.sequence.Append(t2);
		this.sequence.SetLoops(-1, LoopType.Restart);
		this.sequence.SetUpdate(UpdateType.Normal, true);
	}
}
