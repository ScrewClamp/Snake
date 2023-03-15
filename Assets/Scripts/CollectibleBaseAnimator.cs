using DG.Tweening;
using System;
using UnityEngine;

public class CollectibleBaseAnimator : MonoBehaviour
{
	public Vector3 scale;

	public float duration;

	private Vector3 defaultScale;

	private void Animate()
	{
		Tweener t = base.transform.DOScale(this.scale, this.duration);
		t.SetEase(Ease.InQuad);
		t.OnComplete(delegate
		{
			base.transform.DOScale(this.defaultScale, this.duration).SetEase(Ease.OutQuad).OnComplete(new TweenCallback(this.Animate));
		});
	}

	public void StartAnim()
	{
		this.defaultScale = base.transform.localScale;
		this.Animate();
	}
}
