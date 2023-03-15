using System;
using UnityEngine;

public class CameraSpeed : MonoBehaviour
{
	public GameObject player;

	public AnimationCurve speedCurveWhenPlayerIsToLose;

	public AnimationCurve speedCurveWhenPlayerPassesBlocks;

	private PlayerController _playerController;

	private PlayerSpeed _playerSpeed;

	[Range(2.5f, 13f), SerializeField]
	private float _initialSpeed;

	[Range(2.5f, 13f)]
	public float cameraMaxSpeed;

	[SerializeField]
	private float _playerTransitionDuration;

	private float _elapsedTimeOnPlayerTransition;

	private bool _isSpeedingDownAfterTransition;

	private float _prevSpeed;

	private float _cameraKillTime;

	private bool _smoothSlowDown;

	private bool _isAutoBreakerEnabled;

	[SerializeField]
	private float _cameraOffset = 1f;

	public float InitialSpeed
	{
		get
		{
			return this._initialSpeed;
		}
		set
		{
			this._initialSpeed = value;
		}
	}

	private void Start()
	{
		this._playerController = this.player.GetComponent<PlayerController>();
		this._playerSpeed = this.player.GetComponent<PlayerSpeed>();
		this._prevSpeed = this._playerSpeed.initialSpeed;
	}

	public void SetCameraKillTime(float cameraKillTime)
	{
		this._cameraKillTime = cameraKillTime;
	}

	private void Update()
	{
		this.CheckPlayerTransition();
	}

	private void CheckPlayerTransition()
	{
		if (this._playerController.isTransitionStarted)
		{
			this._isSpeedingDownAfterTransition = true;
		}
		if (this._isSpeedingDownAfterTransition)
		{
			this._elapsedTimeOnPlayerTransition += Time.deltaTime;
			this._isSpeedingDownAfterTransition = (this._elapsedTimeOnPlayerTransition <= this._playerTransitionDuration);
		}
		else
		{
			this._elapsedTimeOnPlayerTransition = 0f;
		}
	}

	public float CameraHalfHeight()
	{
		return -Camera.main.transform.position.z * Mathf.Tan(Camera.main.fieldOfView * 0.5f * 0.0174532924f);
	}

	public float GetCurrentSpeed()
	{
		if (Time.deltaTime < Mathf.Epsilon)
		{
			return this._prevSpeed;
		}
		if (this._isAutoBreakerEnabled)
		{
			float num = Mathf.Lerp(base.transform.position.y, this._playerSpeed.transform.position.y, 0.1f);
			if (num > base.transform.position.y)
			{
				this._prevSpeed = (num - base.transform.position.y) / Time.deltaTime;
				return this._prevSpeed;
			}
			return this._prevSpeed;
		}
		else
		{
			if (base.transform.position.y + this._cameraOffset > this.player.transform.position.y)
			{
				float num2 = this.CalculateCameraSpeed();
				if (this._smoothSlowDown && this._prevSpeed - num2 > Mathf.Epsilon)
				{
					num2 = Mathf.Lerp(num2, this._prevSpeed, 0.9f);
				}
				else
				{
					this._smoothSlowDown = false;
				}
				this._prevSpeed = num2;
				return num2;
			}
			float maxSpeed = (!this._playerController.isTransitionStarted) ? this.cameraMaxSpeed : 40f;
			float num3 = Mathf.SmoothDamp(base.transform.position.y, this.player.transform.position.y + 17f, ref this._prevSpeed, 1f, maxSpeed);
			float num4 = num3 - base.transform.position.y;
			float num5 = num4 / Time.deltaTime;
			this._prevSpeed = num5;
			this._smoothSlowDown = true;
			return num5;
		}
	}

	private float CalculateCameraSpeed()
	{
		float defaultSpeed = this._playerSpeed.GetDefaultSpeed();
		float num = this.CameraHalfHeight();
		float num2 = num / this._cameraKillTime;
		float speedFactorWhenPlayerLoosing = this.GetSpeedFactorWhenPlayerLoosing();
		float speedFactorWhenPlayerPassingBlocks = this.GetSpeedFactorWhenPlayerPassingBlocks();
		float num3 = (!this._isSpeedingDownAfterTransition) ? speedFactorWhenPlayerLoosing : speedFactorWhenPlayerPassingBlocks;
		return defaultSpeed + num3 * num2;
	}

	public void SetAutoBreakerEnabled(bool isEnabled)
	{
		this._isAutoBreakerEnabled = isEnabled;
	}

	private float GetSpeedFactorWhenPlayerPassingBlocks()
	{
		float result = 1f;
		if (this._isSpeedingDownAfterTransition)
		{
			float time = this._elapsedTimeOnPlayerTransition / this._playerTransitionDuration;
			result = this.speedCurveWhenPlayerPassesBlocks.Evaluate(time);
		}
		return result;
	}

	private float GetSpeedFactorWhenPlayerLoosing()
	{
		float num = this.CameraHalfHeight();
		float b = base.transform.position.y - this.player.transform.position.y;
		float num2 = Mathf.Max(0f, b);
		float time = num2 / num;
		return this.speedCurveWhenPlayerIsToLose.Evaluate(time);
	}
}
