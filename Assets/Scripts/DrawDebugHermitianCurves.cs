using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawDebugHermitianCurves : MonoBehaviour
{
	public float step = 0.05f;

	public GameObject ball;

	[HideInInspector]
	public List<GameObject> balls = new List<GameObject>();

	private void Start()
	{
		int num = (int)(5f * (1f / this.step + 1f));
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ball);
			gameObject.SetActive(false);
			this.balls.Add(gameObject);
		}
	}

	private void Update()
	{
		int i = 0;
		HermitianCurve[] components = base.GetComponents<HermitianCurve>();
		for (int j = 0; j < components.Length; j++)
		{
			HermitianCurve hermitianCurve = components[j];
			if (hermitianCurve.debugEnabled)
			{
				i = this.DrawCurve(hermitianCurve, i);
			}
			else
			{
				i = this.HideCurve(i);
			}
		}
	}

	private int DrawCurve(HermitianCurve hermitianComp, int i)
	{
		for (float num = 0f; num <= 1f + Mathf.Epsilon; num += this.step)
		{
			float t = (num != 0f) ? (num - this.step) : 0f;
			Vector2 pointOnCurve = hermitianComp.GetPointOnCurve(t);
			Vector2 pointOnCurve2 = hermitianComp.GetPointOnCurve(num);
			UnityEngine.Debug.DrawLine(pointOnCurve, pointOnCurve2, Color.red);
			this.balls[i].transform.position = pointOnCurve2;
			this.balls[i].SetActive(true);
			i++;
		}
		return i;
	}

	private int HideCurve(int i)
	{
		for (float num = 0f; num <= 1f + Mathf.Epsilon; num += this.step)
		{
			this.balls[i].SetActive(false);
			i++;
		}
		return i;
	}
}
