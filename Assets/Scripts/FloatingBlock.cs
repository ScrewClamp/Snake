using System;
using UnityEngine;

public class FloatingBlock : MonoBehaviour
{
	public float rotationSpeed = 0.2f;

	public float rotationAroundXAxisConstraintAngle = 3f;

	public float rotationAroundYAxisConstraintAngle = 3f;

	public float rotationAroundZAxisConstraintAngle = 3f;

	private Vector3 _torqueVector;

	private float _yaw;

	private float _pitch;

	private float _roll;

	private int _inversionCount;

	private void Start()
	{
		float x = UnityEngine.Random.Range(-1f, 1f);
		float y = UnityEngine.Random.Range(-1f, 1f);
		float z = UnityEngine.Random.Range(-1f, 1f);
		Vector3 vector = new Vector3(x, y, z);
		this._torqueVector = vector.normalized;
	}

	private void Update()
	{
		if (this.IsOutsideConstraints())
		{
			this._torqueVector = -this._torqueVector;
			this._inversionCount++;
		}
		this._yaw += this.rotationSpeed * this._torqueVector.x;
		this._pitch += this.rotationSpeed * this._torqueVector.y;
		this._roll += this.rotationSpeed * this._torqueVector.z;
		base.transform.rotation = Quaternion.Euler(this._yaw, this._pitch, this._roll);
		if (this.IsNearZero() && this._inversionCount == 2)
		{
			this._inversionCount = 0;
			float x = UnityEngine.Random.Range(-1f, 1f);
			float y = UnityEngine.Random.Range(-1f, 1f);
			float z = UnityEngine.Random.Range(-1f, 1f);
			Vector3 vector = new Vector3(x, y, z);
			this._torqueVector = vector.normalized;
		}
	}

	private bool IsOutsideConstraints()
	{
		return Mathf.Abs(this._yaw) > this.rotationAroundXAxisConstraintAngle || Mathf.Abs(this._pitch) > this.rotationAroundYAxisConstraintAngle || Mathf.Abs(this._roll) > this.rotationAroundZAxisConstraintAngle;
	}

	private bool IsNearZero()
	{
		return Mathf.Abs(this._yaw) < 0.0001f && Mathf.Abs(this._pitch) < 0.0001f && Mathf.Abs(this._roll) < 0.0001f;
	}
}
