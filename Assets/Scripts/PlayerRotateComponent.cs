using System;
using UnityEngine;

public class PlayerRotateComponent : MonoBehaviour
{
	public float speedMultiplier = 200f;

	private PlayerSpeed _playerSpeed;

	private Vector3 _previousPosition;

	private bool _isInTransition;

	private float yaw;

	private float roll;

	private void Start()
	{
		GameObject gameObject = GameObject.FindWithTag("Player");
		this._playerSpeed = gameObject.GetComponent<PlayerSpeed>();
		this._previousPosition = base.transform.position - new Vector3(0f, 0.5f, 0f);
	}

	private void Update()
	{
		Vector3 vector = base.transform.position - this._previousPosition;
		Vector3 normalized = vector.normalized;
		float num = vector.magnitude * this.speedMultiplier;
		this.yaw += normalized.y * num * Time.deltaTime * 57.29578f;
		this.roll += normalized.x * num * Time.deltaTime * 57.29578f;
		base.transform.localRotation = Quaternion.Euler(this.yaw, 0f, this.roll);
		if (Mathf.Abs(normalized.x) < Mathf.Epsilon)
		{
			if (this._isInTransition)
			{
				this.ResetShape();
			}
		}
		else if (!this._isInTransition)
		{
			this.ResetShape();
		}
		this._previousPosition = base.transform.position;
	}

	private void ResetShape()
	{
		base.transform.rotation = Quaternion.identity;
		this.yaw = 0f;
		this.roll = 0f;
		this._isInTransition = !this._isInTransition;
	}
}
