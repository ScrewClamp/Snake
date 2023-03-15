using DG.Tweening;
using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float rotationAngle;

	private CameraSpeed _cameraSpeed;

	private void Awake()
	{
		this._cameraSpeed = base.GetComponent<CameraSpeed>();
	}

	private void Update()
	{
		float currentSpeed = this._cameraSpeed.GetCurrentSpeed();
		Vector3 b = currentSpeed * Vector3.up * Time.deltaTime;
		base.transform.position = base.transform.position + b;
	}

	public float GetDefaultSpeed()
	{
		return this._cameraSpeed.InitialSpeed;
	}

	public void RotateCameraLeft()
	{
		base.transform.DORotate(new Vector3(0f, 0f, this.rotationAngle), 0.6f, RotateMode.Fast).SetEase(Ease.OutCubic);
	}

	public void RotateCameraRight()
	{
		base.transform.DORotate(new Vector3(0f, 0f, -this.rotationAngle), 0.6f, RotateMode.Fast).SetEase(Ease.OutCubic);
	}
}
