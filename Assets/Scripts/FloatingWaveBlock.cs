using System;
using UnityEngine;

public class FloatingWaveBlock : MonoBehaviour
{
	[HideInInspector]
	public int blockIndex;

	public float slopingAmount = 0.5f;

	public float speedInDegrees = 7f;

	public float waveAngle = 50f;

	private float _yaw;

	private float _pitch;

	private float _roll;

	private float _alpha;

	private void Update()
	{
		if (Time.timeScale > 0f)
		{
			this._alpha += this.speedInDegrees;
			Vector3 vector;
			vector.x = Mathf.Sin(this._alpha * 0.0174532924f);
			vector.y = Mathf.Cos(this._alpha * 0.0174532924f);
			vector.z = 0f;
			this._yaw += this.slopingAmount * vector.x;
			this._pitch += this.slopingAmount * vector.y;
			this._roll += this.slopingAmount * vector.z;
			base.transform.localRotation = Quaternion.Euler(this._yaw - 5f, this._pitch, this._roll);
		}
	}

	public Vector4 GetNextRotation()
	{
		Vector4 result = new Vector4(this._yaw, this._pitch, this._roll, this._alpha - this.waveAngle);
		return result;
	}

	public void SetRotation(Vector4 rotation)
	{
		this._alpha = rotation.w;
		this._yaw = 0f;
		this._pitch = 0f;
		this._roll = 0f;
	}
}
