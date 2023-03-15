using System;
using UnityEngine;

public class FloatingCircularBlock : MonoBehaviour
{
	public float rotationSpeed = 0.4f;

	private float _yaw;

	private float _pitch;

	private float _roll;

	private float alpha;

	private void Update()
	{
		this.alpha += 2f;
		Vector3 vector;
		vector.x = Mathf.Sin(this.alpha * 0.0174532924f);
		vector.y = Mathf.Cos(this.alpha * 0.0174532924f);
		vector.z = 0f;
		this._yaw += this.rotationSpeed * vector.x;
		this._pitch += this.rotationSpeed * vector.y;
		this._roll += this.rotationSpeed * vector.z;
		base.transform.rotation = Quaternion.Euler(this._yaw - 10f, this._pitch, this._roll);
	}
}
