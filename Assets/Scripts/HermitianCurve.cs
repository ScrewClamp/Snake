using System;
using UnityEngine;

public class HermitianCurve : MonoBehaviour
{
	[Tooltip("If this option is enabled, we can manipulate curve values during execution")]
	public bool debugEnabled;

	[SerializeField, Tooltip("Source point")]
	private Vector2 point1;

	[SerializeField, Tooltip("Gradient vector on source point")]
	private Vector2 direction1;

	[SerializeField, Tooltip("Target point")]
	private Vector2 point2;

	[SerializeField, Tooltip("Gradient vector on target point")]
	private Vector2 direction2;

	protected float _curveLength;

	private Vector4 _xCoefficients;

	private Vector4 _yCoefficients;

	protected Matrix4x4 _hermitianBasisMatrix;

	protected virtual void Awake()
	{
		this._hermitianBasisMatrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
		this._hermitianBasisMatrix.SetRow(1, new Vector4(0f, 0f, 1f, 0f));
		this._hermitianBasisMatrix.SetRow(2, new Vector4(-3f, 3f, -2f, -1f));
		this._hermitianBasisMatrix.SetRow(3, new Vector4(2f, -2f, 1f, 1f));
		this.CalcCoefficients();
		this.NumericallyCalculateCurveLength();
	}

	protected virtual void CalcCoefficients()
	{
		this._xCoefficients = this.CalcHermitianCoefficients(new Vector4(this.point1.x, this.point2.x, this.direction1.x, this.direction2.x));
		this._yCoefficients = this.CalcHermitianCoefficients(new Vector4(this.point1.y, this.point2.y, this.direction1.y, this.direction2.y));
	}

	protected virtual void NumericallyCalculateCurveLength()
	{
		float num = 0f;
		Vector2 b = this.GetPointOnCurve(0f);
		for (float num2 = 0.001f; num2 <= 1f; num2 += 0.001f)
		{
			Vector2 pointOnCurve = this.GetPointOnCurve(num2);
			num += (pointOnCurve - b).magnitude;
			b = pointOnCurve;
		}
		this._curveLength = num;
	}

	protected Vector4 CalcHermitianCoefficients(Vector4 coordinateAndDerivates)
	{
		return this._hermitianBasisMatrix * coordinateAndDerivates;
	}

	protected float CalculatePolynomialValue(Vector4 coefficients, float t)
	{
		return coefficients[0] + coefficients[1] * t + coefficients[2] * Mathf.Pow(t, 2f) + coefficients[3] * Mathf.Pow(t, 3f);
	}

	public virtual Vector2 GetPointOnCurve(float t)
	{
		Vector2 result = new Vector2(this.CalculatePolynomialValue(this._xCoefficients, t), this.CalculatePolynomialValue(this._yCoefficients, t));
		return result;
	}

	public float GetCurveLength()
	{
		return this._curveLength;
	}
}
