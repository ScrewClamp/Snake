using System;
using UnityEngine;

public class FollowableComponent : MonoBehaviour
{
	public float interpolationStep;

	public float distanceBetweenBodyParts;

	private Vector2[] _trail;

	private float[] _distances;

	private int _trailCount;

	private float _oldDiff;

	private int _startPosition;

	private float _sqrInterpolationStep;

	private void Awake()
	{
		int maxBodyCount = base.transform.parent.GetComponent<BodyChain>().maxBodyCount;
		int num = (int)(this.distanceBetweenBodyParts / this.interpolationStep);
		this._trailCount = num * maxBodyCount;
		this._trail = new Vector2[this._trailCount];
		this._distances = new float[this._trailCount];
		this._sqrInterpolationStep = this.interpolationStep * this.interpolationStep;
		this.CreateTrail();
	}

	private void CreateTrail()
	{
		for (int i = 0; i < this._trailCount; i++)
		{
			Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y - (float)i * this.interpolationStep);
			this._trail[i] = vector;
			this._distances[i] = (float)i * this.interpolationStep;
		}
		this._startPosition = 0;
		this._oldDiff = 0f;
	}

	private void Update()
	{
		Vector2 lastKeyPoint = this.GetLastKeyPoint();
		float sqrMagnitude = (lastKeyPoint - (Vector2)base.transform.position).sqrMagnitude;
		if (this._sqrInterpolationStep < sqrMagnitude)
		{
			this.AddNewKeyPoint(base.transform.position);
		}
	}

	public Vector3 GetPointAtDistanceFromHead(int index)
	{
		float num = (float)index * this.distanceBetweenBodyParts;
		int num2 = this._startPosition;
		int num3 = (this._startPosition + this._trailCount - 1) % this._trailCount;
		float num4 = Vector2.Distance(base.transform.position, this._trail[num2]);
		Vector3 result;
		if (num4 > num)
		{
			float t = (num4 - num) / num4;
			result = Vector2.Lerp(this._trail[num2], base.transform.position, t);
			result.z = base.transform.position.z;
		}
		else
		{
			float num5;
			while (true)
			{
				num5 = this._distances[num2];
				num4 += num5;
				num2 = (num2 + 1) % this._trailCount;
				if (num4 > num)
				{
					break;
				}
				if (num2 == num3)
				{
					goto IL_E6;
				}
			}
			num4 -= num5;
			IL_E6:
			int num6 = (num2 + this._trailCount - 1) % this._trailCount;
			Vector2 a = this._trail[num6];
			Vector2 b = this._trail[num2];
			float num7 = this._distances[num6];
			float t2 = (num - num4) / num7;
			result = Vector2.Lerp(a, b, t2);
			result.z = base.transform.position.z;
		}
		return result;
	}

	private Vector2 GetLastKeyPoint()
	{
		return this._trail[this._startPosition];
	}

	private void AddNewKeyPoint(Vector2 newPosition)
	{
		this._startPosition = (this._startPosition + this._trailCount - 1) % this._trailCount;
		this._trail[this._startPosition] = newPosition;
		this.CalculateDistances();
	}

	private void CalculateDistances()
	{
		int num = this._startPosition;
		int num2 = (this._startPosition + this._trailCount - 1) % this._trailCount;
		do
		{
			float num3 = Vector2.Distance(this._trail[num], this._trail[(num + 1) % this._trailCount]);
			this._distances[num] = num3;
			num = (num + 1) % this._trailCount;
		}
		while (num != num2);
	}

	public void ResetTrail()
	{
		Array.Clear(this._trail, 0, this._trail.Length);
		this.CreateTrail();
	}
}
