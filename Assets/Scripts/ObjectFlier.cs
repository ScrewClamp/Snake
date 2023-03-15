using DG.Tweening;
using System;
using UnityEngine;

public class ObjectFlier : MonoBehaviour
{
	public Vector3 targetLocalPosition;

	public Vector3 targetScale;

	public void Start()
	{
		base.transform.DOLocalMoveX(this.targetLocalPosition.x, 1f, false);
		base.transform.DOLocalMoveY(this.targetLocalPosition.y, 1f, false);
		base.transform.DOScale(this.targetScale, 1f).OnComplete(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}
}
