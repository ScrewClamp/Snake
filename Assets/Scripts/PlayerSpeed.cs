using System;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
	public BlockParticleManager blockParticleManager;

	[Range(2f, 12.7f)]
	public float maxSpeed = 5f;

	[Range(2f, 12.7f)]
	public float initialSpeed = 3f;

	[Range(0f, 5f)]
	public float defaultAcceleration = 0.01f;

	public float timeWithInertia = 0.033f;

	public float decelerationDuration = 0.3f;

	private float _speed;

	private float _acceleration;

	private float _elapsedTime;

	[HideInInspector]
	public bool _isSpeedWithInertiaStarted;

	private float _speedWithInertiaElapsedTime;

	[HideInInspector]
	public bool _isSpeedDecelerarionStarted;

	private float _decelerationElapsedTime;

	private float _deceleration;

	private int _comboCountFactor;

	private float _relativeSpeed;

	private PlayerController _playerController;

	private void Start()
	{
		this._playerController = base.GetComponent<PlayerController>();
	}

	public void SetDefaultAcceleration(float acceleration)
	{
		this._elapsedTime = 0f;
		this.defaultAcceleration = acceleration;
	}

	private void Update()
	{
		if (!this._playerController.isTransitionStarted && !this._isSpeedWithInertiaStarted && !this._isSpeedDecelerarionStarted)
		{
			this._elapsedTime += Time.deltaTime;
		}
		this.CheckSpeedWithInertia();
		this.CheckSpeedDeceleration();
	}

	public void CalculateSpeed()
	{
		this._speed = this.initialSpeed + this._acceleration * this._elapsedTime;
		this._speed = Mathf.Min(this.maxSpeed, this._speed) + this._relativeSpeed;
	}

	public float GetSpeed()
	{
		return this._speed;
	}

	public void SetSpeed(float speed)
	{
		this._isSpeedWithInertiaStarted = false;
		this._isSpeedDecelerarionStarted = false;
		this._speed = speed;
	}

	public void ResetSpeed()
	{
		this.SetSpeed(this.initialSpeed);
	}

	public void SetAcceleration(float acceleration)
	{
		this._elapsedTime = 0f;
		this._acceleration = acceleration;
	}

	public void ResetAcceleration()
	{
		this.SetAcceleration(this.defaultAcceleration);
	}

	public void StartSpeedWithInertia(float newSpeed)
	{
		this._speed = newSpeed;
		this._isSpeedWithInertiaStarted = true;
		this._speedWithInertiaElapsedTime = 0f;
	}

	public void SetRelativeSpeed(float relativeSpeed)
	{
		this._relativeSpeed = relativeSpeed;
		this.CalculateSpeed();
	}

	private void CheckSpeedWithInertia()
	{
		if (this._isSpeedWithInertiaStarted)
		{
			this._speedWithInertiaElapsedTime += Time.deltaTime;
			if (this._speedWithInertiaElapsedTime > this.timeWithInertia)
			{
				this._isSpeedWithInertiaStarted = false;
				this._speedWithInertiaElapsedTime = 0f;
				this.StartSpeedAcceleration();
			}
		}
	}

	public float GetDefaultSpeed()
	{
		float b = this.initialSpeed + this._acceleration * this._elapsedTime;
		return Mathf.Min(this.maxSpeed, b) + this._relativeSpeed;
	}

	private void StartSpeedAcceleration()
	{
		this._comboCountFactor = this.blockParticleManager.ComboCount + 1;
		float num = this.initialSpeed + this._acceleration * this._elapsedTime;
		this._deceleration = (num - this._speed) / (this.decelerationDuration * Mathf.Max(1f, (float)this._comboCountFactor * 0.5f));
		this._isSpeedDecelerarionStarted = true;
		this._decelerationElapsedTime = 0f;
	}

	private void CheckSpeedDeceleration()
	{
		if (this._isSpeedDecelerarionStarted)
		{
			this._decelerationElapsedTime += Time.deltaTime;
			this._speed += this._deceleration * Time.deltaTime;
			if ((this._deceleration < 0f && this._speed < this.initialSpeed) || (this._deceleration > 0f && this._speed > this.initialSpeed))
			{
				this._speed = this.initialSpeed;
				this._isSpeedDecelerarionStarted = false;
				this._decelerationElapsedTime = 0f;
				this._deceleration = 0f;
				this._comboCountFactor = 1;
			}
		}
	}

	public void ResetSpeedWithInertia()
	{
		this._isSpeedWithInertiaStarted = false;
		this._speedWithInertiaElapsedTime = 0f;
		this._isSpeedDecelerarionStarted = false;
		this._decelerationElapsedTime = 0f;
		this._deceleration = 0f;
		this._comboCountFactor = 1;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(base.transform.position, base.transform.position + new Vector3(0f, this.GetSpeed()));
	}
}
