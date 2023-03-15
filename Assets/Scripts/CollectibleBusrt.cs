using DG.Tweening;
using System;
using UnityEngine;

public class CollectibleBusrt : MonoBehaviour
{
	public float duration;

	private void Start()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		component.material.DOFade(0f, this.duration).OnComplete(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}
}
