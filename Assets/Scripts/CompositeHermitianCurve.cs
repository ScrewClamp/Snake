using System;
using System.Collections.Generic;
using UnityEngine;

public class CompositeHermitianCurve : HermitianCurve
{
	[Serializable]
	public struct CurvePoint
	{
		public Vector2 point;

		public Vector2 direction;
	}

	public struct CurveCoefficients
	{
		public Vector4 xCoefficients;

		public Vector4 yCoefficients;
	}

	[SerializeField]
	private List<CompositeHermitianCurve.CurvePoint> _points = new List<CompositeHermitianCurve.CurvePoint>();

	private List<CompositeHermitianCurve.CurveCoefficients> _curveCoefficients = new List<CompositeHermitianCurve.CurveCoefficients>();

	private List<float> _curveLengths = new List<float>();

	protected sealed override void Awake()
	{
		base.Awake();
	}

	protected sealed override void CalcCoefficients()
	{
		this._curveCoefficients.Clear();
		for (int i = 0; i < this._points.Count - 1; i++)
		{
			CompositeHermitianCurve.CurvePoint curvePoint = this._points[i];
			CompositeHermitianCurve.CurvePoint curvePoint2 = this._points[i + 1];
			CompositeHermitianCurve.CurveCoefficients item;
			item.xCoefficients = base.CalcHermitianCoefficients(new Vector4(curvePoint.point.x, curvePoint2.point.x, curvePoint.direction.x, curvePoint2.direction.x));
			item.yCoefficients = base.CalcHermitianCoefficients(new Vector4(curvePoint.point.y, curvePoint2.point.y, curvePoint.direction.y, curvePoint2.direction.y));
			this._curveCoefficients.Add(item);
		}
	}

	private float GetCurrentCurveLength(CompositeHermitianCurve.CurveCoefficients currentCurveCoeff)
	{
		float num = 0f;
		Vector2 b = this.GetPointOnCurve(0f, currentCurveCoeff);
		for (float num2 = 0.01f; num2 <= 1f; num2 += 0.01f)
		{
			Vector2 pointOnCurve = this.GetPointOnCurve(num2, currentCurveCoeff);
			num += (pointOnCurve - b).magnitude;
			b = pointOnCurve;
		}
		return num;
	}

	protected sealed override void NumericallyCalculateCurveLength()
	{
		this._curveLengths.Clear();
		float num = 0f;
		for (int i = 0; i < this._curveCoefficients.Count; i++)
		{
			CompositeHermitianCurve.CurveCoefficients currentCurveCoeff = this._curveCoefficients[i];
			float currentCurveLength = this.GetCurrentCurveLength(currentCurveCoeff);
			this._curveLengths.Add(currentCurveLength);
			num += currentCurveLength;
		}
		this._curveLength = num;
	}

	private Vector2 GetPointOnCurve(float t, CompositeHermitianCurve.CurveCoefficients currentCurveCoeff)
	{
		Vector2 result = new Vector2(base.CalculatePolynomialValue(currentCurveCoeff.xCoefficients, t), base.CalculatePolynomialValue(currentCurveCoeff.yCoefficients, t));
		return result;
	}

	public sealed override Vector2 GetPointOnCurve(float t)
	{
		float num = t * this._curveLength;
		float num2 = 0f;
		for (int i = 0; i < this._curveCoefficients.Count; i++)
		{
			num2 += this._curveLengths[i];
			if (num2 >= num)
			{
				CompositeHermitianCurve.CurveCoefficients currentCurveCoeff = this._curveCoefficients[i];
				float a = num2 - this._curveLengths[i];
				float t2 = Mathf.InverseLerp(a, num2, num);
				return this.GetPointOnCurve(t2, currentCurveCoeff);
			}
		}
		return base.GetPointOnCurve(-1f);
	}
}
