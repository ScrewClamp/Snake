using DG.Tweening;
using System;
using UnityEngine;

public class ObstacleFollower : MonoBehaviour
{
	public GameObject target;

	public float obstacleOffsetX;

	public float obstacleOffsetY;

	public float cameraHolderOffsetY;

	public float fadeDuration;

	private GameObject _cameraHolder;

	private SpriteRenderer _warningSprite;

	private float _maxDiff;

	private void Start()
	{
		this._cameraHolder = Camera.main.transform.parent.gameObject;
		this._warningSprite = base.GetComponent<SpriteRenderer>();
		this._warningSprite.DOFade(0f, this.fadeDuration).SetEase(Ease.InQuad).OnComplete(new TweenCallback(this.DestroyObjectFromAnim));
	}

	private void Update()
	{
		if (base.gameObject.activeSelf && this.target != null)
		{
			Vector3 position = this.target.transform.position;
			float y = this._cameraHolder.transform.position.y;
			float num = y + this.cameraHolderOffsetY;
			bool flag = position.y < num;
			Vector3 position2;
			position2.x = ((position.x <= 0f) ? (-this.obstacleOffsetX) : this.obstacleOffsetX);
			position2.y = ((!flag) ? num : position.y) + this.obstacleOffsetY;
			position2.z = base.transform.position.z;
			base.transform.position = position2;
		}
	}

	public void DestroyObjectFromAnim()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
