using DG.Tweening;
using System;
using UnityEngine;

public class ParticleBgAnimator : MonoBehaviour
{
	public float rotateZAngle = 10f;

	public float fadeDuration;

	private SpriteRenderer _shadowSprite;

	private void Start()
	{
		this._shadowSprite = base.GetComponent<SpriteRenderer>();
		this._shadowSprite.DOFade(0f, this.fadeDuration).SetEase(Ease.InQuad);
		base.transform.DOScale(base.transform.localScale * 1.2f, this.fadeDuration);
		base.transform.DOLocalRotate(new Vector3(0f, 0f, this.rotateZAngle), this.fadeDuration, RotateMode.Fast);
	}
}
