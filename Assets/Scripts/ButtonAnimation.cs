using System;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
	public AnimatingButton skinsButton;

	public float newScale;

	public float newOffsetX;

	private Vector3 _originalPosition;

	private Vector3 _originalScale;

	private void Awake()
	{
		AnimatingButton expr_06 = this.skinsButton;
		expr_06.onPointerDown = (Action)Delegate.Combine(expr_06.onPointerDown, new Action(this.OnAnimationStart));
		AnimatingButton expr_2D = this.skinsButton;
		expr_2D.onPointerUp = (Action)Delegate.Combine(expr_2D.onPointerUp, new Action(this.OnAnimationEnd));
	}

	private void OnAnimationStart()
	{
		this._originalPosition = base.transform.localPosition;
		this._originalScale = base.transform.localScale;
		base.transform.localPosition = base.transform.localPosition + this.newOffsetX * Vector3.left;
		base.transform.localScale = this.newScale * base.transform.localScale;
	}

	private void OnAnimationEnd()
	{
		base.transform.localPosition = this._originalPosition;
		base.transform.localScale = this._originalScale;
	}
}
